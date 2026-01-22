namespace ScreenGrab
{
    partial class UiScreenshot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UiScreenshot));
            pictureBoxUICaptureHeader = new PictureBox();
            pnlScreenshot = new Panel();
            pictureBoxScreenshot = new PictureBox();
            textBox1 = new TextBox();
            btnSendHome = new Button();
            menuStrip1 = new MenuStrip();
            sendImageToEditorToolStripMenuItem = new ToolStripMenuItem();
            copyImageToolStripMenuItem = new ToolStripMenuItem();
            saveImageToolStripMenuItem = new ToolStripMenuItem();
            exportAllAsMarkdownToolStripMenuItem = new ToolStripMenuItem();
            menuStrip2 = new MenuStrip();
            copyTextToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)pictureBoxUICaptureHeader).BeginInit();
            pnlScreenshot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).BeginInit();
            menuStrip1.SuspendLayout();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxUICaptureHeader
            // 
            pictureBoxUICaptureHeader.Image = (Image)resources.GetObject("pictureBoxUICaptureHeader.Image");
            pictureBoxUICaptureHeader.Location = new Point(12, 12);
            pictureBoxUICaptureHeader.Name = "pictureBoxUICaptureHeader";
            pictureBoxUICaptureHeader.Size = new Size(239, 72);
            pictureBoxUICaptureHeader.TabIndex = 0;
            pictureBoxUICaptureHeader.TabStop = false;
            // 
            // pnlScreenshot
            // 
            pnlScreenshot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlScreenshot.Controls.Add(pictureBoxScreenshot);
            pnlScreenshot.Location = new Point(12, 88);
            pnlScreenshot.Name = "pnlScreenshot";
            pnlScreenshot.Size = new Size(1402, 700);
            pnlScreenshot.TabIndex = 2;
            // 
            // pictureBoxScreenshot
            // 
            pictureBoxScreenshot.Dock = DockStyle.Fill;
            pictureBoxScreenshot.Location = new Point(0, 0);
            pictureBoxScreenshot.Name = "pictureBoxScreenshot";
            pictureBoxScreenshot.Size = new Size(1402, 700);
            pictureBoxScreenshot.TabIndex = 1;
            pictureBoxScreenshot.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BackColor = SystemColors.Desktop;
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Location = new Point(13, 818);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(1402, 358);
            textBox1.TabIndex = 3;
            // 
            // btnSendHome
            // 
            btnSendHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSendHome.BackColor = SystemColors.Desktop;
            btnSendHome.ForeColor = SystemColors.Window;
            btnSendHome.Location = new Point(1339, 12);
            btnSendHome.Name = "btnSendHome";
            btnSendHome.Size = new Size(77, 40);
            btnSendHome.TabIndex = 4;
            btnSendHome.Text = "Home";
            btnSendHome.UseVisualStyleBackColor = false;
            btnSendHome.Click += BtnSendHome_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            menuStrip1.BackColor = SystemColors.Desktop;
            menuStrip1.Dock = DockStyle.None;
            menuStrip1.Items.AddRange(new ToolStripItem[] { sendImageToEditorToolStripMenuItem, copyImageToolStripMenuItem, saveImageToolStripMenuItem, exportAllAsMarkdownToolStripMenuItem });
            menuStrip1.Location = new Point(944, 58);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(470, 24);
            menuStrip1.TabIndex = 5;
            menuStrip1.Text = "menuStrip1";
            // 
            // sendImageToEditorToolStripMenuItem
            // 
            sendImageToEditorToolStripMenuItem.BackColor = SystemColors.Desktop;
            sendImageToEditorToolStripMenuItem.ForeColor = SystemColors.Window;
            sendImageToEditorToolStripMenuItem.Name = "sendImageToEditorToolStripMenuItem";
            sendImageToEditorToolStripMenuItem.Size = new Size(138, 20);
            sendImageToEditorToolStripMenuItem.Text = "Send Image to Editor...";
            sendImageToEditorToolStripMenuItem.Click += SendImageToEditorToolStripMenuItem_Click;
            // 
            // copyImageToolStripMenuItem
            // 
            copyImageToolStripMenuItem.ForeColor = SystemColors.Window;
            copyImageToolStripMenuItem.Name = "copyImageToolStripMenuItem";
            copyImageToolStripMenuItem.Size = new Size(83, 20);
            copyImageToolStripMenuItem.Text = "Copy Image";
            copyImageToolStripMenuItem.Click += CopyTextToolStripMenuItem_Click;
            // 
            // saveImageToolStripMenuItem
            // 
            saveImageToolStripMenuItem.ForeColor = SystemColors.Window;
            saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            saveImageToolStripMenuItem.Size = new Size(88, 20);
            saveImageToolStripMenuItem.Text = "Save Image...";
            saveImageToolStripMenuItem.Click += SaveImageToolStripMenuItem_Click;
            // 
            // exportAllAsMarkdownToolStripMenuItem
            // 
            exportAllAsMarkdownToolStripMenuItem.ForeColor = SystemColors.Window;
            exportAllAsMarkdownToolStripMenuItem.Name = "exportAllAsMarkdownToolStripMenuItem";
            exportAllAsMarkdownToolStripMenuItem.Size = new Size(153, 20);
            exportAllAsMarkdownToolStripMenuItem.Text = "Export All as Markdown...";
            exportAllAsMarkdownToolStripMenuItem.Click += ExportAllAsMarkdownToolStripMenuItem_Click;
            // 
            // menuStrip2
            // 
            menuStrip2.BackColor = SystemColors.Desktop;
            menuStrip2.Dock = DockStyle.None;
            menuStrip2.Items.AddRange(new ToolStripItem[] { copyTextToolStripMenuItem });
            menuStrip2.Location = new Point(1339, 791);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(79, 24);
            menuStrip2.TabIndex = 6;
            menuStrip2.Text = "menuStrip2";
            // 
            // copyTextToolStripMenuItem
            // 
            copyTextToolStripMenuItem.ForeColor = SystemColors.Window;
            copyTextToolStripMenuItem.Name = "copyTextToolStripMenuItem";
            copyTextToolStripMenuItem.Size = new Size(71, 20);
            copyTextToolStripMenuItem.Text = "Copy Text";
            copyTextToolStripMenuItem.Click += CopyTextToolStripMenuItem_Click;
            // 
            // UiScreenshot
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1426, 1188);
            Controls.Add(btnSendHome);
            Controls.Add(textBox1);
            Controls.Add(pnlScreenshot);
            Controls.Add(pictureBoxUICaptureHeader);
            Controls.Add(menuStrip1);
            Controls.Add(menuStrip2);
            MainMenuStrip = menuStrip1;
            Name = "UiScreenshot";
            Text = "ScreenGrab";
            ((System.ComponentModel.ISupportInitialize)pictureBoxUICaptureHeader).EndInit();
            pnlScreenshot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxUICaptureHeader;
        private Panel pnlScreenshot;
        private PictureBox pictureBoxScreenshot;
        private TextBox textBox1;
        private Button btnSendHome;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem sendImageToEditorToolStripMenuItem;
        private ToolStripMenuItem copyImageToolStripMenuItem;
        private ToolStripMenuItem saveImageToolStripMenuItem;
        private ToolStripMenuItem exportAllAsMarkdownToolStripMenuItem;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem copyTextToolStripMenuItem;
    }
}