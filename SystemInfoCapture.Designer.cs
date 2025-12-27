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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).BeginInit();
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
            pictureBoxScreenshot.Location = new Point(12, 88);
            pictureBoxScreenshot.Name = "pictureBoxScreenshot";
            pictureBoxScreenshot.Size = new Size(1402, 700);
            pictureBoxScreenshot.TabIndex = 1;
            pictureBoxScreenshot.TabStop = false;
            // 
            // txtSystemInfo
            // 
            txtSystemInfo.BackColor = SystemColors.Desktop;
            txtSystemInfo.BorderStyle = BorderStyle.FixedSingle;
            txtSystemInfo.ForeColor = SystemColors.Window;
            txtSystemInfo.Location = new Point(12, 825);
            txtSystemInfo.Name = "txtSystemInfo";
            txtSystemInfo.Size = new Size(1402, 23);
            txtSystemInfo.TabIndex = 2;
            // 
            // btnHome
            // 
            btnHome.BackColor = SystemColors.Desktop;
            btnHome.ForeColor = SystemColors.Window;
            btnHome.Location = new Point(1337, 12);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(77, 40);
            btnHome.TabIndex = 3;
            btnHome.Text = "Home";
            btnHome.UseVisualStyleBackColor = false;
            // 
            // SystemInfoCapture
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Desktop;
            ClientSize = new Size(1426, 1188);
            Controls.Add(btnHome);
            Controls.Add(txtSystemInfo);
            Controls.Add(pictureBoxScreenshot);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "SystemInfoCapture";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxScreenshot).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBoxScreenshot;
        private TextBox txtSystemInfo;
        private Button btnHome;
    }
}