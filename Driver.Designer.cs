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
            menuExit = new ToolStripMenuItem();
            SendToSettings = new Button();
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
            SystemTrayMenu.Items.AddRange(new ToolStripItem[] { menuOpen, menuExit });
            SystemTrayMenu.Name = "SystemTrayMenu";
            SystemTrayMenu.Size = new Size(138, 48);
            // 
            // menuOpen
            // 
            menuOpen.ForeColor = Color.Snow;
            menuOpen.Image = Properties.Resources.S1;
            menuOpen.Name = "menuOpen";
            menuOpen.Size = new Size(137, 22);
            menuOpen.Text = "Open Menu";
            // 
            // menuExit
            // 
            menuExit.ForeColor = Color.Snow;
            menuExit.Image = Properties.Resources.S1;
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(137, 22);
            menuExit.Text = "Close App";
            // 
            // SendToSettings
            // 
            SendToSettings.Location = new Point(78, 53);
            SendToSettings.Name = "SendToSettings";
            SendToSettings.Size = new Size(100, 54);
            SendToSettings.TabIndex = 1;
            SendToSettings.Text = "Settings";
            SendToSettings.UseVisualStyleBackColor = true;
            SendToSettings.Click += button1_Click;
            // 
            // Driver
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InfoText;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1315, 847);
            Controls.Add(SendToSettings);
            DoubleBuffered = true;
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
    }
}
