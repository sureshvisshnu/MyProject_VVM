using fa.libraries.Validation;
using fa.views.hms.op;
using fa.api.Hms;
using fa.api.utils;
using fa.model.Hms.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Hms.Ip;
using fa.model.Common;
using fa.api.Accounting;
using fa.model.Hms.Op;
using fa.views.controls.hms;
using fa.views.hms.ip;

namespace fa.views.hms.patient
{
    public enum PatientSearchGridColumn
    {
        NAME, ADDRESS, DOB, PNUM, ASSIGNEE, STATUS, PID
    }
    public partial class FormPatientSearch : FormBase
    {
        public static string NothingFoundMsg = "No patient found!";
        public static string EnterSearchTextMsg = "Please enter search text, it could patient Name or Phone Number or Date of Birth.";
        public static string SelectPatientErrorMsg = "Please select patient";
        public static string SearchPatientErrorMsg = "Please enter at least 3 characters to search.";
        public string Searchstring = string.Empty;
        public PatientSearchType PatientSearchType = PatientSearchType.Patient;
        FormPatientBase parent = null!;
        PatientManager PatientManager = null!;
        DateValidation DateValidation = null!;
        IpManager IpManager = null!;

        public FormPatientSearch(object sender)
        {
            parent = (FormPatientBase)sender;
            PatientManager = PatientManager.Instance;
            IpManager = IpManager.Instance;
            DateValidation = DateValidation.Instance;
            InitializeComponent();
        }

        private void FormPatientSearch_Load(object sender, EventArgs e)
        {
            ResetForm();
            if (PatientSearchType == PatientSearchType.Inpatient)
            {
                TextBoxSearchPatientByName.Text = Searchstring;
                BtnSearchPatientByName_Click(sender, e);
            }
            else if (PatientSearchType == PatientSearchType.SearchPatient)
            {
                TextBoxSearchPatientByName.Text = Searchstring;
                BtnSearchPatientByName_Click(sender, e);
            }
            else
            {
                TextBoxSearchPatientByName.TextBox.Select();
            }

        }
        private IList<Patient> SearchInPatient(string SearchText)
        {
            IList<Patient> PatientInfo = null!;
            //PatientInfo = PatientManager.GetPatientBySearchText(SearchText, Global.Company.CompanyId);
            if (PatientInfo == null || PatientInfo.Count == 0)
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    PatientInfo = PatientManager.ListAllIpPatient();
                }
                else if (DateUtils.ValidDate_TillCurrentDate(SearchText, Global.Company.DateFormat))
                {
                    DateTime DOB = (DateTime)DateUtils.ToDate(SearchText, Global.Company.DateFormat)!;
                    PatientInfo = PatientManager.GetPatientByDOB(DOB, Global.Company.CompanyId, PatientSearchType.Inpatient);
                }
                else if (TextUtils.IsPhoneNumber(SearchText))
                {
                    PatientInfo = PatientManager.GetPatientByPhoneNumber(SearchText, Global.Company.CompanyId, PatientSearchType.Inpatient);
                }
                else
                {
                    PatientInfo = PatientManager.GetPatientByName(SearchText, Global.Company.CompanyId, PatientSearchType.Inpatient);
                }
            }
            return PatientInfo;
        }
        private IList<Patient> SearchPatient(string text)
        {
            IList<Patient> PatientInfo = null!;
            PatientInfo = PatientManager.GetPatientBySearchText(text, Global.Company.CompanyId);
            if (PatientInfo == null || PatientInfo.Count == 0)
            {
                if (TextUtils.isPatientNumber(text))
                {
                    PatientInfo = PatientManager.GetPatientById(text, Global.Company.CompanyId);
                    if (PatientInfo == null || PatientInfo.Count == 0)
                    {
                        PatientInfo = PatientManager.GetPatientByPhoneNumber(text, Global.Company.CompanyId, PatientSearchType.Patient);
                    }
                }
                else if (DateUtils.ValidDate_TillCurrentDate(text, Global.Company.DateFormat))
                {
                    DateTime DOB = (DateTime)DateUtils.ToDate(text, Global.Company.DateFormat)!;
                    PatientInfo = PatientManager.GetPatientByDOB(DOB, Global.Company.CompanyId, PatientSearchType.Patient);
                }
                else if (TextUtils.IsPhoneNumber(text))
                {
                    PatientInfo = PatientManager.GetPatientByPhoneNumber(text, Global.Company.CompanyId, PatientSearchType.Patient);
                }
                else
                {
                    PatientInfo = PatientManager.GetPatientByName(text, Global.Company.CompanyId, PatientSearchType.Patient);
                }
            }
            return PatientInfo;
        }


        private void LoadPatients(IList<Patient> PatientInfo)
        {
            if (PatientInfo != null && PatientInfo.Count > 0)
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
                    if(lPatientInfo.GetStatus(Global.getTransactionDate()) == "In OP")
                    {
                        Registration OpRegistration = OpManager.Instance.GetlastOPRecord((long)lPatientInfo.Id , lPatientInfo.CompanyId);
                        GridViewPatientSearchResult.Rows[i].Cells[(int)PatientSearchGridColumn.ASSIGNEE].Value = OpRegistration.RequestedDoctor != null ? OpRegistration.RequestedDoctor.Name : "";

                    }
                    else if(lPatientInfo.GetStatus(Global.getTransactionDate()) == "In IP")
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
                PatientSearchErrorMsg.Text = NothingFoundMsg;
            }
        }
        private void BtnSearchPatientByName_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            GridViewPatientSearchResult.Rows.Clear();
            PatientSearchErrorMsg.Text = "";
            IList<Patient> PatientInfo = null!;
            string searchText = TextBoxSearchPatientByName.Text.Trim();

            if (searchText.Length < 3)
            {
                PatientSearchErrorMsg.Text = SearchPatientErrorMsg;
                TextBoxSearchPatientByName.Select();
                Cursor.Current = Cursors.Default;
                return;
            }
            if (PatientSearchType == PatientSearchType.Inpatient)
            {
                PatientInfo = SearchInPatient(TextBoxSearchPatientByName.Text.Trim());
                LoadPatients(PatientInfo);
            }
            else if (PatientSearchType == PatientSearchType.SearchPatient)
            {
                PatientInfo = SearchPatient(TextBoxSearchPatientByName.Text.Trim());
                LoadPatients(PatientInfo);
            }
            else
            {
                if (isValidSearchCriteria())
                {
                    PatientInfo = SearchPatient(TextBoxSearchPatientByName.Text.Trim());
                    LoadPatients(PatientInfo);
                }
            }
            if (GridViewPatientSearchResult.Rows.Count > 0)
            {
                BtnPatientSelect.Enabled = true;
                if (PatientSearchType == PatientSearchType.Inpatient && string.IsNullOrEmpty(TextBoxSearchPatientByName.Text))
                {
                    TextBoxSearchPatientByName.TextBox.Select();
                }
                else
                {
                    GridViewPatientSearchResult.Select();
                    GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult[0, 0];
                }
            }
            else
            {
                BtnPatientSelect.Enabled = false;
                TextBoxSearchPatientByName.TextBox.Select();
            }
            Cursor.Current = Cursors.Default;
        }
        private bool isValidSearchCriteria()
        {
            string searchText = TextBoxSearchPatientByName.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                PatientSearchErrorMsg.Text = EnterSearchTextMsg;
                TextBoxSearchPatientByName.Select();
                return false;
            }
            return true;
        }



        private void BtnPatientSelect_Click(object sender, EventArgs e)
        {
            if (GridViewPatientSearchResult.Rows.Count > 0 && GridViewPatientSearchResult.CurrentRow != null)
            {
                if (GridViewPatientSearchResult.CurrentRow.Index > -1)
                {
                    parent.PatientIdTransport.ResetText();
                    parent.PatientIdTransport.Text = GridViewPatientSearchResult.CurrentRow.Cells[(int)PatientSearchGridColumn.PID].Value.ToString();
                    this.Close();
                }
                else
                {
                    PatientSearchErrorMsg.Text = SelectPatientErrorMsg;
                }
            }
        }

        private void ResetForm()
        {
            TextBoxSearchPatientByName.TextBox.ResetText();
            PatientSearchErrorMsg.Text = "";
            GridViewPatientSearchResult.Rows.Clear();
            BtnPatientSelect.Enabled = false;
        }

        private void TextBoxSearchPatientByName_KeyDown(object sender, KeyEventArgs e)
        {

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
            if (e.RowIndex > -1)
            {
                BtnPatientSelect_Click(sender, e);
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F8))
            {
                BtnPatientSelect.PerformClick();
            }
            else if (keyData == (Keys.Escape))
            {
                BtnPatientCancel.PerformClick();
            }
            else if (keyData == Keys.Enter && TextBoxSearchPatientByName.Focused)
            {
                BtnSearchPatientByName.PerformClick();
            }
            else if (keyData == Keys.Enter && GridViewPatientSearchResult.Focused)
            {
                BtnPatientSelect.PerformClick();
            }
            try
            {
                if (GridViewPatientSearchResult.CurrentRow != null)
                {
                    if (keyData == (Keys.Tab) && GridViewPatientSearchResult.CurrentRow.Index > -1)
                    {
                        if (GridViewPatientSearchResult.CurrentRow.Index != GridViewPatientSearchResult.Rows.Count - 1)
                        {
                            GridViewPatientSearchResult.Select();
                            GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult[0, GridViewPatientSearchResult.CurrentRow.Index + 1];
                            GridViewPatientSearchResult.CurrentCell.Selected = true;
                        }
                    }
                }

            }
            catch
            { }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnPatientCancel_Click(object sender, EventArgs e)
        {
            parent.PatientIdTransport.Text = "";
            this.Close();
        }

        private void BtnPatientSelect_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
                if (BtnPatientCancel.Focused) { BtnPatientCancel.Select(); }
                else if (BtnPatientSelect.Focused) { BtnPatientSelect.Select(); }
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
            {
                e.IsInputKey = true;
                if (BtnPatientSelect.Focused) { BtnPatientCancel.Select(); }
                else if (BtnPatientSelect.Enabled) { BtnPatientSelect.Select(); }
                else { BtnPatientCancel.Select(); }
            }
        }

        private void GridViewPatientSearchResult_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                if (GridViewPatientSearchResult.CurrentRow != null)
                {
                    if (GridViewPatientSearchResult.CurrentCell.RowIndex == 0)
                    {
                        TextBoxSearchPatientByName.TextBox.Select();
                    }
                    if (GridViewPatientSearchResult.CurrentRow.Index != 0)
                    {
                        GridViewPatientSearchResult.Select();
                        GridViewPatientSearchResult.CurrentCell = GridViewPatientSearchResult[1, GridViewPatientSearchResult.CurrentRow.Index - 1];
                        GridViewPatientSearchResult.CurrentCell.Selected = true;
                    }
                }
            }
        }


    }
}
