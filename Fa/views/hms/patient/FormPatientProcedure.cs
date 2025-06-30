using fa.api.Hms;
using fa.common;
using fa.model.hms.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VisioForge.Libs.MediaFoundation.OPM;

namespace fa.views.hms.patient
{
    public enum PatientProcedureGridViewColoumn
    {
        SNO, DATE, PERFORMBY, NOTE, STATUS, REMOVE, ID
    }

    public partial class FormPatientProcedure : FormPatientBase
    {
        public static string Grid_MantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_EmptyErrorMsg = "Please enter Procedure progress details.";
        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want to Exit?";
        public static string CancelConfirmText = "There are unsaved changes, Do you want to cancel?";
        public static string SaveSuccessMsg = "Saved...";

        public long PatientId = 0L;
        public long ProcedureId = 0L;
        public PatientTypes PatientType;
        public string DisplayedText;
        bool isSaved = true;
        FormPatientBase parent = null;
        public FormPatientProcedure(object sender)
        {
            InitializeComponent();
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }
        public void LoadPatientProcedureDetails()
        {
            ConsultedProcedure ConsultedProceduresFromDB = ConsultationNoteManager.Instance.GetConsultedProceduresById(ProcedureId);
            if (ConsultedProceduresFromDB != null)
            {
                TextBoxPatientProcedureName.Text = ConsultedProceduresFromDB.Name != null ? ConsultedProceduresFromDB.Name.ToString() : string.Empty;
                TextBoxPatientProcedureRequestBy.Text = ConsultedProceduresFromDB.RequestedBy != null ? ConsultedProceduresFromDB.RequestedBy.Name : string.Empty;
                TextBoxPatientProcedureRequestOn.Text = ConsultedProceduresFromDB.Date.ToString(Global.Company.DateFormat); //String.Format("{0:d}", ConsultedProceduresFromDB.Date);
                TextBoxPatientProcedureDisc.Text = ConsultedProceduresFromDB.Description != null ? ConsultedProceduresFromDB.Description.ToString() : string.Empty;
                TextBoxStatus.Text = ConsultedProceduresFromDB.ProStatus.ToString();
                FillGridView(ProcedureId);
            }
        }
        private void ResetForm()
        {
            isSaved = true;
            PatientProcedureInfo.Clear();
            TextBoxPatientProcedureName.ResetText();
            TextBoxPatientProcedureDisc.ResetText();
            TextBoxPatientProcedureRequestBy.ResetText();
            TextBoxPatientProcedureRequestOn.ResetText();
            TextBoxStatus.ResetText();
            DataGridViewPatientProcedure.Rows.Clear();
        }
        public void FillGridView(long ConsultedProcedurId)
        {
            IList<ConsultedProcedureHistory> ConsultedProcedureHistory = ConsultationNoteManager.Instance.ListConsultedProcedureHistoryById(ConsultedProcedurId, Global.Company.CompanyId);
            if (ConsultedProcedureHistory != null && ConsultedProcedureHistory.Count > 0)
            {
                TextBoxStatus.Text = ConsultedProcedureHistory.Last().ProStatus.ToString();
                int p = 0;
                foreach (ConsultedProcedureHistory ConsultedProcedures in ConsultedProcedureHistory)
                {
                    DataGridViewPatientProcedure.Rows.Add();
                    DataGridViewPatientProcedure.Rows[p].Cells[(int)PatientProcedureGridViewColoumn.DATE].Value = ConsultedProcedures.PerformOn;
                    DataGridViewPatientProcedure.Rows[p].Cells[(int)PatientProcedureGridViewColoumn.PERFORMBY].Value = ConsultedProcedures.PerformedBy != null ? ConsultedProcedures.PerformedBy.Name : string.Empty;
                    DataGridViewPatientProcedure.Rows[p].Cells[(int)PatientProcedureGridViewColoumn.NOTE].Value = ConsultedProcedures.Note != null ? ConsultedProcedures.Note : string.Empty;
                    DataGridViewPatientProcedure.Rows[p].Cells[(int)PatientProcedureGridViewColoumn.STATUS].Value = ConsultedProcedures.ProStatus.ToString();
                    DataGridViewPatientProcedure.Rows[p].Cells[(int)PatientProcedureGridViewColoumn.ID].Value = ConsultedProcedures.Id;
                    p++;
                }
                ReSequence();
                if (ConsultedProcedureHistory.Last().ProStatus == ProcedureStatus.CANCEL ||
                ConsultedProcedureHistory.Last().ProStatus == ProcedureStatus.COMPLETED)
                {
                    DataGridViewPatientProcedure.ReadOnly = true;
                    BtnSave.Enabled = false;
                    BtnCancel.Enabled = false;
                }
            }


        }
        public static void LoadEnumCombo(ComboBox cbo)
        {
            cbo.DataSource = Enum.GetValues(typeof(ProcedureStatus));
        }
        private void FormPatientProcedure_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            LoadProcedure();
            this.Cursor = Cursors.Default;
        }
        private void LoadProcedure()
        {
            ResetForm();
            PatientProcedureInfo.Type = PatientType;
            PatientProcedureInfo.PatientId = PatientId;
            LoadPatientProcedureDetails();
            if (DataGridViewPatientProcedure.Rows.Count > 0)
            {
                DataGridViewPatientProcedure.Select();
                
                DataGridViewPatientProcedure.BeginInvoke(new MethodInvoker(delegate ()
                {
                    if (isSaved)
                    {
                        DataGridViewPatientProcedure.CurrentCell = DataGridViewPatientProcedure[(int)PatientProcedureGridViewColoumn.NOTE, DataGridViewPatientProcedure.Rows.Count - 1];
                        isSaved = true;
                    }
                }));
            }
            ResetDirtyFlag();
        }
        private void CancelPatientProcedureChanges()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    return;
                }
                LoadProcedure();
            }
            this.Cursor = Cursors.Default;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            if (keyData != Keys.Shift && keyData == Keys.Tab && ActiveControl == BtnSave)
            {
                DataGridViewPatientProcedure.Select();
                DataGridViewPatientProcedure.CurrentCell = DataGridViewPatientProcedure[(int)PatientProcedureGridViewColoumn.NOTE, 0];
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && ActiveControl == BtnSave)
            {
                DataGridViewPatientProcedure.Select();
                DataGridViewPatientProcedure.CurrentCell = DataGridViewPatientProcedure[(int)PatientProcedureGridViewColoumn.STATUS, DataGridViewPatientProcedure.Rows.Count - 1];
                DataGridViewPatientProcedure.BeginEdit(true);
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnSave)
            {
                BtnCancel.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnSave)
            {
                BtnExit.Select();
                return true;
            }
            if (keyData == Keys.Right && ActiveControl == BtnCancel)
            {
                BtnSave.Select();
                return true;
            }
            if (keyData == Keys.Left && ActiveControl == BtnExit)
            {
                BtnSave.Select();
                return true;
            }
            if (keyData == Keys.Tab && ActiveControl == BtnCancel)
            {
                BtnExit.Select();
                return true;
            }
            try
            {
                if (DataGridViewPatientProcedure.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewPatientProcedure.CurrentCell.ColumnIndex == (int)PatientProcedureGridViewColoumn.STATUS)
                    {
                        if (DataGridViewPatientProcedure.Rows.Count - 1 == DataGridViewPatientProcedure.CurrentCell.RowIndex)
                        {
                            BtnSave.Select();
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewPatientProcedure.CurrentCell.ColumnIndex == (int)PatientProcedureGridViewColoumn.NOTE)
                    {
                        if (DataGridViewPatientProcedure.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}");
                        }
                        else
                        {
                            BtnSave.Select();
                        }
                    }
                }
            }
            catch
            {
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CancelPatientProcedureChanges();
        }
        private void DataGridViewPatientProcedure_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ErrorMsgProcedure.Text = "";
            if (e.RowIndex > -1 && !DataGridViewPatientProcedure.ReadOnly)
            {
                if (e.ColumnIndex == (int)PatientProcedureGridViewColoumn.REMOVE && (DataGridViewPatientProcedure.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[0].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        DataGridViewPatientProcedure.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        DataGridViewPatientProcedure.Rows.RemoveAt(e.RowIndex);
                        Row_Removed();

                    }
                }
            }
        }
        private void Row_Removed()
        {
            ReSequence();
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewPatientProcedure.Rows.Count; i++)
            {
                DataGridViewPatientProcedure.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void DataGridViewPatientProcedure_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.SNO].ReadOnly = true;
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.DATE].ReadOnly = true;
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.PERFORMBY].ReadOnly = true;
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.REMOVE].ReadOnly = true;
        }
        private void DataGridViewPatientProcedure_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DataGridViewPatientProcedure_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).FormattingEnabled = true;

                if (DataGridViewPatientProcedure.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(DataGridViewPatientProcedure_KeyPress1);
            }
        }
        private void DataGridViewPatientProcedure_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)DataGridViewPatientProcedure.EditingControl).DroppedDown = false;
        }
        public enum ProcedureCombo
        {
            INPROGRESS, COMPLETED, CANCEL
        }
        private void DataGridViewPatientProcedure_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var values = (from Enum en in Enum.GetValues(typeof(ProcedureCombo))
                          select new { Id = en, Name = (ProcedureCombo)en }).ToList();
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.SNO].Value = DataGridViewPatientProcedure.Rows.Count;
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.DATE].Value = Global.getTransactionDate();
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.PERFORMBY].Value = Global.User.Name;
            (DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.STATUS] as DataGridViewComboBoxCell).DataSource = null;
            (DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.STATUS] as DataGridViewComboBoxCell).DataSource = values.ToList();
            (DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.STATUS] as DataGridViewComboBoxCell).ValueMember = "Id";
            (DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.STATUS] as DataGridViewComboBoxCell).DisplayMember = "Name";
            DataGridViewPatientProcedure.Rows[e.RowIndex].Cells[(int)PatientProcedureGridViewColoumn.REMOVE].Value = "X";
        }
        private IList<ConsultedProcedureHistory> GetProcedureFromForm()
        {
            IList<ConsultedProcedureHistory> lConsultedProcedureHistory = new List<ConsultedProcedureHistory>();
            for (int i = 0; i < DataGridViewPatientProcedure.Rows.Count - 1; i++)
            {
                ConsultedProcedureHistory ConsultedProcedureHistory = new ConsultedProcedureHistory();
                ConsultedProcedureHistory.Id = DataGridViewPatientProcedure.Rows[i].Cells[(int)PatientProcedureGridViewColoumn.ID].Value != null ? (long)DataGridViewPatientProcedure.Rows[i].Cells[(int)PatientProcedureGridViewColoumn.ID].Value : 0L;
                ConsultedProcedureHistory.ConsultedProcedureId = ProcedureId;
                ConsultedProcedureHistory.CompanyId = Global.Company.CompanyId;
                ConsultedProcedureHistory.Note = DataGridViewPatientProcedure.Rows[i].Cells[(int)PatientProcedureGridViewColoumn.NOTE].Value != null ? DataGridViewPatientProcedure.Rows[i].Cells[(int)PatientProcedureGridViewColoumn.NOTE].Value.ToString() : "";
                ConsultedProcedureHistory.PerformedById = Global.User.UserId;
                ConsultedProcedureHistory.PerformOn = (DateTime)DataGridViewPatientProcedure.Rows[i].Cells[(int)PatientProcedureGridViewColoumn.DATE].Value;
                string lProcedureCombo = DataGridViewPatientProcedure.Rows[i].Cells[(int)PatientProcedureGridViewColoumn.STATUS].Value.ToString();
                ConsultedProcedureHistory.ProStatus = lProcedureCombo == "INPROGRESS" ? ProcedureStatus.INPROGRESS : lProcedureCombo == "COMPLETED" ? ProcedureStatus.COMPLETED : ProcedureStatus.CANCEL;
                lConsultedProcedureHistory.Add(ConsultedProcedureHistory);
            }
            return lConsultedProcedureHistory;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            ErrorMsgProcedure.Text = "";
            this.Cursor = Cursors.WaitCursor;
            if (ValidateForm())
            {
                IList<ConsultedProcedureHistory> ConsultedProcedureHistory = GetProcedureFromForm();
                if (ConsultedProcedureHistory != null)
                {
                    bool SaveSuccess = ConsultationNoteManager.Instance.UpdateConsultedProcedureDetails(ConsultedProcedureHistory, ProcedureId);
                    LoadProcedure();
                    if (SaveSuccess) { ErrorMsgProcedure.Text = SaveSuccessMsg; }
                    isSaved = false;
                }
            }
            this.Cursor = Cursors.Default;
        }
        private Boolean ValidateForm()
        {
            int Count = DataGridViewPatientProcedure.Rows.Count;
            if (Count > 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 3; j < 5; j++)
                    {
                        if (DataGridViewPatientProcedure.Rows[i].Cells[j].Value == null)
                        {
                            DataGridViewPatientProcedure.Select();
                            DataGridViewPatientProcedure.CurrentCell = DataGridViewPatientProcedure[j, i];
                            DataGridViewPatientProcedure.BeginEdit(true);
                            ErrorMsgProcedure.Text = string.Format(Grid_MantatoryFiledErrorMsg, DataGridViewPatientProcedure.Columns[j].HeaderText);
                            return false;
                        }
                    }
                }
            }
            else
            {
                DataGridViewPatientProcedure.Select();
                DataGridViewPatientProcedure.CurrentCell = DataGridViewPatientProcedure[(int)PatientProcedureGridViewColoumn.NOTE, 0];
                DataGridViewPatientProcedure.BeginEdit(true);
                ErrorMsgProcedure.Text = Grid_EmptyErrorMsg;
                return false;
            }
            return true;
        }
    }
}
