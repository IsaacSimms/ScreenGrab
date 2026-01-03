namespace ScreenGrab
{
    partial class Driver
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Driver));
            SystemTrayIcon = new NotifyIcon(components);
            SystemTrayMenu = new ContextMenuStrip(components);
            menuOpen = new ToolStripMenuItem();
            regionCaptureToolStripMenuItem = new ToolStripMenuItem();
            activeWindowToolStripMenuItem = new ToolStripMenuItem();
            oCRCaptureToolStripMenuItem = new ToolStripMenuItem();
            uIElementCaptureToolStripMenuItem = new ToolStripMenuItem();
            delayedRegionToolStripMenuItem = new ToolStripMenuItem();
            delayedActiveWindowToolStripMenuItem = new ToolStripMenuItem();
            freeformToolStripMenuItem = new ToolStripMenuItem();
            openEditorToolStripMenuItem = new ToolStripMenuItem();
            openMSPaintToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            menuExit = new ToolStripMenuItem();
            btnSendToSettings = new Button();
            btnShutdownScreengrab = new Button();
            activeWindowScreenshotButton = new Button();
            DriverHeader = new PictureBox();
            delayedActiveWindowScreenshotButton = new Button();
            delayedRegionScreenshotButton = new Button();
            button2 = new Button();
            button1 = new Button();
            button3 = new Button();
            btnOcrCapture = new Button();
            regionScreenshotButton = new Button();
            btnUiElementCapture = new Button();
            SystemTrayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DriverHeader).BeginInit();
            SuspendLayout();
            // 
            // SystemTrayIcon
            // 
            SystemTrayIcon.Icon = (Icon)resources.GetObject("SystemTrayIcon.Icon");
            SystemTrayIcon.Text = "ScreenGrab";
            // 
            // SystemTrayMenu
            // 
            SystemTrayMenu.BackColor = SystemColors.Desktop;
            SystemTrayMenu.Items.AddRange(new ToolStripItem[] { menuOpen, regionCaptureToolStripMenuItem, activeWindowToolStripMenuItem, oCRCaptureToolStripMenuItem, uIElementCaptureToolStripMenuItem, delayedRegionToolStripMenuItem, delayedActiveWindowToolStripMenuItem, freeformToolStripMenuItem, openEditorToolStripMenuItem, openMSPaintToolStripMenuItem, settingsToolStripMenuItem, menuExit });
            SystemTrayMenu.Name = "SystemTrayMenu";
            SystemTrayMenu.Size = new Size(200, 268);
            // 
            // menuOpen
            // 
            menuOpen.ForeColor = Color.Snow;
            menuOpen.Image = Properties.Resources.S1;
            menuOpen.Name = "menuOpen";
            menuOpen.Size = new Size(199, 22);
            menuOpen.Text = "Home";
            // 
            // regionCaptureToolStripMenuItem
            // 
            regionCaptureToolStripMenuItem.ForeColor = SystemColors.Window;
            regionCaptureToolStripMenuItem.Image = (Image)resources.GetObject("regionCaptureToolStripMenuItem.Image");
            regionCaptureToolStripMenuItem.Name = "regionCaptureToolStripMenuItem";
            regionCaptureToolStripMenuItem.Size = new Size(199, 22);
            regionCaptureToolStripMenuItem.Text = "Region Capture";
            regionCaptureToolStripMenuItem.Click += regionScreenshotButton_Click;
            // 
            // activeWindowToolStripMenuItem
            // 
            activeWindowToolStripMenuItem.ForeColor = SystemColors.Window;
            activeWindowToolStripMenuItem.Image = (Image)resources.GetObject("activeWindowToolStripMenuItem.Image");
            activeWindowToolStripMenuItem.Name = "activeWindowToolStripMenuItem";
            activeWindowToolStripMenuItem.Size = new Size(199, 22);
            activeWindowToolStripMenuItem.Text = "Active Window";
            activeWindowToolStripMenuItem.Click += activeWindowScreenshotButton_Click;
            // 
            // oCRCaptureToolStripMenuItem
            // 
            oCRCaptureToolStripMenuItem.ForeColor = SystemColors.Window;
            oCRCaptureToolStripMenuItem.Image = (Image)resources.GetObject("oCRCaptureToolStripMenuItem.Image");
            oCRCaptureToolStripMenuItem.Name = "oCRCaptureToolStripMenuItem";
            oCRCaptureToolStripMenuItem.Size = new Size(199, 22);
            oCRCaptureToolStripMenuItem.Text = "OCR Capture";
            oCRCaptureToolStripMenuItem.Click += ocrRegionScreenshotButton_Click;
            // 
            // uIElementCaptureToolStripMenuItem
            // 
            uIElementCaptureToolStripMenuItem.ForeColor = SystemColors.Window;
            uIElementCaptureToolStripMenuItem.Image = (Image)resources.GetObject("uIElementCaptureToolStripMenuItem.Image");
            uIElementCaptureToolStripMenuItem.Name = "uIElementCaptureToolStripMenuItem";
            uIElementCaptureToolStripMenuItem.Size = new Size(199, 22);
            uIElementCaptureToolStripMenuItem.Text = "UI Element Capture";
            uIElementCaptureToolStripMenuItem.Click += btnUiElementCapture_Click;
            // 
            // delayedRegionToolStripMenuItem
            // 
            delayedRegionToolStripMenuItem.ForeColor = SystemColors.Window;
            delayedRegionToolStripMenuItem.Image = (Image)resources.GetObject("delayedRegionToolStripMenuItem.Image");
            delayedRegionToolStripMenuItem.Name = "delayedRegionToolStripMenuItem";
            delayedRegionToolStripMenuItem.Size = new Size(199, 22);
            delayedRegionToolStripMenuItem.Text = "Delayed Region";
            delayedRegionToolStripMenuItem.Click += delayedRegionScreenshotButton_Click;
            // 
            // delayedActiveWindowToolStripMenuItem
            // 
            delayedActiveWindowToolStripMenuItem.ForeColor = SystemColors.Window;
            delayedActiveWindowToolStripMenuItem.Image = (Image)resources.GetObject("delayedActiveWindowToolStripMenuItem.Image");
            delayedActiveWindowToolStripMenuItem.Name = "delayedActiveWindowToolStripMenuItem";
            delayedActiveWindowToolStripMenuItem.Size = new Size(199, 22);
            delayedActiveWindowToolStripMenuItem.Text = "Delayed Active Window";
            delayedActiveWindowToolStripMenuItem.Click += delayedActiveWindowScreenshotButton_Click;
            // 
            // freeformToolStripMenuItem
            // 
            freeformToolStripMenuItem.ForeColor = SystemColors.Window;
            freeformToolStripMenuItem.Image = (Image)resources.GetObject("freeformToolStripMenuItem.Image");
            freeformToolStripMenuItem.Name = "freeformToolStripMenuItem";
            freeformToolStripMenuItem.Size = new Size(199, 22);
            freeformToolStripMenuItem.Text = "Freeform";
            freeformToolStripMenuItem.Click += freeformScreenshotButton_Click;
            // 
            // openEditorToolStripMenuItem
            // 
            openEditorToolStripMenuItem.ForeColor = SystemColors.Window;
            openEditorToolStripMenuItem.Image = (Image)resources.GetObject("openEditorToolStripMenuItem.Image");
            openEditorToolStripMenuItem.Name = "openEditorToolStripMenuItem";
            openEditorToolStripMenuItem.Size = new Size(199, 22);
            openEditorToolStripMenuItem.Text = "Open Editor";
            openEditorToolStripMenuItem.Click += SendToEditor_Click;
            // 
            // openMSPaintToolStripMenuItem
            // 
            openMSPaintToolStripMenuItem.ForeColor = SystemColors.Window;
            openMSPaintToolStripMenuItem.Image = (Image)resources.GetObject("openMSPaintToolStripMenuItem.Image");
            openMSPaintToolStripMenuItem.Name = "openMSPaintToolStripMenuItem";
            openMSPaintToolStripMenuItem.Size = new Size(199, 22);
            openMSPaintToolStripMenuItem.Text = "Open MSPaint";
            openMSPaintToolStripMenuItem.Click += openClipboardImageInPaintButton_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.ForeColor = SystemColors.Window;
            settingsToolStripMenuItem.Image = (Image)resources.GetObject("settingsToolStripMenuItem.Image");
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(199, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += SendToSettings_Click;
            // 
            // menuExit
            // 
            menuExit.ForeColor = Color.Snow;
            menuExit.Image = Properties.Resources.S1;
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(199, 22);
            menuExit.Text = "Shutdown";
            // 
            // btnSendToSettings
            // 
            btnSendToSettings.BackColor = SystemColors.Desktop;
            btnSendToSettings.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSendToSettings.ForeColor = SystemColors.Window;
            btnSendToSettings.Location = new Point(365, 12);
            btnSendToSettings.Name = "btnSendToSettings";
            btnSendToSettings.Size = new Size(77, 40);
            btnSendToSettings.TabIndex = 1;
            btnSendToSettings.Text = "Settings";
            btnSendToSettings.UseVisualStyleBackColor = false;
            btnSendToSettings.Click += SendToSettings_Click;
            // 
            // btnShutdownScreengrab
            // 
            btnShutdownScreengrab.BackColor = SystemColors.Desktop;
            btnShutdownScreengrab.ForeColor = SystemColors.Window;
            btnShutdownScreengrab.Location = new Point(448, 12);
            btnShutdownScreengrab.Name = "btnShutdownScreengrab";
            btnShutdownScreengrab.Size = new Size(77, 40);
            btnShutdownScreengrab.TabIndex = 2;
            btnShutdownScreengrab.Text = "Shutdown ScreenGrab";
            btnShutdownScreengrab.UseVisualStyleBackColor = false;
            btnShutdownScreengrab.Click += MenuExit_Click;
            // 
            // activeWindowScreenshotButton
            // 
            activeWindowScreenshotButton.BackColor = SystemColors.Desktop;
            activeWindowScreenshotButton.ForeColor = SystemColors.Window;
            activeWindowScreenshotButton.Location = new Point(12, 99);
            activeWindowScreenshotButton.Name = "activeWindowScreenshotButton";
            activeWindowScreenshotButton.Size = new Size(77, 40);
            activeWindowScreenshotButton.TabIndex = 4;
            activeWindowScreenshotButton.Text = "Active Window";
            activeWindowScreenshotButton.UseVisualStyleBackColor = false;
            activeWindowScreenshotButton.Click += activeWindowScreenshotButton_Click;
            // 
            // DriverHeader
            // 
            DriverHeader.Image = (Image)resources.GetObject("DriverHeader.Image");
            DriverHeader.Location = new Point(12, 12);
            DriverHeader.Name = "DriverHeader";
            DriverHeader.Size = new Size(221, 60);
            DriverHeader.TabIndex = 5;
            DriverHeader.TabStop = false;
            // 
            // delayedActiveWindowScreenshotButton
            // 
            delayedActiveWindowScreenshotButton.BackColor = SystemColors.Desktop;
            delayedActiveWindowScreenshotButton.ForeColor = SystemColors.Window;
            delayedActiveWindowScreenshotButton.Location = new Point(12, 145);
            delayedActiveWindowScreenshotButton.Name = "delayedActiveWindowScreenshotButton";
            delayedActiveWindowScreenshotButton.Size = new Size(77, 86);
            delayedActiveWindowScreenshotButton.TabIndex = 7;
            delayedActiveWindowScreenshotButton.Text = "Delayed Active Window";
            delayedActiveWindowScreenshotButton.UseVisualStyleBackColor = false;
            delayedActiveWindowScreenshotButton.Click += delayedActiveWindowScreenshotButton_Click;
            // 
            // delayedRegionScreenshotButton
            // 
            delayedRegionScreenshotButton.BackColor = SystemColors.Desktop;
            delayedRegionScreenshotButton.ForeColor = SystemColors.Window;
            delayedRegionScreenshotButton.Location = new Point(95, 145);
            delayedRegionScreenshotButton.Name = "delayedRegionScreenshotButton";
            delayedRegionScreenshotButton.Size = new Size(77, 86);
            delayedRegionScreenshotButton.TabIndex = 8;
            delayedRegionScreenshotButton.Text = "Delayed Region Capture";
            delayedRegionScreenshotButton.UseVisualStyleBackColor = false;
            delayedRegionScreenshotButton.Click += delayedRegionScreenshotButton_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.Desktop;
            button2.ForeColor = SystemColors.Window;
            button2.Location = new Point(448, 99);
            button2.Name = "button2";
            button2.Size = new Size(77, 40);
            button2.TabIndex = 9;
            button2.Text = "Open MS Paint";
            button2.UseVisualStyleBackColor = false;
            button2.Click += openClipboardImageInPaintButton_Click;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.Desktop;
            button1.ForeColor = SystemColors.Window;
            button1.Location = new Point(178, 191);
            button1.Name = "button1";
            button1.Size = new Size(77, 40);
            button1.TabIndex = 10;
            button1.Text = "Freeform Capture";
            button1.UseVisualStyleBackColor = false;
            button1.Click += freeformScreenshotButton_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.Desktop;
            button3.ForeColor = SystemColors.Window;
            button3.Location = new Point(365, 99);
            button3.Name = "button3";
            button3.Size = new Size(77, 40);
            button3.TabIndex = 11;
            button3.Text = "Open Editor";
            button3.UseVisualStyleBackColor = false;
            button3.Click += SendToEditor_Click;
            // 
            // btnOcrCapture
            // 
            btnOcrCapture.BackColor = SystemColors.Desktop;
            btnOcrCapture.ForeColor = SystemColors.Window;
            btnOcrCapture.Location = new Point(178, 145);
            btnOcrCapture.Name = "btnOcrCapture";
            btnOcrCapture.Size = new Size(77, 40);
            btnOcrCapture.TabIndex = 12;
            btnOcrCapture.Text = "OCR Capture";
            btnOcrCapture.UseVisualStyleBackColor = false;
            btnOcrCapture.Click += ocrRegionScreenshotButton_Click;
            // 
            // regionScreenshotButton
            // 
            regionScreenshotButton.BackColor = SystemColors.Desktop;
            regionScreenshotButton.ForeColor = SystemColors.Window;
            regionScreenshotButton.Location = new Point(95, 99);
            regionScreenshotButton.Name = "regionScreenshotButton";
            regionScreenshotButton.Size = new Size(77, 40);
            regionScreenshotButton.TabIndex = 6;
            regionScreenshotButton.Text = "Region Capture";
            regionScreenshotButton.UseVisualStyleBackColor = false;
            regionScreenshotButton.Click += regionScreenshotButton_Click;
            // 
            // btnUiElementCapture
            // 
            btnUiElementCapture.BackColor = SystemColors.Desktop;
            btnUiElementCapture.ForeColor = SystemColors.Window;
            btnUiElementCapture.Location = new Point(178, 99);
            btnUiElementCapture.Name = "btnUiElementCapture";
            btnUiElementCapture.Size = new Size(77, 40);
            btnUiElementCapture.TabIndex = 13;
            btnUiElementCapture.Text = "UI Element Capture";
            btnUiElementCapture.UseVisualStyleBackColor = false;
            btnUiElementCapture.Click += btnUiElementCapture_Click;
            // 
            // Driver
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InfoText;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(537, 242);
            Controls.Add(btnUiElementCapture);
            Controls.Add(btnOcrCapture);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(button2);
            Controls.Add(delayedRegionScreenshotButton);
            Controls.Add(delayedActiveWindowScreenshotButton);
            Controls.Add(regionScreenshotButton);
            Controls.Add(DriverHeader);
            Controls.Add(activeWindowScreenshotButton);
            Controls.Add(btnShutdownScreengrab);
            Controls.Add(btnSendToSettings);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Driver";
            Text = " ScreenGrab";
            SystemTrayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DriverHeader).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon SystemTrayIcon;
        private ContextMenuStrip SystemTrayMenu;
        private ToolStripMenuItem menuOpen;
        private ToolStripMenuItem menuExit;
        private Button btnSendToSettings;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Button btnShutdownScreengrab;
        private Button activeWindowScreenshotButton;
        private PictureBox DriverHeader;
        private Button delayedActiveWindowScreenshotButton;
        private Button delayedRegionScreenshotButton;
        private Button button2;
        private Button button1;
        private ToolStripMenuItem regionCaptureToolStripMenuItem;
        private ToolStripMenuItem activeWindowToolStripMenuItem;
        private ToolStripMenuItem delayedActiveWindowToolStripMenuItem;
        private ToolStripMenuItem delayedRegionToolStripMenuItem;
        private ToolStripMenuItem freeformToolStripMenuItem;
        private ToolStripMenuItem openMSPaintToolStripMenuItem;
        private Button button3;
        private ToolStripMenuItem openEditorToolStripMenuItem;
        private Button btnOcrCapture;
        private ToolStripMenuItem oCRCaptureToolStripMenuItem;
        private Button regionScreenshotButton;
        private Button btnUiElementCapture;
        private Button button4;
        private ToolStripMenuItem uIElementCaptureToolStripMenuItem;
    }
}
