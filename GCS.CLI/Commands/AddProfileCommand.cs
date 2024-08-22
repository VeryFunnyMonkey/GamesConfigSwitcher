using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class AddProfileCommand
    {
        private readonly IGameDataManager _gameManager;
        public AddProfileCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command("profile", Description = "Adds a new profile to a game.")]
        public void AddProfile
        (
            [Argument(Description = "The title of the profile")] string title,
            [Argument(Description = "The title of the game")] string game,
            [Option('s', Description = "The source config file to be copied to the destination")] List<string> source,
            [Option('d', Description = "The config file that will be replaced by the source config file (usually the games config file)")] List<string> destination
        )

        {
            if (source.Count != destination.Count)
            {
                Console.WriteLine($"Non-matching number of config file locations, source: {source.Count}, destination: {destination.Count}");
                return;
            }

            var configFiles = new List<ConfigFile>();

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

            _gameManager.AddProfile(title, game, configFiles);
        }
    }
}