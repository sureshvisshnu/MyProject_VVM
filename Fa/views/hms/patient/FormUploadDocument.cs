using fa.api.Hms;
using fa.libraries.utils;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.patient
{
    public partial class FormUploadDocument : Form
    {
        public long PatientId = 0L;
        public long CategoryId = 0L;
        private object DocFile=null;
        private string Extension=string.Empty;
        public bool FileUploadStatus = true;

        public static string UploadStatus = "Uploaded...";
        public static string UploadStatusFileSizeError = "File size large than 10MB cant Upload it";
        public static string UploadStatusError = "Something went wrong. Please contact administrator.";
        public FormUploadDocument()
        {
            InitializeComponent();
        }

        private void FormUploadDocument_Load(object sender, EventArgs e)
        {
            DocumentManager.Instance.CheckGlobalMaxAllowedPackets();
            TextBoxFileNameWithPath.Select();
            LoadLocation();
        }
        private void LoadLocation()
        {
            if (!LabelPath.Visible)
            {
                LabelDescription.Location =new Point(PictureBoxPath.Location.X, PictureBoxPath.Location.Y-3);
                TextBoxDescription.Location = new Point(LabelDescription.Location.X, LabelDescription.Location.Y +15);
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
                openFileDialog.Filter = "Document Files (*pdf;)|*pdf; |Image Files(*.jpeg;*.png;*.jpg)|*.jpeg;*.png;*.jpg";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PictureBoxPath.Visible = true;
                    LabelPath.Visible = true;
                    var FileSize = new FileInfo(openFileDialog.FileName).Length;
                    Extension = Path.GetExtension(openFileDialog.FileName);
                    var name = Path.GetFileNameWithoutExtension(openFileDialog.FileName) ;
                    TextBoxFileNameWithPath.Text = name;
                    LabelPath.Text = openFileDialog.FileName;
                    DocFile = ReadImageFile(openFileDialog.FileName); //File.ReadAllBytes(openFileDialog.FileName);
                    LoadLocation();
                }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ResetForm()
        {
            DocFile = null;
            Extension = string.Empty;
            TextBoxDescription.ResetText();
            TextBoxFileNameWithPath.ResetText();
            TextBoxFileNameWithPath.Select();
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

                return ms.ToArray();
            }
        }
        public static byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
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
            PatientDocument.CompanyId = Global.Company.CompanyId;
            PatientDocument.File = (byte[])DocFile;
            PatientDocument.FileName = TextBoxFileNameWithPath.Text;
            PatientDocument.Description = TextBoxDescription.Text;
            PatientDocument.FileType = ComboUtils.GetFileType(Extension);
            DocumentManager.Instance.AddPatientDocument(PatientDocument);
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                if (DocFile != null && PatientId != 0L && CategoryId != 0L)
                {
                    try
                    {
                        UpdatePatientDocument();
                    }
                    catch (Exception Ex)
                    {
                        if (Ex.HResult == -2146233087)
                        {                            
                            if(DocumentManager.Instance.SetGlobalMaxAllowedPackets())
                            {
                                UpdatePatientDocument();
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
                    TextBoxFileNameWithPath.Select();

                }
            }
        }
        
        private bool validate()
        {
            ErrorMsg.Text = string.Empty;
            if(string.IsNullOrEmpty(TextBoxFileNameWithPath.Text.Trim()))
            {
                ErrorMsg.Text = "Please enter file name.";
                TextBoxFileNameWithPath.Select();
                return false;
            }
            if (!(Extension.ToUpper() == ".DOC" || Extension.ToUpper() == ".DOCX" || Extension.ToUpper() == ".PDF"
                || Extension.ToUpper() == ".JPEG" || Extension.ToUpper() == ".JPG" || Extension.ToUpper() == ".PNG"))
            {
                ErrorMsg.Text = "Please choose valid file.";
                TextBoxFileNameWithPath.Select();
                return false;
            }
            if(!DocumentManager.Instance.CheckFileNameExists(CategoryId,PatientId, TextBoxFileNameWithPath.Text))
            {
                ErrorMsg.Text = "The file name " +TextBoxFileNameWithPath.Text+ Extension + " exists. Please alter filename";
                TextBoxFileNameWithPath.Select();
                return false;
            }
            long FileSize = 0;
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, DocFile);
                FileSize = s.Length;
                if (FileSize >= 10000000)
                {
                    ErrorMsg.Text = UploadStatusFileSizeError;
                    return false;
                }
            }
            
            return true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnChooseFile.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
