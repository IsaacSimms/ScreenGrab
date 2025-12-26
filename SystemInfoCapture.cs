///// ties into active window capture and has an on/off toggle in settings /////
///// when true, and an active window capture is taken, open image in this form. Gather system information including:
///// ProcessName, ProcessId, ExcutablePath, FileVersion, and CaptureTime
///// all of that information gets printed in copiable text under the active window screenshot. 

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenGrab
{
    public partial class SystemInfoCapture : Form
    {
     
        // == required componenets == //
        public Bitmap Screenshot { get; set; }
        public DateTime CaptureTime { get; set; }
        public IntPtr WindowHandle { get; set; }

        // == process information == //
        public string? ProcessName    { get; set; }
        public int     ProcessId      { get; set; }
        public string? ExecutablePath { get; set; }
        public string? FileVersion    { get; set; }
        public string? ProductVersion { get; set; }
        public string? WindowTitle    { get; set; }


        public SystemInfoCapture()
        {
            InitializeComponent();
        }
    }
}
