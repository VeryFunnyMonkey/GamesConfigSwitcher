using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class EditProfileCommand
    {
        private readonly IGameDataManager _gameManager;
        public EditProfileCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command("profile", Description = "edit an existing game.")]
        public void EditProfile
        (
            [Argument(Description = "The title of the profile")] string currentTitle,
            [Argument(Description = "The title of the game")] string game,
            [Option('s', Description = "The source config file to be copied to the destination")] List<string>? source,
            [Option('d', Description = "The config file that will be replaced by the source config file (usually the games config file)")] List<string>? destination,
            [Option('t', Description = "The new title of the profile")] string? title = null
        )

        {
            if (source == null && destination == null && title == null)
            {
                Console.WriteLine("No options provided to edit the profile. Please provide at least one option.");
                return;
            }

            if (source == null || destination == null)
            {
                Console.WriteLine("Source and destination must be updated together. Please provide both options.");
                return;
            }

            var configFiles = new List<ConfigFile>();
            if (source != null & destination != null)
            {
                if (source.Count != destination.Count)
                {
                    Console.WriteLine($"Non-matching number of config file locations, source: {source.Count}, destination: {destination.Count}");
                    return;
                }

                for (var i = 0; i < source.Count; i++)
                {
                    var configSource = source[i];

                    var configDest = destination[i];

                    if (!PathValidator.IsValidWindowsFilePath(configSource))
                    {
                        Console.WriteLine($"The source path '{configSource}' is invalid. Please check the path and try again.");
                        return;
                    }

                    else if (!PathValidator.IsValidWindowsFilePath(configDest))
                    {
                        Console.WriteLine($"The destination path '{configDest}' is invalid. Please check the path and try again.");
                        return;
                    }
                    else
                    {
                        configFiles.Add(new ConfigFile { SourceFile = configSource, DestinationFile = configDest });
                    }
                }
            }

            else
            {
                configFiles = null;
            }

            _gameManager.EditProfile(currentTitle, game, new Profile { Title = title, ConfigFiles = configFiles });
        }
    }
}