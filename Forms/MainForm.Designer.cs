namespace DAWindower
{
    partial class MainForm
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
            this.thumbnailTbl = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.launchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.small = new System.Windows.Forms.ToolStripMenuItem();
            this.large = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.allWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.primaryMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // thumbnailTbl
            // 
            this.thumbnailTbl.AutoScroll = true;
            this.thumbnailTbl.AutoSize = true;
            this.thumbnailTbl.ColumnCount = 3;
            this.thumbnailTbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33233F));
            this.thumbnailTbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33233F));
            this.thumbnailTbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33533F));
            this.thumbnailTbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailTbl.Location = new System.Drawing.Point(0, 24);
            this.thumbnailTbl.Name = "thumbnailTbl";
            this.thumbnailTbl.RowCount = 2;
            this.thumbnailTbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.thumbnailTbl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.thumbnailTbl.Size = new System.Drawing.Size(575, 331);
            this.thumbnailTbl.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchToolStripMenuItem,
            this.windowSizeToolStripMenuItem,
            this.allWindowsToolStripMenuItem,
            this.cascadeToolStripMenuItem,
            this.primaryMonitorToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(575, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // launchToolStripMenuItem
            // 
            this.launchToolStripMenuItem.Name = "launchToolStripMenuItem";
            this.launchToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.launchToolStripMenuItem.Text = "Launch";
            this.launchToolStripMenuItem.Click += new System.EventHandler(this.LaunchDA);
            // 
            // windowSizeToolStripMenuItem
            // 
            this.windowSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.small,
            this.large,
            this.fullscreen});
            this.windowSizeToolStripMenuItem.Name = "windowSizeToolStripMenuItem";
            this.windowSizeToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.windowSizeToolStripMenuItem.Text = "Size Selector";
            // 
            // small
            // 
            this.small.BackColor = System.Drawing.Color.White;
            this.small.Checked = true;
            this.small.CheckState = System.Windows.Forms.CheckState.Checked;
            this.small.ForeColor = System.Drawing.Color.Black;
            this.small.Name = "small";
            this.small.Size = new System.Drawing.Size(187, 22);
            this.small.Text = "640x480 (Small)";
            this.small.Click += new System.EventHandler(this.DropDownCheck);
            // 
            // large
            // 
            this.large.BackColor = System.Drawing.Color.White;
            this.large.ForeColor = System.Drawing.Color.Black;
            this.large.Name = "large";
            this.large.Size = new System.Drawing.Size(187, 22);
            this.large.Text = "1280x960 (Large)";
            this.large.Click += new System.EventHandler(this.DropDownCheck);
            // 
            // fullscreen
            // 
            this.fullscreen.BackColor = System.Drawing.Color.White;
            this.fullscreen.ForeColor = System.Drawing.Color.Black;
            this.fullscreen.Name = "fullscreen";
            this.fullscreen.Size = new System.Drawing.Size(187, 22);
            this.fullscreen.Text = "Windowed Fullscreen";
            this.fullscreen.Click += new System.EventHandler(this.DropDownCheck);
            // 
            // allWindowsToolStripMenuItem
            // 
            this.allWindowsToolStripMenuItem.Name = "allWindowsToolStripMenuItem";
            this.allWindowsToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.allWindowsToolStripMenuItem.Text = "All Windows";
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.cascadeToolStripMenuItem.Text = "Cascade";
            // 
            // primaryMonitorToolStripMenuItem
            // 
            this.primaryMonitorToolStripMenuItem.Name = "primaryMonitorToolStripMenuItem";
            this.primaryMonitorToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.primaryMonitorToolStripMenuItem.Text = "Primary Monitor";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(575, 355);
            this.Controls.Add(this.thumbnailTbl);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.Color.Black;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "DA Windower";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel thumbnailTbl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem launchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem small;
        private System.Windows.Forms.ToolStripMenuItem large;
        private System.Windows.Forms.ToolStripMenuItem fullscreen;
        private System.Windows.Forms.ToolStripMenuItem allWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem primaryMonitorToolStripMenuItem;
    }
}

