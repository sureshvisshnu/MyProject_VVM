using fa;
using fa.api.Accounting;
using fa.api.Hms;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.report.Inventory;
using fa.reports.Inventory;
using fa.views;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using fa.views.utils.Report.Inventory;
using Fa.api.Hms;
using Fa.reports.Inventory;
using Fa.reports.sales;
using Fa.views.utils.Report.Hms;
using Fa.views.utils.Report.Inventory;
using FADataAccessLibrary.report.Hms;
using FADataAccessLibrary.report.Inventory;
using FADataAccessLibrary.report.sales;
using Microsoft.Office.Interop.Word;
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
using VisioForge.Libs.MediaFoundation.OPM;
using Global = fa.Global;

namespace Fa.reports.Hms
{
    enum PatientVisitCountingByDateTableColumn
    {
        SNO, DATE, MALE_ADULT, FEMALE_ADULT, MALE_CHILD, FEMALE_CHILD, OTHERS, TOTAL, DEPARTMENT
    }
    enum PatientVisitCountingByDeptTableColumn
    {
        SNO, DATE, MALE_ADULT, FEMALE_ADULT, MALE_CHILD, FEMALE_CHILD, OTHERS, TOTAL, DEPARTMENT
    }
    enum PatientVisitCountingByDiagnTableColumn
    {
        SNO, DATE, MALE_ADULT, FEMALE_ADULT, MALE_CHILD, FEMALE_CHILD, OTHERS, TOTAL, DEPARTMENT
    }
    public partial class FormPatientVisitCountingReports : Form
    {
        public static string InformationMsg = "No Information Found..!";
        public static string SelectferenceErrorMsg = "Please select {0}";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string CheckValidDateErrorMsg = "From date is greater than Todays date";

        RptPatientVisitCounting RptPatientVisitCounting = null;

        public FormPatientVisitCountingReports()
        {
            InitializeComponent();
        }
        private void FormPatientVisitCountingReports_Load(object sender, EventArgs e)
        {
            LoadComboBox();
            ResetForm();

        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            GraphLable.Visible = Enable;
            GraphResetBtn.Visible = Enable;
        }
        private void ResetForm()
        {
            GridViewNewPatient.Rows.Clear();
            GridViewRepeatPatient.Rows.Clear();
            ErrorMsgPatientVisitCountingReport.Text = "";
            EnableButton(false);
            CheckedTreeComboBoxDept.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDept.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            CheckedTreeComboBoxDiag.SelectedNode = null;
            foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDiag.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            PatientVisitCountingFromDate.Format = Global.Company.DateFormat;
            PatientVisitCountingFromDate.Date = Global.getTransactionDate().AddDays(-30);
            PatientVisitCountingToDate.Format = Global.Company.DateFormat;
            PatientVisitCountingToDate.Date = Global.getTransactionDate();
            ComboBoxTypeSelection.SelectedIndex = 0;
            ComboBoxPatientTypeSelection.SelectedIndex = 0;
            CheckedTreeComboBoxDept.Visible = false;
            CheckedTreeComboBoxDiag.Visible = false;
            GridViewNewPatient.Visible = true;
            GridViewRepeatPatient.Visible = false;
            ToolStripLabelPatientVistDept.Visible = false;
            ToolStripLabelPatientVistDiagn.Visible = false;
            PatientVisittoolStripSeparator1.Visible = true;
            PatientVisittoolStripSeparator2.Visible = false;
            PatientVisittoolStripSeparator3.Visible = false;
            double[] categories = { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
            string[] categoryLabels = categories.Select(c => c.ToString()).ToArray();
            PatientVisitCountGraph.Plot.Clear();
            PatientVisitCountGraph.Plot.XTicks(categories, categoryLabels);
            PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
            PatientVisitCountGraph.Plot.Title("");
            PatientVisitCountGraph.Plot.YLabel("");
            PatientVisitCountGraph.Plot.XLabel("");
            PatientVisitCountGraph.Refresh();
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
            ComboUtils.InitializeAllDepartmentCombo(CheckedTreeComboBoxDept, Global.Company.CompanyId);
            ComboUtils.InitializeAllDiagnosisCombo(CheckedTreeComboBoxDiag, Global.Company.CompanyId);
        }

        private void ComboBoxTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxTypeSelection.SelectedIndex == 0)
            {
                CheckedTreeComboBoxDept.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDept.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxDiag.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDiag.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxDept.Visible = false;
                CheckedTreeComboBoxDiag.Visible = false;
                GridViewNewPatient.Visible = true;
                GridViewRepeatPatient.Visible = false;
                ToolStripLabelPatientVistDept.Visible = false;
                ToolStripLabelPatientVistDiagn.Visible = false;
                PatientVisittoolStripSeparator1.Visible = false;
                PatientVisittoolStripSeparator2.Visible = true;
                PatientVisittoolStripSeparator3.Visible = false;
                GridViewNewPatient.Rows.Clear();
                EnableButton(false);
                this.Text = "Patient Visit Counting Report";
                ErrorMsgPatientVisitCountingReport.Text = "";
                double[] categories = { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
                string[] categoryLabels = categories.Select(c => c.ToString()).ToArray();
                PatientVisitCountGraph.Plot.Clear();
                PatientVisitCountGraph.Plot.XTicks(categories, categoryLabels);
                PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
                PatientVisitCountGraph.Plot.Title("");
                PatientVisitCountGraph.Plot.YLabel("");
                PatientVisitCountGraph.Plot.XLabel("");
                PatientVisitCountGraph.Refresh();
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                CheckedTreeComboBoxDiag.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDiag.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxDept.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDept.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxDept.Visible = true;
                CheckedTreeComboBoxDiag.Visible = false;
                GridViewNewPatient.Visible = true;
                GridViewRepeatPatient.Visible = false;
                ToolStripLabelPatientVistDept.Visible = true;
                ToolStripLabelPatientVistDiagn.Visible = false;
                PatientVisittoolStripSeparator1.Visible = true;
                PatientVisittoolStripSeparator2.Visible = true;
                PatientVisittoolStripSeparator3.Visible = false;
                GridViewNewPatient.Rows.Clear();
                EnableButton(false);
                this.Text = "Patient Visit Counting Report";
                ErrorMsgPatientVisitCountingReport.Text = "";
                double[] categories = { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
                string[] categoryLabels = categories.Select(c => c.ToString()).ToArray();
                PatientVisitCountGraph.Plot.Clear();
                PatientVisitCountGraph.Plot.XTicks(categories, categoryLabels);
                PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
                PatientVisitCountGraph.Plot.Title("");
                PatientVisitCountGraph.Plot.YLabel("");
                PatientVisitCountGraph.Plot.XLabel("");
                PatientVisitCountGraph.Refresh();
            }
            else
            {
                CheckedTreeComboBoxDept.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDept.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxDiag.SelectedNode = null;
                foreach (ComboTreeNode ComboTreeNode in CheckedTreeComboBoxDiag.Nodes)
                {
                    ComboTreeNode.Checked = false;
                }
                CheckedTreeComboBoxDept.Visible = false;
                CheckedTreeComboBoxDiag.Visible = true;
                GridViewNewPatient.Visible = true;
                GridViewRepeatPatient.Visible = false;
                ToolStripLabelPatientVistDept.Visible = false;
                ToolStripLabelPatientVistDiagn.Visible = true;
                PatientVisittoolStripSeparator1.Visible = true;
                PatientVisittoolStripSeparator2.Visible = false;
                PatientVisittoolStripSeparator3.Visible = true;
                GridViewNewPatient.Rows.Clear();
                EnableButton(false);
                this.Text = "Patient Visit Counting Report";
                ErrorMsgPatientVisitCountingReport.Text = "";
                double[] categories = { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
                string[] categoryLabels = categories.Select(c => c.ToString()).ToArray();
                PatientVisitCountGraph.Plot.Clear();
                PatientVisitCountGraph.Plot.XTicks(categories, categoryLabels);
                PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
                PatientVisitCountGraph.Plot.Title("");
                PatientVisitCountGraph.Plot.YLabel("");
                PatientVisitCountGraph.Plot.XLabel("");
                PatientVisitCountGraph.Refresh();
            }
        }
        private bool FormValidate()
        {
            ErrorMsgPatientVisitCountingReport.Text = "";
            if (ComboBoxTypeSelection.SelectedIndex < 0)
            {
                ErrorMsgPatientVisitCountingReport.Text = "Please select type...";
                ComboBoxTypeSelection.Select();
                return false;
            }
            if (ComboBoxPatientTypeSelection.SelectedIndex < 0)
            {
                ErrorMsgPatientVisitCountingReport.Text = "Please select patient type...";
                ComboBoxPatientTypeSelection.Select();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 1 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxDept).Count < 1)
            {
                ErrorMsgPatientVisitCountingReport.Text = "Please select Departmen...t";
                CheckedTreeComboBoxDept.Focus();
                return false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 2 && CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxDiag).Count < 1)
            {
                ErrorMsgPatientVisitCountingReport.Text = "Please select Diagnosis...";
                CheckedTreeComboBoxDept.Focus();
                return false;
            }
            if (PatientVisitCountingFromDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientVisitCountingFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgPatientVisitCountingReport.Text = EnterValidDateErrorMsg;
                PatientVisitCountingFromDate.Focus();
                return false;
            }
            if (PatientVisitCountingToDate.Date == null || !DateUtils.ValidDate(((DateTime)PatientVisitCountingToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgPatientVisitCountingReport.Text = EnterValidDateErrorMsg;
                PatientVisitCountingToDate.Focus();
                return false;
            }
            if (PatientVisitCountingToDate.Date < PatientVisitCountingFromDate.Date)
            {
                ErrorMsgPatientVisitCountingReport.Text = "Please enter valid Fromdate...";
                PatientVisitCountingFromDate.Focus();
                return false;
            }
            return true;
        }
        private void DisplayCheckedAccount()
        {
            string CheckedNodes = string.Empty;
            if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                if (CheckedTreeComboBoxDept.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in CheckedTreeComboBoxDept.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Department"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = "Patient Visit Counting Report " + " @ " + CheckedNodes;
                }
                else
                {
                    this.Text = "Patient Visit Counting Report";
                }
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 2)
            {
                if (CheckedTreeComboBoxDiag.CheckedNodes.Count > 0)
                {
                    foreach (ComboTreeNode node in CheckedTreeComboBoxDiag.CheckedNodes)
                    {
                        if (node.Name == "All") { CheckedNodes = "All Diagnosis"; break; }
                        CheckedNodes += ((string.IsNullOrEmpty(CheckedNodes) ? "" : ", ") + node.Text);
                    }
                    this.Text = "Patient Visit Counting Report " + " @ " + CheckedNodes;
                }
                else
                {
                    this.Text = "Patient Visit Counting Report";
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

        private void CheckedTreeComboBoxDept_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void CheckedTreeComboBoxDiag_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedAccount();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void LoadPatientVisitCountReport()
        {
            RptPatientVisitCounting = new RptPatientVisitCounting();
            RptPatientVisitCounting.FromDate = (DateTime)PatientVisitCountingFromDate.Date;
            RptPatientVisitCounting.ToDate = (DateTime)PatientVisitCountingToDate.Date;
            RptPatientVisitCounting.Company = Global.Company;
            if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                RptPatientVisitCounting.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboBoxDept);
            }
            if (ComboBoxTypeSelection.SelectedIndex == 2)
            {
                RptPatientVisitCounting.ReportHeader = "For" + " @ " + SelectedNodesText(CheckedTreeComboBoxDiag);
            }
            RptPatientVisitCounting.Type = ComboBoxTypeSelection.SelectedIndex == 0 ? PatientVisitCountingType.BYDATE : ComboBoxTypeSelection.SelectedIndex == 1 ? PatientVisitCountingType.BYDEPARTMENT : PatientVisitCountingType.BYDIAGONSIS;
            RptPatientVisitCounting.PType = ComboBoxPatientTypeSelection.SelectedIndex == 0 ? PatientType.NEWPATIENT : PatientType.REPEATEDPATIENT;
            if (ComboBoxTypeSelection.SelectedIndex == 1)
            {
                RptPatientVisitCounting.DeptIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxDept).ToArray();
                RptPatientVisitCounting.Dept = this.Text;
                RptPatientVisitCounting.IsAllDept = SelectedNodesText(CheckedTreeComboBoxDept) == "All Location" ? true : false;
            }
            if (ComboBoxTypeSelection.SelectedIndex == 2)
            {
                RptPatientVisitCounting.DiagIds = CheckedTreeUtils.SelectedNodes(CheckedTreeComboBoxDiag).ToArray();
                RptPatientVisitCounting.Diag = this.Text;
                RptPatientVisitCounting.IsAllDiag = SelectedNodesText(CheckedTreeComboBoxDiag) == "All Location" ? true : false;
            }
            RptPatientVisitCounting.GenerateReport();

            int irow = -1;
            if (RptPatientVisitCounting.Type == PatientVisitCountingType.BYDATE)
            {
                GridViewNewPatient.Rows.Clear();
                if (RptPatientVisitCounting.PatientVisitCountingLineItems != null && RptPatientVisitCounting.PatientVisitCountingLineItems.Count > 0)
                {
                    int Sno = 1;
                    double MaleAdultTotal = 0, FemaleAdultTotal = 0, MaleChildTotal = 0, FemaleChildTotal = 0, OthersTotal = 0;
                    String previousDateTime = null;
                    EnableButton(true);

                    foreach (PatientVisitCountingLineItem LineItem in RptPatientVisitCounting.PatientVisitCountingLineItems.OrderBy(x => x.DateOfVisit))
                    {
                        String stringLineItemDate = DateUtils.FormatDate(LineItem.DateOfVisit, Global.Company.DateFormat);

                        if (previousDateTime == null || previousDateTime != stringLineItemDate)
                        {
                            irow = GridViewNewPatient.Rows.Add();
                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.SNO].Value = Sno;
                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.DATE].Value = LineItem.DateOfVisit.ToString(Global.Company.DateFormat);
                            previousDateTime = stringLineItemDate;
                            Sno++;
                        }

                        GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT].Value =
                            (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT].Value ?? 0) + LineItem.MaleAdult);
                        MaleAdultTotal += LineItem.MaleAdult;

                        GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT].Value =
                            (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT].Value ?? 0) + LineItem.FemaleAdult);
                        FemaleAdultTotal += LineItem.FemaleAdult;

                        GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD].Value =
                            (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD].Value ?? 0) + LineItem.MaleChild);
                        MaleChildTotal += LineItem.MaleChild;

                        GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD].Value =
                            (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD].Value ?? 0) + LineItem.FemaleChild);
                        FemaleChildTotal += LineItem.FemaleChild;

                        GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.OTHERS].Value =
                            (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.OTHERS].Value ?? 0) + LineItem.Others);
                        OthersTotal += LineItem.Others;

                        int Rowtotal = Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.MALE_ADULT].Value) +
                                       Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.FEMALE_ADULT].Value) +
                                       Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.MALE_CHILD].Value) +
                                       Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.FEMALE_CHILD].Value) +
                                       Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.OTHERS].Value);

                        GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDateTableColumn.TOTAL].Value = Rowtotal;
                    }
                    double GrandTotal = MaleAdultTotal + FemaleAdultTotal + MaleChildTotal + FemaleChildTotal + OthersTotal;

                    int lastRowIndex = GridViewNewPatient.Rows.Add();
                    GridViewNewPatient.Rows[lastRowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewNewPatient.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewNewPatient.Rows[lastRowIndex].Cells[(int)PatientVisitCountingByDateTableColumn.OTHERS].Value = "Grand Total";
                    GridViewNewPatient.Rows[lastRowIndex].Cells[(int)PatientVisitCountingByDateTableColumn.TOTAL].Value = GrandTotal;

                    string[] categories = { "Male Adult", "Female Adult", "Male Child", "Female Child", "Others" };
                    double[] values = { MaleAdultTotal, FemaleAdultTotal, MaleChildTotal, FemaleChildTotal, OthersTotal };

                    PatientVisitCountGraph.Plot.Clear();
                    var bar = PatientVisitCountGraph.Plot.AddBar(values);
                    bar.Label = "Visit Totals";
                    bar.FillColor = System.Drawing.ColorTranslator.FromHtml("#df790f");

                    PatientVisitCountGraph.Plot.XTicks(new double[] { 0, 1, 2, 3, 4 }, categories);
                    PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
                    PatientVisitCountGraph.Plot.Title("Patient Visit Counting By Date");
                    PatientVisitCountGraph.Plot.YLabel("Number of Visits");
                    PatientVisitCountGraph.Plot.XLabel("Categories");

                    PatientVisitCountGraph.Refresh();
                }
                else
                {
                    ErrorMsgPatientVisitCountingReport.Text = "No Information Found...";
                }
            }
            else if (RptPatientVisitCounting.Type == PatientVisitCountingType.BYDEPARTMENT)
            {
                GridViewNewPatient.Rows.Clear();
                if (RptPatientVisitCounting.PatientVisitCountingLineItems != null && RptPatientVisitCounting.PatientVisitCountingLineItems.Count > 0)
                {
                    int Sno = 1;
                    double MaleAdultTotal = 0, FemaleAdultTotal = 0, MaleChildTotal = 0, FemaleChildTotal = 0, OthersTotal = 0;
                    String previousDateTime = null;
                    long DeptId = 0;
                    EnableButton(true);

                    foreach (var group in RptPatientVisitCounting.PatientVisitCountingLineItems.GroupBy(x => x.DeptId))
                    {
                        foreach (PatientVisitCountingLineItem LineItem in group)
                        {
                            if (DeptId != LineItem.DeptId)
                            {
                                irow = GridViewNewPatient.Rows.Add();
                                Department Department = DepartmentManager.GetDepartmentInfoByIdForReport(LineItem.DeptId);
                                if (Department != null)
                                {
                                    GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.DEPARTMENT].Value = "DepartMent Name : " + Department.Name;
                                    DeptId = LineItem.DeptId;
                                }
                            }

                            String stringLineItemDate = DateUtils.FormatDate(LineItem.DateOfVisit, Global.Company.DateFormat);

                            if (previousDateTime == null || previousDateTime != stringLineItemDate)
                            {
                                irow = GridViewNewPatient.Rows.Add();
                                GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.SNO].Value = Sno;
                                GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.DATE].Value = LineItem.DateOfVisit.ToString(Global.Company.DateFormat);
                                previousDateTime = stringLineItemDate;
                                Sno++;
                            }

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT].Value ?? 0) + LineItem.MaleAdult);
                            MaleAdultTotal += LineItem.MaleAdult;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT].Value ?? 0) + LineItem.FemaleAdult);
                            FemaleAdultTotal += LineItem.FemaleAdult;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD].Value ?? 0) + LineItem.MaleChild);
                            MaleChildTotal += LineItem.MaleChild;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD].Value ?? 0) + LineItem.FemaleChild);
                            FemaleChildTotal += LineItem.FemaleChild;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.OTHERS].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.OTHERS].Value ?? 0) + LineItem.Others);
                            OthersTotal += LineItem.Others;

                            int Rowtotal = Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.MALE_CHILD].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.FEMALE_CHILD].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.OTHERS].Value);

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDeptTableColumn.TOTAL].Value = Rowtotal;
                        }
                    }

                    double GrandTotal = MaleAdultTotal + FemaleAdultTotal + MaleChildTotal + FemaleChildTotal + OthersTotal;

                    int lastRowIndex = GridViewNewPatient.Rows.Add();
                    GridViewNewPatient.Rows[lastRowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewNewPatient.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewNewPatient.Rows[lastRowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.OTHERS].Value = "Grand Total";
                    GridViewNewPatient.Rows[lastRowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.TOTAL].Value = GrandTotal;

                    string[] categories = { "Male Adult", "Female Adult", "Male Child", "Female Child", "Others" };
                    double[] values = { MaleAdultTotal, FemaleAdultTotal, MaleChildTotal, FemaleChildTotal, OthersTotal };

                    PatientVisitCountGraph.Plot.Clear();
                    var bar = PatientVisitCountGraph.Plot.AddBar(values);
                    bar.Label = "Visit Totals";
                    bar.FillColor = System.Drawing.ColorTranslator.FromHtml("#df790f");

                    PatientVisitCountGraph.Plot.XTicks(new double[] { 0, 1, 2, 3, 4 }, categories);
                    PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
                    PatientVisitCountGraph.Plot.Title("Patient Visit Counting By Department");
                    PatientVisitCountGraph.Plot.YLabel("Number of Visits");
                    PatientVisitCountGraph.Plot.XLabel("Categories");

                    PatientVisitCountGraph.Refresh();
                }
                else
                {
                    ErrorMsgPatientVisitCountingReport.Text = "No Information Found...";
                }
            }
            else
            {
                GridViewNewPatient.Rows.Clear();
                if (RptPatientVisitCounting.PatientVisitCountingLineItemForDiagnosiss != null && RptPatientVisitCounting.PatientVisitCountingLineItemForDiagnosiss.Count > 0)
                {
                    int Sno = 1;
                    double MaleAdultTotal = 0, FemaleAdultTotal = 0, MaleChildTotal = 0, FemaleChildTotal = 0, OthersTotal = 0;
                    String previousDateTime = null;
                    long DiagId = 0;
                    EnableButton(true);

                    foreach (var group in RptPatientVisitCounting.PatientVisitCountingLineItemForDiagnosiss.GroupBy(x => x.DiagId))
                    {
                        foreach (PatientVisitCountingLineItemForDiagnosis LineItem in group)
                        {
                            if (DiagId != LineItem.DiagId)
                            {
                                irow = GridViewNewPatient.Rows.Add();
                                Symptom symptom = SymptomsManager.Instance.GetSymptomsById(LineItem.DiagId);
                                if (symptom != null)
                                {
                                    GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.DEPARTMENT].Value = "Diagnosis Name : " + symptom.Name;
                                    DiagId = LineItem.DiagId;
                                }
                            }

                            String stringLineItemDate = DateUtils.FormatDate(LineItem.DateOfVisit, Global.Company.DateFormat);

                            if (previousDateTime == null || previousDateTime != stringLineItemDate)
                            {
                                irow = GridViewNewPatient.Rows.Add();
                                GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.SNO].Value = Sno;
                                GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.DATE].Value = LineItem.DateOfVisit.ToString(Global.Company.DateFormat);
                                previousDateTime = stringLineItemDate;
                                Sno++;
                            }

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT].Value ?? 0) + LineItem.MaleAdult);
                            MaleAdultTotal += LineItem.MaleAdult;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT].Value ?? 0) + LineItem.FemaleAdult);
                            FemaleAdultTotal += LineItem.FemaleAdult;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD].Value ?? 0) + LineItem.MaleChild);
                            MaleChildTotal += LineItem.MaleChild;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD].Value ?? 0) + LineItem.FemaleChild);
                            FemaleChildTotal += LineItem.FemaleChild;

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.OTHERS].Value =
                                (Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.OTHERS].Value ?? 0) + LineItem.Others);
                            OthersTotal += LineItem.Others;

                            int Rowtotal = Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.MALE_ADULT].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_ADULT].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.MALE_CHILD].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.FEMALE_CHILD].Value) +
                                           Convert.ToInt32(GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.OTHERS].Value);

                            GridViewNewPatient.Rows[irow].Cells[(int)PatientVisitCountingByDiagnTableColumn.TOTAL].Value = Rowtotal;
                        }
                    }

                    double GrandTotal = MaleAdultTotal + FemaleAdultTotal + MaleChildTotal + FemaleChildTotal + OthersTotal;

                    int lastRowIndex = GridViewNewPatient.Rows.Add();
                    GridViewNewPatient.Rows[lastRowIndex].DefaultCellStyle.BackColor = SystemColors.Control;
                    GridViewNewPatient.Rows[lastRowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Control;
                    GridViewNewPatient.Rows[lastRowIndex].Cells[(int)PatientVisitCountingByDiagnTableColumn.OTHERS].Value = "Grand Total";
                    GridViewNewPatient.Rows[lastRowIndex].Cells[(int)PatientVisitCountingByDiagnTableColumn.TOTAL].Value = GrandTotal;

                    string[] categories = { "Male Adult", "Female Adult", "Male Child", "Female Child", "Others" };
                    double[] values = { MaleAdultTotal, FemaleAdultTotal, MaleChildTotal, FemaleChildTotal, OthersTotal };

                    PatientVisitCountGraph.Plot.Clear();
                    var bar = PatientVisitCountGraph.Plot.AddBar(values);
                    bar.Label = "Visit Totals";
                    bar.FillColor = System.Drawing.ColorTranslator.FromHtml("#df790f");

                    PatientVisitCountGraph.Plot.XTicks(new double[] { 0, 1, 2, 3, 4 }, categories);
                    PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
                    PatientVisitCountGraph.Plot.Title("Patient Visit Counting By Diagnosis");
                    PatientVisitCountGraph.Plot.YLabel("Number of Visits");
                    PatientVisitCountGraph.Plot.XLabel("Categories");

                    PatientVisitCountGraph.Refresh();
                }
                else
                {
                    ErrorMsgPatientVisitCountingReport.Text = "No Information Found...";
                }
            }
        }

        private void BtnSearchPatientVisitCounting_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                GridViewNewPatient.Rows.Clear();
                double[] categories = { -4, -3, -2, -1, 0, 1, 2, 3, 4 };
                string[] categoryLabels = categories.Select(c => c.ToString()).ToArray();
                PatientVisitCountGraph.Plot.Clear();
                PatientVisitCountGraph.Plot.XTicks(categories, categoryLabels);
                PatientVisitCountGraph.Plot.SetAxisLimits(yMin: 0);
                PatientVisitCountGraph.Plot.Title("");
                PatientVisitCountGraph.Plot.YLabel("");
                PatientVisitCountGraph.Plot.XLabel("");
                PatientVisitCountGraph.Refresh();
                EnableButton(false);
                if (FormValidate())
                {
                    LoadPatientVisitCountReport();
                }
            }
            catch (Exception ex)
            {
                ErrorMsgPatientVisitCountingReport.Text = "Error fetching Stock (Error:" + ex.InnerException.Message + ")";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            GridViewNewPatient.DefaultCellStyle.SelectionForeColor = GridViewNewPatient.DefaultCellStyle.ForeColor;
            GridViewNewPatient.DefaultCellStyle.SelectionBackColor = GridViewNewPatient.DefaultCellStyle.BackColor;
        }

        private void GridViewNewPatient_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (ComboBoxTypeSelection.SelectedIndex == 1 && CheckedTreeComboBoxDept.Visible == true)
            {
                if (GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.TOTAL].Value == null && e.RowIndex > -1)
                {
                    System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                        0, e.RowBounds.Top,
                        this.GridViewNewPatient.Columns.GetColumnsWidth(
                            DataGridViewElementStates.Visible) -
                        this.GridViewNewPatient.HorizontalScrollingOffset,
                        e.RowBounds.Height);

                    System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                    string rr = GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.DEPARTMENT].Value != null ? GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.DEPARTMENT].Value.ToString() : string.Empty;
                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;

                    e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
                }
            }
            else if (ComboBoxTypeSelection.SelectedIndex == 2 && CheckedTreeComboBoxDiag.Visible == true)
            {
                if (GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDiagnTableColumn.TOTAL].Value == null && e.RowIndex > -1)
                {
                    System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                        0, e.RowBounds.Top,
                        this.GridViewNewPatient.Columns.GetColumnsWidth(
                            DataGridViewElementStates.Visible) -
                        this.GridViewNewPatient.HorizontalScrollingOffset,
                        e.RowBounds.Height);

                    System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                    string rr = GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDiagnTableColumn.DEPARTMENT].Value != null ? GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDiagnTableColumn.DEPARTMENT].Value.ToString() : string.Empty;
                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;

                    e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds, stringFormat);
                }
            }
        }

        private void GridViewNewPatient_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex > -1 && GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.SNO].Value == null
            && GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.TOTAL].Value != null)
            {
                if (e.ColumnIndex == (int)PatientVisitCountingByDeptTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                if (e.ColumnIndex == (int)PatientVisitCountingByDeptTableColumn.MALE_ADULT || e.ColumnIndex == (int)PatientVisitCountingByDeptTableColumn.FEMALE_ADULT || e.ColumnIndex == (int)PatientVisitCountingByDeptTableColumn.DATE || e.ColumnIndex == (int)PatientVisitCountingByDeptTableColumn.MALE_CHILD)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
            else if (e.RowIndex > -1 && GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.TOTAL].Value == null &&
                    GridViewNewPatient.Rows[e.RowIndex].Cells[(int)PatientVisitCountingByDeptTableColumn.MALE_ADULT].Value == null)
            {
                if (e.ColumnIndex == (int)PatientVisitCountingByDeptTableColumn.SNO)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else if (e.ColumnIndex == (int)PatientVisitCountingByDeptTableColumn.TOTAL)
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientVisitCountReportPrintSave PatientVisitCountReportPrintSave = new PatientVisitCountReportPrintSave();
            PatientVisitCountReportPrintSave.ExportOrPrintToFile(RptPatientVisitCounting, "Patient Visit Counting Report", "pdf", false);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientVisitCountReportPrintSave PatientVisitCountReportPrintSave = new PatientVisitCountReportPrintSave();
            PatientVisitCountReportPrintSave.ExportOrPrintToFile(RptPatientVisitCounting, "Patient Visit Counting Report", "pdf", true);
            Cursor.Current = Cursors.Default;
        }

        private void GraphReset_Click(object sender, EventArgs e)
        {
            PatientVisitCountGraph.Plot.AxisAuto();
            PatientVisitCountGraph.Refresh();
        }
    }
}
