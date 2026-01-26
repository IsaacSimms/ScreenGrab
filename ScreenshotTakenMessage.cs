using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ScreenGrab
{
    public class ScreenshotMessageBox : Form
    {
        // == private variables == //
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Label _titleLabel;
        private readonly Label _messageLabel;

        // == Windows APIs for focus management == //
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // == make sure that the mesasge box does not steal focus from other applications, including other forms of the app == // // not necessarily required but improves defense
        // override ShowWithoutActivation to prevent stealing focus
        protected override bool ShowWithoutActivation => true;
        // override CreateParams to add WS_EX_NOACTIVATE style // works at low level to prevent focus stealing
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                CreateParams param = base.CreateParams;
                param.ExStyle |= WS_EX_NOACTIVATE;
                return param;
            }
        }

        // == constructor == //
        public ScreenshotMessageBox(string message, string title, int durationInMs)
        {
            // message box properties
            Text                 = title;                             // set title
            FormBorderStyle      = FormBorderStyle.FixedDialog;       // fixed dialog
            this.StartPosition   = FormStartPosition.Manual;          // manual position
            this.Location        = new Point(                         // position at bottom right of primary screen
                (Screen.PrimaryScreen?.WorkingArea.Right ?? 310) - 310,
                (Screen.PrimaryScreen?.WorkingArea.Bottom ?? 160) - 160);
            this.FormBorderStyle = FormBorderStyle.None;              // border set in overriden onPaint method
            BackColor            = Color.Black;                       //  background
            Size                 = new Size(300, 100);                // set size
            MaximizeBox          = false;                             // disable maximize box
            MinimizeBox          = false;                             // disable minimize box
            TopMost              = true;                              // always on top
            ShowInTaskbar        = false;                             // do not show in taskbar

            // title label
            _titleLabel = new Label
            {
                Text      = title,                                       // set title text
                AutoSize  = false,                                       // disable autosize
                Dock      = DockStyle.Top,                               // dock at top
                Height    = 30,                                          // fixed height for title
                TextAlign = ContentAlignment.MiddleCenter,               // center text
                Font      = new Font("Segoe UI", 14, FontStyle.Bold),    // set font (bold for title)
                ForeColor = Color.White                                  // set text color
            };
            Controls.Add(_titleLabel);

            // message label w/ styling
            _messageLabel = new Label
            {
                Text      = message,                                     // set message text
                AutoSize  = false,                                       // disable autosize
                Dock      = DockStyle.Fill,                              // fill remaining space
                TextAlign = ContentAlignment.MiddleCenter,               // center text
                Font      = new Font("Segoe UI", 12, FontStyle.Regular), // set font 
                ForeColor = Color.White                                  // set text color
            };
            Controls.Add(_messageLabel);

            // timer to close message box after duration (duration set in class constructor)
            _timer          = new System.Windows.Forms.Timer();
            _timer.Interval = durationInMs;
            _timer.Tick    += (s, e) =>
            {
                _timer.Stop();
                Close();
            };
        }
        // == override OnShown to start timer == //
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _timer.Start();
        }

        // == static method to show message box == //
        public static void ShowMessage(string message, string title, int durationInMs)
        {
            // small delay to allow the calling form to fully activate and receive focus first
            System.Threading.Thread.Sleep(500);
            IntPtr previousForegroundWindow = GetForegroundWindow();          // get current foreground window
            var box = new ScreenshotMessageBox(message, title, durationInMs); // create message box instance
            box.Show();

            // restore focus 
            if (previousForegroundWindow != IntPtr.Zero)
            {
                SetForegroundWindow(previousForegroundWindow);
            }
        }
    }
}
