using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Drawing;
using fa.api.Hms;
using fa.model.Hms.common;
using fa.model.Hms.Ip;

namespace fa.views.controls.hms
{
    public partial class InPatientVisit : UserControl
    {
        public enum AddIpPatientVisitGridColumn
        {
            DATE, IPNOTE, DISCHARGE
        }

        IpManager IpManager = IpManager.Instance;

        public int GridRows = 0;
        public int GridRowIndex = 0;
        public bool LastRow = false;
        public bool FirstRow = false;

        public InPatientVisit()
        {
            InitializeComponent();
        }
        public long PatientId
        {
            set
            {
                LoadViewInPatientGrid(value);
            }
        }
        public bool IsEmpty
        {
            get
            {
                return GridViewInPatient.RowCount <= 0;
            }
        }
        public void Clear()
        {
            GridViewInPatient.Rows.Clear();
        }
        private void LoadViewInPatientGrid(long PatientId)
        {

            IList<InPatientAdmission> IpRegistration = IpManager.ListAllIpByPatientId(PatientId);


            GridViewInPatient.Rows.Clear();
            if (IpRegistration.Count > 0)
            {
                GridViewInPatient.Rows.Add(IpRegistration.Count);
            }

            if (IpRegistration.Count > 0)
            {
                int i = 0;
                foreach (var lIpRegistration in IpRegistration)
                {
                    GridViewInPatient.Rows[i].Cells[(int)AddIpPatientVisitGridColumn.DATE].Value = lIpRegistration.DateOfAdmission.ToString(Global.Company.DateFormat);
                    GridViewInPatient.Rows[i].Cells[(int)AddIpPatientVisitGridColumn.IPNOTE].Value = lIpRegistration.AdmissionNote != null ? lIpRegistration.AdmissionNote.ToString() : "";
                    if (lIpRegistration.Status == InPatientStatus.DISCHARGED)
                    {
                        DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(lIpRegistration.Id);
                        if (DischargeNote != null)
                        {
                            string dateString = ((DateTime)DischargeNote.DischargeOn!).ToString(Global.Company.DateFormat);
                            string timeString = ((DateTime)DischargeNote.DischargeOn!).ToShortTimeString();
                            int count = dateString.Length;
                            if (Global.Company.DateFormat == "M/d/yy")
                            {
                                GridViewInPatient.Rows[i].Cells[(int)AddIpPatientVisitGridColumn.DISCHARGE].Value = ((DateTime)DischargeNote.DischargeOn).ToString(Global.Company.DateFormat) + (count == 6 ? "      " : count == 7 ? "    " : "  ") + ((DateTime)DischargeNote.DischargeOn).ToShortTimeString();
                            }
                            else if (Global.Company.DateFormat == "M/d/yyyy")
                            {
                                GridViewInPatient.Rows[i].Cells[(int)AddIpPatientVisitGridColumn.DISCHARGE].Value = ((DateTime)DischargeNote.DischargeOn).ToString(Global.Company.DateFormat) + (count == 8 ? "      " : count == 9 ? "    " : "  ") + ((DateTime)DischargeNote.DischargeOn).ToShortTimeString();
                            }
                            else
                            {
                                GridViewInPatient.Rows[i].Cells[(int)AddIpPatientVisitGridColumn.DISCHARGE].Value = ((DateTime)DischargeNote.DischargeOn!).ToString(Global.Company.DateFormat) + " " + ((DateTime)DischargeNote.DischargeOn).ToShortTimeString();
                            }
                        }
                    }
                    i++;
                }
                GridRows = GetRowCount();
            }
        }
        private void SizeChange()
        {
            GridViewInPatient.Size = new Size(this.Width - 5, this.Height - 24);
            GridViewInPatient.Columns[(int)AddIpPatientVisitGridColumn.IPNOTE].Width = GridViewInPatient.Width - 240;
        }

        private void InPatientVisit_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void InPatientVisit_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }

        public int GetRowCount()
        {
            return GridViewInPatient.RowCount;
        }

        private void GridViewInPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == GridViewInPatient.Rows.Count - 1)
            {
                LastRow = true;
            }
            else if (e.RowIndex == GridViewInPatient.RowCount)
            {
                LastRow = true;
            }
            else
            {
                LastRow = false;
            }
            if (e.RowIndex == 0)
            {
                FirstRow = true;
            }
            else
            {
                FirstRow = false;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (GridViewInPatient.CurrentRow != null)
                {
                    if (keyData == (Keys.Tab) && GridViewInPatient.CurrentRow.Index > -1)
                    {
                        if (GridViewInPatient.CurrentRow.Index != GridViewInPatient.Rows.Count - 1)
                        {
                            GridViewInPatient.Select();
                            GridRowIndex = GridViewInPatient.CurrentRow.Index + 1;
                            GridViewInPatient.CurrentCell = GridViewInPatient[0, GridViewInPatient.CurrentRow.Index + 1];
                            GridViewInPatient.CurrentCell.Selected = true;
                        }
                        else if (GridViewInPatient.CurrentRow.Index == GridViewInPatient.Rows.Count - 1)
                        {
                            LastRow = true;
                            GridViewInPatient.Select();
                            GridRowIndex = GridViewInPatient.CurrentRow.Index + 1;
                            GridViewInPatient.CurrentCell = GridViewInPatient[0, GridViewInPatient.CurrentRow.Index];
                            GridViewInPatient.CurrentCell.Selected = true;
                        }
                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && GridViewInPatient.CurrentRow.Index > -1)
                    {
                        if (GridViewInPatient.CurrentRow.Index != 0)
                        {
                            GridViewInPatient.Select();
                            GridRowIndex = GridViewInPatient.CurrentRow.Index + 1;
                            GridViewInPatient.CurrentCell = GridViewInPatient[0, GridViewInPatient.CurrentRow.Index - 1];
                            GridViewInPatient.CurrentCell.Selected = true;
                        }
                        else if (GridViewInPatient.CurrentRow.Index == GridViewInPatient.Rows.Count - 1)
                        {
                            LastRow = true;
                            GridViewInPatient.Select();
                            GridRowIndex = GridViewInPatient.CurrentRow.Index + 1;
                            GridViewInPatient.CurrentCell = GridViewInPatient[0, GridViewInPatient.CurrentRow.Index];
                            GridViewInPatient.CurrentCell.Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InPatientVisit_Enter(object sender, EventArgs e)
        {
            DataGridViewCell currentCell = GridViewInPatient.CurrentCell;

            if (currentCell != null)
            {
                int currentRowIndex = currentCell.RowIndex;

                if (currentRowIndex == GridViewInPatient.Rows.Count - 1)
                {
                    LastRow = true;
                }
                else
                {
                    LastRow = false;
                }
                if (currentRowIndex == 0)
                {
                    FirstRow = true;
                }
                else
                {
                    FirstRow = false;
                }
            }
        }
    }
}
