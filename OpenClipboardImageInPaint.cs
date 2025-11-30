///// This is a helper class which does the heavy lifting of opening an image from the clipboard into MS Paint. /////
///// The hotkey for this action is Ctrl+Shift+P. This action is wired into HotkeyScreenshot.cs /////

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace ScreenGrab
{
    internal class OpenClipboardImageInPaint
    {
        public static void OpenImageInPaint()
        {
            // error handle for no image in clipboard
            if (!Clipboard.ContainsImage())
            {
                ScreenshotMessageBox.ShowMessage(                             // show message box on screenshot taken
                       $"ScreenGrab: there is not a .png saved to clipboard", // message
                       $"ScreenGrab",                                         // title //not displaying in current config
                       4000);                                                 // duration in ms
            }
            // get image from clipboard
            Image? clipboardImage = Clipboard.GetImage();
            // error handle for null image 
            if (clipboardImage == null)
            { 
                ScreenshotMessageBox.ShowMessage(                                // show message box on screenshot taken
                    $"ScreenGrab: Clipboard image not found. Please try again.", // message
                    $"ScreenGrab",                                               // title //not displaying in current config
                     4000);                                                      // duration in ms
            }
            // save image to temp file
            string tempFilePath = Path.Combine(Path.GetTempPath());
            string fileName = $"ClipboardImage_{Guid.NewGuid()}.png";
            string fullPath = Path.Combine(tempFilePath, fileName);

            clipboardImage?.Save(fullPath, ImageFormat.Png);

            // open image in MS Paint
            var paintProcess = new ProcessStartInfo
            {
                FileName = "mspaint.exe",
                Arguments = $"\"{fullPath}\"",
                UseShellExecute = true
            };
            // start MS Paint w/ error handle
            try
            {
                Process.Start(paintProcess);
            }
            catch (Exception ex)
            {
                ScreenshotMessageBox.ShowMessage(                                       // show message box on screenshot taken
                    $"ScreenGrab: Failure to open MS Paint. Error: {ex.Message}",       // message
                    $"ScreenGrab",                                                      // title //not displaying in current config
                    4000);                                                              // duration in ms
            }
        }
    }
}
