/**
 * @file gcs.h
 * @brief Main header file for Game Config Switcher (GCS).
 * * Defines the core data structures for game profiles and file pairs,
 * along with function prototypes for configuration management,
 * file operations, and utility helpers.
 */

#ifndef GCS_H
#define GCS_H

#define _POSIX_C_SOURCE 200809L

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include <sys/stat.h>
#include <unistd.h>
#include <errno.h>

/** Current version of Game Config Switcher. */
#define GCS_VERSION "2.0.1"

/** Maximum length for a line in the config file. */
#define MAX_LINE 1024

/** Default name of the configuration file. */
#define CONFIG_FILE "gcs.ini"

/* --- Data Structures --- */

/**
 * @brief Represents a pair of source and destination file paths.
 * * Implements a singly linked list to allow multiple file operations
 * per single game profile.
 */
typedef struct FilePair {
    char *src;              /**< Path to the source configuration file (template). */
    char *dst;              /**< Path where the configuration file should be copied to. */
    struct FilePair *next;  /**< Pointer to the next file pair in the list, or NULL. */
} FilePair;

/**
 * @brief Represents a specific Game Profile.
 * * A profile consists of a Game Title, a Profile Title (e.g., "TV", "Monitor"),
 * and a linked list of FilePairs associated with that profile.
 */
typedef struct Profile {
    char *game_name;        /**< Name of the game (e.g., "Skyrim"). */
    char *profile_name;     /**< Name of the profile (e.g., "LivingRoom"). */
    FilePair *files;        /**< Head of the linked list of file operations. */
    struct Profile *next;   /**< Pointer to the next profile in the list. */
} Profile;

/* --- Function Prototypes --- */

/* src/utils.c */

/**
 * @brief Trims leading and trailing whitespace from a string.
 * @param s The string to modify in place.
 */
void trim(char *s);

/**
 * @brief Checks if a file exists on the filesystem.
 * @param filename The path to the file.
 * @return 1 if exists, 0 otherwise.
 */
int file_exists(const char *filename);

/**
 * @brief Safely duplicates a string, handling NULL inputs.
 * @param s The string to duplicate.
 * @return A pointer to the new string, or NULL if input was NULL.
 */
char *strdup_safe(const char *s);


/* src/config.c */

/**
 * @brief Loads the configuration from disk into memory.
 * @return A pointer to the head of the Profile linked list.
 */
Profile* load_config(void);

/**
 * @brief Saves the current list of profiles to disk.
 * @param head The head of the Profile linked list.
 */
void save_config(Profile *head);

/**
 * @brief Frees all memory associated with the profile list.
 * @param head The head of the list to free.
 */
void free_profiles(Profile *head);

/**
 * @brief Adds a new file pair to a profile. Creates the profile if it doesn't exist.
 * @param game The game title.
 * @param prof The profile title.
 * @param src The source file path.
 * @param dst The destination file path.
 */
void add_profile_entry(const char *game, const char *prof, const char *src, const char *dst);

/**
 * @brief Deletes a specific profile and all its associated file pairs.
 * @param game The game title.
 * @param prof The profile title.
 */
void delete_profile(const char *game, const char *prof);

/**
 * @brief Edits the title of a game across all its profiles.
 * @param old_game The current name of the game.
 * @param new_game The new name to apply.
 */
void edit_game_title(const char *old_game, const char *new_game);

/**
 * @brief Edits a profile's name and/or its file paths.
 * @param game The game title.
 * @param old_prof The current profile name.
 * @param new_prof The new profile name (optional, pass NULL to skip).
 * @param new_src The new source path (optional, pass NULL to skip).
 * @param new_dst The new destination path (optional, pass NULL to skip).
 */
void edit_profile(const char *game, const char *old_prof, const char *new_prof, const char *new_src, const char *new_dst);

/**
 * @brief Prints all currently configured profiles to stdout.
 */
void list_profiles(void);


/* src/ops.c */

/**
 * @brief Copies a single file from src to dst, replacing variables.
 * @param src Source path.
 * @param dst Destination path.
 * @param var_count Number of variable replacements provided.
 * @param vars Array of strings in "key:value" format.
 */
void perform_copy(const char *src, const char *dst, int var_count, char **vars);

/**
 * @brief Executes all file operations for a specific profile.
 * @param p Pointer to the profile to execute.
 * @param var_count Number of variable replacements provided.
 * @param vars Array of strings in "key:value" format.
 */
void execute_profile(Profile *p, int var_count, char **vars);

/**
 * @brief Executes a matching profile name across ALL games.
 * @param prof_name The profile name to match (e.g., "TV").
 * @param var_count Number of variable replacements provided.
 * @param vars Array of strings in "key:value" format.
 */
void execute_all_named_profiles(const char *prof_name, int var_count, char **vars);

#endif // GCS_H