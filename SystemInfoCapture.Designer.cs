namespace ScreenGrab
{
    partial class SystemInfoCapture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemInfoCapture));
            pictureBox1 = new PictureBox();
            pictureBoxScreenshot = new PictureBox();
            txtSystemInfo = new TextBox();
            btnHome = new Button();
            menuStripTextBox = new MenuStrip();
            copyTextToolStripMenuItem = new ToolStripMenuItem();
            printTextOnImageToolStripMenuItem = new ToolStripMenuItem();
            panelImageContainerSysteminfo = new Panel();
            menuStripScreenshot = new MenuStrip();
            sendImageToEditorToolStripMenuItem = new ToolStripMenuItem();
            copyToClipboardToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exportAsMarkdownToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).BeginInit();
            menuStripTextBox.SuspendLayout();
            panelImageContainerSysteminfo.SuspendLayout();
            menuStripScreenshot.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(367, 70);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBoxScreenshot
            // 
            pictureBoxScreenshot.Dock = DockStyle.Fill;
            pictureBoxScreenshot.Location = new Point(0, 0);
            pictureBoxScreenshot.Name = "pictureBoxScreenshot";
            pictureBoxScreenshot.Size = new Size(1402, 700);
            pictureBoxScreenshot.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxScreenshot.TabIndex = 1;
            pictureBoxScreenshot.TabStop = false;
            // 
            // txtSystemInfo
            // 
            txtSystemInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSystemInfo.BackColor = SystemColors.Desktop;
            txtSystemInfo.BorderStyle = BorderStyle.FixedSingle;
            txtSystemInfo.ForeColor = SystemColors.Window;
            txtSystemInfo.Location = new Point(13, 818);
            txtSystemInfo.Multiline = true;
            txtSystemInfo.Name = "txtSystemInfo";
            txtSystemInfo.ReadOnly = true;
            txtSystemInfo.ScrollBars = ScrollBars.Vertical;
            txtSystemInfo.Size = new Size(1402, 358);
            txtSystemInfo.TabIndex = 2;
            // 
            // btnHome
            // 
            btnHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHome.BackColor = SystemColors.Desktop;
            btnHome.ForeColor = SystemColors.Window;
            btnHome.Location = new Point(1337, 12);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(77, 40);
            btnHome.TabIndex = 3;
            btnHome.Text = "Home";
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            // 
            // menuStripTextBox
            // 
            menuStripTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            menuStripTextBox.BackColor = SystemColors.Desktop;
            menuStripTextBox.Dock = DockStyle.None;
            menuStripTextBox.Items.AddRange(new ToolStripItem[] { copyTextToolStripMenuItem, printTextOnImageToolStripMenuItem });
            menuStripTextBox.Location = new Point(1215, 791);
            menuStripTextBox.Name = "menuStripTextBox";
            menuStripTextBox.Size = new Size(200, 24);
            menuStripTextBox.TabIndex = 4;
            menuStripTextBox.Text = "menuStrip1";
            menuStripTextBox.Click += btnCopyInfo_Click;
            // 
            // copyTextToolStripMenuItem
            // 
            copyTextToolStripMenuItem.ForeColor = SystemColors.Window;
            copyTextToolStripMenuItem.Name = "copyTextToolStripMenuItem";
            copyTextToolStripMenuItem.Size = new Size(71, 20);
            copyTextToolStripMenuItem.Text = "Copy Text";
            // 
            // printTextOnImageToolStripMenuItem
            // 
            printTextOnImageToolStripMenuItem.ForeColor = SystemColors.Window;
            printTextOnImageToolStripMenuItem.Name = "printTextOnImageToolStripMenuItem";
            printTextOnImageToolStripMenuItem.Size = new Size(121, 20);
            printTextOnImageToolStripMenuItem.Text = "Print Text on Image";
            printTextOnImageToolStripMenuItem.Click += btnPrintInfoOnImage_Click;
            // 
            // panelImageContainerSysteminfo
            // 
            panelImageContainerSysteminfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelImageContainerSysteminfo.Controls.Add(pictureBoxScreenshot);
            panelImageContainerSysteminfo.Location = new Point(12, 88);
            panelImageContainerSysteminfo.Name = "panelImageContainerSysteminfo";
            panelImageContainerSysteminfo.Size = new Size(1402, 700);
            panelImageContainerSysteminfo.TabIndex = 5;
            // 
            // menuStripScreenshot
            // 
            menuStripScreenshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            menuStripScreenshot.BackColor = SystemColors.Desktop;
            menuStripScreenshot.Dock = DockStyle.None;
            menuStripScreenshot.Items.AddRange(new ToolStripItem[] { sendImageToEditorToolStripMenuItem, copyToClipboardToolStripMenuItem, saveToolStripMenuItem, exportAsMarkdownToolStripMenuItem });
            menuStripScreenshot.Location = new Point(945, 58);
            menuStripScreenshot.Name = "menuStripScreenshot";
            menuStripScreenshot.Size = new Size(469, 24);
            menuStripScreenshot.TabIndex = 6;
            menuStripScreenshot.Text = "menuStrip1";
            // 
            // sendImageToEditorToolStripMenuItem
            // 
            sendImageToEditorToolStripMenuItem.ForeColor = SystemColors.Window;
            sendImageToEditorToolStripMenuItem.Name = "sendImageToEditorToolStripMenuItem";
            sendImageToEditorToolStripMenuItem.Size = new Size(138, 20);
            sendImageToEditorToolStripMenuItem.Text = "Send Image to Editor...";
            sendImageToEditorToolStripMenuItem.Click += btnOpenEditor_Click;
            // 
            // copyToClipboardToolStripMenuItem
            // 
            copyToClipboardToolStripMenuItem.ForeColor = SystemColors.Window;
            copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            copyToClipboardToolStripMenuItem.Size = new Size(83, 20);
            copyToClipboardToolStripMenuItem.Text = "Copy Image";
            copyToClipboardToolStripMenuItem.Click += btnCopyScreenshot_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.ForeColor = SystemColors.Window;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(88, 20);
            saveToolStripMenuItem.Text = "Save Image...";
            saveToolStripMenuItem.Click += btnSaveScreenshot_Click;
            // 
            // exportAsMarkdownToolStripMenuItem
            // 
            exportAsMarkdownToolStripMenuItem.ForeColor = SystemColors.Window;
            exportAsMarkdownToolStripMenuItem.Name = "exportAsMarkdownToolStripMenuItem";
            exportAsMarkdownToolStripMenuItem.Size = new Size(152, 20);
            exportAsMarkdownToolStripMenuItem.Text = "Export All as Markdown...";
            exportAsMarkdownToolStripMenuItem.Click += btnExportMarkdown_Click;
            // 
            // SystemInfoCapture
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1426, 1188);
            Controls.Add(panelImageContainerSysteminfo);
            Controls.Add(btnHome);
            Controls.Add(txtSystemInfo);
            Controls.Add(pictureBox1);
            Controls.Add(menuStripTextBox);
            Controls.Add(menuStripScreenshot);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStripTextBox;
            Name = "SystemInfoCapture";
            Text = "ScreenGrab";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).EndInit();
            menuStripTextBox.ResumeLayout(false);
            menuStripTextBox.PerformLayout();
            panelImageContainerSysteminfo.ResumeLayout(false);
            menuStripScreenshot.ResumeLayout(false);
            menuStripScreenshot.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBoxScreenshot;
        private TextBox txtSystemInfo;
        private Button btnHome;
        private MenuStrip menuStripTextBox;
        private ToolStripMenuItem copyTextToolStripMenuItem;
        private Panel panelImageContainerSysteminfo;
        private MenuStrip menuStripScreenshot;
        private ToolStripMenuItem sendImageToEditorToolStripMenuItem;
        private ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem printTextOnImageToolStripMenuItem;
        private ToolStripMenuItem exportAsMarkdownToolStripMenuItem;
    }
}