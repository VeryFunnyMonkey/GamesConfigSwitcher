# Game Config Switcher (GCS)
## Easily Copy Game Config File Profiles via UI or CLI

<p align="center">
  <img width="354" alt="UI Screenshot" src="https://github.com/user-attachments/assets/bad2873d-eebc-4ca9-bb17-08d4c08b4ac9"> 
  <img width="1151" alt="CLI Screenshot" src="https://github.com/user-attachments/assets/a66e6b1f-8ea3-4ec0-9615-2a6da660eb9e">
</p>

**GCS** is a tool designed to alleviate the hassle of manually adjusting in-game resolutions when switching between multiple monitors (e.g., TV, PC Monitor, Moonlight Streaming). Some games fail to automatically adjust the resolution to the correct monitor, forcing you to change settings manually. GCS solves this problem by allowing you to easily swap out game config files using either a graphical interface or the command line. You can further automate this process with tools like AutoHotkey (AHK).

## Features
- **Swap Config Files:** Easily switch between different config files using a UI or CLI.
- **Multiple Profiles:** Create and manage multiple game profiles for different use cases.

## Usage
_Both the CLI and UI share the same JSON file for storing game data, so you can switch between them seamlessly. The UI is primarily designed for adding and managing games, while the CLI is ideal for quickly applying profiles._

### Game Profile Structure
A game profile consists of the following:

- **Game Title:** The name of the game.
- **Config Path:** The path to the game's main config file. For example, for Skyrim, this would be:
  ```\users\username\My Documents\My Games\Skyrim\skyrimprefs.ini```
* Profiles - the file that will replace the file in the Config Path

### CLI
You can execute commands by running the binary in a terminal, using: ```.\gcs```

```.\gcs help``` will list out the commands, and explain how to use them.

### UI (Windows Only)
Running the file: ```GCS.UI.exe``` opens a basic UI that allows you to use game profiles, add new games, or edit the profile paths of games.

## Installation
GCS is portable and stores no appdata files. It only creates a gameData.json file in the directory the binary is executed.

### CLI
1. Download the latest gcs binary from the releases, matching your operating system and architecture.
2. On Unix based systems, you may need to make the ```gcs``` binary executable, by running:
   ```bash
   chmod +x gcs
4. Run the ```gcs``` executable, use the ```help``` command to see all available commands.

### UI (Windows Only)
1. Download the latest GCS.UI zip file from the releases.
3. Extract the files and un the  ```GCS.UI.exe``` executable.

## TODO
* ~~Implement unlimited profiles.~~
* Make UI pretty.
* ~~Create a json file if one is not present.~~
* Implement a feature that can swap variables within the config file with a value (good for changing resolution settings in a game's config file).
* Eventually add the ability to have multiple different config files. This is not in the scope currently, but would be a nice to have in the future.
