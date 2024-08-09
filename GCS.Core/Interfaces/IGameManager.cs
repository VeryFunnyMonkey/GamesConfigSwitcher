namespace GCS.Core
{
    public interface IGameDataManager
    {
        GameData LoadGameData();
        void SaveGameData(GameData gameData);
        void AddGameData(string title, string configPath, List<Profile> profiles);
        void DeleteGameData(string title);
    }
}
