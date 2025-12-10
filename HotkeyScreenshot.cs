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
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text; // for async delay (used in delayed screenshot)
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Drawing2D;

namespace ScreenGrab
{
    internal class HotkeyScreenshot : NativeWindow
    {
        // variables for hotkey configuration and registration
        private readonly HotkeyConfig _config;  // instance variable for hotkey configuration
        private IntPtr _handle;          // instance variable for window handle

        // == WinAPI Imports == //
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk); // allows for hotkey press to be sent to app 
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        // == Active Window Screenshot Imports == //
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();                                       // gets handle of active window
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);                  // gets dimensions of active window
        private struct RECT                                                                      // struct to hold window dimensions
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // == Hotkey varibale registration == //
        // Hotkey IDs //
        private const int WM_HOTKEY = 0x0312;                     // Windows message ID for hotkey
        private readonly int _ActiveWindowHotkeyId;                          // instance variable for active window hotkey ID
        private readonly int _RegionSelectHotkeyId;                          // instance variable for region select hotkey ID
        private readonly int _FreeformSelectHotkeyId;                        // instance variable for freeform select hotkey ID
        private readonly int _ActiveWindowDelayHotkeyId;                     // ID for active window delayed screenshot hotkey
        private readonly int _RegionSelectDelayHotkeyId;                     // ID for region select delayed screenshot hotkey
        private readonly int _OpenClipboardInPaintHotkeyId;                  // ID for open clipboard image in paint hotkey
        public event Action<string>? OnScreenshotTaken;                      // event to notify when a screenshot is taken

        // == Register hotkeys == //
        // attach hotkey configruation to handle
        public void AttachToHandle(IntPtr newHandle)
        {
            if (Handle != IntPtr.Zero)
            {
                ReleaseHandle(); // release existing handle if already assigned
            }
            _handle = newHandle;     // assign new handle
            AssignHandle(newHandle); // assign new handle to NativeWindow
            RegisterAllHotkeys();    // register all hotkeys with new handle
        }
        public HotkeyScreenshot(Form owner, HotkeyConfig config)
        {
            _config = config;            // assign hotkey configuration from class to local variable
            _handle = owner.Handle;      // assign window handle from owner form
            AssignHandle(owner.Handle);  // assign window handle from owner form
            _ActiveWindowHotkeyId         = 1;
            _RegionSelectHotkeyId         = 2;
            _FreeformSelectHotkeyId       = 3;
            _ActiveWindowDelayHotkeyId    = 4;
            _RegionSelectDelayHotkeyId    = 5;
            _OpenClipboardInPaintHotkeyId = 6;
            AttachToHandle(owner.Handle);      // register all hotkeys
            //IntPtr hwnd = this.Handle;
        }
        // == register hotkeys helper method == //
        private void RegisterAllHotkeys()
        {
            UnregisterHotkeys(); // unregister existing hotkeys before registering new ones
            // register hotkeys from configuration using helper method
            RegisterFromConfig(_ActiveWindowHotkeyId,         _config.ActiveWindowCapture);
            RegisterFromConfig(_RegionSelectHotkeyId,         _config.RegionCapture);
            RegisterFromConfig(_FreeformSelectHotkeyId,       _config.FreeformRegionCapture); 
            RegisterFromConfig(_ActiveWindowDelayHotkeyId,    _config.ActiveWindowDelayedCapture);
            RegisterFromConfig(_RegionSelectDelayHotkeyId,    _config.RegionDelayedCapture);
            RegisterFromConfig(_OpenClipboardInPaintHotkeyId, _config.OpenPaint);
        }
        // == Unregister hotkeys  == //
        public void UnregisterHotkeys()
        {
            // unregister all hotkeys using WinAPI
            UnregisterHotKey(_handle, _ActiveWindowHotkeyId);
            UnregisterHotKey(_handle, _RegionSelectHotkeyId);
            UnregisterHotKey(_handle, _FreeformSelectHotkeyId);
            UnregisterHotKey(_handle, _ActiveWindowDelayHotkeyId);
            UnregisterHotKey(_handle, _RegionSelectDelayHotkeyId);
            UnregisterHotKey(_handle, _OpenClipboardInPaintHotkeyId);
        }
        private void RegisterFromConfig(int id, HotkeyDefinition def)
        {
            uint mods = (uint)def.Modifiers;              // get modifier flags
            uint key = (uint)def.Key;                     // get key for keyboard hotkey
                                                          //bool ok RegisterHotKey(_handle, id, mods, key); // register hotkey using WinAPI
            if (!RegisterHotKey(_handle, id, mods, key))  // error handling for hotkey registration
            {
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: error registering hotkeys. please relaunch app",
                    $"ScreenGrab",
                    4000);
                System.Diagnostics.Debug.WriteLine($"Failed to register hotkeys");
                throw new InvalidOperationException("Failed to register one or more hotkeys.");
            }
        }
        // == UpdateHotkey configuration == //
        public void UpdateHotkeyConfig(HotkeyConfig newConfig)
        {
            // update configuration
            _config.ActiveWindowCapture        = newConfig.ActiveWindowCapture;
            _config.RegionCapture              = newConfig.RegionCapture;
            _config.FreeformRegionCapture      = newConfig.FreeformRegionCapture;
            _config.ActiveWindowDelayedCapture = newConfig.ActiveWindowDelayedCapture;
            _config.RegionDelayedCapture       = newConfig.RegionDelayedCapture;
            _config.OpenPaint                  = newConfig.OpenPaint;
            RegisterAllHotkeys(); // re-register hotkeys with new configuration
        }


        // == Handle window messages to detect hotkey presses == //
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)                            // check if message is hotkey press
            {
                int id = m.WParam.ToInt32();                   // get hotkey ID
                if (id == _ActiveWindowHotkeyId)               // check if active window hotkey was pressed
                {
                    CaptureActiveWindow();
                }
                else if (id == _RegionSelectHotkeyId)               // check if region select hotkey was pressed
                {
                    CaptureRegion();
                }
                else if (id == _FreeformSelectHotkeyId)             // check if freeform select hotkey was pressed
                {
                    CaptureFreeform();
                }
                else if (id == _ActiveWindowDelayHotkeyId)          // check if active window delay hotkey was pressed
                {
                    CaptureActiveWindowDelayed();
                }
                else if (id == _RegionSelectDelayHotkeyId)          // check if region select delay hotkey was pressed
                {
                    CaptureRegionDelayed();
                }
                else if (id == _OpenClipboardInPaintHotkeyId) // check if open clipboard in paint hotkey was pressed
                {
                    OpenClipboardImageInPaint.OpenImageInPaint();
                }
            }
            // pass message to base WndProc
            base.WndProc(ref m);
        }

        // == Capture active window and and save to clipboard and onedrive == //
        public void CaptureActiveWindow()
        {
            IntPtr hWnd = GetForegroundWindow();                // get handle of active window
            if (hWnd == IntPtr.Zero)                            // validate handle
            {
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: Invalid Region selection.",
                    $"ScreenGrab",
                    4000);
                return;
            }
            if (!GetWindowRect(hWnd, out RECT rect))           // validate getting window dimensions
            {
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: Invalid Region dimnesion aquisition.",
                    $"ScreenGrab",
                    4000);
                return;
            }
            int width = rect.Right - rect.Left;               // calculate width
            int height = rect.Bottom - rect.Top;              // calculate height
            if (width <= 0 || height <= 0)                    // validate dimensions
            {
                ScreenshotMessageBox.ShowMessage(             // show message box on screenshot taken
                    $"ScreenGrab: Invalid Region selection.", // message
                    $"ScreenGrab",                            // title //not displaying in current config
                    4000);                                    // duration in ms
                return;
            }
            Rectangle area = new Rectangle(rect.Left, rect.Top, width, height); // define capture area
            CaptureAndSave(area, "Active Window");                              // capture and save screenshot

        }
        // == Capture selected region and save to clipboard and onedrive == //
        public void CaptureRegion()
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
                    ScreenshotMessageBox.ShowMessage(                                                      // show message box on screenshot taken
                        $"ScreenGrab: Invalid Region selection.", // message
                        $"ScreenGrab",                                                                     // title //not displaying in current config
                        4000);                                                                             // duration in ms
                    return;
                }
                CaptureAndSave(selectedArea, "Region Select");           // capture and save screenshot
            }
        }

        // == Capture freeform region and save to clipboard and onedrive == //
        public void CaptureFreeform()
        {
            using (var selector = new FreeformSelectForm())
            {
                if (selector.ShowDialog() != DialogResult.OK)             // user cancelled
                {
                    return;
                }
                List<Point> freeformPath = selector.FreeformPath;        // get selected region points
                if (freeformPath == null || freeformPath.Count < 2)      // validate selected area
                {
                    ScreenshotMessageBox.ShowMessage(                    // show message box on screenshot taken
                        $"ScreenGrab: Invalid Region selection.",        // message
                        $"ScreenGrab",                                   // title //not displaying in current config
                        4000);                                           // duration in ms
                    return;
                }
                CaptureAndSaveFreeform(freeformPath, "Freeform Select");         // capture and save screenshot
            }
        }

        // == Form for region selection == //
        private class RegionSelectForm : Form
        {
            // variable declarations
            private bool _isSelecting;                         // flag to track if selection is in progress
            private Point _startPointScreen;                   // starting point of selection
            private Rectangle _selectedArea = Rectangle.Empty; // selected area rectangle
            public Rectangle SelectedRegion => _selectedArea;  // public property to access selected region

            // start region select on left mouse click
            private void onMouseDown(object? sender, MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left) return; // only start selection on left mouse button
                if (e.Button == MouseButtons.Left)         // start selection
                {
                    _isSelecting = true;
                    _startPointScreen = PointToScreen(e.Location);
                    _selectedArea = Rectangle.Empty;       // reset selected area upon new mouse click
                    Invalidate();
                }
            }

            // update selection rectangle on mouse move
            private void OnMouseMove(object? sender, MouseEventArgs e)
            {
                if (!_isSelecting) return;                               // do not call function if mouse is not moving
                Point currentScreen = PointToScreen(e.Location);        // get current mouse position in screen coordinates

                // calculate selection rectangle
                int x1 = Math.Min(_startPointScreen.X, currentScreen.X);
                int y1 = Math.Min(_startPointScreen.Y, currentScreen.Y);
                int x2 = Math.Max(_startPointScreen.X, currentScreen.X);
                int y2 = Math.Max(_startPointScreen.Y, currentScreen.Y);

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
                    DialogResult = DialogResult.Cancel;                  // cancel if no valid area selected
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
                    // convert selected area to client coordinates for drawing
                    Rectangle clientRectangle = new Rectangle(
                        _selectedArea.X - this.Bounds.X,
                        _selectedArea.Y - this.Bounds.Y,
                        _selectedArea.Width,
                        _selectedArea.Height);
                    using (var PenSelection = new Pen(Color.Red, 4))               // pen for selection rectangle
                    {
                        e.Graphics.DrawRectangle(PenSelection, clientRectangle);   // draw selection rectangle
                    }
                    Rectangle innerRectangle = new Rectangle(
                        clientRectangle.X + 4,
                        clientRectangle.Y + 4,
                        clientRectangle.Width - 8,
                        clientRectangle.Height - 8);
                    // Draw dashed border for selection rectangle
                    using (var PenDashed = new Pen(Color.Black, 2))
                    {
                        PenDashed.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        e.Graphics.DrawRectangle(PenDashed, clientRectangle);
                    }
                    // draw semi-transparent fill for selection rectangle
                    using (var BrushSelection = new SolidBrush(Color.FromArgb(25, Color.White)))
                    {
                        e.Graphics.FillRectangle(BrushSelection, clientRectangle);
                    }
                }
            }

            // constructor to setup form properties and event handlers
            public RegionSelectForm()
            {
                // setup form properties
                var vs = System.Windows.Forms.SystemInformation.VirtualScreen; // get virtual screen dimensions
                this.FormBorderStyle = FormBorderStyle.None;                   // no border
                this.StartPosition = FormStartPosition.Manual;                 // manual position
                this.Bounds = vs;                                              // cover entire virtual screen
                this.BackColor = Color.White;                                  // white background
                this.Opacity = .15;                                             // semi-transparent
                this.TopMost = true;                                           // always on top
                this.DoubleBuffered = true;                                    // reduce flicker
                Cursor = Cursors.Cross;
                // attach event handlers
                this.MouseDown += new MouseEventHandler(onMouseDown);
                this.MouseMove += new MouseEventHandler(OnMouseMove);
                this.MouseUp   += new MouseEventHandler(OnMouseUp);
                this.KeyDown   += new KeyEventHandler(EscKeyDown);
            }
        }

        // == Capture active window after delay == //
        public async void CaptureActiveWindowDelayed()
        {
            await Task.Delay(5000); // 5 second delay
            CaptureActiveWindow();
        }
        // == Capture selected region after delay == //
        public async void CaptureRegionDelayed()
        {
            await Task.Delay(3000); // 5 second delay
            CaptureRegion();
        }

        // == freeform selection form == //
        private class FreeformSelectForm : Form
        {
            private bool _isDrawing;                               // flag to track if drawing is in progress
            private List<Point> _freeformPath = new List<Point>(); // list to hold freeform path points
            public List<Point> FreeformPath => _freeformPath;      // public property to access freeform path

            // start freeform drawing on left mouse click
            private void OnMouseDown(object? sender, MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left) return;    // only start drawing on left mouse button
                _isDrawing = true;
                _freeformPath.Clear();                        // clear existing path //prevents unwanted behavior
                _freeformPath.Add(PointToScreen(e.Location)); // add starting point
                Invalidate();
            }

            // update freeform path on mouse move
            private void OnMouseMove(object? sender, MouseEventArgs e)
            {
                if (!_isDrawing) return;
                Point currentPoint = PointToScreen(e.Location);                       // get current mouse position in screen coordinates
                if (_freeformPath.Count == 0 || _freeformPath.Last() != currentPoint) // check if point is different from last point
                {
                    _freeformPath.Add(currentPoint);           // add point to path
                    Invalidate();                              // request redraw
                }
            }

            // finalize freeform drawing on mouse up
            private void OnMouseUp(object? sender, MouseEventArgs e)
            {
                if (!_isDrawing) return;
                _isDrawing = false;                             // end drawing
                if (_freeformPath.Count > 0)
                {
                    _freeformPath.Add(_freeformPath[0]);        // close the path by adding starting point at the end
                }
                if (_freeformPath.Count > 2)                    // validate path has enough points
                {
                    DialogResult = DialogResult.OK;             // set dialog result to OK if valid path
                }
                else
                {
                    DialogResult = DialogResult.Cancel;         // cancel if not enough points
                }
                Close();                                        // close the form
            }

            // if escape key is pressed, cancel drawing
            private void EscKeyDown(object? sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    _isDrawing = false;                  // end drawing
                    _freeformPath.Clear();               // clear path
                    DialogResult = DialogResult.Cancel;  // set dialog result to Cancel
                    Close();                             // close the form
                }
            }

            // override OnPaint to draw freeform path
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (_freeformPath.Count > 1)
                {
                    // convert freeform path points to client coordinates for drawing
                    Point[] clientPoints = _freeformPath
                        .Select(p => new Point(p.X - this.Bounds.X, p.Y - this.Bounds.Y))
                        .ToArray();
                    // anti-aliasing for smoother lines
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    using (var PenFreeform = new Pen(Color.Red, 4))               // pen for freeform path
                    {
                        e.Graphics.DrawLines(PenFreeform, clientPoints);          // draw freeform path
                    }
                    //dashed overlay
                    using (var PenDashedFreeform = new Pen(Color.Black, 2))
                    {
                        PenDashedFreeform.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        e.Graphics.DrawLines(PenDashedFreeform, clientPoints);
                    }
                }
            }

            // == Capture freeform region constructor & properties setup // should mirror look of region select == //
            public FreeformSelectForm()
            {
                var vs = System.Windows.Forms.SystemInformation.VirtualScreen; // get virtual screen dimensions
                this.FormBorderStyle = FormBorderStyle.None;                   // no border
                this.StartPosition = FormStartPosition.Manual;                 // manual position
                this.Bounds = vs;                                              // cover entire virtual screen
                this.BackColor = Color.White;                                  // white background
                this.Opacity = .15;                                             // semi-transparent
                this.TopMost = true;                                           // always on top
                this.DoubleBuffered = true;                                    // reduce flicker

                // event handlers
                this.MouseDown += new MouseEventHandler(OnMouseDown);
                this.MouseMove += new MouseEventHandler(OnMouseMove);
                this.MouseUp += new MouseEventHandler(OnMouseUp);
                this.KeyDown += new KeyEventHandler(EscKeyDown);
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
            string basePath = _config.ScreenshotSaveLocation;
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
                OnScreenshotTaken?.Invoke(filePath); // trigger event to notify screenshot taken
            }
            catch (Exception ex)
            {
                ScreenshotMessageBox.ShowMessage(                                                  // show message box on screenshot taken
                $"ScreenGrab: Failed to save screenshot: {ex.Message}.",                           // message
                $"ScreenGrab",                                                                     // title //not displaying in current config
                4000);                                                                             // duration in ms
                System.Diagnostics.Debug.WriteLine($"Failed to take a screenshot: {ex.Message}");
                throw new InvalidOperationException($"Failed to take a screenshot: {ex.Message}.");
            }
        }

        // == Capture freeform region and save to clipboard and onedrive == //
        private void CaptureAndSaveFreeform(List<Point> freeformPath, string prefix)
        {
            // calculate bounding rectangle of freeform path
            int minX = freeformPath.Min(p => p.X);
            int minY = freeformPath.Min(p => p.Y);
            int maxX = freeformPath.Max(p => p.X);
            int maxY = freeformPath.Max(p => p.Y);

            //calculate width and height
            int width = maxX - minX;
            int height = maxY - minY;

            // error handling for invalid dimensions
            if (width <= 0 || height <= 0)
            {
                ScreenshotMessageBox.ShowMessage(                                                  // show message box on screenshot taken
                    $"ScreenGrab: Invalid Region selection.",                                      // message
                    $"ScreenGrab",                                                                 // title //not displaying in current config
                    4000);                                                                         // duration in ms
                return;
            }

            // create bitmap to hold screenshot
            using Bitmap fullCapture = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(fullCapture);
            {
                g.CopyFromScreen(new Point(minX, minY), Point.Empty, new Size(width, height));     // capture bounding rectangle area
            }

            // create mask for freeform shape //uses graphics path to create a mask
            using Bitmap maskedCapture = new Bitmap(width, height);
            using (Graphics gMask = Graphics.FromImage(maskedCapture))
            {
                using GraphicsPath gp = new GraphicsPath();
                {
                    // create path relative to bounding rectangle
                    Point[] relativePoints = freeformPath
                        .Select(p => new Point(p.X - minX, p.Y - minY))
                        .ToArray();
                    gp.AddPolygon(relativePoints);
                    gMask.Clear(Color.Transparent);             // clear background to transparent
                    gMask.SetClip(gp);                          // set clipping region to freeform path
                    gMask.DrawImage(fullCapture, Point.Empty); // draw captured image onto masked bitmap
                }
            }

            // save to clipboard
            Clipboard.SetImage(maskedCapture);

            // save to OneDrive folder
            string basePath = _config.ScreenshotSaveLocation;
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);                            // create directory if it does not exist
            }
            string fileName = $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png";   // generate filename with prefix and timestamp
            string filePath = Path.Combine(basePath, fileName);                 // full file path

            // save masked bitmap as PNG
            try
            {
                maskedCapture.Save(filePath, ImageFormat.Png);
                OnScreenshotTaken?.Invoke(filePath); // trigger event to notify screenshot taken
            }
            catch (Exception ex)
            {
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: Failed to save screenshot: {ex.Message}.",                           // message
                    $"ScreenGrab",                                                                     // title //not displaying in current config
                    4000);                                                                             // duration in ms
                System.Diagnostics.Debug.WriteLine($"Failed to take a screenshot: {ex.Message}");
                throw new InvalidOperationException($"Failed to take a screenshot: {ex.Message}.");
            }
        }

        // == Cleanup: Unregister hotkeys when done == //
        public void Dispose()
        {

            UnregisterHotkeys(); // unregister all hotkeys
            ReleaseHandle();     // release window handle
        }
    }
}