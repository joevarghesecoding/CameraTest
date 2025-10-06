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
using Tesseract;
using ZXing;

namespace Camera
{
    public class FrameManager
    {
        public const int TOTAL_SLOTS = 4;

        public string Model { get; set; }
        public string SN { get; set; }
        private readonly string MistralPath = @"C:\mistraltest.ps1";
        private readonly string PicPath = @"C:\CameraCaptures\";
        private static readonly string TessdataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");
        private const string Languages = "eng"; //
        public bool AiCallMade;
        public bool SnCallMade;
        private Form _cameraForm;

        private List<string> KnownAdapters = new List<string> { "MU24E1120200-A1", "PS-2.1-12-2WT2", "CPS024E120200U" };
        private Dictionary<string, string> KnownAdaptersMatch = new Dictionary<string, string>
        {
            { @"(?<model>P?S?-?2?\.?1?\-?1?2?\-?2?W?T?2?)", "PS-2.1-12-2WT2" },
            { @"(?<model>M?U?2?4?E?1?1?2?0?2?0?0?\-?A?1?)", "MU24E1120200-A1" },
            { @"(?<model>C?P?S?0?2?4?E?1?2?0?2?0?0?U?)" , "CPS024E120200U" }
        };


        public FrameManager(Form cameraForm) { _cameraForm = cameraForm; }
     

        public async Task<(Mat, string imagePath, string name)>  InspectFrames(Mat _frame, CancellationToken sensorCt, CancellationToken foundCt, string frame, string camType)
        {
            Console.WriteLine($"---------------Beginning {frame} Inspection---------------------");
            Mat sharpenedImage = new Mat();
            Mat _gsframe = new Mat();
            try
            {
                sensorCt.ThrowIfCancellationRequested();
                foundCt.ThrowIfCancellationRequested();
                Cv2.CvtColor(_frame, _gsframe, ColorConversionCodes.BGR2GRAY);

                Mat blurryImage = new Mat();
                //DEV COMMENT - This is somewhat working don't delete
                Cv2.GaussianBlur(_gsframe, blurryImage, new OpenCvSharp.Size(0, 0), sigmaX: 0.25); // Adjust sigmaX for blur intensity

                
                //DEV COMMENT - This is somewhat working don't delete
                Cv2.AddWeighted(_gsframe, 1.75, blurryImage, -0.25, 0, sharpenedImage); // Adjust weights for sharpening intensity

                double lap_score = LaplacianVariance(sharpenedImage);
                double ten_score = Tenengrad(sharpenedImage);
                Console.WriteLine($"------------{frame} Laplacian Variance: {lap_score}---------------------");
                Console.WriteLine($"------------{frame} Tenengrad : {ten_score}-----------------------------");


                if (lap_score > 40 && ten_score > 10)
                {
                    Console.WriteLine($"---------------Completing {frame} Inspection----------------");
                    string ImagePath = $@"{PicPath}{camType}\{frame}\Image_1.jpg";
                    string OutputPath = $@"{PicPath}{camType}\{frame}\output.txt";
                    var foundFrame = await TakePhoto(sharpenedImage, camType, foundCt, frame);
                    if(camType == "Barcode")
                    {
                        if (!String.IsNullOrEmpty(BarCodeReader(ImagePath, foundCt)) && !BarCodeReader(ImagePath, foundCt).Contains("Not Found"))
                            return (sharpenedImage, ImagePath, BarCodeReader(ImagePath, foundCt));
                    }
                    else
                    {
                        string Model = string.Empty;
                        GetOcrTesseract(ImagePath, out Model, foundCt);
                        if (!String.IsNullOrEmpty(Model))
                            return (sharpenedImage, ImagePath, Model);
                    }
                    
                }
                //return Task.FromResult(sharpenedImage);
            }
            catch (OperationCanceledException) when (sensorCt.IsCancellationRequested || foundCt.IsCancellationRequested) { }
            return (null, "", "");
        }
        

        //public async Task<bool> TakePhoto(Mat _sharpenedImage, string camera, CancellationToken sensorCt, CancellationTokenSource foundCts, string foundFrameStr)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        foundCts.Token.ThrowIfCancellationRequested();
        //        if (_sharpenedImage == null || _sharpenedImage.Empty())
        //        {
        //            //MessageBox.Show("No image to capture yet.", "Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return false;
        //        }
        //        string ImagePath = $@"{PicPath}\{camera}\{foundFrameStr}\Image_1.jpg";
        //        string OutputPath = $@"{PicPath}\{camera}\{foundFrameStr}\output.txt";
        //        Cv2.ImWrite(ImagePath, _sharpenedImage);
        //        return true;
 
        //    }
        //    catch(OperationCanceledException) when (sensorCt.IsCancellationRequested || foundCts.Token.IsCancellationRequested) { }
        //    return false ;
        //}

        public async Task<bool> TakePhoto(Mat _sharpenedImage, string camera, CancellationToken sensorCt, string foundFrameStr)
        {
            string result = string.Empty;
            try
            {
                if (_sharpenedImage == null || _sharpenedImage.Empty())
                {
                    //MessageBox.Show("No image to capture yet.", "Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                string ImagePath = $@"{PicPath}\{camera}\{foundFrameStr}\Image_1.jpg";
                string OutputPath = $@"{PicPath}\{camera}\{foundFrameStr}\output.txt";
                Cv2.ImWrite(ImagePath, _sharpenedImage);
                return true;

            }
            catch (OperationCanceledException) when (sensorCt.IsCancellationRequested) { }
            return false;
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

        public string TesseractScanner(string inputPath, CancellationToken ct = default)
        {
            try
            {
                ct.ThrowIfCancellationRequested();
                using var engine = new TesseractEngine(TessdataPath, Languages, EngineMode.Default);
                using var img = Pix.LoadFromFile(inputPath);
                using var page = engine.Process(img);

                string text = page.GetText();
                float conf = page.GetMeanConfidence() * 100f;
                if (conf > 40.0)
                {
                    return text;
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { }
            return string.Empty;
        }

        public string TesseractBarcodeScanner(string inputPath, CancellationToken ct = default)
        {
            try
            {
                ct.ThrowIfCancellationRequested();
                using var engine = new TesseractEngine(TessdataPath, Languages, EngineMode.Default);
                using var img = Pix.LoadFromFile(inputPath);
                using var page = engine.Process(img);

                string text = page.GetText();
                float conf = page.GetMeanConfidence() * 100f;
                if (conf > 40.0)
                {
                    return text;
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { }
            return string.Empty;
        }

        public bool GetOcrTesseract(string inputPath, out string Model, CancellationToken ct = default/*, CancellationTokenSource cts = default*/)
        {
            Model = string.Empty;
            try
            {
                ct.ThrowIfCancellationRequested();
                //cts.Token.ThrowIfCancellationRequested();
                using var engine = new TesseractEngine(TessdataPath, Languages, EngineMode.Default);
                using var img = Pix.LoadFromFile(inputPath);
                using var page = engine.Process(img);
               
                string text = page.GetText();
                float conf = page.GetMeanConfidence() * 100f;
                if(conf > 50.0)
                {
                    Console.WriteLine("=== OCR TEXT ===");
                    Console.WriteLine(text);
                    Console.WriteLine($"[Mean confidence: {conf:F1}%]");

                    Model = BestMatch(text);
                    return !String.IsNullOrEmpty(Model);
                }
                
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested /*|| cts.Token.IsCancellationRequested*/) { }
            return false;
        }

        private string BestMatch(string text)
        {
            foreach (var match in KnownAdaptersMatch)
            {
                var matched = new Regex(match.Key).Matches(text).Cast<Match>()
                                .Select(i => i.ToString().Length >= (match.Value.Length * 0.8)).ToList()
                                .Where(i => i == true);
                if (matched.Any())
                    return match.Value.ToString();

            }
            return string.Empty;
        }

        public async Task<string> GetModelNoFromAI(string inputPath, string outputPath, CancellationToken ct = default, CancellationTokenSource cts = default)
        {
            string ModelNo = string.Empty;
            try
            {
                AiCallMade = true;
                ct.ThrowIfCancellationRequested();
                cts.Token.ThrowIfCancellationRequested();
                string output = await AICall(inputPath, outputPath); // Call the AI Script 
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
                {
                    //cts.Cancel();
                    return ModelNo;
                }
                
                AiCallMade = false;
            }
            catch(OperationCanceledException) when (ct.IsCancellationRequested || cts.Token.IsCancellationRequested) { }
            return "";
        }

        public string BarCodeReader(string imagePath, CancellationToken ct = default)
        {
            try
            {
                SnCallMade = true;
                ct.ThrowIfCancellationRequested();
                var BarcodeReader = new BarcodeReader()
                {
                    AutoRotate = true,
                    Options = new ZXing.Common.DecodingOptions
                    {
                        TryHarder = true,
                        TryInverted = true,
                        PureBarcode = false
                        //PossibleFormats = new[] { BarcodeFormat.QR_CODE }
                    },

                };
                var barcodeBitmap = (Bitmap)Image.FromFile(imagePath);
                var result = BarcodeReader.Decode(barcodeBitmap);

                if (result != null)
                {
                    Console.WriteLine("SN : " + result.ToString());
                    return result.ToString();
                }
                SnCallMade = false;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { }
            return "Not Found";

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

    public enum PhotoType
    {
        Barcode,
        Model
    }


}
