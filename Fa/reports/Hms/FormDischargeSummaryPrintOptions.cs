using fa.views.utils.Hms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.reports.Hms
{
    public partial class FormDischargeSummaryPrintOptions : Form
    {
        public long PatientId = 0L;
        public long PatientIpId = 0L;
        public long DischargeId = 0L;
        public static string DischargeSummaryErrorMsg = "Discharge summary not available for current IP.";

        public FormDischargeSummaryPrintOptions()
        {
            InitializeComponent();
        }
        private void FormDischargeSummaryPrintOptions_Load(object sender, EventArgs e)
        {
            ErrorMsg.Text = string.Empty;
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            ErrorMsg.Text = string.Empty;
            MemoryStream Stream = new MemoryStream();
            PictureBoxMedicalHis.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] CheckedImg = Stream.ToArray();
            Stream = new MemoryStream();
            PictureBoxMedHisUnChecked.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] UnCheckedImg = Stream.ToArray();

            if (CheckBoxDischargeSummary.Checked)
            {               
                DischargeSummaryPrinting DischargeSummaryPrinting = new DischargeSummaryPrinting();
                DischargeSummaryPrinting.GenerateDischargeSummary(PatientId,PatientIpId, CheckedImg, UnCheckedImg, CheckBoxPartientChart.Checked, CheckBoxFull.Checked, CheckBoxPartientIPChart.Checked);
            }
            else if (CheckBoxPartientIPChart.Checked)
            {
                PatientIpChartPrinting PatientIpChartPrinting = new PatientIpChartPrinting()
                {
                    PatientId = PatientId,
                    PatientIpId = PatientIpId,
                    CheckedImg = CheckedImg,
                    UnCheckedImg = UnCheckedImg,
                    IncludeChart = CheckBoxPartientChart.Checked,
                    IncludeFullChart=CheckBoxFull.Checked
                };
                PatientIpChartPrinting.GenerateIpChart();
            }
            else if (CheckBoxPartientChart.Checked)
            {               
                List<string> Pages = new List<string>() { "Patient Details", "Insurance Details", "Medical History", "Prescription History", "LabTest History" };                  
                PatientChartPrinting PatientChartPrinting = new PatientChartPrinting()
                {
                    PatientId = PatientId,
                    PatientIpId=PatientIpId,
                    Pages = Pages,
                    CheckedImg = CheckedImg,
                    UnCheckedImg = UnCheckedImg,
                    IncludeFullChart = CheckBoxFull.Checked
                };
                PatientChartPrinting.GenerateChart();                
            }
            
            System.Windows.Forms.Cursor.Current = Cursors.Default;
            CheckBoxDischargeSummary.Focus();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckBoxPartientChart_CheckedChanged(object sender, EventArgs e)
        {
            if(CheckBoxPartientChart.Checked)
            {
                CheckBoxFull.Enabled = true;
            }
            else
            {
                CheckBoxFull.Checked = false;
                CheckBoxFull.Enabled = false;
            }
            EnableButton();
        }

        private void CheckBoxDischargeSummary_CheckedChanged(object sender, EventArgs e)
        {
            EnableButton();
        }
        private void EnableButton()
        {
            if(!CheckBoxPartientChart.Checked && !CheckBoxDischargeSummary.Checked && !CheckBoxPartientIPChart.Checked)
            {
                BtnPrint.Enabled = false;
            }
            else
            {
                BtnPrint.Enabled = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                BtnPrint.PerformClick();
            }
            if (keyData == Keys.F10)
            {
                BtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
