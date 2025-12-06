///// Code block that controls the setting page of the ScreenGrab application. /////
/// Give the user ability to configure hotkey settings in the future. ///
/// 

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
            None, Active, Region, ActiveDelayed, RegionDelayed, Paint
        }
        private HotkeyBeingConfigured _currentHotkeyEditTarget = HotkeyBeingConfigured.None; // initialize to none
        public event Action<HotkeyConfig>? HotkeysChanged;                                   // event to notify main app of hotkey changes
        public SettingsForm(HotkeyConfig config)
        {
            InitializeComponent();   // initialize form components
            _config = config;        // assign passed config to variable within class

            // set text boxes to current hotkey settings
            txtActive.Text        = _config.ActiveWindowCapture.ToString();
            txtRegion.Text        = _config.RegionCapture.ToString();
            txtActiveDelayed.Text = _config.ActiveWindowDelayedCapture.ToString();
            txtRegionDelayed.Text = _config.RegionDelayedCapture.ToString();
            txtPaint.Text         = _config.OpenPaint.ToString();

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
            // receive keydown events for text boxes
            this.KeyPreview = true;
            this.KeyUp    += new KeyEventHandler(SettingsForm_KeyDown);               //
        }

        // button click handlers to change hotkey being configured // print to label the staus
        private void btnChangeActiveWindowHotkeyConfig_Click(object sender, EventArgs e)
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.Active;
            lblStatus.Text = "Press new hotkey for active window capture...";
        }
        private void btnChangeRegionCaptureHotkeyConfig_Click(object sender, EventArgs e)
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.Region;
            lblStatus.Text = "Press new hotkey fore region capture...";
        }
        private void btnChangeActiveWindowDelayedHotkeyConfig_Click(object sender, EventArgs e)
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.ActiveDelayed;
            lblStatus.Text = "Press new hotkey for delayed active window capture...";
        }
        private void btnChangeRegionDelayedHotkeyConfig_Click(object sender, EventArgs e)
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.RegionDelayed;
            lblStatus.Text = "Press new hotkey for delayed region capture...";
        }
        private void btnChangeOpenPaintHotkeyConfig_Click(object sender, EventArgs e)
        {
            _currentHotkeyEditTarget = HotkeyBeingConfigured.Paint;
            lblStatus.Text = "Press new hotkey for open MS Paint...";
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
            }
            // reset current hotkey being configured and status label
            _currentHotkeyEditTarget = HotkeyBeingConfigured.None;
            lblStatus.Text = "Hotkey updated successfully.";
            e.SuppressKeyPress = true;                     // prevent keystroke from being processed further
            HotkeysChanged?.Invoke(_config); // raise event //important to notify main app of hotkey change
        }
        // get configuration method
        public HotkeyConfig GetResultConfig()
        {
            return _config;
        }
    }
}