using fa.api.Hms;
using fa.api.utils;
using fa.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.hms.patient
{
    public partial class FormPatientSearchForConsulting : FormPatientBase
    {
        public static string NothingFoundMsg = "No patient found!";
        public static string EnterSearchTextMsg = "Please enter search text, it could patient Name or Phone Number or Date of Birth.";
        public static string SelectPatientErrorMsg = "Please select patient";
        public static string SearchPatientErrorMsg = "Please enter at least 3 characters to search.";

        public PatientSearchType PatientSearchType = PatientSearchType.SearchPatient;
        Container parent = null!;
        public FormPatientSearchForConsulting(object sender)
        {
            parent = (Container)sender;
            InitializeComponent();
        }

        private void FormPatientSearchForConsulting_Load(object sender, EventArgs e)
        {
            ResetForm();
            if (PatientSearchType == PatientSearchType.SearchPatient)
            {
                this.Text = "Patient Search For Consulting";
                BtnConsultingPatientConsulting.Size = new Size(88, 23);
                BtnConsultingPatientConsulting.Location = new Point(781, 367);
                BtnConsultingPatientConsulting.Text = "Consult[F8]";
            }
            else if (PatientSearchType == PatientSearchType.PatientProcedure)
            {
                this.Text = "Patient Search For Procedure";
                BtnConsultingPatientConsulting.Size = new Size(158, 23);
                BtnConsultingPatientConsulting.Location = new Point(711, 367);
                BtnConsultingPatientConsulting.Text = "Perform Procedures [F8]";
            }
            TextBoxSearchPatientByName.TextBox.Select();
        }
        private IList<Patient> SearchPatient(string text)
        {
            IList<Patient> PatientInfo = null!;
            PatientInfo = PatientManager.Instance.GetPatientBySearchText(text, Global.Company.CompanyId);
            if (PatientInfo == null || PatientInfo.Count == 0)
            {
                if (TextUtils.isPatientNumber(text))
                {
                    PatientInfo = PatientManager.Instance.GetPatientById(text, Global.Company.CompanyId);
                    if (PatientInfo == null || PatientInfo.Count == 0)
                    {
                        PatientInfo = PatientManager.Instance.GetPatientByPhoneNumber(text, Global.Company.CompanyId, PatientSearchType.Patient);
                    }
                }
                else if (DateUtils.ValidDate_TillCurrentDate(text, Global.Company.DateFormat))
                {
                    DateTime DOB = (DateTime)DateUtils.ToDate(text, Global.Company.DateFormat)!;
                    PatientInfo = PatientManager.Instance.GetPatientByDOB(DOB, Global.Company.CompanyId, PatientSearchType.Patient);
                }
                else if (TextUtils.IsPhoneNumber(text))
                {
                    PatientInfo = PatientManager.Instance.GetPatientByPhoneNumber(text, Global.Company.CompanyId, PatientSearchType.Patient);
                }
                else
                {
                    PatientInfo = PatientManager.Instance.GetPatientByName(text, Global.Company.CompanyId, PatientSearchType.Patient);
                }
            }
            return PatientInfo;
        }
        
        private void LoadPatients(IList<Patient> PatientInfo)
        {
            if (PatientInfo.Count > 0)
            {
                GridViewPatientSearchResult.Rows.Clear();
                GridViewPatientSearchResult.Rows.Add(PatientInfo.Count);
                int i = 0;
                foreach (var lPatientInfo in PatientInfo)
                {
                    GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.NAME].Value = lPatientInfo.Name;
                    GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.ADDRESS].Value = lPatientInfo.Address.FullAddress;
                    if (DateUtils.ValidDate_TillCurrentDate(lPatientInfo.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                    {
                        GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.DOB].Value = lPatientInfo.DateOfBirth.ToString(Global.Company.DateFormat);
                    }
                    GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.PID].Value = lPatientInfo.Id;
                    GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.PNUM].Value = lPatientInfo.PatientNumber;
                    if (lPatientInfo.GetStatus(Global.getTransactionDate()) == "In OP")
                    {
                        Registration OpRegistration = OpManager.Instance.GetlastOPRecord((long)lPatientInfo.Id , lPatientInfo.CompanyId);
                        GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.ASSIGNEE].Value = OpRegistration.RequestedDoctor != null ? OpRegistration.RequestedDoctor.Name : "";
                    }
                    else if (lPatientInfo.GetStatus(Global.getTransactionDate()) == "In IP")
                    {
                        InPatientAdmission InPatientAdmissions = IpManager.Instance.GetAdmittedInPatientAdmissionByPatientId((long)lPatientInfo.Id);
                        long IpAdmitId = InPatientAdmissions.Id;
                        MedicalTeam IpMedicalTeam = MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionId(IpAdmitId);
                        GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.ASSIGNEE].Value = IpMedicalTeam.PrimaryDoctor.Name;
                    }
                    else
                    {
                        GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.ASSIGNEE].Value = "";
                    }
                    GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.STATUS].Value = lPatientInfo.GetStatus(Global.getTransactionDate());
                    i++;
                }
            }
            else
            {
                GridViewPatientSearchResult.Rows.Clear();
                PatientConsultingSearchErrorMsg.Text = NothingFoundMsg;
            }
        }

        private void BtnSearchPatientByName_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                GridViewPatientSearchResult.Rows.Clear();
                PatientConsultingSearchErrorMsg.Text = "";
                string searchText = TextBoxSearchPatientByName.Text.Trim();

                if (searchText.Length < 3)
                {
                    PatientConsultingSearchErrorMsg.Text = SearchPatientErrorMsg;
                    TextBoxSearchPatientByName.Select();
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (isValidSearchCriteria())
                {
                    IList<Patient> PatientInfo = SearchPatient(TextBoxSearchPatientByName.Text.Trim());
                    LoadPatients(PatientInfo);
                }
                if (GridViewPatientSearchResult.Rows.Count > 0)
                {
                    BtnConsultingPatientConsulting.Enabled = true;
                    GridViewPatientSearchResult.Select();
                    GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult[0, 0];
                }
                else
                {
                    BtnConsultingPatientConsulting.Enabled = false;
                    BtnSearchPatientByName.Select();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool isValidSearchCriteria()
        {
            string searchText = TextBoxSearchPatientByName.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                PatientConsultingSearchErrorMsg.Text = EnterSearchTextMsg;
                TextBoxSearchPatientByName.Select();
                return false;
            }
            return true;
        }
        private void ResetForm()
        {
            TextBoxSearchPatientByName.TextBox.ResetText();
            PatientConsultingSearchErrorMsg.Text = "";
            GridViewPatientSearchResult.Rows.Clear();
            BtnConsultingPatientConsulting.Enabled = false;
        }

        private void TextBoxSearchPatientByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearchPatientByName_Click(sender, e);
            }
            if (e.KeyCode == Keys.F2)
            {
                BtnSearchPatientByName_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down)
            {
                if (GridViewPatientSearchResult.Rows.Count > 0)
                {
                    GridViewPatientSearchResult.Select();
                    GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult[0, 0];
                }
            }
        }

        private void GridViewPatientSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8 || (GridViewPatientSearchResult != null && GridViewPatientSearchResult.Focused && keyData == Keys.Enter))
            {
                BtnConsultingPatientConsulting.PerformClick();
                return true;
            }
            try
            {
                if(ActiveControl == GridViewPatientSearchResult)
                {
                    if (keyData == Keys.Tab && GridViewPatientSearchResult != null && GridViewPatientSearchResult.CurrentCell != null && GridViewPatientSearchResult.CurrentCell.ColumnIndex == 0)
                    {
                        SendKeys.Send("{tab}{tab}{tab}{tab}");
                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && GridViewPatientSearchResult != null && GridViewPatientSearchResult.CurrentCell != null && GridViewPatientSearchResult.CurrentCell.ColumnIndex == 0)
                    {
                        if (GridViewPatientSearchResult.CurrentRow != null && GridViewPatientSearchResult.CurrentRow.Index != 0)
                        {
                            SendKeys.Send("{tab}{tab}{tab}{tab}");
                        }                                                                       
                    }
                }
            }
            catch
            {
                MessageBox.Show("Unexpected error! Please contact your administrator");
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        private void BtnConsultingPatientConsulting_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (GridViewPatientSearchResult.Rows.Count > 0)
                {
                    GridViewPatientSearchResult.Select();
                    GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult[0, 0];
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewPatientSearchResult.Rows.Count > 0)
                {
                    GridViewPatientSearchResult.Select();
                    GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult[0, GridViewPatientSearchResult.Rows.Count - 1];
                }

            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.IsInputKey = true;
                if (BtnConsultingPatientConsulting.Focused) { BtnConsultingPatientConsulting.Select(); }
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
            {
                e.IsInputKey = true;
                if (BtnConsultingPatientConsulting.Focused) { BtnConsultingPatientConsulting.Select(); }

            }
        }

        private void GridViewPatientSearchResult_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private void BtnConsultingPatientConsulting_Click(object sender, EventArgs e)
        {
            if (GridViewPatientSearchResult.Rows.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (GridViewPatientSearchResult.CurrentRow.Index > -1)
                {
                    int selectedRowIndex = GridViewPatientSearchResult.CurrentRow.Index;
                    if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.PID].Value != null)
                    {
                        this.PatientIdTransport.Text = GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.PID].Value.ToString();
                        if (PatientSearchType == PatientSearchType.SearchPatient)
                        {
                            FormConsulting formConsulting = new FormConsulting(this);
                            InPatientAdmission InPatientAdmission = IpManager.Instance.GetUnDischargedInPatientAdmissionByPatientId((long)GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.PID].Value);
                            if (InPatientAdmission != null)
                            {
                                if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value == null || GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value.ToString() == string.Empty)
                                {
                                    formConsulting.PatientType = PatientTypes.ImPatient;
                                }
                                if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value.ToString() == "In IP")
                                {
                                    formConsulting.PatientType = PatientTypes.InPatient;
                                }
                                if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value.ToString() == "In OP")
                                {
                                    formConsulting.PatientType = PatientTypes.OutPatient;
                                }
                                formConsulting.PatientOpId = InPatientAdmission.OpRegistrationId;
                                formConsulting.PatientIPId = InPatientAdmission.Id;
                            }
                            else
                            {
                                Registration Registration = OpManager.Instance.GetOpByPatientId((long)GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.PID].Value, Global.getTransactionDate());
                                if (Registration != null)
                                {
                                    if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value == null || GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value.ToString() == string.Empty)
                                    {
                                        formConsulting.PatientType = PatientTypes.ImPatient;
                                    }
                                    if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value.ToString() == "In IP")
                                    {
                                        formConsulting.PatientType = PatientTypes.InPatient;
                                    }
                                    if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value.ToString() == "In OP")
                                    {
                                        formConsulting.PatientType = PatientTypes.OutPatient;
                                    }
                                    formConsulting.PatientOpId = Registration.Id;
                                }
                                else
                                {
                                    if (GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value == null || GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.STATUS].Value.ToString() == string.Empty)
                                    {
                                        formConsulting.PatientType = PatientTypes.ImPatient;
                                    }
                                }
                            }
                            formConsulting.ShowDialog();
                        }
                        else if (PatientSearchType == PatientSearchType.PatientProcedure)
                        {
                            FormPatientProcedures PatientProceduresList = new FormPatientProcedures(this);
                            PatientProceduresList.ShowDialog();
                        }
                        GridViewPatientSearchResult.ClearSelection();
                        if (selectedRowIndex >= 0 && selectedRowIndex < GridViewPatientSearchResult.Rows.Count)
                        {
                            GridViewPatientSearchResult.Rows[selectedRowIndex].Selected = true;
                            GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult.Rows[selectedRowIndex].Cells[0];
                        }
                    }
                }
                else
                {
                    PatientConsultingSearchErrorMsg.Text = SelectPatientErrorMsg;
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void GridViewPatientSearchResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                BtnConsultingPatientConsulting_Click(sender, e);
            }
        }
    }
}
