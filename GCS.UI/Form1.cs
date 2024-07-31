using GCS.Core;

namespace GCS.UI
{
    public partial class Form1 : Form
    {
        private GameDataManager gameDataManager;
        private GameData gameData;

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


        private void SaveChanges()
        {
            if (gameData == null || gameData.Games == null)
            {
                MessageBox.Show("No game data available to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the selected game title
            string selectedGameTitle = gamePicker.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedGameTitle))
            {
                MessageBox.Show("No game selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Find the game with the selected title
            var game = gameData.Games.FirstOrDefault(g => g.Title == selectedGameTitle);

            if (game != null && game.Profiles != null)
            {
                // Update the profiles with text box values
                game.configPath = gameConfigPath.Text;
                game.Profiles.Profile1 = profile1.Text;
                game.Profiles.Profile2 = profile2.Text;

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

        private void gamePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected game title
            string selectedGameTitle = gamePicker.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedGameTitle))
            {
                // Find the game with the selected title
                var game = gameData.Games.FirstOrDefault(g => g.Title == selectedGameTitle);

                if (game != null && game.configPath != null && game.Profiles != null)
                {
                    // Update text boxes with config paths and profile paths
                    gameConfigPath.Text = game.configPath;
                    profile1.Text = game.Profiles.Profile1;
                    profile2.Text = game.Profiles.Profile2;
                }
            }

            gameConfigPath.Visible = true;
            gameConfigButton.Visible = true;
            profile1.Visible = true;
            profile1Button.Visible = true;
            profile1UseButton.Visible = true;
            profile2.Visible = true;
            profile2Button.Visible = true;
            profile2UseButton.Visible = true;
            saveButton.Visible = true;
        }

        private void profileButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            TextBox textBox = null;

            // Determine which button was clicked and set the corresponding TextBox
            if (button == profile1Button)
                textBox = profile1;
            else if (button == profile2Button)
                textBox = profile2;

            if (textBox != null)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = openFileDialog.FileName;
                    }
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

        private void profileUseButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            TextBox textBox = null;

            // Determine which button was clicked and set the corresponding TextBox
            if (button == profile1UseButton)
                textBox = profile1;
            else if (button == profile2UseButton)
                textBox = profile2;

            if (textBox != null)
            {
                bool copyFile = FileHelper.profileCopier(textBox.Text, gameConfigPath.Text);

                if (copyFile)
                {
                    MessageBox.Show("File copied successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("An error occurred or invalid paths provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
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
