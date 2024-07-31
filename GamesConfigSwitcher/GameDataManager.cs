using GamesConfigSwitcher;
using Newtonsoft.Json;

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
}
