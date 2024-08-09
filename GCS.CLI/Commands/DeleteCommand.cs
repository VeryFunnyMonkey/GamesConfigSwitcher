using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class DeleteCommand
    {
        private readonly IGameDataManager _gameManager;
        public DeleteCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command ("delete", Description = "Deletes a game.")]
        public async Task delete
        (
            [Option('t', Description = "The title of the game")] string title
        )
        {
            _gameManager.DeleteGameData(title);
        }

    }
}