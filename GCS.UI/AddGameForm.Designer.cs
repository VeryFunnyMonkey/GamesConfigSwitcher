namespace GCS.UI
{
    partial class AddGameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            titleTxtBox = new TextBox();
            ConfigPathTxtBox = new TextBox();
            profile1TxtBox = new TextBox();
            profile2TxtBox = new TextBox();
            saveButton = new Button();
            titleLabel = new Label();
            configPathLabel = new Label();
            profile1Label = new Label();
            profile2Label = new Label();
            configPathButton = new Button();
            profile1Button = new Button();
            profile2Button = new Button();
            SuspendLayout();
            // 
            // titleTxtBox
            // 
            titleTxtBox.Location = new Point(19, 25);
            titleTxtBox.Name = "titleTxtBox";
            titleTxtBox.Size = new Size(100, 23);
            titleTxtBox.TabIndex = 0;
            // 
            // ConfigPathTxtBox
            // 
            ConfigPathTxtBox.Location = new Point(19, 80);
            ConfigPathTxtBox.Name = "ConfigPathTxtBox";
            ConfigPathTxtBox.ReadOnly = true;
            ConfigPathTxtBox.Size = new Size(100, 23);
            ConfigPathTxtBox.TabIndex = 1;
            // 
            // profile1TxtBox
            // 
            profile1TxtBox.Location = new Point(19, 148);
            profile1TxtBox.Name = "profile1TxtBox";
            profile1TxtBox.ReadOnly = true;
            profile1TxtBox.Size = new Size(100, 23);
            profile1TxtBox.TabIndex = 2;
            // 
            // profile2TxtBox
            // 
            profile2TxtBox.Location = new Point(19, 207);
            profile2TxtBox.Name = "profile2TxtBox";
            profile2TxtBox.ReadOnly = true;
            profile2TxtBox.Size = new Size(100, 23);
            profile2TxtBox.TabIndex = 3;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(35, 249);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 4;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(35, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(63, 15);
            titleLabel.TabIndex = 5;
            titleLabel.Text = "Game Title";
            // 
            // configPathLabel
            // 
            configPathLabel.AutoSize = true;
            configPathLabel.Location = new Point(15, 62);
            configPathLabel.Name = "configPathLabel";
            configPathLabel.Size = new Size(104, 15);
            configPathLabel.TabIndex = 6;
            configPathLabel.Text = "Game Config Path";
            // 
            // profile1Label
            // 
            profile1Label.AutoSize = true;
            profile1Label.Location = new Point(44, 130);
            profile1Label.Name = "profile1Label";
            profile1Label.Size = new Size(50, 15);
            profile1Label.TabIndex = 7;
            profile1Label.Text = "Profile 1";
            // 
            // profile2Label
            // 
            profile2Label.AutoSize = true;
            profile2Label.Location = new Point(44, 189);
            profile2Label.Name = "profile2Label";
            profile2Label.Size = new Size(50, 15);
            profile2Label.TabIndex = 8;
            profile2Label.Text = "Profile 2";
            // 
            // configPathButton
            // 
            configPathButton.Location = new Point(125, 80);
            configPathButton.Name = "configPathButton";
            configPathButton.Size = new Size(75, 23);
            configPathButton.TabIndex = 9;
            configPathButton.Text = "Browse";
            configPathButton.UseVisualStyleBackColor = true;
            configPathButton.Click += configPathButton_Click;
            // 
            // profile1Button
            // 
            profile1Button.Location = new Point(125, 148);
            profile1Button.Name = "profile1Button";
            profile1Button.Size = new Size(75, 23);
            profile1Button.TabIndex = 10;
            profile1Button.Text = "Browse";
            profile1Button.UseVisualStyleBackColor = true;
            profile1Button.Click += profileButton_Click;
            // 
            // profile2Button
            // 
            profile2Button.Location = new Point(125, 206);
            profile2Button.Name = "profile2Button";
            profile2Button.Size = new Size(75, 23);
            profile2Button.TabIndex = 11;
            profile2Button.Text = "Browse";
            profile2Button.UseVisualStyleBackColor = true;
            profile2Button.Click += profileButton_Click;
            // 
            // AddGameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(profile2Button);
            Controls.Add(profile1Button);
            Controls.Add(configPathButton);
            Controls.Add(profile2Label);
            Controls.Add(profile1Label);
            Controls.Add(configPathLabel);
            Controls.Add(titleLabel);
            Controls.Add(saveButton);
            Controls.Add(profile2TxtBox);
            Controls.Add(profile1TxtBox);
            Controls.Add(ConfigPathTxtBox);
            Controls.Add(titleTxtBox);
            Name = "AddGameForm";
            Text = "AddGameForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox titleTxtBox;
        private TextBox ConfigPathTxtBox;
        private TextBox profile1TxtBox;
        private TextBox profile2TxtBox;
        private Button saveButton;
        private Label titleLabel;
        private Label configPathLabel;
        private Label profile1Label;
        private Label profile2Label;
        private Button configPathButton;
        private Button profile1Button;
        private Button profile2Button;
    }
}