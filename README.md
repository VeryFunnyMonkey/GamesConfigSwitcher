# Game Config Switcher (GCS)
## Copy game config file profiles using UI or CLI

Some games fail to automatically adjust the monitor in-game resolution to the correct resolution for the primary monitor. In setups where you may use multiple different monitors (TV, PC Monitor, Moonlight Streaming), this can be a pain, as you need to go into the settings and change the in-game resolution manually. My solution to this, is developing this program that lets you swap out config files for games using a UI or CLI. This can then be further automated with a tool like AHK.

## Features
* Swap out config files using a UI or CLI.
* Create multiple game profiles, for different use-cases.

## Usage
_Both the CLI & UI share the same json file for storing game data, you can swap between them without any issues.
I created the UI for the purpose of adding games, as I found it easier than CLI, but I use the CLI to use my game profiles_

Both CLI and the UI work in similiar ways, a game profile of comprised of the following:

* Game Title - title of the game
* Config Path - the main path of the game's config file. E.g. for Skyrim this would be
  ```\users\username\My Documents\My Games\Skyrim\skyrimprefs.ini```
* Profiles - the file that will replace the file in the Config Path

### CLI
You can execute commands by running the binary in a terminal, using ```.\gcs```
```.\gcs help``` will list out the commands, and explain how to use them.

### UI (Windows Only)
Running the file: ```GCS.UI.exe``` opens a basic UI that allows you to use game profiles, add new games, or edit the profile paths of games.

## Installation
GCS is portable and stores no appdata files. It only creates a gameData.json file in the directory the binary is executed.

### CLI
1. Download the latest gcs binary from the releases, matching your operating system and architecture.
2. On Unix based systems, you may need to make the ```gcs``` binary executable, by running ```chmod +x gcs```.
3. Run the ```gcs``` executable, use the ```help``` command to see all available commands.

### UI (Windows Only)
1. Download the latest GCS.UI zip file from the releases.
3. Extract the files and un the  ```GCS.UI.exe``` executable.

## TODO
* ~~Implement unlimited profiles.~~
* Make UI pretty.
* ~~Create a json file if one is not present.~~
* Implement a feature that can swap variables within the config file with a value (good for changing resolution settings in a game's config file).
* Eventually add the ability to have multiple different config files. This is not in the scope currently, but would be a nice to have in the future.
