///// Code block that controls the setting page of the ScreenGrab application. /////
/// Give the user ability to configure hotkey settings in the future. ///


using System;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace ScreenGrab
{
    public partial class SettingsForm : Form
    {
        private HotkeyConfig _config; // hotkey configuration local variable

        //track current hot being configured
        private enum HotkeyBeingConfigured
        {
            None, Active, Region, ActiveDelayed, RegionDelayed, Paint, Editor
        }
        private HotkeyBeingConfigured _currentHotkeyEditTarget = HotkeyBeingConfigured.None; // initialize to none
        public event Action<HotkeyConfig>? HotkeysChanged;                                   // event to notify main app of hotkey changes
        public event Action<HotkeyConfig>? SaveFileLocationChanged;                          // event to notify main app of save file location changes
        public SettingsForm(HotkeyConfig config)
        {
            InitializeComponent();   // initialize form components 
            _config = config;        // assign passed config to variable within class

            // set text boxes to current hotkey settings
            txtActive.Text                          = _config.ActiveWindowCapture.ToString();
            txtRegion.Text                          = _config.RegionCapture.ToString();
            txtActiveDelayed.Text                   = _config.ActiveWindowDelayedCapture.ToString();
            txtRegionDelayed.Text                   = _config.RegionDelayedCapture.ToString();
            txtPaint.Text                           = _config.OpenPaint.ToString();
            txtEditor.Text                          = _config.OpenEditor.ToString();
            txtChangeScreenCaptureFileLocation.Text = _config.ScreenshotSaveLocation;
            chkAutoCopyToClipboard.Checked          = _config.AutoCopyToClipboard;
            chkAutoOpenEditorOnCapture.Checked      = _config.AutoOpenEditorOnCapture;
            chkSaveToFileLocation.Checked           = _config.SaveToFileLocation;
            chkEnableSystemCapture.Checked          = _config.SystemCaptureMode;

            //make text boxes read-only to prevent manual editing
            txtActive.ReadOnly                           = true;
            txtRegion.ReadOnly                           = true;
            txtActiveDelayed.ReadOnly                    = true;
            txtRegionDelayed.ReadOnly                    = true;
            txtPaint.ReadOnly                            = true;
            txtHotkeySettingsGUIHeader.ReadOnly          = true;
            txtCaptureActiveWindowConfigHeader.ReadOnly  = true;
            txtCaptureRegionConfigHeader.ReadOnly        = true;
            txtDelayedActiveWindowConfigHeader.ReadOnly  = true;
            txtDelayedCaptureRegionConfigHeader.ReadOnly = true;
            txtOpenPNGInPaintConfigHeader.ReadOnly       = true;
            txtOpenEditorHeader.ReadOnly                 = true;
            txtSaveFileLocationHeader.ReadOnly           = true;
            txtAutoCopyHeader.ReadOnly                   = true;
            txtAutoOpenEditorHeader.ReadOnly             = true;
            txtSaveFileLocationHeader.ReadOnly           = true;
            txtGeneralSettingsHeader.ReadOnly            = true;
            txtSystemCaptureModeToggleHeader.ReadOnly    = true;

            // receive keydown events for text boxes
            this.KeyPreview = true;
            this.KeyUp += new KeyEventHandler(SettingsForm_KeyDown);

            // wire up checkboox even handlers
            chkEnableSystemCapture.CheckedChanged += chkEnableSystemCapture_CheckChanged;
        }

        // button click handlers to change hotkey being configured // print to label the staus
        private void btnChangeActiveWindowHotkeyConfig_Click(object sender, EventArgs e)        // active window capture
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.Active;
            lblStatus.Text = "Press new hotkey for active window capture...";
        }
        private void btnChangeRegionCaptureHotkeyConfig_Click(object sender, EventArgs e)       // region capture
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.Region;
            lblStatus.Text = "Press new hotkey fore region capture...";
        }
        private void btnChangeActiveWindowDelayedHotkeyConfig_Click(object sender, EventArgs e) // delayed active window capture
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.ActiveDelayed;
            lblStatus.Text = "Press new hotkey for delayed active window capture...";
        }
        private void btnChangeRegionDelayedHotkeyConfig_Click(object sender, EventArgs e)       // delayed region capture
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.RegionDelayed;
            lblStatus.Text = "Press new hotkey for delayed region capture...";
        }
        private void btnChangeOpenPaintHotkeyConfig_Click(object sender, EventArgs e)          // open in paint hotkey
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.Paint;
            lblStatus.Text = "Press new hotkey for open MS Paint...";
        }
        private void btnChangeOpenEditorHotkeyConfig_Click(object sender, EventArgs e)         // open in editor hotkey
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.Editor; // not implemented yet
            lblStatus.Text = "Press new hotkey for open image editor...";
        }
        private void btnChangeScreenCaptureFileLocation_Click(object sender, EventArgs e)    // change save file location button
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select folder to save screenshots:";
                folderDialog.SelectedPath = _config.ScreenshotSaveLocation; // set initial path to current config
                folderDialog.ShowNewFolderButton = true;
                if (folderDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    _config.ScreenshotSaveLocation = folderDialog.SelectedPath; // update config with new path
                    txtChangeScreenCaptureFileLocation.Text = folderDialog.SelectedPath; // update text box
                    lblStatus.Text = "Screenshot save location updated successfully.";
                    SaveFileLocationChanged?.Invoke(_config); // raise event to notify main app of save location change
                }
                else 
                {
                    lblStatus.Text = "Screenshot save location change canceled.";
                }
            }
        }
        // == auto copy to clipboard checkbox change handler == //
        private void chkAutoCopyToClipboard_CheckedChanged(object sender, EventArgs e) // auto copy to clipboard checkbox change handler
        {
            _config.AutoCopyToClipboard = chkAutoCopyToClipboard.Checked;              // update config based on checkbox state
            lblStatus.Text = _config.AutoCopyToClipboard                               // update status label
                ? "Auto-copy to clipboard enabled."
                : "Auto-copy to clipboard disabled.";
            SaveFileLocationChanged?.Invoke(_config);                                  // raise event to notify main app of change
        }
        // == auto open in editor checkbox change handler == //
        private void chkAutoOpenInEditor_CheckedChanged(object sender, EventArgs e) // auto open in editor checkbox change handler
        {
            _config.AutoOpenEditorOnCapture = chkAutoOpenEditorOnCapture.Checked;                    // update config based on checkbox state
            lblStatus.Text = _config.AutoOpenEditorOnCapture
                ? "Auto-open in editor enabled."
                : "Auto-open in editor disabled.";
            SaveFileLocationChanged?.Invoke(_config);                                  // raise event to notify main app of change
        }
        // == save to file location checkbox change handler == //
        private void chkSaveToFileLocation_CheckChanged(object sender, EventArgs e)
        {
            _config.SaveToFileLocation = chkSaveToFileLocation.Checked;  // update config based on checkbox state
            lblStatus.Text = _config.SaveToFileLocation
                ? "Auto-save to file location enabled."
                : "Auto-save to file location disabled.";
            SaveFileLocationChanged?.Invoke(_config);
        }
        // == system capture mode checkbox change handler == //
        private void chkEnableSystemCapture_CheckChanged(object sender, EventArgs e)
        {
            _config.SystemCaptureMode = chkEnableSystemCapture.Checked;  // update config based on checkbox state
            lblStatus.Text = _config.SystemCaptureMode
                ? "System capture mode enabled."
                : "System capture mode disabled.";
            SaveFileLocationChanged?.Invoke(_config);

        }

        // keydown event handler to capture new hotkey
        private void SettingsForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // handle if HotkeyBeingConfigured is none
            if (_currentHotkeyEditTarget == HotkeyBeingConfigured.None)
            {
                return; // no hotkey is being configured
            }
            //build modifiers
            var mods = HotkeyModifiers.None;
            if (e.Control) mods |= HotkeyModifiers.Control;
            if (e.Alt) mods |= HotkeyModifiers.Alt;
            if (e.Shift) mods |= HotkeyModifiers.Shift;

            // create new hotkey definition
            var def = new HotkeyDefinition
            {
                Modifiers = mods,
                Key = e.KeyCode
            };
            // switch/case to assign new hotkey based on which is being configured
            switch (_currentHotkeyEditTarget)
            {
                case HotkeyBeingConfigured.Active:
                    _config.ActiveWindowCapture = def;
                    txtActive.Text = def.ToString();
                    break;
                case HotkeyBeingConfigured.Region:
                    _config.RegionCapture = def;
                    txtRegion.Text = def.ToString();
                    break;
                case HotkeyBeingConfigured.ActiveDelayed:
                    _config.ActiveWindowDelayedCapture = def;
                    txtActiveDelayed.Text = def.ToString();
                    break;
                case HotkeyBeingConfigured.RegionDelayed:
                    _config.RegionDelayedCapture = def;
                    txtRegionDelayed.Text = def.ToString();
                    break;
                case HotkeyBeingConfigured.Paint:
                    _config.OpenPaint = def;
                    txtPaint.Text = def.ToString();
                    break;
                case HotkeyBeingConfigured.Editor:
                    _config.OpenEditor = def;
                    txtEditor.Text = def.ToString();
                    break;

            }
            // reset current hotkey being configured and status label
            _currentHotkeyEditTarget = HotkeyBeingConfigured.None;
            lblStatus.Text = "Hotkey updated successfully.";
            e.SuppressKeyPress = true;                     // prevent keystroke from being processed further
            HotkeysChanged?.Invoke(_config);               // raise event //important to notify main app of hotkey change
        }
        // get configuration method
        public HotkeyConfig GetResultConfig()
        {
            return _config;
        }
        // button to open form named "Driver"
        private void btnGoHome_Click(object sender, EventArgs e)
        {
            Form? parentForm = this.Owner ?? this.Tag as Form;
            if (parentForm != null)
            {
                parentForm.Show();                               // show main app form
                parentForm.WindowState = FormWindowState.Normal; // set window state to normal
                parentForm.ShowInTaskbar = true;                 // show in taskbar
                parentForm.Activate();
            }
            this.Close();                                        // close settings form to return to main app
        }
    }
}