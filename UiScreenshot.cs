///// Class which allows for taking screenshots of specifc UI elements on screen /////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Windows.Forms;
using UIAutomationClient;

namespace ScreenGrab
{
    public partial class UiScreenshot : Form
    {
        // == private variable == //
        private Bitmap? _capturedImage;
        private IUIAutomationElement? _capturedElement;

        public UiScreenshot()
        {
            InitializeComponent();
            // SetupEventHandler();
        }

        // == event handlers == //
        //private void SetupEventHandler()
        //{
        //    copyImageToolStripMenuItem.Click += CopyImageToolStripMenuItem_Click;
        //    saveImageToolStripMenuItem.Click += SaveImageToolStripMenuItem_Click;
        //    copyTextToolStripMenuItem.Click += CopyTextToolStripMenuItem_Click;
        //    exportAllAsMarkdownToolStripMenuItem.Click += ExportAllAsMarkdownToolStripMenuItem_Click;
        //    sendImageToEditorToolStripMenuItem.Click += SendImageToEditorToolStripMenuItem_Click;
        //    btnSendHome.Click += BtnSendHome_Click;
        //}

        // == initiate UI element capture == //
        public void StartUiElementCapture()
        {
            this.Hide();
            Application.DoEvents();

            // message to indicate capture is starting
            ScreenshotMessageBox.ShowMessage("Select a UI element by clicking on it. Press ESC to cancel.", "ScreenGrab", 4000);

            using var selector = new UiElementSelectorForm();
            if (selector.ShowDialog() == DialogResult.OK && selector.CapturedImage != null)
            {
                _capturedImage = selector.CapturedImage;
                _capturedElement = selector.SelectedElement;

                // display captured image
                pictureBoxScreenshot.Image = _capturedImage;
                pictureBoxScreenshot.SizeMode = PictureBoxSizeMode.Zoom;

                // display captured element info
                PopulateElementProperties(selector.SelectedElement);
            }
            this.Show();
            this.BringToFront();
        }

        // == gather element propertiies == //
        private void PopulateElementProperties(IUIAutomationElement element)
        {
            if (element == null)
            {
                textBox1.Text = "No element captured.";
                return;
            }
            try
            {
                // Helper to get ControlType name
                var sb = new StringBuilder();
                // build properties
                sb.AppendLine("=== UI Element Properties ===");
                sb.AppendLine();
                sb.AppendLine($"ControlType:    {GetControlTypeName(element.CurrentControlType)}");
                sb.AppendLine($"Name:           {element.CurrentName}");
                sb.AppendLine($"AutomationId:   {element.CurrentAutomationId}");
                sb.AppendLine($"ClassName:      {element.CurrentClassName}");
                sb.AppendLine();
                sb.AppendLine("=== Additional Properties ===");
                sb.AppendLine();
                sb.AppendLine($"LocalizedControlType: {element.CurrentLocalizedControlType}");
                sb.AppendLine($"IsEnabled:      {element.CurrentIsEnabled != 0}");
                sb.AppendLine($"IsOffscreen:    {element.CurrentIsOffscreen != 0}");
                sb.AppendLine($"ProcessId:      {element.CurrentProcessId}");
                sb.AppendLine($"FrameworkId:    {element.CurrentFrameworkId}");

                // Bounding Rectangle
                var rect = element.CurrentBoundingRectangle;
                sb.AppendLine($"BoundingRect:   X={rect.left}, Y={rect.top}, W={rect.right - rect.left}, H={rect.bottom - rect.top}");

                // Optional properties
                if (!string.IsNullOrEmpty(element.CurrentHelpText))
                    sb.AppendLine($"HelpText:       {element.CurrentHelpText}");

                if (!string.IsNullOrEmpty(element.CurrentAccessKey))
                    sb.AppendLine($"AccessKey:      {element.CurrentAccessKey}");

                if (!string.IsNullOrEmpty(element.CurrentAcceleratorKey))
                    sb.AppendLine($"AcceleratorKey: {element.CurrentAcceleratorKey}");

                // Set text box content
                textBox1.Text = sb.ToString();
                textBox1.ForeColor = Color.White;
            }
            catch (COMException comEx)
            {
                textBox1.Text = $"Error retrieving element properties: {comEx.Message}";
                return;
            }
            catch (Exception ex)
            {
                textBox1.Text = $"Unexpected error: {ex.Message}";
                return;
            }
        }
        // == helper to get ControlType name from int == //
        private static string GetControlTypeName(int controlTypeId)
        {
            return controlTypeId switch
            {
                50000 => "Button",
                50001 => "Calendar",
                50002 => "CheckBox",
                50003 => "ComboBox",
                50004 => "Edit",
                50005 => "Hyperlink",
                50006 => "Image",
                50007 => "ListItem",
                50008 => "List",
                50009 => "Menu",
                50010 => "MenuBar",
                50011 => "MenuItem",
                50012 => "ProgressBar",
                50013 => "RadioButton",
                50014 => "ScrollBar",
                50015 => "Slider",
                50016 => "Spinner",
                50017 => "StatusBar",
                50018 => "Tab",
                50019 => "TabItem",
                50020 => "Text",
                50021 => "ToolBar",
                50022 => "ToolTip",
                50023 => "Tree",
                50024 => "TreeItem",
                50025 => "Custom",
                50026 => "Group",
                50027 => "Thumb",
                50028 => "DataGrid",
                50029 => "DataItem",
                50030 => "Document",
                50031 => "SplitButton",
                50032 => "Window",
                50033 => "Pane",
                50034 => "Header",
                50035 => "HeaderItem",
                50036 => "Table",
                50037 => "TitleBar",
                50038 => "Separator",
                _ => $"Unknown ({controlTypeId})"
            };
        }

        // == Overlay for UI element selection == //
        internal class UiElementSelectorForm : Form
        {
            [DllImport("user32.dll")]
            private static extern bool SetProcessDPIAware();
            private readonly IUIAutomation _automation;
            private IUIAutomationElement? _hoveredElement;
            private tagRECT _hoveredRect;
            private readonly System.Windows.Forms.Timer _hoverTimer;

            public IUIAutomationElement? SelectedElement { get; private set; }
            public Bitmap? CapturedImage { get; private set; }

            // == sets up overlay form == //
            public UiElementSelectorForm()
            {
                SetProcessDPIAware();                            // Ensure DPI awareness
                _automation = new CUIAutomation();               // Initialize UI Automation

                var vs = SystemInformation.VirtualScreen;        // Get virtual screen dimensions
                this.FormBorderStyle = FormBorderStyle.None;     // No border
                this.StartPosition = FormStartPosition.Manual;   // Manual positioning
                this.Bounds = vs;                                // Full screen and all screens
                this.TopMost = true;                             // Always on top
                this.Cursor = Cursors.Default;                     // Crosshair cursor
                this.DoubleBuffered = true;                      // Reduce flicker
                this.ShowInTaskbar = false;                      // Don't show in taskbar

                // Capture the screen as background, then overlay on top
                _backgroundImage = CaptureScreen(vs);
                this.BackgroundImage = _backgroundImage;
                this.BackgroundImageLayout = ImageLayout.None;

                this.MouseClick += OnMouseClick;                 // Mouse click event
                this.MouseMove += OnMouseMove;                   // Mouse move event
                this.KeyDown += OnKeyDown;                       // Key down event
                this.KeyPreview = true;                          // Capture key events

                // Timer to update hovered element
                _hoverTimer = new System.Windows.Forms.Timer { Interval = 50 };
                _hoverTimer.Tick += HoverTimer_Tick;
                _hoverTimer.Start();
            }

            private Bitmap? _backgroundImage;

            private static Bitmap CaptureScreen(Rectangle bounds)
            {
                var bmp = new Bitmap(bounds.Width, bounds.Height);
                using var g = Graphics.FromImage(bmp);
                g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
                return bmp;
            }

            // == Hover timer tick == //
            private void HoverTimer_Tick(object? sender, EventArgs e)
            {
                try
                {
                    var screenPoint = Control.MousePosition;
                    // Temporarily hide to detect elements underneath
                    this.Visible = false;
                    Application.DoEvents();

                    var element = _automation.ElementFromPoint(new tagPOINT
                    {
                        x = screenPoint.X,
                        y = screenPoint.Y
                    });

                    // Show the overlay again
                    this.Visible = true;

                    if (element != null)
                    {
                        var newRect = element.CurrentBoundingRectangle;
                        // Only update if the element or its bounds changed
                        if (_hoveredElement == null ||
                            newRect.left != _hoveredRect.left ||
                            newRect.top != _hoveredRect.top ||
                            newRect.right != _hoveredRect.right ||
                            newRect.bottom != _hoveredRect.bottom)
                        {
                            _hoveredElement = element;
                            _hoveredRect = newRect;
                            Invalidate(); // Trigger repaint
                        }
                    }
                }
                catch
                {
                    // Ignore exceptions during hover detection
                }
            }

            // == Paint event to draw hovered rectangle == //
            private void OnMouseMove(object? sender, MouseEventArgs e)
            {
                // Handled by hover timer
            }

            // == Mouse click event to select element == //
            private void OnMouseClick(object? sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left && _hoveredElement != null)
                {
                    _hoverTimer.Stop();
                    SelectedElement = _hoveredElement;
                    CaptureElementScreenshot();
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }

            // == Key down event to cancel selection == //
            private void OnKeyDown(object? sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    _hoverTimer.Stop();
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            }

            // == Capture screenshot of selected element == //
            private void CaptureElementScreenshot()
            {
                if (SelectedElement == null) return;
                try
                {
                    var rect = SelectedElement.CurrentBoundingRectangle;
                    int width = rect.right - rect.left;
                    int height = rect.bottom - rect.top;
                    if (width <= 0 || height <= 0) return;

                    // hide overlay before capture
                    this.Hide();
                    Application.DoEvents();

                    // capture screen area
                    CapturedImage = new Bitmap(width, height);
                    using var g = Graphics.FromImage(CapturedImage);
                    g.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height));
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Failed to capture element screenshot.");
                }
            }

            // == Paint event override to draw rectangle == //
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                int width = _hoveredRect.right - _hoveredRect.left;
                int height = _hoveredRect.bottom - _hoveredRect.top;

                if (width <= 0 || height <= 0) return;

                // convert to client coordinates
                var clientRect = new Rectangle(
                    _hoveredRect.left - this.Bounds.X,
                    _hoveredRect.top - this.Bounds.Y,
                    width,
                    height);

                // draw semi transparent rectangle excluding hovered element
                using (var OverlayBrush = new SolidBrush(Color.FromArgb(80, Color.Black)))
                using (var formRegion = new Region(this.ClientRectangle))
                {
                    formRegion.Exclude(clientRect);
                    e.Graphics.FillRegion(OverlayBrush, formRegion);
                }

                // draw border around hovered element
                using (var borderPen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(borderPen, clientRect);
                }
                DrawElementTooltip(e.Graphics, clientRect);
            }

            // == Draw tooltip with element info == //
            private void DrawElementTooltip(Graphics g, Rectangle elementRect)
            {
                if (_hoveredElement == null) return;
                try
                {
                    string controlType = GetControlTypeName(_hoveredElement.CurrentControlType);
                    string name = _hoveredElement.CurrentName ?? "";
                    string displayText = string.IsNullOrEmpty(name)
                    ? controlType
                    : $"{controlType}: {(name.Length > 40 ? name[..40] + "..." : name)}";

                    // define font
                    using var font = new Font("Segoe UI", 10, FontStyle.Bold);
                    var textSize = g.MeasureString(displayText, font);
                    var tooltipRect = new Rectangle(
                        elementRect.Left,
                        elementRect.Top - (int)textSize.Height - 8,
                        (int)textSize.Width + 12,
                        (int)textSize.Height + 6);

                    // ensure tooltip is within screen bounds
                    if (tooltipRect.Top < 0)
                    {
                        tooltipRect.Y = elementRect.Bottom + 8;
                    }

                    using (var bgBrush = new SolidBrush(Color.FromArgb(230, Color.Black)))
                        g.FillRectangle(bgBrush, tooltipRect);

                    using (var borderPen = new Pen(Color.Red, 2))
                        g.DrawRectangle(borderPen, tooltipRect);

                    using (var textBrush = new SolidBrush(Color.White))
                        g.DrawString(displayText, font, textBrush, tooltipRect.X + 6, tooltipRect.Y + 3);
                }
                catch
                {
                    // Ignore exceptions during tooltip drawing
                }
            }
            // == dispose of timer == //
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _hoverTimer?.Stop();
                    _hoverTimer?.Dispose();
                    _backgroundImage?.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        // == copy image to clipboard == //
        private void CopyImageToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (_capturedImage != null)
            {
                Clipboard.SetImage(_capturedImage);
                ScreenshotMessageBox.ShowMessage("Image copied to clipboard.", "ScreenGrab", 1500);
            }
        }

        // == save image to file == //
        private void SaveImageToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (_capturedImage == null) return;

            using var saveDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp",
                Title = "Save UI Element Screenshot",
                FileName = $"UIElement_{DateTime.Now:yyyyMMdd_HHmmss}"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var format = saveDialog.FilterIndex switch
                {
                    2 => ImageFormat.Jpeg,
                    3 => ImageFormat.Bmp,
                    _ => ImageFormat.Png
                };
                _capturedImage.Save(saveDialog.FileName, format);
                ScreenshotMessageBox.ShowMessage($"Image saved: {saveDialog.FileName}", "ScreenGrab", 2000);
            }
        }

        // == copy element properties text to clipboard == //
        private void CopyTextToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                Clipboard.SetText(textBox1.Text);
                ScreenshotMessageBox.ShowMessage("Element properties copied to clipboard.", "ScreenGrab", 2000);
            }
        }

        // == send image to editor == //
        private void SendImageToEditorToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (_capturedImage == null) return;

            var editorForm = new ImageEditorForm(new Bitmap(_capturedImage));
            editorForm.Show();
        }

        // == button to return to main form == //
        private void BtnSendHome_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        // == export all info as markdown == //
        private void ExportAllAsMarkdownToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (_capturedImage == null) return;

            using var saveDialog = new SaveFileDialog
            {
                Filter = "Markdown|*.md",
                Title = "Export as Markdown",
                FileName = $"UIElement_{DateTime.Now:yyyyMMdd_HHmmss}"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var imagePath = Path.ChangeExtension(saveDialog.FileName, ".png");
                _capturedImage.Save(imagePath, ImageFormat.Png);

                var markdown = $"# UI Element Capture\n\n![UI Element]({Path.GetFileName(imagePath)})\n\n```\n{textBox1.Text}\n```";
                File.WriteAllText(saveDialog.FileName, markdown);
                ScreenshotMessageBox.ShowMessage($"Exported: {saveDialog.FileName}", "ScreenGrab", 2000);
            }
        }
    }
}
