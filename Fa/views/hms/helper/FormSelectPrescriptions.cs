using fa.api.catalog;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Catalog;
using fa.model.hms.common;
using fa.model.OrderManagement;
using fa.views.hms.ip;
using fa.views.hms.patient;
using fa.views.purchase;
using Standard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.hms.helper
{
    public enum PrescriptionSelectTableColoumn
    {
        NAME, TOTAL, DAY, BEFOREAFTER, INTRVL, MORNING, AFTERNOON, EVENING, NIGHT, ADDNOTE, REMOVE, PID, ISCUSTOM
    }
    public enum BatchTableColumnForSelectPrescription
    {
        NAME, BATCHNO, EXPDATE, QTY, UOM, LOCATION, MRP, ID
    }
    public partial class FormSelectPrescriptions : FormPatientBase
    {
        public static string SelectPrescriptionErrorMsg = "Please select Prescription.";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete this Prescription Product {0}?";
        public static string SelectPrescripedMedicienPreferedAfterorBeforeFoodErrorMsg = "Please select prescriped medicine to be taken after/before food";
        public static string EnterPrescripedMedicienDosageErrorMsg = "Please enter the valid dosage details";
        public static string EnterPrescripedMedicienTakenDaysErrorMsg = "Please enter the valid number of days for the medication";
        public static string SelectPrescripedMedicienTakenTimeDelayErrorMsg = "Please select the interval for the medication";
        public static string SelectPrescripedMedicienTakenTimeErrorMsg = "Please select the interval for the medication";
        public static string PrescriptionSendMedicalMsg = "Prescriptions has been sent to medical";
        public static string PrescriptionTimeIntervalErrorMsg = "Can't have a breakup for Time Interval";
        public static string NoBatchfoundErrorMsg = "You do not have any batch, Please add a batch!";
        public static string SearchOutput = "No Batch found!";
        public List<long> Oldids = new List<long>();
        public List<long> Newids = new List<long>();
        public bool isDirty = false;
        List<long> PrescriptionIds = new List<long>();
        public DataGridViewComboBoxCell SelectedPrescriptionIds;
        public DataGridViewComboBoxCell SelectedPrescriptionDisc;
        public DataGridViewComboBoxCell SelectedPrescriptionsDosage;
        public DataGridViewComboBoxCell SelectedPrescriptionsDosageDays;
        public DataGridViewComboBoxCell SelectedPrescriptionsInterval;
        public DataGridViewComboBoxCell SelectedPrescriptionsBeforeAfter;
        public DataGridViewComboBoxCell SelectedPrescriptionsMorning;
        public DataGridViewComboBoxCell SelectedPrescriptionsAfterNoon;
        public DataGridViewComboBoxCell SelectedPrescriptionsEvening;
        public DataGridViewComboBoxCell SelectedPrescriptionsNight;
        public DataGridViewComboBoxCell SelectedPrescriptionsNotes;
        public DataGridViewComboBoxCell SelectedPrescriptionsName;
        public string SelectedPrescriptionsNames;

        FormPatientBase parent = null!;
        public FormSelectPrescriptions(Object Sender)
        {
            if (Sender is FormConsulting)
            {
                parent = (FormConsulting)Sender;
            }
            InitializeComponent();
        }
        private void FormSelectPrescriptions_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                GetPrescriptionsId();
                LoadPrescriptionWithFilter();
                LoadPrescriptions();
                PrescriptionSearchTextBox.Select();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadPrescriptionWithFilter()
        {
            Cursor.Current = Cursors.WaitCursor;
            string FilterString = PrescriptionSearchTextBox.Text.Trim();
            PrescriptionListBox.BeginUpdate();
            PrescriptionListBox.Items.Clear();
            var lPrescription = Global.ProductDetailList.AsParallel().Where(x => x.CompanyId == Global.Company.CompanyId && x.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1).OrderByDescending(x => char.IsLetter(x.Name[0]) ? 0 : 1).ThenBy(x => x.Name).ToList();
            if (lPrescription.Count > 0)
            {
                PrescriptionListBox.Items.AddRange(lPrescription.ToArray());
                PrescriptionListBox.SelectedIndex = 0;
            }

            if (PrescriptionListBox.Items.Count > 0)
            {
                PrescriptionListBox.SelectedIndex = 0;
            }
            PrescriptionListBox.EndUpdate();
            Cursor.Current = Cursors.Default;
        }
        private void PrescriptionSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadPrescriptionWithFilter();
        }
        private void PrescriptionListBox_DoubleClick(object sender, EventArgs e)
        {
            if (PrescriptionListBox.Items.Count > 0)
            {
                var item = (Product)PrescriptionListBox.Items[PrescriptionListBox.SelectedIndex];
                if (!PrescriptionIds.Contains(item.Id))
                {
                    PrescriptionIds.Add(item.Id);
                    InsertTickedPrescription(item.Id);
                }
            }
        }
        private void ResetForm()
        {
            ErrorMsgSelectPrescription.Text = string.Empty;
            PrescriptionSearchTextBox.Text = string.Empty;
            GridViewSelectPrescription.Rows.Clear();
        }
        private void GetPrescriptionsId()
        {
            Oldids = new List<long>();
            Newids = new List<long>();
            PrescriptionIds = new List<long>();
            if (SelectedPrescriptionIds != null && SelectedPrescriptionIds.Items.Count > 0)
            {
                foreach (var Id in SelectedPrescriptionIds.Items)
                {
                    if (Id.ToString().IsNumeric())
                    {
                        Oldids.Add(long.Parse(Id.ToString()));
                        Newids.Add(long.Parse(Id.ToString()));
                        PrescriptionIds.Add(long.Parse(Id.ToString()!));
                    }
                }
            }
        }
        private void LoadPrescriptions()
        {
            GridViewSelectPrescription.Rows.Clear();
            if (SelectedPrescriptionsName != null && SelectedPrescriptionsName.Items.Count > 0)
            {
                for (int p = 0; p < SelectedPrescriptionsName.Items.Count; p++)
                {
                    GridViewSelectPrescription.Rows.Add();
                    GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.PID].Value = SelectedPrescriptionIds.Items[p];
                    GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.NAME].Value = SelectedPrescriptionsName.Items[p];
                    GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.TOTAL].Value = SelectedPrescriptionsDosage.Items[p];
                    GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.DAY].Value = SelectedPrescriptionsDosageDays.Items[p];
                    GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.INTRVL].Value = SelectedPrescriptionsInterval.Items[p];
                    GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER].Value = SelectedPrescriptionsBeforeAfter.Items[p];
                    GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.ADDNOTE].Value = SelectedPrescriptionsNotes.Items[p];
                    if (string.IsNullOrEmpty(SelectedPrescriptionsInterval.Items[p].ToString()))
                    {
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.MORNING].Value = SelectedPrescriptionsMorning.Items[p];
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].Value = SelectedPrescriptionsAfterNoon.Items[p];
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.EVENING].Value = SelectedPrescriptionsEvening.Items[p];
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.NIGHT].Value = SelectedPrescriptionsNight.Items[p];
                    }
                    else
                    {
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.MORNING].ReadOnly = true;
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].ReadOnly = true;
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.EVENING].ReadOnly = true;
                        GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.NIGHT].ReadOnly = true;
                    }
                }
            }
        }
        private void GridViewSelectPrescription_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewSelectPrescription_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (e.ColumnIndex == (int)PrescriptionSelectTableColoumn.INTRVL)
                {
                    if (GridViewSelectPrescription.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.IsNullOrWhiteSpace(GridViewSelectPrescription.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                    {
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.MORNING].ReadOnly = true;
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].ReadOnly = true;
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.EVENING].ReadOnly = true;
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NIGHT].ReadOnly = true;
                    }
                    else
                    {
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.MORNING].ReadOnly = false;
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].ReadOnly = false;
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.EVENING].ReadOnly = false;
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NIGHT].ReadOnly = false;
                    }
                }
                if (e.ColumnIndex == (int)PrescriptionSelectTableColoumn.MORNING
                    || e.ColumnIndex == (int)PrescriptionSelectTableColoumn.AFTERNOON
                    || e.ColumnIndex == (int)PrescriptionSelectTableColoumn.EVENING
                    || e.ColumnIndex == (int)PrescriptionSelectTableColoumn.NIGHT)
                {
                    if (GridViewSelectPrescription.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.INTRVL].ReadOnly = false;
                    }
                    else
                    {
                        GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.INTRVL].ReadOnly = false;
                    }
                }
                else
                {
                    GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.INTRVL].ReadOnly = false;
                }
            }
        }
        private void GridViewSelectPrescription_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var Value = from Enum en in Enum.GetValues(typeof(TakeDosage)) select new { Id = en, Name = en.ToString().Replace("None", " ") };

            (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.DataSource = null;
            (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.DataSource = Value.ToList();
            (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.ValueMember = "Id";
            (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
            (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.AutoComplete = true;
            (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.Value = TakeDosage.AFTER;
            GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.REMOVE].Value = "X";
            GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.MORNING].Value = "";
            GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].Value = "";
            GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.EVENING].Value = "";
            GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NIGHT].Value = "";
        }
        private void InsertTickedPrescription(long PresciptionProductId)
        {
            GridViewSelectPrescription.Rows.Add();
            var PresciptionProduct = Global.ProductList.FirstOrDefault(x => x.Id == PresciptionProductId);
            if (PresciptionProduct != null)
            {
                int p = GridViewSelectPrescription.Rows.Count - 1;
                GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.NAME].Value = PresciptionProduct.Name;
                GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.PID].Value = PresciptionProduct.Id;
                GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.ISCUSTOM].Value = false;
                GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.REMOVE].Value = "X";
            }
        }
        private void GridViewSelectPrescription_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 10 && (GridViewSelectPrescription.Rows.Count) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NAME].Value != null ? GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NAME].Value.ToString() : "Custom row"), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        long UnCheckPrescription = GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.PID].Value != null && GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.PID].Value.ToString().IsNumeric() ? long.Parse(GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.PID].Value.ToString()!) : 0L;
                        GridViewSelectPrescription.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSelectPrescription.Rows.RemoveAt(e.RowIndex);
                        if (UnCheckPrescription != 0L)
                        {
                            PrescriptionIds.Remove(UnCheckPrescription);
                        }
                    }
                }
            }
        }
        private void GridViewSelectPrescription_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NAME].ReadOnly = true;
            GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.REMOVE].ReadOnly = true;
            if (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NAME].Value != null)
            {
                GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.TOTAL].ReadOnly = false;
                GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.INTRVL].ReadOnly = false;
            }
            if (GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.ISCUSTOM].Value != null &&
                (bool)GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.ISCUSTOM].Value)
            {
                GridViewSelectPrescription.Rows[e.RowIndex].Cells[(int)PrescriptionSelectTableColoumn.NAME].ReadOnly = false;
            }
        }
        private void GridViewSelectPrescription_SelectionChanged(object sender, EventArgs e)
        {
            GridViewSelectPrescription.ClearSelection();
        }
        private void GridViewSelectPrescription_Enter(object sender, EventArgs e)
        {
            if (GridViewSelectPrescription.Rows.Count > 1)
            {
                GridViewSelectPrescription.CurrentCell = GridViewSelectPrescription[1, 0];
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelectPrescriptionDone.PerformClick();
            }
            if (keyData == (Keys.Escape))
            {
                BtnSelectPrescriptionCancel.PerformClick();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.BtnSelectPrescriptionDone)
            {
                GridViewSelectPrescription.Select();
                GridViewSelectPrescription.CurrentCell = GridViewSelectPrescription[1, 0];
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnSelectPrescriptionCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridViewSelectPrescription.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewSelectPrescription.CurrentCell.ColumnIndex == (int)PrescriptionSelectTableColoumn.ADDNOTE)
                    {
                        if (GridViewSelectPrescription.CurrentCell.RowIndex == GridViewSelectPrescription.Rows.Count - 1)
                        {
                            GridViewSelectPrescription.CurrentCell = null;
                            ActiveControl = BtnSelectPrescriptionDone;
                            BtnSelectPrescriptionDone.Focus();
                            return true;
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSelectPrescription.CurrentCell.ColumnIndex == (int)PrescriptionSelectTableColoumn.TOTAL)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnSelectPrescriptionDone_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SelectedPrescriptionIds = new DataGridViewComboBoxCell();
                SelectedPrescriptionsNames = string.Empty;
                SelectedPrescriptionsName = new DataGridViewComboBoxCell();
                SelectedPrescriptionsDosage = new DataGridViewComboBoxCell();
                SelectedPrescriptionsDosageDays = new DataGridViewComboBoxCell();
                SelectedPrescriptionsInterval = new DataGridViewComboBoxCell();
                SelectedPrescriptionsBeforeAfter = new DataGridViewComboBoxCell();
                SelectedPrescriptionsMorning = new DataGridViewComboBoxCell();
                SelectedPrescriptionsAfterNoon = new DataGridViewComboBoxCell();
                SelectedPrescriptionsEvening = new DataGridViewComboBoxCell();
                SelectedPrescriptionsNight = new DataGridViewComboBoxCell();
                SelectedPrescriptionsNotes = new DataGridViewComboBoxCell();
                Newids = new List<long>();
                int i = 0;
                foreach (DataGridViewRow row in GridViewSelectPrescription.Rows)
                {
                    if (row.Cells[(int)PrescriptionSelectTableColoumn.NAME].Value != null)
                    {
                        SelectedPrescriptionsName.Items.Add(row.Cells[(int)PrescriptionSelectTableColoumn.NAME].Value.ToString());
                        SelectedPrescriptionIds.Items.Add(row.Cells[(int)PrescriptionSelectTableColoumn.PID].Value != null ? row.Cells[(int)PrescriptionSelectTableColoumn.PID].Value.ToString() : row.Cells[(int)PrescriptionSelectTableColoumn.NAME].Value.ToString());
                        SelectedPrescriptionsDosage.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.TOTAL].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.TOTAL].Value.ToString());
                        SelectedPrescriptionsDosageDays.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.DAY].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.DAY].Value.ToString());
                        SelectedPrescriptionsInterval.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.INTRVL].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.INTRVL].Value.ToString());
                        SelectedPrescriptionsBeforeAfter.Items.Add((TakeDosage)Enum.Parse(typeof(TakeDosage), string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER].Value?.ToString()) ? "None" : row.Cells[(int)PrescriptionSelectTableColoumn.BEFOREAFTER].Value?.ToString()!, true));
                        SelectedPrescriptionsNotes.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.ADDNOTE].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.ADDNOTE].Value.ToString());
                        SelectedPrescriptionsMorning.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.MORNING].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.MORNING].Value.ToString());
                        SelectedPrescriptionsAfterNoon.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].Value.ToString());
                        SelectedPrescriptionsEvening.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.EVENING].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.EVENING].Value.ToString());
                        SelectedPrescriptionsNight.Items.Add(string.IsNullOrEmpty(row.Cells[(int)PrescriptionSelectTableColoumn.NIGHT].Value?.ToString()) ? "" : row.Cells[(int)PrescriptionSelectTableColoumn.NIGHT].Value.ToString());

                        var SelectedPrescription = GridViewSelectPrescription.Rows[i].Cells[(int)PrescriptionSelectTableColoumn.PID].Value != null && GridViewSelectPrescription.Rows[i].Cells[(int)PrescriptionSelectTableColoumn.PID].Value.ToString().IsNumeric() ? Global.ProductList.FirstOrDefault(x => x.Id == long.Parse(GridViewSelectPrescription.Rows[i].Cells[(int)PrescriptionSelectTableColoumn.PID].Value.ToString()!)) : null;
                        SelectedPrescriptionsNames = string.IsNullOrEmpty(SelectedPrescriptionsNames) ? (SelectedPrescription != null ? SelectedPrescription.Name : GridViewSelectPrescription.Rows[i].Cells[(int)PrescriptionSelectTableColoumn.NAME].Value.ToString()!) : SelectedPrescriptionsNames + ",\n" + (SelectedPrescription != null ? SelectedPrescription.Name : GridViewSelectPrescription.Rows[i].Cells[(int)PrescriptionSelectTableColoumn.NAME].Value.ToString());
                        Newids.Add(long.Parse(row.Cells[(int)PrescriptionSelectTableColoumn.PID].Value != null ? (GridViewSelectPrescription.Rows[i].Cells[(int)PrescriptionSelectTableColoumn.PID].Value.ToString().IsNumeric() ? GridViewSelectPrescription.Rows[i].Cells[(int)PrescriptionSelectTableColoumn.PID].Value.ToString()! : "0") : "0"));
                    }
                    i++;
                }
                this.Close();
            }
        }
        private bool ValidateForm()
        {
            foreach (DataGridViewRow row in GridViewSelectPrescription.Rows)
            {
                // Validate NAME column
                if (row.Cells[(int)PrescriptionSelectTableColoumn.NAME] != null)
                {
                    var nameCellValue = row.Cells[(int)PrescriptionSelectTableColoumn.NAME].Value;
                    if (nameCellValue == null || string.IsNullOrWhiteSpace(nameCellValue.ToString()))
                    {
                        ErrorMsgSelectPrescription.Text = SelectPrescriptionErrorMsg;
                        GridViewSelectPrescription.CurrentCell = row.Cells[(int)PrescriptionSelectTableColoumn.NAME];
                        GridViewSelectPrescription.BeginEdit(true);
                        return false;
                    }
                }

            }

            return true;
        }

        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }

        private void BtnSelectPrescriptionCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            GetPrescriptionsId();
            LoadPrescriptionWithFilter();
            LoadPrescriptions();
            PrescriptionSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }

        private void BtnRefreshPrescription_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.ProductDetailList = CatalogProductManager.Instance.ListProductByCompanyId(Global.Company.CompanyId);
            LoadPrescriptionWithFilter();
            PrescriptionSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }

        private void PrescriptionCheckedListBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewSelectPrescription.Rows.Count > 0)
                {
                    GridViewSelectPrescription.Select();
                    GridViewSelectPrescription.CurrentCell = GridViewSelectPrescription[(int)PrescriptionSelectTableColoumn.TOTAL, 0];
                    GridViewSelectPrescription.BeginEdit(true);
                }
                else
                {
                    BtnSelectPrescriptionDone.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                PrescriptionSearchTextBox.Select();

            }
        }

        private void BtnSelectPrescriptionDone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                PrescriptionSearchTextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSelectPrescription.Rows.Count > 0)
                {
                    GridViewSelectPrescription.Select();
                    GridViewSelectPrescription.CurrentCell = GridViewSelectPrescription[(int)PrescriptionSelectTableColoumn.TOTAL, GridViewSelectPrescription.Rows.Count - 1];
                    GridViewSelectPrescription.BeginEdit(true);
                }
                else
                {
                    PrescriptionListBox.Select();
                }
            }
        }
        private void GridViewSelectPrescription_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }

        private void BtnSelectPrescriptionAddCustomRow_Click(object sender, EventArgs e)
        {
            GridViewSelectPrescription.Rows.Add();
            int p = GridViewSelectPrescription.Rows.Count - 1;
            GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.ISCUSTOM].Value = true;
            GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.NAME].ReadOnly = false;
            GridViewSelectPrescription.Rows[p].Cells[(int)PrescriptionSelectTableColoumn.REMOVE].Value = "X";
        }

        private void GridViewSelectPrescription_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            System.Windows.Forms.ComboBox? combo = e.Control as System.Windows.Forms.ComboBox;
            if (GridViewSelectPrescription.CurrentCell.ColumnIndex == (int)PrescriptionSelectTableColoumn.INTRVL)
            {
                combo!.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged!);
                combo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged!);
            }
            int RowIndex = GridViewSelectPrescription.CurrentCell.RowIndex;
            if (GridViewSelectPrescription.CurrentCell == GridViewSelectPrescription[(int)PrescriptionSelectTableColoumn.REMOVE, RowIndex]
                    || GridViewSelectPrescription.CurrentCell == GridViewSelectPrescription[(int)PrescriptionSelectTableColoumn.MORNING, RowIndex]
                    || GridViewSelectPrescription.CurrentCell == GridViewSelectPrescription[(int)PrescriptionSelectTableColoumn.EVENING, RowIndex]
                    || GridViewSelectPrescription.CurrentCell == GridViewSelectPrescription[(int)PrescriptionSelectTableColoumn.AFTERNOON, RowIndex]
                    || GridViewSelectPrescription.CurrentCell == GridViewSelectPrescription[(int)PrescriptionSelectTableColoumn.NIGHT, RowIndex])
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            }
            else
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            }
        }

        private void PrescriptionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product Product = (Product)PrescriptionListBox.Items[PrescriptionListBox.SelectedIndex];
            loadDefaultBatch(Product.Id);
        }
        private void loadDefaultBatch(long ProductId)
        {
            GridViewSelectPrescriptionBatch.Rows.Clear();
            IList<InventoryBatch> InventoryBatch = new List<InventoryBatch>();
            IList<Inventory> Inventorys = InventoryLocationManager.Instance.ListInventoryByProductId(ProductId);
            foreach (Inventory Inventory in Inventorys)
            {
                if (Inventory != null && Inventory.InventoryLocationId != 0L)
                {
                    IList<InventoryBatch> lInventoryBatch = InventoryLocationManager.Instance.GetInventoryBatchDetailbySearchText(ProductId, Inventory.InventoryLocationId);
                    foreach (var item in lInventoryBatch)
                    {
                        InventoryBatch.Add(item);
                    }
                }
            }
            if (InventoryBatch != null && InventoryBatch.Count > 0)
            {
                LoadInventoryBatch(InventoryBatch);
            }
            else
            {
                GridViewSelectPrescriptionBatch.Rows.Clear();
                ErrorMsgSelectPrescription.Text = NoBatchfoundErrorMsg;
            }
        }
        private void LoadInventoryBatch(IList<InventoryBatch> InventoryBatch)
        {
            ErrorMsgSelectPrescription.Text = "";
            if (InventoryBatch.Count > 0)
            {
                int i = 0;
                string ProductName = string.Empty;
                foreach (InventoryBatch lInventoryBatch in InventoryBatch)
                {
                    if (((lInventoryBatch.StockDate != null && lInventoryBatch.StockDate <= Global.getTransactionDate().Date ? lInventoryBatch.OpeningStock : 0) + lInventoryBatch.QuantityOnHand) > 0 && lInventoryBatch.BatchNo != null)
                    {
                        if (lInventoryBatch.BatchNo != "")
                        {
                            InventoryBatch InventoryBatchFDB = InventoryLocationManager.Instance.GetInventoryByBatchIdWithLocation(lInventoryBatch.Id, lInventoryBatch.Inventory.InventoryLocationId);
                            i = GridViewSelectPrescriptionBatch.Rows.Add();
                            if (ProductName == string.Empty || ProductName != lInventoryBatch.Product.Name)
                            {
                                GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.NAME].Value = lInventoryBatch.Product.Name;
                            }
                            GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.BATCHNO].Value = lInventoryBatch.BatchNo;
                            GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.EXPDATE].Value = api.utils.DateUtils.FormatDate(lInventoryBatch.ExpDate, Global.Company.DateFormat);
                            GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.QTY].Value = lInventoryBatch.Inventory.InventoryLocationId == 0L ? ((lInventoryBatch.StockDate != null && lInventoryBatch.StockDate <= Global.getTransactionDate().Date ? lInventoryBatch.OpeningStock : 0) + lInventoryBatch.QuantityOnHand) : InventoryBatchFDB == null ? 0 : ((InventoryBatchFDB.StockDate != null && InventoryBatchFDB.StockDate <= Global.getTransactionDate().Date ? InventoryBatchFDB.OpeningStock : 0) + InventoryBatchFDB.QuantityOnHand);
                            GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.UOM].Value = lInventoryBatch.RetailUOM;
                            GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.LOCATION].Value = lInventoryBatch.Inventory.InventoryLocation.Name;
                            GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.MRP].Value = lInventoryBatch.MaxRetailPrice.ToString(TextUtils.DecimalPlace(Global.Company.QuantityPricision));
                            GridViewSelectPrescriptionBatch.Rows[i].Cells[(int)BatchTableColumnForSelectPrescription.ID].Value = lInventoryBatch.Id;
                            ProductName = lInventoryBatch.Product.Name;
                            i++;
                        }
                    }
                }
            }
            else
            {
                ErrorMsgSelectPrescription.Text = SearchOutput;
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)sender;
            string item = cb.Text;
            if (GridViewSelectPrescription.CurrentCell.ColumnIndex == (int)PrescriptionSelectTableColoumn.INTRVL)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.MORNING].ReadOnly = false;
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].ReadOnly = false;
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.EVENING].ReadOnly = false;
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.NIGHT].ReadOnly = false;
                }
                else
                {
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.MORNING].Value = "";
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].Value = "";
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.EVENING].Value = "";
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.NIGHT].Value = "";

                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.MORNING].ReadOnly = true;
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.AFTERNOON].ReadOnly = true;
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.EVENING].ReadOnly = true;
                    GridViewSelectPrescription.CurrentRow.Cells[(int)PrescriptionSelectTableColoumn.NIGHT].ReadOnly = true;
                }
            }
        }
    }
}
