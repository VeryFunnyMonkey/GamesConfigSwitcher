using GamesConfigSwitcher;
using Newtonsoft.Json;
using System;
using System.IO;

public static class CommandLineHandler
{
    private static string jsonFilePath = "gameData.json";

    public static void Handle(string[] args)
    {
        var gameDataManager = new GameDataManager(jsonFilePath);

        if (args.Length == 0 || args[0].ToLower() == "help")
        {
            ShowHelp();
            return;
        }

        switch (args[0].ToLower())
        {

            case "add":
                if (args.Length < 4)
                {
                    Console.WriteLine("Usage: add <GameTitle> <ConfigPath> <Profile1Path> <Profile2Path>");
                }
                else
                {
                    gameDataManager.AddGameData(args[1], args[2], args[3], args[4]);
                    Console.WriteLine($"Game '{args[1]}' added successfully.");
                }
                break;

            case "list":
                ListGames(gameDataManager);
                break;

            default:
                Console.WriteLine($"Unknown command: {args[0]}");
                break;
        }
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

    private static void ShowHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("  help         Shows this help message.");
        Console.WriteLine("  add          Adds a new game. Usage: add <GameTitle> <ConfigPath> <Profile1Path> <Profile2Path>");
        Console.WriteLine("  list         Lists all games and their profiles.");
        Console.WriteLine();
        Console.WriteLine("Example:");
        Console.WriteLine("  YourApp.exe add \"NewGame\" \"C:\\path\\to\\configfile.txt\" \"C:\\path\\to\\profile1.txt\" \"C:\\path\\to\\profile2.txt\"");
        Console.WriteLine("  YourApp.exe list");
    }
}
