﻿namespace DAWindower.Forms
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
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.BrowseDADirectoryButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // darkAgesPath
            // 
            this.darkAgesPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkAgesPath.Location = new System.Drawing.Point(12, 13);
            this.darkAgesPath.Name = "darkAgesPath";
            this.darkAgesPath.Size = new System.Drawing.Size(220, 20);
            this.darkAgesPath.TabIndex = 0;
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveSettingsButton.Location = new System.Drawing.Point(312, 9);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(68, 27);
            this.SaveSettingsButton.TabIndex = 2;
            this.SaveSettingsButton.Text = "Save";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // BrowseDADirectoryButton
            // 
            this.BrowseDADirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseDADirectoryButton.AutoSize = true;
            this.BrowseDADirectoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseDADirectoryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseDADirectoryButton.Location = new System.Drawing.Point(238, 9);
            this.BrowseDADirectoryButton.Name = "BrowseDADirectoryButton";
            this.BrowseDADirectoryButton.Size = new System.Drawing.Size(68, 27);
            this.BrowseDADirectoryButton.TabIndex = 1;
            this.BrowseDADirectoryButton.Text = "Browse";
            this.BrowseDADirectoryButton.UseVisualStyleBackColor = true;
            this.BrowseDADirectoryButton.Click += new System.EventHandler(this.BrowseDADirectoryButton_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(393, 45);
            this.Controls.Add(this.SaveSettingsButton);
            this.Controls.Add(this.BrowseDADirectoryButton);
            this.Controls.Add(this.darkAgesPath);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "OptionsForm";
            this.Text = "OptionsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox darkAgesPath;
        private System.Windows.Forms.Button SaveSettingsButton;
        private System.Windows.Forms.Button BrowseDADirectoryButton;
    }
}