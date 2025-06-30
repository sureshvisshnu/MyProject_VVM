using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.controls.Photo
{
    public partial class CameraControl : UserControl
    {

        Bitmap image;
        Image SwapPhoto = null!;
        bool isPhoto = false;
        int index = 0;

        public VideoCapture videocapture;
        public bool isCameraRunning = false;

        Mat frame;
        public void SetLabelText(string text)
        {
            HeaderName.Text = text;
        }
        //private CameraCapture cameraCapture;

        public CameraControl()
        {
            InitializeComponent();
            //cameraCapture = new CameraCapture(pictureBoxPhoto, ComboBoxChooseCamara);
        }
        private void VisibleChange(bool enable)
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
        private void BtnStart_Click(object sender, EventArgs e)
        {
            ComboBoxChooseCamara.Items.Clear();
            var cameraNames = new List<string>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                foreach (var device in searcher.Get())
                {
                    cameraNames.Add(device["Caption"].ToString()!);
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

        private void CameraControl_TabIndexChanged(object sender, EventArgs e)
        {
            BtnStop.TabIndex = base.TabIndex;
            BtnTake.TabIndex = base.TabIndex;
            BtnStart.TabIndex = base.TabIndex;
        }

        private void CameraControl_Enter(object sender, EventArgs e)
        {
            if (BtnStart.Visible) { BtnStart.Select(); } else { BtnTake.Select(); }
        }

        private void btnPatientPhotoUpload_Click(object sender, EventArgs e)
        {
            if (videocapture != null && !videocapture.IsDisposed)
            {
                Application.Idle -= Streaming;
                videocapture.Release();
                videocapture.Dispose();
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

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (videocapture != null && !videocapture.IsDisposed)
            {
                Application.Idle -= Streaming!;
                videocapture.Release();
                videocapture.Dispose();
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

        private void BtnTake_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                if (videocapture.IsOpened())
                {
                    Bitmap snapshot = new Bitmap(image);
                    Application.Idle -= Streaming!;
                    videocapture.Release();
                    videocapture.Dispose();
                    isCameraRunning = false;
                    isPhoto = true;
                    pictureBoxPhoto.Image = snapshot;
                    VisibleChange(false);
                    BtnRemove.Visible = true;
                }
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            pictureBoxPhoto.Image = fa.Properties.Resources.patientphoto;
            isPhoto = false;
            BtnRemove.Visible = false;
        }
        public event EventHandler<string> CameraCaptureFailed;
        private void ComboBoxChooseCamara_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (ComboBoxChooseCamara.SelectedIndex > -1)
            {
                index = ComboBoxChooseCamara.SelectedIndex;
                image = new Bitmap(201, 204, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                SwapPhoto = pictureBoxPhoto.Image;
                isCameraRunning = true;
                videocaptureCamera(sender, e);
                VisibleChange(true);
                ComboBoxChooseCamara.SelectedIndexChanged -= ComboBoxChooseCamara_SelectedIndexChanged!;
                ComboBoxChooseCamara.Visible = false;
                ComboBoxChooseCamara.SelectedIndexChanged += ComboBoxChooseCamara_SelectedIndexChanged!;
                ComboBoxChooseCamara.SelectedIndex = -1;
                BtnTake.Visible = true;
                Cursor.Current = Cursors.Default;
            }
        }
        private void videocaptureCamera(object sender, System.EventArgs e)
        {
            if (isCameraRunning)
            {
                if (videocapture == null || videocapture.IsDisposed)
                {
                    videocapture = new VideoCapture();
                    frame = videocapture.RetrieveMat();
                    videocapture = new VideoCapture(index, VideoCaptureAPIs.DSHOW);
                }
                Application.Idle += Streaming!;
            }
            else
            {
                Application.Idle -= Streaming!;
            }
        }
        public void Streaming(object sender, System.EventArgs e)
        {
            if (isCameraRunning && videocapture != null && videocapture.IsOpened() == true && videocapture.Read(frame))
            {
                try
                {
                    if (videocapture.Read(frame) && frame.Empty() == false)
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
                    Application.Idle -= Streaming!;
                    VisibleChange(false);
                }
            }
        }
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
    }
}
