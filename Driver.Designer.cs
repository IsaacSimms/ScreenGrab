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
            button2 = new Button();
            SystemTrayMenu.SuspendLayout();
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
            SendToSettings.ForeColor = SystemColors.Window;
            SendToSettings.Location = new Point(642, 58);
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
            button1.Location = new Point(642, 12);
            button1.Name = "button1";
            button1.Size = new Size(77, 40);
            button1.TabIndex = 2;
            button1.Text = "Shutdown ScreenGrab";
            button1.UseVisualStyleBackColor = false;
            button1.Click += MenuExit_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.Desktop;
            button2.ForeColor = SystemColors.Window;
            button2.Location = new Point(559, 12);
            button2.Name = "button2";
            button2.Size = new Size(77, 40);
            button2.TabIndex = 3;
            button2.Text = "Minimize";
            button2.UseVisualStyleBackColor = false;
            button2.Click += MinimizeToTray_Click;
            // 
            // Driver
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InfoText;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(730, 210);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(SendToSettings);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Driver";
            Text = "ScreenGrab";
            SystemTrayMenu.ResumeLayout(false);
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
        private Button button2;
    }
}
