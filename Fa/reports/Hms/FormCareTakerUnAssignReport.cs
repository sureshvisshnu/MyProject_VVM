using fa;
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
using fa.libraries.utils;
using fa.api.utils;
using FADataAccessLibrary.report.Hms;
using fa.reports.Hms;
using fa.views.controls;
using fa.views.utils.Report.Hms;
using NPOI.SS.UserModel;
using fa.api.catalog;

namespace Fa.reports.Hms
{
    public partial class FormCareTakerUnAssignReport : Form
    {
        enum CareTakerUnAssignTableColumn
        {
            SLNO, NAME, AGE, ADDRESS, DEPT, TASK, DATE, AID, ROWHEADING
        }
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string EnterValidTypeErrorMsg = "Please select {0}";
        public static string NoInformationErrorMsg = "No Information Found..!";
        RptCareTakerUnAssign ReportCareTakerUnAssign = new RptCareTakerUnAssign(); // or any other appropriate initialization

        public int ReportIndex = 0;
        public int ReportCount = 0;
        public int GetComboIndex = 0;
        public int GetCurrentIndex = -1;

        private List<string> originalItems;
        private bool allowSelectedIndexChanged = false;
        public List<string> listNew = new List<string>();


        public FormCareTakerUnAssignReport()
        {
            InitializeComponent();
        }

        private void FormCareTakerUnAssignReport_Load(object sender, EventArgs e)
        {
            StoreOriginalColumnWidths();
            StoreOriginalItems();
            ResetForm();
            LoadComboBox();
        }
        private void ResetForm()
        {
            CareTakerReportErrorMsg.Text = "";
            this.Text = "Care Taker UnAssign Report";
            GridviewCareTakerReport.Rows.Clear();
            foreach (ComboTreeNode ComboTreeNode in ComboBoxConsultant.Nodes)
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
            ComboUtils.InitializeDoctorCombo(ComboBoxConsultant, Global.Company.CompanyId);
            ComboUtils.InitializeAllDepartmentCombo(ComboBoxDepartment, Global.Company.CompanyId);
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
                GetCurrentIndex = -1;
                CTReportFromDate.Focus();
                return false;
            }
            if (CTReportToDate.Date == null || !DateUtils.ValidDate(((DateTime)CTReportToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                CareTakerReportErrorMsg.Text = EnterValidDateErrorMsg;
                GetCurrentIndex = -1;
                CTReportToDate.Focus();
                return false;
            }
            if (ReportIndex == 1)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).Count < 1)
                {
                    CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    GetCurrentIndex = -1;
                    ComboBoxConsultant.Focus();
                    return false;
                }
            }
            if (ReportIndex == 2)
            {
                if (CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).Count < 1)
                {
                    CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                    GetCurrentIndex = -1;
                    ComboBoxDepartment.Focus();
                    return false;
                }
            }
            return true;
        }

        public void LoadCareTakerUnAssignHistory()
        {

            CareTakerReportErrorMsg.Text = "";
            ReportCareTakerUnAssign = new RptCareTakerUnAssign();
            ReportCareTakerUnAssign.FromDate = (DateTime)(CTReportFromDate.Date ?? DateTime.MinValue);
            ReportCareTakerUnAssign.ToDate = (DateTime)(CTReportToDate.Date ?? DateTime.MinValue);
            ReportCareTakerUnAssign.Company = Global.Company;
            ReportCareTakerUnAssign.FilterIndex = ReportIndex;
            ReportCareTakerUnAssign.ConsultantId = CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).ToArray();
            ReportCareTakerUnAssign.DepartmentId = CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).ToArray();
            DataTable resultTable = ReportCareTakerUnAssign.CreateUnAssignCareTakerReport(Global.Company.CompanyId, (DateTime)(CTReportFromDate.Date ?? DateTime.MinValue), (DateTime)(CTReportToDate.Date ?? DateTime.MinValue), 1);
            ReportCareTakerUnAssign.GenerateReport();
            if (ReportCareTakerUnAssign.UnAssignStatus == true)
            {
                if (ReportCareTakerUnAssign.LineUnAssignCareTakerReport != null && ReportCareTakerUnAssign.LineUnAssignCareTakerReport.Count > 0)
                {
                    int j = 2;
                    int k = 0;
                    Color[] RowColor = new Color[2];
                    RowColor[0] = Color.White;
                    RowColor[1] = Color.WhiteSmoke;
                    EnableButton(true);
                    int rowCount = 0;
                    int irowslno = 0;

                    string Consultant = string.Empty;
                    string DepartmentOfConsultant = string.Empty;

                    foreach (UnAssignCareTakerReport LineItem in ReportCareTakerUnAssign.LineUnAssignCareTakerReport)
                    {
                        int irow = rowCount;
                        if (ReportIndex == 1)
                        {
                            if (Consultant != LineItem.Name)
                            {
                                irowslno = 0;
                                irow = GridviewCareTakerReport.Rows.Add();
                                k = 0;
                                GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.ROWHEADING].Value = "Consultant : " + LineItem.Name;
                                j++;
                                Consultant = LineItem.Name;
                            }
                        }
                        if (ReportIndex == 2)
                        {
                            if (DepartmentOfConsultant != LineItem.DepartmentOfConsultant)
                            {
                                irowslno = 0;
                                k = 0;
                                irow = GridviewCareTakerReport.Rows.Add();
                                GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                                GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                                GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                                GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.ROWHEADING].Value = "Department : " + LineItem.DepartmentOfConsultant;

                                j++;
                                DepartmentOfConsultant = LineItem.DepartmentOfConsultant;
                            }
                        }
                        irow = GridviewCareTakerReport.Rows.Add();
                        if (ReportIndex == 1)
                        {
                            GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.NAME].Visible = false;
                        }
                        else
                        {
                            GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.NAME].Visible = true;

                        }
                        if (ReportIndex == 2)
                        {
                            GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DEPT].Visible = false;
                        }
                        else
                        {
                            GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DEPT].Visible = true;

                        }
                        k = 0;
                        GridviewCareTakerReport.Rows[irow].DefaultCellStyle.BackColor = RowColor[j % 2];
                        GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionBackColor = RowColor[j % 2];
                        GridviewCareTakerReport.Rows[irow].DefaultCellStyle.SelectionForeColor = Color.Black;
                        j++;
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.SLNO].Value = irowslno + 1;

                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.NAME].Value = LineItem.Name.ToString();
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.AGE].Value = LineItem.Age;
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.ADDRESS].Value = LineItem.Address;
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.DEPT].Value = LineItem.DepartmentOfConsultant;
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.TASK].Value = LineItem.TaskStatus;
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.DATE].Value = LineItem.AssignDate;
                        GridviewCareTakerReport.Rows[irow].Cells[(int)CareTakerUnAssignTableColumn.AID].Value = LineItem.AdmissionId;
                        rowCount++;
                        irowslno++;
                        k++;
                    }
                    //AdjustColumnWidthsAfterHiding(GridviewCareTakerReport);
                    EnableButton(true);
                    BtnSave.Focus();
                }
                else
                {
                    EnableButton(false);
                    GetCurrentIndex = -1;
                    CareTakerReportErrorMsg.Text = NoInformationErrorMsg;
                }
            }
            else if (ReportCareTakerUnAssign.UnAssignStatus == false)
            {
                CareTakerReportErrorMsg.Text = NoInformationErrorMsg;
            }
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

        private void ToolStripBtnGo_Click(object sender, EventArgs e)
        {
            GridviewCareTakerReport.Rows.Clear();
            EnableButton(false);
            if (ValidateForm())
            {
                LoadCareTakerUnAssignHistory();
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridviewCareTakerReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerUnAssignTableColumn.AID].Value == null)
            {
                if (e.ColumnIndex == (int)CareTakerUnAssignTableColumn.SLNO)
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
            if ((GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerUnAssignTableColumn.SLNO].Value == null) && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
                0, e.RowBounds.Top,
                this.GridviewCareTakerReport.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible) -
                this.GridviewCareTakerReport.HorizontalScrollingOffset,
                e.RowBounds.Height);

                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string? rr = GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerUnAssignTableColumn.ROWHEADING].Value != null ? GridviewCareTakerReport.Rows[e.RowIndex].Cells[(int)CareTakerUnAssignTableColumn.ROWHEADING].Value?.ToString() : string.Empty;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }
        private Dictionary<int, int> originalColumnWidths = new Dictionary<int, int>();

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
                        ComboBoxDepartment.Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.NAME].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DEPT].Visible = true;
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
                        GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.NAME].Visible = false;
                        GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DEPT].Visible = true;
                        ResetColumnWidths();
                        InitializeColumnWidths(ReportIndex);
                        LabelType.Visible = true;
                        LabelType.Text = "Consultant";

                    }
                    else if (ComboBoxReportType.Text.Equals("By Department", StringComparison.OrdinalIgnoreCase))
                    {
                        ReportIndex = 2;
                        GridviewCareTakerReport.Visible = true;
                        ComboBoxConsultant.Visible = false;
                        ComboBoxDepartment.Visible = true;
                        ComboBoxDepartment.Size = new Size(200, 25);
                        GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.NAME].Visible = true;
                        GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DEPT].Visible = false;
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

        private void InitializeColumnWidths(int indexedColumn)
        {
            {
                if (indexedColumn == 1)
                {
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.SLNO].Width = 50;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.NAME].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.AGE].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.ADDRESS].Width = 300;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DEPT].Width = 250;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.TASK].Width = 150;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DATE].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.AID].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.ROWHEADING].Width = 2;

                }
                else if (indexedColumn == 2)
                {
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.SLNO].Width = 50;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.NAME].Width = 250;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.AGE].Width = 100;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.ADDRESS].Width = 300;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DEPT].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.TASK].Width = 150;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.DATE].Width = 200;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.AID].Width = 2;
                    GridviewCareTakerReport.Columns[(int)CareTakerUnAssignTableColumn.ROWHEADING].Width = 2;

                }
                // Alternatively, you can set the default column width for all columns
                // GridviewCareTakerReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
        private void ResetColumnWidthsOriginal()
        {
            foreach (var kvp in originalColumnWidths)
            {
                int originalcolumnIndex = kvp.Key;
                int originalWidth = kvp.Value;
                GridviewCareTakerReport.Columns[originalcolumnIndex].Width = originalWidth;
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
            else if (ComboBoxReportType.Text.Equals("By Department", StringComparison.OrdinalIgnoreCase))
            {
                GetComboIndex = 2;
            }
            else
            {
                GetComboIndex = 0;
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

        private void ComboBoxReportType_KeyDown(object sender, KeyEventArgs e)
        {
            ToolStripComboBox comboBox = sender as ToolStripComboBox;
            if (comboBox == null)
                return;

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                allowSelectedIndexChanged = true;
                ExecuteComboBoxSelection();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                ToolStripTabIndexChanged();
                allowSelectedIndexChanged = false;
            }
        }

        private void ComboBoxReportType_TextUpdate(object sender, EventArgs e)
        {
            if (this.ComboBoxReportType == null || this.ComboBoxReportType.IsDisposed)
                return;

            try
            {
                int cursorPosition = this.ComboBoxReportType.SelectionStart;

                this.ComboBoxReportType.Items.Clear();
                listNew.Clear();

                if (string.IsNullOrWhiteSpace(this.ComboBoxReportType.Text))
                    return;

                foreach (var item in originalItems)
                {
                    if (item.ToLower().Contains(this.ComboBoxReportType.Text.ToLower()))
                    {
                        listNew.Add(item);
                    }
                }

                if (listNew.Count == 0)
                {
                    this.ComboBoxReportType.SelectionStart = this.ComboBoxReportType.Text.Length;
                    return;
                }

                this.ComboBoxReportType.Items.AddRange(listNew.ToArray());
                this.ComboBoxReportType.DroppedDown = true;
                if (!string.IsNullOrEmpty(this.ComboBoxReportType.Text))
                    this.ComboBoxReportType.SelectionStart = cursorPosition;

                Cursor = Cursors.Default;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        private void ComboBoxReportType_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null)
                return;
        }

        private void FormCareTakerUnAssignReport_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle clientRect = this.ClientRectangle;

            // Check if the mouse cursor is outside the client rectangle
            if (!clientRect.Contains(this.PointToClient(Cursor.Position)))
            {
                // Calculate the new cursor position to keep it inside the client rectangle
                int newX = Math.Max(clientRect.Left, Math.Min(clientRect.Right, Cursor.Position.X));
                int newY = Math.Max(clientRect.Top, Math.Min(clientRect.Bottom, Cursor.Position.Y));

                // Set the new cursor position
                Cursor.Position = new Point(newX, newY);
            }
        }
        private void DisplayCheckedInformation()
        {

            string ConsultantNodes = GetCheckedNodes(ComboBoxConsultant, "For All Consultant");
            string DepartmentNodes = GetCheckedNodes(ComboBoxDepartment, "For All Department");

            string DisplayFor = "";
            string title = "Care Taker UnAssign Report";
            if (ReportIndex == 1)
            {
                if (ConsultantNodes != null && ConsultantNodes.ToString() != "For All Consultant")
                {
                    DisplayFor = " For Consultant ";
                }
            }
            if (ReportIndex == 2)
            {
                if (DepartmentNodes != null && DepartmentNodes.ToString() != "For All Department")
                {
                    DisplayFor = " For Department ";
                }
            }

            title += AppendToTitleIfNotEmpty(ConsultantNodes!, DisplayFor);
            title += AppendToTitleIfNotEmpty(DepartmentNodes!, DisplayFor);

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

        private void ComboBoxDepartment_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }

        private void ComboBoxConsultant_NodeClickedEvent(object sender, ComboTreeNodeEventArgs e)
        {
            DisplayCheckedInformation();
        }
        private void EnableButtons(bool Enable)
        {
            BtnSave.Enabled = Enable;
            BtnPrint.Enabled = Enable;
            ToolStripBtnPrint.Enabled = Enable;
            ToolStripBtnSave.Enabled = Enable;
        }
        private bool FormValidate()
        {
            CareTakerReportErrorMsg.Text = "";
            if (ComboBoxReportType.SelectedIndex < 0)
            {
                CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, "Type...");
                ComboBoxReportType.Select();
                return false;
            }
            if (ComboBoxReportType.Text.Equals("By Patient", StringComparison.OrdinalIgnoreCase) && CheckedTreeUtils.SelectedNodes(ComboBoxConsultant).Count < 1)
            {
                CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                ComboBoxConsultant.Focus();
                return false;
            }
            if (ComboBoxReportType.Text.Equals("By Test Name", StringComparison.OrdinalIgnoreCase) && CheckedTreeUtils.SelectedNodes(ComboBoxDepartment).Count < 1)
            {
                CareTakerReportErrorMsg.Text = string.Format(EnterValidTypeErrorMsg, LabelType.Text);
                ComboBoxDepartment.Focus();
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
            if (CTReportFromDate.Date != null && CTReportToDate.Date != null && CTReportFromDate.Date > CTReportToDate.Date)
            {
                CareTakerReportErrorMsg.Text = EnterValidDateErrorMsg;
                CTReportFromDate.Focus();
                return false;
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Keys.Escape)
                {
                    BtnReset.PerformClick();
                    return true;
                }
                else if (keyData == Keys.F8)
                {
                    BtnSave.PerformClick();
                    return true;
                }
                else if (keyData == Keys.F9)
                {
                    BtnPrint.PerformClick();
                    return true;
                }
                else if (keyData == Keys.F10)
                {
                    BtnExit.PerformClick();
                    return true;
                }
                else if (keyData == (Keys.Shift | Keys.Tab)) // Shift + Tab
                {
                    HandleShiftTabKey();
                    return true;
                }
                else if (keyData == Keys.Tab) // Tab
                {
                    ToolStripTabIndexChanged();
                    HandleTabKey();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HandleShiftTabKey()
        {
            // Handle Shift + Tab key press for reverse navigation
            if (this.ActiveControl == CTReportFromDate.Control)
            {
                if (GetComboIndex == 0)
                {
                    ComboBoxReportType.Focus();
                }
                else if (GetComboIndex == 1)
                {
                    ComboBoxConsultant.Focus();
                }
                else if (GetComboIndex == 2)
                {
                    ComboBoxDepartment.Focus();
                }
            }
            else if (this.ActiveControl == ComboBoxConsultant.Control)
            {
                ComboBoxReportType.Focus();
            }
            else if (this.ActiveControl == ComboBoxDepartment.Control)
            {
                ComboBoxReportType.Focus();
            }
            else if (this.ActiveControl == CTReportToDate.Control)
            {
                CTReportFromDate.Focus();
            }
            else if (ToolStripBtnGo.Selected == true)
            {
                ToolStripBtnGo.Checked = false;
                CTReportToDate.Focus();
            }
            else if (ToolStripBtnSave.Selected)
            {
                BerklySoftToolStrip.Focus();
                ToolStripBtnPrint.Select();
            }
            else if (ToolStripBtnPrint.Selected)
            {
                BerklySoftToolStrip.Focus();
                ToolStripBtnSave.Select();
            }
            else if (this.ActiveControl == BtnSave)
            {
                BerklySoftToolStrip.Focus();
                ToolStripBtnPrint.Select();
            }
            else if (this.ActiveControl == BtnPrint)
            {
                BtnSave.Focus();
            }
            else if (this.ActiveControl == BtnReset)
            {
                BtnPrint.Focus();
            }
            else if (this.ActiveControl == BtnExit)
            {
                BtnReset.Focus();
            }
        }
        private void HandleTabKey()
        {
            // Handle regular Tab key press
            if (this.ActiveControl == ComboBoxReportType.Control)
            {
                if (GetComboIndex == 0)
                {
                    ExecuteComboBoxSelection();
                    CTReportFromDate.Focus();
                }
                else if (GetComboIndex == 1)
                {
                    ExecuteComboBoxSelection();
                    ComboBoxConsultant.Focus();
                }
                else if (GetComboIndex == 2)
                {
                    ExecuteComboBoxSelection();
                    ComboBoxDepartment.Focus();
                }
            }
            else if (this.ActiveControl == ComboBoxConsultant.Control || this.ActiveControl == ComboBoxDepartment.Control)
            {
                CTReportFromDate.Focus();
            }
            else if (this.ActiveControl == CTReportFromDate.Control)
            {
                CTReportToDate.Focus();
            }
            else if (this.ActiveControl == CTReportToDate.Control)
            {
                BerklySoftToolStrip.Focus();
                ToolStripBtnGo.Select();
            }
            else if (ToolStripBtnGo.Selected)
            {
                if (ToolStripBtnSave.Enabled)
                {
                    BtnSave.Focus();
                }
                else
                {
                    BtnExit.Focus();
                }
            }
            else if (ToolStripBtnSave.Selected)
            {
                BerklySoftToolStrip.Focus();
                ToolStripBtnPrint.Select();
            }
            else if (ToolStripBtnPrint.Selected)
            {
                BtnSave.Focus();
            }
            else if (this.ActiveControl == BtnSave)
            {
                BtnPrint.Focus();
            }
            else if (this.ActiveControl == BtnPrint)
            {
                BtnReset.Focus();
            }
            else if (this.ActiveControl == BtnReset)
            {
                BtnExit.Focus();
            }
            else if (this.ActiveControl == BtnExit)
            {
                BerklySoftToolStrip.Focus();
                ComboBoxReportType.Focus();
            }

        }

        private void ToolStripBtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CareTakerUnAssignSavePrint CTAReportSavePrint = new CareTakerUnAssignSavePrint();
            CTAReportSavePrint.ExportOrPrintToFile(GridviewCareTakerReport, ReportCareTakerUnAssign, "pdf", true, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void ToolStripBtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CareTakerUnAssignSavePrint CTAReportSavePrint = new CareTakerUnAssignSavePrint();
            CTAReportSavePrint.ExportOrPrintToFile(GridviewCareTakerReport, ReportCareTakerUnAssign, "pdf", false, ReportIndex);
            Cursor.Current = Cursors.Default;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ToolStripBtnSave_Click(sender, e);
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            ToolStripBtnPrint_Click(sender, e);
        }

        private void GridviewCareTakerReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == (int)CareTakerUnAssignTableColumn.SLNO) && e.Value != null)
            {
                DataGridViewCell cell = GridviewCareTakerReport.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Style.WrapMode = DataGridViewTriState.False;
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
            else if (ComboBoxReportType.Text.Equals("By Department", StringComparison.OrdinalIgnoreCase))
            {
                GetCurrentIndex = 2;
            }
            else
            {
                GetCurrentIndex = -1;
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
        private void ComboBoxReportType_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBoxReportType.Items.Count == 0)
            {
                StoreComboItems();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
    }
}
