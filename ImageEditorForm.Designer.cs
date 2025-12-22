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
            saveToolStripMenuItem = new ToolStripMenuItem();
            copyToClipboardToolStripMenuItem = new ToolStripMenuItem();
            EditorToolStrip = new ToolStrip();
            toolStripButton2 = new ToolStripButton();
            toolStripButton1 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            toolStripButton5 = new ToolStripButton();
            toolStripButton4 = new ToolStripButton();
            btnSelectColor = new ToolStripButton();
            btnSendToSettings = new Button();
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
            btnGoHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
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
            pictureBoxImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxImage.Location = new Point(12, 106);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(1123, 550);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.TabIndex = 24;
            pictureBoxImage.TabStop = false;
            // 
            // EditorMenu
            // 
            EditorMenu.BackColor = SystemColors.Desktop;
            EditorMenu.Dock = DockStyle.None;
            EditorMenu.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, copyToClipboardToolStripMenuItem });
            EditorMenu.Location = new Point(170, 51);
            EditorMenu.Name = "EditorMenu";
            EditorMenu.Size = new Size(233, 24);
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
            openToolStripMenuItem.Click += btnLoadImage_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.ForeColor = SystemColors.Window;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(52, 20);
            saveToolStripMenuItem.Text = "Save...";
            saveToolStripMenuItem.Click += btnSaveImage_Click;
            // 
            // copyToClipboardToolStripMenuItem
            // 
            copyToClipboardToolStripMenuItem.ForeColor = SystemColors.Window;
            copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            copyToClipboardToolStripMenuItem.Size = new Size(116, 20);
            copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
            copyToClipboardToolStripMenuItem.Click += btnCopyToClipboard_Click;
            // 
            // EditorToolStrip
            // 
            EditorToolStrip.BackColor = SystemColors.Desktop;
            EditorToolStrip.BackgroundImageLayout = ImageLayout.None;
            EditorToolStrip.Dock = DockStyle.None;
            EditorToolStrip.Items.AddRange(new ToolStripItem[] { toolStripButton2, toolStripButton1, toolStripButton3, toolStripButton5, toolStripButton4, btnSelectColor });
            EditorToolStrip.Location = new Point(12, 78);
            EditorToolStrip.Name = "EditorToolStrip";
            EditorToolStrip.RenderMode = ToolStripRenderMode.System;
            EditorToolStrip.Size = new Size(150, 25);
            EditorToolStrip.TabIndex = 26;
            EditorToolStrip.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(23, 22);
            toolStripButton2.Text = "btnRectangle";
            toolStripButton2.Click += btnDrawRectangle_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(23, 22);
            toolStripButton1.Text = "btnArrow";
            toolStripButton1.Click += btnDrawArrow_Click;
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = (Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(23, 22);
            toolStripButton3.Text = "btnFreeform";
            toolStripButton3.Click += btnDrawFreeform_Click;
            // 
            // toolStripButton5
            // 
            toolStripButton5.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton5.Image = (Image)resources.GetObject("toolStripButton5.Image");
            toolStripButton5.ImageTransparentColor = Color.Magenta;
            toolStripButton5.Name = "toolStripButton5";
            toolStripButton5.Size = new Size(23, 22);
            toolStripButton5.Text = "toolStripButton5";
            toolStripButton5.Click += btnDrawHighlight_Click;
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = (Image)resources.GetObject("toolStripButton4.Image");
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new Size(23, 22);
            toolStripButton4.Text = "toolStripButton4";
            toolStripButton4.ToolTipText = "btnText";
            toolStripButton4.Click += btnDrawText_Click;
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
            // btnSendToSettings
            // 
            btnSendToSettings.BackColor = SystemColors.Desktop;
            btnSendToSettings.ForeColor = SystemColors.Window;
            btnSendToSettings.Location = new Point(975, 12);
            btnSendToSettings.Name = "btnSendToSettings";
            btnSendToSettings.Size = new Size(77, 40);
            btnSendToSettings.TabIndex = 27;
            btnSendToSettings.Text = "Settings";
            btnSendToSettings.UseVisualStyleBackColor = false;
            btnSendToSettings.Click += btnSendToSettingsFromEditor_Click;
            // 
            // ImageEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1147, 667);
            Controls.Add(btnSendToSettings);
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
        private ToolStripButton btnSelectColor;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripButton toolStripButton3;
        private ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private ToolStripButton toolStripButton4;
        private ToolStripButton toolStripButton5;
        private Button btnSendToSettings;
    }
}