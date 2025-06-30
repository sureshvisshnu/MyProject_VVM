using fa;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.utils.Report.Inventory;
using Fa.reports.Inventory;
using Fa.views.utils.Report.Hms;
using Fa.views.utils.Report.Inventory;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace Fa.reports.Hms
{
    enum PatientTransferReportByDateTableColumn
    {
        SNO, IN_DATE, WARD, PATIENTId, PATIENT, BED, OUT_DATE, CONSULTANT, DESCRIPTION
    }
    enum PatientTransferReportByWardTableColumn
    {
        SNO, WARD, PATIENTId, PATIENT, BED, IN_DATE, OUT_DATE, CONSULTANT, DESCRIPTION
    }
    enum PatientTransferReportByConsultantTableColumn
    {
        SNO, CONSULTANT, WARD, PATIENTId, PATIENT, BED, IN_DATE, OUT_DATE, DESCRIPTION
    }
    public partial class FormPatientTransferReport : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        RptPatientTransfer RptPatientTransfer = null;
        public FormPatientTransferReport()
        {
            InitializeComponent();
        }

        private void FormPatientTransferReport_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadComboBox();
            ComboBoxTypeSelection.SelectedIndex = 0;
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
        }
        private void ResetForm()
        {
            GridViewTransferByDate.Rows.Clear();
            GridViewTransferByWard.Rows.Clear();
            GridViewTransferByConsultant.Rows.Clear();
            ErrorMsgTransferReport.Text = "";
            EnableButton(false);
            CheckedTreeComboBoxConsultant.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxConsultant.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            CheckedTreeComboBoxWard.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxWard.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            PatientTransferFromDate.Format = Global.Company.DateFormat;
            PatientTransferFromDate.Date = Global.getTransactionDate().AddDays(-30);
            PatientTransferToDate.Format = Global.Company.DateFormat;
            PatientTransferToDate.Date = Global.getTransactionDate();
            ComboBoxTypeSelection.SelectedIndex = 0;
            CheckedTreeComboBoxWard.Visible = false;
            CheckedTreeComboBoxConsultant.Visible = false;
            GridViewTransferByDate.Visible = true;
            GridViewTransferByWard.Visible = false;
            GridViewTransferByConsultant.Visible = false;
            ToolStripLabelPatientTransferConsultant.Visible = false;
            ToolStripLabelPatientTransferWard.Visible = false;
            ToolStripLabelPatientTransferConsultant.Visible = false;
            ToolStripLabelPatientTransferWard.Visible = false;
            PatientTransfertoolStripSeparator1.Visible = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadComboBox()
        {
            ComboUtils.InitializeDoctorCombo(CheckedTreeComboBoxConsultant, Global.Company.CompanyId);
            ComboUtils.InitializeAllWardCombo(CheckedTreeComboBoxWard, Global.Company.CompanyId);
        }
        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxTypeSelection.SelectedIndex == 0)
            {
                CheckedTreeComboBoxConsultant.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxConsultant.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxWard.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxWard.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxWard.Visible = false;
                CheckedTreeComboBoxConsultant.Visible = false;
                GridViewTransferByDate.Visible = true;
                GridViewTransferByWard.Visible = false;
                GridViewTransferByConsultant.Visible = false;
                ToolStripLabelPatientTransferConsultant.Visible = false;
                ToolStripLabelPatientTransferWard.Visible = false;
                ToolStripLabelPatientTransferConsultant.Visible = false;
                ToolStripLabelPatientTransferWard.Visible = false;
                PatientTransfertoolStripSeparator1.Visible = false;
                GridViewTransferByDate.Rows.Clear();
                GridViewTransferByWard.Rows.Clear();
                GridViewTransferByConsultant.Rows.Clear();
                EnableButton(false);
                this.Text = "Patient Transfer Report";
                ErrorMsgTransferReport.Text = "";
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                CheckedTreeComboBoxConsultant.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxConsultant.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxWard.Visible = true;
                CheckedTreeComboBoxConsultant.Visible = false;
                GridViewTransferByDate.Visible = false;
                GridViewTransferByWard.Visible = true;
                GridViewTransferByConsultant.Visible = false;
                ToolStripLabelPatientTransferConsultant.Visible = false;
                ToolStripLabelPatientTransferWard.Visible = false;
                ToolStripLabelPatientTransferConsultant.Visible = false;
                ToolStripLabelPatientTransferWard.Visible = true;
                PatientTransfertoolStripSeparator1.Visible = true;
                GridViewTransferByDate.Rows.Clear();
                GridViewTransferByWard.Rows.Clear();
                GridViewTransferByConsultant.Rows.Clear();
                EnableButton(false);
                this.Text = "Patient Transfer Report";
                ErrorMsgTransferReport.Text = "";
            }
            else
            {
                CheckedTreeComboBoxWard.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxWard.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxWard.Visible = false;
                CheckedTreeComboBoxConsultant.Visible = true;
                GridViewTransferByDate.Visible = false;
                GridViewTransferByWard.Visible = false;
                GridViewTransferByConsultant.Visible = true;
                ToolStripLabelPatientTransferConsultant.Visible = false;
                ToolStripLabelPatientTransferWard.Visible = false;
                ToolStripLabelPatientTransferConsultant.Visible = true;
                ToolStripLabelPatientTransferWard.Visible = false;
                PatientTransfertoolStripSeparator1.Visible = true;
                GridViewTransferByDate.Rows.Clear();
                GridViewTransferByWard.Rows.Clear();
                GridViewTransferByConsultant.Rows.Clear();
                EnableButton(false);
                this.Text = "Patient Transfer Report";
                ErrorMsgTransferReport.Text = "";
            }
        }
        private bool FormValidate()
        {
            ErrorMsgTransferReport.Text = "";
            if (ComboBoxTypeSelection.SelectedIndex < 0)
            {
                ErrorMsgTransferReport.Text = "Please select type";
                ComboBoxTypeSelection.Select();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 1 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxWard).Count < 1)
            {
                ErrorMsgTransferReport.Text = "Please select ward";
                CheckedTreeComboBoxWard.Focus();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxConsultant).Count < 1)
            {
                ErrorMsgTransferReport.Text = "Please select consulted";
                CheckedTreeComboBoxConsultant.Focus();
                return false;
            }
            if (PatientTransferFromDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientTransferFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgTransferReport.Text = EnterValidDateErrorMsg;
                PatientTransferFromDate.Focus();
                return false;
            }
            if (PatientTransferToDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientTransferToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgTransferReport.Text = EnterValidDateErrorMsg;
                PatientTransferToDate.Focus();
                return false;
            }
            return true;
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                if (CheckedTreeComboBoxWard.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in CheckedTreeComboBoxWard.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Wards"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = "Patient Transfer Report " + " @ " + CheckedNodes;
                }
                else
                {
                    this.Text = "Patient Transfer Report";
                }
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 2)
            {
                if (CheckedTreeComboBoxConsultant.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in CheckedTreeComboBoxConsultant.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Consulted"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = "Patient Transfer Report " + " @ " + CheckedNodes;
                }
                else
                {
                    this.Text = "Patient Transfer Report";
                }
            }
        }
        private string SelectedNodesText(ToolstripCheckedTreeComboBox ComboTreeBox)
        {
            int i = 0;
            string Name = string.Empty;
            if (ComboTreeBox.Nodes.Count > 0)
            {
                foreach (ComboTreeNode ComboTreeNode in ComboTreeBox.Nodes)
                {
                    if (ComboTreeNode != null)
                    {
                        if (ComboTreeNode.Checked == true)
                        {
                            if (ComboTreeNode.Name == "All")
                            {
                                Name = string.Empty;
                                Name = "All Location";
                                break;
                            }
                            else
                            {
                                Name += string.IsNullOrEmpty(Name) ? ComboTreeNode.Text : (", " + ComboTreeNode.Text);
                            }
                            i++;
                        }
                    }
                }
            }
            return Name;
        }
        private void BtnSearchPatientTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewTransferByDate.Rows.Clear();
                GridViewTransferByWard.Rows.Clear();
                GridViewTransferByConsultant.Rows.Clear();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadStockReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsgTransferReport.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            GridViewTransferByDate.DefaultCellStyle.SelectionForeColor = GridViewTransferByDate.DefaultCellStyle.ForeColor;
            GridViewTransferByDate.DefaultCellStyle.SelectionBackColor = GridViewTransferByDate.DefaultCellStyle.BackColor;
            GridViewTransferByWard.DefaultCellStyle.SelectionForeColor = GridViewTransferByWard.DefaultCellStyle.ForeColor;
            GridViewTransferByWard.DefaultCellStyle.SelectionBackColor = GridViewTransferByWard.DefaultCellStyle.BackColor;
            GridViewTransferByConsultant.DefaultCellStyle.SelectionForeColor = GridViewTransferByConsultant.DefaultCellStyle.ForeColor;
            GridViewTransferByConsultant.DefaultCellStyle.SelectionBackColor = GridViewTransferByConsultant.DefaultCellStyle.BackColor;
        }

        private void CheckedTreeComboBoxConsulted_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void CheckedTreeComboBoxWard_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadStockReport()
        {
            RptPatientTransfer = new RptPatientTransfer();
            RptPatientTransfer.FromDate = (DateTime)PatientTransferFromDate.Date;
            RptPatientTransfer.ToDate = (DateTime)PatientTransferToDate.Date;
            RptPatientTransfer.Company = Global.Company;
            RptPatientTransfer.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboBoxWard);
            RptPatientTransfer.Type = ComboBoxTypeSelection.SelectedIndex == 0 ? TransferType.BYDATE : ComboBoxTypeSelection.SelectedIndex == 1 ? TransferType.BYWARD : TransferType.BYCONSULTANT;
            RptPatientTransfer.WardIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxWard).ToArray();
            RptPatientTransfer.Ward = this.Text;
            RptPatientTransfer.IsAllWard = SelectedNodesText(CheckedTreeComboBoxWard) == "All Location" ? true : false;
            RptPatientTransfer.ConsultantIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxConsultant).ToArray();
            RptPatientTransfer.AuthorizedByDoctor = this.Text;
            RptPatientTransfer.IsAllConsultant = SelectedNodesText(CheckedTreeComboBoxConsultant) == "All Location" ? true : false;
            RptPatientTransfer.GenerateReport();
            int irow = 0;
            if (RptPatientTransfer.Type == TransferType.BYDATE)
            {
                GridViewTransferByDate.Rows.Clear();
                if (RptPatientTransfer.PatientTransferLineItems != null && RptPatientTransfer.PatientTransferLineItems.Count > 0)
                {
                    int b = 1;
                    String dateTime = null;
                    string currentWard = string.Empty;
                    string consulted = string.Empty;
                    EnableButton(true);
                    foreach (PatientTransferLineItem LineItem in RptPatientTransfer.PatientTransferLineItems.OrderBy(x => x.AdmittedOn).ThenBy(x => x.WardName).ThenBy(x => x.AuthorizedDoctor))
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.AdmittedOn, Global.Company.DateFormat);
                        irow = GridViewTransferByDate.Rows.Add();
                        GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.SNO].Value = b;
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.IN_DATE].Value = LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                            dateTime = stringLineItemDate;
                            currentWard = string.Empty;
                            consulted = string.Empty;
                        }
                        if (currentWard != LineItem.WardName)
                        {
                            GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.WARD].Value = LineItem.WardName;
                            currentWard = LineItem.WardName;
                        }
                        GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.PATIENTId].Value = LineItem.PatientNumber;
                        GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.PATIENT].Value = LineItem.PatientName;
                        GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.BED].Value = LineItem.BedName;
                        if (!LineItem.AreActive)
                        {
                            GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.OUT_DATE].Value = LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                        }
                        if (consulted == string.Empty || consulted != LineItem.AuthorizedDoctor)
                        {
                            GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.CONSULTANT].Value = LineItem.AuthorizedDoctor;
                            consulted = LineItem.AuthorizedDoctor;
                        }
                        IList<DischargeNote> dischargeNotes = new List<DischargeNote>();
                        DischargeNote dischargeNote = DischargeNoteManager.Instance.GetLatestDischargeNoteByPatientId(LineItem.patientId);

                        if (dischargeNote != null && dischargeNote.DischargeOn != null && !LineItem.AreActive)
                        {
                            GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.DESCRIPTION].Value = dischargeNote.DischargeSummary;
                        }
                        else
                        {
                            GridViewTransferByDate.Rows[irow].Cells[(int)PatientTransferReportByDateTableColumn.DESCRIPTION].Value = LineItem.Notes;
                        }
                        b++;
                    }
                }
                else
                {
                    ErrorMsgTransferReport.Text = InformationMsg;
                }
            }
            else if (RptPatientTransfer.Type == TransferType.BYWARD)
            {
                if (RptPatientTransfer.PatientTransferLineItems != null && RptPatientTransfer.PatientTransferLineItems.Count > 0)
                {
                    int a = 1;
                    EnableButton(true);
                    string wards = string.Empty;
                    string consulted = string.Empty;
                    GridViewTransferByWard.Rows.Clear();
                    foreach (PatientTransferLineItem LineItem in RptPatientTransfer.PatientTransferLineItems.OrderBy(x => x.WardName).ThenBy(x => x.AdmittedOn).ThenBy(x => x.AuthorizedDoctor))
                    {
                        irow = GridViewTransferByWard.Rows.Add();
                        GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.SNO].Value = a;
                        if (wards == string.Empty || wards != LineItem.WardName)
                        {
                            GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.WARD].Value = LineItem.WardName;
                            wards = LineItem.WardName;
                            consulted = string.Empty;
                        }
                        GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.PATIENTId].Value = LineItem.PatientNumber;
                        GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.PATIENT].Value = LineItem.PatientName;
                        GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.BED].Value = LineItem.BedName;
                        GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.IN_DATE].Value = LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                        if (!LineItem.AreActive)
                        {
                            GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.OUT_DATE].Value = LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                        }
                        if (consulted == string.Empty || consulted != LineItem.AuthorizedDoctor)
                        {
                            GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.CONSULTANT].Value = LineItem.AuthorizedDoctor;
                            consulted = LineItem.AuthorizedDoctor;
                        }
                        IList<DischargeNote> dischargeNotes = new List<DischargeNote>();
                        DischargeNote dischargeNote = DischargeNoteManager.Instance.GetLatestDischargeNoteByPatientId(LineItem.patientId);

                        if (dischargeNote != null && dischargeNote.DischargeOn != null && !LineItem.AreActive)
                        {
                            GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.DESCRIPTION].Value = dischargeNote.DischargeSummary;
                        }
                        else
                        {
                            GridViewTransferByWard.Rows[irow].Cells[(int)PatientTransferReportByWardTableColumn.DESCRIPTION].Value = LineItem.Notes;
                        }
                        a++;
                    }

                }
                else
                {
                    ErrorMsgTransferReport.Text = InformationMsg;
                }
            }
            else
            {
                if (RptPatientTransfer.PatientTransferLineItems != null && RptPatientTransfer.PatientTransferLineItems.Count > 0)
                {
                    GridViewTransferByConsultant.Rows.Clear();
                    int i = 1;
                    EnableButton(true);
                    string consulted = string.Empty;
                    string currentWard = string.Empty;
                    foreach (PatientTransferLineItem LineItem in RptPatientTransfer.PatientTransferLineItems.OrderBy(x => x.AuthorizedDoctor).ThenBy(x => x.WardName).ThenBy(x => x.AdmittedOn))
                    {
                        irow = GridViewTransferByConsultant.Rows.Add();
                        GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.SNO].Value = i;
                        if (consulted == string.Empty || consulted != LineItem.AuthorizedDoctor)
                        {
                            GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.CONSULTANT].Value = LineItem.AuthorizedDoctor;
                            consulted = LineItem.AuthorizedDoctor;
                            currentWard = string.Empty;
                        }
                        if (currentWard != LineItem.WardName)
                        {
                            GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.WARD].Value = LineItem.WardName;
                            currentWard = LineItem.WardName;
                        }
                        GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.PATIENTId].Value = LineItem.PatientNumber;
                        GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.PATIENT].Value = LineItem.PatientName;
                        GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.BED].Value = LineItem.BedName;
                        GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.IN_DATE].Value = LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                        if (!LineItem.AreActive)
                        {
                            GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.OUT_DATE].Value = LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                        }
                        IList<DischargeNote> dischargeNotes = new List<DischargeNote>();
                        DischargeNote dischargeNote = DischargeNoteManager.Instance.GetLatestDischargeNoteByPatientId(LineItem.patientId);

                        if (dischargeNote != null && dischargeNote.DischargeOn != null && !LineItem.AreActive)
                        {
                            GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.DESCRIPTION].Value = dischargeNote.DischargeSummary;
                        }
                        else
                        {
                            GridViewTransferByConsultant.Rows[irow].Cells[(int)PatientTransferReportByConsultantTableColumn.DESCRIPTION].Value = LineItem.Notes;
                        }
                        i++;
                    }
                }
                else
                {
                    ErrorMsgTransferReport.Text = InformationMsg;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadComboBox();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientTransferSavePrint PatientTransferSavePrint = new PatientTransferSavePrint();
            PatientTransferSavePrint.ExportOrPrintToFile(RptPatientTransfer, "Transfer Patient", "pdf", false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientTransferSavePrint PatientTransferSavePrint = new PatientTransferSavePrint();
            PatientTransferSavePrint.ExportOrPrintToFile(RptPatientTransfer, "Transfer Patient", "pdf", true);
            Cursor.Current = Cursors.Default;
        }
    }
}
