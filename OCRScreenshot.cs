///// OCR ScreenshotForm - captures region and extract text using Windows OCR /////

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace ScreenGrab
{
    public partial class OCRScreenshotForm : Form
    {
        // == properties == //
        public Bitmap? Screenshot { get; set; }
        public DateTime CaptureTime { get; set; }
        public string ExtractedText { get; set; } = string.Empty;

        // == default constructor == //
        public OCRScreenshotForm()
        {
            InitializeComponent();
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
                txtOcrResult.Text = "Performing OCR...";

                // preprocess image for better OCR results
                using Bitmap processedBitmap = PreprocessImageForOcr(Screenshot);

                // convert Bitmap to SoftwareBitmap for Windows OCR
                using var stream = new InMemoryRandomAccessStream();

                // save as BMP format for better compatibility (uncompressed)
                processedBitmap.Save(stream.AsStream(), ImageFormat.Bmp);
                stream.Seek(0);

                // decode the image
                var decoder = await BitmapDecoder.CreateAsync(stream);

                // get software bitmap and convert to required format for OCR
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied);

                // create OCR engine - try user languages first, then fall back to English
                var ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
                if (ocrEngine == null)
                {
                    // try English as fallback
                    var englishLanguage = new Windows.Globalization.Language("en-US");
                    if (OcrEngine.IsLanguageSupported(englishLanguage))
                    {
                        ocrEngine = OcrEngine.TryCreateFromLanguage(englishLanguage);
                    }
                }

                if (ocrEngine == null)
                {
                    txtOcrResult.Text = "OCR engine not available. Please ensure OCR language packs are installed in Windows Settings > Time & Language > Language.";
                    return;
                }

                // perform OCR
                var ocrResult = await ocrEngine.RecognizeAsync(softwareBitmap);

                // check if any text was found
                if (ocrResult == null || ocrResult.Lines.Count == 0)
                {
                    txtOcrResult.Text = "No text detected in the image.";
                    return;
                }

                // extract text with formatting preserved
                ExtractedText = BuildFormattedText(ocrResult);
                txtOcrResult.Text = ExtractedText;
            }
            catch (Exception ex)
            {
                txtOcrResult.Text = $"OCR failed: {ex.Message}\n\nDetails: {ex.GetType().Name}";
                System.Diagnostics.Debug.WriteLine($"OCR Error: {ex}");
            }
        }

        // == preprocess image to improve OCR accuracy == //
        private static Bitmap PreprocessImageForOcr(Bitmap original)
        {
            // calculate scale factor - OCR works best at higher resolutions
            int scaleFactor = 1;
            if (original.Width < 1000 || original.Height < 200)
            {
                scaleFactor = Math.Max(2, Math.Min(4, 1500 / Math.Max(original.Width, 1)));
            }

            int newWidth = original.Width * scaleFactor;
            int newHeight = original.Height * scaleFactor;

            // create a new bitmap with 32bpp ARGB format
            Bitmap processed = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(processed))
            {
                // set high quality rendering
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // draw white background first (helps with transparent images and dark themes)
                g.Clear(Color.White);

                // draw the original image scaled up
                g.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            return processed;
        }

        // == build formatted text preserving approximate spacing (for tabular data) == //
        private static string BuildFormattedText(OcrResult ocrResult)
        {
            var sb = new StringBuilder();

            foreach (var line in ocrResult.Lines)
            {
                var lineText = new StringBuilder();
                OcrWord? previousWord = null;

                foreach (var word in line.Words)
                {
                    if (string.IsNullOrWhiteSpace(word.Text))
                        continue;

                    if (previousWord != null)
                    {
                        // calculate horizontal gap between words
                        double gap = word.BoundingRect.X - (previousWord.BoundingRect.X + previousWord.BoundingRect.Width);
                        double avgCharWidth = previousWord.BoundingRect.Width / Math.Max(1, previousWord.Text.Length);

                        // estimate number of spaces based on gap size
                        int numSpaces = Math.Max(1, (int)Math.Round(gap / Math.Max(avgCharWidth, 1)));

                        // cap at reasonable maximum to prevent runaway spacing
                        numSpaces = Math.Min(numSpaces, 8);

                        lineText.Append(new string(' ', numSpaces));
                    }

                    lineText.Append(word.Text);
                    previousWord = word;
                }

                if (lineText.Length > 0)
                {
                    sb.AppendLine(lineText.ToString());
                }
            }
            return sb.ToString().TrimEnd();
        }

        // == copy screenshot to clipboard == //
        private void btnCopyScreenshot_Click(object sender, EventArgs e)
        {
            if (Screenshot != null)
            {
                Clipboard.SetImage(Screenshot);
            }
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
            }
        }
    }
}