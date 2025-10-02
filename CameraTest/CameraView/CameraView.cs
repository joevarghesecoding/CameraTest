using Camera;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraTest
{
    public partial class CameraView : Form
    {
        private VideoCapture? _cap1;
        private VideoCapture? _cap2;
        private VideoCapture? _cap3;
        private Mat? _frame1;
        private Mat? _gsframe1;
        private Mat? _binaryimg1;
        private Mat? _frame2;
        private Mat? _gsframe2;
        private Mat? _binaryimg2;
        private Mat? _frame3;
        private Mat? _gsframe3;
        private Mat? _binaryimg3;
        double zoom1 = 0.2;
        double zoom2 = 0.2;
        double zoom3 = 0.2;
        const double zoomVal = 0.1;
        private readonly string Path = @"C:\CameraCaptures\";
        private FrameManager frameManager;
        private string modelText;
        private string snText;
        public CameraView()
        {
            InitializeComponent();
            InitializeCameraCapture();
        }

        private void InitializeCameraCapture()
        {
            frameManager = new FrameManager(this);

        }

        #region Initialization_and_load
        private void Camera_form_load(object sender, EventArgs e)
        {
            Camera1_form_load(sender, e);
            //Camera2_form_load(sender, e);
            //Camera3_form_load(sender, e);
        }

        private void Camera1_form_load(object sender, EventArgs e)
        {
            // Setup UI defaults
            focus1_trackbar.Enabled = true;
            focus1_trackbar.Value = 20;               // hardcode focus = 20
            brightness1_trackbar.Value = 70;          // hardcode brightness = 70

            // Start camera (device index 0) using DirectShow backend
            try
            {
                _cap1 = new VideoCapture(0, VideoCaptureAPIs.DSHOW);

                if (!_cap1.IsOpened())
                {
                    MessageBox.Show("Could not open camera. Make sure the Logitech Brio is connected.", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Set 4K resolution and FPS
                // 3840x2160@30

                // Set 1080 
                // 1920x1080@60

                _cap1.Set(VideoCaptureProperties.FrameWidth, 3840);
                _cap1.Set(VideoCaptureProperties.FrameHeight, 2160);
                _cap1.Set(VideoCaptureProperties.Fps, 10);

                // Initialize properties
                _cap1.Set(VideoCaptureProperties.AutoFocus, 1); // auto focus by default

                _frame1 = new Mat();
                _gsframe1 = new Mat();
                _binaryimg1 = new Mat();


                camera1_timer.Start();

                _cap1.Set(VideoCaptureProperties.Focus, 10);    // hardcode focus = 10
                _cap1.Set(VideoCaptureProperties.Brightness, 70); // hardcode brightness = 70

                Task.Delay(500).Wait();

                _cap1.Set(VideoCaptureProperties.Focus, 40);    // hardcode focus = 20
                _cap1.Set(VideoCaptureProperties.Brightness, 70); // hardcode brightness = 90
                _cap1.Set(VideoCaptureProperties.Zoom, 100);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing camera: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void zoom1_Slider(object? sender, EventArgs e)
        {
            zoom1 = ((double)zoom1_trackbar.Value / 10);
            zoom1_val_label.Text = zoom1.ToString();
        }

       

        private void brightness1_Scroll(object sender, EventArgs e)
        {
            if (_cap1 == null) return;
            try
            {
                _cap1.Set(VideoCaptureProperties.Brightness, brightness1_trackbar.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not set brightness: {ex.Message}", "Camera", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       
        private void focus1_Scroll(object sender, EventArgs e)
        {
            if (_cap1 == null) return;
            try
            {
                _cap1.Set(VideoCaptureProperties.Focus, focus1_trackbar.Value);
                focus_val_label.Text = focus1_trackbar.Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not set brightness: {ex.Message}", "Camera", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        


        private void timer1_Tick(object? sender, EventArgs e)
        {
            if (_cap1 == null || _frame1 == null) return;
            try
            {
                if (_cap1.Read(_frame1) && !_frame1.Empty())
                {
                    Mat zoomedoutImage = new Mat();
                    Cv2.Resize(_frame1, zoomedoutImage, new OpenCvSharp.Size(), zoom1, zoom1, InterpolationFlags.Linear);
                    var newBmp = MatToBitmap(zoomedoutImage);
                    // Convert to bitmap and safely swap into PictureBox
                    //var newBmp = MatToBitmap(_frame1);
                    var old = camera1_picturebox.Image;
                    camera1_picturebox.Image = newBmp;
                    old?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Frame read error: {ex.Message}");
            }
        }

        private static Bitmap MatToBitmap(Mat src)
        {
            if (src.Empty())
                throw new ArgumentException("Empty frame");

            // Ensure BGR 8-bit, then copy safely into a managed Bitmap via LockBits
            using var bgr = new Mat();
            if (src.Type() == MatType.CV_8UC3)
            {
                src.CopyTo(bgr);
            }
            else if (src.Channels() == 4)
            {
                Cv2.CvtColor(src, bgr, ColorConversionCodes.BGRA2BGR);
            }
            else if (src.Channels() == 1)
            {
                Cv2.CvtColor(src, bgr, ColorConversionCodes.GRAY2BGR);
            }
            else
            {
                src.ConvertTo(bgr, MatType.CV_8UC3);
            }

            var bmp = new Bitmap(bgr.Width, bgr.Height, PixelFormat.Format24bppRgb);
            var rect = new Rectangle(0, 0, bgr.Width, bgr.Height);
            var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            try
            {
                int srcStride = (int)bgr.Step();
                int dstStride = data.Stride;
                int rowBytes = bgr.Width * 3; // 24bpp
                byte[] buffer = new byte[rowBytes];
                IntPtr srcBase = bgr.Data;
                IntPtr dstBase = data.Scan0;
                for (int y = 0; y < bgr.Height; y++)
                {
                    IntPtr srcRow = srcBase + y * srcStride;
                    IntPtr dstRow = dstBase + y * dstStride;
                    System.Runtime.InteropServices.Marshal.Copy(srcRow, buffer, 0, rowBytes);
                    System.Runtime.InteropServices.Marshal.Copy(buffer, 0, dstRow, rowBytes);
                }
            }
            finally
            {
                bmp.UnlockBits(data);
            }
            return bmp;
        }

        private void Camera_FormClosing(object sender, FormClosingEventArgs e)
        {
            camera1_timer.Stop();
            var old1 = camera1_picturebox.Image;
            camera1_picturebox.Image = null;
            old1?.Dispose();
            _frame1?.Dispose();
            _cap1?.Release();
            _cap1?.Dispose();

        }

        #endregion
        private async Task RunCameraProcess(CancellationToken ct, CancellationTokenSource foundCts, TaskCompletionSource<(string, string)> foundTcs)
        {
            try
            {
                
                string model = string.Empty;
                string barcode = string.Empty;
                DateTime now = DateTime.Now;
                DateTime future = DateTime.Now.AddSeconds(15);
                while (now < future)
                {
                    now = DateTime.Now;
                    Mat foundFrame = await frameManager.InspectFrames(_frame1, ct, foundCts.Token);
                    bool barcodeSaved = await frameManager.TakePhoto(foundFrame, "Barcode", ct, foundCts);
                    if (barcodeSaved)
                    {
                        string ImagePath = $@"{Path}Barcode\Image_1.jpg";
                        barcode = frameManager.TesseractBarcodeScanner(ImagePath, ct);
                        if (String.IsNullOrEmpty(barcode))
                        {
                            barcode = frameManager.BarCodeReader(ImagePath, ct);
                        }
                        Console.WriteLine($"Barcode found: {barcode}");
                        if (!String.IsNullOrEmpty(barcode)) break;
                    }
                }
                now = DateTime.Now;
                future = DateTime.Now.AddSeconds(15);
                while (now < future && !foundCts.Token.IsCancellationRequested)
                {
                    Mat foundFrame = await frameManager.InspectFrames(_frame1, ct, foundCts.Token);
                    bool modelSaved = await frameManager.TakePhoto(foundFrame, "Model", ct, foundCts);
                    if (modelSaved && !foundCts.Token.IsCancellationRequested)
                    {
                        string ImagePath = $@"{Path}Model\Image_1.jpg";
                        string OutputPath = $@"{Path}Model\output.txt";
                        if (!frameManager.GetOcrTesseract(ImagePath, out model , ct, foundCts))
                        {
                            Task.Run(async () =>
                            {
                                await Task.Delay(2000);
                                model = await frameManager.GetModelNoFromAI(ImagePath, OutputPath, ct, foundCts);
                            }, foundCts.Token);
                        }
                        else
                        {
                            foundCts.Cancel();
                        }

                    }
                }

                
                foundTcs.TrySetResult((barcode, model));
                return;
            }
            catch
            {

            }
        }

        private void scanner_timer_Tick(object sender, EventArgs e)
        {
           
        }

     
    }
}
