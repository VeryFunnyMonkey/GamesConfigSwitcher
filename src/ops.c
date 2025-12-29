/**
 * @file ops.c
 * @brief Operations implementation. Handles file copying and variable substitution.
 */

#include "include/gcs.h"

void perform_copy(const char *src, const char *dst, int var_count, char **vars) {
    FILE *fsrc = fopen(src, "r");
    if (!fsrc) {
        fprintf(stderr, "  [ERROR] Source file not found: %s\n", src);
        return;
    }
    
    // Create a temporary file to write processed content
    char temp_path[] = "gcs_temp_XXXXXX";
    int fd = mkstemp(temp_path);
    if (fd == -1) {
        perror("  [ERROR] Failed to create temporary file");
        fclose(fsrc);
        return;
    }
    FILE *ftemp = fdopen(fd, "w");

    char line[MAX_LINE];
    // Process line by line
    while (fgets(line, sizeof(line), fsrc)) {
        char buffer[MAX_LINE * 2]; // Double size buffer to handle expansion
        buffer[0] = '\0';
        char *cursor = line;
        
        while (*cursor) {
            // Search for variable start tag "${"
            char *start = strstr(cursor, "${");
            if (start) {
                // Copy text before the variable
                strncat(buffer, cursor, start - cursor);
                
                // Find closing tag "}"
                char *end = strchr(start, '}');
                if (end) {
                    // Extract variable name
                    int var_len = end - (start + 2);
                    char var_name[128];
                    strncpy(var_name, start + 2, var_len);
                    var_name[var_len] = '\0';
                    
                    // Look up variable value in provided arguments
                    int replaced = 0;
                    for (int i = 0; i < var_count; i++) {
                        // vars[i] format is "key:value"
                        char *v_copy = strdup(vars[i]);
                        char *key = strtok(v_copy, ":");
                        char *val = strtok(NULL, ":");
                        
                        if (key && val && strcmp(key, var_name) == 0) {
                            strcat(buffer, val);
                            replaced = 1;
                        }
                        free(v_copy);
                        if(replaced) break;
                    }
                    
                    // If no replacement found, keep original text
                    if (!replaced) {
                        strncat(buffer, start, end - start + 1);
                    }
                    cursor = end + 1;
                } else {
                    // Malformed tag, treat as normal text
                    strcat(buffer, cursor);
                    break;
                }
            } else {
                // No more variables in line
                strcat(buffer, cursor);
                break;
            }
        }
        fputs(buffer, ftemp);
    }

    fclose(fsrc);
    fclose(ftemp);

    // Overwrite the destination file with the processed temp file
    // Note: rename is atomic on POSIX if on same filesystem, 
    // but we use manual copy here to ensure cross-partition compatibility.
    FILE *fin = fopen(temp_path, "r");
    FILE *fout = fopen(dst, "w");
    
    if (fout && fin) {
        char ch;
        while ((ch = fgetc(fin)) != EOF) fputc(ch, fout);
        printf("  [OK] Copied to: %s\n", dst);
    } else {
        fprintf(stderr, "  [ERROR] Failed writing to: %s\n", dst);
    }
    
    if (fin) fclose(fin);
    if (fout) fclose(fout);
    remove(temp_path); // Cleanup
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

    printf("--- Executing 'useall' for profile: %s ---\n", prof_name);

    while (curr) {
        if (strcmp(curr->profile_name, prof_name) == 0) {
            execute_profile(curr, var_count, vars);
            found_any = 1;
        }
        curr = curr->next;
    }

    if (!found_any) {
        printf("No games found with profile name '%s'.\n", prof_name);
    }
    
    free_profiles(head);
}