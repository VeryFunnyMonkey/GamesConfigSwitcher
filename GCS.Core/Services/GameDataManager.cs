using Newtonsoft.Json;

namespace GCS.Core
{
    public class GameDataManager : IGameDataManager
    {
        private readonly string _jsonFilePath;
        private readonly ILoggingHandler _logger;
        
        public GameDataManager(string jsonFilePath, ILoggingHandler logger)
        {
            _jsonFilePath = jsonFilePath;
            _logger = logger;
        }

        public GameData LoadGameData()
        {
            if (File.Exists(_jsonFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_jsonFilePath);
                    return JsonConvert.DeserializeObject<GameData>(json) ?? new GameData();
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Failed to deserialize game data: {ex.Message}", ex);
                }
                catch (IOException ex)
                {
                    throw new Exception($"File access error: {ex.Message}", ex);
                }
            }

            return CreateEmptyGameData();
        }

        private GameData CreateEmptyGameData()
        {
            var gameData = new GameData();
            SaveGameData(gameData);
            return gameData;
        }

        public void SaveGameData(GameData gameData)
        {
            try
            {
                string updatedJson = JsonConvert.SerializeObject(gameData, Formatting.Indented);
                File.WriteAllText(_jsonFilePath, updatedJson);
            }
            catch (IOException ex)
            {
                throw new Exception($"An error occurred while saving the game data: {ex.Message}");
            }
        }

        public Game? GetGame(string title) => LoadGameData().Games?.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        public Profile? GetProfile(string profileTitle, string gameTitle)
        {
            var game = GetGame(gameTitle);
            return game?.Profiles?.FirstOrDefault(p => p.Title.Equals(profileTitle, StringComparison.OrdinalIgnoreCase));
        }

        private bool HasDuplicateProfiles(List<Profile> profiles) =>
    profiles.GroupBy(p => p.Title, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1);


        public void AddGameData(string title, List<Profile>? profiles = null)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null)
            {
                gameData.Games = new List<Game>();
            }

            if (gameData.Games.Any(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogError($"A game with the title '{title}' already exists.");
                return;
            }

            if (profiles != null && HasDuplicateProfiles(profiles))
            {
               _logger.LogError("Duplicate profile titles found. Please ensure each profile has a unique title.");
                return;
            }

            gameData.Games.Add(new Game { Title = title, Profiles = profiles ?? new List<Profile>() });
            SaveGameData(gameData);
            _logger.LogInfo($"Game '{title}' added successfully.");
        }

        public void AddProfile(string title, string gameTitle, List<ConfigFile> configFiles)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null)
            {
                _logger.LogError("No games found.");
                return;
            }

            var game = gameData.Games?.FirstOrDefault(g => g.Title.Equals(gameTitle, StringComparison.OrdinalIgnoreCase));

            if (game == null)
            {
                _logger.LogError($"Game with the title '{gameTitle}' not found.");
                return;
            }

            if (game.Profiles.Any(p => p.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
            {
                 _logger.LogError($"Profile with title '{title}' already exists for game '{gameTitle}'.");
                return;
            }

            game.Profiles.Add(new Profile { Title = title, ConfigFiles = configFiles });
            SaveGameData(gameData);
            _logger.LogInfo($"Profile '{title}' added to game '{gameTitle}' successfully.");
                    
        }

        public void DeleteGameData(string title)
        {
            var gameData = LoadGameData();

            var gameToRemove = GetGame(title);

            if (gameToRemove == null)
            {
                _logger.LogError($"Game with the title '{title}' not found.");
                return;
            }

            gameData.Games.Remove(gameToRemove);
            SaveGameData(gameData);
            _logger.LogInfo($"Game '{title}' has been deleted successfully.");
        }


        public void DeleteProfile(string title, string gameTitle)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null)
            {
                _logger.LogError("No games to delete a profile from.");
                return;
            }

            var game = gameData.Games?.FirstOrDefault(g => g.Title.Equals(gameTitle, StringComparison.OrdinalIgnoreCase));
            
            if (game == null || game.Profiles == null)
            {
                _logger.LogError($"Game with the title '{gameTitle}' not found, or game has no profiles to delete.");
                return;
            }

            var profileToRemove = game.Profiles.FirstOrDefault(p => p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (profileToRemove == null)
            {
               _logger.LogError($"Profile with the title '{title}' not found.");
                return;
            }

            game.Profiles.Remove(profileToRemove);
            SaveGameData(gameData);
            _logger.LogInfo($"Profile '{title}' has been deleted successfully.");
        }

        public void EditGameData(string oldTitle, string newTitle)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null || gameData.Games.Count == 0)
            {
                 _logger.LogError("No games to edit.");
                return;
            }

            var gameToEdit = gameData.Games?.FirstOrDefault(g => g.Title.Equals(oldTitle, StringComparison.OrdinalIgnoreCase));

            if (gameToEdit == null)
            {
                 _logger.LogError($"Game with the title '{oldTitle}' not found.");
                return;
            }

            if (GetGame(newTitle) != null)
            {
                _logger.LogError($"A game with the title '{newTitle}' already exists.");
                return;
            }

            gameToEdit.Title = newTitle;
            SaveGameData(gameData);
            _logger.LogInfo($"Game '{oldTitle}' has been updated successfully.");
        }

        public void EditProfile(string oldTitle, string gameTitle, Profile profile)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null)
            {
                _logger.LogError("No games found.");
                return;
            }

            var game = gameData.Games?.FirstOrDefault(g => g.Title.Equals(gameTitle, StringComparison.OrdinalIgnoreCase));

            if (game == null || game.Profiles == null)
            {
                _logger.LogError($"Game with the title '{gameTitle}' not found.");
                return;
            }

            var profileToEdit = game.Profiles.FirstOrDefault(p => p.Title.Equals(oldTitle, StringComparison.OrdinalIgnoreCase));

            if (profileToEdit == null)
            {
                _logger.LogError($"Profile with the title '{oldTitle}' not found.");
                return;
            }

            if (game.Profiles.Any(p => p.Title.Equals(profile.Title, StringComparison.OrdinalIgnoreCase) && !p.Title.Equals(oldTitle, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogError($"A profile with the title '{profile.Title}' already exists.");
                return;
            }

            profileToEdit.Title = profile.Title ?? profileToEdit.Title;
            profileToEdit.ConfigFiles = profile.ConfigFiles ?? profileToEdit.ConfigFiles;

            SaveGameData(gameData);
            _logger.LogInfo($"Profile '{oldTitle}' has been updated successfully.");
        }
    }
}
