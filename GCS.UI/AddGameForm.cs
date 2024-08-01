using GCS.Core;

namespace GCS.UI
{
    public partial class AddGameForm : Form
    {
        private GameDataManager _gameDataManager;
        private Dictionary<TextBox, TextBox> textBoxMap = new Dictionary<TextBox, TextBox>();

        public AddGameForm(GameDataManager gameDataManager)
        {
            InitializeComponent();
            _gameDataManager = gameDataManager;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string title = titleTxtBox.Text.Trim();
            string configPath = ConfigPathTxtBox.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(configPath))
            {
                MessageBox.Show("Game Title and Config Path are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var profiles = new List<Profile>();
            bool hasValidProfile = false;

            // Iterate over all profile panels
            foreach (Control control in profilesPanel.Controls)
            {
                if (control is Panel profilePanel)
                {
                    var titleTextBox = profilePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name.StartsWith("ProfileTitle"));
                    var pathTextBox = profilePanel.Controls.OfType<TextBox>().FirstOrDefault(tb => tb.Name.StartsWith("ProfilePath"));

                    if (titleTextBox != null && pathTextBox != null)
                    {
                        string profileTitle = titleTextBox.Text.Trim();
                        string profilePath = pathTextBox.Text.Trim();

                        if (!string.IsNullOrEmpty(profileTitle) && !string.IsNullOrEmpty(profilePath))
                        {
                            profiles.Add(new Profile
                            {
                                title = profileTitle,
                                profilePath = profilePath
                            });
                            hasValidProfile = true; // A valid profile has been found
                        }
                    }
                }
            }

            // Check if there is at least one valid profile
            if (!hasValidProfile)
            {
                MessageBox.Show("At least one profile must be provided.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Use GameDataManager to add the new game data with profiles
            _gameDataManager.AddGameData(title, configPath, profiles);

            MessageBox.Show("Game added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }


        private void configPathButton_Click(object sender, EventArgs e)
        {
            TextBox textBox = ConfigPathTxtBox;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = openFileDialog.FileName;
                }
            }
        }

        private void AddProfileControls()
        {
            // Create a new panel for the profile
            Panel profilePanel = new Panel();
            profilePanel.Width = profilesPanel.Width - 20; // Adjust the width
            profilePanel.Height = 100;
            profilePanel.Margin = new Padding(5);

            // Create the title label and textbox
            Label titleLabel = new Label();
            titleLabel.Text = "Profile Title";
            titleLabel.Location = new Point(0, 0);
            profilePanel.Controls.Add(titleLabel);

            TextBox profileTitleTextBox = new TextBox();
            profileTitleTextBox.Name = "ProfileTitle" + profilesPanel.Controls.Count; // Unique name
            profileTitleTextBox.Location = new Point(0, 20);
            profilePanel.Controls.Add(profileTitleTextBox);

            // Create the path label and textbox
            Label pathLabel = new Label();
            pathLabel.Text = "Profile Path";
            pathLabel.Location = new Point(0, 50);
            profilePanel.Controls.Add(pathLabel);

            TextBox profilePathTextBox = new TextBox();
            profilePathTextBox.Name = "ProfilePath" + profilesPanel.Controls.Count; // Unique name
            profilePathTextBox.ReadOnly = true;
            profilePathTextBox.Location = new Point(0, 70);
            profilePanel.Controls.Add(profilePathTextBox);

            // Create the browse button
            Button browseButton = new Button();
            browseButton.Text = "Browse";
            browseButton.Location = new Point(105, 70);
            browseButton.Click += (s, e) =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        profilePathTextBox.Text = openFileDialog.FileName;
                    }
                }
            };
            profilePanel.Controls.Add(browseButton);

            // Add the profilePanel to the profilesPanel
            profilesPanel.Controls.Add(profilePanel);

            textBoxMap[profileTitleTextBox] = profilePathTextBox;
        }

        private void addProfileButton_Click(object sender, EventArgs e)
        {
            AddProfileControls();
        }
    }
}
