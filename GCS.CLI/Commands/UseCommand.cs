using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class UseCommand
    {
        private readonly IGameDataManager _gameManager;
        public UseCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command ("use", Description = "Copy a profile to the game's config path.")]
        public async Task use
        (
            [Option('t', Description = "The title of the game")] string title,
            [Option('p', Description = "The title of the profile")] string profile,
            [Option('v', Description = "variable to be found and replaced in the profile, used in the format \"variable:value\"")] List<string>? variable
        )
        {
            var gameData = _gameManager.LoadGameData();

            if (gameData.Games != null && gameData.Games.Count > 0)
            {
                var selectedGame = gameData.Games.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                if (selectedGame != null)
                {
                    var selectedProfile = selectedGame.Profiles.FirstOrDefault(p => p.title.Equals(profile, StringComparison.OrdinalIgnoreCase));
                    if (selectedProfile != null)
                    {
                        ProfileHelper.HandleProfile(selectedProfile.profilePath, selectedGame.configPath, variable);
                    }

                    else
                    {
                        Console.WriteLine("Profile not found.");
                    }
                }
                else
                {
                    Console.WriteLine("The selected game could not be found.");
                }
            }
            else
            {
                Console.WriteLine("No games found.");
            }
        }
    }
}