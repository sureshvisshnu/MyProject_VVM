using System;
using System.Linq;
using System.Windows.Forms;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.api.utils;
using System.IO;
using fa.common;
using System.Drawing;
using static fa.Global;

namespace fa.views.controls.hms
{
    public partial class PatientInfoMin : UserControl
    {
        private int _Width = 197;
        private int _Height = 750;
        private int _ShortHeight = 505;

        private Boolean __Short = false;
        public Boolean Short
        {
            get
            {
                return __Short;
            }
            set
            {
                __Short = value;
                if(__Short)
                {
                    this.Height = _ShortHeight;
                }
            }
        }

        public SelectGender PatientGender { get; set; }
        public SelectGender PatientChild { get; set; }
        public PatientInfoMin()
        {
 
            InitializeComponent();
        }
        public void Clear()
        {
            LabelInsurance.Text = "**NO INSURANCE**";
            TextBoxPatientName.ResetText();
            TextBoxPatientDOB.Reset();            
            TextBoxPatientAge.ResetText();
            TextBoxPatientAddress.ResetText();
            TextBoxPatientGaurdianName.ResetText();
            PatientPhoto.Clear();
            PatientNumberOp.PatientNumber = "000000000000";
        }
        public PatientTypes Type;
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
                Patient PatientData = PatientManager.Instance.GetPatientById((long)PatientId);
                if (PatientData != null)
                {
                    
                    TextBoxPatientName.Text = PatientData.Name;
                    if (DateUtils.ValidDate(PatientData.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                    {
                        TextBoxPatientAge.Text = PatientData.Age.ToString();
                        TextBoxPatientDOB.Format = Global.Company.DateFormat;
                        TextBoxPatientDOB.Date = PatientData.DateOfBirth;
                        if (Enum.TryParse(typeof(Gender), PatientData.Gender.ToString(), out var gender))
                        {
                            PatientGender = (SelectGender)gender!;
                        }
                        else
                        {
                            PatientGender = SelectGender.Male; 
                        }
                        DateTime birthDate = PatientData.DateOfBirth;
                        int age = DateTime.Now.Year - birthDate.Year;

                        // Check if the birthday has not occurred yet this year
                        if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                        {
                            age--;
                        }

                        // Set PatientChild property based on age
                        if (age < 10)
                        {
                            PatientChild = SelectGender.Child;
                        }
                        else
                        {
                            PatientChild = SelectGender.Adult;
                        }
                    }                   
                    PatientNumberOp.PatientNumber = PatientData.PatientNumber;                    
                    if (PatientData.Photo != null)
                    {
                        try
                        {
                            using (MemoryStream stream = new MemoryStream(PatientData.Photo))
                            {
                                PatientPhoto.Photo = System.Drawing.Image.FromStream(stream);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine("Invalid image format: " + ex.Message);
                        }
                    }
                    else
                    {
                        PatientPhoto.Clear();
                    }
                    if (PatientData.Guardians != null && PatientData.Guardians.Count > 0)
                    {                       
                            TextBoxPatientGaurdianName.Text = PatientData.Guardians.ToList().First().Name;
                    }
                    else
                    {
                        TextBoxPatientGaurdianName.Text = "";
                    }
                    if (PatientData.InsuranceInfo != null && PatientData.InsuranceInfo.Count > 0)
                    {
                        int InsuranceActiveCount = 0;
                        foreach (var InsuranceActive in PatientData.InsuranceInfo)
                        {
                            if(Type ==PatientTypes.OutPatient)
                            {
                                if (InsuranceActive.IsActive && InsuranceActive.OPInsuranceCoverage)
                                {
                                    InsuranceActiveCount += 1;
                                }
                            }
                            else if (Type == PatientTypes.InPatient)
                            {
                                if (InsuranceActive.IsActive && InsuranceActive.IPInsuranceCoverage)
                                {
                                    InsuranceActiveCount += 1;
                                }
                            }
                        }                        
                        if (InsuranceActiveCount > 0)
                        {
                            LabelInsurance.Text = "**INSURANCE**";
                        }
                        else
                        {
                            LabelInsurance.Text = "**NO INSURANCE**";
                        }                       
                    }
                    else
                    {
                        LabelInsurance.Text = "**NO INSURANCE**";
                    }

                    if (PatientData.Address != null)
                    {
                        TextBoxPatientAddress.Text = PatientData.Address.FullAddress.Replace(",", "," + System.Environment.NewLine);
                    }
                    else
                    {
                        TextBoxPatientAddress.Text = null;
                    }
                }
            }
        }

        private void PatientInfoMin_Resize(object sender, EventArgs e)
        {
            //this.MaximumSize = this.Size;
            //if (__Short)
            //{
            //    this.Height = _ShortHeight;
            //    this.Width = _Width;
            //    this.MinimumSize = this.Size;
            //}
            //else
            //{
            //    this.Height = _Height;
            //    this.Width = _Width;
            //    this.MinimumSize = this.Size;
            //}
        }

        private void PatientInfoMin_ClientSizeChanged(object sender, EventArgs e)
        {
            //this.MaximumSize = this.Size;
            //if (__Short)
            //{
            //    this.Height = _ShortHeight;
            //    this.Width = _Width;
            //    this.MinimumSize = this.Size;
            //}
            //else
            //{
            //    this.Height = _Height;
            //    this.Width = _Width;
            //    this.MinimumSize = this.Size;
            //}
        }
    }
}
