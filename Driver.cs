///// Driver handles the shell, tray behavior, and wires the application together /////

// Namespaces
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using Windows.Devices.AllJoyn;
using System.Threading.Tasks;
namespace ScreenGrab
{
    public partial class Driver : Form
    {

        // == Class Variables == //
        private HotkeyScreenshot? _hotkeyScreenshot; // wire up HotkeyScreenshot class variable
        private HotkeyConfig _hotkeyConfig;          // wir up hotkey configuration class variable

        // == constructor == //
        public Driver()
        {
            InitializeComponent();
            _hotkeyConfig = ConfigurationManager.LoadConfiguration(); // load hotkey configuration from json file
            this.KeyPreview = true;                                   // enable key preview for esc key handling
            this.KeyDown += Driver_KeyDown;                           // wire up keydown event for esc key handling
        }

        // == ovveride OnHandleCreated to account for hotkey registration after handle is created == //
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // error handling
            if (_hotkeyScreenshot != null)
            {
                _hotkeyScreenshot.AttachToHandle(this.Handle);
            }
        }

        // == override OnLoad to hide the form on startup - initialize == //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Hide();                                                                     // hide the form on startup
            this.ShowInTaskbar = false;                                                      // remove from taskbar
            _hotkeyScreenshot = new HotkeyScreenshot(this, _hotkeyConfig);                   // initialize HotkeyScreenshot class
            _hotkeyScreenshot.OnScreenshotTaken += HotkeyScreenshot_OnScreenshotTaken;       // wire up event handler for screenshot taken
            _hotkeyScreenshot.OnScreenshotCaptured += HotkeyScreenshot_OnScreenshotCaptured; // wire up event handler for screenshot captured
            _hotkeyScreenshot.OnOpenEditor += OpenImageEditor;                               // wire up event handler for open image editor

            // set up system tray icon
            SystemTrayIcon.Visible = true;                                         // make icon visible
            SystemTrayIcon.BalloonTipTitle = "ScreenGrab";                         // set balloon tip title
            SystemTrayIcon.DoubleClick += SystemTrayIcon_DoubleClick;              // wire up double click event
            BuildTrayMenu();                                                       // build tray menu
        }

        // == Methods for system tray menu == //
        private void BuildTrayMenu()
        {
            menuOpen.Click += MenuOpen_Click;
            menuExit.Click += MenuExit_Click;
            SystemTrayIcon.ContextMenuStrip = SystemTrayMenu;                      // assign context menu to tray icon
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
            _hotkeyScreenshot = null;                       // set to null
            SystemTrayIcon.Visible = false;                 // hide tray icon
            Application.Exit();
        }
        // == end methods for system tray menu == //

        // == showing main(driver) window called by other classes == //
        private void ShowMainWindow()
        {
            this.Show();                                   // show the form
            this.WindowState = FormWindowState.Normal;     // set window state to normal
            this.ShowInTaskbar = true;                     // show in taskbar
            this.Activate();                               // bring to foreground
        }

        // == event handler for screenshot taken event == //
        private void HotkeyScreenshot_OnScreenshotTaken(string filePath)
        {
            ScreenshotMessageBox.ShowMessage( // show message
                $"ScreenGrab saved to:\n{filePath}",
                $"ScreenGrab",
                4000);
        }

        // == Methods for opening image editor with bitmap == //
        // event handler for screenshot captured event
        private void HotkeyScreenshot_OnScreenshotCaptured(Bitmap screenshot)
        {
            //auto open image editor if configured
            if (_hotkeyConfig.AutoOpenEditorOnCapture)
            {
                OpenEditorWithBitmap(screenshot);
            }
            else
            {
                screenshot.Dispose(); // clean up bitmap if not used
            }
        }
        // open image editor with bitmap
        private void OpenEditorWithBitmap(Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Bitmap>(OpenEditorWithBitmap), bitmap);
                return;
            }
            var imageEditorForm = new ImageEditorForm(bitmap)
            {
                Tag = this, // set owner so ImageEditorForm can return to this instance instead of creating a new Driver // This is used in ImageEditorForm.cs for graceful transitions
                ShowInTaskbar = true
            };
            imageEditorForm.Show();
        }
        // == end methods for opening image editor with bitmap == //




        // == open image editor with file path == //
        private void OpenEditorWithFile(string filePath)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OpenEditorWithFile), filePath);
                return;
            }
            var imageEditorForm = new ImageEditorForm(filePath)
            {
                Tag = this, // set owner so ImageEditorForm can return to this instance instead of creating a new Driver // This is used in ImageEditorForm.cs for graceful transitions
                ShowInTaskbar = true
            };
            imageEditorForm.Show();
        }

        // == event handler for open image editor event == // // for opening editor from hotkey
        private void OpenImageEditor()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OpenImageEditor));
                return;
            }
            // check if clipboard has image
            ImageEditorForm imageEditorForm;
            if (Clipboard.ContainsImage())
            {
                Image? clipboardImage = Clipboard.GetImage();
                if (clipboardImage != null)
                {
                    imageEditorForm = new ImageEditorForm(clipboardImage);
                }
                else
                {
                    imageEditorForm = new ImageEditorForm();
                    ScreenshotMessageBox.ShowMessage( // show message
                        $"There is not a .png saved to clipboard",
                        $"ScreenGrab:",
                        4000);
                }
            }
            else
            {
                imageEditorForm = new ImageEditorForm();
                ScreenshotMessageBox.ShowMessage( // show message
                     $"There is not a .png saved to clipboard",
                     $"ScreenGrab:",
                     4000);
            }

            imageEditorForm.Tag = this; // set owner so ImageEditorForm can return to this instance instead of creating a new Driver // This is used in ImageEditorForm.cs for graceful transitions
            imageEditorForm.ShowInTaskbar = true;
            imageEditorForm.Show();
        }

        // == overide OnFormClosing to clean up resources on app closing == //
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
        // == when button is clicked open up settings form  //additional logic for registering hotkeys after change == //
        private void SendToSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm(_hotkeyConfig);
            settingsForm.Tag = this;  // set owner so SettingsForm can return to this instance instead of creating a new Driver // This is used in SetingsForm.cs for graceful transitions
            settingsForm.HotkeysChanged += config =>
            {
                _hotkeyConfig = config;
                _hotkeyScreenshot?.UpdateHotkeyConfig(_hotkeyConfig);
                ConfigurationManager.SaveConfiguration(_hotkeyConfig);
            };
            settingsForm.SaveFileLocationChanged += config =>
            {
                _hotkeyConfig = config;
                ConfigurationManager.SaveConfiguration(_hotkeyConfig);
            };
            settingsForm.ShowInTaskbar = true;
            settingsForm.Show();
            this.Hide();                // hide the main form // keep persistent state
            this.ShowInTaskbar = false;
        }

        // == FOR SENDING TO SETTIGNS FROM EDITOR == // // required becuase ImageEditorForm cannot access settings instance directly like driver can
        public HotkeyConfig GetHotkeyConfig()
        {
            return _hotkeyConfig;
        }
        public void UpdateHotkeyConfig(HotkeyConfig config)
        {
            _hotkeyConfig = config;
            _hotkeyScreenshot?.UpdateHotkeyConfig(_hotkeyConfig);
            ConfigurationManager.SaveConfiguration(_hotkeyConfig);
        }
        // == END FOR SENDING TO SETTINGS FROM EDITOR == //

        // == when donate button is clicked, open donate link == //
        private void btnDonate_Click(object sender, EventArgs e)
        {
            try
            {
                string donateUrl = "https://isaacsimms.github.io/ScreenGrab-Website/Support";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = donateUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open link. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // == when button is clicked, open Image Editor form == // // for opening editor from button
        private void SendToEditor_Click(object sender, EventArgs e)
        {
            ImageEditorForm imageEditorForm;
            if (Clipboard.ContainsImage())
            {
                Image? clipboardImage = Clipboard.GetImage();
                if (clipboardImage != null)
                {
                    imageEditorForm = new ImageEditorForm(clipboardImage);
                }
                else
                {
                    imageEditorForm = new ImageEditorForm();
                    ScreenshotMessageBox.ShowMessage(
                        $"There is not a .png saved to clipboard",
                        $"ScreenGrab:",
                        4000);
                }
            }
            else
            {
                imageEditorForm = new ImageEditorForm();
                ScreenshotMessageBox.ShowMessage(
                    $"There is not a .png saved to clipboard",
                    $"ScreenGrab:",
                    4000);
            }
            imageEditorForm.Tag = this; // set owner so ImageEditorForm can return to this instance instead of creating a new Driver // This is used in ImageEditorForm.cs for graceful transitions
            imageEditorForm.ShowInTaskbar = true;
            imageEditorForm.Show();
            this.Hide();                // hide the main form // keep persistent state
            this.ShowInTaskbar = false;
        }

        // == click button to launch UI element class // create instance each time to avoid reuse problems == //
        private void btnUiElementCapture_Click(object sender, EventArgs e)
        {
            var uiScreenshotForm = new UiScreenshot
            {
                Tag = this,
                ShowInTaskbar = true
            };
            uiScreenshotForm.Show();
            uiScreenshotForm.StartUiElementCapture();
            this.Hide();
            this.ShowInTaskbar = false;
        }

        // == when button is clicked, take a screenshot of active window == //
        private async void activeWindowScreenshotButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            await Task.Delay(150); // slight delay to ensure screenshot process starts before hiding the main window
            _hotkeyScreenshot?.CaptureActiveWindow();

        }

        // == when button is clicked, take a screenshot of region == //
        private async void regionScreenshotButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            await Task.Delay(100); // slight delay to ensure screenshot process starts before hiding the main window
            _hotkeyScreenshot?.CaptureRegion();


        }
        // == when button is clicked, take OCR screenshot of region == //
        private async void ocrRegionScreenshotButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            await Task.Delay(100); // slight delay to ensure screenshot process starts before hiding the main window
            _hotkeyScreenshot?.CaptureOcrRegion();
        }

        // ==when button is clicked, take a freeform screenshot == //
        private async void freeformScreenshotButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            await Task.Delay(100); // slight delay to ensure screenshot process starts before hiding the main window
            _hotkeyScreenshot?.CaptureFreeform();

        }

        // == when button is clicked, take a delayed screenshot of active window == //
        private async void delayedActiveWindowScreenshotButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            await Task.Delay(100); // slight delay to ensure screenshot process starts before hiding the main window
            _hotkeyScreenshot?.CaptureActiveWindowDelayed();

        }

        // == when button is clicked, take a delayed screenshot of region == //
        private async void delayedRegionScreenshotButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            await Task.Delay(100); // slight delay to ensure screenshot process starts before hiding the main window
            _hotkeyScreenshot?.CaptureRegionDelayed();
        }

        // == when button is clicked, open clipboard image in MS Paint == //
        private async void openClipboardImageInPaintButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            await Task.Delay(100); // slight delay to ensure screenshot process starts before hiding the main window
            OpenClipboardImageInPaint.OpenImageInPaint();
        }

        // == esc key closes form and minimizes to tray == //
        private void Driver_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                e.Handled = true;
            }
        }
    }
}