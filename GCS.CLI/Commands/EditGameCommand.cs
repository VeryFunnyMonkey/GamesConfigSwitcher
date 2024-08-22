using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class EditGameCommand
    {
        private readonly IGameDataManager _gameManager;
        public EditGameCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command("game", Description = "edit an existing game.")]
        public void EditGame
        (
            [Argument(Description = "The title of the game")] string oldTitle,
            [Argument(Description = "The new title of the game")] string newTitle
        )

        {
            _gameManager.EditGameData(oldTitle, newTitle);
        }
    }
}