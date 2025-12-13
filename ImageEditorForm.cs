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
        private Image? _currentImage; //local variable for storing loaded iamge
        // constructor
        public ImageEditorForm()
        {
            InitializeComponent();
        }

        //constructor to load image from path
        public ImageEditorForm(string imagePath) : this()
        {
            LoadImage(imagePath);
        }
        // method to load image from file path
        private void LoadImage(string imagePath)
        {
            try
            {
                _currentImage?.Dispose();                  // dispose previous image if any
                _currentImage = Image.FromFile(imagePath); // load image from file
                pictureBoxImage.Image = _currentImage;         // assign image to picture box
                this.Text = $"Image Editor - {System.IO.Path.GetFileName(imagePath)}"; // set form title
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // method for click button = open file explorer to load image
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

        // button to open form named "Driver"
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
        // clean resources on form closing
        private void ImageEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _currentImage?.Dispose(); // dispose loaded image
            _currentImage = null;
            base.OnFormClosing(e);
        }
    }
}
