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

        private HotkeyScreenshot? _hotkeyScreenshot;             // wire up HotkeyScreenshot class
        private static HotkeyConfig _hotkeyConfig = new HotkeyConfig(); // hotkey configuration instance
        public Driver()
        {
            InitializeComponent();
        }
        // override OnLoad to hide the form on startup - initialize
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Hide();                                                               // hide the form on startup
            this.ShowInTaskbar = false;                               // remove from taskbar
            _hotkeyScreenshot = new HotkeyScreenshot(this, _hotkeyConfig);              // initialize HotkeyScreenshot class
            _hotkeyScreenshot.OnScreenshotTaken += HotkeyScreenshot_OnScreenshotTaken; // wire up event handler

            // set up system tray icon
            SystemTrayIcon.Visible = true;                                     // make icon visible
            SystemTrayIcon.BalloonTipTitle = "ScreenGrab";                             // set balloon tip title
            SystemTrayIcon.DoubleClick += SystemTrayIcon_DoubleClick;              // wire up double click event
            BuildTrayMenu();                                                           // build tray menu
        }
        private void BuildTrayMenu()
        {
            menuOpen.Click += MenuOpen_Click;
            menuExit.Click += MenuExit_Click;
            SystemTrayIcon.ContextMenuStrip = SystemTrayMenu;                          // assign context menu to tray icon
        }
        private void SystemTrayIcon_DoubleClick(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }
        private void MenuOpen_Click(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }
        private void MenuExit_Click(object? sender, EventArgs e)
        {
            //clean up and exit application
            _hotkeyScreenshot?.Dispose();                   // clean up HotkeyScreenshot class
            _hotkeyScreenshot = null;                  // set to null
            SystemTrayIcon.Visible = false;                 // hide tray icon
            Application.Exit();
        }
        private void ShowMainWindow()
        {
            this.Show();                                   // show the form
            this.WindowState = FormWindowState.Normal;   // set window state to normal
            this.ShowInTaskbar = true;                     // show in taskbar
            this.Activate();                               // bring to foreground
        }

        // event handler for screenshot taken event
        private void HotkeyScreenshot_OnScreenshotTaken(string filePath)
        {
            ScreenshotMessageBox.ShowMessage(              // show message box on screenshot taken
                $"ScreenGrab saved to:\n{filePath}",       // message
                $"ScreenGrab",                             // title //not displaying in current config
                4000);                                     // duration in ms
        }
        // overide OnFormClosing to clean up resources on app closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // if user clicks X on form, minimize to tray instead of closing
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.ShowInTaskbar = false;
                return;

            }
            _hotkeyScreenshot?.Dispose();                  // clean up HotkeyScreenshot class
            _hotkeyScreenshot = null;                      // set to null
            base.OnFormClosing(e);                         // call base method
        }
        // when button is clicked open up settings form (SendToSettings button)
        private void button1_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(_hotkeyConfig);
            settingsForm.Show();
        }
    }
}
