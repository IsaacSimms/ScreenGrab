///// This class handles active window screen grabbing functionality /////
///// When control + shift + Z is pressed, capture a screenshot of the currently active window. Saves PNG to clipboard and onedrive folder  /////
///// When control + shift + X is pressed, enable a region select screenshot mode. User can click and drag to select a region of the screen to capture. Saves PNG to clipboard and onedrive folder  /////

// namespaces
using Microsoft.VisualBasic.Devices;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace ScreenGrab
{
    internal class HotkeyScreenshot : NativeWindow
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
        private const uint MOD_CONTROL = 0x0002;       // modifier key for control
        private const uint MOD_SHIFT = 0x0004;         // modifier key for shift
        private const uint MOD_ALT = 0x0001;           // modifier key for alt
        private const uint VK_Z = 0x5A;                // virtual key code for Z key
        private const uint VK_X = 0x58;                // virtual key code for X key
        private const uint VK_escape = 0x1B;           // virtual key code for Escape key
        // Hotkey IDs //
        private const int WM_HOTKEY = 0x0312;          // Windows message ID for hotkey
        private const int HOTKEY_ID_ACTIVE_WINDOW = 1; // ID for active window screenshot hotkey
        private const int HOTKEY_ID_REGION_SELECT = 2; // ID for region select screenshot hotkey
        private readonly int _ActiveWindowHotkeyId;    // instance variable for active window hotkey ID
        private readonly int _RegionSelectHotkeyId;    // instance variable for region select hotkey ID
        private bool _registeredActive;                // instance variable for window handle
        private bool _registeredRegion;                // instance variable to track if region select hotkey is registered

        // == Register hotkeys == //
        public HotkeyScreenshot(Form ownerForm)
        {
            // attach to form handle
            AssignHandle(ownerForm.Handle);

            // generate unique hotkey IDs
            _ActiveWindowHotkeyId = GetHashCode();
            _RegionSelectHotkeyId = GetHashCode() ^ 0x5A5A5A5A;

            // Control + Shift + Z for active window screenshot
            _registeredActive = RegisterHotKey(ownerForm.Handle, _ActiveWindowHotkeyId, MOD_CONTROL | MOD_SHIFT, VK_Z);
            // Control + Shift + X for region select screenshot
            _registeredRegion = RegisterHotKey(ownerForm.Handle, _RegionSelectHotkeyId, MOD_CONTROL | MOD_SHIFT, VK_X);

            // error handling for hotkey registration
            if (!_registeredActive || !_registeredRegion)
            {
                MessageBox.Show("Failed to register one or more hotkeys.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new InvalidOperationException("Failed to register one or more hotkeys.");
            }
        }

        // == Handle window messages to detect hotkey presses == //
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)                   // check if message is hotkey press
            {
                int id = m.WParam.ToInt32();          // get hotkey ID
                if (id == _ActiveWindowHotkeyId)      // check if active window hotkey was pressed
                {
                    CaptureActiveWindow();
                }
                else if (id == _RegionSelectHotkeyId) // check if region select hotkey was pressed
                {
                    CaptureRegion();
                }
            }
            base.WndProc(ref m);
        }

        // == Capture active window and and save to clipboard and onedrive == //
        private void CaptureActiveWindow()
        {
            IntPtr hWnd = GetForegroundWindow();                // get handle of active window
            if (hWnd == IntPtr.Zero)                            // validate handle
            {
                MessageBox.Show("No active window detected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!GetWindowRect(hWnd, out RECT rect))           // validate getting window dimensions
            {
                MessageBox.Show("Failed to get window dimensions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int width = rect.Right - rect.Left;               // calculate width
            int height = rect.Bottom - rect.Top;              // calculate height
            if (width <= 0 || height <= 0)                    // validate dimensions
            {
                MessageBox.Show("Invalid window dimensions.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Rectangle area = new Rectangle(rect.Left, rect.Top, width, height); // define capture area
            CaptureAndSave(area, "Active Window");                              // capture and save screenshot
        }
        // == Capture selected region and save to clipboard and onedrive == //
        private void CaptureRegion()
        {
            using (var selector = new RegionSelectForm())
            {
                if (selector.ShowDialog() != DialogResult.OK)             // user cancelled
                {
                    return;
                }
                Rectangle selectedArea = selector.SelectedRegion;        // get selected region
                if (selectedArea.Width <= 0 || selectedArea.Height <= 0) // validate selected area
                {
                    MessageBox.Show("Invalid selected region.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CaptureAndSave(selectedArea, "Region Select");           // capture and save screenshot
            }
        }

        // == Form for region selection == //
        private class RegionSelectForm : Form
        {
            // variable declarations
            private bool _isSelecting;                         // flag to track if selection is in progress
            private Point _startPoint;                         // starting point of selection
            private Rectangle _selectedArea = Rectangle.Empty; // selected area rectangle
            public Rectangle SelectedRegion => _selectedArea;  // public property to access selected region

            // start region select on left mouse click
            private void onMouseDown(object? sender, MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left) return; // only start selection on left mouse button
                if (e.Button == MouseButtons.Left)         // start selection
                {
                    _isSelecting = true;
                    _startPoint = e.Location;
                    _selectedArea = Rectangle.Empty;       // reset selected area upon new mouse click
                    Invalidate();
                }
            }

            // update selection rectangle on mouse move
            private void OnMouseMove (object? sender, MouseEventArgs e)
            {
                if (!_isSelecting) return;                               // do not call function if mouse is not moving

                // calculate selection rectangle
                int x1 = Math.Min(_startPoint.X, e.X);
                int y1 = Math.Min(_startPoint.Y, e.Y);
                int x2 = Math.Max(_startPoint.X, e.X);
                int y2 = Math.Max(_startPoint.Y, e.Y);

                _selectedArea = new Rectangle(x1, y1, x2 - x1, y2 - y1); // update selected area
                Invalidate();                                            // request redraw
            }

            // finalize selection on mouse up
            private void OnMouseUp(object? sender, MouseEventArgs e)
            {
                if (!_isSelecting) return;                               // do not call function if mouse is not released
                _isSelecting = false;                                    // end selection
                if (_selectedArea.Width > 0 && _selectedArea.Height > 0) // validate selected area
                {
                    DialogResult = DialogResult.OK;                      // set dialog result to OK if valid area selected
                }
                else
                {
                    DialogResult = DialogResult.Cancel;                 // cancel if no valid area selected
                }
                Close();                                                 // close the form
            }
            // if escape key is pressed, cancel selection
            private void EscKeyDown(object? sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    _isSelecting = false;               // end selection
                    _selectedArea = Rectangle.Empty;    // reset selected area
                    DialogResult = DialogResult.Cancel; // set dialog result to Cancel
                    Close();                            // close the form
                }
            }

            // draw selection rectangle
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (_isSelecting && _selectedArea != Rectangle.Empty)
                {
                    using (var PenSelection = new Pen(Color.DarkGreen, 2))         // pen for selection rectangle
                    {
                        e.Graphics.DrawRectangle(PenSelection, _selectedArea);            // draw selection rectangle
                    }
                    using (var BrushSelection = new SolidBrush(Color.FromArgb(50, Color.LightGreen))) {
                        e.Graphics.FillRectangle(BrushSelection, _selectedArea); // fill selection rectangle with semi-transparent color
                    }
                }
            }

            // constructor to setup form properties and event handlers
            public RegionSelectForm()
            {
                this.FormBorderStyle = FormBorderStyle.None;            // no border
                this.WindowState = FormWindowState.Maximized;           // full screen
                this.BackColor = Color.Gray;                            // white background
                this.Opacity = 0.3;                                     // semi-transparent
                this.TopMost = true;                                    // always on top
                this.DoubleBuffered = true;                             // reduce flicker
                Cursor = Cursors.Default;
                // attach event handlers
                this.MouseDown += new MouseEventHandler(onMouseDown);
                this.MouseMove += new MouseEventHandler(OnMouseMove);
                this.MouseUp   += new MouseEventHandler(OnMouseUp);
                this.KeyDown   += new KeyEventHandler(EscKeyDown);
            }
        }

        // == Capture specified area and save to clipboard and onedrive == //
        private void CaptureAndSave(Rectangle area, string prefix)
        {
            using Bitmap bitmap = new Bitmap(area.Width, area.Height);     // create bitmap to hold screenshot
            using Graphics g = Graphics.FromImage(bitmap);                 // create graphics object from bitmap
            {             
                g.CopyFromScreen(area.Location, Point.Empty, area.Size);   // capture screenshot from specified area
            }
            // save to clipboard
            Clipboard.SetImage(bitmap);

            // save to OneDrive folder
            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive", "Screenshots");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);                            // create directory if it does not exist
            }
            string fileName = $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png";   // generate filename with prefix and timestamp
            string filePath = Path.Combine(basePath, fileName);                 // full file path

            // save bitmap as PNG
            try
            {
                bitmap.Save(filePath, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save screenshot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // == Cleanup: Unregister hotkeys when done == //
        public void Dispose()
        {
            // cleanup active window hotkey
            if (_registeredActive)
            {
                UnregisterHotKey(Handle, _ActiveWindowHotkeyId);
                _registeredActive = false;
            }
            // cleanup region select hotkey
            if (_registeredRegion)
            {
                UnregisterHotKey(Handle, _RegionSelectHotkeyId);
                _registeredRegion = false;
            }
            // release native window handle
            ReleaseHandle();
        }
    }
}
