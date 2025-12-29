CC = gcc
CFLAGS = -Wall -Wextra -std=c99 -D_POSIX_C_SOURCE=200809L -Iinclude

# Define paths
SRC_DIR = src
OBJ_DIR = obj

# List of source files
SRCS = $(SRC_DIR)/main.c $(SRC_DIR)/config.c $(SRC_DIR)/ops.c $(SRC_DIR)/utils.c

# Generate object file names
OBJS = $(SRCS:$(SRC_DIR)/%.c=$(OBJ_DIR)/%.o)

# Name of the final executable
TARGET = gcs

# Default target
all: $(TARGET)

# Link the object files
$(TARGET): $(OBJS)
	$(CC) -o $@ $^

# Compile source files into object files
$(OBJ_DIR)/%.o: $(SRC_DIR)/%.c | $(OBJ_DIR)
	$(CC) $(CFLAGS) -c $< -o $@

# Create the obj directory if it doesn't exist
$(OBJ_DIR):
	mkdir -p $(OBJ_DIR)

# Clean up build artifacts
clean:
	rm -rf $(OBJ_DIR) $(TARGET)

# Documentation
# Run doxygen to generate HTML/LaTeX
doc:
	doxygen doxyfile

.PHONY: all clean doc