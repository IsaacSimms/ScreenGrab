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
            EditorHeader = new PictureBox();
            btnGoHome = new Button();
            pictureBoxImage = new PictureBox();
            EditorMenu = new MenuStrip();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            copyToClipboardToolStripMenuItem = new ToolStripMenuItem();
            EditorToolStrip = new ToolStrip();
            toolStripButton2 = new ToolStripButton();
            toolStripButton8 = new ToolStripButton();
            toolStripButton1 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            toolStripButton5 = new ToolStripButton();
            toolStripButton6 = new ToolStripButton();
            toolStripButton4 = new ToolStripButton();
            btnSelectColor = new ToolStripButton();
            toolStripButton7 = new ToolStripButton();
            btnSendToSettings = new Button();
            panelImageContainer = new Panel();
            ZoomToolStrip = new ToolStrip();
            btnZoomPlus = new ToolStripButton();
            btnZoomMinus = new ToolStripButton();
            btnZoomReset = new ToolStripButton();
            trackBarBrushSize = new TrackBar();
            lblBrushSize = new Label();
            ((System.ComponentModel.ISupportInitialize)EditorHeader).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).BeginInit();
            EditorMenu.SuspendLayout();
            EditorToolStrip.SuspendLayout();
            panelImageContainer.SuspendLayout();
            ZoomToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarBrushSize).BeginInit();
            SuspendLayout();
            // 
            // EditorHeader
            // 
            EditorHeader.Image = (Image)resources.GetObject("EditorHeader.Image");
            EditorHeader.Location = new Point(12, 0);
            EditorHeader.Name = "EditorHeader";
            EditorHeader.Size = new Size(167, 75);
            EditorHeader.TabIndex = 0;
            EditorHeader.TabStop = false;
            // 
            // btnGoHome
            // 
            btnGoHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGoHome.BackColor = SystemColors.Desktop;
            btnGoHome.ForeColor = SystemColors.Window;
            btnGoHome.Location = new Point(1337, 12);
            btnGoHome.Name = "btnGoHome";
            btnGoHome.Size = new Size(77, 40);
            btnGoHome.TabIndex = 22;
            btnGoHome.Text = "Home";
            btnGoHome.UseVisualStyleBackColor = false;
            btnGoHome.Click += btnGoHome_Click;
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.Location = new Point(0, -13);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(1402, 779);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxImage.TabIndex = 24;
            pictureBoxImage.TabStop = false;
            // 
            // EditorMenu
            // 
            EditorMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            EditorMenu.BackColor = SystemColors.Desktop;
            EditorMenu.Dock = DockStyle.None;
            EditorMenu.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, copyToClipboardToolStripMenuItem });
            EditorMenu.Location = new Point(1027, 28);
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
            openToolStripMenuItem.ToolTipText = "Open Image";
            openToolStripMenuItem.Click += btnLoadImage_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.ForeColor = SystemColors.Window;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(52, 20);
            saveToolStripMenuItem.Text = "Save...";
            saveToolStripMenuItem.ToolTipText = "Save Image";
            saveToolStripMenuItem.Click += btnSaveImage_Click;
            // 
            // copyToClipboardToolStripMenuItem
            // 
            copyToClipboardToolStripMenuItem.ForeColor = SystemColors.Window;
            copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            copyToClipboardToolStripMenuItem.Size = new Size(116, 20);
            copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
            copyToClipboardToolStripMenuItem.ToolTipText = "Copy Image";
            copyToClipboardToolStripMenuItem.Click += btnCopyToClipboard_Click;
            // 
            // EditorToolStrip
            // 
            EditorToolStrip.BackColor = SystemColors.Desktop;
            EditorToolStrip.BackgroundImageLayout = ImageLayout.None;
            EditorToolStrip.Dock = DockStyle.None;
            EditorToolStrip.Items.AddRange(new ToolStripItem[] { toolStripButton2, toolStripButton8, toolStripButton1, toolStripButton3, toolStripButton5, toolStripButton6, toolStripButton4, btnSelectColor, toolStripButton7 });
            EditorToolStrip.Location = new Point(12, 78);
            EditorToolStrip.Name = "EditorToolStrip";
            EditorToolStrip.RenderMode = ToolStripRenderMode.System;
            EditorToolStrip.Size = new Size(219, 25);
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
            toolStripButton2.ToolTipText = "Rectangle";
            toolStripButton2.Click += btnDrawRectangle_Click;
            // 
            // toolStripButton8
            // 
            toolStripButton8.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton8.Image = (Image)resources.GetObject("toolStripButton8.Image");
            toolStripButton8.ImageTransparentColor = Color.Magenta;
            toolStripButton8.Name = "toolStripButton8";
            toolStripButton8.Size = new Size(23, 22);
            toolStripButton8.Text = "toolStripButton8";
            toolStripButton8.ToolTipText = "Line";
            toolStripButton8.Click += btnDrawLine_Click;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(23, 22);
            toolStripButton1.Text = "btnArrow";
            toolStripButton1.ToolTipText = "Arrow";
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
            toolStripButton3.ToolTipText = "Freeform";
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
            toolStripButton5.ToolTipText = "Highlight";
            toolStripButton5.Click += btnDrawHighlight_Click;
            // 
            // toolStripButton6
            // 
            toolStripButton6.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton6.Image = (Image)resources.GetObject("toolStripButton6.Image");
            toolStripButton6.ImageTransparentColor = Color.Magenta;
            toolStripButton6.Name = "toolStripButton6";
            toolStripButton6.Size = new Size(23, 22);
            toolStripButton6.Text = "toolStripButton6";
            toolStripButton6.ToolTipText = "Blur";
            toolStripButton6.Click += btnDrawBlur_Click;
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = (Image)resources.GetObject("toolStripButton4.Image");
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new Size(23, 22);
            toolStripButton4.Text = "toolStripButton4";
            toolStripButton4.ToolTipText = "Text";
            toolStripButton4.Click += btnDrawText_Click;
            // 
            // btnSelectColor
            // 
            btnSelectColor.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSelectColor.ImageTransparentColor = Color.Magenta;
            btnSelectColor.Name = "btnSelectColor";
            btnSelectColor.Size = new Size(23, 22);
            btnSelectColor.Text = "toolStripButton1";
            btnSelectColor.ToolTipText = "Select Color";
            btnSelectColor.Click += btnSelectColor_Click;
            // 
            // toolStripButton7
            // 
            toolStripButton7.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton7.Image = (Image)resources.GetObject("toolStripButton7.Image");
            toolStripButton7.ImageTransparentColor = Color.Magenta;
            toolStripButton7.Name = "toolStripButton7";
            toolStripButton7.Size = new Size(23, 22);
            toolStripButton7.Text = "toolStripButton7";
            toolStripButton7.ToolTipText = "Undo";
            toolStripButton7.Click += btnUndo_Click;
            // 
            // btnSendToSettings
            // 
            btnSendToSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSendToSettings.BackColor = SystemColors.Desktop;
            btnSendToSettings.ForeColor = SystemColors.Window;
            btnSendToSettings.Location = new Point(1254, 12);
            btnSendToSettings.Name = "btnSendToSettings";
            btnSendToSettings.Size = new Size(77, 40);
            btnSendToSettings.TabIndex = 27;
            btnSendToSettings.Text = "Settings";
            btnSendToSettings.UseVisualStyleBackColor = false;
            btnSendToSettings.Click += btnSendToSettingsFromEditor_Click;
            // 
            // panelImageContainer
            // 
            panelImageContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelImageContainer.AutoScroll = true;
            panelImageContainer.Controls.Add(pictureBoxImage);
            panelImageContainer.Location = new Point(12, 106);
            panelImageContainer.Name = "panelImageContainer";
            panelImageContainer.Size = new Size(1402, 767);
            panelImageContainer.TabIndex = 28;
            // 
            // ZoomToolStrip
            // 
            ZoomToolStrip.BackColor = SystemColors.Desktop;
            ZoomToolStrip.Dock = DockStyle.None;
            ZoomToolStrip.Items.AddRange(new ToolStripItem[] { btnZoomPlus, btnZoomMinus, btnZoomReset });
            ZoomToolStrip.Location = new Point(1337, 78);
            ZoomToolStrip.Name = "ZoomToolStrip";
            ZoomToolStrip.RenderMode = ToolStripRenderMode.System;
            ZoomToolStrip.Size = new Size(81, 25);
            ZoomToolStrip.TabIndex = 29;
            ZoomToolStrip.Text = "toolStrip1";
            // 
            // btnZoomPlus
            // 
            btnZoomPlus.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnZoomPlus.Image = (Image)resources.GetObject("btnZoomPlus.Image");
            btnZoomPlus.ImageTransparentColor = Color.Magenta;
            btnZoomPlus.Name = "btnZoomPlus";
            btnZoomPlus.Size = new Size(23, 22);
            btnZoomPlus.Click += btnZoomIn_Click;
            // 
            // btnZoomMinus
            // 
            btnZoomMinus.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnZoomMinus.Image = (Image)resources.GetObject("btnZoomMinus.Image");
            btnZoomMinus.ImageTransparentColor = Color.Magenta;
            btnZoomMinus.Name = "btnZoomMinus";
            btnZoomMinus.Size = new Size(23, 22);
            btnZoomMinus.Click += btnZoomOut_Click;
            // 
            // btnZoomReset
            // 
            btnZoomReset.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnZoomReset.Image = (Image)resources.GetObject("btnZoomReset.Image");
            btnZoomReset.ImageTransparentColor = Color.Magenta;
            btnZoomReset.Name = "btnZoomReset";
            btnZoomReset.Size = new Size(23, 22);
            btnZoomReset.Click += btnResetZoom_Click;
            // 
            // trackBarBrushSize
            // 
            trackBarBrushSize.Location = new Point(237, 78);
            trackBarBrushSize.Maximum = 20;
            trackBarBrushSize.Name = "trackBarBrushSize";
            trackBarBrushSize.Size = new Size(202, 45);
            trackBarBrushSize.TabIndex = 30;
            trackBarBrushSize.Tag = "Brush Size";
            trackBarBrushSize.TickFrequency = 2;
            trackBarBrushSize.TickStyle = TickStyle.None;
            trackBarBrushSize.Scroll += trackBarBrushSize_Scroll;
            // 
            // lblBrushSize
            // 
            lblBrushSize.AutoSize = true;
            lblBrushSize.ForeColor = SystemColors.Window;
            lblBrushSize.Location = new Point(237, 60);
            lblBrushSize.Name = "lblBrushSize";
            lblBrushSize.Size = new Size(63, 15);
            lblBrushSize.TabIndex = 31;
            lblBrushSize.Text = "Brush Size:";
            // 
            // ImageEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1426, 884);
            Controls.Add(ZoomToolStrip);
            Controls.Add(panelImageContainer);
            Controls.Add(btnSendToSettings);
            Controls.Add(lblBrushSize);
            Controls.Add(trackBarBrushSize);
            Controls.Add(EditorToolStrip);
            Controls.Add(btnGoHome);
            Controls.Add(EditorHeader);
            Controls.Add(EditorMenu);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ImageEditorForm";
            Text = "ScreenGrab";
            ((System.ComponentModel.ISupportInitialize)EditorHeader).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).EndInit();
            EditorMenu.ResumeLayout(false);
            EditorMenu.PerformLayout();
            EditorToolStrip.ResumeLayout(false);
            EditorToolStrip.PerformLayout();
            panelImageContainer.ResumeLayout(false);
            ZoomToolStrip.ResumeLayout(false);
            ZoomToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarBrushSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox EditorHeader;
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
        private ToolStripButton toolStripButton6;
        private ToolStripButton toolStripButton7;
        private Panel panelImageContainer;
        private ToolStrip ZoomToolStrip;
        private ToolStripButton btnZoomPlus;
        private ToolStripButton btnZoomMinus;
        private ToolStripButton btnZoomReset;
        private TrackBar trackBarBrushSize;
        private Label lblBrushSize;
        private ToolStripButton toolStripButton8;
    }
}