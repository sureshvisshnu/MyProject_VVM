using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.api.Hms;
using fa.model.Hms.Op;
using fa.model.hms.common;
using fa.model.Employee;
using fa.api.Accounting;
using fa.model.Hms.common;
using NPOI.SS.Formula.Functions;

namespace fa.views.controls.hms
{
    public partial class OutPatientVisit : UserControl
    {
        public enum AddOutPatientVisitGridColumn
        {
            DATE, REASON, CONSULTANT
        }

        OpManager OpManager = OpManager.Instance;

        public int GridRows = 0;
        public int GridRowIndex = 0;
        public bool LastRow = false;
        public bool FirstRow = false;

        public OutPatientVisit()
        {
            InitializeComponent();
        }
        public long PatientId
        {
            set
            {
                LoadViewOutPatientGrid(value);
            }
        }
        public bool IsEmpty
        {
            get
            {
                return GridViewOutPatient.RowCount <= 0;
            }
        }
        public void Clear()
        {
            GridViewOutPatient.Rows.Clear();
        }

        private void LoadViewOutPatientGrid(long PatientId)
        {

            IList<Registration> OpRegistration = OpManager.ListAllOpByPatientId(PatientId);


            GridViewOutPatient.Rows.Clear();
            if (OpRegistration.Count > 0)
            {
                GridViewOutPatient.Rows.Add(OpRegistration.Count);


                if (OpRegistration.Count > 0)
                {
                    int i = 0;
                    foreach (var lOpRegistration in OpRegistration)
                    {
                        string dateString = lOpRegistration.DateOfRegistration.ToString(Global.Company.DateFormat);
                        string timeString = ((DateTime)lOpRegistration.DateOfRegistration).ToShortTimeString();
                        int count = dateString.Length;
                        if (Global.Company.DateFormat == "M/d/yy")
                        {
                            GridViewOutPatient.Rows[i].Cells[(int)AddOutPatientVisitGridColumn.DATE].Value = lOpRegistration.DateOfRegistration.ToString(Global.Company.DateFormat) + (count == 6 ? "      " : count == 7 ? "    " : "  ") + ((DateTime)lOpRegistration.DateOfRegistration).ToShortTimeString();
                        }
                        else if (Global.Company.DateFormat == "M/d/yyyy")
                        {
                            GridViewOutPatient.Rows[i].Cells[(int)AddOutPatientVisitGridColumn.DATE].Value = lOpRegistration.DateOfRegistration.ToString(Global.Company.DateFormat) + (count == 8 ? "      " : count == 9 ? "    " : "  ") + ((DateTime)lOpRegistration.DateOfRegistration).ToShortTimeString();
                        }
                        else
                        {
                            GridViewOutPatient.Rows[i].Cells[(int)AddOutPatientVisitGridColumn.DATE].Value = lOpRegistration.DateOfRegistration.ToString(Global.Company.DateFormat) + " " + ((DateTime)lOpRegistration.DateOfRegistration).ToShortTimeString();
                        }
                        GridViewOutPatient.Rows[i].Cells[(int)AddOutPatientVisitGridColumn.REASON].Value = (lOpRegistration.ReasonForTheVisit != null) ? lOpRegistration.ReasonForTheVisit.ToString() : "";
                        if (lOpRegistration.RequestedDoctorId != null)
                        {
                            GridViewOutPatient.Rows[i].Cells[(int)AddOutPatientVisitGridColumn.CONSULTANT].Value = "Dr." + lOpRegistration.RequestedDoctor.Name;
                        }
                        else
                        {
                            ConsultationNote ConsultationNote = ConsultationNoteManager.Instance.GetConsultationNoteByPatientIdDate((long)lOpRegistration.PatientId!, lOpRegistration.DateOfRegistration);
                            if (ConsultationNote != null && ConsultationNote.Consultant != null)
                            {
                                GridViewOutPatient.Rows[i].Cells[(int)AddOutPatientVisitGridColumn.CONSULTANT].Value = "Dr." + ConsultationNote.ConsultantName();
                            }
                        }
                        i++;
                    }
                }
                GridRows = GetRowCount();
            }
        }
        private void SizeChange()
        {
            GridViewOutPatient.Size = new Size(this.Width - 5, this.Height - 24);
            GridViewOutPatient.Columns[(int)AddOutPatientVisitGridColumn.REASON].Width = GridViewOutPatient.Width - 320;
        }


        private void OutPatientVisit_SizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }

        private void OutPatientVisit_Resize(object sender, EventArgs e)
        {
            SizeChange();
        }

        public int GetRowCount()
        {
            return GridViewOutPatient.RowCount;
        }

        private void GridViewOutPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == GridViewOutPatient.Rows.Count - 1)
            {
                LastRow = true;
            }
            else if (e.RowIndex == GridViewOutPatient.RowCount)
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
                if (GridViewOutPatient.CurrentRow != null)
                {
                    if (keyData == (Keys.Tab) && GridViewOutPatient.CurrentRow.Index > -1)
                    {
                        if (GridViewOutPatient.CurrentRow.Index != GridViewOutPatient.Rows.Count - 1)
                        {
                            GridViewOutPatient.Select();
                            GridRowIndex = GridViewOutPatient.CurrentRow.Index + 1;
                            GridViewOutPatient.CurrentCell = GridViewOutPatient[0, GridViewOutPatient.CurrentRow.Index + 1];
                            GridViewOutPatient.CurrentCell.Selected = true;
                        }
                        else if (GridViewOutPatient.CurrentRow.Index == GridViewOutPatient.Rows.Count - 1)
                        {
                            LastRow = true;
                            GridViewOutPatient.Select();
                            GridRowIndex = GridViewOutPatient.CurrentRow.Index + 1;
                            GridViewOutPatient.CurrentCell = GridViewOutPatient[0, GridViewOutPatient.CurrentRow.Index];
                            GridViewOutPatient.CurrentCell.Selected = true;
                        }
                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && GridViewOutPatient.CurrentRow.Index > -1)
                    {
                        if (GridViewOutPatient.CurrentRow.Index != 0)
                        {
                            GridViewOutPatient.Select();
                            GridRowIndex = GridViewOutPatient.CurrentRow.Index + 1;
                            GridViewOutPatient.CurrentCell = GridViewOutPatient[0, GridViewOutPatient.CurrentRow.Index - 1];
                            GridViewOutPatient.CurrentCell.Selected = true;
                        }
                        else if (GridViewOutPatient.CurrentRow.Index == GridViewOutPatient.Rows.Count - 1)
                        {
                            LastRow = true;
                            GridViewOutPatient.Select();
                            GridRowIndex = GridViewOutPatient.CurrentRow.Index + 1;
                            GridViewOutPatient.CurrentCell = GridViewOutPatient[0, GridViewOutPatient.CurrentRow.Index];
                            GridViewOutPatient.CurrentCell.Selected = true;
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

        private void OutPatientVisit_Enter(object sender, EventArgs e)
        {
            DataGridViewCell currentCell = GridViewOutPatient.CurrentCell;

            if (currentCell != null)
            {
                int currentRowIndex = currentCell.RowIndex;

                if (currentRowIndex == GridViewOutPatient.Rows.Count - 1)
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
