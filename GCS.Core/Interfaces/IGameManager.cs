namespace GCS.Core
{
    public interface IGameDataManager
    {
        GameData LoadGameData();
        void SaveGameData(GameData gameData);
        void AddGameData(string title, List<Profile>? profiles = null);
        void AddProfile(string title, string gameTitle, Profile profile);
        void DeleteGameData(string title);
        void EditGameData(string oldTitle, string newTitle, List<Profile> newProfiles);
    }
}
