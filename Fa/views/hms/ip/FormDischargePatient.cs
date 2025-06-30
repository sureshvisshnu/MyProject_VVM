using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.utils;
using fa.context;
using fa.libraries.utils;
using fa.libraries.Validation;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.reports.Hms;
using fa.views.hms.helper;
using fa.views.purchase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.hms.ip
{
    public partial class FormDischargePatient : FormPatientBase
    {
        public static string EnterWaiveAmountErrorMsg = "Please enter waive amount.";
        public static string ChooseDischargeByErrorMsg = "Please select discharge person.";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string EnterValidFollwUpDateErrorMsg = "Please enter the valid next follow-up on date..";
        public static string EnterTextErrorMsg = "Please enter {0}.";
        public static string Grid_DeleteConfirmText = "Do you want to delete row {0}?";
        public static string SaveSuccessText = "Saved...";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string CancelConfirmText = "There are unsaved changes, Do you want cancel?";
        public static string ExitConfirmText = "There are unsaved changes, Do you want exit?";
        public static string Grid_ChooseItemErrorMsg = "Please choose prescription.";
        public static string Grid_ItemMantatoryFiledErrorMsg = "Please enter {0}.";
        public static string Grid_ItemMantatoryFiledErrorMsgs = "Please select {0}.";
        public static string SelectPrescriptionErrorMsg = "Please select prescription.";
        public static string SelectPrescripedMedicienPreferedAfterorBeforeFoodErrorMsg = "Please select prescriped medicine taken after or before food";
        public static string EnterPrescripedMedicienDosageErrorMsg = "Please Enter prescriped medicine Dosage";
        public static string EnterPrescripedMedicienTakenDaysErrorMsg = "Please Enter prescriped medicine how many days taken";
        public static string SelectPrescripedMedicienTakenTimeDelayErrorMsg = "Please select prescriped medicine taken time delay";
        public static string SelectPrescripedMedicienTakenTimeErrorMsg = "Please select prescriped medicine taken Time";
        public static string PrescriptionTimeIntervalErrorMsg = "Cant have a breakup for Time Interval";
        public static string EnterDischargeDateErrorMsg = "Please enter proper {0} Date";

        public long PatientIpId = 0L;
        public long PatientId = 0L;
        public long ProductId = 0L;
        public long DischargeId = 0L;
        public bool IsDischarged = false;
        public FormDischargePatient()
        {
            InitializeComponent();
        }
        private void FormDischargePatient_Load(object sender, EventArgs e)
        {
            PatientInfo.Clear();
            PatientInfo.Type = fa.common.PatientTypes.InPatient;
            PatientInfo.PatientId = PatientId;
            LoadIpInfo();
            LoadDischarge();
            BtnPrint.Enabled = false;
            if (IsDischarged)
            {
                EnableForm();
                BtnPrint.Enabled = true;
            }
            this.formIsDirty = false;
        }
        private void LoadIpInfo()
        {
            ResetForm();
        }
        private void LoadDischarge()
        {
            DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(PatientIpId);
            if (DischargeNote != null)
            {
                DischargeId = DischargeNote.Id;
                TextBoxDiagnosisSummary.Text = DischargeNote.DiagnosisSummary;
                TextBoxTreatmentSummary.Text = DischargeNote.TreatmentSummary;
                TextBoxDischargeSummary.Text = DischargeNote.DischargeSummary;
                DateTimePickerDisCharOn.Date = DischargeNote.DischargeOn == null ? Global.getTransactionDate() : DischargeNote.DischargeOn;
                DateTimePickerNextFollowOn.Date = DischargeNote.NextFollowUp;
                ComboBoxDisChargeBy.SelectedIndex = ComboBoxDisChargeBy.FindStringExact(DischargeNote.Employee.Name);
                CheckBoxDiceased.Checked = DischargeNote.IsDeceased;
                TextBoxBalance.Text = GetDischargeBalance().ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                TextBoxWaiveBalance.Text = DischargeNote.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                CheckBoxWaive.Enabled = double.Parse(TextBoxBalance.Text) > 0 ? true : DischargeNote.Waive;
                CheckBoxWaive.Checked = DischargeNote.Waive;
                TextBoxWaiveBalance.Text = DischargeNote.Amount.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                FormButtonStatus();
            }
            TextBoxDiagnosisSummary.Select();
        }
        private void FormButtonStatus()
        {
            InPatientAdmission InPatientAdmission = IpManager.Instance.GetInPatientAdmissionById(PatientIpId);
            if (InPatientAdmission != null && InPatientAdmission.Status == InPatientStatus.DISCHARGED)
            {
                TextBoxDischargeSummary.ReadOnly = true;
                TextBoxTreatmentSummary.ReadOnly = true;
                TextBoxDiagnosisSummary.ReadOnly = true;
                DateTimePickerNextFollowOn.Enabled = false;
                CheckBoxDiceased.Enabled = false;
                ComboBoxDisChargeBy.Visible = false;
                CheckBoxWaive.Enabled = false;
                BtnPrint.Location = BtnDischarge.Location;
                BtnCancel.Visible = false;
                BtnSave.Visible = false;
                BtnDischarge.Visible = false;
            }
        }
        private void EnableForm()
        {
            InPatientAdmission InPatientAdmission = IpManager.Instance.GetInPatientAdmissionById(PatientIpId);
            if (InPatientAdmission != null && InPatientAdmission.Status == InPatientStatus.DISCHARGED)
            {
                TextBoxDischargeSummary.ReadOnly = false;
                TextBoxTreatmentSummary.ReadOnly = false;
                TextBoxDiagnosisSummary.ReadOnly = false;
                DateTimePickerNextFollowOn.ReadOnly = true;
                CheckBoxDiceased.Enabled = true;
                ComboBoxDisChargeBy.Visible = true;
                CheckBoxWaive.Enabled = true;
                BtnPrint.Location = BtnDischarge.Location;
                BtnCancel.Visible = true;
                BtnSave.Visible = true;
                BtnDischarge.Visible = false;
            }
        }
        private double GetDischargeBalance()
        {
            double Amount = 0.00;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmission Admission = IpManager.Instance.GetInPatientAdmissionById(PatientIpId);
                if (Admission != null)
                {
                    Amount = PatientLedgerManager.Instance.GetDischargeBalanceAmount(Admission);
                }
                if (Admission != null)
                {
                    DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(PatientIpId);
                    if (DischargeNote == null)
                    {
                        InPatientLocation location = IpManager.Instance.GetInPatientLocationbyAdmissionId(Admission.Id);
                        int TotalDays = 0;
                        TimeSpan span = Global.getTransactionDate().Subtract(location.DateMovedIn);
                        TotalDays = span.Days + (span.Hours > 0 ? 1 : 0);
                        if (TotalDays > 0 && location.BedId != null)
                        {
                            Bed bed = Context.Beds.Include("Ward").Include("BedType").Include("BedType.Rents").FirstOrDefault(x => x.Id == (long)location.BedId)!;
                            if (bed != null && bed.BedType != null && bed.BedType.Rents != null && bed.BedType.Rents.Count > 0)
                            {
                                double AmountPerday = bed.BedType.Rents.First().Amount;

                                double WardBedAmount = TotalDays * AmountPerday;
                                Amount += WardBedAmount;
                            }
                        }
                    }
                }
            }
            return Amount;
        }
        private void ResetForm()
        {
            ErrorMsgDisPatient.Text = string.Empty;
            TextBoxDiagnosisSummary.ResetText();
            TextBoxTreatmentSummary.ResetText();
            TextBoxDischargeSummary.ResetText();
            TextBoxBalance.Text = GetDischargeBalance().ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
            DateTimePickerDisCharOn.Format = Global.Company.DateFormat;
            DateTimePickerDisCharOn.MinDate = (DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerDisCharOn.Date = Global.getTransactionDate();
            DateTimePickerNextFollowOn.Format = Global.Company.DateFormat;
            DateTimePickerNextFollowOn.Reset();
            ComboUtils.InitializeDoctorCombo(ComboBoxDisChargeBy, Global.Company.CompanyId);
            ComboBoxDisChargeBy.ResetText();
            ComboBoxDisChargeBy.SelectedIndex = -1;
            CheckBoxDiceased.Checked = false;
            CheckBoxWaive.Checked = false;
            CheckBoxWaive.Enabled = true;
            if (double.Parse(TextBoxBalance.Text) == 0)
            {
                CheckBoxWaive.Enabled = false;
            }
            linkLabelLedger.TabStop = false;
            TextBoxWaiveBalance.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
        }
        private DischargeNote GetDischargeNoteFromForm()
        {
            DischargeNote DischargeNote = new DischargeNote();
            DischargeNote.Id = DischargeId;
            DischargeNote.CompanyId = Global.Company.CompanyId;
            DischargeNote.PatientId = PatientId;
            DischargeNote.InPatientAdmissionId = PatientIpId;
            DischargeNote.Amount = double.Parse(TextBoxWaiveBalance.Text);
            DischargeNote.DiagnosisSummary = TextBoxDiagnosisSummary.Text;
            DischargeNote.TreatmentSummary = TextBoxTreatmentSummary.Text;
            DischargeNote.DischargeSummary = TextBoxDischargeSummary.Text;
            DischargeNote.Date = Global.getTransactionDate();
            DischargeNote.NextFollowUp = !CheckBoxDiceased.Checked ? (DateTime)DateTimePickerNextFollowOn.Date! : null;
            DischargeNote.Waive = CheckBoxWaive.Checked;
            DischargeNote.IsDeceased = CheckBoxDiceased.Checked;
            Employee Employee = (Employee)ComboBoxDisChargeBy.Items[ComboBoxDisChargeBy.SelectedIndex];
            if (Employee != null)
            {
                DischargeNote.EmployeeId = Employee.Id;
            }
            return DischargeNote;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(CancelConfirmText, "Confirm",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxDiagnosisSummary.Select();
                    return;
                }
            }
            ResetForm();
            LoadDischarge();
            this.formIsDirty = false;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                DischargeNote DischargeNote = GetDischargeNoteFromForm();
                DischargeNote DischargedNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(PatientIpId);
                if (DischargedNote != null)
                {
                    DischargeNote.DischargeOn = DischargedNote.DischargeOn;
                }
                if (DischargeNote.Id == 0L)
                {
                    DischargeNote = DischargeNoteManager.Instance.AddDischargeNote(DischargeNote, false);
                }
                else
                {
                    DischargeNote = DischargeNoteManager.Instance.UpdateNotes(DischargeNote, false);
                }
                DischargeId = DischargeNote.Id;
                ResetForm();
                LoadDischarge();
                BtnPrint.Enabled = true;
                ErrorMsgDisPatient.Text = SaveSuccessText;
            }
            this.formIsDirty = false;
        }
        private void BtnDischarge_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    DischargeNote DischargeNote = GetDischargeNoteFromForm();
                    DischargeNote.DischargeOn = Global.getTransactionDate();
                    DischargeNote.UserId = Global.User.UserId;
                    if (DateTimePickerNextFollowOn.Date != null) { DischargeNote.NextFollowUp = (DateTime)DateTimePickerNextFollowOn.Date; }
                    if (DischargeNote.Id == 0L)
                    {
                        DischargeNote = DischargeNoteManager.Instance.AddDischargeNote(DischargeNote, true);
                    }
                    else
                    {
                        DischargeNote = DischargeNoteManager.Instance.UpdateNotes(DischargeNote, true);
                    }
                    DischargeId = DischargeNote.Id;
                    LoadDischarge();
                    BtnPrint.Enabled = true;
                    ErrorMsgDisPatient.Text = SaveSuccessText;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.formIsDirty = false;
        }
        private bool ValidateForm()
        {
            ErrorMsgDisPatient.Text = string.Empty;
            if (string.IsNullOrEmpty(TextBoxDiagnosisSummary.Text.Trim()))
            {
                ErrorMsgDisPatient.Text = string.Format(EnterTextErrorMsg, "diagnosis summary");
                TextBoxDiagnosisSummary.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxTreatmentSummary.Text.Trim()))
            {
                ErrorMsgDisPatient.Text = string.Format(EnterTextErrorMsg, "treatment summary");
                TextBoxTreatmentSummary.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxDischargeSummary.Text.Trim()))
            {
                ErrorMsgDisPatient.Text = string.Format(EnterTextErrorMsg, "discharge summary");
                TextBoxDischargeSummary.Select();
                return false;
            }
            if (CheckBoxWaive.Checked && (string.IsNullOrEmpty(TextBoxWaiveBalance.Text) || double.Parse(TextBoxWaiveBalance.Text) == 0))
            {
                ErrorMsgDisPatient.Text = EnterWaiveAmountErrorMsg;
                TextBoxWaiveBalance.Select();
                return false;
            }
            if (DateTimePickerNextFollowOn.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerNextFollowOn.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                if (!CheckBoxDiceased.Checked)
                {
                    DateTimePickerNextFollowOn.Focus();
                    ErrorMsgDisPatient.Text = string.Format(EnterDischargeDateErrorMsg, "Discharge Follow on");
                    return false;
                }
            }
            if (DateTimePickerNextFollowOn.Date != null && !DateUtils.ValidDate(((DateTime)DateTimePickerNextFollowOn.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ErrorMsgDisPatient.Text = EnterValidDateErrorMsg;
                DateTimePickerNextFollowOn.Focus();
                return false;
            }
            if (DateTimePickerNextFollowOn.Date != null && DateTimePickerNextFollowOn.Date <= DateTimePickerDisCharOn.Date)
            {
                ErrorMsgDisPatient.Text = EnterValidFollwUpDateErrorMsg;
                DateTimePickerNextFollowOn.Focus();
                return false;
            }
            if (ComboBoxDisChargeBy.SelectedIndex < 0)
            {
                ErrorMsgDisPatient.Text = ChooseDischargeByErrorMsg;
                ComboBoxDisChargeBy.Select();
                return false;
            }
            return true;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void ProductIdTransportReload(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ProductIdTransport.Text))
            {
                ProductId = long.Parse(ProductIdTransport.Text);
            }
        }
        private void FormDischargePatient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(ExitConfirmText, "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Result == DialogResult.No)
                {
                    TextBoxDiagnosisSummary.Select();
                    e.Cancel = true;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == (Keys.F8))
            {
                BtnSave.PerformClick();
            }
            else if (keyData == (Keys.F7))
            {
                BtnDischarge.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            if (keyData != Keys.Shift && keyData == Keys.Tab)
            {
                if (ActiveControl == TextBoxBalance)
                {
                    CheckBoxWaive.Focus();
                    return true;
                }
                if (ActiveControl == CheckBoxWaive && CheckBoxWaive.Checked)
                {
                    TextBoxWaiveBalance.Focus();
                    return true;
                }
                else if (ActiveControl == CheckBoxWaive && !CheckBoxWaive.Checked)
                {
                    linkLabelLedger.Focus();
                    return true;
                }
                if (ActiveControl == TextBoxWaiveBalance)
                {
                    linkLabelLedger.Focus();
                    return true;
                }
            }
            if (keyData == (Keys.Shift | Keys.Tab))
            {
                if (ActiveControl == CheckBoxDiceased)
                {
                    if (DateTimePickerNextFollowOn.Enabled)
                    {
                        DateTimePickerNextFollowOn.Focus();
                        return true;
                    }
                    else
                    {
                        linkLabelLedger.Focus();
                        return true;
                    }
                }
                if (ActiveControl == linkLabelLedger)
                {
                    if (CheckBoxWaive.Checked)
                    {
                        TextBoxWaiveBalance.Focus();
                        return true;
                    }
                    else
                    {
                        CheckBoxWaive.Focus();
                        return true;
                    }
                }
                if (ActiveControl == TextBoxWaiveBalance)
                {
                    CheckBoxWaive.Focus();
                    return true;
                }
                if (ActiveControl == CheckBoxWaive)
                {
                    TextBoxBalance.Focus();
                    return true;
                }
                if (ActiveControl == DateTimePickerNextFollowOn && DateTimePickerNextFollowOn.Enabled)
                {
                    linkLabelLedger.Focus();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void linkLabelLedger_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(PatientIpId);
            if (DischargeNote != null && DischargeNote.Id != 0)
            {
                FormPatientLedger FormPatientLedger = new FormPatientLedger();
                FormPatientLedger.CreateLedgerOnLoadFromDischarge = true;
                FormPatientLedger.PatientId = PatientId;
                FormPatientLedger.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please save the discharge note first, then open the patient ledger.", "Save Discharge Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void TextBoxDischargeSummary_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxBalance.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTreatmentSummary.Select();
            }
        }
        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxDiagnosisSummary.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ComboBoxDisChargeBy.Select();
            }
        }
        private void TextBoxDiagnosisSummary_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxTreatmentSummary.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnSave.Select();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            FormDischargeSummaryPrintOptions PrintOptions = new FormDischargeSummaryPrintOptions();
            PrintOptions.PatientId = PatientId;
            PrintOptions.PatientIpId = PatientIpId;
            PrintOptions.DischargeId = DischargeId;
            if (DischargeId == 0)
            {
                PrintOptions.CheckBoxDischargeSummary.Checked = false;
                PrintOptions.CheckBoxDischargeSummary.Enabled = false;
                PrintOptions.CheckBoxPartientIPChart.Checked = true;
            }
            else
            {
                PrintOptions.CheckBoxDischargeSummary.Checked = true;
                PrintOptions.CheckBoxDischargeSummary.Enabled = true;
                PrintOptions.CheckBoxPartientIPChart.Checked = false;
            }
            PrintOptions.ShowDialog();
        }

        private void CheckBoxDiceased_CheckedChanged(object sender, EventArgs e)
        {
            DateTimePickerNextFollowOn.Reset();
            DateTimePickerNextFollowOn.Enabled = CheckBoxDiceased.Checked ? false : true;
        }

        private void CheckBoxWaive_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxWaive.Checked)
            {
                TextBoxWaiveBalance.ReadOnly = false;
                TextBoxWaiveBalance.TabStop = true;
                if (DischargeId == 0L)
                {
                    TextBoxWaiveBalance.Text = TextBoxBalance.Text;
                }
                else
                {
                    DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(PatientIpId);
                    if (DischargeNote != null)
                    {
                        TextBoxWaiveBalance.Text = DischargeNote.Amount.ToString(Global.Company.PrimaryCurrency.CurrencyFormat);
                    }
                }
            }
            else
            {
                TextBoxWaiveBalance.Text = TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision);
                TextBoxWaiveBalance.ReadOnly = true;
                TextBoxWaiveBalance.TabStop = false;
            }
        }
        private void TextBoxTreatmentSummary_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TextBoxDiagnosisSummary.Select();
            }
        }
        private void ComboBoxDisChargeBy_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.Modifiers != Keys.Shift && e.KeyCode == Keys.Tab)
            {
                BtnSave.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                CheckBoxDiceased.Focus();
            }
        }

        private void TextBoxDiagnosisSummary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private void TextBoxDischargeSummary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private void TextBoxTreatmentSummary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                e.Handled = true;
            }
        }

        private void ComboBoxDisChargeBy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                BtnCancel.PerformClick();
            }
            if (e.KeyCode == Keys.F8)
            {
                BtnSave.PerformClick();
            }
            if (e.KeyCode == Keys.F7)
            {
                BtnDischarge.PerformClick();
            }
            if (e.KeyCode == Keys.F10)
            {
                BtnExit.PerformClick();
            }
            if (e.KeyCode == Keys.F9 && ActiveControl == BtnPrint)
            {
                BtnPrint.PerformClick();
            }
        }
    }
}
