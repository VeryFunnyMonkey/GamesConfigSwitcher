using Newtonsoft.Json;

namespace GCS.Core
{
    public class GameDataManager(string jsonFilePath) : IGameDataManager
    {
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
                try
                {
                    GameData gameData = new GameData();
                    string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
                    File.WriteAllText(jsonFilePath, json);
                    return gameData;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while creating ${jsonFilePath}: {ex.Message}");
                }
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

        public Game? GetGame(string title)
        {
            var gameData = LoadGameData();
            
            return gameData.Games?.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public Profile? GetProfile(string title, string gameTitle)
        {
            var gameData = LoadGameData();
            
            var game = gameData.Games?.FirstOrDefault(g => g.Title.Equals(gameTitle, StringComparison.OrdinalIgnoreCase));

            return game?.Profiles?.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public void AddGameData(string title, List<Profile>? profiles = null)
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

            if (profiles != null)
            {
                var duplicateProfiles = profiles.GroupBy(p => p.Title, StringComparer.OrdinalIgnoreCase)
                                    .Where(g => g.Count() > 1)
                                    .Select(g => g.Key)
                                    .ToList();
                
                if (duplicateProfiles.Count != 0)
                {
                    Console.WriteLine($"Duplicate profile titles found: {string.Join(", ", duplicateProfiles)}. Please ensure each profile has a unique title.");
                    return;
                }
            }

            var newGame = new Game
            {
                Title = title,
                Profiles = profiles ?? new List<Profile>()
            };

            gameData.Games?.Add(newGame);
            SaveGameData(gameData);
            Console.WriteLine($"Game '{title}' added successfully.");
        }

        public void AddProfile(string title, string gameTitle, Profile profile)
        {
            var gameData = LoadGameData();

            if (gameData.Games != null)
            {
                var game = GetGame(gameTitle);
                if (game != null)
                {
                    if (game.Profiles != null || game.Profiles.Count > 0)
                    {
                        var duplicateProfile = game.Profiles.FirstOrDefault(p => p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

                        if (duplicateProfile != null)
                        {
                            Console.WriteLine($"Profile with title '{title}' already exists for game '{gameTitle}', please ensure each profile has a unique title.");
                            return;
                        }
                    }
                    game.Profiles.Add(profile);
                    SaveGameData(gameData);
                    Console.WriteLine($"Profile '{title}' added to game '{gameTitle}' successfully.");
                    
                }
                else
                {
                    Console.WriteLine($"Game with the title '{gameTitle}' not found.");
                }
            }
            else
            {
                Console.WriteLine("No games found.");
            }

        }

        public void DeleteGameData(string title)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null || gameData.Games.Count == 0)
            {
                Console.WriteLine("No games to delete.");
                return;
            }

            var gameToRemove = GetGame(title);

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


        public void DeleteProfile(string title, string gameTitle)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null || gameData.Games.Count == 0)
            {
                Console.WriteLine("No games to delete a profile from.");
                return;
            }

            var gameToRemove = GetGame(gameTitle);

            if (gameToRemove?.Profiles == null || gameToRemove.Profiles.Count == 0)
            {
                Console.WriteLine("No profiles to delete.");
                return;
            }

            var profileToRemove = gameToRemove.Profiles.FirstOrDefault(p => p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (profileToRemove != null)
            {
                gameToRemove.Profiles.Remove(profileToRemove);
                SaveGameData(gameData);
                Console.WriteLine($"Profile '{title}' has been deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Profile with the title '{title}' not found.");
            }
        }

        public void EditGameData(string oldTitle, string newTitle)
        {
            var gameData = LoadGameData();

            if (gameData.Games == null || gameData.Games.Count == 0)
            {
                Console.WriteLine("No games to edit.");
                return;
            }

            var gameToEdit = GetGame(oldTitle);

            if (gameToEdit != null)
            {
                if (GetGame(newTitle) != null)
                {
                    Console.WriteLine($"A game with the title '{newTitle}' already exists.");
                    return;
                }

                gameToEdit.Title = newTitle;

                SaveGameData(gameData);
                Console.WriteLine($"Game '{oldTitle}' has been updated successfully.");
            }
            else
            {
                Console.WriteLine($"Game with the title '{oldTitle}' not found.");
            }
        }

        public void EditProfile(string oldTitle, string gameTitle, Profile profile)
        {
            var gameData = LoadGameData();

            if (gameData.Games != null)
            {
                var game = GetGame(gameTitle);
                if (game != null)
                {
                    if (game.Profiles != null || game.Profiles.Count > 0)
                    {
                        var profileToEdit = game.Profiles.FirstOrDefault(p => p.Title.Equals(oldTitle, StringComparison.OrdinalIgnoreCase));

                        if (profileToEdit != null)
                        {
                            if (game.Profiles.Any(p => p.Title.Equals(profile.Title, StringComparison.OrdinalIgnoreCase) && !p.Title.Equals(oldTitle, StringComparison.OrdinalIgnoreCase)))
                            {
                                Console.WriteLine($"A game with the title '{profile.Title}' already exists.");
                                return;
                            }

                            profileToEdit.Title = profile.Title ?? profileToEdit.Title;
                            profileToEdit.ConfigFiles = profile.ConfigFiles ?? profileToEdit.ConfigFiles;

                            SaveGameData(gameData);
                            Console.WriteLine($"Profile '{oldTitle}' has been updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Profile with the title '{oldTitle}' not found.");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No profiles to edit.");
                        return;
                    }
                    
                }
                else
                {
                    Console.WriteLine($"Game with the title '{gameTitle}' not found.");
                }
            }
            else
            {
                Console.WriteLine("No games found.");
            }

        }
    }
}
