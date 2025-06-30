using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using fa;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.report.Hms;
using Fa.report.Purchase;
using Fa.views.utils.Report.Hms;
using FADataAccessLibrary.report.Hms;

namespace Fa.reports.Hms
{
    enum PatientProcedureReportTableColumn
    {
        SNO, DATE, PatientId, PatientName, ProcedureName, Doctor, Nurse, Status
    }
    public partial class FormPatientProcedureReport : Form
    {
        public static string ChooseStatusErrorMsg = "Please select status.";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string NoRecordErrorMsg = "No information found..";

        long PatientId = 0L;
        RptPatientProcedure rptPatientProcedure = null!;
        public FormPatientProcedureReport()
        {
            InitializeComponent();
        }
        private void ProcedureRptBtnGo_Click(object sender, EventArgs e)
        {
            LoadPatientProcedure();
        }
        private void LoadPatientProcedure()
        {
            ProcedureRptErrMsg.Text = "";
            GridviewProcedureRpt.Rows.Clear();
            if (ValidateForm())
            {
                rptPatientProcedure = new RptPatientProcedure();
                rptPatientProcedure.PatientId = PatientId;
                rptPatientProcedure.FromDate = (DateTime)ProcedureRptFromDate.Date!;
                rptPatientProcedure.ToDate = (DateTime)ProcedureRptToDate.Date!;
                rptPatientProcedure.Company = Global.Company;
                rptPatientProcedure.CompanyId = Global.Company.CompanyId;
                rptPatientProcedure.GenerateReport();
                if (rptPatientProcedure.LineItems.Count > 0)
                {
                    EnableButton(true);
                    int i = 0;
                    int RowIndex = 0;
                    string lDate = string.Empty;
                    string patientId = string.Empty;
                    string patientName = string.Empty;
                    string status = string.Empty;
                    string Doctor = string.Empty;
                    string Nurse = string.Empty;
                    foreach (PatientProcedureReportLineItem Item in rptPatientProcedure.LineItems.OrderBy(x => x.Date))
                    {
                        GridviewProcedureRpt.Rows.Add();
                        if (lDate == string.Empty || (lDate != Item.Date.ToString(Global.Company.DateFormat) || patientName != Item.PatientName))
                        {
                            GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.SNO].Value = i + 1;
                            GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.DATE].Value = Item.Date.ToString(Global.Company.DateFormat);
                            lDate = Item.Date.ToString(Global.Company.DateFormat);
                            i++;
                        }

                        if (status == string.Empty || (status != Item.Status || patientId != Item.PatientId))
                        {
                            GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.Status].Value = Item.Status;
                            status = Item.Status;
                        }
                        if (Doctor == string.Empty || Doctor != Item.Doctor || patientId != Item.PatientId)
                        {
                            GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.Doctor].Value = Item.Doctor;
                            Doctor = Item.Doctor;
                        }
                        if (Nurse == string.Empty || Nurse != Item.Nurse || patientId != Item.PatientId)
                        {
                            GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.Nurse].Value = Item.Nurse;
                            Nurse = Item.Nurse;
                        }
                        if (patientName == string.Empty || patientName != Item.PatientName || patientId != Item.PatientId)
                        {
                            GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.PatientName].Value = Item.PatientName;
                            patientName = Item.PatientName;
                        }
                        if (patientId == string.Empty || patientId != Item.PatientId)
                        {
                            GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.PatientId].Value = Item.PatientId;
                            patientId = Item.PatientId;
                        }
                        GridviewProcedureRpt.Rows[RowIndex].Cells[(int)PatientProcedureReportTableColumn.ProcedureName].Value = Item.ProcedureName;

                        RowIndex++;
                    }
                }
                else
                {
                    ProcedureRptErrMsg.Text = NoRecordErrorMsg;
                }
            }
        }
        private bool ValidateForm()
        {
            if (ProcedureRptFromDate.Date == null || !DateUtils.ValidDate(((DateTime)ProcedureRptFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ProcedureRptErrMsg.Text = EnterValidDateErrorMsg;
                ProcedureRptFromDate.Focus();
                return false;
            }
            if (ProcedureRptToDate.Date == null || !DateUtils.ValidDate(((DateTime)ProcedureRptToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ProcedureRptErrMsg.Text = EnterValidDateErrorMsg;
                ProcedureRptToDate.Focus();
                return false;
            }
            return true;
        }
        private void FormPatientProcedureReport_Load(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void ResetForm()
        {
            ProcedureRptErrMsg.Text = "";
            GridviewProcedureRpt.Rows.Clear();
            ProcedureRptFromDate.Format = Global.Company.DateFormat;
            ProcedureRptFromDate.Date = Global.getTransactionDate().AddDays(-30);
            ProcedureRptToDate.Format = Global.Company.DateFormat;
            ProcedureRptToDate.Date = Global.getTransactionDate();
            EnableButton(false);
        }
        private void EnableButton(bool Enable)
        {
            buttonPrint.Enabled = Enable;
            buttonSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                buttonCancel.PerformClick();
                return true;
            }
            if (keyData == Keys.F9)
            {
                buttonPrint.PerformClick();
                return true;
            }
            if (keyData == Keys.F8)
            {
                buttonSave.PerformClick();
                return true;
            }
            if (keyData == Keys.F10)
            {
                buttonExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)ProcedureRptFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ProcedureRptToDate.Date!).ToString(Global.Company.DateFormat);
            PatientProcedureReportPrintSaveA4 patientProcedureReportPrint = new PatientProcedureReportPrintSaveA4();
            patientProcedureReportPrint.ExportOrPrintToFile(GridviewProcedureRpt, rptPatientProcedure.ReportName(), "Patient Procedure Report", "pdf", false, lFromDate, lToDate);
        }
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            string lFromDate = ((DateTime)ProcedureRptFromDate.Date!).ToString(Global.Company.DateFormat);
            string lToDate = ((DateTime)ProcedureRptToDate.Date!).ToString(Global.Company.DateFormat);
            PatientProcedureReportPrintSaveA4 patientProcedureReportPrint = new PatientProcedureReportPrintSaveA4();
            patientProcedureReportPrint.ExportOrPrintToFile(GridviewProcedureRpt, rptPatientProcedure.ReportName(), "Patient Procedure Report", "pdf", true, lFromDate, lToDate);
        }
    }
}
