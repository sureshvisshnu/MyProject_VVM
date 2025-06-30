using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Hms.Ip;
using fa.api.utils;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.model.UserProfile;
using fa.api.Accounting;
using fa.model.Hms.common;
using DocumentFormat.OpenXml.Office.Word.Y2020.OEmbed;
using fa.model.Hms.Op;
using static FADataAccessLibrary.report.Hms.RptPatientDueList;
using Fa.reports.Hms;
using System.Net.NetworkInformation;

namespace fa.views.controls.hms
{
    enum IPQueueGridColumn
    {
        PATIENT_ID = 0, NAME = 1, AGE = 2, ADMITTED_DATE = 3, WARD = 4, BED = 5, PRIMARY_CONSULTANT = 6, PRIMARY_CAREGIVER = 7, REGISTRATION_ID = 8, STATUS = 9
    }
    public enum IpQueueStatus
    {
        ALL, ADMITTED, RELEASED_FOR_LAB_WORK, DISCHARGED, FOR_DOCTORS
    }
    public partial class IPQueueGrid : UserControl
    {
        IpManager IpManager = IpManager.Instance;
        WardManager WardManager = WardManager.Instance;
        IpAndDischargeReport IpAndDischargeReport = null!;
        public long? IpId { get; set; }
        private string _SearchString = string.Empty;
        public int RowIndex { get; set; }
        public string SearchString
        {
            get
            {
                return _SearchString;
            }
            set
            {
                _SearchString = value;
                if (value == null) { _SearchString = string.Empty; }
            }
        }
        private bool _SingleClickSelection = true;
        public bool SingleClickSelection
        {
            get
            {
                return _SingleClickSelection;
            }
            set
            {
                _SingleClickSelection = value;
            }
        }
        private bool _IsDoctor = true;
        public bool IsDoctor
        {
            get
            {
                return _IsDoctor;
            }
            set
            {
                _IsDoctor = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return GridViewIp.RowCount <= 0;
            }
        }

        private long _WardId = 0L;
        public long WardId
        {
            get
            {
                return _WardId;
            }
            set
            {
                _WardId = value;
                ReLoadGrid();
            }
        }
        private int _StatusIndex = 0;
        public int StatusIndex
        {
            get
            {
                return _StatusIndex;
            }
            set
            {
                _StatusIndex = value;
                ReLoadGrid();
            }
        }
        private IpQueueStatus _FillterBy = IpQueueStatus.ALL;
        public IpQueueStatus FillterBy
        {
            get
            {
                return _FillterBy;
            }
            set
            {
                _FillterBy = value;
                //ReLoadGrid();
            }
        }
        IList<InPatientStatus> _lStatus = new List<InPatientStatus>();
        private IList<InPatientStatus> lStatus
        {
            get
            {
                _lStatus.Clear();
                if (FillterBy == IpQueueStatus.ALL)
                {
                    _lStatus.Add(InPatientStatus.ADMITTED);
                    _lStatus.Add(InPatientStatus.RELEASED_FOR_LAB_WORK);
                    _lStatus.Add(InPatientStatus.DISCHARGED);
                }
                if (FillterBy == IpQueueStatus.ADMITTED)
                {
                    _lStatus.Add(InPatientStatus.ADMITTED);
                }
                if (FillterBy == IpQueueStatus.RELEASED_FOR_LAB_WORK)
                {
                    _lStatus.Add(InPatientStatus.RELEASED_FOR_LAB_WORK);
                }
                if (FillterBy == IpQueueStatus.DISCHARGED)
                {
                    _lStatus.Add(InPatientStatus.DISCHARGED);
                }
                if (FillterBy == IpQueueStatus.FOR_DOCTORS)
                {
                    _lStatus.Add(InPatientStatus.ADMITTED);
                    _lStatus.Add(InPatientStatus.RELEASED_FOR_LAB_WORK);
                }
                return _lStatus;
            }
        }
        public void Clear()
        {
            GridViewIp.Rows.Clear();
            //IpId = null;
        }
        public void ReLoadGrid()
        {
            if (!DesignMode)
            {
                IpAndDischargeReport = new IpAndDischargeReport();
                IpAndDischargeReport.GenerateReport(StatusIndex, lStatus, WardId, SearchString);
                GridViewIp.Rows.Clear();
                //IpId = null;
                string Status = string.Empty;
                if (IpAndDischargeReport.LineItems.Count > 0)
                {
                    int i = 0;
                    foreach (IpAndDischargeLineItem LineItem in IpAndDischargeReport.LineItems.OrderByDescending(x => x.Status).ThenByDescending(x => x.Date).ThenByDescending(x => x.DischargeOn))
                    {
                        GridViewIp.Rows.Add(1);
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.PATIENT_ID].Value = LineItem.PatientNumber;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.NAME].Value = LineItem.PatientName;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.AGE].Value = LineItem.Age;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.PRIMARY_CONSULTANT].Value = LineItem.PrimaryConsultant;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.PRIMARY_CAREGIVER].Value = LineItem.PrimaryNurse;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.WARD].Value = LineItem.Ward;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.BED].Value = LineItem.Bed;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.STATUS].Value = LineItem.Status;
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.ADMITTED_DATE].Value = LineItem.Date.Date.ToString(fa.Global.Company.DateFormat);  // "4 04 16 16"      hour 12/24 lRegistration.DateOfAdmission.ToLongTimeString();
                        GridViewIp.Rows[i].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value = LineItem.RegistrationId;
                        Status = LineItem.Status;
                        i++;
                    }
                    int irowIndex = 0;
                    foreach (DataGridViewRow row in GridViewIp.Rows)
                    {
                        if (row.Cells[8].Value != null && row.Cells[8].Value.ToString()!.Equals(IpId.ToString()))
                        {
                            irowIndex = row.Index;
                            break;
                        }
                    }
                    if (irowIndex == 0)
                    {
                        GridViewIp.Select();
                        GridViewIp.CurrentCell = GridViewIp[0, 0];
                        IpId = (long)GridViewIp.Rows[0].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value;
                    }
                    else
                    {
                        GridViewIp.ClearSelection();
                        GridViewIp.Rows[irowIndex].Selected = true;
                        IpId = (long)GridViewIp.Rows[irowIndex].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value;
                    }
                    return;
                }
            }
        }
        public IPQueueGrid()
        {
            InitializeComponent();
            GridViewIp.Size = this.Size;
        }
        public void FocusModifiedRowByIpId(long IPId)
        {
            foreach (DataGridViewRow Row in GridViewIp.Rows)
            {
                if (Row.Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value != null && Row.Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value.ToString() == IPId.ToString())
                {
                    GridViewIp.Select();
                    GridViewIp.CurrentCell = GridViewIp[0, Row.Index];
                    GridViewIp.CurrentCell.Selected = true;
                    IpId = IPId;
                    RowIndex = Row.Index;
                }
            }
        }
        private void IPQueueGrid_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            if (this.Size.Width > 900)
            {
                GridViewIp.Size = new Size(this.Width - 5, this.Height - 5);
                GridViewIp.Columns[(int)IPQueueGridColumn.NAME].Width = this.Width - 900 + 140;
            }
        }
        private void GridViewIp_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewIp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowIndex = GridViewIp.CurrentRow.Index;
            if (e.RowIndex > -1 && GridViewIp.Rows[e.RowIndex].Cells[8].Value != null)
            {
                IpId = (long)GridViewIp.Rows[e.RowIndex].Cells[8].Value;
            }
            else
            {
                IpId = null;
            }
            base.OnClick(e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (GridViewIp.CurrentRow != null && GridViewIp.CurrentRow.Index > -1)
                {
                    if (keyData == Keys.Tab)
                    {
                        if (GridViewIp.CurrentRow.Index < GridViewIp.Rows.Count - 1)
                        {
                            GridViewIp.Select();
                            GridViewIp.CurrentCell = GridViewIp[0, GridViewIp.CurrentRow.Index + 1];
                            GridViewIp.CurrentCell.Selected = true;
                        }
                    }
                    if (keyData == Keys.Up && GridViewIp.CurrentRow.Index > 0)
                    {
                        IpId = (long)GridViewIp.Rows[GridViewIp.CurrentRow.Index - 1].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value;
                        RowIndex = GridViewIp.CurrentRow.Index;
                    }
                    if (keyData == Keys.Down && GridViewIp.CurrentRow.Index < GridViewIp.Rows.Count - 1)
                    {
                        IpId = (long)GridViewIp.Rows[GridViewIp.CurrentRow.Index + 1].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value;
                        RowIndex = GridViewIp.CurrentRow.Index;
                    }
                }
            }
            catch
            { }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void GridViewIp_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers == Keys.Shift && GridViewIp.CurrentRow != null && GridViewIp.CurrentRow.Index > 0)
            {
                int rowIndex = GridViewIp.CurrentCell.RowIndex;
                GridViewIp.CurrentCell = GridViewIp[3, rowIndex - 1];

                if (GridViewIp.CurrentCell != null)
                {
                    GridViewIp.CurrentCell.Selected = true;
                }
            }
            OnPreviewKeyDown(e);
        }
        private void GridViewIp_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void GridViewIp_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewIp.Rows[e.RowIndex].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value != null)
            {
                IpId = (long)GridViewIp.Rows[e.RowIndex].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value;
            }
        }
        public void SelectFirstRow()
        {
            if (GridViewIp.Rows.Count > 0)
            {
                GridViewIp.Rows[0].Selected = true;

                IpId = (long)GridViewIp.Rows[0].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value;

                GridViewIp.CurrentCell = GridViewIp.Rows[0].Cells[2];
            }
        }
        public void SelectLastRow()
        {
            if (GridViewIp.Rows.Count > 0)
            {
                GridViewIp.Rows[GridViewIp.Rows.Count - 1].Selected = true;

                IpId = (long)GridViewIp.Rows[GridViewIp.Rows.Count - 1].Cells[(int)IPQueueGridColumn.REGISTRATION_ID].Value;

                GridViewIp.CurrentCell = GridViewIp.Rows[GridViewIp.Rows.Count - 1].Cells[2];
            }
        }
        public bool CheckIsLastRow()
        {
            int rowCount = GridViewIp.Rows.Count;
            if (GridViewIp.Rows.Count > 0)
            {
                int rowIndex = GridViewIp.CurrentRow.Index;
                if (rowIndex == rowCount - 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CheckIsFirstRow()
        {
            int rowCount = GridViewIp.Rows.Count;
            if (GridViewIp.Rows.Count > 0)
            {
                int rowIndex = GridViewIp.CurrentRow.Index;
                if (rowIndex == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public int CheckRowCount()
        {
            int rowCount = 0;
            if (GridViewIp.Rows.Count > 0)
            {
                return GridViewIp.Rows.Count;
            }
            return rowCount;
        }
        public string GetStatus()
        {
            string Status = "";
            if (GridViewIp.Rows.Count > 0 && GridViewIp.CurrentRow.Cells[(int)IPQueueGridColumn.STATUS] != null)
            {
                Status = GridViewIp.CurrentRow.Cells[(int)IPQueueGridColumn.STATUS].Value != null ? GridViewIp.CurrentRow.Cells[(int)IPQueueGridColumn.STATUS].Value.ToString()! : string.Empty;
            }
            return Status;
        }
        public string GetStatus(int RowIndex)
        {
            string Status = "";
            if (GridViewIp.Rows.Count > 0 && GridViewIp.Rows[RowIndex].Cells[(int)IPQueueGridColumn.STATUS] != null)
            {
                Status = GridViewIp.Rows[RowIndex].Cells[(int)IPQueueGridColumn.STATUS].Value != null ? GridViewIp.Rows[RowIndex].Cells[(int)IPQueueGridColumn.STATUS].Value.ToString()! : string.Empty;
            }
            return Status;
        }

        private void GridViewIp_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridViewIp.Rows[e.RowIndex].Cells[(int)IPQueueGridColumn.NAME].Value == null)
                {
                    for (int i = 0; i < GridViewIp.Columns.Count; i++)
                    {
                        if (i > 0 && i < 10)
                        {
                            e.AdvancedBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                            e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                        }
                    }
                }
            }
        }
    }
    public class IpAndDischargeReport
    {
        public IList<IpAndDischargeLineItem> LineItems { get; } = new List<IpAndDischargeLineItem>();
        public void GenerateReport(int StatusIndex, IList<InPatientStatus> lStatus, long WardId, string SearchString)
        {
            IList<InPatientAdmission> inPatientAdmissions = null!;
            inPatientAdmissions = IpManager.Instance.ListAllIP(StatusIndex, lStatus, Global.Company.CompanyId, WardId);
            if (!string.IsNullOrEmpty(SearchString))
            {
                inPatientAdmissions = inPatientAdmissions.Where(x => x.Patient.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) || x.Patient.PatientNumber.Contains(SearchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            //else
            //{
            //    inPatientAdmissions = IpManager.Instance.ListAllIPBySearchString(lStatus, SearchString, Global.Company.CompanyId, WardId);
            //}
            if (inPatientAdmissions.Count > 0)
            {
                foreach (var lRegistration in inPatientAdmissions)
                {
                    DischargeNote DischargedNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(lRegistration.Id);
                    MedicalTeam IpMedicalTeam = MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionId(lRegistration.Id);
                    InPatientLocation IpLoctn = IpManager.Instance.GetInPatientLocationbyAdmissionId(lRegistration.Id);
                    if (IpLoctn != null)
                    {
                        Ward GetWard = WardManager.Instance.GetWardById((long)IpLoctn.WardId!);
                        Bed GetBed = WardManager.Instance.GetWardBedById((long)IpLoctn.BedId!);

                        IpAndDischargeLineItem ipAndDischargeLineItem = new IpAndDischargeLineItem(lRegistration, IpMedicalTeam, GetWard, GetBed, null!)!;
                        LineItems.Add(ipAndDischargeLineItem);
                    }
                    else
                    {
                        IpAndDischargeLineItem ipAndDischargeLineItem = new IpAndDischargeLineItem(lRegistration, IpMedicalTeam, null!, null!, DischargedNote);
                        LineItems.Add(ipAndDischargeLineItem);
                    }
                }
            }
        }
    }
    public class IpAndDischargeLineItem
    {
        public String PatientNumber { get; set; }
        public String PatientName { get; set; }
        public int? Age { get; set; }
        public DateTime Date { get; set; }
        public string Ward { get; set; }
        public string Bed { get; set; }
        public String PrimaryConsultant { get; set; }
        public String PrimaryNurse { get; set; }
        public long RegistrationId { get; set; }
        public String Status { get; set; }
        public DateTime? DischargeOn { get; set; }
        public IpAndDischargeLineItem(InPatientAdmission lRegistration, MedicalTeam IpMedicalTeam,Ward GetWard,Bed GetBed, DischargeNote DischargedNote)
        {
            this.PatientNumber = lRegistration.Patient.PatientNumber;
            this.PatientName = lRegistration.Patient.Name;
            this.Age = lRegistration.Patient.Age;
            this.Date = lRegistration.DateOfAdmission.Date;
            this.Ward = GetWard == null ? "" : GetWard.Name;
            this.Bed = GetBed == null ? "" : GetBed.Name;
            this.PrimaryConsultant = IpMedicalTeam.PrimaryDoctor.Name;
            this.PrimaryNurse = IpMedicalTeam.PrimaryCareGiver.Name;
            this.RegistrationId = lRegistration.Id;
            this.Status = (lRegistration.Status == InPatientStatus.ADMITTED) ? "IP" : (lRegistration.Status == InPatientStatus.DISCHARGED) ? "Discharged" : string.Empty;
            this.DischargeOn = DischargedNote != null ? DischargedNote.DischargeOn : null;
        }
    }
}
