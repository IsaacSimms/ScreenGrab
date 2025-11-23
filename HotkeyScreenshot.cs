///// This class handles active window screen grabbing functionality /////
///// When control + shift + Z is pressed, capture a screenshot of the currently active window. Saves PNG to clipboard and onedrive folder  /////
///// When control + shift + X is pressed, enable a region select screenshot mode. User can click and drag to select a region of the screen to capture. Saves PNG to clipboard and onedrive folder  /////

// namespaces
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Windows.Forms;

namespace ScreenGrab
{
    internal class HotkeyScreenshot
    {
        // == WinAPI Imports == //
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk); // allows for hotkey press to be sent to app 
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);                         // unregisters hotkey so windows does not try to send it when app is closed

        // == Active Window Screenshot Imports == //
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();                                      // gets handle of active window
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);                 // gets dimensions of active window
        private struct RECT                                                                     // struct to hold window dimensions
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // == Hotkey varibale registration == //
        // Modifier key codes //
        private const uint MOD_CONTROL = 0x0002; // modifier key for control
        private const uint MOD_SHIFT   = 0x0004; // modifier key for shift
        private const uint MOD_ALT     = 0x0001; // modifier key for alt
        private const uint VK_Z        = 0x5A;   // virtual key code for Z key
        private const uint VK_X        = 0x58;   // virtual key code for X key
        // Hotkey IDs //
        private const int HOTKEY_ID_ACTIVE_WINDOW = 1; // ID for active window screenshot hotkey
        private const int HOTKEY_ID_REGION_SELECT = 2; // ID for region select screenshot hotkey
        private readonly int _ActiveWindowHotkeyId;    // instance variable for active window hotkey ID
        private readonly int _RegionSelectHotkeyId;    // instance variable for region select hotkey ID
        private bool _registeredActive;                // instance variable for window handle
        private bool _registeredRegion;                // instance variable to track if region select hotkey is registered

        // funtion to register hotkeys
        public HotkeyScreenshot(Form ownerForm)
        {
            // attach to form handle
            AssignHandle(ownerForm.Handle);

            // generate unique hotkey IDs
            _Active
        }
    }
}
