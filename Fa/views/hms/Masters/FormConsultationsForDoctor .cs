using fa.api.Hms;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace fa.views.hms.Masters
{
    public enum DoctorConsultationTableColumn
    {
        NAME, DISPLAY, DESC, FEE, REMOVE, ID, CID
    }
    public partial class FormConsultationForDoctor : FormBase
    {
        ConsultationManager ConsultationManager = null;
        DoctorConsultationManager DoctorConsultationManager = null;

        public static string Grid_ConfirmRowDeleteText = "Do you want to delete row {0}?";
        public FormConsultationForDoctor()
        {
            ConsultationManager = ConsultationManager.Instance;
            DoctorConsultationManager = DoctorConsultationManager.Instance;
            InitializeComponent();
        }
        private void FormConsultationForDoctor_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadDoctorConsultation();
        }
        private void LoadDoctorConsultation()
        {
            IList<ConsultedDoctorConsultationFee> DoctorConsultation = DoctorConsultationManager.ListDoctorConsultationByConsultantIdCompanyId(Global.User.UserId, Global.Company.CompanyId);
            if (DoctorConsultation.Count > 0)
            {

                int i = 0;
                foreach (ConsultedDoctorConsultationFee Con in DoctorConsultation)
                {
                    //GridViewConsultationDoctor.Rows.Add();
                    //GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.NAME].Value = Con.Consultation.Name;
                    //GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.DISPLAY].Value = Con.Consultation.DisplayAs;
                    //GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.DESC].Value = Con.Consultation.Discription;
                    //GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.FEE].Value = Con.Fee;
                    //GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.ID].Value = Con.Id;
                    //GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.CID].Value = Con.ConsultationId;
                    i++;
                }
            }
        }
        private void BtnConsultationDoctorCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadDoctorConsultation();
        }
        private Boolean Validation()
        {
            ErrorMsgConsultationDoctor.Text = string.Empty;
            if (GridViewConsultationDoctor.Rows.Count < 1)
            {
                ErrorMsgConsultationDoctor.Text = "Please choose consultation.";
                return false;
            }
            for (int i = 0; i < GridViewConsultationDoctor.Rows.Count - 1; i++)
            {
                if (GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.NAME].Value == null)
                {
                    ErrorMsgConsultationDoctor.Text = "Please choose valid consultation.";
                    return false;
                }
            }
            return true;
        }
        private void BtnConsultationDoctorSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                IList<ConsultedDoctorConsultationFee> lDoctorConsultation = new List<ConsultedDoctorConsultationFee>();
                for (int i = 0; i < GridViewConsultationDoctor.Rows.Count - 1; i++)
                {
                    //DoctorConsultation DoctorConsultation = new DoctorConsultation();
                    //DoctorConsultation.ConsultantId = Global.User.UserId;
                    //DoctorConsultation.CompanyId = Global.Company.CompanyId;
                    //DoctorConsultation.ConsultationId = long.Parse(GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.CID].Value.ToString());
                    //DoctorConsultation.Fee = double.Parse(GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.FEE].Value.ToString());
                    //DoctorConsultation.Id = GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.ID].Value != null ? long.Parse(GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.ID].Value.ToString()) : 0L;

                    //if (lDoctorConsultation.FirstOrDefault(x => x.ConsultationId == DoctorConsultation.ConsultationId) == null)
                    //{
                    //    lDoctorConsultation.Add(DoctorConsultation);
                    //}
                    //else
                    //{
                    //    ErrorMsgConsultationDoctor.Text = "Consultation " + GridViewConsultationDoctor.Rows[i].Cells[(int)DoctorConsultationTableColumn.DISPLAY].Value + " is repeated please check it.";
                    //    GridViewConsultationDoctor.CurrentCell = GridViewConsultationDoctor[0, GridViewConsultationDoctor.Rows[i].Index];
                    //    GridViewConsultationDoctor.BeginEdit(true); return;
                    //}
                }
                DoctorConsultationManager.SaveDoctorConsultation(lDoctorConsultation, Global.User, Global.Company);
                ResetForm();
                LoadDoctorConsultation();
                ErrorMsgConsultationDoctor.Text = "Saved Success.";
            }
        }
        private void BtnConsultationDoctorExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GridViewConsultationDoctor_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            IList<Consultation> Consultation = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
            if (Consultation.Count > 0)
            {
                GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.REMOVE].Value = "X";
                (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME] as DataGridViewComboBoxCell).DataSource = null;
                (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME] as DataGridViewComboBoxCell).DataSource = Consultation;
                (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME] as DataGridViewComboBoxCell).ValueMember = "Id";
                (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME] as DataGridViewComboBoxCell).DisplayMember = "Name";
                (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME] as DataGridViewComboBoxCell).AutoComplete = true;
            }
        }
        private void ResetForm()
        {
            ErrorMsgConsultationDoctor.Text = string.Empty;
            GridViewConsultationDoctor.Rows.Clear();
        }
        private void GridViewConsultationDoctor_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.DISPLAY].ReadOnly = true;
            GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.DESC].ReadOnly = true;
            GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.FEE].ReadOnly = true;
            GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.REMOVE].ReadOnly = true;

            if (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME].Value != null)
            {
                GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.FEE].ReadOnly = false;
            }
            if (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.ID].Value != null)
            {
                //ConsultedDoctorConsultationFee ConsCons = ConsultationNoteManager.Instance.GetConsultedConsultationByConsultationsandConsultantId(Global.User.UserId, long.Parse(GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.ID].Value.ToString()));
                //if (ConsCons != null)
                //{
                //    GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME].ReadOnly = true;
                //}
            }
        }

        private void GridViewConsultationDoctor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 4 && (GridViewConsultationDoctor.Rows.Count - 1) != e.RowIndex)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        if (GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.CID].Value != null
                            && GridViewConsultationDoctor.Rows[e.RowIndex].Cells[(int)DoctorConsultationTableColumn.NAME].ReadOnly)
                        {
                            ErrorMsgConsultationDoctor.Text = "Do not delete this its used in some ware else";
                        }
                        else
                        {
                            GridViewConsultationDoctor.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewConsultationDoctor.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                }
            }
        }

        private void GridViewConsultationDoctor_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                Consultation Consultation = ConsultationManager.Instance.GetConsultationByName(GridViewConsultationDoctor.CurrentCell.EditedFormattedValue.ToString(), Global.Company.CompanyId);
                if (Consultation != null)
                {
                    GridViewConsultationDoctor.CurrentCell.Value = Consultation.Id;
                }
            }
        }

        private void GridViewConsultationDoctor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewConsultationDoctor_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.Suggest;
                ((ComboBox)e.Control).FormattingEnabled = true;

                if (GridViewConsultationDoctor.CurrentCell.Value == null)
                {
                    ((ComboBox)e.Control).SelectedIndex = -1;
                }
                e.Control.KeyPress += new KeyPressEventHandler(GridViewConsultationDoctor_KeyPress1);
                ((ComboBox)e.Control).KeyDown += GridViewConsultationDoctor_KeyDown1;

                GridViewConsultationDoctor.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (GridViewConsultationDoctor.CurrentCell.ColumnIndex == 0)
                {
                    ((ComboBox)e.Control).SelectedIndexChanged -= new EventHandler(NameColumnComboSelectionChanged);
                    ((ComboBox)e.Control).SelectedIndexChanged += new EventHandler(NameColumnComboSelectionChanged);

                    ((ComboBox)e.Control).TextChanged -= NameColumnComboTextChanged;
                    ((ComboBox)e.Control).TextChanged += NameColumnComboTextChanged;
                }
            }
        }
        private void GridViewConsultationDoctor_KeyPress1(object sender, KeyPressEventArgs e)
        {
            ((ComboBox)GridViewConsultationDoctor.EditingControl).DroppedDown = false;
        }

        private void GridViewConsultationDoctor_KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && GridViewConsultationDoctor.CurrentCell.ColumnIndex == (int)DoctorConsultationTableColumn.NAME)
            {
                GridViewConsultationDoctor.Rows[GridViewConsultationDoctor.CurrentRow.Index].Cells[(int)DoctorConsultationTableColumn.NAME].Value = null;
                GridViewConsultationDoctor.Rows[GridViewConsultationDoctor.CurrentRow.Index].Cells[(int)DoctorConsultationTableColumn.CID].Value = null;
                GridViewConsultationDoctor.CommitEdit(DataGridViewDataErrorContexts.Commit);

                DataGridViewComboBoxEditingControl ck = new DataGridViewComboBoxEditingControl();
                ck.SelectedIndex = -1;
                NameColumnComboSelectionChanged(ck, e);
            }

        }

        private void NameColumnComboTextChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1 && !string.IsNullOrEmpty(((ComboBox)sender).Text))
            {
                int index = ((ComboBox)sender).FindStringExact(((ComboBox)sender).Text);
                if (index != -1)
                {
                    DataGridViewComboBoxEditingControl ck = (DataGridViewComboBoxEditingControl)sender;
                    ck.SelectedIndex = index;
                    NameColumnComboSelectionChanged(ck, e);
                }
                else
                {
                    GridViewConsultationDoctor.Rows[GridViewConsultationDoctor.CurrentRow.Index].Cells[(int)DoctorConsultationTableColumn.NAME].Value = null;
                    GridViewConsultationDoctor.Rows[GridViewConsultationDoctor.CurrentRow.Index].Cells[(int)DoctorConsultationTableColumn.CID].Value = null;
                    Reset(GridViewConsultationDoctor.CurrentRow.Index);
                }
            }
            else if (((ComboBox)sender).SelectedIndex != -1)
            {
                NameColumnComboSelectionChanged(sender, e);
            }

        }
        private void NameColumnComboSelectionChanged(object sender, EventArgs e)
        {
            int Index = GridViewConsultationDoctor.CurrentCell.RowIndex;
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                GridViewConsultationDoctor.CommitEdit(DataGridViewDataErrorContexts.Commit);
                Consultation Consultation = (Consultation)(((ComboBox)sender).Items[((ComboBox)sender).SelectedIndex]);
                if (GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.NAME].Value != null && GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.CID].Value != null && Consultation.Name == GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.CID].Value.ToString())
                {
                    GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.CID].Value = Consultation.Id;
                    return;
                }
                if (GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.NAME].Value != null && GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.CID].Value != null && GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.NAME].Value.ToString() == GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.CID].Value.ToString())
                {
                    GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.CID].Value = GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.NAME].Value;
                    return;
                }

                GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.CID].Value = Consultation.Id;

                if (Consultation != null)
                {
                    GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.DISPLAY].Value = Consultation.DisplayAs;
                    GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.DESC].Value = Consultation.Discription;
                    GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.FEE].Value = Consultation.Fee;

                }

                GridViewConsultationDoctor.BeginInvoke(new MethodInvoker(delegate ()
                {
                    //GridViewConsultationDoctor.CurrentCell = GridViewConsultationDoctor[(int)DoctorConsultationTableColumn.DISPLAY, Index];
                    //GridViewConsultationDoctor.CurrentCell = GridViewConsultationDoctor[(int)DoctorConsultationTableColumn.NAME, Index];

                }));
            }
            else
            {
                if (GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.NAME].Value == null)
                {
                    Reset(Index);
                }
            }

        }

        private void Reset(int Index)
        {
            GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.DISPLAY].Value = null;
            GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.DESC].Value = null;
            GridViewConsultationDoctor.Rows[Index].Cells[(int)DoctorConsultationTableColumn.FEE].Value = 0.00;
        }
    }
}
