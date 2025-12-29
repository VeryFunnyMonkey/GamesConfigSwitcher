/**
 * @file utils.c
 * @brief Utility functions for string manipulation and file system checks.
 */

#include "include/gcs.h"

void trim(char *s) {
    if (!s) return;
    
    char *p = s;
    int l = strlen(p);

    // Trim trailing whitespace
    while(l > 0 && isspace((unsigned char)p[l - 1])) p[--l] = 0;

    // Trim leading whitespace
    while(*p && isspace((unsigned char)*p)) ++p, --l;

    // Shift valid string to start of buffer
    memmove(s, p, l + 1);
}

int file_exists(const char *filename) {
    // F_OK tests for existence
    return access(filename, F_OK) != -1;
}

char *strdup_safe(const char *s) {
    if (!s) return NULL;
    return strdup(s);
}