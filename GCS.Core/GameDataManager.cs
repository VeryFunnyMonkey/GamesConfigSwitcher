using Newtonsoft.Json;

namespace GCS.Core
{ 
    public class GameDataManager(string filePath)
    {
        private readonly string jsonFilePath = filePath;

        public GameData LoadGameData()
        {
            if (File.Exists(jsonFilePath))
            {
                try
                {
                    string json = File.ReadAllText(jsonFilePath);
                    GameData gameData = JsonConvert.DeserializeObject<GameData>(json);
                    return gameData;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while loading the game data: {ex.Message}");
                }
            }
            else
            {
                GameData gameData = new GameData();
                string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
                File.WriteAllText(jsonFilePath, json);
                return gameData;
            }
        }

        public void SaveGameData(GameData gameData)
        {
            try
            {
                string updatedJson = JsonConvert.SerializeObject(gameData, Formatting.Indented);
                File.WriteAllText(jsonFilePath, updatedJson);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving the game data: {ex.Message}");
            }
        }

        public void AddGameData(string title, string configPath, List<Profile> profiles)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null)
            {
                gameData.Games = new List<Game>();
            }

            if (gameData.Games != null && gameData.Games.Any(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"A game with the title '{title}' already exists.");
                return;
            }

            var duplicateProfiles = profiles.GroupBy(p => p.title, StringComparer.OrdinalIgnoreCase)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();

            if (duplicateProfiles.Count != 0)
            {
                Console.WriteLine($"Duplicate profile titles found: {string.Join(", ", duplicateProfiles)}. Please ensure each profile has a unique title.");
                return;
            }

            var newGame = new Game
            {
                Title = title,
                configPath = configPath,
                Profiles = profiles
            };

            gameData.Games.Add(newGame);
            SaveGameData(gameData);
            Console.WriteLine($"Game '{title}' added successfully.");
        }

        public void DeleteGameData(string title)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null || gameData.Games.Count == 0)
            {
                Console.WriteLine("No games to delete.");
                return;
            }

            var gameToRemove = gameData.Games.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (gameToRemove != null)
            {
                gameData.Games.Remove(gameToRemove);
                SaveGameData(gameData);
                Console.WriteLine($"Game '{title}' has been deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Game with the title '{title}' not found.");
            }
        }

    }
}
