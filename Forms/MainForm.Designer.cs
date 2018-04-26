namespace Echo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.launchBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeSelector = new System.Windows.Forms.ToolStripMenuItem();
            this.small = new System.Windows.Forms.ToolStripMenuItem();
            this.large = new System.Windows.Forms.ToolStripMenuItem();
            this.large4k = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.allWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.allToggleHide = new System.Windows.Forms.ToolStripMenuItem();
            this.allSmall = new System.Windows.Forms.ToolStripMenuItem();
            this.allLarge = new System.Windows.Forms.ToolStripMenuItem();
            this.allLarge4k = new System.Windows.Forms.ToolStripMenuItem();
            this.cascade = new System.Windows.Forms.ToolStripMenuItem();
            this.allVisible = new System.Windows.Forms.ToolStripMenuItem();
            this.commander = new System.Windows.Forms.ToolStripMenuItem();
            this.placeholderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitors = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.thumbTbl = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchBtn,
            this.sizeSelector,
            this.allWindows,
            this.cascade,
            this.monitors,
            this.optionsBtn});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(599, 23);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // launchBtn
            // 
            this.launchBtn.Name = "launchBtn";
            this.launchBtn.Size = new System.Drawing.Size(58, 19);
            this.launchBtn.Text = "Launch";
            this.launchBtn.Click += new System.EventHandler(this.LaunchDA);
            // 
            // sizeSelector
            // 
            this.sizeSelector.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.small,
            this.large,
            this.large4k,
            this.fullscreen});
            this.sizeSelector.Name = "sizeSelector";
            this.sizeSelector.Size = new System.Drawing.Size(84, 19);
            this.sizeSelector.Text = "Size Selector";
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
            // large4k
            // 
            this.large4k.Name = "large4k";
            this.large4k.Size = new System.Drawing.Size(187, 22);
            this.large4k.Text = "2560x1920 (Large4k)";
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
            // allWindows
            // 
            this.allWindows.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToggleHide,
            this.allSmall,
            this.allLarge,
            this.allLarge4k});
            this.allWindows.Name = "allWindows";
            this.allWindows.Size = new System.Drawing.Size(85, 19);
            this.allWindows.Text = "All Windows";
            // 
            // allToggleHide
            // 
            this.allToggleHide.Name = "allToggleHide";
            this.allToggleHide.Size = new System.Drawing.Size(138, 22);
            this.allToggleHide.Text = "Toggle Hide";
            this.allToggleHide.Click += new System.EventHandler(this.allToggleHide_Click);
            // 
            // allSmall
            // 
            this.allSmall.Name = "allSmall";
            this.allSmall.Size = new System.Drawing.Size(138, 22);
            this.allSmall.Text = "Small";
            this.allSmall.Click += new System.EventHandler(this.allSmall_Click);
            // 
            // allLarge
            // 
            this.allLarge.Name = "allLarge";
            this.allLarge.Size = new System.Drawing.Size(138, 22);
            this.allLarge.Text = "Large";
            this.allLarge.Click += new System.EventHandler(this.allLarge_Click);
            // 
            // allLarge4k
            // 
            this.allLarge4k.Name = "allLarge4k";
            this.allLarge4k.Size = new System.Drawing.Size(138, 22);
            this.allLarge4k.Text = "Large(4k)";
            this.allLarge4k.Click += new System.EventHandler(this.allLarge4k_Click);
            // 
            // cascade
            // 
            this.cascade.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allVisible,
            this.commander});
            this.cascade.Name = "cascade";
            this.cascade.Size = new System.Drawing.Size(63, 19);
            this.cascade.Text = "Cascade";
            // 
            // allVisible
            // 
            this.allVisible.Name = "allVisible";
            this.allVisible.Size = new System.Drawing.Size(141, 22);
            this.allVisible.Text = "All Visible";
            this.allVisible.Click += new System.EventHandler(this.allVisible_Click);
            // 
            // commander
            // 
            this.commander.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.placeholderToolStripMenuItem});
            this.commander.Name = "commander";
            this.commander.Size = new System.Drawing.Size(141, 22);
            this.commander.Text = "Commander";
            this.commander.DropDownClosed += new System.EventHandler(this.dropClosed);
            this.commander.MouseEnter += new System.EventHandler(this.commander_MouseEnter);
            // 
            // placeholderToolStripMenuItem
            // 
            this.placeholderToolStripMenuItem.Name = "placeholderToolStripMenuItem";
            this.placeholderToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.placeholderToolStripMenuItem.Text = "placeholder";
            // 
            // monitors
            // 
            this.monitors.Name = "monitors";
            this.monitors.Size = new System.Drawing.Size(101, 19);
            this.monitors.Text = "Primary Display";
            // 
            // optionsBtn
            // 
            this.optionsBtn.Name = "optionsBtn";
            this.optionsBtn.Size = new System.Drawing.Size(61, 19);
            this.optionsBtn.Text = "Options";
            this.optionsBtn.Click += new System.EventHandler(this.optionsBtn_Click);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(599, 196);
            this.Controls.Add(this.thumbTbl);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(615, 235);
            this.Name = "MainForm";
            this.Text = "DA Windower";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem launchBtn;
        private System.Windows.Forms.ToolStripMenuItem sizeSelector;
        private System.Windows.Forms.ToolStripMenuItem small;
        private System.Windows.Forms.ToolStripMenuItem large;
        private System.Windows.Forms.ToolStripMenuItem fullscreen;
        private System.Windows.Forms.ToolStripMenuItem allWindows;
        private System.Windows.Forms.ToolStripMenuItem cascade;
        private System.Windows.Forms.ToolStripMenuItem monitors;
        private System.Windows.Forms.ToolStripMenuItem allToggleHide;
        private System.Windows.Forms.ToolStripMenuItem allSmall;
        private System.Windows.Forms.ToolStripMenuItem allLarge;
        private System.Windows.Forms.TableLayoutPanel thumbTbl;
        private System.Windows.Forms.ToolStripMenuItem optionsBtn;
        private System.Windows.Forms.ToolStripMenuItem allVisible;
        private System.Windows.Forms.ToolStripMenuItem commander;
        private System.Windows.Forms.ToolStripMenuItem placeholderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allLarge4k;
        private System.Windows.Forms.ToolStripMenuItem large4k;
    }
}

