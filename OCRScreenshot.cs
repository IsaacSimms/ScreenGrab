///// OCR ScreenshotForm - captures region and extract text using Windows OCR /////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text; 
using System.Windows.Forms;
using Windows.Media.Ocr;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;


namespace ScreenGrab
{
    public partial class OCRScreenshotForm : Form
    {
        // == properties == //
        public Bitmap Screenshot { get; set; }
        public DateTime CaptureTime { get; set; }
        public string ExtractedText { get; set; } = string.Empty;

        // == default constructor == //
        public OCRScreenshotForm()
        {
            InitializeComponent();
        }

        // == contructor using screenshot bitmap == //
        public OCRScreenshotForm(Bitmap screenshot) : this()
        {
            Screenshot = screenshot;                 // assign screenshot
            CaptureTime = DateTime.Now;              // assign capture time
            pictureBoxScreenshot.Image = Screenshot; // display screenshot in picture box


        }
    }
}
