/**
 * @file config.c
 * @brief Logic for parsing, modifying, and saving the GCS INI configuration.
 */

#include "include/gcs.h"

/**
 * @brief Helper to free a linked list of FilePairs.
 * @param head The head of the FilePair list.
 */
static void free_file_pairs(FilePair *head) {
    FilePair *tmp;
    while (head) {
        tmp = head;
        head = head->next;
        if (tmp->src) free(tmp->src);
        if (tmp->dst) free(tmp->dst);
        free(tmp);
    }
}

void free_profiles(Profile *head) {
    Profile *tmp;
    while (head) {
        tmp = head;
        head = head->next;
        if (tmp->game_name) free(tmp->game_name);
        if (tmp->profile_name) free(tmp->profile_name);
        free_file_pairs(tmp->files);
        free(tmp);
    }
}

Profile* load_config(void) {
    FILE *fp = fopen(CONFIG_FILE, "r");
    if (!fp) {
        // It is not an error if the config doesn't exist yet, just return NULL.
        return NULL; 
    }

    Profile *head = NULL;
    Profile *current_prof = NULL;
    FilePair *current_pair_tail = NULL;
    
    char line[MAX_LINE];
    char *pending_src = NULL; 

    int line_num = 0;
    while (fgets(line, sizeof(line), fp)) {
        line_num++;
        trim(line);
        // Skip empty lines or comments
        if (strlen(line) == 0 || line[0] == ';') continue;

        // Check for Section Header: [Game|Profile]
        if (line[0] == '[' && line[strlen(line)-1] == ']') {
            if (pending_src) {
                fprintf(stderr, "[WARN] Malformed config (Line %d): 'src' defined without 'dst'. Discarding orphan: %s\n", line_num, pending_src);
                free(pending_src);
                pending_src = NULL;
            }

            line[strlen(line)-1] = '\0'; // Remove closing bracket
            char *content = line + 1;    // Skip opening bracket
            
            char *pipe = strchr(content, '|');
            if (!pipe) {
                fprintf(stderr, "[WARN] Malformed section header (Line %d): Missing pipe separator. Skipping.\n", line_num);
                continue; 
            }
            *pipe = '\0'; // Split string at pipe

            // Allocation
            Profile *new_prof = malloc(sizeof(Profile));
            if (!new_prof) {
                fprintf(stderr, "[ERROR] Memory allocation failed for new profile.\n");
                exit(EXIT_FAILURE);
            }
            new_prof->game_name = strdup(content);
            new_prof->profile_name = strdup(pipe + 1);
            new_prof->files = NULL;
            new_prof->next = NULL;

            // Link
            if (head == NULL) {
                head = new_prof;
            } else {
                Profile *p = head;
                while(p->next) p = p->next;
                p->next = new_prof;
            }

            // Reset state
            current_prof = new_prof;
            current_pair_tail = NULL;
        }
        // Check for Key=Value pairs
        else if (current_prof) {
            char *eq = strchr(line, '=');
            if (eq) {
                *eq = '\0';
                char *key = line;
                char *val = eq + 1;
                trim(key); 
                trim(val);

                if (strcmp(key, "src") == 0) {
                   if (pending_src) {
                       fprintf(stderr, "[WARN] Overwriting orphaned 'src' (Line %d). Previous: %s\n", line_num, pending_src);
                       free(pending_src);
                   }
                   pending_src = strdup(val);
                }
                else if (strcmp(key, "dst") == 0) {
                    if (!pending_src) {
                        fprintf(stderr, "[WARN] 'dst' encountered without matching 'src' (Line %d). Skipping.\n", line_num);
                        continue;
                    }

                    FilePair *pair = malloc(sizeof(FilePair));
                    if (!pair) {
                        fprintf(stderr, "[ERROR] Memory allocation failed for file pair.\n");
                        exit(EXIT_FAILURE);
                    }
                    pair->src = pending_src; // Transfer ownership
                    pair->dst = strdup(val);
                    pair->next = NULL;
                    pending_src = NULL;

                    if (current_prof->files == NULL) {
                        current_prof->files = pair;
                    } else {
                        current_pair_tail->next = pair;
                    }
                    current_pair_tail = pair;
                }
            }
        }
    }
    
    if(pending_src) {
        fprintf(stderr, "[WARN] Config ended with orphaned 'src': %s\n", pending_src);
        free(pending_src);
    }
    
    fclose(fp);
    return head;
}

void save_config(Profile *head) {
    // Write to a temporary file, then atomic rename.
    // This prevents config corruption if the program crashes during write.
    char temp_config[] = CONFIG_FILE ".tmp";
    FILE *fp = fopen(temp_config, "w");
    
    if (!fp) {
        fprintf(stderr, "[ERROR] Failed to open temporary config file '%s': %s\n", temp_config, strerror(errno));
        return;
    }

    Profile *curr = head;
    while (curr) {
        fprintf(fp, "[%s|%s]\n", curr->game_name, curr->profile_name);
        
        FilePair *pair = curr->files;
        while(pair) {
            fprintf(fp, "src=%s\n", pair->src);
            fprintf(fp, "dst=%s\n", pair->dst);
            pair = pair->next;
        }
        fprintf(fp, "\n");
        curr = curr->next;
    }
    
    // Ensure buffer is flushed to disk
    fflush(fp);
    
    if (fclose(fp) != 0) {
        fprintf(stderr, "[ERROR] Failed to close temporary config file: %s\n", strerror(errno));
        remove(temp_config);
        return;
    }

    // Remove old config if it exists
    if (file_exists(CONFIG_FILE)) {
        if (remove(CONFIG_FILE) != 0) {
            fprintf(stderr, "[ERROR] Failed to remove old config file before update: %s\n", strerror(errno));
            remove(temp_config);
            return;
        }
    }

    // Move temp file to permanent location
    if (rename(temp_config, CONFIG_FILE) != 0) {
        fprintf(stderr, "[ERROR] Failed to commit config file (rename failed): %s\n", strerror(errno));
        remove(temp_config);
    }
}

void add_profile_entry(const char *game, const char *prof, const char *src, const char *dst) {
    Profile *head = load_config();
    Profile *p = head;
    Profile *target = NULL;

    // Search for existing profile
    while(p) {
        if(strcmp(p->game_name, game) == 0 && strcmp(p->profile_name, prof) == 0) {
            target = p;
            break;
        }
        p = p->next;
    }

    // Create if not exists
    if (!target) {
        target = malloc(sizeof(Profile));
        if (!target) {
             fprintf(stderr, "[ERROR] Allocation failed.\n");
             free_profiles(head);
             exit(EXIT_FAILURE);
        }
        target->game_name = strdup(game);
        target->profile_name = strdup(prof);
        target->files = NULL;
        target->next = head; 
        head = target;
    }

    // Append FilePair
    FilePair *pair = malloc(sizeof(FilePair));
    if (!pair) {
        fprintf(stderr, "[ERROR] Allocation failed.\n");
        free_profiles(head);
        exit(EXIT_FAILURE);
    }
    pair->src = strdup(src);
    pair->dst = strdup(dst);
    pair->next = NULL;

    if (target->files == NULL) {
        target->files = pair;
    } else {
        FilePair *fp = target->files;
        while (fp->next) fp = fp->next;
        fp->next = pair;
    }

    save_config(head);
    free_profiles(head);
    printf("Successfully added configuration pair to [%s | %s].\n", game, prof);
}

void delete_profile(const char *game, const char *prof) {
    Profile *head = load_config();
    Profile *curr = head, *prev = NULL;
    int deleted = 0;

    while(curr) {
        if (strcmp(curr->game_name, game) == 0 && strcmp(curr->profile_name, prof) == 0) {
            // Unlink
            if(prev) prev->next = curr->next;
            else head = curr->next;
            
            // Free
            free(curr->game_name);
            free(curr->profile_name);
            free_file_pairs(curr->files);
            free(curr);
            
            deleted = 1;
            break; 
        }
        prev = curr;
        curr = curr->next;
    }

    if (deleted) {
        save_config(head);
        printf("Deleted profile [%s | %s].\n", game, prof);
    } else {
        printf("[WARN] Profile [%s | %s] not found. No changes made.\n", game, prof);
    }
    
    free_profiles(head);
}

void edit_game_title(const char *old_game, const char *new_game) {
    Profile *head = load_config();
    Profile *curr = head;
    int count = 0;

    while (curr) {
        if (strcmp(curr->game_name, old_game) == 0) {
            free(curr->game_name);
            curr->game_name = strdup(new_game);
            count++;
        }
        curr = curr->next;
    }

    if (count > 0) {
        save_config(head);
        printf("Updated %d profile(s) from game title '%s' to '%s'.\n", count, old_game, new_game);
    } else {
        printf("[WARN] Game title '%s' not found.\n", old_game);
    }
    free_profiles(head);
}

void edit_profile(const char *game, const char *old_prof, const char *new_prof, const char *new_src, const char *new_dst) {
    Profile *head = load_config();
    Profile *curr = head;
    int found = 0;

    while (curr) {
        if (strcmp(curr->game_name, game) == 0 && strcmp(curr->profile_name, old_prof) == 0) {
            
            // Rename Profile if requested
            if (new_prof) {
                free(curr->profile_name);
                curr->profile_name = strdup(new_prof);
                printf("Renamed profile '%s' to '%s'.\n", old_prof, new_prof);
            }

            // Update Paths if requested 
            if (new_src && new_dst) {
                // Wipe existing list
                free_file_pairs(curr->files);
                
                // Create new root node
                FilePair *new_pair = malloc(sizeof(FilePair));
                if (!new_pair) {
                    fprintf(stderr, "[ERROR] Memory allocation failed during edit.\n");
                    exit(EXIT_FAILURE);
                }
                new_pair->src = strdup(new_src);
                new_pair->dst = strdup(new_dst);
                new_pair->next = NULL;
                
                curr->files = new_pair;
                
                printf("Overwrote file paths for profile.\n");
            }
            
            found = 1;
            break;
        }
        curr = curr->next;
    }

    if (found) {
        save_config(head);
    } else {
        printf("[WARN] Profile [%s | %s] not found.\n", game, old_prof);
    }
    free_profiles(head);
}

void list_profiles(void) {
    Profile *head = load_config();
    Profile *curr = head;
    
    if (!curr) {
        printf("No profiles configured.\n");
        return;
    }

    printf("\n=== GCS Configuration ===\n");
    while (curr) {
        printf("[%s] %s\n", curr->game_name, curr->profile_name);
        FilePair *fp = curr->files;
        int i = 1;
        while(fp) {
            printf("  %d. Src: %s\n     Dst: %s\n", i++, fp->src, fp->dst);
            fp = fp->next;
        }
        printf("\n");
        curr = curr->next;
    }
    printf("=========================\n");
    free_profiles(head);
}