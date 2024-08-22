using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class DeleteGameCommand
    {
        private readonly IGameDataManager _gameManager;
        public DeleteGameCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command("game", Description = "Deletes a game.")]
        public void DeleteGame
        (
            [Argument(Description = "The title of the game")] string title
        )
        {
            _gameManager.DeleteGameData(title);
        }
    }
}