namespace GCS.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            gamePicker = new ComboBox();
            gameConfigPath = new TextBox();
            gameConfigButton = new Button();
            saveButton = new Button();
            addGameButton = new Button();
            gamePickerLabel = new Label();
            ConfigPathLabel = new Label();
            profileLabel = new Label();
            profilesPanel = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // gamePicker
            // 
            gamePicker.FormattingEnabled = true;
            gamePicker.Location = new Point(12, 24);
            gamePicker.Name = "gamePicker";
            gamePicker.Size = new Size(121, 23);
            gamePicker.TabIndex = 0;
            gamePicker.SelectedIndexChanged += gamePicker_SelectedIndexChanged;
            // 
            // gameConfigPath
            // 
            gameConfigPath.Location = new Point(12, 77);
            gameConfigPath.Name = "gameConfigPath";
            gameConfigPath.ReadOnly = true;
            gameConfigPath.Size = new Size(253, 23);
            gameConfigPath.TabIndex = 1;
            gameConfigPath.Visible = false;
            // 
            // gameConfigButton
            // 
            gameConfigButton.Location = new Point(271, 77);
            gameConfigButton.Name = "gameConfigButton";
            gameConfigButton.Size = new Size(75, 23);
            gameConfigButton.TabIndex = 4;
            gameConfigButton.Text = "Browse";
            gameConfigButton.UseVisualStyleBackColor = true;
            gameConfigButton.Visible = false;
            gameConfigButton.Click += gameConfigButton_Click;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(139, 24);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 9;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Visible = false;
            saveButton.Click += saveButton_Click;
            // 
            // addGameButton
            // 
            addGameButton.Location = new Point(220, 24);
            addGameButton.Name = "addGameButton";
            addGameButton.Size = new Size(75, 23);
            addGameButton.TabIndex = 10;
            addGameButton.Text = "Add Game";
            addGameButton.UseVisualStyleBackColor = true;
            addGameButton.Click += addGameButton_Click;
            // 
            // gamePickerLabel
            // 
            gamePickerLabel.AutoSize = true;
            gamePickerLabel.Location = new Point(12, 6);
            gamePickerLabel.Name = "gamePickerLabel";
            gamePickerLabel.Size = new Size(41, 15);
            gamePickerLabel.TabIndex = 11;
            gamePickerLabel.Text = "Game:";
            // 
            // ConfigPathLabel
            // 
            ConfigPathLabel.AutoSize = true;
            ConfigPathLabel.Location = new Point(12, 59);
            ConfigPathLabel.Name = "ConfigPathLabel";
            ConfigPathLabel.Size = new Size(104, 15);
            ConfigPathLabel.TabIndex = 12;
            ConfigPathLabel.Text = "Game Config Path";
            ConfigPathLabel.Visible = false;
            // 
            // profileLabel
            // 
            profileLabel.AutoSize = true;
            profileLabel.Location = new Point(15, 116);
            profileLabel.Name = "profileLabel";
            profileLabel.Size = new Size(46, 15);
            profileLabel.TabIndex = 13;
            profileLabel.Text = "Profiles";
            profileLabel.Visible = false;
            // 
            // profilesPanel
            // 
            profilesPanel.AutoScroll = true;
            profilesPanel.FlowDirection = FlowDirection.TopDown;
            profilesPanel.Location = new Point(15, 134);
            profilesPanel.Name = "profilesPanel";
            profilesPanel.Size = new Size(331, 304);
            profilesPanel.TabIndex = 15;
            profilesPanel.WrapContents = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 450);
            Controls.Add(profilesPanel);
            Controls.Add(profileLabel);
            Controls.Add(ConfigPathLabel);
            Controls.Add(gamePickerLabel);
            Controls.Add(addGameButton);
            Controls.Add(saveButton);
            Controls.Add(gameConfigButton);
            Controls.Add(gameConfigPath);
            Controls.Add(gamePicker);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox gamePicker;
        private TextBox gameConfigPath;
        private TextBox profile1;
        private TextBox profile2;
        private Button gameConfigButton;
        private Button profile1Button;
        private Button profile2Button;
        private Button profile1UseButton;
        private Button profile2UseButton;
        private Button saveButton;
        private Button addGameButton;
        private Label gamePickerLabel;
        private Label ConfigPathLabel;
        private Label profileLabel;
        private Label profile2Label;
        private FlowLayoutPanel profilesPanel;
    }
}
