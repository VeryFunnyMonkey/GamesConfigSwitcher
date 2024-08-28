# Game Config Switcher (GCS)

**⚠️⚠️⚠️ NOTE: As of version 1.3 the UI has been removed, it can still be used as apart of v1.2.1.1 & under I have left references to it on the README for now, but they will be removed soon.⚠️⚠️⚠️**

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Usage](#usage)
  - [Game Profile Structure](#game-profile-structure)
  - [CLI](#cli)
    - [Commands](#commands)
    - [Variables](#variables)
  - [UI (Windows Only)](#ui-windows-only)
- [Installation](#installation)
  - [CLI](#cli-installation)
  - [UI (Windows Only)](#ui-windows-only)
- [Building](#building)
  - [Prerequisites](#prerequisites)
  - [Steps](#steps)
    - [Clone the Repository](#clone-the-repository)
    - [Restore Dependencies](#restore-dependencies)
    - [Build the Solution](#build-the-solution)
    - [Publish the CLI Application](#publish-the-cli-application)
    - [Publish the UI Application (Windows Only)](#publish-the-ui-application-windows-only)
    - [Locate the Binaries](#locate-the-binaries)
    - [Run the Application](#run-the-application)
- [Libraries and Dependencies](#libraries-and-dependencies)
- [Known Issues](#known-issues)
- [TODO](#todo)

## Overview
Easily Copy Game Config File Profiles via UI or CLI

<p align="center">
  <img width="354" alt="UI Screenshot" src="https://github.com/user-attachments/assets/bad2873d-eebc-4ca9-bb17-08d4c08b4ac9"> 
  <img width="935" alt="CLI Screenshot" src="https://github.com/user-attachments/assets/b7940bdf-e8ab-4d3c-a27c-123d98394a32">
</p>

**GCS** is a tool designed to alleviate the hassle of manually adjusting in-game resolutions when switching between multiple monitors (e.g., TV, PC Monitor, Moonlight Streaming). Some games fail to automatically adjust the resolution to the correct monitor, forcing you to change settings manually. GCS solves this problem by allowing you to easily swap out game config files using either a graphical interface or the command line. You can further automate this process with tools like AutoHotkey (AHK).

## Features
- **Multiple Profiles:** Create and manage multiple game profiles for different use cases.
- **Variables:** Define variables within the source file, and replacing it with whatever value is defined at runtime. See [variables](#variables) for more info.

## Usage
_Both the CLI and UI share the same JSON file for storing game data, so you can switch between them seamlessly. The UI is primarily designed for adding and managing games, while the CLI is ideal for quickly applying profiles._

### Game Profile Structure
A game profile consists of the following:

  - **Game Title:** The name of the game.
  - **Profiles:** - the source and destination pairs that will be replaced when the ``use`` command is run.

### CLI
You can execute commands by running the binary in a terminal, using: 
```bash
.\gcs
```
To list out the commands, and see their usage, run:
```bash
.\gcs --help
```
#### Commands

- **Add Game**
  - **Description:** Adds a new game.
  - Syntax
    ```bash
    .\gcs add game "Game Title"
    ```
  - **Usage Example:**
    ```bash
    .\gcs add game "Skyrim"
    ```
  - **Explanation:** This command adds a game titled "Skyrim".

  - **Add Profile**
  - **Description:** Adds a new profile to a game, with source and destination configuration paths. You must have at least 1 pair of source & destination paths, but can have as many pairs as you need.
  - Syntax
    ```bash
    .\gcs add profile "Profile Title" "Game Title" -s "<Path To Be Copied To Config File Destination>" -d "<Config File Destination>"
    ```
  - **Usage Example:**
    ```bash
    .\gcs add profile "tv" "Skyrim" -s "C:\Path\To\Profile1.ini" -d "\users\username\My Documents\My Games\Skyrim\skyrimprefs.ini"
    ```
  - **Explanation:** This command adds a profile titled "tv" to the game "Skyrim", with 1 source and destination pair. "Profile1" will be copied to the destination when the ```use``` command is run.

- **Delete Game**
  - **Description:** Deletes an existing game from the configuration.
  - Syntax
    ```bash
    .\gcs delete game "Game Title"
    ```
  - **Usage Example:**
    ```bash
    .\gcs delete game "Skyrim"
    ```
  - **Explanation:** This command deletes the game titled "Skyrim" from the configuration.

- **Delete Profile**
  - **Description:** Deletes an existing profile from a game.
  - Syntax
    ```bash
    .\gcs delete profile "Profile Title" "Game Title"
    ```
  - **Usage Example:**
    ```bash
    .\gcs delete "tv" "Skyrim"
    ```
  - **Explanation:** This command deletes the profile titled "tv" from the game titled "Skyrim".

- **Edit Game**
  - **Description:** Edits an existing game’s title.
  - **Syntax:**
    ```bash
    .\gcs edit "Old Game Title" "New Title"
    ```
  - **Usage Example:**
    ```bash
    .\gcs edit "Skyrim" "Skyrim SE"
    ```
  - **Explanation:** This command changes the title of "Skyrim" to "Skyrim SE".

- **Edit Profile**
  - **Description:** Edits an existing profile's title, source path, or destination path. Each option is optional, but at least 1 must be supplied.
  - **Syntax:**
    ```bash
    .\gcs edit "Profile Title" "Game Title" -t "New Profile Title" -s "New Source Path" -d "New Destination Path"
    ```
  - **Usage Example:**
    ```bash
    .\gcs edit "tv" "Skyrim SE" -t "pc" -s "C:\Path\To\NewProfile1.ini" -d "\users\username\My Documents\My Games\Skyrim Special Edition\skyrimprefs.ini"
    ```
  - **Explanation:** This command changes the title of the profile to to "pc" and updates the profile's source path and destination path.

- **List**
  - **Description:** Lists all games and their profiles along with the configuration paths.
  - **Syntax:**
    ```bash
    .\gcs list
    ```
  - **Usage Example:**
    ```bash
    .\gcs list
    ```
  - **Explanation:** This command lists all stored games, their configuration paths, and associated profiles.

- **Use**
  - **Description:** Copies the selected profile's source path file to the profile's destination path file. [variables](#variables) are optional.
  - **Syntax:**
    ```bash
    .\gcs use "Game Title" "Profile Title" -v "variable:value"
    ```
  - **Usage Example:**
    ```bash
    .\gcs use "Skyrim" "Profile1" -v "x:3840" -v "y:2160"
    ```
  - **Explanation:** This command copies "Profile1" for "Skyrim" to its destination path file, replacing variables `x` and `y` with `3840` and `2160` respectively.

- **UseAll**
  - **Description:** Copies the matching profiles for all games to their destination path file. [variables](#variables) are optional.
  - **Syntax:**
    ```bash
    .\gcs useall "Profile Title" -v "variable:value"
    ```
  - **Usage Example:**
    ```bash
    .\gcs useall "Profile1" -v "x:3840" -v "y:2160"
    ```
  - **Explanation:** This command copies "Profile1" for all games to their their destination path file, replacing variables `x` and `y` with `3840` and `2160` respectively.

#### Variables
_Currently only available in CLI_

When using the ```use``` or ```useall``` command, you can optionally pass in the ```--variable -v``` option to use the variables function.
It works by finding a variable within the source file, and replacing it with whatever value is defined at runtime.
Variables are defined in the source file using the following format: ```${variable}```
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
```.\gcs use myGame tv -v x:3840 -v y:2160```
This will set the x & y value to 3840 & 2160 respectively.

### UI (Windows Only)
Running the file: ```GCS.UI.exe``` opens a basic UI that allows you to use game profiles, add new games, or edit the profile paths of games.

## Installation
GCS is portable and stores no appdata files. It only creates a gameData.json file in the directory the binary is executed.

### CLI Installation
1. Download the latest gcs binary from the releases, matching your operating system and architecture.
2. On Unix based systems, you may need to make the ```gcs``` binary executable, by running:
   ```bash
   chmod +x gcs
4. Run the ```gcs``` executable, use the ```--help``` option to see all available commands.

### UI Installation (Windows Only)
1. Download the latest GCS.UI zip file from the releases.
3. Extract the files and un the  ```GCS.UI.exe``` executable.

## Building
### Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Steps

#### Clone the Repository
   ```bash
   git clone https://github.com/VeryFunnyMonkey/GamesConfigSwitcher.git
   cd GamesConfigSwitcher
   ```
   
#### Restore Dependencies
   Navigate to the root directory of your project and run:
   ```bash
   dotnet restore
   ```

#### Build the Solution
   To build the solution, use the following command:
   ```bash
   dotnet build --configuration Release
   ```
   
#### Publish the CLI Application
   To create a single-file executable for the CLI, run:
   ```bash
   dotnet publish GCS.CLI -r <runtime> -c Release /p:PublishSingleFile=true --output ./output/cli
   ```
   __See [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog#known-rids) for the available runtimes__
   
#### Publish the UI Application (Windows Only)
   To create a single-file executable for the CLI, run:
   ```bash
   dotnet publish GCS.UI -r <runtime> -c Release /p:PublishSingleFile=true --output ./output/ui
   ```
   __See [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog#known-rids) for the available runtimes__
   
#### Locate the Binaries
   After publishing, the executables will be located in the ```./output/cli``` and ```./output/ui``` directories for the CLI and UI respectively.

#### Run the Application
   
   **- CLI:** Navigate to the ```./output/cli``` directory and run:
   ```bash
   ./gcs --help
   ```
   **- UI:** Navigate to the ./publish/ui directory and double-click the GCS.UI.exe file to launch the UI.

## Libraries and Dependencies
GCS relies on several libraries and packages to function. Below are the key libraries used:

- **[Cocona](https://github.com/mayuki/Cocona):** Used for handling command-line interface commands and arguments.
- **Newtonsoft.Json:** Used for handling JSON serialization and deserialization.
- **Microsoft.Extensions.DependencyInjection:** Provides dependency injection capabilities for the application (currently only used in CLI).
- **Microsoft.Extensions.Logging:** Provides logging capabilities for the application (File logging to come in future update).
- **WinForms:** Used for building the graphical user interface for the UI (Windows Only).

All necessary dependencies are restored automatically when you run `dotnet restore`. If you wish to explore or modify the dependencies, you can find them listed in the `.csproj` files of the respective projects.

## Known Issues
* None, raise a issue if you find one :)

## TODO
* Remake the UI in Avalonia