# Game Config Switcher (GCS)
## Easily Copy Game Config File Profiles via UI or CLI

<p align="center">
  <img width="354" alt="UI Screenshot" src="https://github.com/user-attachments/assets/bad2873d-eebc-4ca9-bb17-08d4c08b4ac9"> 
  <img width="935" alt="CLI Screenshot" src="https://github.com/user-attachments/assets/b7940bdf-e8ab-4d3c-a27c-123d98394a32">
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

```.\gcs --help``` will list out the commands, and explain how to use them.

#### Variables
_Currently only available in CLI_

When using the ```use``` or ```useall``` command, you can optionally pass in the ```--variable -v``` option to use the variables function.
It works by finding a variable within the profile file, and replacing it with whatever value is defined at runtime.
Variables are defined in the profile file using the following format: ```${variable}```
When the ```--variable -v``` option is used, you can define the name of the variable and the value in the following format: ```variable:value```

***Example:***
You want to dynamically set the resolution of the game to your TVs resolution.

In your profile, for this example let's call it "tv", the way the game defines the resolution is with an x & y value, it may look something like this: 
```
x: 1920
y: 1080
```
You would replace the values with a variable like so:
```
x: ${x}
y: ${y}
```
Then using the following command, you can set the games resolution at runtime:
```.\gcs use -t myGame -p tv -v x:3840 -v y:2160```
This will set the x & y value to 3840 & 2160 respectively.


### UI (Windows Only)
Running the file: ```GCS.UI.exe``` opens a basic UI that allows you to use game profiles, add new games, or edit the profile paths of games.

## Installation
GCS is portable and stores no appdata files. It only creates a gameData.json file in the directory the binary is executed.

### CLI
1. Download the latest gcs binary from the releases, matching your operating system and architecture.
2. On Unix based systems, you may need to make the ```gcs``` binary executable, by running:
   ```bash
   chmod +x gcs
4. Run the ```gcs``` executable, use the ```--help``` option to see all available commands.

### UI (Windows Only)
1. Download the latest GCS.UI zip file from the releases.
3. Extract the files and un the  ```GCS.UI.exe``` executable.

### Building
#### Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later

#### Steps
1. **Clone the Repository:**
   ```bash
   git clone https://github.com/VeryFunnyMonkey/GamesConfigSwitcher.git
   cd GamesConfigSwitcher
   ```
   
3. **Restore Dependencies:**
   
   Navigate to the root directory of your project and run:
   ```bash
   dotnet restore
   ```

5. **Build the Solution:**
   
   To build the solution, use the following command:
   ```bash
   dotnet build --configuration Release
   ```
   
6. **Publish the CLI Application:**
   
   To create a single-file executable for the CLI, run:
   ```bash
   dotnet publish GCS.CLI -r <runtime> -c Release /p:PublishSingleFile=true --output ./output/cli
   ```
   __See [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog#known-rids) for the available runtimes__
   
7. **Publish the UI Application (Windows Only):**
   
   To create a single-file executable for the CLI, run:
   ```bash
   dotnet publish GCS.UI -r <runtime> -c Release /p:PublishSingleFile=true --output ./output/ui
   ```
   __See [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog#known-rids) for the available runtimes__
   
8. **Locate the Binaries:**
   
   After publishing, the executables will be located in the ```./output/cli``` and ```./output/ui``` directories for the CLI and UI respectively.

9. **Run the Application:**
   
   **- CLI:** Navigate to the ```./output/cli``` directory and run:
   ```bash
   ./gcs --help
   ```
   **- UI:** Navigate to the ./publish/ui directory and double-click the GCS.UI.exe file to launch the UI.
   
## TODO
* ~~Implement unlimited profiles.~~
* Make UI pretty.
* ~~Create a json file if one is not present.~~
* ~~Implement a feature that can swap variables within the config file with a value (good for changing resolution settings in a game's config file).~~
* Eventually add the ability to have multiple different config files. This is not in the scope currently, but would be a nice to have in the future.
