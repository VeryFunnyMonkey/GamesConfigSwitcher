/**
 * @file main.c
 * @brief Entry point for Game Config Switcher (GCS).
 * * Handles command line argument parsing and routing commands
 * to the appropriate logic handlers.
 */

#include "include/gcs.h"

/**
 * @brief Prints the usage help message.
 */
void print_usage() {
    printf("Game Config Switcher (GCS) v%s\n", GCS_VERSION);
    printf("Usage:\n");
    printf("  list (l, ls)\n");
    printf("  add (a) <Game> <Profile> -s <Src> -d <Dst>\n");
    printf("  use (u) <Game> <Profile> [-v k:v ...]\n");
    printf("  useall (ua) <Profile> [-v k:v ...]\n");
    printf("  delete (d, rm) <Game> <Profile>\n");
    printf("  edit (e) game <OldName> <NewName>\n");
    printf("  edit (e) profile <Game> <Profile> [-n <NewName>] [-s <Src> -d <Dst>]\n");
}

int main(int argc, char *argv[]) {
    if (argc < 2) {
        print_usage();
        return 1;
    }

    char *cmd = argv[1];

    /* --- VERSION COMMAND --- */
    if (strcmp(cmd, "version") == 0 || strcmp(cmd, "v") == 0) {
        printf("Game Config Switcher (GCS) version %s\n", GCS_VERSION);
        return 0;
    }

    /* --- LIST COMMAND --- */
    if (strcmp(cmd, "list") == 0 || strcmp(cmd, "l") == 0 || strcmp(cmd, "ls") == 0) {
        list_profiles();
    }
    
    /* --- ADD COMMAND --- */
    else if (strcmp(cmd, "add") == 0 || strcmp(cmd, "a") == 0) {
        if (argc < 8) {
            printf("Usage: add <Game> <Profile> -s <Src> -d <Dst>\n");
            return 1;
        }
        char *game = argv[2];
        char *prof = argv[3];
        char *src = NULL;
        char *dst = NULL;
        
        for (int i = 4; i < argc; i++) {
            if (strcmp(argv[i], "-s") == 0 && i+1 < argc) src = argv[++i];
            else if (strcmp(argv[i], "-d") == 0 && i+1 < argc) dst = argv[++i];
        }

        if (src && dst) {
            add_profile_entry(game, prof, src, dst);
        } else {
            printf("Error: Missing -s (source) or -d (destination) path.\n");
        }
    }
    
    /* --- DELETE COMMAND --- */
    else if (strcmp(cmd, "delete") == 0 || strcmp(cmd, "d") == 0 || strcmp(cmd, "rm") == 0) {
        if (argc < 4) {
            printf("Usage: delete <Game> <Profile>\n");
            return 1;
        }
        delete_profile(argv[2], argv[3]);
    }

    /* --- EDIT COMMAND --- */
    else if (strcmp(cmd, "edit") == 0 || strcmp(cmd, "e") == 0) {
        if (argc < 3) {
            print_usage(); 
            return 1;
        }

        char *subcmd = argv[2];

        if (strcmp(subcmd, "game") == 0) {
            // edit game <Old> <New>
            if (argc < 5) {
                printf("Usage: edit game <OldName> <NewName>\n");
                return 1;
            }
            edit_game_title(argv[3], argv[4]);
        }
        else if (strcmp(subcmd, "profile") == 0) {
            // edit profile <Game> <Profile> [-n NewName] [-s Src -d Dst]
            if (argc < 5) {
                printf("Usage: edit profile <Game> <Profile> ...\n");
                return 1;
            }
            char *game = argv[3];
            char *prof = argv[4];
            char *new_prof = NULL;
            char *new_src = NULL;
            char *new_dst = NULL;

            for (int i = 5; i < argc; i++) {
                if (strcmp(argv[i], "-n") == 0 && i+1 < argc) new_prof = argv[++i];
                else if (strcmp(argv[i], "-s") == 0 && i+1 < argc) new_src = argv[++i];
                else if (strcmp(argv[i], "-d") == 0 && i+1 < argc) new_dst = argv[++i];
            }
            
            if (new_prof || (new_src && new_dst)) {
                edit_profile(game, prof, new_prof, new_src, new_dst);
            } else {
                printf("Error: Provide -n (rename) or both -s and -d (update paths).\n");
            }
        }
        else {
            printf("Unknown edit command. Use 'edit game' or 'edit profile'.\n");
        }
    }
    
    /* --- USE COMMAND --- */
    else if (strcmp(cmd, "use") == 0 || strcmp(cmd, "u") == 0) {
        if (argc < 4) {
            printf("Usage: use <Game> <Profile> [-v k:v]\n");
            return 1;
        }
        
        char *game = argv[2];
        char *prof = argv[3];
        char *vars[20];
        int var_count = 0;

        for (int i = 4; i < argc; i++) {
            if (strcmp(argv[i], "-v") == 0 && i+1 < argc && var_count < 20) {
                vars[var_count++] = argv[++i];
            }
        }

        Profile *head = load_config();
        Profile *curr = head;
        int found = 0;
        
        while(curr) {
            if (strcmp(curr->game_name, game) == 0 && strcmp(curr->profile_name, prof) == 0) {
                execute_profile(curr, var_count, vars);
                found = 1;
                break;
            }
            curr = curr->next;
        }
        
        if (!found) printf("Profile [%s | %s] not found.\n", game, prof);
        free_profiles(head);
    }

    /* --- USEALL COMMAND --- */
    else if (strcmp(cmd, "useall") == 0 || strcmp(cmd, "ua") == 0) {
        if (argc < 3) {
            printf("Usage: useall <Profile> [-v k:v ...]\n");
            return 1;
        }

        char *prof = argv[2];
        char *vars[20];
        int var_count = 0;

        for (int i = 3; i < argc; i++) {
            if (strcmp(argv[i], "-v") == 0 && i+1 < argc && var_count < 20) {
                vars[var_count++] = argv[++i];
            }
        }

        execute_all_named_profiles(prof, var_count, vars);
    }

    /* --- UNKNOWN COMMAND --- */
    else {
        printf("Unknown command: %s\n", cmd);
        print_usage();
        return 1;
    }

    return 0;
}