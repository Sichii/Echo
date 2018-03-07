namespace DAWindower.Forms
{
    partial class OptionsForm
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
            this.darkAgesPath = new System.Windows.Forms.TextBox();
            this.BrowseDADirectoryButton = new System.Windows.Forms.Button();
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // darkAgesPath
            // 
            this.darkAgesPath.Location = new System.Drawing.Point(12, 9);
            this.darkAgesPath.Name = "darkAgesPath";
            this.darkAgesPath.Size = new System.Drawing.Size(220, 20);
            this.darkAgesPath.TabIndex = 0;
            // 
            // BrowseDADirectoryButton
            // 
            this.BrowseDADirectoryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseDADirectoryButton.Location = new System.Drawing.Point(238, 9);
            this.BrowseDADirectoryButton.Name = "BrowseDADirectoryButton";
            this.BrowseDADirectoryButton.Size = new System.Drawing.Size(68, 27);
            this.BrowseDADirectoryButton.TabIndex = 1;
            this.BrowseDADirectoryButton.Text = "Browse";
            this.BrowseDADirectoryButton.UseVisualStyleBackColor = true;
            this.BrowseDADirectoryButton.Click += new System.EventHandler(this.BrowseDADirectoryButton_Click);
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSettingsButton.Location = new System.Drawing.Point(238, 42);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(68, 27);
            this.SaveSettingsButton.TabIndex = 2;
            this.SaveSettingsButton.Text = "Save";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 75);
            this.Controls.Add(this.SaveSettingsButton);
            this.Controls.Add(this.BrowseDADirectoryButton);
            this.Controls.Add(this.darkAgesPath);
            this.Name = "OptionsForm";
            this.Text = "OptionsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox darkAgesPath;
        private System.Windows.Forms.Button BrowseDADirectoryButton;
        private System.Windows.Forms.Button SaveSettingsButton;
    }
}