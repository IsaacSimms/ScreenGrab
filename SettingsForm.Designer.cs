namespace ScreenGrab
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            txtActive = new TextBox();
            txtRegion = new TextBox();
            SuspendLayout();
            // 
            // txtActive
            // 
            txtActive.Location = new Point(35, 119);
            txtActive.Name = "txtActive";
            txtActive.Size = new Size(90, 23);
            txtActive.TabIndex = 0;
            txtActive.TextChanged += txtActive_TextChanged;
            // 
            // txtRegion
            // 
            txtRegion.Location = new Point(25, 172);
            txtRegion.Name = "txtRegion";
            txtRegion.Size = new Size(100, 23);
            txtRegion.TabIndex = 1;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(1226, 893);
            Controls.Add(txtRegion);
            Controls.Add(txtActive);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SettingsForm";
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtActive;
        private TextBox txtRegion;
    }
}