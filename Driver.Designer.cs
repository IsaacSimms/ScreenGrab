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
            settingsToolStripMenuItem = new ToolStripMenuItem();
            menuExit = new ToolStripMenuItem();
            SendToSettings = new Button();
            button1 = new Button();
            activeWindowScreenshotButton = new Button();
            pictureBox1 = new PictureBox();
            regionScreenshotButton = new Button();
            delayedActiveWindowScreenshotButton = new Button();
            delayedRegionScreenshotButton = new Button();
            button2 = new Button();
            SystemTrayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
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
            SystemTrayMenu.Items.AddRange(new ToolStripItem[] { menuOpen, settingsToolStripMenuItem, menuExit });
            SystemTrayMenu.Name = "SystemTrayMenu";
            SystemTrayMenu.Size = new Size(129, 70);
            // 
            // menuOpen
            // 
            menuOpen.ForeColor = Color.Snow;
            menuOpen.Image = Properties.Resources.S1;
            menuOpen.Name = "menuOpen";
            menuOpen.Size = new Size(128, 22);
            menuOpen.Text = "Actions";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.ForeColor = SystemColors.Window;
            settingsToolStripMenuItem.Image = Properties.Resources.S1;
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(128, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += SendToSettings_Click;
            // 
            // menuExit
            // 
            menuExit.ForeColor = Color.Snow;
            menuExit.Image = Properties.Resources.S1;
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(128, 22);
            menuExit.Text = "Shutdown";
            // 
            // SendToSettings
            // 
            SendToSettings.BackColor = SystemColors.Desktop;
            SendToSettings.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SendToSettings.ForeColor = SystemColors.Window;
            SendToSettings.Location = new Point(365, 12);
            SendToSettings.Name = "SendToSettings";
            SendToSettings.Size = new Size(77, 40);
            SendToSettings.TabIndex = 1;
            SendToSettings.Text = "Settings";
            SendToSettings.UseVisualStyleBackColor = false;
            SendToSettings.Click += SendToSettings_Click;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.Desktop;
            button1.ForeColor = SystemColors.Window;
            button1.Location = new Point(448, 12);
            button1.Name = "button1";
            button1.Size = new Size(77, 40);
            button1.TabIndex = 2;
            button1.Text = "Shutdown ScreenGrab";
            button1.UseVisualStyleBackColor = false;
            button1.Click += MenuExit_Click;
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
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(221, 60);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
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
            // delayedActiveWindowScreenshotButton
            // 
            delayedActiveWindowScreenshotButton.BackColor = SystemColors.Desktop;
            delayedActiveWindowScreenshotButton.ForeColor = SystemColors.Window;
            delayedActiveWindowScreenshotButton.Location = new Point(12, 145);
            delayedActiveWindowScreenshotButton.Name = "delayedActiveWindowScreenshotButton";
            delayedActiveWindowScreenshotButton.Size = new Size(77, 60);
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
            delayedRegionScreenshotButton.Size = new Size(77, 60);
            delayedRegionScreenshotButton.TabIndex = 8;
            delayedRegionScreenshotButton.Text = "Delayed Region Capture";
            delayedRegionScreenshotButton.UseVisualStyleBackColor = false;
            delayedRegionScreenshotButton.Click += delayedRegionScreenshotButton_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.Desktop;
            button2.ForeColor = SystemColors.Window;
            button2.Location = new Point(12, 211);
            button2.Name = "button2";
            button2.Size = new Size(77, 40);
            button2.TabIndex = 9;
            button2.Text = "Open MS Paint";
            button2.UseVisualStyleBackColor = false;
            button2.Click += openClipboardImageInPaintButton_Click;
            // 
            // Driver
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InfoText;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(537, 269);
            Controls.Add(button2);
            Controls.Add(delayedRegionScreenshotButton);
            Controls.Add(delayedActiveWindowScreenshotButton);
            Controls.Add(regionScreenshotButton);
            Controls.Add(pictureBox1);
            Controls.Add(activeWindowScreenshotButton);
            Controls.Add(button1);
            Controls.Add(SendToSettings);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Driver";
            Text = " ";
            SystemTrayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon SystemTrayIcon;
        private ContextMenuStrip SystemTrayMenu;
        private ToolStripMenuItem menuOpen;
        private ToolStripMenuItem menuExit;
        private Button SendToSettings;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Button button1;
        private Button activeWindowScreenshotButton;
        private PictureBox pictureBox1;
        private Button regionScreenshotButton;
        private Button delayedActiveWindowScreenshotButton;
        private Button delayedRegionScreenshotButton;
        private Button button2;
    }
}
