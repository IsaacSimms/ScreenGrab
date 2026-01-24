///// This is a helper class which does the heavy lifting of opening an image from the clipboard into MS Paint. /////
///// The hotkey for this action is Ctrl+Shift+P. This action is wired into HotkeyScreenshot.cs                 /////
///// If there is not a .png on the clipboard, it will open MS Paint with no image loaded.                      /////

// namespaces
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace ScreenGrab
{
    internal class OpenClipboardImageInPaint
    {
        public static void OpenImageInPaint()
        {
            // let user know there is no image on clipboard
            if (!Clipboard.ContainsImage())
            {
                ScreenshotMessageBox.ShowMessage( // show message
                       $"There is not a .png saved to clipboard",
                       $"ScreenGrab:",
                       4000);
            }

            // == get image from clipboard and save to temp file == //
            Image? clipboardImage = Clipboard.GetImage();                 // get image from clipboard
            // save image to temp file
            string tempFilePath = Path.GetTempPath();
            string fileName = $"ClipboardImage_{Guid.NewGuid()}.png";
            string fullPath = Path.Combine(tempFilePath, fileName);
            clipboardImage?.Save(fullPath, ImageFormat.Png);             // save image as png

            // == opens MS paint with image loaded if there is a .png on clipboard == //
            if (clipboardImage != null)
            {
                // open image in MS Paint using cmd.exe to ensure proper environment
                var paintProcess = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start \"\" mspaint.exe \"{fullPath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                // starts MS Paint w/ error handle
                try
                {
                    Process.Start(paintProcess);
                }
                catch (Exception ex)
                {
                    ScreenshotMessageBox.ShowMessage( // show message box
                        $"Failure to open MS Paint. Error: {ex.Message}",
                        $"ScreenGrab:",
                        4000);
                    System.Diagnostics.Debug.WriteLine($"Opening MsPaint failed {ex.Message}");
                }
            }

            // == opens MS paint with no image loaded if there is not a .png on clipboard == //
            if (clipboardImage == null)
            {
                var paintProcessNoImage = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c start \"\" mspaint.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                // starts MS Paint w/ error handle
                try
                {
                    Process.Start(paintProcessNoImage);
                }
                catch (Exception ex)
                {
                    ScreenshotMessageBox.ShowMessage(
                        $"Failure to open MS Paint. Error: {ex.Message}",
                        $"ScreenGrab:",
                        4000);
                    System.Diagnostics.Debug.WriteLine($"Opening MSPaint failed {ex.Message}");
                }
            }
        }
    }
}
