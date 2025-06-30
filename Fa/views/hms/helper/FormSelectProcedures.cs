using fa.views.hms.masters;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fa.model.Hms.Master;
using Fa.api.Hms;
using fa.views.hms.patient;
using fa.api.Hms;
using fa.libraries.utils;
using fa.views.controls.ComboTreeView;
using System.Linq;

namespace fa.views.hms.helper
{
    public enum ProceduresSelectTableColoumn
    {
        NAME, DESC, FEE, REMOVE, ID, PID, PROCDNAME, ISACTIVE
    }
    public partial class FormSelectProcedures : FormPatientBase
    {
        public static string Grid_ConfirmDiscChangeText = "Do you want to save the Procedure Description for {0} as {1}?";
        public static string Grid_ConfirmFeeChangeText = "Do you want to save the Procedure fee for {0} as {1}?";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete Procedure {0}?";
        public static string SelectProcedureErrorMsg = "Please select Procedure.";
        public static string SelectProcedureUpdateMsg = "Updated {0} for Procedure {1}.";
        public static string SelectFeeErrorMsg = "Fee could not be empty, Please enter Fee";
        public List<long> Oldids = new List<long>();
        public List<long> Newids = new List<long>();
        public bool isDirty = false;
        List<long> ProcedureIds = new List<long>();
        MedicalProcedureManager MedicalProcedureManager = null;
        public DataGridViewComboBoxCell SelectedProcedureIds;
        public DataGridViewComboBoxCell SelectedProcedureFees;
        public DataGridViewComboBoxCell SelectedProcedureDisc;
        public string SelectedProcedureNames;
        public string SelectedProcedureDiscp;

        FormPatientBase parent = null;
        public FormSelectProcedures(Object Sender)
        {
            if (Sender is FormConsulting)
            {
                parent = (FormConsulting)Sender;
            }
            MedicalProcedureManager = MedicalProcedureManager.Instance;
            InitializeComponent();
        }

        private void FormSelectProcedures_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                GetProceduresId();
                LoadProcedureWithFilter();
                LoadProcedures();
                ProcedureSearchTextBox.Select();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        private void LoadProcedureWithFilter()
        {
            string FilterString = ProcedureSearchTextBox.Text.Trim();
            ProcedureCheckedListBox.Items.Clear();
            foreach (var lPrescription in Global.ProcedureDetailList)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lPrescription.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ProcedureCheckedListBox.Items.Add(lPrescription, ProcedureIds.Contains(lPrescription.Id) ? true : false);
                }
            }
            if (ProcedureCheckedListBox.Items.Count > 0)
            {
                ProcedureCheckedListBox.SelectedIndex = 0;
            }
        }
        private void ProcedureSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadProcedureWithFilter();
        }

        private void ProcedureCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index > -1)
            {
                var item = (MedicalProcedure)ProcedureCheckedListBox.Items[e.Index];
                if (!ProcedureCheckedListBox.GetItemChecked(e.Index))
                {
                    if (!ProcedureIds.Contains(item.Id))
                    {
                        ProcedureIds.Add(item.Id);
                        InsertTickedProcedures(item.Id);
                    }
                }
                else
                {
                    ProcedureIds.Remove(item.Id);
                    RemoveUnTickedProcedures(item.Id.ToString());
                }
            }
        }
        private Boolean Validation()
        {
            ErrorMsgSelectProcedures.Text = string.Empty;
            if (GridViewSelectProcedures.Rows.Count > 1)
            {
                for (int i = 0; i < GridViewSelectProcedures.Rows.Count - 1; i++)
                {
                    if (GridViewSelectProcedures.Rows[i].Cells[(int)ConsultedConsultationTableColumn.NAME].Value == null)
                    {
                        ErrorMsgSelectProcedures.Text = SelectProcedureErrorMsg;
                        return false;
                    }
                    if (GridViewSelectProcedures.Rows[i].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString() == "0" || GridViewSelectProcedures.Rows[i].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString() == "0.00" || string.IsNullOrEmpty(GridViewSelectProcedures.Rows[i].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString().Trim()))
                    {
                        ErrorMsgSelectProcedures.Text = SelectFeeErrorMsg;
                        GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures.Rows[i].Cells[2];
                        GridViewSelectProcedures.CurrentCell.Selected = true;
                        return false;
                    }
                }
            }
            return true;
        }
        private void ResetForm()
        {
            ErrorMsgSelectProcedures.Text = string.Empty;
            ProcedureSearchTextBox.Text = string.Empty;
            GridViewSelectProcedures.Rows.Clear();
        }

        private void GetProceduresId()
        {
            Oldids = new List<long>();
            Newids = new List<long>();
            ProcedureIds = new List<long>();
            if (SelectedProcedureIds != null && SelectedProcedureIds.Items.Count > 0)
            {
                foreach (var Id in SelectedProcedureIds.Items)
                {
                    Oldids.Add(long.Parse(Id.ToString()));
                    Newids.Add(long.Parse(Id.ToString()));
                    ProcedureIds.Add(long.Parse(Id.ToString()));
                }
            }
        }

        private void LoadProcedures()
        {
            if (ProcedureIds.Count > 0)
            {
                for (int p = 0; p < ProcedureIds.Count; p++)
                {
                    MedicalProcedure Proced = Global.AllProcesdureDetailList.FirstOrDefault(x => x.Id == ProcedureIds[p]);
                    if (Proced != null)
                    {
                        GridViewSelectProcedures.Rows.Add();
                        GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.NAME].Value = Proced.Name;
                        GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.DESC].Value = SelectedProcedureDisc.Items[p].ToString();
                        GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.PROCDNAME].Value = Proced.Name;
                        GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.FEE].Value = SelectedProcedureFees.Items[p];
                        GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.PID].Value = Proced.Id;
                        GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.ISACTIVE].Value = Proced.IsActive;
                    }
                }
            }
        }
        private void GridViewSelectProcedures_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewSelectProcedures_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.NAME].ReadOnly = true;
            GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC].ReadOnly = true;
            GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].ReadOnly = true;
            GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.REMOVE].ReadOnly = true;
            if (GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.NAME].Value != null)
            {
                GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC].ReadOnly = false;
                GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].ReadOnly = false;
            }
        }
        private void GridViewSelectProcedures_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                MedicalProcedure MedicalProcedures = MedicalProcedureManager.GetMedicalProcedureByName(GridViewSelectProcedures.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (MedicalProcedures != null)
                {
                    GridViewSelectProcedures.CurrentCell.Value = MedicalProcedures.Name;
                }
            }
        }

        double Fee = 0.00;
        string Desc = string.Empty;
        private void GridViewSelectProcedures_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ProceduresSelectTableColoumn.FEE &&
                GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.NAME].Value != null &&
                GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value != null)
            {
                Fee = double.Parse(GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value.ToString());
                GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures.Rows[e.RowIndex].Cells[2];
            }
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ProceduresSelectTableColoumn.DESC &&
                GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.NAME].Value != null &&
                GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value != null)
            {
                Desc = GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC].Value != null ? GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC].Value.ToString() : "";
                GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC];
            }
        }
        private void GridViewSelectProcedures_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ProceduresSelectTableColoumn.DESC &&
               GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.NAME].Value != null &&
               GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC].Value != null)
            {
                if (Desc != GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC].Value.ToString())
                {
                    Desc = GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.DESC].Value.ToString();
                }
            }
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ProceduresSelectTableColoumn.FEE &&
               GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.NAME].Value != null &&
               GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value != null)
            {
                if (Fee != double.Parse(GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value.ToString()))
                {
                    Fee = double.Parse(GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value.ToString());  //GetUpdatedFee(long.Parse(GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.PID].Value.ToString()));
                    GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value = Convert.ToDouble(GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.FEE].Value);
                }
            }
        }
        private void GridViewSelectProcedures_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3 && (GridViewSelectProcedures.Rows.Count) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.NAME].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        bool IsActive = (bool)GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.ISACTIVE].Value;
                        long UnCheckProcedure = long.Parse(GridViewSelectProcedures.Rows[e.RowIndex].Cells[(int)ProceduresSelectTableColoumn.PID].Value.ToString());
                        GridViewSelectProcedures.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSelectProcedures.Rows.RemoveAt(e.RowIndex);
                        ProcedureIds.Remove(UnCheckProcedure);
                        if (IsActive)
                        {
                            int Index = ProcedureCheckedListBox.Items.IndexOf(Global.ProcedureDetailList.FirstOrDefault(x => x.Id == UnCheckProcedure));
                            if (Index > -1)
                            {
                                ProcedureCheckedListBox.SetItemChecked(Index, false);
                            }
                        }
                    }
                }
                if (e.ColumnIndex == 1 && (GridViewSelectProcedures.Rows.Count) != e.RowIndex)
                {
                    GridViewSelectProcedures.Focus();
                    GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures.Rows[e.RowIndex].Cells[1];
                    GridViewSelectProcedures.CurrentCell.Selected = true;
                }
                if (e.ColumnIndex == 2 && (GridViewSelectProcedures.Rows.Count) != e.RowIndex)
                {
                    GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures.Rows[e.RowIndex].Cells[2];
                    GridViewSelectProcedures.CurrentCell.Selected = true;
                }
            }
        }
        private void BtnSelectProceduresDone_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                SelectedProcedureIds = new DataGridViewComboBoxCell();
                SelectedProcedureFees = new DataGridViewComboBoxCell();
                SelectedProcedureDisc = new DataGridViewComboBoxCell();
                SelectedProcedureNames = string.Empty;
                SelectedProcedureDiscp = string.Empty;
                Newids = new List<long>();
                int i = 0;
                foreach (DataGridViewRow row in GridViewSelectProcedures.Rows)
                {
                    if (row.Cells[(int)ProceduresSelectTableColoumn.NAME].Value != null)
                    {
                        SelectedProcedureIds.Items.Add(row.Cells[(int)ProceduresSelectTableColoumn.PID].Value.ToString());
                        SelectedProcedureFees.Items.Add(row.Cells[(int)ProceduresSelectTableColoumn.FEE].Value.ToString());
                        SelectedProcedureDisc.Items.Add(string.IsNullOrEmpty(row.Cells[(int)ProceduresSelectTableColoumn.DESC].Value?.ToString()) ? "" : row.Cells[(int)ProceduresSelectTableColoumn.DESC].Value.ToString());
                        MedicalProcedure SelectedProcedure = MedicalProcedureManager.GetMedicalProcedureById(long.Parse(GridViewSelectProcedures.Rows[i].Cells[(int)ProceduresSelectTableColoumn.PID].Value.ToString()));
                        SelectedProcedureNames = string.IsNullOrEmpty(SelectedProcedureNames) ? SelectedProcedure.Name : SelectedProcedureNames + ",\n" + SelectedProcedure.Name;
                        Newids.Add(long.Parse(row.Cells[(int)ProceduresSelectTableColoumn.PID].Value.ToString()));
                    }
                    i++;
                }
                this.Close();
            }
        }
        private void BtnSelectProceduresCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            GetProceduresId();
            LoadProcedureWithFilter();
            LoadProcedures();
            ProcedureSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelectProceduresDone.PerformClick();
            }
            if (keyData == (Keys.Escape))
            {
                BtnSelectProceduresCancel.PerformClick();
                return true;
            }
            if(keyData == (Keys.F3))
            {
                BtnProceduresNew.PerformClick();
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.BtnSelectProceduresDone)
            {
                GridViewSelectProcedures.Select();
                GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures[1, 0];
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnSelectProceduresCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridViewSelectProcedures.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewSelectProcedures.CurrentCell.ColumnIndex == (int)ConsultedConsultationTableColumn.FEE)
                    {
                        if (GridViewSelectProcedures.CurrentCell.RowIndex != GridViewSelectProcedures.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            GridViewSelectProcedures.CurrentCell = null;
                            BtnSelectProceduresDone.Select();
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSelectProcedures.CurrentCell.ColumnIndex == (int)ConsultedConsultationTableColumn.DESC)
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

        private void InsertTickedProcedures(long ProcedureId)
        {
            GridViewSelectProcedures.Rows.Add();
            MedicalProcedure Proced = Global.ProcedureDetailList.FirstOrDefault(x => x.Id == ProcedureId);
            if (Proced != null)
            {
                int p = GridViewSelectProcedures.Rows.Count - 1;
                GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.NAME].Value = Proced.Name;
                GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.DESC].Value = Proced.Description;
                GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.PROCDNAME].Value = Proced.Name;
                GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.FEE].Value = Proced.Fee;
                GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.PID].Value = Proced.Id;
                GridViewSelectProcedures.Rows[p].Cells[(int)ProceduresSelectTableColoumn.ISACTIVE].Value = Proced.IsActive;
            }
        }
        private void RemoveUnTickedProcedures(string ProcedureId)
        {
            int Index = -1;
            for (int i = 0; i < GridViewSelectProcedures.Rows.Count; i++)
            {
                if (GridViewSelectProcedures.Rows[i].Cells[(int)ProceduresSelectTableColoumn.PID].Value.ToString() == ProcedureId)
                {
                    Index = GridViewSelectProcedures.Rows[i].Index;
                }
                if (Index > -1)
                {
                    GridViewSelectProcedures.Rows.RemoveAt(Index);
                    break;
                }
            }
        }

        private void GridViewSelectProcedures_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.CellStyle.BackColor = System.Drawing.Color.White;
        }

        private void GridViewSelectProcedures_Enter(object sender, EventArgs e)
        {
            if (GridViewSelectProcedures.Rows.Count > 1)
            {
                GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures[(int)ConsultedConsultationTableColumn.DESC, 0];
            }
        }

        private void GridViewSelectProcedures_SelectionChanged(object sender, EventArgs e)
        {
            GridViewSelectProcedures.ClearSelection();
        }

        private void BtnRefreshProcedure_Click(object sender, EventArgs e)
        {
            BtnRefreshMedicalProcedure();
        }
        public void BtnRefreshMedicalProcedure()
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.AllProcesdureDetailList = MedicalProcedureManager.Instance.ListMedicalProcedureByCompanyId(Global.Company.CompanyId);
            LoadProcedureWithFilter();
            ProcedureSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }

        private void ProcedureCheckedListBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewSelectProcedures.Rows.Count > 0)
                {
                    GridViewSelectProcedures.Select();
                    GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures[(int)ConsultedConsultationTableColumn.DESC, 0];
                    GridViewSelectProcedures.BeginEdit(true);
                }
                else
                {
                    BtnSelectProceduresDone.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ProcedureSearchTextBox.Select();

            }
        }

        private void BtnSelectProceduresDone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                ProcedureSearchTextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSelectProcedures.Rows.Count > 0)
                {
                    GridViewSelectProcedures.Select();
                    GridViewSelectProcedures.CurrentCell = GridViewSelectProcedures[(int)ProceduresSelectTableColoumn.FEE, GridViewSelectProcedures.Rows.Count - 1];
                    GridViewSelectProcedures.BeginEdit(true);
                }
                else
                {
                    ProcedureCheckedListBox.Select();
                }
            }
        }

        private void BtnProceduresNew_Click(object sender, EventArgs e)
        {
            FormProcedures FormProcedures = new FormProcedures(true);
            FormProcedures.CreateMedicalProcedureOnLoad = true;
            FormProcedures.ShowDialog(this);
            BtnRefreshMedicalProcedure();
        }
    }
}
