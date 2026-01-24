///// Manages the image editor and functionalities //////

using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ScreenGrab
{
    public partial class ImageEditorForm : Form
    {
        // enum for drawing tools
        private enum DrawingTool
        {
            None,
            Rectangle,
            Arrow,
            Freeform,
            Text,
            Highlight,
            Blur,
            Line,
            Crop
        }
        // == private variables == //
        private Image?         _currentImage;                                     // local variable for storing loaded iamge
        private Bitmap?        _editableImage;                                    // local variable for storing editable image
        private DrawingTool    _activeDrawingTool = DrawingTool.None;             // currently selected drawing tool // initially none
        private Point          _drawStartPoint;                                   // starting point for drawing
        private Point          _drawEndPoint;                                     // ending point for drawing
        private bool           _isDrawing = false;                                // flag to indicate if drawing is in progress
        private Color          _SelectedColor = Color.Red;                        // default color (red) for drawing
        private Pen?           _currentPen;                                       // pen for drawing shapes
        private List<Point>    _freeformPoints = new List<Point>();               // points for freeform drawing
        private int            _brushSize = 5;                                    // variable brush size for drawing including default
        // text tool variables
        private RichTextBox?   _textBox;
        private Point          _textStartPoint;
        private Font           _textFont = new Font("Arial", 16, FontStyle.Bold); // default font for text tool
        private bool           _isTextInputActive = false;                        // flag to indicate if text input is active
        // undo tool variables 
        private Stack <Bitmap> _undoStack = new Stack<Bitmap>();                  // stack to store previous image states for undo functionality
        private const int      MaxUndoSteps = 30;                                 // maximum number of undo steps to store
        // zoom variables
        private float          _zoomFactor  = 1.0f;                               // current zoom factor
        private const float    ZoomMin  = 0.1f;                                   // minimum zoom level
        private const float    ZoomMax  = 5.0f;                                   // maximum zoom level
        private const float    ZoomStep = 0.1f;                                   // zoom step increment
        // crop variables
        private Bitmap?   _originalImageBeforeCrop;                               // store original image before crop operation
        private bool      _isCropping = false;                                    // flag to indicate if cropping is in progress

        // == constructor == //
        public ImageEditorForm()
        {
            InitializeComponent();
            _currentPen = new Pen(_SelectedColor, 5); // initialize pen with default color and width
            UpdateColorButtonDisplay();               // update button display with default color

            // wire resize event for repositioning toolstrips
            this.Resize += ImageEditorForm_Resize;
            PositionToolStripsRelativeToPanel();

            // wire up mouse events for drawing
            pictureBoxImage.MouseDown  += pictureBoxImage_MouseDown;
            pictureBoxImage.MouseMove  += pictureBoxImage_MouseMove;
            pictureBoxImage.MouseUp    += pictureBoxImage_MouseUp;
            pictureBoxImage.Paint      += pictureBoxImage_Paint;
            pictureBoxImage.MouseWheel += pictureBoxImage_MouseWheel; // for zooming

            // wire crop tool button events 
            // toolStripBtnCrop.Click      += btnCrop_Click; // this MUST be wired in designer to avoid conflict with other click events. here for mouse even reference
            toolStripBtnResetCrop.Click += btnResetCrop_Click;

            // for ctrl + Z for undo shortcut
            this.KeyPreview = true; // allow form to capture key events
            this.KeyDown += ImageEditorForm_KeyDown;
        }

        //== KEYBOARD SHORTCUT HANDLING == //
        private void ImageEditorForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // handle Ctrl + Z for undo
            if (e.Control && e.KeyCode == Keys.Z)
            {
                Undo();
                e.Handled = true; // prevent further processing
            }
            // ctrl + 0 to reset zoom
            else if (e.Control && e.KeyCode == Keys.D0)
            {
                ResetZoom();
                e.Handled = true; // prevent further processing
            }
            // escape key to cancel current selection
            else if (e.KeyCode == Keys.Escape)
            {
                if (_isTextInputActive)
                {
                    CancelTextInput();
                }
                else if (_isDrawing)
                {
                    _isDrawing  = false;          // reset drawing flag
                    _freeformPoints.Clear();      // clear freeform points
                    pictureBoxImage.Invalidate(); // request redraw to clear preview
                }
                else if (_isCropping)
                {
                    _isCropping = false;
                    _activeDrawingTool = DrawingTool.None; // deactivate crop tool
                    pictureBoxImage.Invalidate();          // request redraw to clear crop rectangle
                }
                else
                {
                    e.Handled = true; // prevent further processing
                    this.Close();     // close the editor
                }
            }
        }
        // == REPOSITION TOOLSTRIPS ON RESIZE == //
        private void ImageEditorForm_Resize(object? sender, EventArgs e)
        {
            PositionToolStripsRelativeToPanel();

            // apply zoom to image after form resize
            if (_editableImage != null)
            {
                ApplyZoom();
            }
        }
        private void PositionToolStripsRelativeToPanel()
        {
            // position EditorToolStrip at top left of panel
            EditorToolStrip.Location = new Point(
                panelImageContainer.Left,
                panelImageContainer.Top - EditorToolStrip.Height - 3);
            ZoomToolStrip.Location = new Point(
                panelImageContainer.Right - ZoomToolStrip.Width,
                panelImageContainer.Top - ZoomToolStrip.Height - 3);
        }
        // == constructor to load image from path == //
        public ImageEditorForm(string imagePath) : this()
        {
            LoadImage(imagePath);
        }
        // == load image from file path == //
        private void LoadImage(string imagePath)
        {
            try
            {
                _currentImage?.Dispose();                                              // dispose previous image if any
                _editableImage?.Dispose();                                             // dispose previous editable image if any
                _originalImageBeforeCrop?.Dispose();                                   // dispose previous original image before crop if any
                _originalImageBeforeCrop = null;                                       // reset original image before crop
                _currentImage = Image.FromFile(imagePath);                             // load image from file
                _editableImage = new Bitmap(_currentImage);                            // create editable bitmap copy
                pictureBoxImage.Image = _editableImage;                                // assign image to picture box
                this.Text = $"Image Editor - {System.IO.Path.GetFileName(imagePath)}"; // set form title
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // == construcotr to load image from Image object (used to load from clipboard) == //
        public ImageEditorForm(Image image) : this()
        {
            LoadImageFromObject(image);
        }
        // == load image from Image object == //
        private void LoadImageFromObject(Image image)
        {
            try
            {
                // Store a copy of the image to avoid issues with disposed objects
                var _oldCurrentImage = _currentImage;
                var _oldEditableImage = _editableImage;

                _currentImage = new Bitmap(image);         // create copy of image
                _editableImage = new Bitmap(image);        // create editable bitmap copy
                pictureBoxImage.Image = _editableImage;            // assign image to picture box
                _oldCurrentImage?.Dispose();                          // dispose previous image if any
                _oldEditableImage?.Dispose();                         // dispose previous editable image if any
                this.Text = $"Image Editor - Clipboard Image";     // set form title
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // == MENUSTRIP: CLICK BUTTON == OPEN FILE EXPLORER TO LOAD IMAGE == //
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select an Image";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadImage(openFileDialog.FileName); // load selected image
                }
            }
        }

        // == MENUSTRIP: CLICK BUTTON == SAVE EDITED IMAGE TO FILE == //
        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (_editableImage == null)
            {
                ScreenshotMessageBox.ShowMessage(
                $"There is no image in editor to save",
                $"ScreenGrab:",
                4000);
                return;
            }
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Save Edited Image";
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|GIF Image|*.gif";
                saveFileDialog.DefaultExt = "png";
                saveFileDialog.FileName = $"EditedImage_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // determine image format based on selected extension
                        System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;
                        string extension = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();
                        switch (extension)
                        {
                            case ".jpg":
                            case ".jpeg":
                                format = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case ".bmp":
                                format = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            case ".gif":
                                format = System.Drawing.Imaging.ImageFormat.Gif;
                                break;
                        }
                        _editableImage.Save(saveFileDialog.FileName, format); // save image to file
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        // == MENUSTRIP: CLICK BUTTON == COPY IMAGE TO CLIPBOARD == //
        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            if (_editableImage != null)
            {
                Clipboard.SetImage(_editableImage); // copy image to clipboard
                ScreenshotMessageBox.ShowMessage(
                $"Edited image copied to clipboard",
                $"ScreenGrab:",
                4000);
            }
            else
            {
                ScreenshotMessageBox.ShowMessage(
                $"There is no image in editor to copy",
                $"ScreenGrab:",
                4000);
            }
        }

        // == MENUSTRIP: CLICK BUTTON == OPEN IMAGE IN MS PAINT - INCLUDE EDITS == //
        private void btnOpenInPaint_Click(object sender, EventArgs e)
        {
            if (_editableImage == null)
            {
                ScreenshotMessageBox.ShowMessage(
                $"There is no image in editor to open in MS Paint",
                $"ScreenGrab:",
                4000);
                return;
            }

            // save edited image to clipboard
            Clipboard.SetImage(_editableImage);

            // start MS Paint process
            OpenClipboardImageInPaint.OpenImageInPaint();
        }


        // == TOOLSTRIP: FUNCTIONALITY FOR SELECTING COLOR FOR DRAWING == //
        // button to select color for drawing
        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color            = _SelectedColor; // set current selected color
                colorDialog.FullOpen         = true;           // allow full color selection
                colorDialog.AllowFullOpen    = true;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _SelectedColor = colorDialog.Color;                // update selected color
                    _currentPen?.Dispose();                            // dispose previous pen
                    _currentPen = new Pen(_SelectedColor, _brushSize); // create new pen with selected color
                    UpdateColorButtonDisplay();                        // update button display
                }
            }
        }

        // == update button display w/ currently selected color == //
        private void UpdateColorButtonDisplay()
        {
            int _buttonSize = 16;                                      // size of color display square
            Bitmap colorBitmap = new Bitmap(_buttonSize, _buttonSize); // create bitmap for color display
            using (Graphics g = Graphics.FromImage(colorBitmap))
            {
                // fill background with button face color
                using (Brush brush = new SolidBrush(_SelectedColor))
                {
                    g.FillRectangle(brush, 0, 0, _buttonSize, _buttonSize); // fill rectangle with selected color
                }
                // draw border around color square
                using (Pen borderPen = new Pen(Color.White))
                {
                    g.DrawRectangle(borderPen, 0, 0, _buttonSize - 1, _buttonSize - 1); // draw border
                }
            }
            // set button image to color bitmap
            var oldImage = btnSelectColor.Image;
            btnSelectColor.Image = colorBitmap;
            oldImage?.Dispose(); // dispose previous image
        }
        // == END OF COLOR SELECTION FUNCTIONALITY == //

        // == BRUSH SIZE SELECTION FUNCTIONALITY == //
        private void trackBarBrushSize_Scroll(object sender, EventArgs e)
        {
            _brushSize = trackBarBrushSize.Value;                 // update brush size
            _currentPen?.Dispose();                              // dispose previous pen
            _currentPen = new Pen(_SelectedColor, _brushSize);   // create new pen with updated size

            // Update text font size depending on brush size for consistency
            _textFont.Dispose();                                           // dispose previous font
            _textFont = new Font("Arial", _brushSize + 8, FontStyle.Bold); // create new font with updated size
        }

        // == MENUSTRIP: UNDO FUNCTIONALITY == //
        private void SaveStateForUndo()
        {
            if (_editableImage == null) return;

            // limit undo stack size
            if (_undoStack.Count >= MaxUndoSteps)
            {
                var items = _undoStack.ToArray();
                _undoStack.Clear();
                for (int i = items.Length - 2; i >= 0; i--)
                {
                    _undoStack.Push(items[i]); // keep all but the oldest state
                }
                items[items.Length - 1].Dispose(); // dispose oldest state
            }
            // push current state onto stack
            _undoStack.Push(new Bitmap(_editableImage));
        }

        // undo functionality
        private void Undo()
        {
            if (_undoStack.Count > 0 && _editableImage != null)
            {
                _editableImage.Dispose();
                _editableImage = _undoStack.Pop(); // pop last state
                pictureBoxImage.Image = _editableImage; // update picture box
                pictureBoxImage.Invalidate();          // refresh display
            }
        }
        // button to undo last action
        private void btnUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        // == END OF UNDO FUNCTIONALITY == //

        // == CROP TOOL FUNCTIONALITY == //
        // == button to activate crop tool == //
        private void btnCrop_Click(object? sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Crop tool activated"); // debug log
            ActivateDrawingTool(DrawingTool.Crop);
        }

        // button to reset crop
        private void btnResetCrop_Click(object? sender, EventArgs e)
        {
            ResetCrop();
        }

        // == reset crop operation == //
        private void ResetCrop()
        {
            if (_originalImageBeforeCrop != null && _editableImage != null)
            {
                SaveStateForUndo();                                    // save current state for undo
                _editableImage.Dispose();                              // dispose current editable image
                _editableImage = new Bitmap(_originalImageBeforeCrop); // restore original image
                pictureBoxImage.Image = _editableImage;                // update picture box
                pictureBoxImage.Invalidate();                          // refresh display
                ResetZoom();                                           // reset zoom
            }
            else
            {
                ScreenshotMessageBox.ShowMessage(
                    $"No crop to reset",
                    $"ScreenGrab:",
                    4000);
            }
        }

        // == draw crop rectangle preview == //
        private void DrawCropPreview(Graphics g)
        {
            // calculate rectangle dimensions
            int x      = Math.Min(_drawStartPoint.X, _drawEndPoint.X);
            int y      = Math.Min(_drawStartPoint.Y, _drawEndPoint.Y);
            int width  = Math.Abs(_drawEndPoint.X - _drawStartPoint.X);
            int height = Math.Abs(_drawEndPoint.Y - _drawStartPoint.Y);

            // draw semi-transparent overlay
            using (Brush dimBrush = new SolidBrush(Color.FromArgb(120, Color.Black)))
            {
                // top
                g.FillRectangle(dimBrush, 0, 0, pictureBoxImage.Width, y);
                // bottom
                g.FillRectangle(dimBrush, 0, y + height, pictureBoxImage.Width, pictureBoxImage.Height - (y + height));
                // left
                g.FillRectangle(dimBrush, 0, y, x, height);
                // right
                g.FillRectangle(dimBrush, x + width, y, pictureBoxImage.Width - (x + width), height);
            }

            // draw crop rectangle border
            using (Pen cropPen = new Pen(Color.White, 2))
            {
                cropPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawRectangle(cropPen, x, y, width, height);
            }
        }

        // == apply crop to image == //
        private void PerformCrop()
        {
            if (_editableImage == null) return; // handle error

            // calculate crop rectangle in image coordinates
            Point imgStart = ConvertToImageCoordinates(_drawStartPoint);
            Point imgEnd   = ConvertToImageCoordinates(_drawEndPoint);

            // calculate rectangle dimensions
            int x      = Math.Min(imgStart.X, imgEnd.X);
            int y      = Math.Min(imgStart.Y, imgEnd.Y);
            int width  = Math.Abs(imgEnd.X - imgStart.X);
            int height = Math.Abs(imgEnd.Y - imgStart.Y);

            // ensure crop rectangle is within image bounds
            if (width <= 0 || height <= 0)
            {
                ScreenshotMessageBox.ShowMessage(
                    $"Invalid crop area selected",
                    $"ScreenGrab:",
                    4000);
                return;
            }

            // clamp to image bounds
            x      = Math.Max(0, x);
            y      = Math.Max(0, y);
            width  = Math.Min(width, _editableImage.Width - x);
            height = Math.Min(height, _editableImage.Height - y);
            if (width <= 0 || height <= 0) // recheck for error after clamping
            {
                ScreenshotMessageBox.ShowMessage(
                    $"Invalid crop area selected",
                    $"ScreenGrab:",
                    4000);
                return;
            }

            // save current state for undo
            if (_originalImageBeforeCrop == null)
            {
                _originalImageBeforeCrop = new Bitmap(_editableImage); // store original image before first crop
            }
            SaveStateForUndo();

            // perform crop
            Rectangle cropRect  = new Rectangle(x, y, width, height);                         // define crop rectangle
            Bitmap croppedImage = _editableImage.Clone(cropRect, _editableImage.PixelFormat); // crop image

            // update editable image
            _editableImage.Dispose();
            _editableImage        = croppedImage;
            pictureBoxImage.Image = _editableImage; // update picture box

            // reset zoom
            ResetZoom();
            pictureBoxImage.Invalidate(); // refresh display

            // reset cropping state
            _activeDrawingTool     = DrawingTool.None;
            pictureBoxImage.Cursor = Cursors.Default;
        }
        // == END OF CROP TOOL FUNCTIONALITY == //

        // == ZOOM FUNCTIONALITY == //
        // mouse wheel event for zooming
        private void pictureBoxImage_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.None && _editableImage != null)
            {
                Point mousePosBeforeZoom = e.Location; // store mouse position relative to image before zoom
                float oldZoomFactor = _zoomFactor;  // calculate zoom
                if (e.Delta > 0)
                {
                    _zoomFactor = Math.Min(_zoomFactor + ZoomStep, ZoomMax); // zoom in
                }
                else
                {
                    _zoomFactor = Math.Max(_zoomFactor - ZoomStep, ZoomMin); // zoom out
                }
                ApplyZoom(mousePosBeforeZoom, oldZoomFactor);
            }
        }

        // apply zoom to picture box
        private void ApplyZoom(Point? mousePos = null, float oldZoom = 1.0f)
        {
            if (_editableImage == null) return;
            Panel? container = pictureBoxImage.Parent as Panel;

            // calculate new size based on zoom factor
            int newWidth = (int)(_editableImage.Width * _zoomFactor);
            int newHeight = (int)(_editableImage.Height * _zoomFactor);

            // update picture box size
            if (_zoomFactor == 1.0f)
            {
                pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxImage.Dock = DockStyle.Fill;
            }
            else
            {
                pictureBoxImage.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxImage.Dock = DockStyle.None;

                if (mousePos.HasValue && container != null && oldZoom > 0)
                {
                    // Get current scroll position (AutoScrollPosition returns negative values)
                    Point currentScroll = container.AutoScrollPosition;
                    int oldScrollX = -currentScroll.X;
                    int oldScrollY = -currentScroll.Y;

                    // Calculate the mouse position in the ACTUAL image space
                    // mousePos is relative to PictureBox, which might be at Zoom mode
                    // We need to convert it to the original image coordinates
                    float imageMouseX = mousePos.Value.X / oldZoom;
                    float imageMouseY = mousePos.Value.Y / oldZoom;

                    // Update size first
                    pictureBoxImage.Size = new Size(newWidth, newHeight);

                    // Calculate new scroll position to keep the same image point under cursor
                    // The cursor should remain at the same position relative to the panel
                    int newScrollX = (int)(imageMouseX * _zoomFactor) - mousePos.Value.X;
                    int newScrollY = (int)(imageMouseY * _zoomFactor) - mousePos.Value.Y;

                    // Adjust for the container position
                    newScrollX += oldScrollX;
                    newScrollY += oldScrollY;

                    // Set scroll position
                    container.AutoScrollPosition = new Point(
                        Math.Max(0, newScrollX),
                        Math.Max(0, newScrollY)
                    );
                }
                else
                {
                    pictureBoxImage.Size = new Size(newWidth, newHeight);
                }
            }
            pictureBoxImage.Invalidate();
        }

        // reset zoom to 100%
        private void ResetZoom()
        {
            _zoomFactor = 1.0f;
            ApplyZoom();
        }

        // == ZOOM BUTTONS == //
        // zoom in button
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (_editableImage == null) return;
            _zoomFactor = Math.Min(_zoomFactor + ZoomStep, ZoomMax);
            ApplyZoom(); // no mouse position provided
        }
        // zoom out button
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (_editableImage == null) return;
            _zoomFactor = Math.Max(_zoomFactor - ZoomStep, ZoomMin);
            ApplyZoom(); // no mouse position provided
        }
        // reset zoom button
        private void btnResetZoom_Click(object sender, EventArgs e)
        {
            ResetZoom();
        }

        // == END OF ZOOM FUNCTIONALITY == //
        // TOOLSTRIP: TOOL SELECTION FUNCTIONALITY == //
        // == method to activate selected drawing tool == //
        private void ActivateDrawingTool(DrawingTool tool)
        {
            System.Diagnostics.Debug.WriteLine($"Activating tool: {tool}"); // debug log
            // toggle off if same tool is selected
            if (_activeDrawingTool == tool)
            {
                _activeDrawingTool = DrawingTool.None;    // deactivate tool
                pictureBoxImage.Cursor = Cursors.Default; // reset cursor
                System.Diagnostics.Debug.WriteLine($"Deactivating tool: {tool}"); // debug log
            }
            else
            {
                _activeDrawingTool = tool;              // activate selected tool
                pictureBoxImage.Cursor = Cursors.Cross; // change cursor to crosshair
                System.Diagnostics.Debug.WriteLine($"Tool activated: {tool}"); // debug log
            }
        }
        // == button to select rectangle drawing tool == //
        private void btnDrawRectangle_Click(object sender, EventArgs e)
        {
            ActivateDrawingTool(DrawingTool.Rectangle);
        }
        // == button to select arrow drawing tool == //
        private void btnDrawArrow_Click(object sender, EventArgs e)
        {
            ActivateDrawingTool(DrawingTool.Arrow);
        }
        // == button to select freeform drawing tool == //
        private void btnDrawFreeform_Click(object sender, EventArgs e)
        {
            ActivateDrawingTool(DrawingTool.Freeform);
        }

        // == button to select text drawing tool == //
        private void btnDrawText_Click(object sender, EventArgs e)
        {
            ActivateDrawingTool(DrawingTool.Text);
        }
        // == button to select highlight drawing tool == //
        private void btnDrawHighlight_Click(object sender, EventArgs e)
        {
            ActivateDrawingTool(DrawingTool.Highlight);
        }
        // == button to select blur drawing tool == //
        private void btnDrawBlur_Click(object sender, EventArgs e)
        {
            ActivateDrawingTool(DrawingTool.Blur);
        }
        // == button to select line drawing tool == //
        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            ActivateDrawingTool(DrawingTool.Line);
        }

        // == END OF TOOL SELECTION FUNCTIONALITY == //

        // == TEXT TOOL FUNCTIONALITY == //
        // Custom textbox for text input
        public class TransparentTextBox : RichTextBox
        {
            public TransparentTextBox()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                BackColor = Color.Transparent; // semi-transparent background
            }
            // ovveride paint background
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                // Do not paint background to keep it transparent
                if (Parent != null)
                {
                    e.Graphics.TranslateTransform(-Left, -Top);
                    Parent.GetType().GetMethod("OnPaintBackground", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?
                        .Invoke(Parent, new object[] { e });
                    e.Graphics.TranslateTransform(Left, Top);
                }
            }
        }
        private void ShowTextInputBox(Point location)
        {
            // commit previous text if any
            CommitTextToImage();
            _textStartPoint = location; // set text insertion point
            _isTextInputActive = true;

            var textBox = new TransparentTextBox()
            {
                Location = new Point(-1000, -1000), // temporary box off-screen // transparant textbox will be drawn at correct location in Paint event
                Multiline = true,
                Font = _textFont,
                ForeColor = _SelectedColor,
                BorderStyle = BorderStyle.None,
                MinimumSize = new Size(100, 30),
                Width = 200,
                ScrollBars = RichTextBoxScrollBars.None,
            };
            _textBox = textBox;

            // wire up events
            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.LostFocus += TextBox_LostFocus;
            _textBox.TextChanged += TextBox_TextChanged;

            // add textbox to form
            pictureBoxImage.Controls.Add(_textBox);
            _textBox.BringToFront();
            _textBox.Focus(); // focus textbox for immediate typing
        }

        // commit text to image on Enter key press
        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && _textBox != null)
            {
                e.Handled = true; // prevent ding sound
                CommitTextToImage();
            }
            else if (e.KeyCode == Keys.Escape) // cancel text input on Escape key
            {
                CancelTextInput();
            }
        }
        private void TextBox_LostFocus(object? sender, EventArgs e)
        {
            CommitTextToImage();
        }
        // redraw textbox on text change
        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            pictureBoxImage.Invalidate(); // request redraw to update textbox display
        }
        // commit text from textbox to image
        private void CommitTextToImage()
        {
            if (_textBox != null && _editableImage != null)
            {
                string text = _textBox.Text.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    using (Graphics g = Graphics.FromImage(_editableImage))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // improve quality
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;          // improve text quality
                        Point imagePoint = ConvertToImageCoordinates(_textStartPoint);      // convert to image coordinates
                        using (Brush textBrush = new SolidBrush(_SelectedColor))
                        {
                            g.DrawString(text, _textFont, textBrush, imagePoint);           // draw text on image
                        }
                    }
                    // update picture box with edited image
                    pictureBoxImage.Image = _editableImage;
                }
                // remove textbox from form
                RemoveTextInputBox();
            }
        }
        // cancel text input and remove textbox
        private void CancelTextInput()
        {
            RemoveTextInputBox();
        }
        // remove textbox from form
        private void RemoveTextInputBox()
        {
            if (_textBox != null)
            {
                // unhook events
                _textBox.KeyDown     -= TextBox_KeyDown;
                _textBox.LostFocus   -= TextBox_LostFocus;
                _textBox.TextChanged -= TextBox_TextChanged;
                this.Controls.Remove(_textBox);  // remove textbox from form
                _textBox.Dispose();              // dispose textbox
                _textBox = null;                 // clear reference
                _isTextInputActive = false;      // reset text input flag
                pictureBoxImage.Invalidate();    // request redraw to remove textbox display
            }
        }
        // == END OF TEXT TOOL FUNCTIONALITY == //

        // == HIGHLIGHTING TOOL FUNCTIONALITY == //
        private void DrawHighlightPreview(Graphics g)
        {
            int x = Math.Min(_drawStartPoint.X, _drawEndPoint.X);
            int y = Math.Min(_drawStartPoint.Y, _drawEndPoint.Y);
            int width = Math.Abs(_drawEndPoint.X - _drawStartPoint.X);
            int height = Math.Abs(_drawEndPoint.Y - _drawStartPoint.Y);
            Color color = Color.FromArgb(80, _SelectedColor); // semi-transparent color
            using (Brush highlightBrush = new SolidBrush(color))
            {
                g.FillRectangle(highlightBrush, x, y, width, height);
            }
        }
        private void DrawHighlightOnImage(Graphics g, Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(end.X - start.X);
            int height = Math.Abs(end.Y - start.Y);
            Color color = Color.FromArgb(80, _SelectedColor); // semi-transparent color
            using (Brush highlightBrush = new SolidBrush(color))
            {
                g.FillRectangle(highlightBrush, x, y, width, height);
            }
        }
        // == END OF HIGHLIGHTING TOOL FUNCTIONALITY == //

        // == BLUR TOOL FUNCTIONALITY == //
        // draw blur preview
        private void DrawBlurPreview(Graphics g)
        {
            int x = Math.Min(_drawStartPoint.X, _drawEndPoint.X);
            int y = Math.Min(_drawStartPoint.Y, _drawEndPoint.Y);
            int width = Math.Abs(_drawEndPoint.X - _drawStartPoint.X);
            int height = Math.Abs(_drawEndPoint.Y - _drawStartPoint.Y);
            Color color = Color.FromArgb(120, Color.Gray); // semi-transparent gray
            using (Brush blurBrush = new SolidBrush(color))
            {
                g.FillRectangle(blurBrush, x, y, width, height);
            }
        }
        // draw blur on image
        private void DrawBlurOnImage(Graphics g, Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(end.X - start.X);
            int height = Math.Abs(end.Y - start.Y);
            if (width <= 0 || height <= 0) return; // avoid invalid dimensions

            // clean up image bounds before use
            x = Math.Max(x, 0);
            y = Math.Max(y, 0);
            width = Math.Min(width, _editableImage!.Width - x);
            height = Math.Min(height, _editableImage!.Height - y);

            // create blur rectangle
            Rectangle blurRect = new Rectangle(x, y, width, height);

            // extract the area to be blurred
            using (Bitmap blurArea = _editableImage!.Clone(blurRect, _editableImage.PixelFormat))
            {
                int pixelSize = 5; // size of the blur pixelation // adjust for stronger/weaker blur
                int smallWidth = Math.Max(1, blurArea.Width / pixelSize);
                int smallHeight = Math.Max(1, blurArea.Height / pixelSize);

                using (Bitmap smallBitMap = new Bitmap(smallWidth, smallHeight))
                {
                    using (Graphics gSmall = Graphics.FromImage(smallBitMap))
                    {
                        gSmall.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        gSmall.DrawImage(blurArea, 0, 0, smallWidth, smallHeight);
                    }
                    // scale back to original size
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.DrawImage(smallBitMap, blurRect);
                }

            }
        }
        // == END OF BLUR TOOL FUNCTIONALITY == //


        // == EVENTS DRAWING FUNCTIONALITY == //
        // start drawing shape on image
        private void pictureBoxImage_MouseDown(object? sender, MouseEventArgs e)
        {
            
            if (_activeDrawingTool != DrawingTool.None && e.Button == MouseButtons.Left && _editableImage != null)
            {
                System.Diagnostics.Debug.WriteLine($"MouseDown - Tool: {_activeDrawingTool}, Button: {e.Button}, Location: {e.Location}"); // debug log
                // handle crop tool
                if (_activeDrawingTool == DrawingTool.Crop)     // for crop tool
                {

                    System.Diagnostics.Debug.WriteLine($"Starting drawing operation for tool: {_activeDrawingTool}"); // debug log
                    _isDrawing      = true;                     // set drawing flag
                    _isCropping     = true;                     // set cropping flag
                    _drawStartPoint = e.Location;               // set starting point
                    _drawEndPoint   = e.Location;               // initialize ending point
                    return;
                }

                if (_currentPen == null) return;                // safety check
                _isDrawing = true;                              // set drawing flag
                _drawStartPoint = e.Location;                   // set starting point
                _drawEndPoint = e.Location;                     // initialize ending point

                if (_activeDrawingTool == DrawingTool.Freeform) //for freeform drawing
                {
                    _freeformPoints.Clear();                    // clear previous points
                    _freeformPoints.Add(e.Location);            // add starting point
                }
                if (_activeDrawingTool == DrawingTool.Text)     // for text tool
                {
                    ShowTextInputBox(e.Location);               // show text input box
                    return;
                }
            }
        }
        // continue drawing shape on image
        private void pictureBoxImage_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDrawing && _activeDrawingTool != DrawingTool.None)
            {
                _drawEndPoint = e.Location;          // update ending point
                if (_activeDrawingTool == DrawingTool.Freeform)
                {
                    _freeformPoints.Add(e.Location); // add point for freeform drawing
                }
                pictureBoxImage.Invalidate();       // request redraw to show preview
            }
        }
        // finish drawing shape on image
        private void pictureBoxImage_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_isDrawing && _activeDrawingTool != DrawingTool.None && _editableImage != null)
            {
                _drawEndPoint = e.Location;          // set ending point

                // handle cropping
                if (_activeDrawingTool == DrawingTool.Crop && _isCropping)
                {
                    _isDrawing = false;               // reset drawing flag
                    _isCropping = false;              // reset cropping flag
                    PerformCrop();                    // perform crop operation
                    pictureBoxImage.Invalidate();     // refresh picture box to show cropped image
                    return;
                }

                // handle freeform drawing
                if (_activeDrawingTool == DrawingTool.Freeform)
                {
                    _freeformPoints.Add(e.Location); // add final point for freeform drawing
                }
                _isDrawing = false;                  // reset drawing flag

                // draw final shape on image
                DrawShapeOnImage();                  // draw shape on image
                pictureBoxImage.Invalidate();        // refresh picture box to show final drawing
            }
        }
        // paint event to show drawing preview
        private void pictureBoxImage_Paint(object? sender, PaintEventArgs e)
        {
            if (_isDrawing && _activeDrawingTool == DrawingTool.Crop)
            {
                DrawCropPreview(e.Graphics); // draw crop preview
            }
            else if (_isDrawing && _activeDrawingTool != DrawingTool.None && _currentPen != null)
            {
                // draw preview shape
                switch (_activeDrawingTool)
                {
                    case DrawingTool.Rectangle:
                        DrawRectanglePreview(e.Graphics);
                        break;
                    case DrawingTool.Arrow:
                        DrawArrowPreview(e.Graphics);
                        break;
                    case DrawingTool.Freeform:
                        DrawFreeformPreview(e.Graphics);
                        break;
                    case DrawingTool.Highlight:
                        DrawHighlightPreview(e.Graphics);
                        break;
                    case DrawingTool.Blur:
                        DrawBlurPreview(e.Graphics);
                        break;
                    case DrawingTool.Line:
                        DrawLinePreview(e.Graphics);
                        break;
                    case DrawingTool.Crop:
                        DrawCropPreview(e.Graphics);
                        break;
                }
            }
            
            // draw textbox if active
            if (_isTextInputActive && _textBox != null)
            {
                // textbox is drawn automatically as a control
                DrawTextPreview(e.Graphics);
            }
        }

        // draw text preview in textbox
        private void DrawTextPreview(Graphics g)
        {
            if (_textBox != null)
            {
                string text = _textBox.Text;
                if (string.IsNullOrEmpty(text))
                {
                    text = " | "; // placeholder text
                }
                else
                {
                    text += " | "; // add cursor indicator
                }
                // improve quality
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                // use brush for text color
                using (Brush textBrush = new SolidBrush(_SelectedColor))
                {
                    g.DrawString(text, _textFont, textBrush, _textStartPoint);
                }
            }
        }
        // == END OF DRAWING EVENTS == //

        // == DRAWING METHODS == //
        // == draw rectangle preview == //
        private void DrawRectanglePreview(Graphics g)
        {
            int x = Math.Min(_drawStartPoint.X, _drawEndPoint.X);
            int y = Math.Min(_drawStartPoint.Y, _drawEndPoint.Y);
            int width = Math.Abs(_drawEndPoint.X - _drawStartPoint.X);
            int height = Math.Abs(_drawEndPoint.Y - _drawStartPoint.Y);
            g.DrawRectangle(_currentPen!, x, y, width, height);
        }
        // == draw arrow preview == //
        private void DrawArrowPreview(Graphics g)
        {
            if (_currentPen != null)
            {
                using (Pen arrowPen = new Pen(_SelectedColor, _brushSize))
                {
                    arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(_brushSize, _brushSize);
                    g.DrawLine(arrowPen, _drawStartPoint, _drawEndPoint);
                }
            }
        }
        // == draw freeform preview == //
        private void DrawFreeformPreview(Graphics g)
        {
            if (_freeformPoints.Count > 1 || _currentPen != null)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;  // improve quality
                g.DrawLines(_currentPen!, _freeformPoints.ToArray());                // draw freeform lines
            }
        }

        // == draw line preview == //
        private void DrawLinePreview(Graphics g)
        {
            if (_currentPen != null)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // improve quality
                g.DrawLine(_currentPen, _drawStartPoint, _drawEndPoint);            // draw line
            }
        }
        // == draw shape on image == //
        private void DrawShapeOnImage()
        {
            if (_editableImage == null || _currentPen == null) return;
            SaveStateForUndo(); // save current state for undo functionality

            using (Graphics g = Graphics.FromImage(_editableImage))
            {
                g.SmoothingMode  = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // improve quality
                Point imageStart = ConvertToImageCoordinates(_drawStartPoint);       // convert start point
                Point imageEnd   = ConvertToImageCoordinates(_drawEndPoint);         // convert end point

                // draw shape based on selected tool
                switch (_activeDrawingTool)
                {
                    case DrawingTool.Rectangle:
                        DrawRectangleOnImage(g, imageStart, imageEnd);
                        break;
                    case DrawingTool.Arrow:
                        DrawArrowOnImage(g, imageStart, imageEnd);
                        break;
                    case DrawingTool.Freeform:
                        List<Point> imagePoints = new List<Point>();
                        foreach (Point p in _freeformPoints)
                        {
                            imagePoints.Add(ConvertToImageCoordinates(p)); // convert each point
                        }
                        DrawFreeformOnImage(g, imagePoints);
                        break;
                    case DrawingTool.Highlight:
                        DrawHighlightOnImage(g, imageStart, imageEnd);
                        break;
                    case DrawingTool.Blur:
                        DrawBlurOnImage(g, imageStart, imageEnd);
                        break;
                    case DrawingTool.Line:
                        DrawLineOnImage(g, imageStart, imageEnd);
                        break;
                }
            }
            // update picture box with edited image
            pictureBoxImage.Image = _editableImage;
        }

        // == draw rectangle on image // helper for DrawShapeOnImage() == //
        private void DrawRectangleOnImage(Graphics g, Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(end.X - start.X);
            int height = Math.Abs(end.Y - start.Y);
            g.DrawRectangle(_currentPen!, x, y, width, height);
        }

        // == draw arrow on image // helper for DrawShapeOnImage() == //
        private void DrawArrowOnImage(Graphics g, Point start, Point end)
        {
            if (_currentPen != null)
            {
                using (Pen arrowPen = new Pen(_SelectedColor, _brushSize))
                {
                    arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(_brushSize, _brushSize);
                    g.DrawLine(arrowPen, start, end);
                }
            }
        }
        // == draw freeform on image // helper for DrawShapeOnImage() == //
        private void DrawFreeformOnImage(Graphics g, List<Point> points)
        {
            if (points.Count > 1 && _currentPen != null)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // improve quality
                g.DrawLines(_currentPen, points.ToArray());                         // draw freeform lines
            }
        }

        // == convert picture box coordinates to image coordinates // helper for DrawShapeOnImage() == //
        private Point ConvertToImageCoordinates(Point pictureBoxPoint)
        {
            if (pictureBoxImage.Image == null) return pictureBoxPoint;
            if (pictureBoxImage.SizeMode == PictureBoxSizeMode.Zoom && _editableImage != null)
            {
                // calculate scaling factors
                float imageAspect = (float)_editableImage.Width / _editableImage.Height;   // aspect ratio of the image
                float boxAspect = (float)pictureBoxImage.Width / pictureBoxImage.Height;   // aspect ratio of the picture box
                float scaleFactor;                                                         // scaling factor
                int offsetX = 0; int offsetY = 0;                                          // offsets for centering image
                // determine scaling and offsets
                if (imageAspect > boxAspect)
                {
                    // image is wider than box
                    scaleFactor = (float)_editableImage.Width / pictureBoxImage.Width;
                    offsetY = (pictureBoxImage.Height - (int)(_editableImage.Height / scaleFactor)) / 2;
                }
                else
                {
                    // image is taller than box
                    scaleFactor = (float)_editableImage.Height / pictureBoxImage.Height;
                    offsetX = (pictureBoxImage.Width - (int)(_editableImage.Width / scaleFactor)) / 2;
                }
                // convert coordinates
                int imageX = (int)((pictureBoxPoint.X - offsetX) * scaleFactor);
                int imageY = (int)((pictureBoxPoint.Y - offsetY) * scaleFactor);
                return new Point(imageX, imageY);
            }
            return pictureBoxPoint;
        }

        // == Draw line on image method
        private void DrawLineOnImage(Graphics g, Point start, Point end)
        {
            if (_currentPen != null)
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // improve quality
                g.DrawLine(_currentPen, start, end);                                // draw line
            }
        }
        // == END DRAWING METHODS == //

        // == button to open form named "Driver" == //
        private void btnGoHome_Click(object sender, EventArgs e)
        {
            Form? parentForm = this.Owner ?? this.Tag as Form;
            if (parentForm != null)
            {
                parentForm.Show();                               // show main app form
                parentForm.WindowState = FormWindowState.Normal; // set window state to normal
                parentForm.ShowInTaskbar = true;                 // show in taskbar
                parentForm.Activate();
            }
            this.Close();                                        // close settings form to return to main app
        }

        // == Send to settings form button == //
        private void btnSendToSettingsFromEditor_Click(object sender, EventArgs e)
        {
            Form? parentForm = this.Tag as Form;
            if (parentForm != null && parentForm is Driver driverForm)
            {
                var settingsForm = new SettingsForm(driverForm.GetHotkeyConfig());
                settingsForm.Tag = parentForm;
                settingsForm.HotkeysChanged += config =>
                {
                    driverForm.UpdateHotkeyConfig(config);
                };
                settingsForm.ShowInTaskbar = true;                 // show in taskbar
                settingsForm.Show();                               // show settings form
                this.Close();                                        // close editor form
            }
            else
            {
                MessageBox.Show("Unable to open Settings: Parent form not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // == clean resources on form closing ==//
        private void ImageEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // unsubscribe event handlers
            pictureBoxImage.MouseDown   -= pictureBoxImage_MouseDown;
            pictureBoxImage.MouseMove   -= pictureBoxImage_MouseMove;
            pictureBoxImage.MouseUp     -= pictureBoxImage_MouseUp; 
            pictureBoxImage.Paint       -= pictureBoxImage_Paint;
            pictureBoxImage.MouseWheel  -= pictureBoxImage_MouseWheel;
            toolStripBtnCrop.Click      -= btnCrop_Click;
            toolStripBtnResetCrop.Click -= btnResetCrop_Click;
            this.KeyDown                -= ImageEditorForm_KeyDown;

            // dispose of undo stack
            while (_undoStack.Count > 0)
            {
                _undoStack.Pop().Dispose();
            }

            // dispose of resources
            btnSelectColor.Image?.Dispose();     // dispose of selected color image
            _currentImage?.Dispose();            // dispose loaded image
            _currentPen?.Dispose();              // dispose drawing pen
            _editableImage?.Dispose();           // dispose editable image
            _originalImageBeforeCrop?.Dispose(); // dispose original image before crop
            _currentImage = null;                // clear reference
            _editableImage = null;               // clear reference
            _originalImageBeforeCrop = null;     // clear reference
            _freeformPoints.Clear();             // clear freeform points
            base.OnFormClosing(e);               // call base class method
        }
    }
}