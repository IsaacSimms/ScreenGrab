///// PaddleOCER service: handles the OCR functionality using PaddleOCR engine. NuGet packages are onboard to provide functionality /////
///// OCRScreenshot.cs handles the UI and interaction with this service                                                             /////
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;
using Sdcb.PaddleOCR.Models.LocalV5;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenGrab
{
    // == PaddleOcrService class to handle OCR functionality == //
    public class PaddleOcrService : IDisposable
    {
        private readonly PaddleOcrAll _ocrEngine;
        private bool _disposed;

        // == Constructor: Initialize PaddleOCR engine with LocalV5 model == //
        public PaddleOcrService()
        {
            FullOcrModel model = LocalFullModels.EnglishV4;
            _ocrEngine = new PaddleOcrAll(model)
            {
                AllowRotateDetection = true,
                Enable180Classification = true
            };
        }

        // == Perform OCR on the provided Bitmap image == //
        public Task<OcrResultData> RecognizeAsync(Bitmap image)
        {
            return Task.Run(() =>
            {
                if (image == null)
                {
                    return new OcrResultData
                    {
                        Success = false,
                        ErrorMessage = "No image provided."
                    };
                }
                try
                {
                    // preprocess image
                    using var processedBitmap = PreprocessImageForOcr(image);

                    // perform OCR directly on Bitmap
                    using var mat = BitmapConverter.ToMat(processedBitmap);

                    // run OCR
                    var result = _ocrEngine.Run(mat);

                    if (result.Regions.Length == 0)
                    {
                        return new OcrResultData
                        {
                            Success = true,
                            TextBox = string.Empty,
                            ErrorMessage = "No text detected in the image."
                        };
                    }

                    // build formatted text output
                    string formattedText = BuildFormattedText(result);
                    return new OcrResultData
                    {
                        Success = true,
                        TextBox = formattedText,
                        RegionCount = result.Regions.Length
                    };
                }
                catch (Exception ex)
                {
                    return new OcrResultData
                    {
                        Success = false,
                        ErrorMessage = $"OCR processing failed: {ex.Message}",
                        Exception = ex
                    };
                }
            });
        }

        // == Preprocess image for better OCR accuracy == //
        private static Bitmap PreprocessImageForOcr(Bitmap original)
        {
            // calculate scale factor
            int scaleFactor = 1;
            if (original.Width < 1000 || original.Height < 200)
            {
                scaleFactor = Math.Max(2, Math.Min(4, 1500 / Math.Min(original.Width, 1)));
            }
            int newWidth = original.Width * scaleFactor;
            int newHeight = original.Height * scaleFactor;

            // build new bitmap with 24pp RGB format
            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                // set high quality rendering
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // draw white background first
                g.Clear(Color.White);

                // draw the original image scaled up
                g.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            return resizedBitmap;
        }


        // == Build formatted text output from OCR result == //
        private static string BuildFormattedText(PaddleOcrResult ocrResult)
        {
            var sb = new StringBuilder();

            //sort regions by their position (top to bottom, left to right)
            var sortedRegions = ocrResult.Regions
                .OrderBy(r => r.Rect.Center.Y)
                .ThenBy(r => r.Rect.Center.X)
                .ToList();

            float? lastY = null;
            var lineBuilder = new StringBuilder();

            foreach (var region in sortedRegions)
            {
                float currentY = region.Rect.Center.Y;
                if (lastY.HasValue && Math.Abs(currentY - lastY.Value) > 15)
                {
                    // new line detected
                    if (lineBuilder.Length > 0)
                    {
                        sb.AppendLine(lineBuilder.ToString().Trim());
                        lineBuilder.Clear();
                    }
                }
                if (lineBuilder.Length > 0) 
                {
                    lineBuilder.Append(" ");
                }
                lineBuilder.Append(region.Text);
                lastY = currentY;
            }
            // append last line
            if (lineBuilder.Length > 0)
            {
                sb.AppendLine(lineBuilder.ToString().Trim());
            }
            // return final formatted text
            return sb.ToString().Trim();
        }

        // == Data class for OCR result == //
        public class OcrResultData 
        { 
            public bool Success { get; set; }
            public string TextBox { get; set; } = string.Empty;
            public int RegionCount { get; set; }
            public string? ErrorMessage { get; set; }
            public Exception? Exception { get; set; }
        }

        // == Dispose method to clean up resources == //
        public void Dispose()
        {
            if (!_disposed)
            {
                _ocrEngine.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}




