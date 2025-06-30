using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using fa.api.System;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Management;
using System.Text.RegularExpressions;
using Fa.views.utils.Camera;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace fa.views.controls
{
    public partial class PatientPhotoControl : UserControl
    {
        public VideoCapture capture;
        Mat frame;
        Bitmap image;
        Image SwapPhoto = null;
        int index = 0;
        public bool isCameraRunning = false;

        public PatientPhotoControl()
        {
            InitializeComponent();
        }
        //start
        private void BtnStart_Click(object sender, EventArgs e)
        {
            ComboBoxChooseCamara.Items.Clear();
            var cameraNames = new List<string>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                foreach (var device in searcher.Get())
                {
                    cameraNames.Add(device["Caption"].ToString());
                }
            }
            if (cameraNames.Count > 0)
            {
                ComboBoxChooseCamara.Items.AddRange(cameraNames.ToArray());
                ComboBoxChooseCamara.SelectedIndex = -1;
                if (cameraNames.Count > 1)
                {
                    VisibleChange(true);
                    ComboBoxChooseCamara.SelectedIndex = -1;
                    ComboBoxChooseCamara.Select();
                    ComboBoxChooseCamara.Text = "   Please select the camera";
                    ComboBoxChooseCamara.SelectionLength = ComboBoxChooseCamara.Text.Length;
                }
                if (cameraNames.Count == 1)
                {
                    ComboBoxChooseCamara.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("No device found for capture image..", "Warning..", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ComboBoxChooseCamara_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (ComboBoxChooseCamara.SelectedIndex > -1)
            {
                index = ComboBoxChooseCamara.SelectedIndex;
                image = new Bitmap(201, 204, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                SwapPhoto = pictureBoxPhoto.Image;
                isCameraRunning = true;
                CaptureCamera(sender, e);
                VisibleChange(true);
                ComboBoxChooseCamara.SelectedIndexChanged -= ComboBoxChooseCamara_SelectedIndexChanged;
                ComboBoxChooseCamara.Visible = false;
                ComboBoxChooseCamara.SelectedIndexChanged += ComboBoxChooseCamara_SelectedIndexChanged;
                ComboBoxChooseCamara.SelectedIndex = -1;
                BtnTake.Visible = true;
                Cursor.Current = Cursors.Default;
            }
        }
        private void CaptureCamera(object sender, System.EventArgs e)
        {
            if (isCameraRunning)
            {
                if (capture == null || capture.IsDisposed)
                {
                    capture = new VideoCapture();
                    frame = capture.RetrieveMat();
                    capture = new VideoCapture(index, VideoCaptureAPIs.DSHOW);
                }
                Application.Idle += Streaming;
            }
            else
            {
                Application.Idle -= Streaming;
            }
        }

        public void Streaming(object sender, System.EventArgs e)
        {
            if (isCameraRunning && capture != null && capture.IsOpened() == true && capture.Read(frame))
            {
                try
                {
                    if (capture.Read(frame) && frame.Empty() == false)
                    {
                        image = BitmapConverter.ToBitmap(frame);
                        pictureBoxPhoto.Image = image;
                    }
                }
                catch (Exception ex)
                {
                    fa.api.Log.Logger.LogError(ex);
                }
            }
            else
            {
                if (MessageBox.Show("Its look like another app is using the camera already.. ", "Close other apps..", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Idle -= Streaming;
                    VisibleChange(false);
                }
            }
        }

        //stop
        public void BtnStop_Click(object sender, EventArgs e)
        {
            if (capture != null && !capture.IsDisposed)
            {
                Application.Idle -= Streaming;
                capture.Release();
                capture.Dispose();
                isCameraRunning = false;
                BtnRemove.Visible = isPhoto;
                if (SwapPhoto != null)
                {
                    pictureBoxPhoto.Image = SwapPhoto;
                    isPhoto = true;
                }
                BtnStart.Select();
                VisibleChange(false);
            }
            else
            {
                VisibleChange(false);
            }
        }
        //Take photo
        private void BtnTake_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                if (capture.IsOpened())
                {
                    Bitmap snapshot = new Bitmap(image);
                    Application.Idle -= Streaming;
                    capture.Release();
                    capture.Dispose();
                    isCameraRunning = false;
                    isPhoto = true;
                    pictureBoxPhoto.Image = snapshot;
                    VisibleChange(false);
                    BtnRemove.Visible = true;
                }
            }
        }
        //Remove photo
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            pictureBoxPhoto.Image = fa.Properties.Resources.patientphoto;
            isPhoto = false;
            BtnRemove.Visible = false;
        }
        bool isPhoto = false;
        public Image Photo
        {
            get
            {
                if (isPhoto)
                {
                    return pictureBoxPhoto.Image;
                }
                return null;
            }
            set
            {
                pictureBoxPhoto.Image = value;
                if (value != null)
                {
                    BtnRemove.Visible = true;
                    isPhoto = true;
                }
            }
        }
        public void Clear()
        {
            BtnStop.PerformClick();
            pictureBoxPhoto.Image = fa.Properties.Resources.patientphoto;
            isPhoto = false;
            BtnRemove.Visible = false;
        }
        public void VisibleChange(bool enable)
        {
            if (enable)
            {
                BtnRemove.Visible = !enable;
                BtnStart.Visible = !enable;
                BtnTake.Visible = !enable;
                ComboBoxChooseCamara.Visible = enable;
                btnPatientPhotoUpload.Visible = !enable;
                BtnStop.Visible = enable;
            }
            else
            {
                BtnStart.Visible = !enable;
                BtnStop.Visible = enable;
                BtnTake.Visible = enable;
                ComboBoxChooseCamara.Visible = enable;
                btnPatientPhotoUpload.Visible = !enable;
            }
        }
        private void PatientPhotoControl_TabIndexChanged(object sender, EventArgs e)
        {
            BtnStop.TabIndex = base.TabIndex;
            BtnTake.TabIndex = base.TabIndex;
            BtnStart.TabIndex = base.TabIndex;
        }
        private void button3_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnStop.Select();
            }
        }
        private void PatientPhotoControl_Enter(object sender, EventArgs e)
        {
            if (BtnStart.Visible) { BtnStart.Select(); } else { BtnTake.Select(); }
        }
        private void btnPatientPhotoUpload_Click(object sender, EventArgs e)
        {
            if (capture != null && !capture.IsDisposed)
            {
                Application.Idle -= Streaming;
                capture.Release();
                capture.Dispose();
                isCameraRunning = false;
            }
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap Image = new Bitmap(open.FileName);
                Bitmap Resize = new Bitmap(Image, new System.Drawing.Size(230, 247));
                pictureBoxPhoto.Image = Resize;
                VisibleChange(false);
                isPhoto = true;
                BtnRemove.Visible = true;
                BtnStart.Select();
            }
        }
        private void PatientPhotoControl_Load(object sender, EventArgs e)
        {

        }
    }
}