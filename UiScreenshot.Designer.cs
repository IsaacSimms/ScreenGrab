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
            ((System.ComponentModel.ISupportInitialize)pictureBoxUICaptureHeader).BeginInit();
            pnlScreenshot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).BeginInit();
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
            // UiScreenshot
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1426, 1188);
            Controls.Add(textBox1);
            Controls.Add(pnlScreenshot);
            Controls.Add(pictureBoxUICaptureHeader);
            Name = "UiScreenshot";
            Text = "ScreenGrab";
            ((System.ComponentModel.ISupportInitialize)pictureBoxUICaptureHeader).EndInit();
            pnlScreenshot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxUICaptureHeader;
        private Panel pnlScreenshot;
        private PictureBox pictureBoxScreenshot;
        private TextBox textBox1;
    }
}