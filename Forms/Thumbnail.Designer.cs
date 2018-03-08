namespace DAWindower
{
    partial class Thumbnail
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.exitBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleHide = new System.Windows.Forms.ToolStripMenuItem();
            this.small = new System.Windows.Forms.ToolStripMenuItem();
            this.large = new System.Windows.Forms.ToolStripMenuItem();
            this.large4k = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.windowTitleLbl = new System.Windows.Forms.ToolStripMenuItem();
            this.hiddenFsLbl = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitBtn,
            this.resizeBtn,
            this.windowTitleLbl});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip.Size = new System.Drawing.Size(198, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // exitBtn
            // 
            this.exitBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exitBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitBtn.ForeColor = System.Drawing.Color.Red;
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Padding = new System.Windows.Forms.Padding(0);
            this.exitBtn.Size = new System.Drawing.Size(24, 24);
            this.exitBtn.Text = "╳";
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // resizeBtn
            // 
            this.resizeBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.resizeBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleHide,
            this.small,
            this.large,
            this.large4k,
            this.fullscreen});
            this.resizeBtn.Font = new System.Drawing.Font("Segoe UI Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resizeBtn.ForeColor = System.Drawing.Color.Red;
            this.resizeBtn.Name = "resizeBtn";
            this.resizeBtn.Padding = new System.Windows.Forms.Padding(0);
            this.resizeBtn.Size = new System.Drawing.Size(23, 24);
            this.resizeBtn.Text = "\\/";
            // 
            // toggleHide
            // 
            this.toggleHide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.toggleHide.ForeColor = System.Drawing.Color.White;
            this.toggleHide.Name = "toggleHide";
            this.toggleHide.Size = new System.Drawing.Size(149, 22);
            this.toggleHide.Text = "Toggle Hide";
            this.toggleHide.Click += new System.EventHandler(this.toggleHide_Click);
            // 
            // small
            // 
            this.small.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.small.ForeColor = System.Drawing.Color.White;
            this.small.Name = "small";
            this.small.Size = new System.Drawing.Size(149, 22);
            this.small.Text = "Small";
            this.small.Click += new System.EventHandler(this.small_Click);
            // 
            // large
            // 
            this.large.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.large.ForeColor = System.Drawing.Color.White;
            this.large.Name = "large";
            this.large.Size = new System.Drawing.Size(149, 22);
            this.large.Text = "Large";
            this.large.Click += new System.EventHandler(this.large_Click);
            // 
            // large4k
            // 
            this.large4k.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.large4k.ForeColor = System.Drawing.Color.White;
            this.large4k.Name = "large4k";
            this.large4k.Size = new System.Drawing.Size(149, 22);
            this.large4k.Text = "Large(4k)";
            this.large4k.Click += new System.EventHandler(this.large4k_Click);
            // 
            // fullscreen
            // 
            this.fullscreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.fullscreen.ForeColor = System.Drawing.Color.White;
            this.fullscreen.Name = "fullscreen";
            this.fullscreen.Size = new System.Drawing.Size(149, 22);
            this.fullscreen.Text = "Full Screen";
            this.fullscreen.Click += new System.EventHandler(this.fullscreen_Click);
            // 
            // windowTitleLbl
            // 
            this.windowTitleLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.windowTitleLbl.ForeColor = System.Drawing.Color.White;
            this.windowTitleLbl.Name = "windowTitleLbl";
            this.windowTitleLbl.Padding = new System.Windows.Forms.Padding(0);
            this.windowTitleLbl.Size = new System.Drawing.Size(65, 24);
            this.windowTitleLbl.Text = "Unknown";
            // 
            // hiddenFsLbl
            // 
            this.hiddenFsLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hiddenFsLbl.AutoSize = true;
            this.hiddenFsLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hiddenFsLbl.ForeColor = System.Drawing.Color.Black;
            this.hiddenFsLbl.Location = new System.Drawing.Point(32, 67);
            this.hiddenFsLbl.Name = "hiddenFsLbl";
            this.hiddenFsLbl.Size = new System.Drawing.Size(135, 50);
            this.hiddenFsLbl.TabIndex = 1;
            this.hiddenFsLbl.Text = "CANNOT\r\nGENERATE";
            this.hiddenFsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.hiddenFsLbl.Visible = false;
            this.hiddenFsLbl.Click += new System.EventHandler(this.Thumbnail_Click);
            // 
            // Thumbnail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.hiddenFsLbl);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(200, 175);
            this.Name = "Thumbnail";
            this.Size = new System.Drawing.Size(198, 173);
            this.Click += new System.EventHandler(this.Thumbnail_Click);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem exitBtn;
        private System.Windows.Forms.ToolStripMenuItem resizeBtn;
        internal System.Windows.Forms.ToolStripMenuItem windowTitleLbl;
        private System.Windows.Forms.ToolStripMenuItem toggleHide;
        private System.Windows.Forms.ToolStripMenuItem small;
        private System.Windows.Forms.ToolStripMenuItem large;
        private System.Windows.Forms.ToolStripMenuItem fullscreen;
        internal System.Windows.Forms.Label hiddenFsLbl;
        private System.Windows.Forms.ToolStripMenuItem large4k;
    }
}
