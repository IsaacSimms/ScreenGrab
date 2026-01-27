namespace ScreenGrab
{
    partial class OCRScreenshotForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OCRScreenshotForm));
            OCRForm = new PictureBox();
            pictureBoxScreenshot = new PictureBox();
            panelImageContainerOCR = new Panel();
            menuStrip2 = new MenuStrip();
            copyTextToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            sendImageToEditorToolStripMenuItem = new ToolStripMenuItem();
            copyImageToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exportAllAsMarkdownToolStripMenuItem = new ToolStripMenuItem();
            txtOcrResult = new TextBox();
            btnHome = new Button();
            ((System.ComponentModel.ISupportInitialize)OCRForm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).BeginInit();
            panelImageContainerOCR.SuspendLayout();
            menuStrip2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // OCRForm
            // 
            OCRForm.Image = (Image)resources.GetObject("OCRForm.Image");
            OCRForm.Location = new Point(12, 12);
            OCRForm.Name = "OCRForm";
            OCRForm.Size = new Size(292, 77);
            OCRForm.TabIndex = 0;
            OCRForm.TabStop = false;
            // 
            // pictureBoxScreenshot
            // 
            pictureBoxScreenshot.Dock = DockStyle.Fill;
            pictureBoxScreenshot.Location = new Point(0, 0);
            pictureBoxScreenshot.Name = "pictureBoxScreenshot";
            pictureBoxScreenshot.Size = new Size(1402, 699);
            pictureBoxScreenshot.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxScreenshot.TabIndex = 1;
            pictureBoxScreenshot.TabStop = false;
            // 
            // panelImageContainerOCR
            // 
            panelImageContainerOCR.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelImageContainerOCR.Controls.Add(pictureBoxScreenshot);
            panelImageContainerOCR.Location = new Point(12, 95);
            panelImageContainerOCR.Name = "panelImageContainerOCR";
            panelImageContainerOCR.Size = new Size(1402, 699);
            panelImageContainerOCR.TabIndex = 2;
            // 
            // menuStrip2
            // 
            menuStrip2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            menuStrip2.BackColor = SystemColors.Desktop;
            menuStrip2.Dock = DockStyle.None;
            menuStrip2.GripStyle = ToolStripGripStyle.Visible;
            menuStrip2.Items.AddRange(new ToolStripItem[] { copyTextToolStripMenuItem });
            menuStrip2.Location = new Point(1334, 800);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.RenderMode = ToolStripRenderMode.System;
            menuStrip2.Size = new Size(83, 24);
            menuStrip2.TabIndex = 5;
            menuStrip2.Text = "menuStripText";
            // 
            // copyTextToolStripMenuItem
            // 
            copyTextToolStripMenuItem.ForeColor = SystemColors.Window;
            copyTextToolStripMenuItem.Name = "copyTextToolStripMenuItem";
            copyTextToolStripMenuItem.Size = new Size(71, 20);
            copyTextToolStripMenuItem.Text = "Copy Text";
            copyTextToolStripMenuItem.Click += btnCopyText_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            menuStrip1.BackColor = SystemColors.Desktop;
            menuStrip1.Dock = DockStyle.None;
            menuStrip1.GripStyle = ToolStripGripStyle.Visible;
            menuStrip1.Items.AddRange(new ToolStripItem[] { sendImageToEditorToolStripMenuItem, copyImageToolStripMenuItem, saveToolStripMenuItem, exportAllAsMarkdownToolStripMenuItem });
            menuStrip1.Location = new Point(941, 68);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.RenderMode = ToolStripRenderMode.System;
            menuStrip1.Size = new Size(473, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStripScreenshot";
            // 
            // sendImageToEditorToolStripMenuItem
            // 
            sendImageToEditorToolStripMenuItem.ForeColor = SystemColors.Window;
            sendImageToEditorToolStripMenuItem.Name = "sendImageToEditorToolStripMenuItem";
            sendImageToEditorToolStripMenuItem.Size = new Size(138, 20);
            sendImageToEditorToolStripMenuItem.Text = "Send Image to Editor...";
            sendImageToEditorToolStripMenuItem.Click += btnOpenEditor_Click;
            // 
            // copyImageToolStripMenuItem
            // 
            copyImageToolStripMenuItem.ForeColor = SystemColors.Window;
            copyImageToolStripMenuItem.Name = "copyImageToolStripMenuItem";
            copyImageToolStripMenuItem.Size = new Size(83, 20);
            copyImageToolStripMenuItem.Text = "Copy Image";
            copyImageToolStripMenuItem.Click += btnCopyScreenshot_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.ForeColor = SystemColors.Window;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(88, 20);
            saveToolStripMenuItem.Text = "Save Image...";
            saveToolStripMenuItem.Click += btnSaveScreenshot_Click;
            // 
            // exportAllAsMarkdownToolStripMenuItem
            // 
            exportAllAsMarkdownToolStripMenuItem.ForeColor = SystemColors.Window;
            exportAllAsMarkdownToolStripMenuItem.Name = "exportAllAsMarkdownToolStripMenuItem";
            exportAllAsMarkdownToolStripMenuItem.Size = new Size(152, 20);
            exportAllAsMarkdownToolStripMenuItem.Text = "Export All as Markdown...";
            exportAllAsMarkdownToolStripMenuItem.Click += btnExportMarkdown_Click;
            // 
            // txtOcrResult
            // 
            txtOcrResult.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtOcrResult.BackColor = SystemColors.Desktop;
            txtOcrResult.BorderStyle = BorderStyle.FixedSingle;
            txtOcrResult.ForeColor = SystemColors.Window;
            txtOcrResult.Location = new Point(15, 827);
            txtOcrResult.Multiline = true;
            txtOcrResult.Name = "txtOcrResult";
            txtOcrResult.ScrollBars = ScrollBars.Vertical;
            txtOcrResult.Size = new Size(1402, 349);
            txtOcrResult.TabIndex = 4;
            // 
            // btnHome
            // 
            btnHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnHome.BackColor = SystemColors.Desktop;
            btnHome.ForeColor = SystemColors.Window;
            btnHome.Location = new Point(1339, 12);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(77, 40);
            btnHome.TabIndex = 6;
            btnHome.Text = "Home";
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnGoHome_Click;
            // 
            // OCRScreenshotForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1426, 1187);
            Controls.Add(menuStrip2);
            Controls.Add(btnHome);
            Controls.Add(txtOcrResult);
            Controls.Add(panelImageContainerOCR);
            Controls.Add(OCRForm);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "OCRScreenshotForm";
            Text = " ScreenGrab";
            ((System.ComponentModel.ISupportInitialize)OCRForm).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).EndInit();
            panelImageContainerOCR.ResumeLayout(false);
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox OCRForm;
        private PictureBox pictureBoxScreenshot;
        private Panel panelImageContainerOCR;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem sendImageToEditorToolStripMenuItem;
        private ToolStripMenuItem copyImageToolStripMenuItem;
        private ToolStripMenuItem exportAllAsMarkdownToolStripMenuItem;
        private TextBox txtOcrResult;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem copyTextToolStripMenuItem;
        private Button btnHome;
    }
}