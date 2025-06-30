using fa;
using fa.libraries.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.controls.ComboTreeView;
using Fa.views.utils.TextSearch;
using FADataAccessLibrary.report.Hms;
using fa.api.utils;
using fa.report.Ip;
using fa.views.hms.ip;
using fa.reports.Hms;
using fa.views.utils.Report.Hms;
using fa.views.controls;
using static fa.views.utils.Common.DataGridViewColoumnAdjustment;

namespace Fa.reports.Hms
{
    public partial class FormCareTakerAssignReport : Form
    {
        enum CareTakerAssignTableColumn
        {
            FROM, TO, DEPT, PDOCTOR, SDOCTOR, PNURSE, SNURSE, NOTE, ADOCTOR, AID, ROWHEADING
        }
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string EnterValidTypeErrorMsg = "Please select {0}";

        RptCareTakerAssign ReporttCareTakerAssign = new RptCareTakerAssign(); // or any other appropriate initialization
        public int ReportIndex = 0;
        public int ReportCount = 0;
        public int GetComboIndex = 0;
        public int GetCurrentIndex = -1;

        private List<string> originalItems;
        private bool allowSelectedIndexChanged = false;
        public List<string> listNew = new List<string>();

        private ComboBoxTextSearchHelper TextSearchHelper;
        public FormCareTakerAssignReport()
        {
            InitializeComponent();
            //SetupComboBoxAutoComplete();
            //TextSearchHelper = new ComboBoxTextSearchHelper(ComboBoxReportType);
        }
        private void SetupComboBoxAutoComplete()
        {
            // Set up auto-complete properties
            ComboBoxReportType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ComboBoxReportType.AutoCompleteSource = AutoCompleteSource.CustomSource;

            // Populate auto-complete source with items from the ComboBox
            AutoCompleteStringCollection autoCompleteSource = new AutoCompleteStringCollection();
            foreach (var item in ComboBoxReportType.Items)
            {
                autoCompleteSource.Add(item.ToString());
            }

            // Set the custom auto-complete source
            ComboBoxReportType.AutoCompleteCustomSource = autoCompleteSource;
        }
        private void FormCareTakerAssignReport_Load(object sender, EventArgs e)
        {
            StoreOriginalColumnWidths();
            StoreOriginalItems();
            ResetForm();
            LoadComboBox();
        }
        private void StoreOriginalColumnWidths()
        {
            originalColumnWidths.Clear(); // Clear previous stored widths
            foreach (DataGridViewColumn column in GridviewCareTakerReport.Columns)
            {
                originalColumnWidths[column.Index] = column.Width;
            }
        }
        private void StoreOriginalItems()
        {
            originalItems = new List<string>();
            foreach (string item in ComboBoxReportType.Items)
            {
                originalItems.Add(item);
            }
        }
        private void ResetForm()
        {
            CareTakerReportErrorMsg.Text = "";
            this.Text = "Care Taker Assign Report";
            GridviewCareTakerReport.Rows.Clear();
            foreach (ComboTreeNode ComboTreeNode in ComboBoxConsultant.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxNurse.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxTechnician.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            foreach (ComboTreeNode ComboTreeNode in ComboBoxDepartment.Nodes)
            {
                ComboTreeNode.Checked = false;
            }
            CTReportFromDate.Format = Global.Company.DateFormat;
            CTReportFromDate.Date = Global.getTransactionDate().AddDays(-30);
            CTReportToDate.Format = Global.Company.DateFormat;
            CTReportToDate.Date = Global.getTransactionDate();
            EnableButton(false);
        }
        private void EnableButton(bool Enable)
        {
            BtnPrint.Enabled = Enable;
            BtnSave.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private void LoadComboBox()
        {
            ComboUtils.InitializeEmployeeCombo(ComboBoxConsultant, Global.Company.CompanyId, "Doctor");
            ComboUtils.InitializeEmployeeCombo(ComboBoxNurse, Global.Company.CompanyId, "Nurse");
            ComboUtils.InitializeEmployeeCombo(ComboBoxTechnician, Global.Company.CompanyId, "Technician");
            ComboUtils.InitializeAllDepartmentCombo(ComboBoxDepartment, Global.Company.CompanyId);
        }

        private void ComboBoxReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!allowSelectedIndexChanged)
                    return;

                ExecuteComboBoxSelection();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private bool ValidateForm()
        {
            if (ComboBoxReportType.SelectedIndex < 0)
            {
                CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, "Type...");
                GetCurrentIndex = -1;
                ComboBoxReportType.Select();
                return false;
            }
            if (CTReportFromDate.Date == null || !DateUtils.ValidDate(((DateTime)CTReportFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                CareTakerReportErrorMsg.Text = EnterValidDateErrorMsg;
                CTReportFromDate.Focus();
                return false;
            }
            if (CTReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)CTReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                CareTakerReportErrorMsg.Text = EnterValidDateErrorMsg;
                CTReportToDate.Focus();
                return false;
            }
            if (ReportIndex == 1)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).Count < 1)
                {
                    CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxConsultant.Focus();
                    return false;
                }
            }
            if (ReportIndex == 2)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxNurse).Count < 1)
                {
                    CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxNurse.Focus();
                    return false;
                }
            }
            if (ReportIndex == 3)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxTechnician).Count < 1)
                {
                    CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxTechnician.Focus();
                    return false;
                }
            }
            if (ReportIndex == 4)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).Count < 1)
                {
                    CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    ComboBoxDepartment.Focus();
                    return false;
                }
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnReset.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);

        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboBoxReportType_TextChanged(object sender, EventArgs e)
        {

        }
        private void ToolStripBtnGo_Click(object sender, EventArgs e)
        {
            GridviewCareTakerReport.Rows.Clear();
            EnableButton(false);
            if (ValidateForm())
            {
                LoadCareTakerHistory();
            }
        }
        public void LoadCareTakerHistory()
        {
            CareTakerReportErrorMsg.Text = "";
            ReporttCareTakerAssign = new RptCareTakerAssign();
            ReporttCareTakerAssign.FromDate = (DateTime)(CTReportFromDate.Date ?? DateTime.MinValue);
            ReporttCareTakerAssign.ToDate = (DateTime)(CTReportToDate.Date ?? DateTime.MinValue);
            ReporttCareTakerAssign.Company = Global.Company;
            ReporttCareTakerAssign.FilterIndex = ReportIndex;
            ReporttCareTakerAssign.ConsultantId = CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).ToArray();
            ReporttCareTakerAssign.DepartmentId = CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).ToArray();
            ReporttCareTakerAssign.NurseId = CheckedTreeUtils.SelectedNodes(ComboBoxNurse).ToArray();
            ReporttCareTakerAssign.TechnicianId = CheckedTreeUtils.SelectedNodes(ComboBoxTechnician).ToArray();

            ReporttCareTakerAssign.GenerateReport();
            if (ReporttCareTakerAssign.LineCareTakerReport != null && ReporttCareTakerAssign.LineCareTakerReport.Count > 0)
            {
                int j = 2;
                int k = 0;
                Color[] RowColor = new Color[2];
                RowColor[0] = Color.White;
                RowColor[1] = Color.WhiteSmoke;
                EnableButton(true);
                int rowCount = 0;

                string Consultant = string.Empty;
                string DepartmentOfConsultant = string.Empty;
                string PrimaryCareTaker = string.Empty;
                string Technician = string.Empty;

                foreach (CareTakerReport LineItem in ReporttCareTakerAssign.LineCareTakerReport)
                {
                    int irow = rowCount;

                    if (ReportIndex == 1)
                    {
                        if (Consultant != LineItem.PrimaryDr)
                        {
                            irow = GridviewCareTakerReport.Rows.Add();
                            k = 0;
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.ROWHEADING].Value = "Consultant : " + LineItem.PrimaryDr;
                            j++;
                            Consultant = LineItem.PrimaryDr;
                        }
                    }
                    if (ReportIndex == 2)
                    {
                        if (PrimaryCareTaker != LineItem.PrimaryCT)
                        {
                            k = 0;
                            irow = GridviewCareTakerReport.Rows.Add();
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.ROWHEADING].Value = "Nurse : " + LineItem.PrimaryCT;

                            j++;
                            PrimaryCareTaker = LineItem.PrimaryCT;
                        }
                    }
                    if (ReportIndex == 3)
                    {
                        if (Technician != LineItem.DepartmentOfConsultant)
                        {
                            k = 0;
                            irow = GridviewCareTakerReport.Rows.Add();
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.ROWHEADING].Value = "Technician : " + LineItem.SecondaryCT;

                            j++;
                            Technician = LineItem.DepartmentOfConsultant;
                        }
                    }
                    if (ReportIndex == 4)
                    {
                        if (DepartmentOfConsultant != LineItem.DepartmentOfConsultant)
                        {
                            k = 0;
                            irow = GridviewCareTakerReport.Rows.Add();
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                            GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                            GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.ROWHEADING].Value = "Department : " + LineItem.DepartmentOfConsultant;

                            j++;
                            DepartmentOfConsultant = LineItem.DepartmentOfConsultant;
                        }
                    }
                    irow = GridviewCareTakerReport.Rows.Add();
                    if (ReportIndex == 1)
                    {
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Visible = false;
                    }
                    else
                    {
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Visible = true;

                    }
                    if (ReportIndex == 2 || ReportIndex == 3)
                    {
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Visible = false;
                    }
                    else
                    {
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Visible = true;

                    }
                    if (ReportIndex == 4)
                    {
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Visible = false;
                    }
                    else
                    {
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Visible = true;

                    }
                    k = 0;
                    GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                    GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                    GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                    j++;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.FROM].Value = LineItem.From.ToString(Global.Company.DateFormat);
                    if (LineItem.To != DateTime.MinValue)
                    {
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.TO].Value = LineItem.To.ToString(Global.Company.DateFormat);
                    }
                    else
                    {
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.TO].Value = string.Empty;
                    }
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.DEPT].Value = LineItem.DepartmentOfConsultant;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.PDOCTOR].Value = LineItem.PrimaryDr;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.SDOCTOR].Value = LineItem.SecondaryDr;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.PNURSE].Value = LineItem.PrimaryCT;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.SNURSE].Value = LineItem.SecondaryCT;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.NOTE].Value = LineItem.Notes;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.ADOCTOR].Value = LineItem.AuthorizedDr;
                    GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerAssignTableColumn.AID].Value = LineItem.AdmissionId;
                    rowCount++;
                    k++;
                }
                //AdjustColumnWidthsAfterHiding(GridviewCareTakerReport);
                EnableButton(true);
            }
            else
            {
                EnableButton(false);
                CareTakerReportErrorMsg.Text = "No Information Found..!";
            }
        }

        private void DisplayCheckedInformation()
        {

            string ConsultantNodes = GetCheckedNodes(ComboBoxConsultant, "For All Consultant");
            string DepartmentNodes = GetCheckedNodes(ComboBoxDepartment, "For All Department");
            string NurseNodes = GetCheckedNodes(ComboBoxNurse, "For All Nurse");
            string TechnicianNodes = GetCheckedNodes(ComboBoxTechnician, "For All Technician");

            string DisplayFor = "";
            string title = "Care Taker Assign Report";
            if (ReportIndex == 1)
            {
                if (ConsultantNodes != null && ConsultantNodes.ToString() != "For All Consultant")
                {
                    DisplayFor = " For Consultant ";
                }
            }
            if (ReportIndex == 2)
            {
                if (NurseNodes != null && NurseNodes.ToString() != "For All Nurse")
                {
                    DisplayFor = " For Nurse ";
                }
            }
            if (ReportIndex == 3)
            {
                if (TechnicianNodes != null && TechnicianNodes.ToString() != "For All Technician")
                {
                    DisplayFor = " For Technician ";
                }
            }
            if (ReportIndex == 4)
            {
                if (DepartmentNodes != null && DepartmentNodes.ToString() != "For All Department")
                {
                    DisplayFor = " For Department ";
                }
            }


            title += AppendToTitleIfNotEmpty(ConsultantNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(DepartmentNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(NurseNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(TechnicianNodes!, DisplayFor);

            this.Text = title;
        }

        private string AppendToTitleIfNotEmpty(string nodes, string prefix)
        {
            return string.IsNullOrEmpty(nodes) ? "" : $"{prefix} {nodes}";
        }
        private string GetCheckedNodes(ToolstripCheckedTreeComboBox comboBox, string nodeName)
        {
            if (comboBox == null || comboBox.CheckedNodes == null || comboBox.CheckedNodes.Count == 0)
            {
                return string.Empty;
            }

            bool isAllSelected = comboBox.CheckedNodes.Any(node => node.Name == "All");

            if (isAllSelected)
            {
                return nodeName;
            }

            string checkedNodes = string.Join(", ", comboBox.CheckedNodes.Select(node => node.Text));
            return checkedNodes;
        }
        private void GridviewCareTakerReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == (int)CareTakerAssignTableColumn.FROM) && e.Value != null)
            {
                DataGridViewCell cell = GridviewCareTakerReport.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.WrapMode = DataGridViewTriState.False;
            }
        }

        private void GridviewCareTakerReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerAssignTableColumn.AID].Value == null)
            {
                if (e.ColumnIndex == (int)CareTakerAssignTableColumn.FROM)
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                }
            }
        }

        private void GridviewCareTakerReport_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if ((GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerAssignTableColumn.FROM].Value == null) && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.GridviewCareTakerReport.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible) -
                this.GridviewCareTakerReport.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string? rr = GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerAssignTableColumn.ROWHEADING].Value != null ? GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerAssignTableColumn.ROWHEADING].Value?.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CareTakerAssignSavePrint CTAReportSavePrint = new CareTakerAssignSavePrint();
            CTAReportSavePrint.ExportOrPrintToFile(GridviewCareTakerReport, ReporttCareTakerAssign, "pdf", false, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CareTakerAssignSavePrint CTAReportSavePrint = new CareTakerAssignSavePrint();
            CTAReportSavePrint.ExportOrPrintToFile(GridviewCareTakerReport, ReporttCareTakerAssign, "pdf", true, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void ComboBoxDepartment_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void ComboBoxConsultant_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }
        private void ExecuteComboBoxSelection()
        {
            try
            {
                ToolStripTabIndexChanged();
                if (GetCurrentIndex != GetComboIndex)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    ResetForm();
                    SetComboIndex();
                    if (ComboBoxReportType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 0;
                        ComboBoxReportType.Text = "By Date";
                        GridviewCareTakerReport.Visible = true;
                        ComboBoxConsultant.Visible = false;
                        ComboBoxNurse.Visible = false;
                        ComboBoxTechnician.Visible = false;
                        ComboBoxDepartment.Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Visible = true;
                        ResetColumnWidths();
                        ResetColumnWidthsOriginal();
                        LabelType.Visible = false;
                    }
                    else if (ComboBoxReportType.Text.Equals("By Consultant", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 1;
                        GridviewCareTakerReport.Visible = true;
                        ComboBoxConsultant.Visible = true;
                        ComboBoxConsultant.Size = new Size(200, 25);
                        ComboBoxDepartment.Visible = false;
                        ComboBoxNurse.Visible = false;
                        ComboBoxTechnician.Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Visible = true;
                        ResetColumnWidths();
                        InitializeColumnWidths(ReportIndex);
                        LabelType.Visible = true;
                        LabelType.Text = "Consultant";

                    }
                    else if (ComboBoxReportType.Text.Equals("By Nurse", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 2;
                        GridviewCareTakerReport.Visible = true;
                        ComboBoxConsultant.Visible = false;
                        ComboBoxDepartment.Visible = false;
                        ComboBoxNurse.Size = new Size(200, 25);
                        ComboBoxNurse.Visible = true;
                        ComboBoxTechnician.Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Visible = true;
                        ResetColumnWidths();
                        InitializeColumnWidths(ReportIndex);
                        LabelType.Visible = true;
                        LabelType.Text = "Nurse";
                    }
                    else if (ComboBoxReportType.Text.Equals("By Technician", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 3;
                        GridviewCareTakerReport.Visible = true;
                        ComboBoxConsultant.Visible = false;
                        ComboBoxDepartment.Visible = false;
                        ComboBoxTechnician.Size = new Size(200, 25);
                        ComboBoxNurse.Visible = false;
                        ComboBoxTechnician.Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Visible = true;
                        ResetColumnWidths();
                        InitializeColumnWidths(ReportIndex);
                        LabelType.Visible = true;
                        LabelType.Text = "Technician";
                    }
                    else if (ComboBoxReportType.Text.Equals("By Department", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 4;
                        GridviewCareTakerReport.Visible = true;
                        ComboBoxConsultant.Visible = false;
                        ComboBoxDepartment.Visible = true;
                        ComboBoxDepartment.Size = new Size(200, 25);
                        ComboBoxNurse.Visible = false;
                        ComboBoxTechnician.Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Visible = false;
                        ResetColumnWidths();
                        InitializeColumnWidths(ReportIndex);
                        LabelType.Visible = true;
                        LabelType.Text = "Department";
                    }

                    ComboBoxReportType.DroppedDown = false;
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void ToolStripTabIndexChanged()
        {
            if (ComboBoxReportType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 0;
            }
            else if (ComboBoxReportType.Text.Equals("By Consultant", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 1;
            }
            else if (ComboBoxReportType.Text.Equals("By Nurse", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 2;
            }
            else if (ComboBoxReportType.Text.Equals("By Technician", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 3;
            }
            else if (ComboBoxReportType.Text.Equals("By Department", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 4;
            }
            else
            {
                GetComboIndex = 0;
            }
        }
        private void SetComboIndex()
        {
            if (ComboBoxReportType.Text.Equals("By Date", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 0;
            }
            else if (ComboBoxReportType.Text.Equals("By Consultant", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 1;
            }
            else if (ComboBoxReportType.Text.Equals("By Nurse", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 2;
            }
            else if (ComboBoxReportType.Text.Equals("By Technician", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 3;
            }
            else if (ComboBoxReportType.Text.Equals("By Department", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 4;
            }
            else
            {
                GetCurrentIndex = -1;
            }

        }
        private const int MaxGridViewWidth = 875;
        private const int ScrollbarWidth = 25;
        private void ResetColumnWidths()
        {
            foreach (DataGridViewColumn column in GridviewCareTakerReport.Columns)
            {
                if (column.Visible)
                {
                    column.Width = 0;
                }
            }
        }
        private void InitializeColumnWidths(int indexedColumn)
        {
            {
                if (indexedColumn == 1)
                {
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.FROM].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.TO].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Width = 135;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.SDOCTOR].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.SNURSE].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.NOTE].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.ADOCTOR].Width = 180;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.AID].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.ROWHEADING].Width = 2;

                }
                if (indexedColumn == 2 || indexedColumn == 3)
                {
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.FROM].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.TO].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Width = 135;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.SDOCTOR].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.SNURSE].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.NOTE].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.ADOCTOR].Width = 180;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.AID].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.ROWHEADING].Width = 2;

                }
                else if (indexedColumn == 4)
                {
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.FROM].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.TO].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.DEPT].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PDOCTOR].Width = 190;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.SDOCTOR].Width = 190;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.PNURSE].Width = 190;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.SNURSE].Width = 190;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.NOTE].Width = 170;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.ADOCTOR].Width = 180;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.AID].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerAssignTableColumn.ROWHEADING].Width = 2;

                }
                // Alternatively, you can set the default column width for all columns
                // GridviewCareTakerReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private Dictionary<int, int> originalColumnWidths = new Dictionary<int, int>();

        private void ResetColumnWidthsOriginal()
        {
            foreach (var kvp in originalColumnWidths)
            {
                int originalcolumnIndex = kvp.Key;
                int originalWidth = kvp.Value;
                GridviewCareTakerReport.Columns[originalcolumnIndex].Width = originalWidth;
            }
        }
        private void StoreComboItems()
        {
            ComboBoxReportType.Items.Clear();
            foreach (string item in originalItems)
            {
                ComboBoxReportType.Items.Add(item);
            }
        }
        private void ComboBoxReportType_DropDown(object sender, EventArgs e)
        {
            var toolStripComboBox = sender as ToolStripComboBox;
            if (toolStripComboBox == null || toolStripComboBox.Name != "ComboBoxReportType") // Check the name of the ToolStripComboBox
                return;

            allowSelectedIndexChanged = true;
            toolStripComboBox.Items.Clear();
            toolStripComboBox.Items.AddRange(originalItems.ToArray());
        }

        private void ComboBoxReportType_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBoxReportType.Items.Count == 0)
            {
                StoreComboItems();
            }
        }

        private void ComboBoxNurse_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void ComboBoxTechnician_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }
    }
}