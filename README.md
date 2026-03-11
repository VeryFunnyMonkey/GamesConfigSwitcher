# Game Config Switcher (GCS)

Game Config Switcher (GCS) is a C-based tool that comes with both a CLI and a native GTK3 GUI. It lets you swap out config files for games and applications, which is handy if you regularly move between different setups (e.g., Desktop to TV, or PC to Steam Deck). 

It also handles variable substitution, so you can inject values like resolution or refresh rate into your configs at runtime.

## Table of Contents
- [Features](#features)
- [Project Structure](#project-structure)
- [Building](#building)
- [Usage (GUI)](#usage-gui)
- [Usage (CLI)](#usage-cli)
- [Configuration File](#configuration-file)
- [Documentation](#documentation)

## Features
- **CLI & GUI:** Use `gcs` for terminal use and scripting, or `gcs-gui` if you prefer a visual interface.
- **Hybrid GUI Binary:** The `gcs-gui` executable also accepts CLI arguments. If you pass commands to it, it runs headlessly without opening the window.
- **Low Overhead:** Written in POSIX C. No heavy runtimes (like Electron, .NET, or Java) required.
- **Multi-file Profiles:** Swap out several files at once under a single profile (e.g., `graphics.ini` and `controls.ini`).
- **Variables:** Put placeholders like `${width}` in your template files and fill them at runtime.
- **Global Apply:** Push a profile name (like "TV") to every game in your list with one command.
- **Simple Storage:** Saves all configurations to a plain text `gcs.ini` file.

## Project Structure

```text
GamesConfigSwitcher/
├── Makefile             # Build automation
├── gcs.ini              # Config storage (created on first run)
├── include/             # Headers (.h)
│   └── gcs.h            # Data structures & prototypes
└── src/                 # Implementation (.c)
    ├── main.c           # CLI entry point
    ├── gui_main.c       # GTK3 GUI entry point
    ├── cli.c            # Shared command-line logic
    ├── config.c         # INI parsing & list management
    ├── ops.c            # File copying & variable logic
    └── utils.c          # Helper functions
```

## Building

### Prerequisites
To build the CLI, you only need GCC/Clang and Make. 
To build the GUI, you also need the GTK3 development headers and `pkg-config`.

* **Arch Linux:** `sudo pacman -S gtk3 pkgconf base-devel`
* **Ubuntu/Debian:** `sudo apt install libgtk-3-dev pkg-config build-essential`
* **Windows (MSYS2):** `pacman -S mingw-w64-x86_64-gcc mingw-w64-x86_64-gtk3 mingw-w64-x86_64-pkgconf make`

### Build Steps
1.  Clone the repository:
    ```bash
    git clone [https://github.com/YourUsername/GamesConfigSwitcher.git](https://github.com/YourUsername/GamesConfigSwitcher.git)
    cd GamesConfigSwitcher
    ```

2.  Compile:
    ```bash
    make
    ```
    This generates the `gcs` and `gcs-gui` binaries.

3.  Clean build artifacts (optional):
    ```bash
    make clean
    ```

---

## Usage (GUI)

Launch the app:
```bash
./gcs-gui
```

* **Managing Games & Profiles:** Use the top bar to select a Game and Profile. Click **Add**, **Edit**, or **Delete** to modify them. 
* **Mapping Files:** The middle pane shows the config files for the selected profile. Click **Add Pair** to select your Source (template) and Destination (the live config file).
* **Applying Profiles:** Enter any variables you need (e.g., `RES:1080p FPS:60`) in the bottom text box and click **Apply Selected Profile** (or hit `Enter`). 
* **Global Apply:** Click **Use All** to apply the active Profile Name to every game currently tracked by GCS.

---

## Usage (CLI)

**Note:** The examples below use the `gcs` binary, but you can safely swap it out for `gcs-gui` if that's the only binary you have compiled or deployed. 

### Adding Profiles
Use the `add` command to create a profile. You'll need the Game Name, Profile Name, Source file, and Destination file.

```bash
./gcs add "Game Name" "Profile Name" -s "./path/to/source" -d "./path/to/dest"
```

To attach a second file to the *same* profile, run the command again with the same Game and Profile names:

```bash
# Add graphics config
./gcs add "Skyrim" "TV" -s "./configs/skyrim_tv_graphics.ini" -d "~/Documents/My Games/Skyrim/SkyrimPrefs.ini"

# Add controls config
./gcs add "Skyrim" "TV" -s "./configs/skyrim_controller.ini" -d "~/Documents/My Games/Skyrim/ControlMap.txt"
```

### Using Profiles
The `use` command pushes your source files to the destination paths.

```bash
./gcs use "Skyrim" "TV"
```

### Global Switch (UseAll)
The `useall` command applies a specific profile name to **all** your games at once. Games that lack a matching profile name are ignored.

```bash
# Switches Skyrim, Cyberpunk, and Witcher 3 to their "TV" profiles
./gcs useall "TV"
```

### Editing
Modify existing entries using the `edit` command.

**1. Rename a Game (updates all associated profiles):**
```bash
./gcs edit game "Skyrim" "The Elder Scrolls V"
```

**2. Rename a Profile:**
```bash
./gcs edit profile "Skyrim" "TV" -n "LivingRoom"
```

**3. Update Profile Paths:**
*(Note: This replaces the file list for the profile)*
```bash
./gcs edit profile "Skyrim" "TV" -s "./new_src.ini" -d "./new_dest.ini"
```

### Listing & Deleting
- **List everything:**
  ```bash
  ./gcs list
  ```

- **Delete a profile and its file mappings:**
  ```bash
  ./gcs delete "Skyrim" "TV"
  ```

### Variable Substitution
Pass values using the `-v` flag to replace variables in your config templates. This works with `use` and `useall`.

1.  Set up your template file with `${variable}` tags:
    ```ini
    [Display]
    ResolutionWidth=${x}
    ResolutionHeight=${y}
    ```

2.  Run the command with your variables:
    ```bash
    ./gcs use "MyGame" "TV" -v x:3840 -v y:2160
    ```

3.  The destination file will output:
    ```ini
    [Display]
    ResolutionWidth=3840
    ResolutionHeight=2160
    ```

## Configuration File
GCS reads and writes to a plain text INI file named `gcs.ini`. Sections use the `[GameName|ProfileName]` format.

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
- [ ] Support dry-run (preview mode).
- [ ] File backups before overwriting.

## Documentation

Doxygen docs are available at:
https://veryfunnymonkey.github.io/GamesConfigSwitcher/

To build the docs locally:
```bash
make doc
# or
doxygen Doxyfile
```