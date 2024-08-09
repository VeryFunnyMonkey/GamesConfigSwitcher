using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class ListCommand
    {
        private readonly IGameDataManager _gameManager;
        public ListCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command ("list")]
        public async Task List()
        {
            var gameData = _gameManager.LoadGameData();

            if (gameData.Games != null && gameData.Games.Count > 0)
            {
                foreach (var game in gameData.Games)
                {
                    Console.WriteLine($"Title: {game.Title}");
                    Console.WriteLine($"  Config Path: {game.configPath}");
                    foreach (var profile in game.Profiles)
                    {
                        Console.WriteLine($"  {profile.title}: {profile.profilePath}");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No games found.");
            }
        }

    }
}