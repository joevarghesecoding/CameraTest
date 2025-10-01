using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace Camera
{
    public class FrameManager
    {
        public const int TOTAL_SLOTS = 4;

        private readonly string MistralPath = @"C:\mistraltest.ps1";
        private readonly string Path = @"C:\CameraCaptures\";

        private Form _cameraForm;

        public FrameManager(Form cameraForm) { _cameraForm = cameraForm; }
     

        public async Task<Mat> InspectFrames(Mat _frame, CancellationToken sensorCt, CancellationToken foundCt)
        {
            Mat sharpenedImage = new Mat();
            Mat _gsframe = new Mat();
            try
            {
                sensorCt.ThrowIfCancellationRequested();
                foundCt.ThrowIfCancellationRequested();
                Cv2.CvtColor(_frame, _gsframe, ColorConversionCodes.BGR2GRAY);

                Mat blurryImage = new Mat();
                //DEV COMMENT - This is somewhat working don't delete
                //Cv2.GaussianBlur(_gsframe, blurryImage, new OpenCvSharp.Size(0, 0), sigmaX: 1.5); // Adjust sigmaX for blur intensity
                Cv2.GaussianBlur(_gsframe, blurryImage, new OpenCvSharp.Size(0, 0), sigmaX: 0.25); // Adjust sigmaX for blur intensity

                
                //DEV COMMENT - This is somewhat working don't delete
                //Cv2.AddWeighted(_gsframe, 1.25, blurryImage, -0.75, 0, sharpenedImage); // Adjust weights for sharpening intensity
                Cv2.AddWeighted(_gsframe, 1.75, blurryImage, -0.25, 0, sharpenedImage); // Adjust weights for sharpening intensity

                double lap_score = LaplacianVariance(sharpenedImage);
                double ten_score = Tenengrad(sharpenedImage);

                if (lap_score > 8 && ten_score > 5)
                {
                    return sharpenedImage;
                }
            }
            catch (OperationCanceledException) when (sensorCt.IsCancellationRequested || foundCt.IsCancellationRequested) { }
            return null;
        }
        

        public async Task<(string barcode, string model)> TakePhoto(Mat _sharpenedImage, string camera, CancellationToken sensorCt, CancellationTokenSource foundCts)
        {
            string barcode = string.Empty;
            try
            {
                foundCts.Token.ThrowIfCancellationRequested();
                if (_sharpenedImage == null || _sharpenedImage.Empty())
                {
                    MessageBox.Show("No image to capture yet.", "Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return ("", "");
                }


                string ImagePath = $@"{Path}\{camera}\Image_1.jpg";
                string OutputPath = $@"{Path}\{camera}\output.txt";
                Cv2.ImWrite(ImagePath, _sharpenedImage);
                try
                {
                    foundCts.Token.ThrowIfCancellationRequested();
                    //string barcode = await BarCodeReader(ImagePath, foundCts.Token);
                    string model = await GetModelNoFromAI(ImagePath, OutputPath, foundCts.Token);
                    if (/*!String.IsNullOrEmpty(barcode) &&*/ !String.IsNullOrEmpty(model))
                        foundCts.Cancel();
                    //await Task.Run(async () =>
                    //{

                    //    //string barcode = await BarCodeReader(ImagePath);
                    //    //string model = await GetModelNoFromAI(ImagePath, OutputPath);
                    //    //if (!String.IsNullOrEmpty(barcode) && !String.IsNullOrEmpty(model))
                    //    //    foundCts.Cancel();
                    //});
                    return (barcode, model);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Save failed: {ex.Message}", "Capture", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ("", ""); 
                }

            }
            catch(OperationCanceledException) when (sensorCt.IsCancellationRequested || foundCts.Token.IsCancellationRequested) { return ("", ""); }
                      
        }


        private double LaplacianVariance(Mat gray)
        {
            using(var lap = new Mat())
            {
                Cv2.Laplacian(gray, lap, MatType.CV_64F);
                Cv2.MeanStdDev(lap, out _, out var stddev);
                return stddev.Val0 * stddev.Val0;
            }
        }

        private double Tenengrad(Mat gray)
        {
            using (var gx = new Mat())
            {
                using (var gy = new Mat())
                {
                    Cv2.Sobel(gray, gx, MatType.CV_32F, 1, 0);
                    Cv2.Sobel(gray, gy, MatType.CV_32F, 0, 1);
                    using (var mag = new Mat())
                    {
                        Cv2.Magnitude(gx, gy, mag);
                        // Optionally threshold mag to ignore noise, e.g., > 10
                        return Cv2.Mean(mag).Val0;
                    }
                       
                }
            }
        }

        private async Task<string> GetModelNoFromAI(string inputPath, string outputPath, CancellationToken ct = default)
        {
            string ModelNo = string.Empty;
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    string output = await AICall(inputPath, outputPath); // Call the AI Script 
                    ct.ThrowIfCancellationRequested();
                    string text = File.ReadAllText(outputPath);
                    ModelNo = new Regex(@"\**MODEL(\s?No.)?:\**[^\S\r\n]*(?<model>[\w.\-\\]+(?: [\w.-]+)*)")
                                     .Match(text)
                                     .Groups["model"]
                                     .Value;
                    if (String.IsNullOrEmpty(ModelNo))
                    {
                        ModelNo = new Regex(@"MODEL: (?<model>[\w]+)")
                            .Match(text).Groups["model"].Value;
                    }

                    if (!String.IsNullOrEmpty(ModelNo))
                        break;

                }
                MessageBox.Show($"Model: {ModelNo}");
            }
            catch(OperationCanceledException) when (ct.IsCancellationRequested) { }
         
           
            return ModelNo;
        }

        private Task<string> BarCodeReader(string imagePath, CancellationToken ct = default)
        {
            string val = string.Empty;
            try
            {
                ct.ThrowIfCancellationRequested();
                var BarcodeReader = new BarcodeReader()
                {
                    AutoRotate = true,
                    Options = new ZXing.Common.DecodingOptions
                    {
                        TryHarder = true,
                        //PossibleFormats = new[] { BarcodeFormat.QR_CODE }
                    },

                };
                var barcodeBitmap = (Bitmap)Image.FromFile(imagePath);
                var result = BarcodeReader.Decode(barcodeBitmap);
                
                if (result != null)
                {
                    MessageBox.Show(result.ToString());
                    val = result.ToString();
                }
               
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { }
            return Task.FromResult(val);

        }

        private Task<string> AICall(string ImagePath, string outputPath, CancellationToken ct = default)
        {
            string output = string.Empty;
            try
            {
                ct.ThrowIfCancellationRequested();
                if (!File.Exists(outputPath))
                {
                    File.Create(outputPath).Close();
                }
                
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "powershell.exe";

                // Pass parameters to the script if needed
                startInfo.Arguments = $"-ExecutionPolicy Bypass -NoProfile -File \"{MistralPath}\" {ImagePath} {outputPath} ";


                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    Console.WriteLine("Output:");
                    Console.WriteLine(output);

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Errors:");
                        Console.WriteLine(error);
                    }
                }

               
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { }
            return Task.FromResult(output);
        }

    }


}
