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
            saveButton = new Button();
            titleLabel = new Label();
            configPathLabel = new Label();
            configPathButton = new Button();
            profilesPanel = new FlowLayoutPanel();
            addProfileButton = new Button();
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
            // saveButton
            // 
            saveButton.Location = new Point(125, 25);
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
            // profilesPanel
            // 
            profilesPanel.AutoScroll = true;
            profilesPanel.FlowDirection = FlowDirection.TopDown;
            profilesPanel.Location = new Point(19, 118);
            profilesPanel.Name = "profilesPanel";
            profilesPanel.Size = new Size(326, 320);
            profilesPanel.TabIndex = 10;
            profilesPanel.WrapContents = false;
            // 
            // addProfileButton
            // 
            addProfileButton.Location = new Point(270, 80);
            addProfileButton.Name = "addProfileButton";
            addProfileButton.Size = new Size(75, 23);
            addProfileButton.TabIndex = 11;
            addProfileButton.Text = "Add Profile";
            addProfileButton.UseVisualStyleBackColor = true;
            addProfileButton.Click += addProfileButton_Click;
            // 
            // AddGameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 450);
            Controls.Add(addProfileButton);
            Controls.Add(profilesPanel);
            Controls.Add(configPathButton);
            Controls.Add(configPathLabel);
            Controls.Add(titleLabel);
            Controls.Add(saveButton);
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
        private Button saveButton;
        private Label titleLabel;
        private Label configPathLabel;
        private Button configPathButton;
        private FlowLayoutPanel profilesPanel;
        private Button addProfileButton;
    }
}