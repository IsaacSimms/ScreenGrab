///// This class handles active window screen grabbing functionality /////
///// When control + shift + Z is pressed, the application captures a screenshot of the currently active window. Saves PNG to clipboard and onedrive folder  /////
// Namespaces
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenGrab
{
    internal class ActiveWindowGrab
    {
        // == WinAPI Imports == //
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk); // allows for hotkey press to be sent to app 
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);                         // unregisters hotkey so windows does not try to send it when app is closed
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();                                      // gets handle of active window
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);                 // gets dimensions of active window

        // == Structs == //
        private struct RECT // struct to hold window dimensions
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


    }
}
