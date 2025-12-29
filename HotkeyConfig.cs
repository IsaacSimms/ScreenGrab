///// handles backend of the user being able to configure hotkeys /////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScreenGrab
{
    // == hotkey modifier flags == //
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HotkeyModifiers
    {
        None    = 0x0000,
        Alt     = 0x0001,
        Control = 0x0002,
        Shift   = 0x0004,
        Win     = 0x0008
    }
    // == define hotkey structure and return hotkeys as string == //
    public class HotkeyDefinition
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HotkeyModifiers Modifiers { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Keys            Key       { get; set; }

        // == override ToString to return hotkey as string == //
        public override string ToString()
        { 
            string text = "";
            if (Modifiers.HasFlag(HotkeyModifiers.Control)) text += "Ctrl + ";
            if (Modifiers.HasFlag(HotkeyModifiers.Alt))     text += "Alt + ";
            if (Modifiers.HasFlag(HotkeyModifiers.Shift))   text += "Shift + ";
            if (Modifiers.HasFlag(HotkeyModifiers.Win))     text += "Win + ";
            text += Key.ToString();
            return text;
        }
    }
    // == hotkey configuration class == //
    public class HotkeyConfig
    {
        /// add modifiers and one key to hotkey definition ///
        // active window
        public HotkeyDefinition ActiveWindowCapture { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Shift,
            Key       = Keys.A
        };
        // Region capture
        public HotkeyDefinition RegionCapture { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Shift,
            Key       = Keys.R
        };
        // lasso tool for region selection
        public HotkeyDefinition FreeformRegionCapture { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Shift,
            Key       = Keys.F
        };
        // OCR region capture
        public HotkeyDefinition OcrRegionCapture { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Shift,
            Key       = Keys.O
        };
        // active window delayed capture
        public HotkeyDefinition ActiveWindowDelayedCapture { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Alt,
            Key       = Keys.A
        };
        // region delayed capture
        public HotkeyDefinition RegionDelayedCapture { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Alt,
            Key       = Keys.R
        };
        // open clipboard png in MS Paint
        public HotkeyDefinition OpenPaint { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Shift,
            Key       = Keys.P
        };
        // open editor window
        public HotkeyDefinition OpenEditor { get; set; } = new HotkeyDefinition
        {
            Modifiers = HotkeyModifiers.Control | HotkeyModifiers.Shift,
            Key       = Keys.E
        };
        //  screenshot save location // defaults to oneDrive Pictures folder
        public string ScreenshotSaveLocation { get; set; } = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), 
                "ScreenGrab"
        );
        // save to file location toggle // default to true
        public bool SaveToFileLocation { get; set; } = true;
        // auto-copy to clipboard option // default to true
        public bool AutoCopyToClipboard { get; set; } = true;
        // auto-open editor option // default to false
        public bool AutoOpenEditorOnCapture { get; set; } = false;
        public bool SystemCaptureMode { get; set; }       = true;
    }
}
