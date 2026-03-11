CC = gcc
CFLAGS = -Wall -Wextra -std=c99 -D_POSIX_C_SOURCE=200809L -Iinclude
GTK_CFLAGS = `pkg-config --cflags gtk+-3.0`
GTK_LIBS = `pkg-config --libs gtk+-3.0`

# Define paths
SRC_DIR = src
OBJ_DIR = obj

# Core logic files shared by both CLI and GUI
CORE_SRCS = $(SRC_DIR)/config.c $(SRC_DIR)/ops.c $(SRC_DIR)/utils.c
CORE_OBJS = $(CORE_SRCS:$(SRC_DIR)/%.c=$(OBJ_DIR)/%.o)

# CLI specific files
CLI_SRC = $(SRC_DIR)/main.c
CLI_OBJ = $(OBJ_DIR)/main.o

# GUI specific files
GUI_SRC = $(SRC_DIR)/gui_main.c
GUI_OBJ = $(OBJ_DIR)/gui_main.o

# Name of the final executables
TARGET_CLI = gcs
TARGET_GUI = gcs-gui

# Default target builds both
all: $(TARGET_CLI) $(TARGET_GUI)

# Link the object files for CLI
$(TARGET_CLI): $(CORE_OBJS) $(CLI_OBJ)
	$(CC) -o $@ $^

# Link the object files for GUI
$(TARGET_GUI): $(CORE_OBJS) $(GUI_OBJ)
	$(CC) -o $@ $^ $(GTK_LIBS)

# Compile core source files into object files
$(OBJ_DIR)/%.o: $(SRC_DIR)/%.c | $(OBJ_DIR)
	$(CC) $(CFLAGS) -c $< -o $@

# Compile GUI source file with GTK flags
$(OBJ_DIR)/gui_main.o: $(SRC_DIR)/gui_main.c | $(OBJ_DIR)
	$(CC) $(CFLAGS) $(GTK_CFLAGS) -c $< -o $@

# Create the obj directory if it doesn't exist
$(OBJ_DIR):
	mkdir -p $(OBJ_DIR)

# Clean up build artifacts
clean:
	rm -rf $(OBJ_DIR) $(TARGET_CLI) $(TARGET_GUI)

# Documentation
# Run doxygen to generate HTML/LaTeX
doc:
	doxygen doxyfile

.PHONY: all clean doc