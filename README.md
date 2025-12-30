# Game Config Switcher (GCS)

**Game Config Switcher (GCS)** is a lightweight, zero-dependency CLI tool written in C. It allows you to easily swap configuration files for games or applications, making it ideal for switching settings between different environments (e.g., "TV Mode" vs. "Monitor Mode" or "Steam Deck" vs. "Desktop").

It supports variable substitution, allowing you to dynamically inject values (like resolution or refresh rate) into configuration files at runtime.

## Table of Contents
- [Features](#features)
- [Project Structure](#project-structure)
- [Building](#building)
- [Usage](#usage)
  - [Adding Profiles](#adding-profiles)
  - [Using Profiles](#using-profiles)
  - [Global Switch (UseAll)](#global-switch-useall)
  - [Editing](#editing)
  - [Listing & Deleting](#listing--deleting)
  - [Variable Substitution](#variable-substitution)
- [Configuration File](#configuration-file)
- [Documentation](#documentation)

## Features
- **Zero Dependencies:** Written in standard POSIX C. No heavy runtimes (.NET/Java) required.
- **Multiple Files per Profile:** A single profile can swap out multiple config files simultaneously (e.g., `graphics.ini` AND `controls.ini`).
- **Variable Substitution:** Define placeholders like `${width}` in your source files and replace them via CLI arguments.
- **Global Switching:** Apply a specific profile name (e.g., "TV") across *all* configured games with one command.
- **Portable:** Stores configuration in a simple `gcs.ini` file alongside the binary.

## Project Structure
The project follows a standard C makefile structure:

```text
GamesConfigSwitcher/
├── Makefile             # Build automation
├── gcs.ini              # Config storage (created on first run)
├── src/                 # Implementation (.c)
│   ├── main.c           # CLI entry point
│   ├── config.c         # INI parsing & list management
│   ├── ops.c            # File copying & variable logic
│   └── utils.c          # Helper functions
└── include/             # Headers (.h)
    └── gcs.h            # Data structures & prototypes
```

## Building

### Prerequisites
- GCC (GNU Compiler Collection) or Clang
- Make

### Build Steps
1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/YourUsername/GamesConfigSwitcher.git](https://github.com/YourUsername/GamesConfigSwitcher.git)
    cd GamesConfigSwitcher
    ```

2.  **Compile:**
    Run `make` in the root directory.
    ```bash
    make
    ```
    This will create the `gcs` binary.

3.  **Clean (Optional):**
    To remove build artifacts:
    ```bash
    make clean
    ```

## Usage

### Adding Profiles
Use the `add` command to create a game profile. You specify the Game Name, Profile Name, Source file (your template), and Destination file (where the game reads configs).

**Syntax:**
```bash
./gcs add "Game Name" "Profile Name" -s "./path/to/source" -d "./path/to/dest"
```

**Multiple Files per Profile:**
To add a second config file to the *same* profile, simply run the `add` command again with the same Game and Profile names.

```bash
# Add graphics config
./gcs add "Skyrim" "TV" -s "./configs/skyrim_tv_graphics.ini" -d "~/Documents/My Games/Skyrim/SkyrimPrefs.ini"

# Add controls config to the SAME profile
./gcs add "Skyrim" "TV" -s "./configs/skyrim_controller.ini" -d "~/Documents/My Games/Skyrim/ControlMap.txt"
```

### Using Profiles
The `use` command copies the source file(s) to the destination path(s) for a specific game.

**Syntax:**
```bash
./gcs use "Game Name" "Profile Name"
```

**Example:**
```bash
./gcs use "Skyrim" "TV"
```

### Global Switch (UseAll)
The `useall` command attempts to apply a specific profile name to **all** configured games. Games that do not have a profile with that name are skipped.

**Syntax:**
```bash
./gcs useall "Profile Name"
```

**Example:**
```bash
# Switches Skyrim, Cyberpunk, and Witcher 3 to "TV" mode simultaneously
./gcs useall "TV"
```

### Editing
You can rename games or modify profiles using the `edit` command.

**1. Rename a Game Title:**
Updates the game name across all profiles.
```bash
./gcs edit game "Skyrim" "The Elder Scrolls V"
```

**2. Rename a Profile:**
```bash
./gcs edit profile "Skyrim" "TV" -n "LivingRoom"
```

**3. Update Profile Paths:**
Updates the source and destination paths for an existing profile.
*Note: This replaces the file list for this profile.*
```bash
./gcs edit profile "Skyrim" "TV" -s "./new_src.ini" -d "./new_dest.ini"
```

### Listing & Deleting
- **List:** View all configured games, profiles, and their file paths.
  ```bash
  ./gcs list
  ```

- **Delete:** Remove a profile and all its associated file pairings.
  ```bash
  ./gcs delete "Skyrim" "TV"
  ```

### Variable Substitution
You can inject values into your config files at runtime using the `-v` flag. This works with both `use` and `useall`.

1.  **Prepare your source file:** Use `${variableName}` tags.
    *Source File (`template.ini`):*
    ```ini
    [Display]
    ResolutionWidth=${x}
    ResolutionHeight=${y}
    ```

2.  **Run the command:**
    ```bash
    ./gcs use "MyGame" "TV" -v x:3840 -v y:2160
    ```

3.  **Result:** The destination file is written as:
    ```ini
    [Display]
    ResolutionWidth=3840
    ResolutionHeight=2160
    ```

## Configuration File
GCS uses a plain text INI file named `gcs.ini` to store data. Sections are formatted as `[GameName|ProfileName]`.

**Example `gcs.ini`:**
```ini
[Skyrim|TV]
src=./configs/tv_graphics.ini
dst=/home/user/games/skyrim/prefs.ini
src=./configs/controller.ini
dst=/home/user/games/skyrim/controls.txt

[Skyrim|Monitor]
src=./configs/monitor_graphics.ini
dst=/home/user/games/skyrim/prefs.ini
```

## Todo
- [ ] Add support for dry-run (preview mode).

- [ ] Add backup functionality before overwriting destination files.

## Documentation

Doxygen Documentation is available at:
[https://veryfunnymonkey.github.io/GamesConfigSwitcher/](https://veryfunnymonkey.github.io/GamesConfigSwitcher/)

To generate the documentation locally:
```bash
make doc
```
Or
```bash
doxygen Doxyfile
```
