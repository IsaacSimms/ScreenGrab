namespace ScreenGrab
{
    partial class ImageEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageEditorForm));
            pictureBox1 = new PictureBox();
            btnGoHome = new Button();
            pictureBoxImage = new PictureBox();
            EditorMenu = new MenuStrip();
            openToolStripMenuItem = new ToolStripMenuItem();
            EditorToolStrip = new ToolStrip();
            toolStripSplitButton1 = new ToolStripSplitButton();
            btnSelectColor = new ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).BeginInit();
            EditorMenu.SuspendLayout();
            EditorToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(167, 75);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnGoHome
            // 
            btnGoHome.BackColor = SystemColors.Desktop;
            btnGoHome.ForeColor = SystemColors.Window;
            btnGoHome.Location = new Point(1058, 12);
            btnGoHome.Name = "btnGoHome";
            btnGoHome.Size = new Size(77, 40);
            btnGoHome.TabIndex = 22;
            btnGoHome.Text = "Home";
            btnGoHome.UseVisualStyleBackColor = false;
            btnGoHome.Click += btnGoHome_Click;
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.Location = new Point(0, 105);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(1123, 550);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.TabIndex = 24;
            pictureBoxImage.TabStop = false;
            // 
            // EditorMenu
            // 
            EditorMenu.Anchor = AnchorStyles.None;
            EditorMenu.BackColor = SystemColors.Desktop;
            EditorMenu.Dock = DockStyle.None;
            EditorMenu.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            EditorMenu.Location = new Point(170, 51);
            EditorMenu.Name = "EditorMenu";
            EditorMenu.Size = new Size(65, 24);
            EditorMenu.TabIndex = 25;
            EditorMenu.Text = "menuStrip1";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.BackColor = SystemColors.Desktop;
            openToolStripMenuItem.ForeColor = SystemColors.Window;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(57, 20);
            openToolStripMenuItem.Text = "Open...";
            // 
            // EditorToolStrip
            // 
            EditorToolStrip.Anchor = AnchorStyles.None;
            EditorToolStrip.BackColor = SystemColors.Desktop;
            EditorToolStrip.Dock = DockStyle.None;
            EditorToolStrip.Items.AddRange(new ToolStripItem[] { toolStripSplitButton1, btnSelectColor });
            EditorToolStrip.Location = new Point(12, 78);
            EditorToolStrip.Name = "EditorToolStrip";
            EditorToolStrip.Size = new Size(67, 25);
            EditorToolStrip.TabIndex = 26;
            EditorToolStrip.Text = "toolStrip1";
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripSplitButton1.Image = (Image)resources.GetObject("toolStripSplitButton1.Image");
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new Size(32, 22);
            toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // btnSelectColor
            // 
            btnSelectColor.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSelectColor.ImageTransparentColor = Color.Magenta;
            btnSelectColor.Name = "btnSelectColor";
            btnSelectColor.Size = new Size(23, 22);
            btnSelectColor.Text = "toolStripButton1";
            btnSelectColor.Click += btnSelectColor_Click;
            // 
            // ImageEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1147, 667);
            Controls.Add(EditorToolStrip);
            Controls.Add(pictureBoxImage);
            Controls.Add(btnGoHome);
            Controls.Add(pictureBox1);
            Controls.Add(EditorMenu);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ImageEditorForm";
            Text = "ScreenGrab";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).EndInit();
            EditorMenu.ResumeLayout(false);
            EditorMenu.PerformLayout();
            EditorToolStrip.ResumeLayout(false);
            EditorToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnGoHome;
        private PictureBox pictureBoxImage;
        private MenuStrip EditorMenu;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStrip EditorToolStrip;
        private ToolStripSplitButton toolStripSplitButton1;
        private ToolStripButton btnSelectColor;
    }
}