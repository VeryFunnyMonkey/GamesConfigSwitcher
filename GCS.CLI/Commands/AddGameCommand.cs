using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class AddGameCommand
    {
        private readonly IGameDataManager _gameManager;
        public AddGameCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command("game", Description = "Adds a new game.")]
        public void AddGame
        (
            [Argument(Description = "The title of the game")] string title
        )

        {
            _gameManager.AddGameData(title);
        }
    }
}