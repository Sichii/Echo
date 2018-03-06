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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.launchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.small = new System.Windows.Forms.ToolStripMenuItem();
            this.large = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.allWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.primaryMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thumbTbl = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchToolStripMenuItem,
            this.windowSizeToolStripMenuItem,
            this.allWindowsToolStripMenuItem,
            this.cascadeToolStripMenuItem,
            this.primaryMonitorToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(599, 23);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // launchToolStripMenuItem
            // 
            this.launchToolStripMenuItem.Name = "launchToolStripMenuItem";
            this.launchToolStripMenuItem.Size = new System.Drawing.Size(58, 19);
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
            this.windowSizeToolStripMenuItem.Size = new System.Drawing.Size(84, 19);
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
            this.allWindowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleHideToolStripMenuItem,
            this.smallToolStripMenuItem,
            this.largeToolStripMenuItem});
            this.allWindowsToolStripMenuItem.Name = "allWindowsToolStripMenuItem";
            this.allWindowsToolStripMenuItem.Size = new System.Drawing.Size(85, 19);
            this.allWindowsToolStripMenuItem.Text = "All Windows";
            // 
            // toggleHideToolStripMenuItem
            // 
            this.toggleHideToolStripMenuItem.Name = "toggleHideToolStripMenuItem";
            this.toggleHideToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.toggleHideToolStripMenuItem.Text = "Toggle Hide";
            this.toggleHideToolStripMenuItem.Click += new System.EventHandler(this.toggleHideToolStripMenuItem_Click);
            // 
            // smallToolStripMenuItem
            // 
            this.smallToolStripMenuItem.Name = "smallToolStripMenuItem";
            this.smallToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.smallToolStripMenuItem.Text = "Small";
            this.smallToolStripMenuItem.Click += new System.EventHandler(this.smallToolStripMenuItem_Click);
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            this.largeToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.largeToolStripMenuItem.Text = "Large";
            this.largeToolStripMenuItem.Click += new System.EventHandler(this.largeToolStripMenuItem_Click);
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(63, 19);
            this.cascadeToolStripMenuItem.Text = "Cascade";
            // 
            // primaryMonitorToolStripMenuItem
            // 
            this.primaryMonitorToolStripMenuItem.Name = "primaryMonitorToolStripMenuItem";
            this.primaryMonitorToolStripMenuItem.Size = new System.Drawing.Size(106, 19);
            this.primaryMonitorToolStripMenuItem.Text = "Primary Monitor";
            // 
            // thumbTbl
            // 
            this.thumbTbl.AutoSize = true;
            this.thumbTbl.BackColor = System.Drawing.Color.Transparent;
            this.thumbTbl.ColumnCount = 3;
            this.thumbTbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.thumbTbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.thumbTbl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.thumbTbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbTbl.Location = new System.Drawing.Point(0, 23);
            this.thumbTbl.Name = "thumbTbl";
            this.thumbTbl.RowCount = 3;
            this.thumbTbl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.thumbTbl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.thumbTbl.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.thumbTbl.Size = new System.Drawing.Size(599, 173);
            this.thumbTbl.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(599, 196);
            this.Controls.Add(this.thumbTbl);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(615, 235);
            this.Name = "MainForm";
            this.Text = "DA Windower";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem launchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem small;
        private System.Windows.Forms.ToolStripMenuItem large;
        private System.Windows.Forms.ToolStripMenuItem fullscreen;
        private System.Windows.Forms.ToolStripMenuItem allWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem primaryMonitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleHideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel thumbTbl;
    }
}

