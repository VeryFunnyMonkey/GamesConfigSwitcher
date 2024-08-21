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

        [Command ("game", Description = "edit an existing game.")]
        public async Task EditGame
        (
            [Argument(Description = "The new title of the game")] string title,
            [Option('g', Description = "The old title of the game you want to update")] string game          
        )

        {
            _gameManager.EditGameData(game, title);
        }
    }
}