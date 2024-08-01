using GCS.Core;

namespace GCS.UI
{
    public partial class Form1 : Form
    {
        private GameDataManager gameDataManager;
        private GameData gameData;
        private Dictionary<Label, TextBox> labelTextBoxMap = new Dictionary<Label, TextBox>();

        public Form1()
        {
            InitializeComponent();
            string jsonFilePath = "gameData.json";
            gameDataManager = new GameDataManager(jsonFilePath); //initalise the constructor
            PopulateGamePicker();
        }

        private void PopulateGamePicker()
        {
            try
            {
                gameData = gameDataManager.LoadGameData();
                gamePicker.Items.Clear();
                if (gameData != null && gameData.Games != null)
                {
                    foreach (var game in gameData.Games)
                    {
                        gamePicker.Items.Add(game.Title);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred while loading the Game Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gamePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear existing profile controls
            profilesPanel.Controls.Clear();

            // Get the selected game title
            string selectedGameTitle = gamePicker.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedGameTitle))
            {
                // Find the game with the selected title
                var game = gameData.Games.FirstOrDefault(g => g.Title == selectedGameTitle);

                if (game != null && game.configPath != null && game.Profiles != null)
                {
                    // Update text box with config path
                    gameConfigPath.Text = game.configPath;

                    // Create controls for each profile
                    foreach (var profile in game.Profiles)
                    {
                        // Create and configure the label for the profile
                        Label profileLabel = new Label
                        {
                            Text = profile.title,
                            AutoSize = true,
                            Width = 253,
                            Padding = new Padding(0, 10, 0, 0) // Add some space below the label
                        };

                        // Create and configure the text box for the profile path
                        TextBox profileTextBox = new TextBox
                        {
                            Text = profile.profilePath,
                            ReadOnly = true,
                            Width = 253
                        };

                        // Create a panel for buttons to keep them next to each other
                        FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                        {
                            AutoSize = true,
                            FlowDirection = FlowDirection.LeftToRight,
                            WrapContents = false,
                            Padding = new Padding(0)
                        };

                        // Create and configure the browse button
                        Button browseButton = new Button
                        {
                            Text = "Browse",
                            Width = 75
                        };
                        browseButton.Click += (s, args) =>
                        {
                            using (OpenFileDialog openFileDialog = new OpenFileDialog())
                            {
                                if (openFileDialog.ShowDialog() == DialogResult.OK)
                                {
                                    profileTextBox.Text = openFileDialog.FileName;
                                }
                            }
                        };

                        // Create and configure the use button
                        Button useButton = new Button
                        {
                            Text = "Use",
                            Width = 75
                        };
                        useButton.Click += (s, args) =>
                        {
                            bool copyFile = FileHelper.profileCopier(profileTextBox.Text, gameConfigPath.Text);
                            if (copyFile)
                            {
                                MessageBox.Show("File copied successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("An error occurred or invalid paths provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };

                        // Add buttons to buttonPanel
                        buttonPanel.Controls.Add(browseButton);
                        buttonPanel.Controls.Add(useButton);

                        // Add profile controls to panel
                        profilesPanel.Controls.Add(profileLabel);
                        profilesPanel.Controls.Add(profileTextBox);
                        profilesPanel.Controls.Add(buttonPanel);

                        labelTextBoxMap[profileLabel] = profileTextBox;
                    }

                    // Make the config path section visible
                    gameConfigPath.Visible = true;
                    gameConfigButton.Visible = true;
                    ConfigPathLabel.Visible = true;
                    saveButton.Visible = true;
                    profileLabel.Visible = true;
                }
            }
        }
        private void gameConfigButton_Click(object sender, EventArgs e)
        {
            TextBox textBox = gameConfigPath;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = openFileDialog.FileName;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (gameData == null || gameData.Games == null)
            {
                MessageBox.Show("No game data available to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedGameTitle = gamePicker.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedGameTitle))
            {
                MessageBox.Show("No game selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var game = gameData.Games.FirstOrDefault(g => g.Title == selectedGameTitle);

            if (game != null && game.Profiles != null)
            {
                game.configPath = gameConfigPath.Text;

                foreach (var kvp in labelTextBoxMap)
                {
                    var profileLabel = kvp.Key;
                    var profileTextBox = kvp.Value;

                    // Find the corresponding profile by its label text
                    var profile = game.Profiles.FirstOrDefault(p => p.title == profileLabel.Text);
                    if (profile != null)
                    {
                        // Update the profilePath
                        profile.profilePath = profileTextBox.Text;
                    }

                }

                try
                {
                    gameDataManager.SaveGameData(gameData);
                    MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The selected game could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addGameButton_Click(object sender, EventArgs e)
        {
            using (var addGameForm = new AddGameForm(gameDataManager))
            {
                addGameForm.ShowDialog();
                // Optionally, refresh the games list or combo box after adding a new game
                PopulateGamePicker();
            }
        }
    }
}
