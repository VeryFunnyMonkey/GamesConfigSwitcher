using GCS.Core;
using GCS.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                    if (args.Length < 5 || (args.Length - 2) % 2 != 1)
                    {
                        Console.WriteLine(args.Length);
                        Console.WriteLine((args.Length - 2) % 2);
                        Console.WriteLine("Usage: add <GameTitle> <ConfigPath> <ProfileTitle1> <ProfilePath1> [<ProfileTitle2> <ProfilePath2> ...]");
                    }
                    else
                    {
                        Console.WriteLine(args.Length);
                        Console.WriteLine((args.Length - 2) % 2);
                        string title = args[1];
                        string configPath = args[2];
                        var profiles = new List<Profile>();

                        for (int i = 3; i < args.Length; i += 2)
                        {
                            string profileTitle = args[i];
                            string profilePath = args[i + 1];

                            if (!PathValidator.IsValidWindowsFilePath(profilePath))
                            {
                                Console.WriteLine($"The file path '{profilePath}' is invalid. Please check the path and try again.");
                                return;
                            }

                            profiles.Add(new Profile { title = profileTitle, profilePath = profilePath });
                        }

                        gameDataManager.AddGameData(title, configPath, profiles);
                    }
                    break;

                case "list":
                    ListGames(gameDataManager);
                    break;

                case "use":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("Usage: use <GameTitle> <ProfileTitle>");
                    }
                    else
                    {
                        var gameData = gameDataManager.LoadGameData();

                        if (gameData.Games != null && gameData.Games.Count > 0)
                        {
                            var selectedGame = gameData.Games.FirstOrDefault(g => g.Title.Equals(args[1], StringComparison.OrdinalIgnoreCase));

                            if (selectedGame != null)
                            {
                                var selectedProfile = selectedGame.Profiles.FirstOrDefault(p => p.title.Equals(args[2], StringComparison.OrdinalIgnoreCase));
                                if (selectedProfile != null)
                                {
                                    useProfile(selectedProfile.profilePath, selectedGame.configPath);
                                }
                                else
                                {
                                    Console.WriteLine("Profile not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("The selected game could not be found.");
                            }
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
            Console.WriteLine("  add          Adds a new game. Usage: add <GameTitle> <ConfigPath> <ProfileTitle1> <ProfilePath1> [<ProfileTitle2> <ProfilePath2> ...]");
            Console.WriteLine("  list         Lists all games and their profiles.");
            Console.WriteLine("  use          Copy a profile to the game's config path. Usage: use <GameTitle> <ProfileTitle>");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("  GCS.CLI.exe add \"NewGame\" \"C:\\path\\to\\configfile.txt\" \"Profile1\" \"C:\\path\\to\\profile1.txt\" \"Profile2\" \"C:\\path\\to\\profile2.txt\"");
            Console.WriteLine("  GCS.CLI.exe list");
            Console.WriteLine("  GCS.CLI.exe use \"NewGame\" \"Profile1\"");
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

        private static void useProfile(string profilePath, string configPath)
        {
            FileHelper.profileCopier(profilePath, configPath);
        }
    }
}
