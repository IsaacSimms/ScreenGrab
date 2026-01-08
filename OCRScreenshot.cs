///// OCR ScreenshotForm - captures region and extract text using Windows OCR /////

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ScreenGrab
{
    public partial class OCRScreenshotForm : Form
    {
        // == properties == //
        public Bitmap? Screenshot { get; set; }
        public DateTime CaptureTime { get; set; }
        public string ExtractedText { get; set; } = string.Empty;

        // == OCR service instance == //
        private readonly PaddleOcrService _ocrService;

        // == default constructor == //
        public OCRScreenshotForm()
        {
            InitializeComponent();
            _ocrService = new PaddleOcrService();
        }

        // == constructor using screenshot bitmap == //
        public OCRScreenshotForm(Bitmap screenshot) : this()
        {
            Screenshot = screenshot;                 // assign screenshot
            CaptureTime = DateTime.Now;              // assign capture time
            pictureBoxScreenshot.Image = Screenshot; // display screenshot in picture box

            // perform OCR and display extracted text
            PerformOcrAsync();
        }

        // == perform OCR on the screenshot == //
        private async void PerformOcrAsync()
        {
            if (Screenshot == null)
            {
                txtOcrResult.Text = "No screenshot available.";
                return;
            }

            try
            {
                txtOcrResult.Text = "Performing OCR, please wait...";
                var ocrResult = await _ocrService.RecognizeAsync(Screenshot);

                // use PaddleOCR to recognize text
                if  (!ocrResult.Success) // check for success
                {
                    txtOcrResult.Text = ocrResult.ErrorMessage ?? "OCR failed with unknown error";
                    if (ocrResult.Exception != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"OCR Error: {ocrResult.Exception}");
                    }
                    return;
                }

                if (string.IsNullOrEmpty(ocrResult.TextBox))
                {
                    txtOcrResult.Text = "No text detected in the screenshot.";
                    return;
                }

                // display extracted text
                ExtractedText     = ocrResult.TextBox;
                txtOcrResult.Text = ExtractedText;
                Debug.WriteLine($"OCR Success: Detected {ocrResult.RegionCount} text regions.");
            }
            catch (Exception ex)
            {
                txtOcrResult.Text = $"OCR failed: {ex.Message}\n\nDetails: {ex.GetType().Name}";
                System.Diagnostics.Debug.WriteLine($"OCR Error: {ex}");
            }
        }

        // == copy screenshot to clipboard == //
        private void btnCopyScreenshot_Click(object sender, EventArgs e)
        {
            if (Screenshot != null)
            {
                Clipboard.SetImage(Screenshot);
            }
            ScreenshotMessageBox.ShowMessage(
                $"ScreenGrab: Screenshot copied to clipboard.",
                $"ScreenGrab:",
                4000);
        }
        // == send to driver form button click event == //
        private void btnGoHome_Click(object sender, EventArgs e)
        {
            Form? parentForm = this.Owner ?? this.Tag as Form;
            if (parentForm != null)
            {
                parentForm.Show();
                parentForm.WindowState = FormWindowState.Normal;
                parentForm.ShowInTaskbar = true;
                parentForm.Activate();
            }
            this.Close();
        }

        // == save screenshot to file == //
        private void btnSaveScreenshot_Click(object sender, EventArgs e)
        {
            if (Screenshot == null) return;
            using SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Screenshot",
                Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|GIF Image|*.gif",
                DefaultExt = "png",
                FileName = $"Screenshot_{CaptureTime:yyyyMMdd_HHmmss}"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // determine image format based on file extension
                    var extension = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();
                    System.Drawing.Imaging.ImageFormat format = extension switch
                    {
                        ".jpg" or ".jpeg" => System.Drawing.Imaging.ImageFormat.Jpeg,
                        ".bmp" => System.Drawing.Imaging.ImageFormat.Bmp,
                        ".gif" => System.Drawing.Imaging.ImageFormat.Gif,
                        _ => System.Drawing.Imaging.ImageFormat.Png,
                    };
                    // save the screenshot
                    Screenshot.Save(saveFileDialog.FileName, format);
                    MessageBox.Show("Screenshot saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save screenshot. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // == export as markdown to folder == //
        private void btnExportMarkdown_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new FolderBrowserDialog
            {
                Description = "Select Folder to Save Markdown File",
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true
            };
            if (folderDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                // == builds markdown content == //
                // create timestamped subfolder
                string folderName = $"OcrCapture_{CaptureTime:yyyyMMdd_HHmmss}";
                string exportPath = Path.Combine(folderDialog.SelectedPath, folderName);
                Directory.CreateDirectory(exportPath);

                // save screenshot
                string screenshotFIleName = "screenshot.png";
                string screenshotFilePath = Path.Combine(exportPath, screenshotFIleName);
                Screenshot?.Save(screenshotFilePath, System.Drawing.Imaging.ImageFormat.Png);

                // generate markdown
                string markdownContent = GenerateMarkdownContent(screenshotFIleName);
                string markdownFilePath = Path.Combine(exportPath, "OcrInfo.md");
                File.WriteAllText(markdownFilePath, markdownContent);

                MessageBox.Show($"Markdown file and screenshot saved successfully to:\r\n{exportPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export markdown file. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // == generate markdown content == //
        private string GenerateMarkdownContent(string screenshotFileName)
        {
            return $"""
            # OCR Capture Information

            **Capture Time:** {CaptureTime:yyyy-MM-dd HH:mm:ss.fff}  
           

            ## Screenshot

            ![Screenshot]({screenshotFileName})

            ---

            ## OCR Extracted Text
            ```text
            {ExtractedText}
            ```

            ---
            ## Statistics
            
            | Property              | Value |
            |-----------------------|-------|
            | Capture Time          | {CaptureTime:yyyy-MM-dd HH:mm:ss.fff} |
            | Screenshot Dimensions | {Screenshot?.Width ?? 0} x {Screenshot?.Height ?? 0} pixels |
            | Characters Detected   | {ExtractedText.Length} |
            | Lines Detected        | {(string.IsNullOrEmpty(ExtractedText) ? 0 : ExtractedText.Split('\n').Length)} |
            """;
        }

        // == open screenshot in editr form == //
        private void btnOpenEditor_Click(object sender, EventArgs e)
        {
            if (Screenshot == null) return;

            // create copy of screenshot (avoids disposal issues)
            Bitmap screenshotCopy = new Bitmap(Screenshot);

            // open editor form with screenshot
            ImageEditorForm editorForm = new ImageEditorForm(screenshotCopy);
            editorForm.Show();
            this.Close();
        }

        //== copy text from textbox to clipboard ==//
        private void btnCopyText_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOcrResult.Text))
            {
                Clipboard.SetText(txtOcrResult.Text);
                ScreenshotMessageBox.ShowMessage(
                    $"ScreenGrab: OCR text copied to clipboard.",
                    $"ScreenGrab:",
                    4000);
            }
        }

        // == dispose OCR service on form close == //
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            //dispose screenshot bitmap
            if (pictureBoxScreenshot.Image != null)
            {
                pictureBoxScreenshot.Image.Dispose();
                pictureBoxScreenshot.Image = null;
            }

            // dispose of screenshot property
            Screenshot?.Dispose();
            Screenshot = null;

            //  dispose OCR service
            _ocrService.Dispose();
            base.OnFormClosed(e);
        }
    }
}