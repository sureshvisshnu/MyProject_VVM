using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Hms.Master;
using fa.api.Hms;
using fa.api.utils;
using System.IO;

namespace fa.views.controls.hms
{
    public partial class PatientInfoMiniHorizontal : UserControl
    {
        int Minimum_Width = 230;
        int Maximum_Width =831;
        int Maximum_Height = 121;
        int Default_FirstRow_Width = 324;
        int Default_SecondRow_Width = 28;
        int Default_ThiredRow_Width = 93;
        int Default_FourthRow_Width = 280;
        int Default_FifthRow_Width = 150;



        public PatientInfoMiniHorizontal()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            TextBoxPatientName.ResetText();
            TextBoxPatientDOB.ResetText();
            TextBoxPatientAge.ResetText();
            TextBoxPatientAddress.ResetText();
            PatientPhoto.Clear();
            PatientNumber.PatientNumber = "000000000000";
        }
        private long? _PatientId;
        public long? PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                _PatientId = value;
                LoadPatientInfo();
            }
        }
        private void LoadPatientInfo()
        {
            if (PatientId != null)
            {
                Clear();
                Patient PatientData = PatientManager.Instance.GetPatientById((long)PatientId);
                if (PatientData != null)
                {
                    TextBoxPatientName.Text = PatientData.Name;
                    if (DateUtils.ValidDate(PatientData.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                    {
                        TextBoxPatientAge.Text = PatientData.Age.ToString();
                        TextBoxPatientDOB.Format = Global.Company.DateFormat;
                        TextBoxPatientDOB.Date = (DateTime)DateUtils.ToDate(PatientData.DateOfBirth.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat);
                    }
                    PatientNumber.PatientNumber = PatientData.PatientNumber;
                    if (PatientData.Photo != null)
                    {
                        MemoryStream Stream = new MemoryStream(PatientData.Photo);
                        PatientPhoto.Photo = System.Drawing.Image.FromStream(Stream);
                    }
                    
                    if (PatientData.Address != null)
                    {
                        TextBoxPatientAddress.Text = PatientData.Address.FullAddress.Replace(",", "," + System.Environment.NewLine);
                    }
                }
            }
        }

        private void PatientInfoMin_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void TextBoxPatientName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        private void PatientInfoMiniHorizontal_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            if (this.Width > Minimum_Width)
            {
                int Difference = Default_FirstRow_Width - (Maximum_Width - this.Width);
                TextBoxPatientName.Width = Difference;
                TextBoxPatientAddress.Width = Difference;
                LabelAge.Location = new Point(TextBoxPatientName.Location.X+Difference+ 10, LabelAge.Location.Y);
                LabelDOB.Location = new Point(TextBoxPatientAddress.Location.X+ Difference + 10, LabelDOB.Location.Y);
                TextBoxPatientDOB.Location = new Point(LabelDOB.Location.X+Default_SecondRow_Width+10, TextBoxPatientDOB.Location.Y);
                TextBoxPatientAge.Location = new Point(LabelAge.Location.X+ Default_SecondRow_Width+10, TextBoxPatientAge.Location.Y);
                PatientNumber.Width = Default_FourthRow_Width;
                PatientNumber.Location = new Point(TextBoxPatientDOB.Location.X+Default_ThiredRow_Width+10, PatientNumber.Location.Y);
                PatientNumber.Width = Default_FifthRow_Width;
                PatientPhoto.Location = new Point(TextBoxPatientDOB.Location.X+Default_FourthRow_Width+10, PatientPhoto.Location.Y);
            }
            this.Height = Maximum_Height;
        }
    }
}
