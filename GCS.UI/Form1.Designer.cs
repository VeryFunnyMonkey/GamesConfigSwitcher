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
            profile1 = new TextBox();
            profile2 = new TextBox();
            gameConfigButton = new Button();
            profile1Button = new Button();
            profile2Button = new Button();
            profile1UseButton = new Button();
            profile2UseButton = new Button();
            saveButton = new Button();
            addGameButton = new Button();
            gamePickerLabel = new Label();
            ConfigPathLabel = new Label();
            profile1Label = new Label();
            profile2Label = new Label();
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
            // profile1
            // 
            profile1.Location = new Point(12, 135);
            profile1.Name = "profile1";
            profile1.ReadOnly = true;
            profile1.Size = new Size(253, 23);
            profile1.TabIndex = 2;
            profile1.Visible = false;
            // 
            // profile2
            // 
            profile2.Location = new Point(12, 220);
            profile2.Name = "profile2";
            profile2.ReadOnly = true;
            profile2.Size = new Size(253, 23);
            profile2.TabIndex = 3;
            profile2.Visible = false;
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
            // profile1Button
            // 
            profile1Button.Location = new Point(271, 135);
            profile1Button.Name = "profile1Button";
            profile1Button.Size = new Size(75, 23);
            profile1Button.TabIndex = 5;
            profile1Button.Text = "Browse";
            profile1Button.UseVisualStyleBackColor = true;
            profile1Button.Visible = false;
            profile1Button.Click += profileButton_Click;
            // 
            // profile2Button
            // 
            profile2Button.Location = new Point(271, 220);
            profile2Button.Name = "profile2Button";
            profile2Button.Size = new Size(75, 23);
            profile2Button.TabIndex = 6;
            profile2Button.Text = "Browse";
            profile2Button.UseVisualStyleBackColor = true;
            profile2Button.Visible = false;
            profile2Button.Click += profileButton_Click;
            // 
            // profile1UseButton
            // 
            profile1UseButton.Location = new Point(12, 163);
            profile1UseButton.Name = "profile1UseButton";
            profile1UseButton.Size = new Size(75, 23);
            profile1UseButton.TabIndex = 7;
            profile1UseButton.Text = "Use";
            profile1UseButton.UseVisualStyleBackColor = true;
            profile1UseButton.Visible = false;
            profile1UseButton.Click += profileUseButton_Click;
            // 
            // profile2UseButton
            // 
            profile2UseButton.Location = new Point(12, 249);
            profile2UseButton.Name = "profile2UseButton";
            profile2UseButton.Size = new Size(75, 23);
            profile2UseButton.TabIndex = 8;
            profile2UseButton.Text = "Use";
            profile2UseButton.UseVisualStyleBackColor = true;
            profile2UseButton.Visible = false;
            profile2UseButton.Click += profileUseButton_Click;
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
            ConfigPathLabel.Location = new Point(15, 59);
            ConfigPathLabel.Name = "ConfigPathLabel";
            ConfigPathLabel.Size = new Size(104, 15);
            ConfigPathLabel.TabIndex = 12;
            ConfigPathLabel.Text = "Game Config Path";
            ConfigPathLabel.Visible = false;
            // 
            // profile1Label
            // 
            profile1Label.AutoSize = true;
            profile1Label.Location = new Point(17, 116);
            profile1Label.Name = "profile1Label";
            profile1Label.Size = new Size(50, 15);
            profile1Label.TabIndex = 13;
            profile1Label.Text = "Profile 1";
            profile1Label.Visible = false;
            // 
            // profile2Label
            // 
            profile2Label.AutoSize = true;
            profile2Label.Location = new Point(17, 202);
            profile2Label.Name = "profile2Label";
            profile2Label.Size = new Size(50, 15);
            profile2Label.TabIndex = 14;
            profile2Label.Text = "Profile 2";
            profile2Label.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(profile2Label);
            Controls.Add(profile1Label);
            Controls.Add(ConfigPathLabel);
            Controls.Add(gamePickerLabel);
            Controls.Add(addGameButton);
            Controls.Add(saveButton);
            Controls.Add(profile2UseButton);
            Controls.Add(profile1UseButton);
            Controls.Add(profile2Button);
            Controls.Add(profile1Button);
            Controls.Add(gameConfigButton);
            Controls.Add(profile2);
            Controls.Add(profile1);
            Controls.Add(gameConfigPath);
            Controls.Add(gamePicker);
            Name = "Form1";
            Text = "Games Config Switcher";
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
        private Label profile1Label;
        private Label profile2Label;
    }
}
