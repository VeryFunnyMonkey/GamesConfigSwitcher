namespace GamesConfigSwitcher
{
    public class GameData
    {
        public List<Game> Games { get; set; }
    }

        public class Game
    {
        public string Title { get; set; }
        public string configPath { get; set; }
        public Profile Profiles { get; set; }
    }

    public class Profile
    {
        public string Profile1 { get; set; }
        public string Profile2 { get; set; }
    }
}
