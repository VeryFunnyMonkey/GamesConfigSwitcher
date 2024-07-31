using GCS.Core;
using GCS.UI;

namespace GCS.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gameData.json");
            var gameDataManager = new GameDataManager(jsonFilePath);

            switch (args[0].ToLower())
            {
                case "help":
                    ShowHelp();
                    break;

                case "add":
                    if (args.Length < 5)
                    {
                        Console.WriteLine("Usage: add <GameTitle> <ConfigPath> <Profile1Path> <Profile2Path>");
                    }
                    else
                    {
                        if (!PathValidator.IsValidWindowsFilePath(args[2]) || !PathValidator.IsValidWindowsFilePath(args[3]) || !PathValidator.IsValidWindowsFilePath(args[4]))
                        {
                            Console.WriteLine("One or more file paths are invalid. Please check the paths and try again.");
                            return;
                        }
                        gameDataManager.AddGameData(args[1], args[2], args[3], args[4]);
                    }
                    break;

                case "list":
                    ListGames(gameDataManager);
                    break;

                case "use": // this is not great, going to re-do when I add support for unlimited profiles

                    if (args.Length < 3)
                    {
                        Console.WriteLine("Usage: use <GameTitle> <profile>");
                    }
                    else
                    {

                        var gameData = gameDataManager.LoadGameData();

                        if (gameData.Games != null && gameData.Games.Count > 0)
                        {
                            var selectedGame = gameData.Games.FirstOrDefault(g => g.Title == args[1]);

                            string profilePath = null;

                            if (selectedGame != null)
                            {
                                string configPath = selectedGame.configPath;

                                if (args[2].ToLower() == "profile1")
                                {
                                    profilePath = selectedGame.Profiles.Profile1;
                                }
                                if (args[2].ToLower() == "profile2")
                                {
                                    profilePath = selectedGame.Profiles.Profile2;
                                }

                                if (profilePath != null)
                                {
                                    useProfile(profilePath, configPath);
                                }
                                else
                                {
                                    Console.WriteLine("Profile not found.");
                                }
                                break;
                            }

                            else
                            {
                                Console.WriteLine("The selected game could not be found.");
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("No games found.");
                        }


                    }
                    break;



                default:
                    Console.WriteLine($"Unknown command: {args[0]}");
                    ShowHelp();
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  help         Shows this help message.");
            Console.WriteLine("  add          Adds a new game. Usage: add <GameTitle> <ConfigPath> <Profile1Path> <Profile2Path>");
            Console.WriteLine("  list         Lists all games and their profiles.");
            Console.WriteLine("  use          Copy a profile to the game's config path.");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  GCS.CLI.exe add \"NewGame\" \"C:\\path\\to\\configfile.txt\" \"C:\\path\\to\\profile1.txt\" \"C:\\path\\to\\profile2.txt\"");
            Console.WriteLine("  GCS.CLI.exe list");
            Console.WriteLine("  GCS.CLI.exe use \"Elden Ring\" profile1");
        }

        private static void ListGames(GameDataManager gameDataManager)
        {
            var gameData = gameDataManager.LoadGameData();

            if (gameData.Games != null && gameData.Games.Count > 0)
            {
                foreach (var game in gameData.Games)
                {
                    Console.WriteLine($"Title: {game.Title}");
                    Console.WriteLine($"  Config Path: {game.configPath}");
                    Console.WriteLine($"  Profile 1: {game.Profiles.Profile1}");
                    Console.WriteLine($"  Profile 2: {game.Profiles.Profile2}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No games found.");
            }
        }

        private static void useProfile(string profilePath, string configPath)
        {
            FileHelper.profileCopier(profilePath, configPath);
        }
    }
}
