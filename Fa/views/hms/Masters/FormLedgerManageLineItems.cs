using DocumentFormat.OpenXml.InkML;
using fa.api.Accounting;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.UserProfile;
using fa.api.utils;
using fa.context;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Catalog;
using fa.model.hms.common;
using fa.model.hms.config;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.views.controls.grid;
using fa.views.hms.patient;
using fa.views.sales;
using Fa.api.Hms;
using Fa.views.hms.helper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace fa.views.hms.Masters
{
    public enum LedgerLineItemGridColumn
    {
        SNO, NAME, DESC, FEECHARGED, REMOVE, ID, INVID, FEEID
    }
    public partial class FormLedgerManageLineItems : FormPatientBase
    {
        public long PatientId = 0L;
        public long? PatientOPId = null;
        public long? PatientIPId = null;
        public long FeeId = 0L;
        public static string SaveSuccessText = "Saved success...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string DeleteErrorText = "Error in item deleting. !";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string DeleteConfirmText = "Do you want to delete {0}?";
        public static string RefNoErrorMsg = "Please contact administrator to generate reference number.";
        public static string EnterAmountErrorMsg = "Please enter valid amount.";
        public static string Grid_DeleteConfirmText = "Do you want to delete row {0}?";
        public static string Grid_EnterDescErrorMsg = "Please enter fee charges.";
        public static string Grid_EmptyErrorMsg = "Please enter item details.";
        public static string InvalidInvoiceErrorMsg = "Invalid item.";
        public static string SearchOutput = "No items found";
        public FormLedgerManageLineItems()
        {
            InitializeComponent();
        }
        private void FormGenerateInvoice_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            LoadPatientInfo();
            loadUnappliedChargedFee();
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
            {
                ErrorMsg.Text = "";
            DataGridViewLineItems.Rows.Clear();
            DataGridViewLineItems.Rows.Add();
            DataGridViewInvoiceTotal.Rows.Clear();
            DataGridViewInvoiceTotal.Rows.Add();
            DataGridViewInvoiceTotal.Rows[0].Cells[0].Value = "Total : ";
            DataGridViewInvoiceTotal.Rows[0].Cells[1].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)DataGridViewLineItems.Columns["FeeCharged"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }
        private void LoadPatientInfo()
        {
            Patient PatientData = PatientManager.Instance.GetPatientById(PatientId);
            if (PatientData != null)
            {
                PatientInfoMiniHorizontal.PatientId = PatientData.Id;
                InPatientAdmission Admission = IpManager.Instance.GetUnDischargedInPatientAdmissionByPatientId(PatientId);
                if (Admission != null)
                {
                    PatientIPId = Admission.Id;
                    PatientOPId = Admission.OpRegistrationId;
                }
                else
                {
                    Registration Registration = OpManager.Instance.GetPatientLastOp(PatientId, Global.getTransactionDate());
                    if (Registration != null)
                    {
                        PatientOPId = Registration.Id;
                        InPatientAdmission lAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByOpId(Registration.Id);
                        if (lAdmission != null)
                        {
                            PatientIPId = lAdmission.Id;
                        }
                    }
                }
            }
        }
        private void loadUnappliedChargedFee()
        {
            IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListFeeChargeFromPatientLedgerByPatientId(PatientId);
            if (lPatientLedger != null && lPatientLedger.Count > 0)
            {
                int Count = 0;
                string AddDesc = string.Empty;
                foreach (PatientLedger Ledger in lPatientLedger)
                {
                    if (Ledger.Amount == 0) { continue; }
                    DataGridViewLineItems.Rows.Add();
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.REMOVE].Value = "X";
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.SNO].Value = Count + 1;
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.NAME].Value = Ledger.ConsultedConsultationFee != null ? Ledger.ConsultedConsultationFee.Name : Ledger.ConsultedLabTest != null ? Ledger.ConsultedLabTest.Name : Ledger.ConsultedProcedure != null ? Ledger.ConsultedProcedure.Name : string.Empty;
                    if (Ledger.Type == TransactionType.REGISTRATION_FEE)
                    {
                        DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.NAME].Value = Ledger.InPatientAdmissionId != null ? "Admission fee" : "Registration fee";
                    }
                    if (Ledger.Type == TransactionType.ROOM_RENT)
                    {
                        DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.NAME].Value = "Room rent";
                    }
                    if (Ledger.Type == TransactionType.PAYMENT)
                    {
                        AddDesc = AdditionalDescription(Ledger.Id);
                    }
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.DESC].Value = Ledger.Type == TransactionType.PAYMENT ? Ledger.Description + (string.IsNullOrEmpty(AddDesc) ? "" : ("\n" + AddDesc)) : (Ledger.ConsultedConsultationId != null ? Ledger.ConsultedConsultationFee.Description : Ledger.ConsultedProcedureId != null ? Ledger.ConsultedProcedure.Description : Ledger.Description);
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].Value = Ledger.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.ID].Value = Ledger.Id;
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.INVID].Value = Ledger.PatientInvoiceId;
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.SNO].ReadOnly = true;
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.DESC].ReadOnly = true;
                    DataGridViewLineItems.Rows[Count].Cells[(int)LedgerLineItemGridColumn.FEEID].Value = Ledger.ConsultedConsultationFee != null ? Ledger.ConsultedConsultationFee.ConsultationId : Ledger.ConsultedLabTest != null ? Ledger.ConsultedLabTest.ConsultedLabTestId : Ledger.ConsultedProcedure != null ? Ledger.ConsultedProcedure.ConsultedProcedureId : string.Empty;
                    Count++;
                }
                ReSequence();
                ComputeFormTotal();
                DataGridViewLineItems.BeginInvoke(new MethodInvoker(delegate ()
                {
                    DataGridViewLineItems.Focus();
                    DataGridViewLineItems.CurrentCell = DataGridViewLineItems[1, DataGridViewLineItems.Rows.Count - 1];
                    DataGridViewLineItems.BeginEdit(true);
                }));
            }
        }
        private string AdditionalDescription(long LedgId)
        {
            string Desc = string.Empty;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<PatientInvoicePayment> lPatientInvoicePayment = Context.PatientInvoicePayments.Include("PatientInvoice").Where(x => x.LedgerId == LedgId).ToList();
                if (lPatientInvoicePayment != null)
                {
                    foreach (PatientInvoicePayment Payment in lPatientInvoicePayment)
                    {
                        Desc += string.IsNullOrEmpty(Desc) ? "Against invoice #" + Payment.PatientInvoice.ReferenceNumber : ", #" + Payment.PatientInvoice.ReferenceNumber;
                    }
                }
            }
            return Desc;
        }
        private void ReSequence()
        {
            for (int i = 0; i < DataGridViewLineItems.Rows.Count; i++)
            {
                DataGridViewLineItems.Rows[i].Cells[0].Value = i + 1;
            }
        }
        private void ComputeFormTotal()
        {
            double TotalAmount = 0.00;
            for (int i = 0; i < DataGridViewLineItems.Rows.Count - 1; i++)
            {
                double Amount = (DataGridViewLineItems.Rows[i].Cells[3].Value) == null ? 0.00 : (float.Parse(DataGridViewLineItems.Rows[i].Cells[3].Value.ToString()));
                TotalAmount = TotalAmount + Amount;
            }
            DataGridViewInvoiceTotal.Rows[0].Cells[1].Value = TotalAmount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
        }
        private List<PatientLedger> GetLineItemFromForm()
        {
            List<PatientLedger> ListPatientLedger = new List<PatientLedger>();
            if (DataGridViewLineItems.Rows.Count > 1)
            {
                for (int i = 0; i < DataGridViewLineItems.Rows.Count - 1; i++)
                {
                    if (DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.ID].Value != null)
                    {
                        PatientLedger lPatientLedger = PatientLedgerManager.Instance.FindPatientLedgerById((long)DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.ID].Value);
                        if (lPatientLedger != null)
                        {
                            lPatientLedger.Amount = double.Parse(DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].Value.ToString());
                            lPatientLedger.Description = DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.DESC].Value.ToString();
                            ListPatientLedger.Add(lPatientLedger);
                        }
                    }
                    else
                    {
                        PatientLedger PatientLedger = new PatientLedger();
                        PatientLedger.Date = Global.getTransactionDate();
                        PatientLedger.PatientId = PatientId;
                        PatientLedger.Amount = double.Parse(DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].Value.ToString());
                        PatientLedger.Description = "Charge for " + DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.NAME].Value.ToString() + " by " + Global.User.Name;
                        PatientLedger.CompanyId = Global.Company.CompanyId;
                        PatientLedger.OpRegistrationId = PatientOPId;
                        PatientLedger.InPatientAdmissionId = PatientIPId;
                        PatientLedger.Type = TransactionType.CONSULTATION_FEE;
                        PatientLedger.ConsultedConsultationId = long.Parse(DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.FEEID].Value.ToString());
                        ListPatientLedger.Add(PatientLedger);
                    }
                }
            }
            return ListPatientLedger;
        }
        private bool ValidateForm()
        {
            int Count = DataGridViewLineItems.Rows.Count;
            if (Count > 1)
            {
                for (int i = 0; i < Count - 1; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        if (DataGridViewLineItems.Rows[i].Cells[j].Value == null || DataGridViewLineItems.Rows[i].Cells[j].Value.Equals(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) || DataGridViewLineItems.Rows[i].Cells[j].Value.Equals("0"))
                        {
                            DataGridViewLineItems.Select();
                            DataGridViewLineItems.CurrentCell = DataGridViewLineItems[j, i];
                            DataGridViewLineItems.BeginEdit(true);
                            ErrorMsg.Text = string.Format(Grid_EnterDescErrorMsg, DataGridViewLineItems.Columns[j].HeaderText);
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    List<PatientLedger> lPatientLedger = GetLineItemFromForm();
                    List<ConsultedProcedure> lProcedure = GetLineItemForProcedure();
                    List<ConsultedConsultationFee> lConsultationFee = GetLineItemForConsultationFee();
                    if (lPatientLedger.Count > 0)
                    {
                        PatientLedgerManager.Instance.UpdatePatientFeesInLedger(lPatientLedger);
                    }
                    if (lProcedure.Count > 0)
                    {
                        PatientLedgerManager.Instance.UpdateProcedureInLedger(lProcedure);
                    }
                    if (lConsultationFee.Count > 0)
                    {
                        PatientLedgerManager.Instance.UpdateConsultationFeeInLedger(lConsultationFee);
                    }
                    ResetForm();
                    loadUnappliedChargedFee();
                    ErrorMsg.Text = SaveSuccessText;
                    this.formIsDirty = false;
                }
                catch (Exception ex)
                {
                    ErrorMsg.Text = ex.Message;
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private List<ConsultedProcedure> GetLineItemForProcedure()
        {
            List<ConsultedProcedure> ListConsultedProcedure = new List<ConsultedProcedure>();
            if (DataGridViewLineItems.Rows.Count > 1)
            {
                for (int i = 0; i < DataGridViewLineItems.Rows.Count - 1; i++)
                {
                    if (DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.ID].Value != null)
                    {
                        PatientLedger lPatientLedger = PatientLedgerManager.Instance.FindPatientLedgerById((long)DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.ID].Value);
                        if (lPatientLedger.ConsultedProcedureId != null)
                        {
                            ConsultedProcedure procedure = ConsultationNoteManager.Instance.GetConsultedProcedureById((long)lPatientLedger.ConsultedProcedureId);
                            procedure.Description = DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.DESC].Value.ToString();
                            procedure.Fees = Double.Parse(DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].Value.ToString());
                            ListConsultedProcedure.Add(procedure);
                        }
                    }
                }
            }
            return ListConsultedProcedure;
        }
        private List<ConsultedConsultationFee> GetLineItemForConsultationFee()
        {
            List<ConsultedConsultationFee> ListConsultedDoctorConsultationFee = new List<ConsultedConsultationFee>();
            if (DataGridViewLineItems.Rows.Count > 1)
            {
                for (int i = 0; i < DataGridViewLineItems.Rows.Count - 1; i++)
                {
                    if (DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.ID].Value != null)
                    {
                        PatientLedger lPatientLedger = PatientLedgerManager.Instance.FindPatientLedgerById((long)DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.ID].Value);
                        if (lPatientLedger.ConsultedConsultationId != null)
                        {
                            ConsultedConsultationFee ConsultationFee = ConsultationNoteManager.Instance.GetConsultedConsultationFeeById((long)lPatientLedger.ConsultedConsultationId);
                            ConsultationFee.Description = DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.DESC].Value.ToString();
                            ConsultationFee.Fee = Double.Parse(DataGridViewLineItems.Rows[i].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].Value.ToString());
                            ListConsultedDoctorConsultationFee.Add(ConsultationFee);
                        }
                    }
                }
            }
            return ListConsultedDoctorConsultationFee;
        }
        private void DataGridViewInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < DataGridViewLineItems.Rows.Count - 1 && e.ColumnIndex == (int)LedgerLineItemGridColumn.REMOVE)
            {
                if (DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.ID].Value != null || DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.FEEID].Value != null)
                {
                    long LedgerID = DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.ID].Value == null ? long.Parse(DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.FEEID].Value.ToString()) : long.Parse(DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.ID].Value.ToString());
                    PatientLedger PatientLedger = PatientLedgerManager.Instance.FindPatientLedgerById(LedgerID);
                    if (PatientLedger != null)
                    {
                        DialogResult Result = MessageBox.Show(string.Format(DeleteConfirmText, PatientLedger.Description), "Delete Confirm",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            PatientLedgerManager.Instance.DeleteFeeFromPatientLedger(PatientLedger);
                            DataGridViewLineItems.Rows.RemoveAt(e.RowIndex);
                            ReSequence();
                            ComputeFormTotal();
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    else
                    {
                        ErrorMsg.Text = "Somting went wrong, please check this item is still valid.";
                        return;
                    }
                }
                else
                {
                    ErrorMsg.Text = "Somting went wrong, please check this item is still valid.";
                }
            }
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void FormGenerateInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void DataGridViewInvoice_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{tab}");
            }
            DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.NAME].ReadOnly = false;
            DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.DESC].ReadOnly = false;
            DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].ReadOnly = true;
            DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.REMOVE].ReadOnly = true;
            if (DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.FEEID].Value != null)
            {
                DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].ReadOnly = false;
            }
            if (DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.ID].Value != null)
            {
                DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.NAME].ReadOnly = true;
            }
        }
        private void DataGridViewInvoice_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridViewLineItems.CurrentCell.ColumnIndex == (int)LedgerLineItemGridColumn.FEECHARGED)
            {
                ComputeFormTotal();
            }
        }
        private void DataGridViewInvoice_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DateTimePickerInvoiceDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewLineItems.Select();
                DataGridViewLineItems.CurrentCell = DataGridViewLineItems[0, 0];
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSave.Focus();
            }
        }

        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                DataGridViewLineItems.Select();
                if (DataGridViewLineItems[(int)LedgerLineItemGridColumn.DESC, 0].ReadOnly)
                {
                    DataGridViewLineItems.CurrentCell = DataGridViewLineItems[(int)LedgerLineItemGridColumn.FEECHARGED, 0];
                }
                else
                {
                    DataGridViewLineItems.CurrentCell = DataGridViewLineItems[(int)LedgerLineItemGridColumn.DESC, 0];
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (DataGridViewLineItems.Rows.Count > 0)
                {
                    DataGridViewLineItems.Select();
                    if (DataGridViewLineItems[(int)LedgerLineItemGridColumn.DESC, DataGridViewLineItems.Rows.Count - 1].ReadOnly)
                    {
                        DataGridViewLineItems.CurrentCell = DataGridViewLineItems[(int)LedgerLineItemGridColumn.FEECHARGED, DataGridViewLineItems.Rows.Count - 1];
                    }
                    else
                    {
                        DataGridViewLineItems.CurrentCell = DataGridViewLineItems[(int)LedgerLineItemGridColumn.DESC, DataGridViewLineItems.Rows.Count - 1];
                    }
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            try
            {
                if ((keyData == Keys.F2) && DataGridViewLineItems.CurrentCell.ColumnIndex == (int)LedgerLineItemGridColumn.NAME)
                {
                    SearchFee();
                    return true;
                }
                if (DataGridViewLineItems.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && DataGridViewLineItems.CurrentCell.ColumnIndex == (int)LedgerLineItemGridColumn.FEECHARGED)
                    {
                        int NextIndex = DataGridViewLineItems.CurrentRow.Index + 1;
                        if (NextIndex <= DataGridViewLineItems.RowCount && DataGridViewLineItems.Rows[NextIndex].Cells[(int)LedgerLineItemGridColumn.FEEID].Value == null)
                        {
                            SendKeys.Send("{tab}");
                        }
                        else
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewLineItems.CurrentCell.ColumnIndex == (int)LedgerLineItemGridColumn.FEECHARGED)
                    {
                        int Index = DataGridViewLineItems.CurrentRow.Index;
                        if (DataGridViewLineItems.Rows[Index].Cells[(int)LedgerLineItemGridColumn.FEEID].Value != null)
                        {
                            if (Index - 1 >= 0)
                            {
                                SendKeys.Send("{tab}{tab}");
                            }
                            else
                            {
                                BtnSave.Select();
                            }
                        }
                        else
                        {
                            SendKeys.Send("{tab}");
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && DataGridViewLineItems.CurrentCell.ColumnIndex == (int)LedgerLineItemGridColumn.NAME && DataGridViewLineItems.CurrentRow.Index != 0)
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
        int CurrentRow = 0;
        private void SearchFee()
        {
            bool IsDirty = this.formIsDirty;
            long Check = (DataGridViewLineItems.CurrentRow.Cells[(int)LedgerLineItemGridColumn.ID].Value != null) ? (long)DataGridViewLineItems.CurrentRow.Cells[(int)LedgerLineItemGridColumn.ID].Value : 0L;
            FeeId = 0L;
            FormSelectFee FormSelectFee = new FormSelectFee(this);
            DataGridViewLineItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            FormSelectFee.SearchText = (DataGridViewLineItems.CurrentRow.Cells[(int)LedgerLineItemGridColumn.NAME].Value != null) ? DataGridViewLineItems.CurrentRow.Cells[(int)LedgerLineItemGridColumn.NAME].Value.ToString() : "";
            FormSelectFee.ShowDialog();
            if (FeeId != 0)
            {
                CurrentRow = DataGridViewLineItems.CurrentRow.Index;
                if (DataGridViewLineItems.Rows.Count - 1 == DataGridViewLineItems.CurrentRow.Index)
                {
                    DataGridViewLineItems.Rows.Add();
                    DataGridViewLineItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                LoadFee();
                DataGridViewLineItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
                DataGridViewLineItems.CurrentCell = DataGridViewLineItems[(int)LedgerLineItemGridColumn.FEECHARGED, DataGridViewLineItems.CurrentRow.Index];
                DataGridViewLineItems.CurrentCell.Selected = true;
                ReSequence();
                ComputeFormTotal();
            }
            else
            {
                FeeId = Check;
                this.formIsDirty = IsDirty;
                return;
            }
        }
        private void LoadFee()
        {
            Consultation Consultation = ConsultationManager.Instance.GetConsultationById(FeeId);
            if (Consultation != null)
            {
                int index = CurrentRow;
                DataGridViewLineItems.Rows[index].Cells[(int)LedgerLineItemGridColumn.FEEID].Value = Consultation.Id;
                DataGridViewLineItems.Rows[index].Cells[(int)LedgerLineItemGridColumn.SNO].Value = index + 1;
                DataGridViewLineItems.Rows[index].Cells[(int)LedgerLineItemGridColumn.NAME].Value = Consultation.Name;
                DataGridViewLineItems.Rows[index].Cells[(int)LedgerLineItemGridColumn.DESC].Value = Consultation.Discription;
                DataGridViewLineItems.Rows[index].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].Value = Consultation.Fee.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
            }
        }
        private void DataGridViewLineItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.REMOVE].Value = "X";
                DataGridViewLineItems.Rows[e.RowIndex].Cells[(int)LedgerLineItemGridColumn.FEECHARGED].Value = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
            }
        }
    }
}
