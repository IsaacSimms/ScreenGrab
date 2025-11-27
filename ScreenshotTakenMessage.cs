using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenGrab
{
    public class ScreenshotMessageBox : Form
    {
        // variables 
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Label _label;

        // constructor
        public ScreenshotMessageBox(string mesasge, string title, int durationInMs)
        {
            // message box properties
            Text = title;                                    // set title
            FormBorderStyle = FormBorderStyle.FixedDialog;   // fixed dialog
            StartPosition = FormStartPosition.CenterScreen;  // center screen
            BackColor = Color.White;                         //  background
            Size = new Size(300, 150);                       // set size
            MaximizeBox = false;                             // disable maximize box
            MinimizeBox = false;                             // disable minimize box
            TopMost = true;                                  // always on top
            // label for message w/ styling
            _label = new Label
            {
                Text = mesasge,                                    // set message text
                AutoSize = false,                                  // disable autosize
                Dock = DockStyle.Fill,                             // fill form
                TextAlign = ContentAlignment.MiddleCenter,         // center text
                Font = new Font("Segoe UI", 12, FontStyle.Regular) // set font 
            };
            Controls.Add(_label);
            // timer to close message box after duration (duration set in class constructor)
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = durationInMs;
            _timer.Tick += (s, e) =>
            {
                _timer.Stop();
                Close();
            };
        }
        // override OnShown to start timer
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _timer.Start();
        }
        // static method to show message box 
        public static void ShowMessage(string message, string title, int durationInMs)
        {
            var box = new ScreenshotMessageBox(message, title, durationInMs);
            box.Show();
        }
    }
}
