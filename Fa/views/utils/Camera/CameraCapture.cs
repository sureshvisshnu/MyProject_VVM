using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System;
using VisioForge.MediaFramework.AudioEffects;
using OpenCvSharp.Extensions;
using AForge.Video.DirectShow;
using VisioForge.Libs.DirectShowLib;
using fa.api.Log;
using System.Management;

namespace Fa.views.utils.Camera
{
    public class CameraCapture
    {
        private readonly PictureBox pictureBox;
        private readonly ComboBox comboBoxChooseCamera;
        public VideoCapture capture;
        private VideoCaptureDevice videoSource;
        private VideoCaptureDevice videoCaptureDevice;
        private bool isCameraRunning;

        public CameraCapture(PictureBox pictureBox, ComboBox comboBoxChooseCamera)
        {
            this.pictureBox = pictureBox ?? throw new ArgumentNullException(nameof(pictureBox));
            this.comboBoxChooseCamera = comboBoxChooseCamera ?? throw new ArgumentNullException(nameof(comboBoxChooseCamera));
        }

        public bool CheckForAttachedCamera()
        {
            try
            {
                DsDevice[] devices = DsDevice.GetDevicesOfCat(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
                return devices.Length > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking for attached camera: " + ex.Message);
                return false;
            }
        }
        public int PopulateCameraList()
        {
            try
            {
                comboBoxChooseCamera.Items.Clear();
                var cameraNames = new List<string>();

                // Use ManagementObjectSearcher to query for camera devices
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
                {
                    // Iterate through search results to obtain device names (Caption)
                    foreach (var device in searcher.Get())
                    {
                        cameraNames.Add(device["Caption"].ToString());
                        comboBoxChooseCamera.Items.Add(device["Caption"].ToString());
                    }
                }
                return cameraNames.Count;

                // Check if any camera devices were found
                //if (cameraNames.Count > 0)
                //{
                //    // Add the device names to the ComboBox
                //    comboBoxChooseCamera.Items.AddRange(cameraNames.ToArray());
                //    comboBoxChooseCamera.SelectedIndex = -1;

                //    // Handle visibility and selection based on the count of devices
                //    if (cameraNames.Count > 1)
                //    {
                //        comboBoxChooseCamera.SelectedIndex = -1;
                //        comboBoxChooseCamera.Select();
                //    }
                //    else if (cameraNames.Count == 1)
                //    {
                //        comboBoxChooseCamera.SelectedIndex = 0;
                //    }

                //    // Return the count of devices found
                //    return cameraNames.Count;
                //}
                //else
                //{
                //    // No devices found, handle visibility and display a message
                //    MessageBox.Show("No camera devices found for capture...", "No Devices", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return 0;
                //}
            }
            catch (Exception ex)
            {
                // Exception occurred, display error message and return 0
                MessageBox.Show($"Error populating camera list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        //public int PopulateCameraList()
        //{
        //    try
        //    {
        //        comboBoxChooseCamera.Items.Clear();
        //        List<string> cameraNames = new List<string>();

        //        // Enumerate video capture devices
        //        DsDevice[] devices = DsDevice.GetDevicesOfCat(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);

        //        foreach (var device in devices)
        //        {
        //            comboBoxChooseCamera.Items.Add(device.Name);
        //        }

        //        return devices.Length;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0; 
        //    }
        //}
        //public void TakePhoto()
        //{
        //    if (isCameraRunning && capture != null)
        //    {
        //        // Read a single frame from the camera
        //        using (Mat frame = new Mat())
        //        {
        //            capture.Read(frame);

        //            if (!frame.Empty())
        //            {
        //                // Convert the frame to a Bitmap and display it in the PictureBox
        //                pictureBox.Image = BitmapConverter.ToBitmap(frame);
        //            }
        //        }
        //    }
        //}
        public static void DisplayCapturedImage(CameraCapture cameraCapture, PictureBox pictureBox, int selectedIndex, out bool cameraFound)
        {
            cameraFound = false;
            // Open the first camera device
            using (VideoCapture capture = new VideoCapture(selectedIndex))
            {
                if (!capture.IsOpened())
                {
                    Console.WriteLine("Failed to open camera device.");
                    return;
                }
                cameraFound = true;

                using (Mat frame = new Mat())
                {
                    capture.Read(frame);

                    if (frame.Empty())
                    {
                        Console.WriteLine("Failed to capture frame from camera.");
                        return;
                    }

                    Cv2.ImWrite("captured_image.jpg", frame);
                    Console.WriteLine("Image captured and saved as captured_image.jpg");

                    using (Bitmap capturedImage = (Bitmap)Image.FromFile("captured_image.jpg"))
                    {
                        int maxWidth = pictureBox.Width;
                        int maxHeight = pictureBox.Height;

                        float ratioX = (float)maxWidth / capturedImage.Width;
                        float ratioY = (float)maxHeight / capturedImage.Height;
                        float ratio = Math.Min(ratioX, ratioY);

                        int newWidth = (int)(capturedImage.Width * ratio);
                        int newHeight = (int)(capturedImage.Height * ratio);

                        Bitmap resizedImage = new Bitmap(newWidth, newHeight);
                        using (Graphics g = Graphics.FromImage(resizedImage))
                        {
                            g.DrawImage(capturedImage, 0, 0, newWidth, newHeight);
                        }

                        pictureBox.Image = resizedImage;
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        cameraCapture.isCameraRunning = false;
                        // Cv2.ImShow("Camera", frame);
                        // Cv2.WaitKey(0);
                    }
                }
            }
        }

        public void StartCapture(int selectedIndex)
        {
            if (!isCameraRunning)
            {
                try
                {
                    if (selectedIndex == -1)
                    {
                        Console.WriteLine("No camera selected.");
                        return;
                    }

                    // Open the selected camera device
                    capture = new VideoCapture(selectedIndex);
                    if (capture == null || !capture.IsOpened())
                    {
                        Console.WriteLine("Failed to open camera device.");
                        isCameraRunning = false;
                        return;
                    }

                    Console.WriteLine("Capture object created and camera device opened successfully.");

                    isCameraRunning = true;

                    // Start capturing frames in a separate thread
                    Task.Run(() => CaptureFrames());

                    Console.WriteLine("Camera capture started.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error starting camera capture: " + ex.Message);
                    isCameraRunning = false;
                }
            }
        }

        private void CaptureFrames()
        {
            while (isCameraRunning)
            {
                using (Mat frame = new Mat())
                {
                    if (capture != null)
                    {
                        try
                        {
                            capture.Read(frame);

                            if (frame.Empty())
                            {
                                continue;
                            }
                        }
                        catch (Exception ex) { Logger.LogError(ex); }
                        ProcessFrame(frame);
                    }
                }
            }
        }

        private void ProcessFrame(Mat frame)
        {
            // Process the captured frame
            // For example, display it in the picture box
            pictureBox.Image = BitmapConverter.ToBitmap(frame);
        }


        //public (VideoCapture? capture, string message) StartCapture(int selectedIndex)
        //{
        //    try
        //    {
        //        if (selectedIndex == -1)
        //        {
        //            return (null, "No camera selected.");
        //        }

        //        // Attempt to open the camera
        //        var capture = TryOpenCamera(selectedIndex);

        //        if (capture == null)
        //        {
        //            return (null, "Failed to open camera occupied by another device.");
        //        }

        //        // If the camera was opened successfully, set up the capture process
        //        Console.WriteLine("Capture object created and camera device opened successfully.");

        //        isCameraRunning = true;

        //        Application.Idle += ProcessFrame!;
        //        Console.WriteLine("Camera capture started.");

        //        return (capture, "Camera capture started.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return (null, "Error starting camera capture: " + ex.Message);
        //    }
        //}


        //private VideoCapture TryOpenCamera(int index)
        //{
        //    try
        //    {
        //        return new VideoCapture(index);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to open camera device with index {index}: {ex.Message}");
        //        return null;
        //    }
        //}
        
        private void ProcessFrame(object sender, EventArgs e)
        {
            using (Mat frame = new Mat())
            {
                if(capture != null)
                {
                    capture.Read(frame);

                    if (frame.Empty())
                    {
                        return;
                    }
                    pictureBox.Image = BitmapConverter.ToBitmap(frame);
                }
            }
        }
        public void StopCameraCapture()
        {
            if (isCameraRunning)
            {
                if (capture != null)
                {
                    Application.Idle -= ProcessFrame!;
                    capture.Dispose();
                    capture = null!;
                    isCameraRunning = false;
                }
            }
        }

        //public void StartCapture(int selectedIndex)
        //{
        //    if (!isCameraRunning)
        //    {
        //        try
        //        {
        //            if (selectedIndex == -1)
        //            {
        //                Console.WriteLine("No camera selected.");
        //                return;
        //            }

        //            // Open the selected camera device
        //            capture = new VideoCapture(selectedIndex);
        //            if (capture == null || !capture.IsOpened())
        //            {
        //                Console.WriteLine("Failed to open camera device.");
        //                isCameraRunning = false;
        //                return;
        //            }

        //            Console.WriteLine("Capture object created and camera device opened successfully.");

        //            isCameraRunning = true;

        //            Application.Idle += ProcessFrame!;
        //            Console.WriteLine("Camera capture started.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error starting camera capture: " + ex.Message);
        //            isCameraRunning = false;
        //        }
        //    }
        //}

        //----------------------------------------------
        //public void StartCapture(int selectedIndex)
        //{
        //    if (!isCameraRunning)
        //    {
        //        try
        //        {
        //            // Check if the selected index is within the range of available cameras
        //            if (selectedIndex < 0)
        //            {
        //                Console.WriteLine("Invalid camera index selected.");
        //                return;
        //            }

        //            // Try creating a VideoCapture object with the selected index
        //            capture = TryOpenCamera(selectedIndex);

        //            // Check if the VideoCapture object was created successfully
        //            if (capture == null || !capture.IsOpened())
        //            {
        //                Console.WriteLine("Failed to open camera device.");
        //                isCameraRunning = false;
        //                return;
        //            }

        //            Console.WriteLine("Capture object created and camera device opened successfully.");

        //            // Set the flag to indicate that the camera is running
        //            isCameraRunning = true;

        //            // Register the ProcessFrame method to handle frames
        //            Application.Idle += ProcessFrame!;

        //            Console.WriteLine("Camera capture started.");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error starting camera capture: " + ex.Message);
        //            isCameraRunning = false;
        //        }
        //    }
        //}
    }
}
