using fa.api.Hms;
using fa.api.utils;
using fa.common;
using fa.model.hms.common;
using fa.report.Hms;
using fa.views.controls.ComboTreeView;
using fa.views.controls.grid;
using Standard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace fa.views.hms.patient
{
    enum ConsultedProcedureTableColumn
    {
        SNO, DATE, NAME, DESC, REQUBY, STATUS, PERBY, PERON, NOTE, FEE, ID
    }
    public partial class FormPatientProcedures : Form
    {
        FormPatientBase parent = null!;
        long PatientId = 0L;
        public FormPatientProcedures(object sender)
        {
            InitializeComponent();
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
        }
        public static string ChooseStatusErrorMsg = "Please select status.";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string NoRecordErrorMsg = "Record Not found.";
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            PerformUpload();
            FormPatientProcedures_Activated(sender, e);
        }

        private void GridViewProcedureInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                PerformUpload();
                FormPatientProcedures_Activated(sender, e);
            }
        }
        int lastSelectedRowIndex = -1;
        private void PerformUpload()
        {
            if (GridViewProcedureInfo.CurrentRow != null)
            {
                lastSelectedRowIndex = GridViewProcedureInfo.CurrentRow.Index;
                long CPId = long.Parse(GridViewProcedureInfo.CurrentRow.Cells[(int)ConsultedProcedureTableColumn.ID].Value.ToString()!);
                FormPatientProcedure PatientProcedure = new FormPatientProcedure(this);
                PatientProcedure.ProcedureId = CPId;
                PatientProcedure.PatientId = ConsultationNoteManager.Instance.GetConsultedProceduresById(CPId).ConsultationNote.PatientId;
                PatientProcedure.PatientType = PatientTypes.ImPatient;
                PatientProcedure.ShowDialog();
                //ResetForm();
                LoadPatientProcedure();
            }
        }
        private void FormPatientProcedures_Activated(object sender, EventArgs e)
        {
            if (lastSelectedRowIndex != -1 && GridViewProcedureInfo.Rows.Count > lastSelectedRowIndex)
            {
                GridViewProcedureInfo.Rows[lastSelectedRowIndex].Selected = true;
                GridViewProcedureInfo.FirstDisplayedScrollingRowIndex = lastSelectedRowIndex;
            }
        }
        private void FormPatientProcedures_Load(object sender, EventArgs e)
        {
            PatientId = long.Parse(parent.PatientIdTransport.Text);
            ResetForm();
            LoadPatientProcedure();
        }
        private void ResetForm()
        {
            ProcedureErrorMsg.Text = "";
            GridViewProcedureInfo.Rows.Clear();
            TextBoxName.Text = PatientId == 0L ? string.Empty : PatientManager.Instance.GetPatientById(PatientId).Name;
            FromDate.Format = Global.Company.DateFormat;
            FromDate.Date = Global.getTransactionDate().AddDays(-30);
            ToDate.Format = Global.Company.DateFormat;
            ToDate.Date = Global.getTransactionDate();
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewProcedureInfo.Columns["Fees"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            EnableButton(false);
        }
        private void EnableButton(bool Enable)
        {
            BtnUpdate.Enabled = Enable;
        }
        private bool ValidateForm()
        {
            if (GetStatus().Count == 0)
            {
                ProcedureErrorMsg.Text = ChooseStatusErrorMsg;
                ComboBoxStatus.Focus();
                return false;
            }
            if (FromDate.Date == null || !DateUtils.ValidDate(((DateTime)FromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ProcedureErrorMsg.Text = EnterValidDateErrorMsg;
                FromDate.Focus();
                return false;
            }
            if (ToDate.Date == null || !DateUtils.ValidDate(((DateTime)ToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                ProcedureErrorMsg.Text = EnterValidDateErrorMsg;
                ToDate.Focus();
                return false;
            }
            return true;
        }
        private void LoadPatientProcedure()
        {
            ProcedureErrorMsg.Text = "";
            GridViewProcedureInfo.Rows.Clear();
            if (ValidateForm())
            {
                RptConsultedProcedure RptConsultedProcedure = new RptConsultedProcedure();
                RptConsultedProcedure.PatientId = PatientId;
                RptConsultedProcedure.FromDate = (DateTime)FromDate.Date!;
                RptConsultedProcedure.ToDate = (DateTime)ToDate.Date!;
                RptConsultedProcedure.CompanyId = Global.Company.CompanyId;
                RptConsultedProcedure.ProceduresStatuses = GetStatus();
                List<ProcedureStatus> Status = GetStatus();
                RptConsultedProcedure.GenerateReport();
                if (RptConsultedProcedure.LineItems.Count > 0)
                {
                    IList<ConsultedProcedureLineItem> FilteredLineItems = RptConsultedProcedure.LineItems.Where(x => Status.Contains(x.Status)).ToList();
                    if (FilteredLineItems.Count > 0)
                    {
                        EnableButton(true);
                        int i = 0;
                        foreach (ConsultedProcedureLineItem Item in RptConsultedProcedure.LineItems.Where(x => Status.Contains(x.Status)))
                        {
                            GridViewProcedureInfo.Rows.Add();
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.SNO].Value = i + 1;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.DATE].Value = Item.Date;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.NAME].Value = Item.Name;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.DESC].Value = Item.Desc;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.REQUBY].Value = Item.RequestedBy;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.STATUS].Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Item.Status.ToString().ToLower());
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.PERBY].Value = Item.PerformBy;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.PERON].Value = Item.PerformOn != null ? Item.PerformOn : null;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.NOTE].Value = Item.Note;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.FEE].Value = Item.Fee;
                            GridViewProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.ID].Value = Item.Id;
                            i++;
                        }
                    }
                    else
                    {
                        ProcedureErrorMsg.Text = NoRecordErrorMsg;
                    }
                }
                else
                {
                    ProcedureErrorMsg.Text = NoRecordErrorMsg;
                }
            }
        }
        private List<ProcedureStatus> GetStatus()
        {
            List<ProcedureStatus> Status = new List<ProcedureStatus>();
            foreach (ComboTreeNode node in ComboBoxStatus.Nodes)
            {
                if (node.Checked)
                {
                    if (node.Text == "Requested")
                    {
                        Status.Add(ProcedureStatus.REQUESTED);
                    }
                    else if (node.Text == "In Progress")
                    {
                        Status.Add(ProcedureStatus.INPROGRESS);
                    }
                    else if (node.Text == "Cancelled")
                    {
                        Status.Add(ProcedureStatus.CANCEL);
                    }
                    else
                    {
                        Status.Add(ProcedureStatus.COMPLETED);
                    }
                }
            }
            return Status;
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            LoadPatientProcedure();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F3))
            {
                BtnUpdate.PerformClick();
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TextBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnGo.PerformClick();
            }
        }

        private void GridViewProcedureInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}
