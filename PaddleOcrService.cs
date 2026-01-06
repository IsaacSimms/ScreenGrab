///// PaddleOCER service: handles the OCR functionality using PaddleOCR engine. NuGet packages are onboard to provide functionality /////
///// OCRScreenshot.cs handles the UI and interaction with this service                                                             /////
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;
using Sdcb.PaddleOCR.Models.LocalV5;

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
                    var result = _ocrEngine.Run(processedBitmap);
                }
            });
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




