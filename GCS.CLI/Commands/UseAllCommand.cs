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
                foreach (var selectedGame in gameData.Games)
                {
                    var selectedProfile = selectedGame.Profiles.FirstOrDefault(p => p.title.Equals(profile, StringComparison.OrdinalIgnoreCase));
                    if (selectedProfile  != null)
                    {
                        ProfileHelper.HandleProfile(selectedProfile.profilePath, selectedGame.configPath, variable);
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