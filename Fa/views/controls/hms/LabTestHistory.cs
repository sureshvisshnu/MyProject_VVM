using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.api.Hms;
using fa.model.hms.common;
using fa.model.Hms.Master;
using Fa.api.Hms;
using DocumentFormat.OpenXml.Drawing;
using fa.views.hms.patient;

namespace fa.views.controls.hms
{
    public enum LabTestHistoryGridColumn
    {
        SELECT, DATE, NAME, LID
    }
    public partial class LabTestHistory : UserControl
    {
        protected static int MIN_HEIGHT = 174;
        protected static int MIN_WIDTH = 200;
        protected static int DATAGRID_WIDTH_DIFF = 7;
        protected static int DATAGRID_HEIGHT_DIFF = 24;
        protected static int DATAGRID_DESC_COLUMN_DIFF = 140;
        public LabTestHistory()
        {
            InitializeComponent();
        }
        public long LTestId = 0L;
        private long _PatientId = 0L;
        public int GridRows = 0;
        public int GridRowIndex = 0;
        public bool LastRow = false;
        public bool ReverseTab = false;
        public long PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                _PatientId = value;
                LoadLabTestFilterHistory();
            }
        }
        private long _HiddenNoteId = 0L;
        public long HiddenNoteId
        {
            get
            {
                return _HiddenNoteId;
            }
            set
            {
                _HiddenNoteId = value;
                LoadLabTestFilterHistory();
                _HiddenNoteId = 0L;
            }
        }
        private bool _LoadAllLabTest = true;
        public bool LoadAllLabTest
        {
            get
            {
                return _LoadAllLabTest;
            }
            set
            {
                _LoadAllLabTest = value;
            }
        }
        public void Clear()
        {
            DataGrid.Rows.Clear();
        }
        private void LoadLabTestFilterHistory()
        {
            if (PatientId != 0L)
            {
                DataGrid.Rows.Clear();
                IList<ConsultationNote> ConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientId((long)PatientId);
                if (ConsultationNote != null && ConsultationNote.Count > 0)
                {
                    int i = 0;
                    DateTime LabTestDateTime = Global.getTransactionDate().AddMonths(-1);
                    if (LoadAllLabTest)
                    {
                        LabTestDateTime = Global.getTransactionDate().AddYears(-100);
                    }
                    foreach (ConsultationNote Note in ConsultationNote.Where(d => d.Date > LabTestDateTime))
                    {
                        if (HiddenNoteId != Note.Id)
                        {
                            IList<ConsultedLabTest> lConsultedLabTest = ConsultationNoteManager.Instance.ListLabTestByNoteId(Note.Id);
                            if (lConsultedLabTest != null && lConsultedLabTest.Count > 0)
                            {
                                foreach (ConsultedLabTest ConsLabTest in lConsultedLabTest)
                                {
                                    DataGrid.Rows.Add();
                                    DataGrid.Rows[i].Cells[(int)LabTestHistoryGridColumn.SELECT].Value = i + 1;
                                    DataGrid.Rows[i].Cells[(int)LabTestHistoryGridColumn.DATE].Value = Note.Date.ToString(Global.Company.DateFormat) + " " + Note.Date.ToShortTimeString();
                                    DataGrid.Rows[i].Cells[(int)LabTestHistoryGridColumn.LID].Value = ConsLabTest.ConsultedLabTestId;

                                    MedicalTest MedicalTest = MedicalTestManager.Instance.GetMedicalTestById(ConsLabTest.MedicalTestId);
                                    DataGrid.Rows[i].Cells[(int)LabTestHistoryGridColumn.NAME].Value = MedicalTest.Name;
                                    i++;
                                }
                            }
                        }
                    }
                }
                GridRows = GetRowCount();
            }
        }
        private void DataGrid_ClientSizeChanged(object sender, EventArgs e)
        {
            SizeChange();
        }
        private void LabTestHistory_ClientSizeChanged(object sender, EventArgs e)
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
                DataGrid.Width = this.Width - DATAGRID_WIDTH_DIFF;
                DataGrid.Columns[2].Width = DataGrid.Width - DATAGRID_DESC_COLUMN_DIFF;
            }
            if (this.Height - DATAGRID_HEIGHT_DIFF < MIN_HEIGHT)
            {
                this.Height = MIN_HEIGHT;
            }
            else
            {
                DataGrid.Height = this.Height - DATAGRID_HEIGHT_DIFF;
            }
        }
        private void DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == DataGrid.RowCount + 1) { LastRow = true; } else { LastRow = false; }
        }
        private void DataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void DataGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGrid.CurrentCell != null && DataGrid.CurrentCell.RowIndex > -1 && DataGrid.CurrentRow.Cells[(int)LabTestHistoryGridColumn.LID].Value != null)
            {
                LTestId = long.Parse(DataGrid.CurrentRow.Cells[(int)LabTestHistoryGridColumn.LID].Value.ToString()!);
                this.OnLoad(e);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (DataGrid.CurrentRow != null)
                {
                    if (keyData == (Keys.Tab) && DataGrid.CurrentRow.Index > -1)
                    {
                        if (DataGrid.CurrentRow.Index != DataGrid.Rows.Count - 1)
                        {
                            DataGrid.Select();
                            GridRowIndex = DataGrid.CurrentRow.Index + 1;
                            DataGrid.CurrentCell = DataGrid[0, DataGrid.CurrentRow.Index + 1];
                            DataGrid.CurrentCell.Selected = true;
                        }
                        else if (DataGrid.CurrentRow.Index == DataGrid.Rows.Count - 1)
                        {
                            LastRow = true;
                            DataGrid.Select();
                            GridRowIndex = DataGrid.CurrentRow.Index + 1;
                            DataGrid.CurrentCell = DataGrid[0, DataGrid.CurrentRow.Index];
                            DataGrid.CurrentCell.Selected = true;
                        }
                    }
                    if (keyData == (Keys.Shift | Keys.Tab) && DataGrid.CurrentRow.Index > -1)
                    {
                        if (DataGrid.CurrentRow.Index != 0)
                        {
                            DataGrid.Select();
                            GridRowIndex = DataGrid.CurrentRow.Index + 1;
                            DataGrid.CurrentCell = DataGrid[0, DataGrid.CurrentRow.Index - 1];
                            DataGrid.CurrentCell.Selected = true;
                        }
                        else if (DataGrid.CurrentRow.Index == 0)
                        {
                            LastRow = false;

                            var mainForm = this.FindForm() as FormConsulting;
                            if (mainForm != null)
                            {
                                ReverseTab = false;
                                mainForm.NavigateToTabControl(1);
                                return true;
                            }

                            DataGrid.CurrentCell = DataGrid[0, DataGrid.CurrentRow.Index];
                        }
                        else if (DataGrid.CurrentRow.Index == DataGrid.Rows.Count - 1)
                        {
                            LastRow = true;
                            DataGrid.Select();
                            GridRowIndex = DataGrid.CurrentRow.Index + 1;
                            DataGrid.CurrentCell = DataGrid[0, DataGrid.CurrentRow.Index];
                            DataGrid.CurrentCell.Selected = true;
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
        public int GetRowCount()
        {
            return DataGrid.Rows.Count;
        }
        public bool HasData()
        {
            return DataGrid.Rows.Count > 0;
        }
    }
}
