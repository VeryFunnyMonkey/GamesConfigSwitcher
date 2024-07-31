using GamesConfigSwitcher;
using Newtonsoft.Json;
using System;
using System.IO;

public static class CommandLineHandler
{
    private static string jsonFilePath = "gameData.json";

    public static void Handle(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No command-line arguments provided.");
            return;
        }

        switch (args[0].ToLower())
        {

            case "add":
                if (args.Length < 4)
                {
                    Console.WriteLine("Usage: add <GameTitle> <Profile1Path> <Profile2Path>");
                }
                else
                {
                    AddGame(args[1], args[2], args[3]);
                }
                break;

            case "list":
                ListGames();
                break;

            default:
                Console.WriteLine($"Unknown command: {args[0]}");
                break;
        }
    }

    private static void AddGame(string title, string profile1, string profile2)
    {
        var gameDataManager = new GameDataManager(jsonFilePath);
        var gameData = gameDataManager.LoadGameData();

        var newGame = new Game
        {
            Title = title,
            Profiles = new Profile
            {
                Profile1 = profile1,
                Profile2 = profile2
            }
        };

        gameData.Games.Add(newGame);
        gameDataManager.SaveGameData(gameData);

        Console.WriteLine($"Game '{title}' added successfully.");
    }

    private static void ListGames()
    {
        var gameDataManager = new GameDataManager(jsonFilePath);
        var gameData = gameDataManager.LoadGameData();

        if (gameData.Games != null && gameData.Games.Count > 0)
        {
            foreach (var game in gameData.Games)
            {
                Console.WriteLine($"Title: {game.Title}");
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
}
