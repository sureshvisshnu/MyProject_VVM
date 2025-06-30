using fa.model.Hms.Master;
using fa.views.hms.Masters;
using fa.views.hms.patient;
using Fa.api.Hms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.hms.helper
{
    public enum SymptomsSelectTableColoumn
    {
        NAME, DESC, REMOVE, ID, SID, SYMPTOMNAME, ISACTIVE
    }
    public partial class FormSelectSymptom : FormPatientBase
    {
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete Diagnosis {0}?";
        List<long> Symptomsids = new List<long>();
        public List<long>? Oldids = new List<long>();
        public List<long>? Newids = new List<long>();
        public bool isDirty = false;
        public DataGridViewComboBoxCell? SelectedSymptomsids;
        public DataGridViewComboBoxCell? SelectedSymptomsDiscp;
        public string? SelectedSymptomsNames;
        FormPatientBase parent = null!;
        public FormSelectSymptom(Object Sender)
        {
            if (Sender is FormConsulting)
            {
                parent = (FormConsulting)Sender;
            }
            InitializeComponent();
        }
        private void SelectSymptom_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            GetSymptomIds();
            RefreshDiagnosis();
            LoadSelectedSymptoms();
            DiagnosisSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void GetSymptomIds()
        {
            Symptomsids = new List<long>();
            Oldids = new List<long>();
            Newids = new List<long>();
            if (SelectedSymptomsids != null && SelectedSymptomsids.Items.Count > 0)
            {
                foreach (var Id in SelectedSymptomsids.Items)
                {
                    Oldids.Add(long.Parse(Id.ToString()!));
                    Newids.Add(long.Parse(Id.ToString()!));
                    Symptomsids.Add(long.Parse(Id.ToString()!));
                }
            }
        }
        private void LoadSymptomsWithFilter()
        {
            string FilterString = DiagnosisSearchTextBox.Text.Trim();
            DiagnosisCheckedListBox.Items.Clear();
            foreach (var lDiagnosis in Global.SymptomDetailList)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lDiagnosis.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    DiagnosisCheckedListBox.Items.Add(lDiagnosis, Symptomsids.Contains(lDiagnosis.Id) ? true : false);
                }
            }
            if (DiagnosisCheckedListBox.Items.Count > 0)
            {
                DiagnosisCheckedListBox.SelectedIndex = 0;
            }
        }
        private void LoadSelectedSymptoms()
        {
            if (Symptomsids.Count > 0)
            {
                for (int s = 0; s < Symptomsids.Count; s++)
                {
                    Symptom Symptoms = Global.AllSymptomDetailList.FirstOrDefault(x => x.Id == Symptomsids[s])!;
                    if (Symptoms != null)
                    {
                        GridViewSelectedSymptoms.Rows.Add();
                        GridViewSelectedSymptoms.Rows[s].Cells[(int)SymptomsSelectTableColoumn.NAME].Value = Symptoms.Name;
                        GridViewSelectedSymptoms.Rows[s].Cells[(int)SymptomsSelectTableColoumn.DESC].Value = SelectedSymptomsDiscp.Items[s].ToString();
                        GridViewSelectedSymptoms.Rows[s].Cells[(int)SymptomsSelectTableColoumn.SYMPTOMNAME].Value = Symptoms.Name;
                        GridViewSelectedSymptoms.Rows[s].Cells[(int)SymptomsSelectTableColoumn.SID].Value = Symptoms.Id;
                        GridViewSelectedSymptoms.Rows[s].Cells[(int)SymptomsSelectTableColoumn.ISACTIVE].Value = Symptoms.IsActive;

                    }
                }
            }
        }
        private void DiagnosisCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index > -1)
            {
                var item = (Symptom)DiagnosisCheckedListBox.Items[e.Index];
                if (!DiagnosisCheckedListBox.GetItemChecked(e.Index))
                {
                    if (!Symptomsids.Contains(item.Id))
                    {
                        Symptomsids.Add(item.Id);
                        InsertTickedSymptoms(item.Id);
                    }
                }
                else
                {
                    Symptomsids.Remove(item.Id);
                    RemoveUnTickedSymptoms(item.Id.ToString());
                }
            }
        }
        private void BtnSymptomsDone_Click(object sender, EventArgs e)
        {
            SelectedSymptomsNames = string.Empty;
            SelectedSymptomsids = new DataGridViewComboBoxCell();
            SelectedSymptomsDiscp = new DataGridViewComboBoxCell();
            Newids = new List<long>();
            int i = 0;
            foreach (DataGridViewRow row in GridViewSelectedSymptoms.Rows)
            {
                if (row.Cells[(int)SymptomsSelectTableColoumn.NAME].Value != null)
                {
                    SelectedSymptomsids.Items.Add(row.Cells[(int)SymptomsSelectTableColoumn.SID].Value.ToString());
                    SelectedSymptomsDiscp.Items.Add(string.IsNullOrEmpty(row.Cells[(int)SymptomsSelectTableColoumn.DESC].Value?.ToString()) ? "" : row.Cells[(int)SymptomsSelectTableColoumn.DESC].Value.ToString());
                    Symptom Symptoms = SymptomsManager.Instance.GetSymptomsById(long.Parse(GridViewSelectedSymptoms.Rows[i].Cells[(int)SymptomsSelectTableColoumn.SID].Value.ToString()!));
                    if (Symptoms != null)
                    {
                        SelectedSymptomsNames = string.IsNullOrEmpty(SelectedSymptomsNames) ? Symptoms.Name : SelectedSymptomsNames + ",\n" + Symptoms.Name;
                    }
                    else
                    {
                        SelectedSymptomsNames = string.IsNullOrEmpty(SelectedSymptomsNames) ? "[Deleted Symptom]" : SelectedSymptomsNames + ",\n[Deleted Symptom]";
                    }
                    Newids.Add(long.Parse(row.Cells[(int)SymptomsSelectTableColoumn.SID].Value.ToString()!));
                }
                i++;
            }
            this.Close();
        }
        private void BtnSymptomCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            GetSymptomIds();
            LoadSymptomsWithFilter();
            LoadSelectedSymptoms();
            DiagnosisSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            ErrorMsgSelectedSymptom.Text = string.Empty;
            DiagnosisSearchTextBox.Text = string.Empty;
            GridViewSelectedSymptoms.Rows.Clear();
        }
        private void InsertTickedSymptoms(long SymptomId)
        {
            GridViewSelectedSymptoms.Rows.Add();
            Symptom Symptoms = Global.SymptomDetailList.FirstOrDefault(x => x.Id == SymptomId)!;
            if (Symptoms != null)
            {
                int p = GridViewSelectedSymptoms.Rows.Count - 1;
                GridViewSelectedSymptoms.Rows[p].Cells[(int)SymptomsSelectTableColoumn.NAME].Value = Symptoms.Name;
                GridViewSelectedSymptoms.Rows[p].Cells[(int)SymptomsSelectTableColoumn.DESC].Value = Symptoms.Discription;
                GridViewSelectedSymptoms.Rows[p].Cells[(int)SymptomsSelectTableColoumn.SYMPTOMNAME].Value = Symptoms.Name;
                GridViewSelectedSymptoms.Rows[p].Cells[(int)SymptomsSelectTableColoumn.SID].Value = Symptoms.Id;
                GridViewSelectedSymptoms.Rows[p].Cells[(int)SymptomsSelectTableColoumn.ISACTIVE].Value = Symptoms.IsActive;
            }
        }
        private void RemoveUnTickedSymptoms(string SymptomId)
        {
            int Index = -1;
            for (int i = 0; i < GridViewSelectedSymptoms.Rows.Count; i++)
            {
                if (GridViewSelectedSymptoms.Rows[i].Cells[(int)SymptomsSelectTableColoumn.SID].Value.ToString() == SymptomId)
                {
                    Index = GridViewSelectedSymptoms.Rows[i].Index;
                }
                if (Index > -1)
                {
                    GridViewSelectedSymptoms.Rows.RemoveAt(Index);
                    break;
                }
            }
        }
        private void GridViewSelectedSymptoms_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        string Disc = string.Empty;
        private void GridViewSelectedSymptoms_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)SymptomsSelectTableColoumn.DESC &&
                GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.NAME].Value != null)
            {
                Disc = GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC].Value == null ? string.Empty : GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC].Value.ToString()!;
                GridViewSelectedSymptoms.CurrentCell = GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC];
            }
        }
        private void GridViewSelectedSymptoms_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2 && (GridViewSelectedSymptoms.Rows.Count) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.NAME].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        bool IsActive = (bool)GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.ISACTIVE].Value;
                        long UnCheckSymptom = long.Parse(GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.SID].Value.ToString()!);
                        GridViewSelectedSymptoms.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSelectedSymptoms.Rows.RemoveAt(e.RowIndex);
                        Symptomsids.Remove(UnCheckSymptom);
                        if (IsActive)
                        {
                            int Index = DiagnosisCheckedListBox.Items.IndexOf(Global.SymptomDetailList.FirstOrDefault(x => x.Id == UnCheckSymptom)!);
                            if (Index > -1)
                            {
                                DiagnosisCheckedListBox.SetItemChecked(Index, false);
                            }
                        }
                    }
                }
                if (e.ColumnIndex == 1 && (GridViewSelectedSymptoms.Rows.Count) != e.RowIndex)
                {
                    GridViewSelectedSymptoms.Focus();
                    GridViewSelectedSymptoms.CurrentCell = GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[1];
                    GridViewSelectedSymptoms.CurrentCell.Selected = true;
                }
            }
        }
        private void GridViewSelectedSymptoms_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)SymptomsSelectTableColoumn.DESC &&
               GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.NAME].Value != null &&
               GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC].Value != null)
            {
                if (Disc != GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC].Value.ToString())
                {
                    Disc = GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC].Value.ToString()!;
                }
            }
        }
        private void GridViewSelectedSymptoms_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.NAME].ReadOnly = true;
            GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC].ReadOnly = true;
            GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.REMOVE].ReadOnly = true;
            if (GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.NAME].Value != null)
            {
                GridViewSelectedSymptoms.Rows[e.RowIndex].Cells[(int)SymptomsSelectTableColoumn.DESC].ReadOnly = false;
            }
        }
        private void GridViewSelectedSymptoms_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.CellStyle.BackColor = Color.White;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnNewDiagnosis.PerformClick();
            }
            if (keyData == (Keys.F8))
            {
                BtnSymptomDone.PerformClick();
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.BtnSymptomDone)
            {
                GridViewSelectedSymptoms.Select();
                GridViewSelectedSymptoms.CurrentCell = GridViewSelectedSymptoms[1, 0];
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnSymptomCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridViewSelectedSymptoms.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewSelectedSymptoms.CurrentCell.ColumnIndex == (int)SymptomsSelectTableColoumn.REMOVE)
                    {
                        if (GridViewSelectedSymptoms.CurrentCell.RowIndex != GridViewSelectedSymptoms.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            GridViewSelectedSymptoms.CurrentCell = null;
                            BtnSymptomDone.Select();
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSelectedSymptoms.CurrentCell?.ColumnIndex == (int)SymptomsSelectTableColoumn.DESC)
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void GridViewSelectedSymptoms_SelectionChanged(object sender, EventArgs e)
        {
            GridViewSelectedSymptoms.ClearSelection();
        }
        private void GridViewSelectedSymptoms_Enter(object sender, EventArgs e)
        {
            if (GridViewSelectedSymptoms.Rows.Count > 1)
            {
                GridViewSelectedSymptoms.CurrentCell = GridViewSelectedSymptoms[1, 0];
            }
        }
        private void BtnRefreshDiagnosis_Click(object sender, EventArgs e)
        {
            RefreshDiagnosis();
        }
        public void RefreshDiagnosis()
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.AllSymptomDetailList = SymptomsManager.Instance.ListSymptomByCompanyId(Global.Company.CompanyId);
            LoadSymptomsWithFilter();
            DiagnosisSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void DiagnosisSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadSymptomsWithFilter();
        }

        private void DiagnosisCheckedListBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewSelectedSymptoms.Rows.Count > 0)
                {
                    GridViewSelectedSymptoms.Select();
                    GridViewSelectedSymptoms.CurrentCell = GridViewSelectedSymptoms[1, 0];
                    GridViewSelectedSymptoms.BeginEdit(true);
                }
                else
                {
                    BtnSymptomDone.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DiagnosisSearchTextBox.Select();

            }
        }

        private void BtnSymptomDone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                DiagnosisSearchTextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSelectedSymptoms.Rows.Count > 0)
                {
                    GridViewSelectedSymptoms.Select();
                    GridViewSelectedSymptoms.CurrentCell = GridViewSelectedSymptoms[1, GridViewSelectedSymptoms.Rows.Count - 1];
                    GridViewSelectedSymptoms.BeginEdit(true);
                }
                else
                {
                    DiagnosisCheckedListBox.Select();
                }
            }
        }
        private void BtnNewDiagnosis_Click(object sender, EventArgs e)
        {
            FormSymptom formSymptom = new FormSymptom();
            formSymptom.CreateSymptomOnLoad = true;
            formSymptom.ShowDialog();
            RefreshDiagnosis();
        }
    }
}
