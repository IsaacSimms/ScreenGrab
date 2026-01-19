///// This class handles screen capture logic, hotkey registration, png save logic, with error handling and messaging /////
///// using hotkeys defined in HotkeyConfig.cs /////


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
using System.Runtime.CompilerServices;

namespace ScreenGrab
{
    internal class HotkeyScreenshot : NativeWindow
    {
        // variables for hotkey configuration and registration
        private readonly HotkeyConfig _config;  // instance variable for hotkey configuration
        private IntPtr _handle;                 // instance variable for window handle
        private readonly Form _driverForm;      // instance variable for main driver form (used for passing to other forms, e.g. OCR)

        // == WinAPI Imports == //
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk); // allows for hotkey press to be sent to app 
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        // == Active Window Screenshot Imports == //
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();                                       // gets handle of active window
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);                  // gets dimensions of active window

        // == WinAPI DPI Awareness Context Imports == //
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();                                               // sets process as DPI aware
        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute); // gets window attributes for DPI scaling
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9; // attribute for extended frame bounds


        // == RECT struct for window dimensions == //
        private struct RECT                                                                      // struct to hold window dimensions
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // == Hotkey varibale registration == //
        // Hotkey IDs //
        private const int    WM_HOTKEY = 0x0312;             // Windows message ID for hotkey
        private readonly int _ActiveWindowHotkeyId;          // instance variable for active window hotkey ID
        private readonly int _RegionSelectHotkeyId;          // instance variable for region select hotkey ID
        private readonly int _FreeformSelectHotkeyId;        // instance variable for freeform select hotkey ID
        private readonly int _ActiveWindowDelayHotkeyId;     // ID for active window delayed screenshot hotkey
        private readonly int _RegionSelectDelayHotkeyId;     // ID for region select delayed screenshot hotkey
        private readonly int _OcrRegionSelectHotkeyId;       // ID for OCR region select hotkey
        private readonly int _UiElementCaptureHotkeyId;      // ID for UI element capture hotkey
        private readonly int _OpenClipboardInPaintHotkeyId;  // ID for open clipboard image in paint hotkey
        private readonly int _OpenEditorHotkeyId;            // ID for open editor hotkey
        public event Action<string>? OnScreenshotTaken;      // event to notify when a screenshot is taken
        public event Action<Bitmap>? OnScreenshotCaptured;   // event to notify when a screenshot is captured
        public event Action? OnOpenEditor;                   // event to notify when editor is opened
        public event Action? OnOcrCapture;                   // event to notify when OCR form is opened

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
            _driverForm = owner;         // assign driver form for passing to other forms
            AssignHandle(owner.Handle);  // assign window handle from owner form
            _ActiveWindowHotkeyId         = 1;
            _RegionSelectHotkeyId         = 2;
            _FreeformSelectHotkeyId       = 3;
            _ActiveWindowDelayHotkeyId    = 4;
            _RegionSelectDelayHotkeyId    = 5;
            _OcrRegionSelectHotkeyId      = 6;
            _UiElementCaptureHotkeyId     = 7;
            _OpenClipboardInPaintHotkeyId = 8;
            _OpenEditorHotkeyId           = 9;
            
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
            RegisterFromConfig(_OcrRegionSelectHotkeyId,      _config.OcrRegionCapture);
            RegisterFromConfig(_UiElementCaptureHotkeyId,     _config.UiElementCapture);
            RegisterFromConfig(_OpenClipboardInPaintHotkeyId, _config.OpenPaint);
            RegisterFromConfig(_OpenEditorHotkeyId,           _config.OpenEditor);
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
            UnregisterHotKey(_handle, _OcrRegionSelectHotkeyId);
            UnregisterHotKey(_handle, _UiElementCaptureHotkeyId);
            UnregisterHotKey(_handle, _OpenClipboardInPaintHotkeyId);
            UnregisterHotKey(_handle, _OpenEditorHotkeyId);
        }
        private void RegisterFromConfig(int id, HotkeyDefinition def)
        {
            uint mods = (uint)def.Modifiers;                // get modifier flags
            uint key = (uint)def.Key;                       // get key for keyboard hotkey
            if (!RegisterHotKey(_handle, id, mods, key))    // error handling for hotkey registration
            {
                int errorCode = Marshal.GetLastWin32Error();
                string hotkeyString = def.ToString();
                if (errorCode != 1409)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to register hotkey {hotkeyString}. Error code: {errorCode}");
                    ScreenshotMessageBox.ShowMessage(
                        $"ScreenGrab: error registering hotkeys. please relaunch app",
                        $"ScreenGrab",
                        4000);
                    throw new InvalidOperationException("Failed to register one or more hotkeys.");
                }
                if (errorCode == 1409) // hotkey already registered
                {
                    ScreenshotMessageBox.ShowMessage(
                        $"ScreenGrab: Hotkey {hotkeyString} is already in use by another application. Please choose a different hotkey for ScreenGrab OR close the conflicting application.",
                        $"ScreenGrab - Hotkey conflict",
                        10000);
                }
                System.Diagnostics.Debug.WriteLine($"Failed to register hotkeys");

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
            _config.OcrRegionCapture           = newConfig.OcrRegionCapture;
            _config.UiElementCapture           = newConfig.UiElementCapture;
            _config.OpenPaint                  = newConfig.OpenPaint;
            _config.OpenEditor                 = newConfig.OpenEditor;
            _config.ScreenshotSaveLocation     = newConfig.ScreenshotSaveLocation;
            _config.SaveToFileLocation         = newConfig.SaveToFileLocation;
            _config.AutoCopyToClipboard        = newConfig.AutoCopyToClipboard;
            _config.AutoOpenEditorOnCapture    = newConfig.AutoOpenEditorOnCapture;
            _config.SystemCaptureMode          = newConfig.SystemCaptureMode;
            _config.DelayedCaptureTimerSeconds = newConfig.DelayedCaptureTimerSeconds;
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
                else if (id == _RegionSelectHotkeyId)          // check if region select hotkey was pressed
                {
                    CaptureRegion();
                }
                else if (id == _FreeformSelectHotkeyId)        // check if freeform select hotkey was pressed
                {
                    CaptureFreeform();
                }
                else if (id == _ActiveWindowDelayHotkeyId)     // check if active window delay hotkey was pressed
                {
                    CaptureActiveWindowDelayed();
                }
                else if (id == _RegionSelectDelayHotkeyId)     // check if region select delay hotkey was pressed
                {
                    CaptureRegionDelayed();
                }
                else if (id == _OcrRegionSelectHotkeyId)       // check if OCR region select hotkey was pressed
                {
                    CaptureOcrRegion();                     // trigger event to notify OCR form should be opened
                }
                else if (id == _UiElementCaptureHotkeyId)      // check if UI element capture hotkey was pressed
                {
                    CaptureUiElement();
                }
                else if (id == _OpenClipboardInPaintHotkeyId) // check if open clipboard in paint hotkey was pressed
                {
                    OpenClipboardImageInPaint.OpenImageInPaint();
                }
                else if (id == _OpenEditorHotkeyId)           // check if open editor hotkey was pressed
                {
                    OnOpenEditor?.Invoke();                    // trigger event to notify editor should be opened
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

            // try DWM method first for accurate window dimensions with DPI scaling
            RECT rect;
            int dwmResult = DwmGetWindowAttribute(hWnd, DWMWA_EXTENDED_FRAME_BOUNDS, out rect, Marshal.SizeOf(typeof(RECT)));

            // Fall back to GetWindowRect if DWM fails
            if (dwmResult != 0)
            {
                if (!GetWindowRect(hWnd, out rect))
                {
                    ScreenshotMessageBox.ShowMessage(
                        $"ScreenGrab: Invalid Region dimension acquisition.",
                        $"ScreenGrab",
                        4000);
                    return;
                }
            }

            int width = rect.Right - rect.Left;               // calculate width
            int height = rect.Bottom - rect.Top;              // calculate height
            if (width <= 0 || height <= 0)                    // validate dimensions
            {
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: Invalid Region selection.",
                    $"ScreenGrab",
                    4000);
                return;
            }

            Rectangle area = new Rectangle(rect.Left, rect.Top, width, height); // define capture area

            // check if system capture mode is enabled before capturing
            if (_config.SystemCaptureMode)
            {
                CaptureAndSaveWithSystemInfo(hWnd, area);
            }
            else
            {
                CaptureAndSave(area, "Active Window");       // capture and save screenshot
            }
        }

        // == Capture active window with system info (largely handled in SystemInfoCapture class, this takes care of the screenshot portion and wiring it into this class == //
        private void CaptureAndSaveWithSystemInfo(IntPtr hWnd, Rectangle area)
        {
           //capture screenshot of active window area
            using Bitmap bitmap = new Bitmap(area.Width, area.Height);     // create bitmap to hold screenshot
            using Graphics g = Graphics.FromImage(bitmap);                 // create graphics object from bitmap
            {
                g.CopyFromScreen(area.Location, Point.Empty, area.Size);   // capture screenshot from specified area
            }

            // copy to clipboard if enabled
            if (_config.AutoCopyToClipboard)
            {
                Clipboard.SetImage(bitmap);
            }

            // save to file location if enabled
            if (_config.SaveToFileLocation)
            {
                //save to OneDrive folder
                string basePath = _config.ScreenshotSaveLocation;
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                string fileName = $"ActiveWindow_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string filePath = Path.Combine(basePath, fileName);
                //save bitmap as png
                try
                {
                    bitmap.Save(filePath, ImageFormat.Png);
                    OnScreenshotTaken?.Invoke(filePath);
                }
                catch (Exception ex)
                {
                    ScreenshotMessageBox.ShowMessage(
                        $"ScreenGrab: Failed to save screenshot: {ex.Message}.",
                        $"ScreenGrab:",
                        4000);
                    System.Diagnostics.Debug.WriteLine($"Failed to take screenshot: {ex.Message}");
                    throw new InvalidOperationException($"Failed to take screenshot: {ex.Message}.");
                }
            }

            // Create a copy of the bitmap for passing to the system info form
            Bitmap bitmapForForm = new Bitmap(bitmap);

            // trigger system info capture with screenshot bitmap
            var systemInfoForm = new SystemInfoCapture(hWnd, bitmapForForm);
            systemInfoForm.Show();
            systemInfoForm.Activate();
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

        // == Trigger OCR region capture event == //
        public void CaptureOcrRegion()
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

                // capture region using bitmap
                Bitmap bitmapForOcr = new Bitmap(selectedArea.Width, selectedArea.Height, PixelFormat.Format32bppArgb);

                // capture screenshot of selected area
                using (Graphics g = Graphics.FromImage(bitmapForOcr))
                {
                    g.CopyFromScreen(selectedArea.Location, Point.Empty, selectedArea.Size); // capture screenshot from specified area
                }

                // trigger ocr capture with bitmap
                var ocrForm = new OCRScreenshotForm(bitmapForOcr) 
                { 
                    Tag           = _driverForm, // pass driver form for ownership
                    ShowInTaskbar = true
                };
                ocrForm.Show();
            }
        }

        // == Trigger UI element capture event == //
        public void CaptureUiElement()
        {
            // find UiSecreenshot form if it already exists, create new one if not
            UiScreenshot? uiScreenshotForm = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form is UiScreenshot existingForm)
                {
                    uiScreenshotForm = existingForm;
                    break;
                }
            }
            if (uiScreenshotForm == null)
            {
                uiScreenshotForm = new UiScreenshot();
            }
            uiScreenshotForm.StartUiElementCapture();
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
                CaptureAndSaveFreeform(freeformPath, "Freeform Select"); // capture and save screenshot
            }
        }

        // == Form for region selection == //
        private class RegionSelectForm : Form
        {
            // variable declarations
            private bool _isSelecting;                         // flag to track if selection is in progress
            private Point _startPointScreen;                   // starting point of selection
            private Rectangle _selectedArea = Rectangle.Empty; // selected area rectangle
            private Rectangle _previousArea = Rectangle.Empty; // track previous area to minimize redraws
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
                    _previousArea = Rectangle.Empty;       // reset previous area
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

                Rectangle newArea = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                
                // only invalidate if the area changed significantly (reduces unnecessary redraws)
                if (newArea != _selectedArea)
                {
                    _selectedArea = newArea;
                    // invalidate only the affected regions instead of entire form
                    InvalidateSelectionArea();
                }
            }

            // helper method to invalidate only the affected regions
            private void InvalidateSelectionArea()
            {
                if (_previousArea != Rectangle.Empty)
                {
                    // convert to client coordinates and invalidate previous area
                    Rectangle prevClient = new Rectangle(
                        _previousArea.X - this.Bounds.X,
                        _previousArea.Y - this.Bounds.Y,
                        _previousArea.Width,
                        _previousArea.Height);
                    // add padding for pen width
                    prevClient.Inflate(10, 10);
                    Invalidate(prevClient);
                }

                if (_selectedArea != Rectangle.Empty)
                {
                    // convert to client coordinates and invalidate current area
                    Rectangle currClient = new Rectangle(
                        _selectedArea.X - this.Bounds.X,
                        _selectedArea.Y - this.Bounds.Y,
                        _selectedArea.Width,
                        _selectedArea.Height);
                    // add padding for pen width
                    currClient.Inflate(10, 10);
                    Invalidate(currClient);
                }

                _previousArea = _selectedArea;
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
                    _isSelecting  = false;               // end selection
                    _selectedArea = Rectangle.Empty;     // reset selected area
                    DialogResult  = DialogResult.Cancel; // set dialog result to Cancel
                    Close();                             // close the form
                }
            }

            // draw selection rectangle
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                
                // enable high-quality rendering
                e.Graphics.CompositingMode = CompositingMode.SourceOver;
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
                e.Graphics.SmoothingMode = SmoothingMode.None; // disable anti-aliasing for rectangles (faster)
                
                using (var overlayBrush = new SolidBrush(Color.FromArgb(255, Color.DarkGray))) // semi-transparent overlay brush
                {
                    
                    if (_isSelecting && _selectedArea != Rectangle.Empty)
                    {
                        // convert selected area to client coordinates for drawing
                        Rectangle clientRectangle = new Rectangle(
                            _selectedArea.X - this.Bounds.X,
                            _selectedArea.Y - this.Bounds.Y,
                            _selectedArea.Width,
                            _selectedArea.Height);
                        using (Region formRegion = new Region(this.ClientRectangle))
                        {
                            formRegion.Exclude(clientRectangle);                     // exclude selected area from form region
                            e.Graphics.FillRegion(overlayBrush, formRegion);         // fill remaining area with overlay
                        }
                        // pen for selection rectangle
                        using (var selectionPen = new Pen(Color.White, 3))
                        {
                            selectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            e.Graphics.DrawRectangle(selectionPen, clientRectangle); // draw selection rectangle
                        }
                        // dashed overlay
                        using (var dashedPen = new Pen(Color.Red, 3))
                        {
                            dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            e.Graphics.DrawRectangle(dashedPen, clientRectangle);
                        }
                    }
                    else
                    {
                        e.Graphics.FillRectangle(overlayBrush, this.ClientRectangle); // fill entire form with overlay
                    }
                }
            }

            // constructor to setup form properties and event handlers
            public RegionSelectForm()
            {
                // setup form properties
                var vs = System.Windows.Forms.SystemInformation.VirtualScreen; // get virtual screen dimensions
                this.FormBorderStyle = FormBorderStyle.None;                   // no border
                this.StartPosition   = FormStartPosition.Manual;               // manual position
                this.Bounds          = vs;                                     // cover entire virtual screen
                this.BackColor       = Color.White;                            // white background
                this.TransparencyKey = Color.White;                            // make white color transparent
                this.Opacity         = .15;                                    // semi-transparent
                this.TopMost         = true;                                   // always on top
                this.DoubleBuffered  = true;                                   // reduce flicker
                this.KeyPreview      = true;                                   // enable key preview
                Cursor = Cursors.Cross;
                
                // optimize rendering
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | 
                              ControlStyles.AllPaintingInWmPaint | 
                              ControlStyles.UserPaint, true);
                this.UpdateStyles();
                
                // attach event handlers
                this.KeyDown   += new KeyEventHandler(EscKeyDown);
                this.MouseDown += new MouseEventHandler(onMouseDown);
                this.MouseMove += new MouseEventHandler(OnMouseMove);
                this.MouseUp   += new MouseEventHandler(OnMouseUp);
            }
        }

        // == Capture active window after delay == //
        public async void CaptureActiveWindowDelayed()
        {
            int delaySeconds = _config.DelayedCaptureTimerSeconds; // get delay time from config
            ScreenshotMessageBox.ShowMessage(
                $"ScreenGrab: Delayed capture in 5 seconds...",
                $"ScreenGrab",
                2000);
            await Task.Delay(delaySeconds * 1000); // 5 second delay
            CaptureActiveWindow();
        }
        // == Capture selected region after delay == //
        public async void CaptureRegionDelayed()
        {
            int delaySeconds = _config.DelayedCaptureTimerSeconds; // get delay time from config
            ScreenshotMessageBox.ShowMessage(
                $"ScreenGrab: Delayed capture in 5 seconds...",
                $"ScreenGrab",
                2000);
            await Task.Delay(delaySeconds * 1000); // 5 second delay
            CaptureRegion();
        }

        // == freeform selection form == //
        private class FreeformSelectForm : Form
        {
            private bool _isDrawing;                               // flag to track if drawing is in progress
            private List<Point> _freeformPath = new List<Point>(); // list to hold freeform path points
            private Rectangle _previousBounds = Rectangle.Empty;   // track previous bounds to minimize redraws
            public List<Point> FreeformPath => _freeformPath;      // public property to access freeform path

            // start freeform drawing on left mouse click
            private void OnMouseDown(object? sender, MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left) return;    // only start drawing on left mouse button
                _isDrawing = true;
                _freeformPath.Clear();                        // clear existing path //prevents unwanted behavior
                _freeformPath.Add(PointToScreen(e.Location)); // add starting point
                _previousBounds = Rectangle.Empty;            // reset previous bounds
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
                    InvalidateFreeformArea();                  // invalidate only affected area
                }
            }

            // helper method to invalidate only the affected regions
            private void InvalidateFreeformArea()
            {
                if (_freeformPath.Count < 2) 
                {
                    Invalidate();
                    return;
                }

                // calculate bounding rectangle for the freeform path
                int minX = _freeformPath.Min(p => p.X);
                int minY = _freeformPath.Min(p => p.Y);
                int maxX = _freeformPath.Max(p => p.X);
                int maxY = _freeformPath.Max(p => p.Y);

                Rectangle currentBounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);

                // invalidate previous bounds if exists
                if (_previousBounds != Rectangle.Empty)
                {
                    Rectangle prevClient = new Rectangle(
                        _previousBounds.X - this.Bounds.X,
                        _previousBounds.Y - this.Bounds.Y,
                        _previousBounds.Width,
                        _previousBounds.Height);
                    // add padding for pen width
                    prevClient.Inflate(10, 10);
                    Invalidate(prevClient);
                }

                // invalidate current bounds
                Rectangle currClient = new Rectangle(
                    currentBounds.X - this.Bounds.X,
                    currentBounds.Y - this.Bounds.Y,
                    currentBounds.Width,
                    currentBounds.Height);
                // add padding for pen width
                currClient.Inflate(10, 10);
                Invalidate(currClient);

                _previousBounds = currentBounds;
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

                // optimize rendering settings
                e.Graphics.CompositingMode = CompositingMode.SourceOver;
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;

                using (var overlayBrush = new SolidBrush(Color.FromArgb(120, Color.Black))) // semi-transparent overlay brush
                {
                    // draw freeform path if it has points
                    if (_freeformPath.Count > 1)
                    {
                        // convert freeform path points to client coordinates for drawing
                        Point[] clientPoints = _freeformPath
                            .Select(p => new Point(p.X - this.Bounds.X, p.Y - this.Bounds.Y))
                            .ToArray();
                        // draw filled overlay excluding freeform area 
                        if (clientPoints.Length >= 3)
                        {
                            using (var graphicsPath = new GraphicsPath())
                            {
                                graphicsPath.AddPolygon(clientPoints);               // create polygon from freeform path
                                using (Region formRegion = new Region(this.ClientRectangle))
                                {
                                    formRegion.Exclude(graphicsPath);                // exclude freeform area from form region
                                    e.Graphics.FillRegion(overlayBrush, formRegion); // fill remaining area with overlay
                                }
                                // anti-aliasing for smoother lines
                                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                using (var PenFreeform = new Pen(Color.White, 3))               // pen for freeform path
                                {
                                    e.Graphics.DrawLines(PenFreeform, clientPoints);          // draw freeform path
                                }
                                //dashed overlay
                                using (var PenDashedFreeform = new Pen(Color.Red, 3))
                                {
                                    PenDashedFreeform.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                                    e.Graphics.DrawLines(PenDashedFreeform, clientPoints);
                                }
                            }
                        }
                        // draw partial paths for 1 or 2 points // handles edge cases and prevents errors
                        else if (clientPoints.Length > 0)
                        {
                            e.Graphics.FillRectangle(overlayBrush, this.ClientRectangle); // fill entire form with overlay
                            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            using (var PenFreeform = new Pen(Color.Red, 7))               // pen for freeform path
                            {
                                if (clientPoints.Length == 2)
                                {
                                    e.Graphics.DrawLine(PenFreeform, clientPoints[0], clientPoints[1]); // draw line for two points
                                }
                                else if (clientPoints.Length == 1)
                                {
                                    e.Graphics.DrawEllipse(PenFreeform, clientPoints[0].X - 2, clientPoints[0].Y - 2, 4, 4); // draw single point as small circle
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Graphics.FillRectangle(overlayBrush, this.ClientRectangle); // fill entire form with overlay
                    }
                }
            }

            // == Capture freeform region constructor & properties setup // should mirror look of region select == //
            public FreeformSelectForm()
            {
                var vs = System.Windows.Forms.SystemInformation.VirtualScreen; // get virtual screen dimensions
                this.FormBorderStyle = FormBorderStyle.None;                   // no border
                this.StartPosition   = FormStartPosition.Manual;               // manual position
                this.Bounds          = vs;                                     // cover entire virtual screen
                this.BackColor       = Color.White;                            // white background
                this.TransparencyKey = Color.White;                            // make white color transparent
                this.Opacity         = .15;                                    // semi-transparent
                this.TopMost         = true;                                   // always on top
                this.DoubleBuffered  = true;                                   // reduce flicker

                // optimize rendering
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | 
                              ControlStyles.AllPaintingInWmPaint | 
                              ControlStyles.UserPaint, true);
                this.UpdateStyles();

                // event handlers
                this.MouseDown += new MouseEventHandler(OnMouseDown);
                this.MouseMove += new MouseEventHandler(OnMouseMove);
                this.MouseUp   += new MouseEventHandler(OnMouseUp);
                this.KeyDown   += new KeyEventHandler(EscKeyDown);
                this.KeyPreview = true; // enable key preview for escape key
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
            if (_config.AutoCopyToClipboard)
            {
                Clipboard.SetImage(bitmap);
            }

            //save to file location if enabled
            if (_config.SaveToFileLocation)
            {
                //save to OneDrive folder
                string basePath = _config.ScreenshotSaveLocation;
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                string fileName = $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string filePath = Path.Combine(basePath, fileName);

                //save bitmap as png
                try
                {
                    bitmap.Save(filePath, ImageFormat.Png);
                    OnScreenshotTaken?.Invoke(filePath);
                }
                catch (Exception ex)
                {
                    ScreenshotMessageBox.ShowMessage(
                        $"ScreenGrab: Failed to save screenshot: {ex.Message}.",
                        $"ScreenGrab:",
                        4000);
                    System.Diagnostics.Debug.WriteLine($"Failed to take screenshot: {ex.Message}");
                    throw new InvalidOperationException($"Failed to take screenshot: {ex.Message}.");
                }
            }
            else
            {
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: Screenshot taken.",
                    $"ScreenGrab:",
                    4000);
            }
            if (_config.AutoOpenEditorOnCapture)
            {
                Bitmap bitmapCopy = new Bitmap(bitmap);   // create a copy of the bitmap to pass to the editor
                OnScreenshotCaptured?.Invoke(bitmapCopy); // trigger event to notify screenshot captured
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
            if (_config.AutoCopyToClipboard)
            {
                Clipboard.SetImage(maskedCapture);
            }

            if (_config.SaveToFileLocation)
            {
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
            else
            {
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: Screenshot taken.",
                    $"ScreenGrab:",
                    4000);
            }
            if (_config.AutoOpenEditorOnCapture)
            {
                Bitmap bitmapCopy = new Bitmap(maskedCapture);   // create a copy of the bitmap to pass to the editor
                OnScreenshotCaptured?.Invoke(bitmapCopy);        // trigger event to notify screenshot captured
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