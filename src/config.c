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
        free(tmp->src);
        free(tmp->dst);
        free(tmp);
    }
}

void free_profiles(Profile *head) {
    Profile *tmp;
    while (head) {
        tmp = head;
        head = head->next;
        free(tmp->game_name);
        free(tmp->profile_name);
        free_file_pairs(tmp->files);
        free(tmp);
    }
}

Profile* load_config(void) {
    FILE *fp = fopen(CONFIG_FILE, "r");
    if (!fp) return NULL; // No config file exists yet

    Profile *head = NULL;
    Profile *current_prof = NULL;
    FilePair *current_pair_tail = NULL;
    
    char line[MAX_LINE];
    char *pending_src = NULL; // Temp storage for 'src' until 'dst' is found

    while (fgets(line, sizeof(line), fp)) {
        trim(line);
        // Skip empty lines or comments
        if (strlen(line) == 0 || line[0] == ';') continue;

        // Check for Section Header: [Game|Profile]
        if (line[0] == '[' && line[strlen(line)-1] == ']') {
            line[strlen(line)-1] = '\0'; // Remove closing bracket
            char *content = line + 1;    // Skip opening bracket
            
            char *pipe = strchr(content, '|');
            if (!pipe) continue; // Malformed section
            *pipe = '\0'; // Split string at pipe

            // Create new Profile Node
            Profile *new_prof = malloc(sizeof(Profile));
            if (!new_prof) {
                perror("Failed to allocate profile");
                exit(EXIT_FAILURE);
            }
            new_prof->game_name = strdup(content);
            new_prof->profile_name = strdup(pipe + 1);
            new_prof->files = NULL;
            new_prof->next = NULL;

            // Append to Profile List
            if (head == NULL) {
                head = new_prof;
            } else {
                Profile *p = head;
                while(p->next) p = p->next;
                p->next = new_prof;
            }

            // Reset state for new section
            current_prof = new_prof;
            current_pair_tail = NULL;
            if (pending_src) { free(pending_src); pending_src = NULL; }
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
                   if (pending_src) free(pending_src);
                   pending_src = strdup(val);
                }
                else if (strcmp(key, "dst") == 0 && pending_src) {
                    // Valid Pair Found: Create Node
                    FilePair *pair = malloc(sizeof(FilePair));
                    if (!pair) {
                        perror("Failed to allocate file pair");
                        exit(EXIT_FAILURE);
                    }
                    pair->src = pending_src; // Transfer ownership
                    pair->dst = strdup(val);
                    pair->next = NULL;
                    pending_src = NULL;

                    // Append to FilePair List within Profile
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
    
    if(pending_src) free(pending_src);
    fclose(fp);
    return head;
}

void save_config(Profile *head) {
    FILE *fp = fopen(CONFIG_FILE, "w");
    if (!fp) {
        perror("Error opening config file for writing");
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
    fclose(fp);
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

    // If not found, create new profile
    if (!target) {
        target = malloc(sizeof(Profile));
        target->game_name = strdup(game);
        target->profile_name = strdup(prof);
        target->files = NULL;
        target->next = head; // Prepend to list for efficiency
        head = target;
    }

    // Append new file pair to the end of the profile's file list
    FilePair *pair = malloc(sizeof(FilePair));
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
    printf("Successfully added config pair to [%s | %s]\n", game, prof);
}

void delete_profile(const char *game, const char *prof) {
    Profile *head = load_config();
    Profile *curr = head, *prev = NULL;
    int deleted = 0;

    while(curr) {
        if (strcmp(curr->game_name, game) == 0 && strcmp(curr->profile_name, prof) == 0) {
            // Unlink node
            if(prev) prev->next = curr->next;
            else head = curr->next;
            
            // Free memory
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
        printf("Deleted profile [%s | %s]\n", game, prof);
    } else {
        printf("Profile [%s | %s] not found.\n", game, prof);
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
        printf("Updated %d profile(s) from game '%s' to '%s'.\n", count, old_game, new_game);
    } else {
        printf("Game '%s' not found.\n", old_game);
    }
    free_profiles(head);
}

void edit_profile(const char *game, const char *old_prof, const char *new_prof, const char *new_src, const char *new_dst) {
    Profile *head = load_config();
    Profile *curr = head;
    int found = 0;

    while (curr) {
        if (strcmp(curr->game_name, game) == 0 && strcmp(curr->profile_name, old_prof) == 0) {
            
            // 1. Rename Profile if requested
            if (new_prof) {
                free(curr->profile_name);
                curr->profile_name = strdup(new_prof);
                printf("Renamed profile '%s' to '%s'.\n", old_prof, new_prof);
            }

            // 2. Update Paths if requested (Replaces existing list entirely)
            if (new_src && new_dst) {
                // Free existing files
                free_file_pairs(curr->files);
                
                // Create new single pair
                FilePair *new_pair = malloc(sizeof(FilePair));
                new_pair->src = strdup(new_src);
                new_pair->dst = strdup(new_dst);
                new_pair->next = NULL;
                curr->files = new_pair;
                
                printf("Updated file paths for profile.\n");
            }
            
            found = 1;
            break;
        }
        curr = curr->next;
    }

    if (found) {
        save_config(head);
    } else {
        printf("Profile [%s | %s] not found.\n", game, old_prof);
    }
    free_profiles(head);
}

void list_profiles(void) {
    Profile *head = load_config();
    Profile *curr = head;
    
    if (!curr) {
        printf("No profiles found.\n");
        return;
    }

    printf("\n--- GCS Configuration ---\n");
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
    printf("-------------------------\n");
    free_profiles(head);
}