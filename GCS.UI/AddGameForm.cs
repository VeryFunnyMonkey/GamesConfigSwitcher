using GCS.Core;

namespace GCS.UI
{
    public partial class AddGameForm : Form
    {
        private GameDataManager _gameDataManager;
        public AddGameForm(GameDataManager gameDataManager)
        {
            InitializeComponent();
            _gameDataManager = gameDataManager;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string title = titleTxtBox.Text.Trim();
            string configPath = ConfigPathTxtBox.Text.Trim();
            string profile1Path = profile1TxtBox.Text.Trim();
            string profile2Path = profile2TxtBox.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(profile1Path) || string.IsNullOrEmpty(profile2Path))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Use GameDataManager to add the new game data
            _gameDataManager.AddGameData(title, configPath, profile1Path, profile2Path);

            // Optionally, display a success message
            MessageBox.Show("Game added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Close the form after adding the game
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
        private void profileButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            TextBox textBox = null;

            // Determine which button was clicked and set the corresponding TextBox
            if (button == profile1Button)
                textBox = profile1TxtBox;
            else if (button == profile2Button)
                textBox = profile2TxtBox;

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
    }
}
