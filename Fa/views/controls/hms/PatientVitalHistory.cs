using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fa.model.Hms.common;
using fa.api.Hms;
//using System.Windows.Forms.DataVisualization.Charting;

namespace fa.views.controls.hms
{
    public enum VitalHistoryGridColumn
    {
        DATE, HEIGHT, WIDTH, BMI, TEMP, PULS, RESPRATE, BPRESSURE, BOXGLEVEL, ENTERBY, REMOVE, ID
    }

    public partial class PatientVitalHistory : UserControl
    {
        protected static int MIN_WIDTH = 790;
        protected static int DATAGRID_WIDTH_DIFF = 7;
        protected static int DATAGRID_DESC_COLUMN_DIFF = 705;
        protected static int MIN_HEIGHT = 300;
        protected static int DATAGRID_HEIGHT_DIFF = 40;

        // Define a delegate for the event
        public delegate void RowDeletedEventHandler(object sender, EventArgs e);

        // Define the event using the delegate
        public event RowDeletedEventHandler? RowDeleted;


        public PatientVitalHistory()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            GridViewVitalHistory.Rows.Clear();
        }
        private long? _PatientId;
        public long? PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                _PatientId = value;
                LoadVitalHistory();

            }
        }
        private long? _VitalsId = 0L;
        public long? VitalsId
        {
            get
            {
                return _VitalsId;
            }
            set
            {
                _VitalsId = value;
            }
        }
        private void LoadVitalHistory()
        {
            if (PatientId != null)
            {
                GridViewVitalHistory.Rows.Clear();
                IList<Vital> ListVital = VitalEntryManager.Instance.ListVitalEntryByPatientId((long)PatientId);
                if (ListVital != null && ListVital.Count > 0)
                {
                    GridViewVitalHistory.Rows.Add(ListVital.Count);
                    int i = 0;
                    foreach (Vital Vital in ListVital)
                    {
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.DATE].Value = Vital.Date.ToString(Global.Company.DateFormat);
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.HEIGHT].Value = (Vital.Height == 0) ? string.Empty : Vital.Height.ToString();
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.WIDTH].Value = (Vital.Weight == 0) ? string.Empty : Vital.Weight.ToString();
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.BMI].Value = (Vital.BMI == 0) ? string.Empty : Vital.BMI.ToString();
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.TEMP].Value = (Vital.Temperature == 0) ? string.Empty : Vital.Temperature.ToString();
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.PULS].Value = (Vital.Pulse == 0) ? string.Empty : Vital.Pulse.ToString();
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.RESPRATE].Value = (Vital.RespRate == 0) ? string.Empty : Vital.RespRate.ToString();
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.BPRESSURE].Value = (Vital.BPressure == 0) ? string.Empty : Vital.BPressure.ToString() + " / " + Vital.BPressureOver.ToString();
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.BOXGLEVEL].Value = (Vital.BOxyLevel == 0) ? string.Empty : Vital.BOxyLevel.ToString() + " %";
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.ENTERBY].Value = Vital.LastModifiedBy;
                        GridViewVitalHistory.Rows[i].Cells[(int)VitalHistoryGridColumn.ID].Value = Vital.Id;
                        i++;
                    }
                }
            }
        }

        protected virtual void OnRowDeleted(EventArgs e)
        {
            RowDeleted?.Invoke(this, e);
        }
        private void PatientVitalHistory_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void GridViewVitalHistory_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void SizeChange()
        {
            if (this.Width - DATAGRID_WIDTH_DIFF < MIN_WIDTH)
            {
                this.Width = MIN_WIDTH;
            }
            else
            {
                GridViewVitalHistory.Width = this.Width - DATAGRID_WIDTH_DIFF;
                GridViewVitalHistory.Columns[0].Width = GridViewVitalHistory.Width - DATAGRID_DESC_COLUMN_DIFF;
            }
            GridViewVitalHistory.Height = this.Height - DATAGRID_HEIGHT_DIFF;
        }
        private void GridViewVitalHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)VitalHistoryGridColumn.REMOVE)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete  " + GridViewVitalHistory.Rows[e.RowIndex].Cells[(int)VitalHistoryGridColumn.DATE].Value.ToString() + " vital entry?", "Delete Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (GridViewVitalHistory.Rows[e.RowIndex].Cells[(int)VitalHistoryGridColumn.ID].Value != null)
                        {
                            VitalEntryManager.Instance.DeleteVital((long)GridViewVitalHistory.Rows[e.RowIndex].Cells[(int)VitalHistoryGridColumn.ID].Value);

                            GridViewVitalHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewVitalHistory.Rows.RemoveAt(e.RowIndex);

                            OnRowDeleted(EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    if (GridViewVitalHistory.Rows[e.RowIndex].Cells[(int)VitalHistoryGridColumn.ID].Value != null)
                    {
                        VitalsId = long.Parse(GridViewVitalHistory.Rows[e.RowIndex].Cells[(int)VitalHistoryGridColumn.ID].Value.ToString()!);
                        OnDoubleClick(e);
                    }
                }
            }
        }
        private void GridViewVitalHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (GridViewVitalHistory.Rows[e.RowIndex].Cells[(int)VitalHistoryGridColumn.ID].Value != null)
                {
                    VitalsId = long.Parse(GridViewVitalHistory.Rows[e.RowIndex].Cells[(int)VitalHistoryGridColumn.ID].Value.ToString()!);
                    OnDoubleClick(e);
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.Tab) && GridViewVitalHistory.CurrentCell != null)
                {
                    if (GridViewVitalHistory.CurrentCell.RowIndex != GridViewVitalHistory.Rows.Count - 1)
                    {
                        GridViewVitalHistory.CurrentCell = GridViewVitalHistory[(int)VitalHistoryGridColumn.ENTERBY, (GridViewVitalHistory.CurrentCell.RowIndex + 1)];
                        GridViewVitalHistory.CurrentCell.Selected = true;
                    }

                }
                if (keyData == (Keys.Tab | Keys.Shift) && GridViewVitalHistory.CurrentCell != null)
                {
                    if (GridViewVitalHistory.CurrentRow.Index != 0)
                    {
                        GridViewVitalHistory.CurrentCell = GridViewVitalHistory[(int)VitalHistoryGridColumn.DATE, (GridViewVitalHistory.CurrentCell.RowIndex)];
                        GridViewVitalHistory.CurrentCell.Selected = true;
                        return false;
                    }
                }
            }
            catch
            {
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


    }
}
