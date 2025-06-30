using fa.api.Accounting;
using fa.api.Hms;
using fa.libraries.utils;
using fa.model.Employee;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.UserProfile;
using fa.views.controls.ComboTreeView;
using fa.views.hms.Masters;
using fa.views.hms.patient;
using FADataAccessLibrary.Api.Hms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.hms.helper
{
    public enum ConsultedConsultationTableColumn
    {
        NAME, DESC, FEE, REMOVE, ID, CID, CONSNAME
    }
    public enum ConsultationFeeTotalTableColumn
    {
        SPACE, TOTAL, REMOVE
    }
    public partial class FormSelectConsultation : FormPatientBase
    {
        public DataGridViewComboBoxCell? SelectedConsultationsids;
        public DataGridViewComboBoxCell? SelectedConsultationsFees;
        public DataGridViewComboBoxCell? SelectedConsultationsDiscrp;
        public string? SelectedConsultationsNames;
        public long? PatientOpId;
        ConsultationDetail consultationDetail = null!;
        List<long> ConsultationIds = new List<long>();
        public List<long> Oldids = new List<long>();
        public List<long> Newids = new List<long>();
        public bool isDirty = false;
        DoctorConsultationManager DoctorConsultationManager = null!;
        ConsultationNoteManager ConsultationNoteManager = null!;

        public static string Grid_ConfirmRowDeleteText = "Do you want to delete consultation {0}?";
        public static string Grid_ConfirmFeeChangeText = "Do you want to save the consultation fee for {0} as {1} for consultant {2}?";
        public static string SelectConsultationErrorMsg = "Please select consultation.";
        public static string ZeroAmountErrorMsg = "Please type the amount.";
        public static string NoDescriptionErrorMsg = "Please type the description";

        FormPatientBase parent = null!;
        public FormSelectConsultation(Object Sender)
        {
            if (Sender is FormConsulting)
            {
                parent = (FormConsulting)Sender;
            }
            DoctorConsultationManager = DoctorConsultationManager.Instance;
            ConsultationNoteManager = ConsultationNoteManager.Instance;
            InitializeComponent();
        }

        private void FormSelectConsultation_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ResetForm();
            GetConsultationsId();
            LoadConsultationWithFilter();
            LoadSelectedConsultation();
            ConsultationSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }
        private void ResetForm()
        {
            ErrorMsgSelectConsultation.Text = string.Empty;
            ConsultationSearchTextBox.Text = string.Empty;
            GridViewSelectConsultation.Rows.Clear();
        }
        private void GetConsultationsId()
        {
            Oldids = new List<long>();
            Newids = new List<long>();
            ConsultationIds = new List<long>();
            if (SelectedConsultationsids != null && SelectedConsultationsids.Items.Count > 0)
            {
                foreach (var Id in SelectedConsultationsids.Items)
                {
                    Oldids.Add(long.Parse(Id.ToString()!));
                    Newids.Add(long.Parse(Id.ToString()!));
                    ConsultationIds.Add(long.Parse(Id.ToString()!));
                }
            }
        }
        private void LoadConsultationWithFilter()
        {
            string FilterString = ConsultationSearchTextBox.Text.Trim();
            ConsultationCheckedListBox.Items.Clear();
            foreach (var lPrescription in Global.ConsultationDetailList)
            {
                if (FilterString == null || string.IsNullOrEmpty(FilterString.Trim()) || lPrescription.Name.IndexOf(FilterString, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ConsultationCheckedListBox.Items.Add(lPrescription, ConsultationIds.Contains(lPrescription.Id) ? true : false);
                }
            }
            if (ConsultationCheckedListBox.Items.Count > 0)
            {
                ConsultationCheckedListBox.SelectedIndex = 0;
            }
        }
        private void ConsultationSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadConsultationWithFilter();
        }

        private void ConsultationCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index > -1)
            {
                var item = (Consultation)ConsultationCheckedListBox.Items[e.Index];
                if (!ConsultationCheckedListBox.GetItemChecked(e.Index))
                {
                    if (!ConsultationIds.Contains(item.Id))
                    {
                        ConsultationIds.Add(item.Id);
                        InsertTickedConsultation(item.Id);
                    }
                }
                else
                {
                    ConsultationIds.Remove(item.Id);
                    RemoveUnCheckedConsultation(item.Id.ToString());
                }
                CalculateFeeTotal();
            }
        }
        private void LoadSelectedConsultation()
        {
            if (ConsultationIds.Count > 0)
            {
                for (int p = 0; p < ConsultationIds.Count; p++)
                {
                    Consultation Con = Global.ConsultationDetailList.FirstOrDefault(x => x.Id == ConsultationIds[p])!;
                    if (Con != null)
                    {
                        GridViewSelectConsultation.Rows.Add();
                        int i = GridViewSelectConsultation.Rows.Count - 1;
                        GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.NAME].Value = Con.Name;
                        GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.DESC].Value = SelectedConsultationsDiscrp!.Items[p].ToString();
                        GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CONSNAME].Value = Con.Name;
                        GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.FEE].Value = SelectedConsultationsFees!.Items[p] /* GetLastUpdatedFee(Con.Id)*/;
                        GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CID].Value = Con.Id;
                    }
                }
            }
            CalculateFeeTotal();
        }
        private void InsertTickedConsultation(long ConsultationId)
        {
            User user = Global.User;
            Consultation Con = Global.ConsultationDetailList.FirstOrDefault(x => x.Id == ConsultationId)!;

            if (user.EmployeeId.HasValue)
            {
                consultationDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(
                    Global.Company.CompanyId,
                    user.EmployeeId.Value,
                    Con);
            }
            else if (PatientOpId.HasValue)
            {
                Registration registration = OpManager.Instance.GetRegisterByRegId(PatientOpId.Value);
                if (registration.RequestedDoctorId.HasValue)
                {
                    consultationDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(
                        Global.Company.CompanyId,
                        registration.RequestedDoctorId.Value,
                        Con);
                }
                else
                {
                    consultationDetail = null!;
                }
            }
            else
            {
                consultationDetail = null!;
            }

            if (Con != null)
            {
                GridViewSelectConsultation.Rows.Add();
                int i = GridViewSelectConsultation.Rows.Count - 1;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.NAME].Value = Con.Name;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.DESC].Value = Con.Discription;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CONSNAME].Value = Con.Name;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.FEE].Value = consultationDetail?.Fee ?? 0.00;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CID].Value = Con.Id;
            }
        }

        private void InsertTickedConsultationforModification(long ConsultationId)
        {
            User user = Global.User;
            Consultation Con = Global.ConsultationDetailList.FirstOrDefault(x => x.Id == ConsultationId)!;
            if (user.EmployeeId != null)
            {
                consultationDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(Global.Company.CompanyId, (long)user.EmployeeId, Con);
            }
            else if (PatientOpId != null)
            {
                Registration registration = OpManager.Instance.GetRegisterByRegId((long)PatientOpId!);
                consultationDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(Global.Company.CompanyId, (long)registration.RequestedDoctorId!, Con);
            }
            else
            {
                consultationDetail = null!;
            }
            if (Con != null)
            {
                GridViewSelectConsultation.Rows.Add();
                int i = GridViewSelectConsultation.Rows.Count - 1;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.NAME].Value = Con.Name;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.DESC].Value = Con.Discription;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CONSNAME].Value = Con.Name;

                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.FEE].Value = consultationDetail != null ? consultationDetail.Fee : 0.00;
                GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CID].Value = Con.Id;
            }
        }
        private void RemoveUnCheckedConsultation(string ConsId)
        {
            int Index = -1;
            for (int i = 0; i < GridViewSelectConsultation.Rows.Count; i++)
            {
                if (GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CID].Value.ToString() == ConsId)
                {
                    Index = GridViewSelectConsultation.Rows[i].Index;
                }
                if (Index > -1)
                {
                    GridViewSelectConsultation.Rows.RemoveAt(Index);
                    break;
                }
            }

        }
        private void BtnSelectConsultationCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            GetConsultationsId();
            LoadConsultationWithFilter();
            LoadSelectedConsultation();
            ConsultationSearchTextBox.Select();
        }
        private void BtnSelectConsultationDone_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                SelectedConsultationsids = new DataGridViewComboBoxCell();
                SelectedConsultationsFees = new DataGridViewComboBoxCell();
                SelectedConsultationsDiscrp = new DataGridViewComboBoxCell();
                SelectedConsultationsNames = string.Empty;
                Newids = new List<long>();
                if (GridViewSelectConsultation.Rows.Count > 0)
                {
                    for (int i = 0; i < GridViewSelectConsultation.Rows.Count; i++)
                    {
                        SelectedConsultationsids.Items.Add(GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CID].Value.ToString());
                        SelectedConsultationsFees.Items.Add(GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString());
                        SelectedConsultationsDiscrp.Items.Add(string.IsNullOrEmpty(GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.DESC].Value?.ToString()) ? string.Empty : GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.DESC].Value);
                        Consultation Consultation = ConsultationManager.Instance.GetConsultationById(long.Parse(GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CID].Value.ToString()!));
                        SelectedConsultationsNames = string.IsNullOrEmpty(SelectedConsultationsNames) ? Consultation.Name : SelectedConsultationsNames + ",\n" + Consultation.Name;
                        Newids.Add(long.Parse(GridViewSelectConsultation.Rows[i].Cells[(int)ConsultedConsultationTableColumn.CID].Value.ToString()!));
                    }
                }
                this.Close();
            }
        }
        private Boolean Validation()
        {
            ErrorMsgSelectConsultation.Text = string.Empty;
            int Count = GridViewSelectConsultation.Rows.Count;
            if (Count < 1)
            {
                ErrorMsgSelectConsultation.Text = SelectConsultationErrorMsg;
                return false;
            }
            if (Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (GridViewSelectConsultation.Rows[i].Cells[1].Value == null || string.IsNullOrWhiteSpace(GridViewSelectConsultation.Rows[i].Cells[1].Value.ToString()))
                    {
                        GridViewSelectConsultation.Select();
                        GridViewSelectConsultation.CurrentCell = GridViewSelectConsultation[1, i];
                        GridViewSelectConsultation.BeginEdit(true);
                        ErrorMsgSelectConsultation.Text = NoDescriptionErrorMsg;
                        return false;
                    }
                    if (GridViewSelectConsultation.Rows[i].Cells[2].Value == null || Double.Parse(GridViewSelectConsultation.Rows[i].Cells[2].Value.ToString()!) == 0)
                    {
                        GridViewSelectConsultation.Select();
                        GridViewSelectConsultation.CurrentCell = GridViewSelectConsultation[2, i];
                        GridViewSelectConsultation.BeginEdit(true);
                        ErrorMsgSelectConsultation.Text = ZeroAmountErrorMsg;
                        return false;
                    }
                }
            }
            return true;
        }

        private void GridViewSelectConsultation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)ConsultedConsultationTableColumn.REMOVE)
                {
                    DialogResult Result = MessageBox.Show(string.Format(Grid_ConfirmRowDeleteText, GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.NAME].Value.ToString()), "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Result == DialogResult.Yes)
                    {
                        long UnCheckConsultation = long.Parse(GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.CID].Value.ToString()!);
                        GridViewSelectConsultation.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewSelectConsultation.Rows.RemoveAt(e.RowIndex);
                        ConsultationIds.Remove(UnCheckConsultation);
                        int Index = ConsultationCheckedListBox.Items.IndexOf(Global.ConsultationDetailList.FirstOrDefault(x => x.Id == UnCheckConsultation)!);
                        if (Index > -1)
                        {
                            ConsultationCheckedListBox.SetItemChecked(Index, false);
                        }
                        CalculateFeeTotal();
                    }
                }
            }
        }
        private void GridViewSelectConsultation_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.DESC].ReadOnly = true;
            GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].ReadOnly = true;
            GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.REMOVE].ReadOnly = true;
            GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.NAME].ReadOnly = true;
            GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.DESC].ReadOnly = false;
            GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].ReadOnly = false;
        }
        private void GridViewSelectConsultation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void CalculateFeeTotal()
        {
            double FeeTotal = 0.00;
            foreach (DataGridViewRow Row in GridViewSelectConsultation.Rows)
            {
                FeeTotal = FeeTotal + double.Parse(Row.Cells[(int)ConsultedConsultationTableColumn.FEE].Value != null ? Row.Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString()! : "0");
            }
            GridViewSelectConsultationTotal.Rows[0].Cells[(int)ConsultationFeeTotalTableColumn.TOTAL].Value = FeeTotal;
            GridViewSelectConsultationTotal.Rows[0].Cells[(int)ConsultationFeeTotalTableColumn.SPACE].Value = "Total";

        }
        private double GetLastUpdatedFee(long ConsId)
        {
            double Fee = 0.00;
            ConsultedDoctorConsultationFee DoctorConsultation = DoctorConsultationManager.GetLatestDoctorConsultationByConsultationsandConsultantId(ConsId, Global.User.UserId);
            if (DoctorConsultation != null)
            {
                Fee = DoctorConsultation.Fee;
            }
            else
            {
                Consultation Consultation = ConsultationManager.Instance.GetConsultationById(ConsId);
                if (Consultation != null)
                {
                    Fee = Consultation.Fee;
                }
            }
            return Fee;
        }
        double Fee = 0.00;
        private void GridViewSelectConsultation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ConsultedConsultationTableColumn.FEE &&
               GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].Value != null)
            {
                if (Fee != double.Parse(GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString()!))
                {
                    DialogResult Result = DialogResult.No;
                    Employee employee = null!;
                    if (Global.User.EmployeeId != null || PatientOpId.HasValue)
                    {
                        if (Global.User.EmployeeId != null)
                        {
                            employee = EmployeeManager.Instance.GetEmployeeInfoById((long)Global.User.EmployeeId!);
                        }
                        else if (PatientOpId.HasValue)
                        {
                            Registration registration = OpManager.Instance.GetRegisterByRegId(PatientOpId!.Value);
                            if (registration.RequestedDoctorId.HasValue)
                            {
                                employee = EmployeeManager.Instance.GetEmployeeInfoById((long)registration.RequestedDoctorId);
                            }
                        }
                        if (employee != null)
                        {
                            if (employee.IsServiceProvider)
                            {
                                Result = MessageBox.Show(string.Format(Grid_ConfirmFeeChangeText,
                                GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.CONSNAME].Value.ToString(),
                                GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString(), employee.Name), "Update Confirm",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            }
                            else
                            {
                                Employee lemployee = new Employee();
                                lemployee = employee;
                                lemployee.Id = employee.Id;
                                lemployee.IsServiceProvider = true;
                                EmployeeManager.Instance.UpdateEmployee(lemployee);
                                IList<Consultation> Consultations = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
                                foreach (Consultation consultation in Consultations)
                                {
                                    ConsultationDetail lconsultationDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(Global.Company.CompanyId, lemployee.Id, consultation);
                                    if (lconsultationDetail == null)
                                    {
                                        ConsultationDetail llconsultationDetail = new ConsultationDetail();
                                        llconsultationDetail.ConsultationId = consultation.Id;
                                        llconsultationDetail.EmployeeId = lemployee.Id;
                                        llconsultationDetail.Fee = 0.00;
                                        llconsultationDetail.CompanyId = Global.Company.CompanyId;
                                        ConsultationDetailManager.Instance.AddConsultationDetail(llconsultationDetail);
                                        consultationDetail = llconsultationDetail;
                                    }
                                }
                                Result = DialogResult.Yes;
                            }
                        }
                    }
                    if (Result == DialogResult.No)
                    {
                        CalculateFeeTotal();
                    }
                    else
                    {
                        long ConsultationId = long.Parse(GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.CID].Value.ToString()!);
                        Consultation lConsultationById = ConsultationManager.Instance.GetConsultationById(ConsultationId);
                        if (lConsultationById != null)
                        {
                            Consultation ConsultationFromDB = null!;
                            Consultation lConsultation = new Consultation();
                            lConsultation.Id = ConsultationId;
                            lConsultation.Name = GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.NAME].Value.ToString();
                            lConsultation.Discription = lConsultationById.Discription;
                            lConsultation.DisplayAs = lConsultationById.DisplayAs;
                            lConsultation.CompanyId = Global.Company.CompanyId;
                            if (employee != null)
                            {
                                lConsultation.ConsultationDetail = new List<ConsultationDetail>();
                                foreach (ConsultationDetail detail in lConsultationById.ConsultationDetail)
                                {
                                    if (detail.EmployeeId == employee.Id)
                                    {
                                        ConsultationDetail oldDetail = ConsultationDetailManager.Instance.GetConsultationDetailByEmployeeId(Global.Company.CompanyId, (long)detail.EmployeeId!, lConsultationById);
                                        ConsultationDetail lDetail = new ConsultationDetail();
                                        lDetail.EmployeeId = detail.EmployeeId;
                                        lDetail.ConsultationId = detail.ConsultationId;
                                        lDetail.CompanyId = Global.Company.CompanyId;
                                        lDetail.Id = detail.Id;
                                        lDetail.Fee = double.Parse(GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString()!);
                                        lConsultation.ConsultationDetail.Remove(oldDetail);
                                        lConsultation.ConsultationDetail.Add(lDetail);
                                    }
                                    else
                                    {
                                        lConsultation.ConsultationDetail.Add(detail);
                                    }
                                }
                            }
                            else
                            {
                                lConsultation.Fee = double.Parse(GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString()!);
                            }
                            ConsultationFromDB = ConsultationManager.Instance.UpdateConsultation(lConsultation);
                        }
                        CalculateFeeTotal();
                    }
                }
            }
        }
        private void GridViewSelectConsultation_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ConsultedConsultationTableColumn.FEE &&
                GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.NAME].Value != null &&
                GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].Value != null)
            {
                Fee = double.Parse(GridViewSelectConsultation.Rows[e.RowIndex].Cells[(int)ConsultedConsultationTableColumn.FEE].Value.ToString()!);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnSelectConsultationDone.PerformClick();
            }
            if (keyData == (Keys.Escape))
            {
                BtnSelectConsultationCancel.PerformClick();
                return true;
            }
            if (keyData == (Keys.F3))
            {
                BtnConsultationsNew.PerformClick();
            }
            if (keyData == (Keys.Tab | Keys.Shift) && this.ActiveControl == this.BtnSelectConsultationDone)
            {
                GridViewSelectConsultation.Select();
                GridViewSelectConsultation.CurrentCell = GridViewSelectConsultation[1, 0];
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnSelectConsultationCancel.PerformClick();
                return true;
            }
            try
            {
                if (GridViewSelectConsultation.CurrentCell != null)
                {
                    if (keyData == (Keys.Tab) && GridViewSelectConsultation.CurrentCell.ColumnIndex == (int)ConsultedConsultationTableColumn.FEE)
                    {
                        if (GridViewSelectConsultation.CurrentCell.RowIndex != GridViewSelectConsultation.Rows.Count - 1)
                        {
                            SendKeys.Send("{tab}{tab}");
                        }
                        else
                        {
                            GridViewSelectConsultation.CurrentCell = null;
                            BtnSelectConsultationDone.Select();
                        }
                    }
                    if (keyData == (Keys.Tab | Keys.Shift) && GridViewSelectConsultation.CurrentCell!.ColumnIndex == (int)ConsultedConsultationTableColumn.DESC)
                    {
                        SendKeys.Send("{tab}{tab}");
                    }
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GridViewSelectConsultation_Enter(object sender, EventArgs e)
        {
            if (GridViewSelectConsultation.Rows.Count > 1)
            {
                GridViewSelectConsultation.CurrentCell = GridViewSelectConsultation[1, 0];
            }
        }

        private void BtnRefreshConsultation_Click(object sender, EventArgs e)
        {
            ButtonRefreshConsultation();
        }
        public void ButtonRefreshConsultation()
        {
            Cursor.Current = Cursors.WaitCursor;
            Global.ConsultationDetailList = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
            LoadConsultationWithFilter();
            ConsultationSearchTextBox.Select();
            Cursor.Current = Cursors.Default;
        }

        private void ConsultationCheckedListBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewSelectConsultation.Rows.Count > 0)
                {
                    GridViewSelectConsultation.Select();
                    GridViewSelectConsultation.CurrentCell = GridViewSelectConsultation[1, 0];
                    GridViewSelectConsultation.BeginEdit(true);
                }
                else
                {
                    BtnSelectConsultationDone.Select();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                ConsultationSearchTextBox.Select();
            }
        }

        private void BtnSelectConsultationDone_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                ConsultationSearchTextBox.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewSelectConsultation.Rows.Count > 0)
                {
                    GridViewSelectConsultation.Select();
                    GridViewSelectConsultation.CurrentCell = GridViewSelectConsultation[1, GridViewSelectConsultation.Rows.Count - 1];
                    GridViewSelectConsultation.BeginEdit(true);
                }
                else
                {
                    ConsultationCheckedListBox.Select();
                }
            }
        }

        private void BtnConsultationsNew_Click(object sender, EventArgs e)
        {
            FormConsultations FormConsultations = new FormConsultations(true);
            FormConsultations.CreateConsultationsOnLoad = true;
            FormConsultations.ShowDialog(this);
            ButtonRefreshConsultation();
        }

        private void GridViewSelectConsultation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 2)
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                }
                else
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                }
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
            }
        }
    }
}
