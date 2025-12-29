#!/bin/bash

# ==============================================================================
# GCS Integration Test Suite
# ==============================================================================
# Description:
#   Runs a full lifecycle test of the Game Config Switcher (GCS) binary.
#   - Adds profiles
#   - Verifies listing
#   - Performs variable substitution
#   - Edits/Renames profiles
#   - Tests global switch (useall)
#   - Deletes profiles
#   - cleans up artifacts automatically via trap
# ==============================================================================

# Exit immediately if a command exits with a non-zero status
set -e

# --- Path Resolution ---
# Resolve the directory of this script to ensure relative paths work
# regardless of where the script is invoked from.
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

# --- Configuration ---
TEMP_DIR="$SCRIPT_DIR/temp_data"
SOURCE_FILE="$TEMP_DIR/settings_template.ini"
DEST_FILE="$TEMP_DIR/actual_settings.ini"

# Detect OS to select the correct binary extension
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "cygwin" || "$OSTYPE" == "win32" ]]; then
    EXE="$ROOT_DIR/gcs.exe"
else
    EXE="$ROOT_DIR/gcs"
fi

# --- Cleanup Trap ---
# This function runs automatically on EXIT (success or failure).
function cleanup {
    echo ""
    echo "--- Cleanup Phase ---"
    
    if [ -d "$TEMP_DIR" ]; then
        echo "Removing temporary test directory..."
        rm -rf "$TEMP_DIR"
    fi
    
    # Remove the config file generated in the root during testing
    if [ -f "$ROOT_DIR/gcs.ini" ]; then
        echo "Removing gcs.ini..."
        rm "$ROOT_DIR/gcs.ini"
    fi
    
    echo "Cleanup complete."
}

# Register the trap
trap cleanup EXIT

# --- Verification Start ---
echo "========================================================"
echo "Starting Integration Tests"
echo "Binary: $EXE"
echo "Temp Dir: $TEMP_DIR"
echo "========================================================"

# 1. Prepare Environment
mkdir -p "$TEMP_DIR"

# Create a dummy source file with variables to test substitution
echo "[Graphics]" > "$SOURCE_FILE"
echo "Width=\${w}" >> "$SOURCE_FILE"
echo "Height=\${h}" >> "$SOURCE_FILE"

# --- Test Case 1: ADD ---
echo "[1/6] Testing ADD command..."
"$EXE" add "TestGame" "TV" -s "$SOURCE_FILE" -d "$DEST_FILE"

# --- Test Case 2: LIST ---
echo "[2/6] Testing LIST command..."
if "$EXE" list | grep -q "TestGame"; then
    echo "  -> [PASS] Game found in list output."
else
    echo "  -> [FAIL] 'TestGame' not found in list output."
    exit 1
fi

# --- Test Case 3: USE (Variable Substitution) ---
echo "[3/6] Testing USE command with variables..."
"$EXE" use "TestGame" "TV" -v w:1920 -v h:1080

# Verify destination file content
if grep -q "Width=1920" "$DEST_FILE" && grep -q "Height=1080" "$DEST_FILE"; then
    echo "  -> [PASS] Variables \${w} and \${h} substituted correctly."
else
    echo "  -> [FAIL] Variable substitution failed. File content:"
    cat "$DEST_FILE"
    exit 1
fi

# --- Test Case 4: EDIT (Renaming) ---
echo "[4/6] Testing EDIT command (Renaming profile)..."
"$EXE" edit profile "TestGame" "TV" -n "Monitor"

# Verify "TV" is gone and "Monitor" exists
if "$EXE" list | grep -q "Monitor" && ! "$EXE" list | grep -q "TV"; then
    echo "  -> [PASS] Profile successfully renamed to 'Monitor'."
else
    echo "  -> [FAIL] Rename operation failed."
    exit 1
fi

# --- Test Case 5: USEALL (Global Switch) ---
echo "[5/6] Testing USEALL command..."
# Reset destination file to ensure logic works
echo "" > "$DEST_FILE"

"$EXE" useall "Monitor" -v w:2560 -v h:1440

if grep -q "Width=2560" "$DEST_FILE"; then
    echo "  -> [PASS] UseAll applied correctly."
else
    echo "  -> [FAIL] UseAll failed to update the file."
    exit 1
fi

# --- Test Case 6: DELETE ---
echo "[6/6] Testing DELETE command..."
"$EXE" delete "TestGame" "Monitor"

if ! "$EXE" list | grep -q "TestGame"; then
    echo "  -> [PASS] Profile successfully deleted."
else
    echo "  -> [FAIL] Profile 'TestGame' still exists after delete."
    exit 1
fi

echo "========================================================"
echo "ALL TESTS PASSED"
echo "========================================================"