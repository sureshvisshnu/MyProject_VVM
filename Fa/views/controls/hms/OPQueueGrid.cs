using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.model.Hms.Op;
using fa.api.utils;
using fa.api.Hms;
using fa.api.UserProfile;
using fa.model.UserProfile;

namespace fa.views.controls.hms
{
    public partial class OPQueueGrid : UserControl
    {
        OpManager OpManager = OpManager.Instance;
        string _SearchString = string.Empty;
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
        bool _SingleClickSelection = true;
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
        bool _IsDoctor = true;
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
        bool _IsMyQueue = true;
        public bool IsMyQueue
        {
            get
            {
                return _IsMyQueue;
            }
            set
            {
                _IsMyQueue = value;
            }
        }
        Status _OPStatus;
        public Status OPStatus
        {
            get
            {
                return _OPStatus;
            }
            set
            {
                _OPStatus = value;
                reloadGrid();
            }
        }
        public bool IsEmpty
        {
            get
            {
                return GridViewOpRegistrationToken.RowCount <= 0;
            }
        }
        public long? OpId { get; set; }
        public OPQueueGrid()
        {
            InitializeComponent();
            GridViewOpRegistrationToken.Size = this.Size;
        }

        private void reloadGrid()
        {
            if (!DesignMode)
            {
                OpId = null;
                GridViewOpRegistrationToken.Rows.Clear();
                IList<Registration> Registration = null!;
                IList<Registration> ListRegistrationbyMyQueue = null!;
                DateTime TransactionDate = Global.getTransactionDate();
                string FilterString = SearchString.Trim();
                IList<Status> lStatus = new List<Status>();

                if (OPStatus == Status.ALL)
                {
                    lStatus.Add(Status.OPEN);
                    lStatus.Add(Status.COMPLETED);
                }
                else
                {
                    lStatus.Add(OPStatus);
                }

                if (!IsMyQueue)
                {
                    ListRegistrationbyMyQueue = OpManager.ListAllOpByTransactionDate(lStatus, Global.Company.CompanyId, TransactionDate, this.IsDoctor);
                }
                else
                {
                    User user = UserManager.Instance.GetUserByLogin(Global.User.Login);
                    if (user.Employee != null)
                    {
                        ListRegistrationbyMyQueue = OpManager.ListAllOpByEmployeeId(lStatus, Global.Company.CompanyId, TransactionDate, this.IsDoctor, user);
                    }
                    else
                    {
                        ListRegistrationbyMyQueue = OpManager.ListAllOpByTransactionDate(lStatus, Global.Company.CompanyId, TransactionDate, this.IsDoctor);
                    }
                }
                if (!string.IsNullOrEmpty(FilterString))
                {
                    if (TextUtils.isPatientNumber(FilterString))
                    {
                        Registration = OpManager.ListAllOpByPatientNumber(FilterString, this.IsDoctor, ListRegistrationbyMyQueue);
                        if (Registration == null || Registration.Count == 0)
                        {
                            Registration = OpManager.ListAllOpByPatientPhoneNumber(FilterString, this.IsDoctor, ListRegistrationbyMyQueue);
                        }
                    }
                    else if (DateUtils.ValidDate(FilterString, Global.Company.DateFormat))
                    {
                        Registration = OpManager.ListAllOpByDOB((DateTime)DateUtils.ToDate(FilterString, Global.Company.DateFormat)!, this.IsDoctor, ListRegistrationbyMyQueue);
                    }
                    else if (TextUtils.isTockenNumber(FilterString))
                    {
                        Registration = OpManager.ListAllOpByToken(FilterString, this.IsDoctor, ListRegistrationbyMyQueue);
                        if (Registration == null || Registration.Count == 0)
                        {
                            Registration = OpManager.ListAllOpByPatientPhoneNumber(FilterString, this.IsDoctor, ListRegistrationbyMyQueue);
                        }
                    }
                    else if (TextUtils.IsPhoneNumber(FilterString))
                    {
                        Registration = OpManager.ListAllOpByPatientPhoneNumber(FilterString, this.IsDoctor, ListRegistrationbyMyQueue);
                    }
                    else
                    {
                        Registration = OpManager.ListAllOpByName(FilterString, this.IsDoctor, ListRegistrationbyMyQueue);
                    }
                }
                else { Registration = ListRegistrationbyMyQueue; }
                if (Registration!.Count > 0)
                {
                    Registration = Registration.OrderBy(x => int.Parse(x.TockenNo)).ToList();
                    int i = 0;
                    foreach (var lRegistration in Registration)
                    {
                        if (SingleClickSelection && i == 0)
                        {
                            OpId = lRegistration.Id;
                        }
                        GridViewOpRegistrationToken.Rows.Add(1);
                        GridViewOpRegistrationToken.Rows[i].Cells[0].Value = lRegistration.TockenNo.TrimStart('0');
                        GridViewOpRegistrationToken.Rows[i].Cells[1].Value = lRegistration.Patient.Name;
                        GridViewOpRegistrationToken.Rows[i].Cells[2].Value = lRegistration.Patient.Age;
                        GridViewOpRegistrationToken.Rows[i].Cells[3].Value = lRegistration.DateOfRegistration.ToString(Global.Company.DateFormat)
                            + "/"
                            + lRegistration.DateOfRegistration.ToString("hh:mm tt");
                        GridViewOpRegistrationToken.Rows[i].Cells[4].Value = lRegistration.RequestedDoctor != null ? lRegistration.RequestedDoctor.Name : "";
                        GridViewOpRegistrationToken.Rows[i].Cells[5].Value = lRegistration.Id;
                        i++;
                    }
                    return;
                }
            }
        }

        private void GridViewOpRegistrationToken_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewOpRegistrationToken_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SingleClickSelection && GridViewOpRegistrationToken.CurrentCell != null)
            {
                if (e.RowIndex > -1 && GridViewOpRegistrationToken.CurrentCell.Value != null)
                {
                    OpId = (long)GridViewOpRegistrationToken.Rows[e.RowIndex].Cells[5].Value;
                }
                base.OnClick(e);
                if (OpId != null)
                {
                    SelectedById(OpId.ToString()!);
                }
            }
        }
        private void FetchData()
        {

            foreach (DataGridViewRow Row in GridViewOpRegistrationToken.Rows)
            {
                if (Row.Cells[5].Value.ToString() == OpId.ToString())
                {
                    GridViewOpRegistrationToken.Select();
                    GridViewOpRegistrationToken.CurrentCell = GridViewOpRegistrationToken[0, Row.Index];
                    GridViewOpRegistrationToken.CurrentCell.Selected = true;
                }
            }
        }
        private void GridViewOpRegistrationToken_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!SingleClickSelection)
            {
                if (e.RowIndex > -1)
                {
                    OpId = (long)GridViewOpRegistrationToken.Rows[e.RowIndex].Cells[5].Value;
                }
                base.OnDoubleClick(e);
                if (OpId != null)
                {
                    SelectedById(OpId.ToString()!);
                }
            }
        }

        private void GridViewOpRegistrationToken_SizeChanged(object sender, EventArgs e)
        {
            GridViewOpRegistrationToken.Size = this.Size;
        }

        public void SelectedById(string ID)
        {
            foreach (DataGridViewRow Row in GridViewOpRegistrationToken.Rows)
            {
                if (Row.Cells[5].Value.ToString() == ID)
                {
                    GridViewOpRegistrationToken.Select();
                    GridViewOpRegistrationToken.CurrentCell = GridViewOpRegistrationToken[0, Row.Index];
                    GridViewOpRegistrationToken.CurrentCell.Selected = true;
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (GridViewOpRegistrationToken.CurrentRow != null)
                {
                    if (keyData == (Keys.Tab) && GridViewOpRegistrationToken.CurrentRow.Index > -1)
                    {
                        if (GridViewOpRegistrationToken.CurrentRow.Index != GridViewOpRegistrationToken.Rows.Count - 1)
                        {

                            GridViewOpRegistrationToken.Select();
                            GridViewOpRegistrationToken.CurrentCell = GridViewOpRegistrationToken[0, GridViewOpRegistrationToken.CurrentRow.Index + 1];
                            GridViewOpRegistrationToken.CurrentCell.Selected = true;
                        }


                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && GridViewOpRegistrationToken.CurrentRow.Index > -1)
                    {
                        if (GridViewOpRegistrationToken.CurrentRow.Index != 0)
                        {
                            GridViewOpRegistrationToken.Select();
                            GridViewOpRegistrationToken.CurrentCell = GridViewOpRegistrationToken[0, GridViewOpRegistrationToken.CurrentRow.Index - 1];
                            GridViewOpRegistrationToken.CurrentCell.Selected = true;
                        }

                    }
                    if (keyData == (Keys.Down) && GridViewOpRegistrationToken.CurrentRow.Index > -1)
                    {
                        int rindex = GridViewOpRegistrationToken.CurrentRow.Index;
                        if (GridViewOpRegistrationToken.SelectedRows.Count > 0)
                        {
                            int RowIndex = GridViewOpRegistrationToken.SelectedRows[0].Index;
                            if (GridViewOpRegistrationToken.Rows[RowIndex].Cells[5].Value != null)
                            {
                                OpId = (long)GridViewOpRegistrationToken.Rows[rindex + 1].Cells[5].Value;
                                FetchData();
                            }
                        }
                        return true;
                    }
                    if (keyData == (Keys.Up) && GridViewOpRegistrationToken.CurrentRow.Index > -1)
                    {
                        int rindex = GridViewOpRegistrationToken.CurrentRow.Index;
                        if (GridViewOpRegistrationToken.SelectedRows.Count > 0)
                        {
                            int RowIndex = GridViewOpRegistrationToken.SelectedRows[0].Index;
                            if (GridViewOpRegistrationToken.Rows[RowIndex].Cells[5].Value != null)
                            {
                                OpId = (long)GridViewOpRegistrationToken.Rows[rindex - 1].Cells[5].Value;
                                FetchData();
                            }
                        }
                        return true;
                    }
                }
            }
            catch
            { }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GridViewOpRegistrationToken_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab &&
                (
                    (e.Modifiers != Keys.Shift && GridViewOpRegistrationToken.CurrentRow != null && GridViewOpRegistrationToken.Rows.Count - 1 == GridViewOpRegistrationToken.CurrentRow.Index)
                    || (e.Modifiers == Keys.Shift && GridViewOpRegistrationToken.CurrentRow != null && GridViewOpRegistrationToken.CurrentRow.Index == 0)
                ))
            {
                OnPreviewKeyDown(e);
            }
        }


        private void GridViewOpRegistrationToken_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            OnKeyDown(e);
        }

        private void GridViewOpRegistrationToken_Enter(object sender, EventArgs e)
        {
            if (GridViewOpRegistrationToken.Rows.Count > 0)
            {
                GridViewOpRegistrationToken.Select();
                GridViewOpRegistrationToken.CurrentCell = GridViewOpRegistrationToken[0, 0];
                GridViewOpRegistrationToken.CurrentCell.Selected = true;
            }
        }

        private void GridViewOpRegistrationToken_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewOpRegistrationToken.Rows[e.RowIndex].Cells[5].Value != null)
            {
                OpId = (long)GridViewOpRegistrationToken.Rows[e.RowIndex].Cells[5].Value;
            }
        }

        private void OPQueueGrid_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void OPQueueGrid_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            if (this.Size.Width > 460)
            {
                GridViewOpRegistrationToken.Size = new Size(this.Width - 10, this.Height - 24);
            }

        }

        private void OPQueueGrid_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void GridViewOpRegistrationToken_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            OnKeyDown(e);
        }
    }
}
