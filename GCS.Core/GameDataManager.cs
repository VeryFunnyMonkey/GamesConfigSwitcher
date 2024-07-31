using Newtonsoft.Json;

namespace GCS.Core
{ 
    public class GameDataManager
    {
        private string jsonFilePath;

        public GameDataManager(string filePath)
        {
            jsonFilePath = filePath;
        }

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
                throw new FileNotFoundException($"The JSON file was not found at: {jsonFilePath}");
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

        public void AddGameData(string title, string configPath, string profile1, string profile2)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null)
            {
                gameData.Games = new List<Game>();
            }

            if (gameData.Games.Any(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"A game with the title '{title}' already exists.");
                return;
            }

            var newGame = new Game
            {
                Title = title,
                configPath = configPath,
                Profiles = new Profile
                {
                    Profile1 = profile1,
                    Profile2 = profile2
                }
            };

            gameData.Games.Add(newGame);
            SaveGameData(gameData);
            Console.WriteLine($"Game '{title}' added successfully.");
        }
    }
}
