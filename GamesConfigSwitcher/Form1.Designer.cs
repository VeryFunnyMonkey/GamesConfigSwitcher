namespace GamesConfigSwitcher
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
            SuspendLayout();
            // 
            // gamePicker
            // 
            gamePicker.FormattingEnabled = true;
            gamePicker.Location = new Point(12, 12);
            gamePicker.Name = "gamePicker";
            gamePicker.Size = new Size(121, 23);
            gamePicker.TabIndex = 0;
            gamePicker.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // gameConfigPath
            // 
            gameConfigPath.Location = new Point(12, 62);
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
            profile2.Location = new Point(12, 209);
            profile2.Name = "profile2";
            profile2.ReadOnly = true;
            profile2.Size = new Size(253, 23);
            profile2.TabIndex = 3;
            profile2.Visible = false;
            // 
            // gameConfigButton
            // 
            gameConfigButton.Location = new Point(271, 62);
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
            profile2Button.Location = new Point(271, 209);
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
            profile2UseButton.Location = new Point(12, 238);
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
            saveButton.Location = new Point(139, 11);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 9;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Visible = false;
            saveButton.Click += saveButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}
