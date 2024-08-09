using Cocona;
using GCS.Core;

namespace GCS.CLI
{
    public class AddCommand
    {
        private readonly IGameDataManager _gameManager;
        public AddCommand(IGameDataManager gameManager)
        {
            _gameManager = gameManager;
        }

        [Command ("add", Description = "Adds a new game.")]
        public async Task Add
        (            
            [Option('t', Description = "The title of the game")] string title,
            [Option('c', Description = "The path to the game config")] string config,
            [Option('p', Description = "The profile title and path in the format \"<ProfileTitle1> <ProfilePath1>\" (can be used multiple times)")] List<string> profile 
            // ^^ not a huge fan of this, look at making an argument in the future so users dont have to use ""
        )

        {
            var profiles = new List<Profile>();
            foreach (var profileString in profile)
            {
                var profileArray = profileString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if ((profileArray.Length % 2) != 0)
                {
                    Console.WriteLine("Invalid number of arguments in profile");
                    return;

                }

                var profileTitle = profileArray[0];
                var profilePath = profileArray[1];

                if (!PathValidator.IsValidWindowsFilePath(profilePath))
                {
                    Console.WriteLine($"The file path '{profilePath}' is invalid. Please check the path and try again.");
                    return;
                }

                else
                {
                    profiles.Add(new Profile { title = profileTitle, profilePath = profilePath });
                }
            }

            _gameManager.AddGameData(title, config, profiles);

        }
    }
}