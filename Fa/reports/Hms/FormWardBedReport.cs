using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Hms.Master;
using fa.report;
using Microsoft.Office.Interop.Excel;
using fa.views.utils.Report.Hms;
using fa.api.Hms;
using fa.report.Hms;
using Global = fa.Global;
using fa.report.Ip;
using static fa.report.Hms.RptWardAndBed;
using fa.model.Hms.Ip;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Fa.views.utils.Report.Hms;
using fa.views.controls.ComboTreeView;

namespace Fa.reports.Hms
{
    enum WardBedReportGridColumn
    {
        Ward, BedNumber, BedType, RentType, Rent, AdmittedOn, IPNumber, PatientName, Available
    }
    public partial class FormWardBedReport : Form
    {
        WardManager WardManager = null;
        RptWardAndBed wardBedReport = null;

        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string SelectWardErrorMsg = "Please select ward.";
        public static string EmptyFieldErrorMsg = "No information found..";
        public static string SaveSuccessMsg = "Save success...";
        public FormWardBedReport()
        {
            WardManager = WardManager.Instance;
            InitializeComponent();
        }
        private void FormWardBedReport_Load(object sender, EventArgs e)
        {
            Loadcombo();
            ResetForm();
        }
        private void Loadcombo()
        {
            ComboUtils.InitializeAllWardCombo(ComboBoxWard, Global.Company.CompanyId);
        }
        private void RunWardBedReportButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            gridViewWarBedReport.Rows.Clear();
            EnableButtons(false);
            if (ValidateForm())
            {
                RunWardAndBedReport();
            }
        }
        private bool ValidateForm()
        {
            if (WardandBedFromDate.Date == null || !DateUtils.ValidDate(((DateTime)WardandBedFromDate.Date).ToString(fa.Global.Company.DateFormat), fa.Global.Company.DateFormat))
            {
                WardBedReportErrMsg.Text = EnterValidDateErrorMsg;
                WardandBedFromDate.Focus();
                return false;
            }
            if (WardandBedToDate.Date == null || !DateUtils.ValidDate(((DateTime)WardandBedToDate.Date).ToString(fa.Global.Company.DateFormat), fa.Global.Company.DateFormat))
            {
                WardBedReportErrMsg.Text = EnterValidDateErrorMsg;
                WardandBedToDate.Focus();
                return false;
            }
            if (WardandBedFromDate.Date > WardandBedToDate.Date)
            {
                WardBedReportErrMsg.Text = EnterValidDateErrorMsg;
                WardandBedFromDate.Focus();
                return false;
            }
            if (CheckedTreeUtils.SelectedNodes(ComboBoxWard).Count < 1)
            {
                WardBedReportErrMsg.Text = SelectWardErrorMsg;
                ComboBoxWard.Focus();
                return false;
            }
            return true;
        }
        private void RunWardAndBedReport()
        {
            WardBedReportErrMsg.Text = "";
            wardBedReport = new RptWardAndBed();
            wardBedReport.FromDate = (DateTime)WardandBedFromDate.Date;
            wardBedReport.ToDate = (DateTime)WardandBedToDate.Date;
            wardBedReport.Company = Global.Company;
            wardBedReport.WardId = CheckedTreeUtils.SelectedNodes(ComboBoxWard).Cast<long?>().ToArray();
            wardBedReport.GenerateReport();
            if (wardBedReport.LineItems != null && wardBedReport.LineItems.Count > 0)
            {
                gridViewWarBedReport.Rows.Add(wardBedReport.LineItems.Count);
                gridViewWarBedReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
                int NewRow = 0;
                foreach (WardBedReport LineItem in wardBedReport.LineItems)
                {
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.Ward].Value = LineItem.Ward;
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.BedNumber].Value = LineItem.BedNumber;
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.BedType].Value = LineItem.BedType;
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.RentType].Value = LineItem.RentType;
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.Rent].Value = LineItem.Rent.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.AdmittedOn].Value = LineItem.AdmittedOn.Year < 1900 ? string.Empty : LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.IPNumber].Value = LineItem.IPNumber;
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.PatientName].Value = LineItem.PatientName;
                    gridViewWarBedReport.Rows[NewRow].Cells[(int)WardBedReportGridColumn.Available].Value = LineItem.Available;
                    NewRow++;
                }
                EnableButtons(true);
            }
            else
            {
                EnableButtons(false);
                WardBedReportErrMsg.Text = EmptyFieldErrorMsg;
            }
        }
        private void ResetForm()
        {
            this.Text = " Ward and Bed Report ";
            statusStripWardBed.Text = string.Empty;
            gridViewWarBedReport.Rows.Clear();
            ComboBoxWard.Text = string.Empty;
            ComboBoxWard.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in ComboBoxWard.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            WardandBedFromDate.Format = fa.Global.Company.DateFormat;
            WardandBedFromDate.Date = fa.Global.getTransactionDate().AddDays(-30);
            WardandBedToDate.Format = fa.Global.Company.DateFormat;
            WardandBedToDate.Date = fa.Global.getTransactionDate();
            EnableButtons(false);
            this.Text = " Ward and Bed Report ";
        }
        private void EnableButtons(bool enable)
        {
            BtnPrint.Enabled = enable;
            BtnSave.Enabled = enable;
            BtnReset.Enabled = true;
            ToolStripWardBedReportSave.Enabled = enable;
            ToolStripWardBedReportPrint.Enabled = enable;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F10)
            {
                BtnExit.PerformClick();
            }
            else if (keyData == Keys.F9)
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == Keys.F8)
            {
                BtnSave.PerformClick();
            }
            else if (keyData == Keys.Escape)
            {
                BtnReset.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            WardBedReportSavePrint wardBedReportSavePrint = new WardBedReportSavePrint();
            wardBedReportSavePrint.SaveOrPrintToFile(gridViewWarBedReport, wardBedReport, "pdf", true);
            Cursor.Current = Cursors.Default;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            WardBedReportSavePrint wardBedReportSavePrint = new WardBedReportSavePrint();
            wardBedReportSavePrint.SaveOrPrintToFile(gridViewWarBedReport, wardBedReport, "pdf", false);
            Cursor.Current = Cursors.Default;
        }
        private void ComboBoxWard_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            string CheckedWardNodes = string.Empty;
            string CheckedPatientNodes = string.Empty;
            if (ComboBoxWard.CheckedNodes != null && ComboBoxWard.CheckedNodes.Count > -1)
            {
                foreach (ComboTreeNode node in ComboBoxWard.CheckedNodes)
                {
                    if (node.Name == "All") { CheckedWardNodes = " All wards"; break; }
                    CheckedWardNodes += ((string.IsNullOrEmpty(CheckedWardNodes) ? " " : ", ") + node.Text);
                }
                this.Text = "Ward and Bed Report @ " + CheckedWardNodes;
            }
            if (CheckedWardNodes == "" && CheckedPatientNodes == "")
            {
                this.Text = "Ward and Bed Report";
            }
        }
    }
}
