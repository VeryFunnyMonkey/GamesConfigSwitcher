/**
 * @file ops.c
 * @brief Operations implementation. Handles file copying and variable substitution.
 */

#include "include/gcs.h"

/**
 * @brief Safely concatenates source to buffer, checking bounds.
 * @return 1 on success, 0 on buffer overflow.
 */
static int safe_strcat(char *buf, size_t buf_size, const char *str) {
    size_t current_len = strlen(buf);
    size_t new_len = strlen(str);
    if (current_len + new_len >= buf_size) {
        return 0; // Failure
    }
    strcat(buf, str);
    return 1; // Success
}

/**
 * @brief Safely concatenates N characters from source to buffer.
 * @return 1 on success, 0 on buffer overflow.
 */
static int safe_strncat(char *buf, size_t buf_size, const char *str, size_t n) {
    size_t current_len = strlen(buf);
    if (current_len + n >= buf_size) {
        return 0; // Failure
    }
    strncat(buf, str, n);
    return 1; // Success
}

void perform_copy(const char *src, const char *dst, int var_count, char **vars) {
    // Determine if we need variable processing or a direct binary copy
    int direct_copy = (var_count == 0);

    FILE *fsrc = fopen(src, direct_copy ? "rb" : "r");
    if (!fsrc) {
        fprintf(stderr, "[ERROR] Source file not found: %s\n", src);
        return;
    }

    // Create a temporary file in the SAME directory as the destination.
    // This allows the use of rename(), which is atomic on POSIX.
    char temp_dst[MAX_LINE];
    int needed = snprintf(temp_dst, sizeof(temp_dst), "%s.tmp", dst);
    
    if (needed < 0 || (size_t)needed >= sizeof(temp_dst)) {
        fprintf(stderr, "[ERROR] Destination path too long or formatting failed.\n");
        fclose(fsrc);
        return;
    }

    FILE *ftemp = fopen(temp_dst, direct_copy ? "wb" : "w");
    if (!ftemp) {
        fprintf(stderr, "[ERROR] Failed to create temporary file '%s': %s\n", temp_dst, strerror(errno));
        fclose(fsrc);
        return;
    }

    /* --- Path A: Direct Binary Copy --- */
    if (direct_copy) {
        char buffer[BUFSIZ];
        size_t bytes;
        while ((bytes = fread(buffer, 1, sizeof(buffer), fsrc)) > 0) {
            if (fwrite(buffer, 1, bytes, ftemp) != bytes) {
                fprintf(stderr, "[ERROR] Write error during copy to '%s'.\n", temp_dst);
                fclose(fsrc);
                fclose(ftemp);
                remove(temp_dst);
                return;
            }
        }
    } 
    /* --- Path B: Variable Substitution --- */
    else {
        char line[MAX_LINE];
        // Double buffer size allows expansion, but strict bounds checking is still enforced below.
        char buffer[MAX_LINE * 2]; 

        while (fgets(line, sizeof(line), fsrc)) {
            buffer[0] = '\0';
            char *cursor = line;
            int overflow_flag = 0;
            
            while (*cursor) {
                char *start = strstr(cursor, "${");
                if (start) {
                    // 1. Copy text BEFORE the variable
                    if (!safe_strncat(buffer, sizeof(buffer), cursor, start - cursor)) {
                        overflow_flag = 1; break; 
                    }
                    
                    char *end = strchr(start, '}');
                    if (end) {
                        int var_len = end - (start + 2);
                        char var_name[128] = {0}; // Ensure zero-initialization

                        if (var_len < 127) {
                            strncpy(var_name, start + 2, var_len);
                            var_name[var_len] = '\0'; // Strictly ensure null-termination
                        } else {
                            // Variable name too long for buffer, ignore it safely
                            var_name[0] = '\0'; 
                        }
                        
                        int replaced = 0;
                        if (var_name[0] != '\0') {
                            for (int i = 0; i < var_count; i++) {
                                char *v_copy = strdup(vars[i]);
                                if (!v_copy) continue;

                                char *key = strtok(v_copy, ":");
                                char *val = strtok(NULL, ":");
                                
                                if (key && val && strcmp(key, var_name) == 0) {
                                    // 2. Copy the REPLACEMENT value
                                    if (!safe_strcat(buffer, sizeof(buffer), val)) {
                                        overflow_flag = 1;
                                    }
                                    replaced = 1;
                                }
                                free(v_copy);
                                if(replaced || overflow_flag) break;
                            }
                        }
                        
                        // If no replacement found, keep original text "${VAR}"
                        if (!replaced && !overflow_flag) {
                            if (!safe_strncat(buffer, sizeof(buffer), start, end - start + 1)) {
                                overflow_flag = 1;
                            }
                        }
                        cursor = end + 1;
                    } else {
                        // Malformed tag (missing closing brace), treat as normal text
                        if (!safe_strcat(buffer, sizeof(buffer), cursor)) overflow_flag = 1;
                        break;
                    }
                } else {
                    // No more variables in line
                    if (!safe_strcat(buffer, sizeof(buffer), cursor)) overflow_flag = 1;
                    break;
                }

                if (overflow_flag) break;
            }

            if (overflow_flag) {
                fprintf(stderr, "[ERROR] Line expansion caused buffer overflow. Truncating line.\n");
            }
            fputs(buffer, ftemp);
        }
    }

    fclose(fsrc);
    
    // Ensure all data is physically on disk before renaming
    fflush(ftemp);
    if (fclose(ftemp) != 0) {
        fprintf(stderr, "[ERROR] Failed to close temporary file: %s\n", strerror(errno));
        remove(temp_dst);
        return;
    }

    // Replace destination with temp file
    if (rename(temp_dst, dst) != 0) {
        fprintf(stderr, "[ERROR] Failed to move temp file to destination: %s\n", strerror(errno));
        remove(temp_dst); // Clean up temp junk
    } else {
        printf("Processed copy successful: %s\n", dst);
    }
}

void execute_profile(Profile *p, int var_count, char **vars) {
    if (!p) return;
    printf("Applying Profile: [%s | %s]\n", p->game_name, p->profile_name);
    
    FilePair *pair = p->files;
    while(pair) {
        perform_copy(pair->src, pair->dst, var_count, vars);
        pair = pair->next;
    }
}

void execute_all_named_profiles(const char *prof_name, int var_count, char **vars) {
    Profile *head = load_config();
    Profile *curr = head;
    int found_any = 0;

    printf("\n--- Executing 'useall' for profile identifier: %s ---\n", prof_name);

    while (curr) {
        if (strcmp(curr->profile_name, prof_name) == 0) {
            execute_profile(curr, var_count, vars);
            found_any = 1;
        }
        curr = curr->next;
    }

    if (!found_any) {
        printf("[WARN] No profiles found matching name '%s'.\n", prof_name);
    }
    printf("---------------------------------------------------\n");
    
    free_profiles(head);
}