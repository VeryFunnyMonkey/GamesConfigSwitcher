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

        [Command ("profile", Description = "edit an existing game.")]
        public async Task EditProfile
        (
            [Argument(Description = "The new title of the profile")] string title,
            [Option('g', Description = "The title of the game")] string game,
            [Option('p', Description = "The old title of the profile you want to update")] string profile,
            [Option('s', Description = "The source config file to be copied to the destination")] List<string>? source,
            [Option('d', Description = "The config file that will be replaced by the source config file (usually the games config file)")] List<string>? destination 
        )

        {
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
                        configFiles.Add(new ConfigFile {SourceFile = configSource, DestinationFile = configDest});
                    }
                }
            }
            else
            {
                configFiles = null;
            }
            
            _gameManager.EditProfile(profile, game, new Profile { Title = title, ConfigFiles = configFiles });
        }
    }
}