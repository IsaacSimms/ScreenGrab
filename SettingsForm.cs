///// Code block that controls the setting page of the ScreenGrab application. /////
/// Give the user ability to configure hotkey settings in the future. ///
/// 

using System;
using System.Formats.Asn1;
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
        private HotkeyBeingConfigured _currentHotkeyEditTarget = HotkeyBeingConfigured.None;
        public SettingsForm(HotkeyConfig config)
        {
            InitializeComponent();   // initialize form components
            _config = config;        // assign passed config to variable within class

            txtActive.Text = _config.ActiveWindowCapture.ToString();
            txtRegion.Text = _config.RegionCapture.ToString();
        }

        private void txtActive_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
