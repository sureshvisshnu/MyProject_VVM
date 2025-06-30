using fa.api.Hms;
using fa.libraries.utils;
using fa.model.hms.common;
using fa.model.Hms.Master;
using fa.views.hms;
using fa.views.hms.ip;
using fa.views.hms.patient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.patient
{
    public partial class FormUploadDocumentWithPreview : FormPatientBase
    {
        private object DocFile = null!;
        private string Extension = string.Empty;
        public bool FileUploadStatus = true;

        public long PatientId = 0L;
        public long CategoryId = 0L;
        public long PatientDocumentId = 0L;

        public static string UploadStatus = "Uploaded...";
        public static string UploadStatusFileSizeError = "File size large than 10MB cant Upload it";
        public static string UploadStatusError = "Something went wrong. Please contact administrator.";
        public static string FileErrStatusMsg = "Please enter file name..";
        public static string FileInvldStatusMsg = "Invalid file data...";
        public static string FileSelectStatusMsg = "Please choose a valid file...";
        public static string ImgFileErrStatusMsg = "The file is not a valid image...";
        public static string ErrStatus = "Error";

        FormPatientBase parent = null!;
        public FormUploadDocumentWithPreview(object sender)
        {
            InitializeComponent();
            if (sender is FormInPatientCareForNurse)
            {
                parent = (FormInPatientCareForNurse)sender;
            }
            else if (sender is FormLabTestResults)
            {
                parent = (FormLabTestResults)sender;
            }
            else if (sender is FormConsulting)
            {
                parent = (FormConsulting)sender;
            }
            else if (sender is PatientRegistration)
            {
                parent = (PatientRegistration)sender;
            }
        }

        private void FormUploadDocumentWithPreview_Load(object sender, EventArgs e)
        {
            DocumentManager.Instance.CheckGlobalMaxAllowedPackets();
            TextBoxFileName.Select();
        }
        private void LoadLocation()
        {
            if (!LabelPath.Visible)
            {
                LabelDescription.Location = new Point(PictureBoxPath.Location.X, PictureBoxPath.Location.Y - 3);
                TextBoxDescription.Location = new Point(LabelDescription.Location.X, LabelDescription.Location.Y + 15);
            }
            else
            {
                LabelDescription.Location = new Point(PictureBoxPath.Location.X, PictureBoxPath.Location.Y + 25); ;
                TextBoxDescription.Location = new Point(LabelDescription.Location.X, LabelDescription.Location.Y + 15);
            }
        }

        private void BtnChooseFile_Click(object sender, EventArgs e)
        {
            UploadDownload();
        }
        private void UploadDownload()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Document Files (*pdf;*.jpeg;*.png;*.jpg;*.gif;*.tiff;*.psd;*.eps;*.ai;*.indd;*.raw;*.jfif;*.bmp;*.pcx;*.tga;*.cr2;*.nef;*.orf;*.sr2;*.dwg;*.dxf;*.tpl;*.cvx;*.cnv;*.cvi)|*pdf;*.jpeg;*.png;*.jpg;*.gif;*.tiff;*.psd;*.eps;*.ai;*.indd;*.raw;*.jfif;*.bmp;*.pcx;*.tga;*.cr2;*.nef;*.orf;*.sr2;*.dwg;*.dxf;*.tpl;*.cvx;*.cnv;*.cvi";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PictureBoxPath.Visible = true;
                LabelPath.Visible = true;
                var FileSize = new FileInfo(openFileDialog.FileName).Length;
                Extension = Path.GetExtension(openFileDialog.FileName);
                var name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                TextBoxFileName.Text = name;
                LabelPath.Text = openFileDialog.FileName;
                DocFile = ReadImageFile(openFileDialog.FileName);
                EnableViewer();
                ShowDocument();
            }
        }
        private void EnableViewer()
        {
            LapTestPictureBox.Visible = true;
            LapTestPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            LapTestPictureBox.Image = null;
            DocBrowserPatient.LoadDocument("about:blank");
            pdfDocumentView.Refresh();
            DocBrowserPatient.Visible = false;
            pdfDocumentView.Visible = false;
        }

        private void ShowDocument()
        {
            FileType type = ComboUtils.GetFileType(Extension);
            byte[] attachment = (byte[])DocFile;

            if (IsImageFile(type))
            {
                ShowImage(attachment);
            }
            else
            {
                SaveAndOpenDocument(attachment, type);
            }
        }

        private bool IsImageFile(FileType type)
        {
            var imageTypes = new[]
            {
                FileType.RAW, FileType.JPG, FileType.PNG, FileType.GIF, FileType.CVX, FileType.CNV,
                FileType.CVI, FileType.EPS, FileType.BMP, FileType.PCX, FileType.TGA, FileType.TPL,
                FileType.NEF, FileType.ORF, FileType.DXF, FileType.DWG, FileType.PSD, FileType.CR2,
                FileType.SR2, FileType.AI, FileType.INDD, FileType.TIFF, FileType.JPEG, FileType.JFIF
            };

            return imageTypes.Contains(type);
        }

        private void ShowImage(byte[] attachment)
        {
            if (!IsValidFile(attachment))
            {
                MessageBox.Show(ImgFileErrStatusMsg, ErrStatus, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LapTestPictureBox.Image = null;
                return;
            }

            try
            {
                using (var stream = new MemoryStream(attachment))
                {
                    LapTestPictureBox.Image = System.Drawing.Image.FromStream(stream);
                    LapTestPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                Debug.WriteLine($"Error loading image: {ex.Message}");
                MessageBox.Show($"An error occurred. The file may not be valid ", ErrStatus, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (LapTestPictureBox != null)
                {
                    LapTestPictureBox.Image = null;
                }
            }
        }

        // Check for common image file signatures PNG JPEG GIF BMP TIFF TIFF (big) RIFF WEBP HEIC/HEIF ftyp heic PDF DOC, XLS DOCX, XLSX
        private bool IsValidFile(byte[] data)
        {
            if (data == null || data.Length < 12)
                return false;

            if (data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47)
                return true;

            if (data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF && (data[3] == 0xE0 || data[3] == 0xE1))
                return true;

            if (data[0] == 0x47 && data[1] == 0x49 && data[2] == 0x46 && data[3] == 0x38)
                return true;

            if (data[0] == 0x42 && data[1] == 0x4D)
                return true;

            if ((data[0] == 0x49 && data[1] == 0x49 && data[2] == 0x2A && data[3] == 0x00) ||
                (data[0] == 0x4D && data[1] == 0x4D && data[2] == 0x00 && data[3] == 0x2A))
                return true;

            if (data[0] == 0x52 && data[1] == 0x49 && data[2] == 0x46 && data[3] == 0x46 &&
                data[8] == 0x57 && data[9] == 0x45 && data[10] == 0x42 && data[11] == 0x50)
                return true;

            if (data[4] == 0x66 && data[5] == 0x74 && data[6] == 0x79 && data[7] == 0x70 &&
                data[8] == 0x68 && data[9] == 0x65 && data[10] == 0x69 && data[11] == 0x63)
                return true;

            if (data[0] == 0x25 && data[1] == 0x50 && data[2] == 0x44 && data[3] == 0x46)
                return true;

            if (data[0] == 0xD0 && data[1] == 0xCF && data[2] == 0x11 && data[3] == 0xE0)
                return true;

            if (data[0] == 0x50 && data[1] == 0x4B && data[2] == 0x03 && data[3] == 0x04)
                return true;

            return false;
        }
        
        private void SaveAndOpenDocument(byte[] attachment, FileType type)
        {
            try
            {
                string filename = TextBoxFileName.Text;

                string tempFilePath = Path.Combine(Path.GetTempPath(), filename + Extension);
                int counter = 1;

                while (File.Exists(tempFilePath))
                {
                    filename = $"{TextBoxFileName.Text}_{counter}";
                    tempFilePath = Path.Combine(Path.GetTempPath(), filename + Extension);
                    counter++;
                }

                File.WriteAllBytes(tempFilePath, attachment);

                if (type == FileType.DOC || type == FileType.DOCX)
                {
                    DocBrowserPatient.Visible = true;
                    DocBrowserPatient.LoadDocument(tempFilePath);
                }
                else
                {
                    pdfDocumentView.Visible = true;
                    pdfDocumentView.Load(tempFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                Debug.WriteLine($"Error loading image: {ex.Message}");
                MessageBox.Show($"An error occurred while opening the document. The file may be corrupted", ErrStatus, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null!;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }
        private void UpdatePatientDocument()
        {
            PatientDocument PatientDocument = new PatientDocument();
            PatientDocument.PatientId = PatientId;
            PatientDocument.PatientDocumentCategoryId = CategoryId;
            //PatientDocument.Id = PatientId;
            PatientDocument.CompanyId = Global.Company.CompanyId;
            PatientDocument.File = (byte[])DocFile;
            PatientDocument.FileName = TextBoxFileName.Text;
            PatientDocument.Description = TextBoxDescription.Text;
            PatientDocument.FileType = ComboUtils.GetFileType(Extension);
            DocumentManager.Instance.AddPatientDocument(PatientDocument);
            PatientDocumentId = PatientDocument.Id;
            CategoryId = PatientDocument.PatientDocumentCategoryId;
        }
        private void UpdateDocument()
        {
            LabTestAttachment LabTestAttachment = new LabTestAttachment();
            LabTestAttachment.Attachment = (byte[])DocFile;
            LabTestAttachment.FileName = TextBoxFileName.Text;
            LabTestAttachment.Description = TextBoxDescription.Text;
            LabTestAttachment.FileType = ComboUtils.GetFileType(Extension);
            if (parent is FormLabTestResults)
                ((FormLabTestResults)parent).LabTestAttachment = LabTestAttachment;
            if (parent is FormInPatientCareForNurse)
                ((FormInPatientCareForNurse)parent).LabTestAttachment = LabTestAttachment;
            if (parent is FormConsulting)
                ((FormConsulting)parent).LabTestAttachment = LabTestAttachment;
            if (parent is PatientRegistration)
            {
                UpdatePatientDocument(); // ((PatientRegistration)parent).LabTestAttachment = LabTestAttachment;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                if (DocFile != null)
                {
                    try
                    {
                        UpdateDocument();
                    }
                    catch (Exception Ex)
                    {
                        if (Ex.HResult == -2146233087)
                        {
                            if (DocumentManager.Instance.SetGlobalMaxAllowedPackets())
                            {
                                UpdateDocument();
                            }
                            else
                            {
                                Console.WriteLine(Ex.HResult);
                                FileUploadStatus = false;
                            }
                        }
                    }
                    this.Close();
                }
                else
                {
                    ErrorMsg.Text = UploadStatusError;
                    TextBoxFileName.Select();
                }
            }
        }
        private new bool Validate()
        {
            ErrorMsg.Text = string.Empty;

            if (string.IsNullOrEmpty(TextBoxFileName.Text.Trim()))
            {
                ErrorMsg.Text = FileSelectStatusMsg;  
                TextBoxFileName.Select();
                return false;
            }

            var validExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".DOC", ".DOCX", ".PDF", ".JPEG", ".JPG", ".PNG", ".GIF",
                ".TIFF", ".PSD", ".EPS", ".AI", ".INDD", ".RAW", ".JFIF",
                ".BMP", ".PCX", ".TGA", ".CR2", ".NEF", ".ORF", ".SR2",
                ".DWG", ".DXF", ".TPL", ".CVX", ".CVN", ".CVI"
            };

            if (!validExtensions.Contains(Extension))
            {
                ErrorMsg.Text = FileErrStatusMsg;  
                TextBoxFileName.Select();
                return false;
            }

            if (DocFile == null || !(DocFile is byte[] data))
            {
                ErrorMsg.Text = FileInvldStatusMsg;
                return false;
            }

            long fileSize = data.Length;
            if (fileSize >= 10000000) 
            {
                ErrorMsg.Text = UploadStatusFileSizeError;
                return false;
            }

            return true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F2:
                    BtnChooseFile.PerformClick();
                    return true;

                case Keys.F8:
                    BtnSave.PerformClick();
                    return true;

                case Keys.Escape:
                    BtnCancel.PerformClick();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
