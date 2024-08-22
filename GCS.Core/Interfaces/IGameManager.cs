namespace GCS.Core
{
    public interface IGameDataManager
    {
        GameData LoadGameData();
        void SaveGameData(GameData gameData);
        Game? GetGame(string title);
        Profile? GetProfile(string title, string gameTitle);
        void AddGameData(string title, List<Profile>? profiles = null);
        void AddProfile(string title, string gameTitle, Profile profile);
        void DeleteGameData(string title);
        void DeleteProfile(string title, string gameTitle);
        void EditGameData(string oldTitle, string newTitle);
        void EditProfile(string oldTitle, string gameTitle, Profile profile);
    }
}
