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

        [Command("list", Description = "Lists all games and their profiles.")]
        public void List()
        {
            var gameData = _gameManager.LoadGameData();

            if (gameData.Games != null && gameData.Games.Count > 0)
            {
                foreach (var game in gameData.Games)
                {
                    Console.WriteLine($"Title: {game.Title}");
                    if (game.Profiles != null)
                    {
                        foreach (var profile in game.Profiles)
                        {
                            Console.WriteLine($"  {profile.Title}: ");
                            foreach (var configFile in profile.ConfigFiles)
                            {
                                Console.WriteLine($"    {configFile.SourceFile} : {configFile.DestinationFile}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("  No profiles found.");
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