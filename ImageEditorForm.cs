///// Manages the image editor and functionalities //////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenGrab
{
    public partial class ImageEditorForm : Form
    {
        // enum for drawing tools
        private enum DrawingTool
        {
            None,
            Rectangle,
            Arrow
        }
        private Image?      _currentImage;                         // local variable for storing loaded iamge
        private Bitmap?     _editableImage;                        // local variable for storing editable image
        private DrawingTool _activeDrawingTool = DrawingTool.None; // currently selected drawing tool // initially none
        private Point      _drawStartPoint;                        //starting point for drawing
        private Point      _drawEndPoint;                          //ending point for drawing
        private bool       _isDrawing = false;                     //flag to indicate if drawing is in progress
        private Color      _SelectedColor = Color.Red;             //default color (red) for drawing
        private Pen?       _currentPen;                            //pen for drawing shapes
         
        // == constructor == //
        public ImageEditorForm()
        {
            InitializeComponent();
            _currentPen = new Pen(_SelectedColor, 2); // initialize pen with default color and width
            UpdateColorButtonDisplay();               // update button display with default color

            // wire up mouse events for drawing
            pictureBoxImage.MouseDown += pictureBoxImage_MouseDown;
            pictureBoxImage.MouseMove += pictureBoxImage_MouseMove;
            pictureBoxImage.MouseUp   += pictureBoxImage_MouseUp;
            pictureBoxImage.Paint     += pictureBoxImage_Paint;
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
                _currentImage = Image.FromFile(imagePath);                             // load image from file
                pictureBoxImage.Image = _editableImage;                                 // assign image to picture box
                this.Text = $"Image Editor - {System.IO.Path.GetFileName(imagePath)}"; // set form title
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // == menustrip: click button == open file explorer to load image == //
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

        // == TOOLSTRIP: FUNCTIONALITY FOR SELECTING COLOR FOR DRAWING == //
        // button to select color for drawing
        private void btnSelectColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color         = _SelectedColor; // set current selected color
                colorDialog.FullOpen      = true;           // allow full color selection
                colorDialog.AllowFullOpen = true;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _SelectedColor = colorDialog.Color;       // update selected color
                    _currentPen?.Dispose();                   // dispose previous pen
                    _currentPen = new Pen(_SelectedColor, 2); // create new pen with selected color
                    UpdateColorButtonDisplay();               // update button display
                }
            }
        }

        // == update button display w/ currently selected color == //
        private void UpdateColorButtonDisplay()
        {
            int _buttonSize = 16;                              // size of color display square
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
            btnSelectColor.Image = colorBitmap;
        }
        // == END OF COLOR SELECTION FUNCTIONALITY == //

        // TOOLSTRIP: TOOL SELECTION FUNCTIONALITY == //
        // == method to activate selected drawing tool == //
        private void ActivateDrawingTool(DrawingTool tool)
        {
            // toggle off if same tool is selected
            if (_activeDrawingTool == tool)
            {
                _activeDrawingTool = DrawingTool.None;    // deactivate tool
                pictureBoxImage.Cursor = Cursors.Default; // reset cursor
            }
            else
            {
                _activeDrawingTool = tool;              // activate selected tool
                pictureBoxImage.Cursor = Cursors.Cross; // change cursor to crosshair
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

        // == END OF TOOL SELECTION FUNCTIONALITY == //

        // == EVENTS DRAWING FUNCTIONALITY == //
        // start drawing shape on image
        private void pictureBoxImage_MouseDown(object? sender, MouseEventArgs e)
        {
            if (_activeDrawingTool != DrawingTool.None && e.Button == MouseButtons.Left && _currentPen != null && _editableImage != null)
            {
                _isDrawing = true;                // set drawing flag
                _drawStartPoint = e.Location;     // set starting point
                _drawEndPoint = e.Location;       // initialize ending point
            }
        }
        // continue drawing shape on image
        private void pictureBoxImage_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDrawing && _activeDrawingTool != DrawingTool.None)
            {
                _drawEndPoint = e.Location;       // update ending point
                pictureBoxImage.Invalidate();     // request redraw to show preview
            }
        }
        // finish drawing shape on image
        private void pictureBoxImage_MouseUp(object? sender, MouseEventArgs e) 
        {
            if (_isDrawing && _activeDrawingTool != DrawingTool.None && _editableImage != null)
            {
                _drawEndPoint = e.Location;       // set ending point
                _isDrawing = false;               // reset drawing flag

                // draw final shape on image
                DrawShapeOnImage();               // draw shape on image
                pictureBoxImage.Invalidate();     // refresh picture box to show final drawing
            }
        }
        // paint event to show drawing preview
        private void pictureBoxImage_Paint(object? sender, PaintEventArgs e)
        {
            if (_isDrawing && _activeDrawingTool != DrawingTool.None && _currentPen != null)
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
                }
            }
        }
        // == END OF DRAWING EVENTS == //

        // == DRAWING METHODS == //
        // == draw rectangle preview == //
        private void DrawRectanglePreview(Graphics g)
        {
            int x      = Math.Min(_drawStartPoint.X, _drawEndPoint.X);
            int y      = Math.Min(_drawStartPoint.Y, _drawEndPoint.Y);
            int width  = Math.Abs(_drawEndPoint.X - _drawStartPoint.X);
            int height = Math.Abs(_drawEndPoint.Y - _drawStartPoint.Y);
            g.DrawRectangle(_currentPen!, x, y, width, height);
        }
        // == draw arrow preview == //
        private void DrawArrowPreview(Graphics g)
        {
            if (_currentPen != null) 
            {
                using (Pen arrowPen = new Pen(_SelectedColor, 2))
                {
                    arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5,5);
                    g.DrawLine(arrowPen, _drawStartPoint, _drawEndPoint);
                }
            }
        }

        // == draw shape on image == //
        private void DrawShapeOnImage()
        {
            if (_editableImage == null || _currentPen == null) return;
            if (_currentImage != null || _currentPen != null)
            {
                using (Graphics g = Graphics.FromImage(_editableImage))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // improve quality
                    Point imageStart = ConvertToImageCoordinates(_drawStartPoint); // convert start point
                    Point imageEnd   = ConvertToImageCoordinates(_drawEndPoint);   // convert end point

                    // draw shape based on selected tool
                    switch (_activeDrawingTool)
                    {
                        case DrawingTool.Rectangle:
                            DrawRectangleOnImage(g, imageStart, imageEnd);
                            break;
                        case DrawingTool.Arrow:
                            DrawArrowOnImage(g, imageStart, imageEnd);
                            break;
                    }
                }
                // update picture box with edited image
                pictureBoxImage.Image = _editableImage;
            }
        }

        // == draw rectangle on image // helper for DrawShapeOnImage() == //
        private void DrawRectangleOnImage(Graphics g, Point start, Point end)
        {
            int x      = Math.Min(start.X, end.X);
            int y      = Math.Min(start.Y, end.Y);
            int width  = Math.Abs(end.X - start.X);
            int height = Math.Abs(end.Y - start.Y);
            g.DrawRectangle(_currentPen!, x, y, width, height);
        }

        // == draw arrow on image // helper for DrawShapeOnImage() == //
        private void DrawArrowOnImage(Graphics g, Point start, Point end)
        {
            if (_currentPen != null) 
            {
                using (Pen arrowPen = new Pen(_SelectedColor, 2))
                {
                    arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5,5);
                    g.DrawLine(arrowPen, start, end);
                }
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
                float boxAspect   = (float)pictureBoxImage.Width / pictureBoxImage.Height; // aspect ratio of the picture box
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

        // == clean resources on form closing ==//
        private void ImageEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnSelectColor.Image?.Dispose(); // dispose of selected color image
            _currentImage?.Dispose();        // dispose loaded image
            _currentPen?.Dispose();          // dispose drawing pen
            _editableImage?.Dispose();       // dispose editable image
            _currentImage = null;            // clear reference
            _editableImage = null;           // clear reference
            base.OnFormClosing(e);           // call base class method

        }
    }
}
