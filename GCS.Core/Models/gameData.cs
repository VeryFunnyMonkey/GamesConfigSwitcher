namespace GCS.Core
{
    public class GameData
    {
        public List<Game> Games { get; set; }
    }

        public class Game
    {
        public string Title { get; set; }
        public string configPath { get; set; }
        public List<Profile> Profiles { get; set; }
    }

    public class Profile
    {
        public string title { get; set; }
        public string profilePath { get; set; }
        public Dictionary<string, string> Variables { get; set; }
    }
}
