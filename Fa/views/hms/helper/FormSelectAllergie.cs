using fa;
using fa.model.Hms.Master;
using fa.views.hms;
using fa.views.hms.Masters;
using fa.views.hms.patient;
using Fa.api.Hms;
using Fa.views.hms.Masters;
using FADataAccessLibrary.Api.Hms;
using FADataAccessLibrary.Model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Libs.NDI;

namespace Fa.views.hms.helper
{
    public enum AllergiesSelectTableColoumn
    {
        NAME, DESC, REMOVE, ID, SID, AllergieNAME, ISACTIVE
    }
    public partial class FormSelectAllergie : FormPatientBase
    {
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete Allergie {0}?";
        List<long> Allergiesids = new List<long>();
        public DataGridViewComboBoxCell SelectedAllergiesids;
        public DataGridViewComboBoxCell SelectedAllergiesDiscp;
        public string SelectedAllergiesNames;
        FormPatientBase parent = null;
        public FormSelectAllergie(Object Sender)
        {
            if (Sender is FormConsulting)
            {
                parent = (FormConsulting)Sender;
            }
            InitializeComponent();
        }

        private void SelectAllergie_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            GetAllergieIds();
            LoadAllergiesWithFilter();
            LoadSelectedAllergies();
            AllergieSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void GetAllergieIds()
        {
            Allergiesids = new List<long>();
            if (SelectedAllergiesids != null && SelectedAllergiesids.Items.Count > 0)
            {
                foreach (var Id in SelectedAllergiesids.Items)
                {
                    Allergiesids.Add(long.Parse(Id.ToString()));
                }
            }
        }
        private void LoadAllergiesWithFilter()
        {
            string FilterString = AllergieSearchTextBox.Text.Trim();
            AllergieCheckedListBox.Items.Clear();
            foreach (var lAllergie in Global.AllergieDetailList)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lAllergie.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    AllergieCheckedListBox.Items.Add(lAllergie, Allergiesids.Contains(lAllergie.Id) ? true : false);
                }
            }
            if (AllergieCheckedListBox.Items.Count > 0)
            {
                AllergieCheckedListBox.SelectedIndex = 0;
            }
        }
        private void LoadSelectedAllergies()
        {
            if (Allergiesids.Count > 0)
            {
                for (int s = 0; s < Allergiesids.Count; s++)
                {
                    Allergie Allergies = Global.AllAllergieDetailList.FirstOrDefault(x => x.Id == Allergiesids[s]);
                    if (Allergies != null)
                    {
                        GridViewSelectedAllergie.Rows.Add();
                        GridViewSelectedAllergie.Rows[s].Cells[(int)AllergiesSelectTableColoumn.NAME].Value = Allergies.Name;
                        GridViewSelectedAllergie.Rows[s].Cells[(int)AllergiesSelectTableColoumn.DESC].Value = SelectedAllergiesDiscp.Items[s].ToString();
                        GridViewSelectedAllergie.Rows[s].Cells[(int)AllergiesSelectTableColoumn.AllergieNAME].Value = Allergies.Name;
                        GridViewSelectedAllergie.Rows[s].Cells[(int)AllergiesSelectTableColoumn.SID].Value = Allergies.Id;
                        GridViewSelectedAllergie.Rows[s].Cells[(int)AllergiesSelectTableColoumn.ISACTIVE].Value = Allergies.IsActive;

                    }
                }
            }
        }
        private void AllergieCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index > -1)
            {
                var item = (Allergie)AllergieCheckedListBox.Items[e.Index];
                if (!AllergieCheckedListBox.GetItemChecked(e.Index))
                {
                    if (!Allergiesids.Contains(item.Id))
                    {
                        Allergiesids.Add(item.Id);
                        InsertTickedAllergies(item.Id);
                    }
                }
                else
                {
                    Allergiesids.Remove(item.Id);
                    RemoveUnTickedAllergies(item.Id.ToString());
                }
            }
        }
        private void BtnAllergiesDone_Click(object sender, EventArgs e)
        {
            SelectedAllergiesNames = string.Empty;
            SelectedAllergiesids = new DataGridViewComboBoxCell();
            SelectedAllergiesDiscp = new DataGridViewComboBoxCell();
            int i = 0;
            foreach (DataGridViewRow row in GridViewSelectedAllergie.Rows)
            {
                if (row.Cells[(int)AllergiesSelectTableColoumn.NAME].Value != null)
                {
                    SelectedAllergiesids.Items.Add(row.Cells[(int)AllergiesSelectTableColoumn.SID].Value.ToString());
                    SelectedAllergiesDiscp.Items.Add(string.IsNullOrEmpty(row.Cells[(int)AllergiesSelectTableColoumn.DESC].Value?.ToString()) ? "" : row.Cells[(int)AllergiesSelectTableColoumn.DESC].Value.ToString());
                    Allergie Allergies = QuestionManager.Instance.GetAllergiesById(long.Parse(GridViewSelectedAllergie.Rows[i].Cells[(int)AllergiesSelectTableColoumn.SID].Value.ToString()));
                    SelectedAllergiesNames = string.IsNullOrEmpty(SelectedAllergiesNames) ? Allergies.Name : SelectedAllergiesNames + ",\n" + Allergies.Name;
                }
                i++;
            }
            this.Close();
        }
        private void BtnAllergieCancel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            GetAllergieIds();
            LoadAllergiesWithFilter();
            LoadSelectedAllergies();
            AllergieSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            ErrorMsgSelectedAllergie.Text = string.Empty;
            AllergieSearchTextBox.Text = string.Empty;
            GridViewSelectedAllergie.Rows.Clear();
        }
        private void InsertTickedAllergies(long AllergieId)
        {
            GridViewSelectedAllergie.Rows.Add();
            Allergie Allergies = Global.AllergieDetailList.FirstOrDefault(x => x.Id == AllergieId);
            if (Allergies != null)
            {
                int p = GridViewSelectedAllergie.Rows.Count - 1;
                GridViewSelectedAllergie.Rows[p].Cells[(int)AllergiesSelectTableColoumn.NAME].Value = Allergies.Name;
                GridViewSelectedAllergie.Rows[p].Cells[(int)AllergiesSelectTableColoumn.DESC].Value = Allergies.Discription;
                GridViewSelectedAllergie.Rows[p].Cells[(int)AllergiesSelectTableColoumn.AllergieNAME].Value = Allergies.Name;
                GridViewSelectedAllergie.Rows[p].Cells[(int)AllergiesSelectTableColoumn.SID].Value = Allergies.Id;
                GridViewSelectedAllergie.Rows[p].Cells[(int)AllergiesSelectTableColoumn.ISACTIVE].Value = Allergies.IsActive;
            }
        }
        private void RemoveUnTickedAllergies(string AllergieId)
        {
            int Index = -1;
            for (int i = 0; i < GridViewSelectedAllergie.Rows.Count; i++)
            {
                if (GridViewSelectedAllergie.Rows[i].Cells[(int)AllergiesSelectTableColoumn.SID].Value.ToString() == AllergieId)
                {
                    Index = GridViewSelectedAllergie.Rows[i].Index;
                }
                if (Index > -1)
                {
                    GridViewSelectedAllergie.Rows.RemoveAt(Index);
                    break;
                }
            }
        }
        private void GridViewSelectedAllergies_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        string Disc = string.Empty;
        private void GridViewSelectedAllergies_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)AllergiesSelectTableColoumn.DESC &&
                GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.NAME].Value != null)
            {
                Disc = GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC].Value == null ? string.Empty : GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC].Value.ToString();
                GridViewSelectedAllergie.CurrentCell = GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC];
            }
        }
        private void GridViewSelectedAllergies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2 && (GridViewSelectedAllergie.Rows.Count) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.NAME].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        bool IsActive = (bool)GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.ISACTIVE].Value;
                        long UnCheckAllergie = long.Parse(GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.SID].Value.ToString());
                        GridViewSelectedAllergie.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSelectedAllergie.Rows.RemoveAt(e.RowIndex);
                        Allergiesids.Remove(UnCheckAllergie);
                        if (IsActive)
                        {
                            int Index = AllergieCheckedListBox.Items.IndexOf(Global.AllergieDetailList.FirstOrDefault(x => x.Id == UnCheckAllergie));
                            if (Index > -1)
                            {
                                AllergieCheckedListBox.SetItemChecked(Index, false);
                            }
                        }
                    }
                }
                if (e.ColumnIndex == 1 && (GridViewSelectedAllergie.Rows.Count) != e.RowIndex)
                {
                    GridViewSelectedAllergie.Focus();
                    GridViewSelectedAllergie.CurrentCell = GridViewSelectedAllergie.Rows[e.RowIndex].Cells[1];
                    GridViewSelectedAllergie.CurrentCell.Selected = true;
                }
            }
        }
        private void GridViewSelectedAllergies_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)AllergiesSelectTableColoumn.DESC &&
               GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.NAME].Value != null &&
               GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC].Value != null)
            {
                if (Disc != GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC].Value.ToString())
                {
                    Disc = GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC].Value.ToString();
                }
            }
        }
        private void GridViewSelectedAllergies_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.NAME].ReadOnly = true;
            GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC].ReadOnly = true;
            GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.REMOVE].ReadOnly = true;
            if (GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.NAME].Value != null)
            {
                GridViewSelectedAllergie.Rows[e.RowIndex].Cells[(int)AllergiesSelectTableColoumn.DESC].ReadOnly = false;
            }
        }
        private void GridViewSelectedAllergies_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.CellStyle.BackColor = Color.White;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnAllergieDone.PerformClick();
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.BtnAllergieDone)
            {
                GridViewSelectedAllergie.Select();
                GridViewSelectedAllergie.CurrentCell = GridViewSelectedAllergie[1, 0];
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnAllergieCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridViewSelectedAllergie.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewSelectedAllergie.CurrentCell.ColumnIndex == (int)AllergiesSelectTableColoumn.REMOVE)
                    {
                        if (GridViewSelectedAllergie.CurrentCell.RowIndex != GridViewSelectedAllergie.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            GridViewSelectedAllergie.CurrentCell = null;
                            BtnAllergieDone.Select();
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSelectedAllergie.CurrentCell.ColumnIndex == (int)AllergiesSelectTableColoumn.DESC)
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
        private void GridViewSelectedAllergies_SelectionChanged(object sender, EventArgs e)
        {
            GridViewSelectedAllergie.ClearSelection();
        }
        private void GridViewSelectedAllergies_Enter(object sender, EventArgs e)
        {
            if (GridViewSelectedAllergie.Rows.Count > 1)
            {
                GridViewSelectedAllergie.CurrentCell = GridViewSelectedAllergie[1, 0];
            }
        }
        private void BtnRefreshAllergie_Click(object sender, EventArgs e)
        {
            RefreshAllergie();
        }
        public void RefreshAllergie()
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.AllAllergieDetailList = QuestionManager.Instance.ListAllergieByCompanyId(Global.Company.CompanyId);
            LoadAllergiesWithFilter();
            AllergieSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void AllergieSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadAllergiesWithFilter();
        }

        private void AllergieCheckedListBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewSelectedAllergie.Rows.Count > 0)
                {
                    GridViewSelectedAllergie.Select();
                    GridViewSelectedAllergie.CurrentCell = GridViewSelectedAllergie[1, 0];
                    GridViewSelectedAllergie.BeginEdit(true);
                }
                else
                {
                    BtnAllergieDone.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                AllergieSearchTextBox.Select();

            }
        }

        private void BtnAllergieDone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                AllergieSearchTextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSelectedAllergie.Rows.Count > 0)
                {
                    GridViewSelectedAllergie.Select();
                    GridViewSelectedAllergie.CurrentCell = GridViewSelectedAllergie[1, GridViewSelectedAllergie.Rows.Count - 1];
                    GridViewSelectedAllergie.BeginEdit(true);
                }
                else
                {
                    AllergieCheckedListBox.Select();
                }
            }
        }
        private void BtnNewAllergie_Click(object sender, EventArgs e)
        {
            FormQuestion formAllergie = new FormQuestion();
            formAllergie.CreateAllergieOnLoad = true;
            formAllergie.ShowDialog();
            RefreshAllergie();
        }

    }
}
