using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class UseAllCommand
    {
        private readonly IGameDataManager _gameManager;
        public UseAllCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command ("useall", Description = "Copy all the matching profiles for all games to their respective profile config path.")]
        public async Task useall
        (
            [Option('p', Description = "The title of the profile")] string profile,
            [Option('v', Description = "variable to be found and replaced in all profiles, used in the format \"variable:value\"")] List<string>? variable

        )
        {
            var gameData = _gameManager.LoadGameData();

            if (gameData.Games != null && gameData.Games.Count > 0)
            {
                foreach (var game in gameData.Games)
                {
                    var selectedProfile = game.Profiles.FirstOrDefault(p => p.title.Equals(profile, StringComparison.OrdinalIgnoreCase));
                    if (selectedProfile  != null)
                    {
                        FileHelper.profileCopier(selectedProfile.profilePath, game.configPath);

                        if (variable != null) // copy and pasted from "use", probably should turn this into a shared function later
                        {
                            var variables = new Dictionary<string, string>();

                            foreach (var variableString in variable)
                            {
                                var variableArray = variableString.Split(':', StringSplitOptions.RemoveEmptyEntries);
                                if ((variableArray.Length % 2) != 0)
                                {
                                    Console.WriteLine("Invalid number of arguments in variable");
                                    return;

                                }

                                var key = variableArray[0];
                                var value = variableArray[1];

                                variables.Add(key, value);

                                VariableHandler.useVariable(game.configPath, variables);
                            }
                        }
                    }

                }
            }
            else
            {
                Console.WriteLine("No games found.");
            }
        }
    }
}