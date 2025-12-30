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
            menuStrip1 = new MenuStrip();
            sendImageToEditorToolStripMenuItem = new ToolStripMenuItem();
            copyImageToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exportAllAsMarkdownToolStripMenuItem = new ToolStripMenuItem();
            txtOcrResult = new TextBox();
            menuStrip2 = new MenuStrip();
            copyTextToolStripMenuItem = new ToolStripMenuItem();
            btnHome = new Button();
            ((System.ComponentModel.ISupportInitialize)OCRForm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).BeginInit();
            menuStrip1.SuspendLayout();
            menuStrip2.SuspendLayout();
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
            pictureBoxScreenshot.Location = new Point(12, 98);
            pictureBoxScreenshot.Name = "pictureBoxScreenshot";
            pictureBoxScreenshot.Size = new Size(1402, 703);
            pictureBoxScreenshot.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxScreenshot.TabIndex = 1;
            pictureBoxScreenshot.TabStop = false;
            // 
            // panelImageContainerOCR
            // 
            panelImageContainerOCR.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelImageContainerOCR.Location = new Point(12, 95);
            panelImageContainerOCR.Name = "panelImageContainerOCR";
            panelImageContainerOCR.Size = new Size(1402, 708);
            panelImageContainerOCR.TabIndex = 2;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Desktop;
            menuStrip1.Dock = DockStyle.None;
            menuStrip1.Items.AddRange(new ToolStripItem[] { sendImageToEditorToolStripMenuItem, copyImageToolStripMenuItem, saveToolStripMenuItem, exportAllAsMarkdownToolStripMenuItem });
            menuStrip1.Location = new Point(948, 65);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(470, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStripScreenshot";
            // 
            // sendImageToEditorToolStripMenuItem
            // 
            sendImageToEditorToolStripMenuItem.ForeColor = SystemColors.Window;
            sendImageToEditorToolStripMenuItem.Name = "sendImageToEditorToolStripMenuItem";
            sendImageToEditorToolStripMenuItem.Size = new Size(138, 20);
            sendImageToEditorToolStripMenuItem.Text = "Send Image to Editor...";
            // 
            // copyImageToolStripMenuItem
            // 
            copyImageToolStripMenuItem.ForeColor = SystemColors.Window;
            copyImageToolStripMenuItem.Name = "copyImageToolStripMenuItem";
            copyImageToolStripMenuItem.Size = new Size(83, 20);
            copyImageToolStripMenuItem.Text = "Copy Image";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.ForeColor = SystemColors.Window;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(88, 20);
            saveToolStripMenuItem.Text = "Save Image...";
            // 
            // exportAllAsMarkdownToolStripMenuItem
            // 
            exportAllAsMarkdownToolStripMenuItem.ForeColor = SystemColors.Window;
            exportAllAsMarkdownToolStripMenuItem.Name = "exportAllAsMarkdownToolStripMenuItem";
            exportAllAsMarkdownToolStripMenuItem.Size = new Size(153, 20);
            exportAllAsMarkdownToolStripMenuItem.Text = "Export All as Markdown...";
            // 
            // txtOcrResult
            // 
            txtOcrResult.BackColor = SystemColors.Desktop;
            txtOcrResult.BorderStyle = BorderStyle.FixedSingle;
            txtOcrResult.ForeColor = SystemColors.Window;
            txtOcrResult.Location = new Point(15, 827);
            txtOcrResult.Multiline = true;
            txtOcrResult.Name = "txtOcrResult";
            txtOcrResult.ScrollBars = ScrollBars.Vertical;
            txtOcrResult.Size = new Size(1402, 358);
            txtOcrResult.TabIndex = 4;
            // 
            // menuStrip2
            // 
            menuStrip2.BackColor = SystemColors.Desktop;
            menuStrip2.Dock = DockStyle.None;
            menuStrip2.Items.AddRange(new ToolStripItem[] { copyTextToolStripMenuItem });
            menuStrip2.Location = new Point(1346, 800);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(79, 24);
            menuStrip2.TabIndex = 5;
            menuStrip2.Text = "menuStripText";
            // 
            // copyTextToolStripMenuItem
            // 
            copyTextToolStripMenuItem.ForeColor = SystemColors.Window;
            copyTextToolStripMenuItem.Name = "copyTextToolStripMenuItem";
            copyTextToolStripMenuItem.Size = new Size(71, 20);
            copyTextToolStripMenuItem.Text = "Copy Text";
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
            // 
            // OCRScreenshotForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1426, 1196);
            Controls.Add(btnHome);
            Controls.Add(pictureBoxScreenshot);
            Controls.Add(txtOcrResult);
            Controls.Add(panelImageContainerOCR);
            Controls.Add(OCRForm);
            Controls.Add(menuStrip1);
            Controls.Add(menuStrip2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "OCRScreenshotForm";
            Text = " ";
            ((System.ComponentModel.ISupportInitialize)OCRForm).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
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