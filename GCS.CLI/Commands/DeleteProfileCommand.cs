using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class DeleteProfileCommand
    {
        private readonly IGameDataManager _gameManager;
        public DeleteProfileCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command("profile", Description = "Deletes a profile from a game.")]
        public void DeleteProfile
        (
            [Argument(Description = "The title of the profile")] string title,
            [Argument(Description = "The title of the game")] string game
        )
        {
            _gameManager.DeleteProfile(title, game);
        }
    }
}