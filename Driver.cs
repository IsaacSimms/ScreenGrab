///// Driver handles the shell, tray behavior, and wires the application together /////

// Namespaces
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
namespace ScreenGrab
{
    public partial class Driver : Form
    {
        private HotkeyScreenshot? _hotkeyScreenshot;        //wire up HotkeyScreenshot class
        public Driver()
        {
            InitializeComponent();

        }
        // override OnLoad to hide the form on startup - initialize HotkeyScreenshot class
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Hide();                                    //hide the form on startup
            this.ShowInTaskbar = false;                     //remove from taskbar
            _hotkeyScreenshot = new HotkeyScreenshot(this); //initialize HotkeyScreenshot class
            _hotkeyScreenshot.OnScreenshotTaken += HotkeyScreenshot_OnScreenshotTaken; //wire up event handler
        }
        // event handler for screenshot taken event
        private void HotkeyScreenshot_OnScreenshotTaken(string filePath)
        {
            ScreenshotMessageBox.ShowMessage(              // show message box on screenshot taken
                $"Screenshot saved to:\n{filePath}",       // message
                $"ScreenGrab",                             // title
                2000);                                     // duration in ms
        }
        // overide OnFormClosing to clean up resources on app closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _hotkeyScreenshot?.Dispose();                   //clean up HotkeyScreenshot class
            base.OnFormClosing(e);
        }
    }
}
