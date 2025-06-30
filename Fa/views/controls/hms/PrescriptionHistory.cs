using fa.api.catalog;
using fa.api.Hms;
using fa.model.Catalog;
using fa.model.hms.common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using fa.model.Hms.common;
using fa.api.Accounting;
using fa.model.Employee;
using fa.views.hms.patient;

namespace fa.views.controls.hms
{
    public enum PrescriptionHistoryGridColumn
    {
        SELECT, DATE, NOS, AUTHOR, DETAIL, NOTEID, EID, IsDischarge
    }
    public partial class PrescriptionHistory : UserControl
    {
        protected static int MIN_HEIGHT = 174;
        protected static int MIN_WIDTH = 200;
        protected static int DATAGRID_WIDTH_DIFF = 7;
        protected static int DATAGRID_HEIGHT_DIFF = 24;
        protected static int DATAGRID_NOS_COLUMN_DIFF = 110;
        protected static int DATAGRID_DESC_COLUMN_DIFF = 100;
        protected static int DATAGRID_PRESCRIBEDBY_DIFF = 140;
        protected static int DATAGRID_COLUM_ADJST_DIFF = 67;


        public PrescriptionHistory()
        {
            InitializeComponent();
            DataGrid.Enter += DataGrid_Enter!;
            this.Enter += PrescriptionHistory_Enter!;
        }
        public long NoteId = 0L;
        public long RefNos = 0L;
        public string RefeBy;
        public long? EmpId = 0L;
        public bool IsDischarge = false;
        private long _PatientId = 0L;
        public int GridRows = 0;
        public int GridRowIndex = 0;
        public bool LastRow = false;
        public bool ReverseTab = false;

        public int SelectedRowIndex = 0;
        public int LastSelectedRowIndex = 0;
        public bool RowSelection = false;
        public bool RowClickedData = false;

        public long PatientId
        {
            get
            {
                return _PatientId;
            }
            set
            {
                _PatientId = value;
                LoadPrescriptionFilterHistory();
            }
        }
        public void Clear()
        {
            DataGrid.Rows.Clear();
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
                LoadPrescriptionFilterHistory();
                _HiddenNoteId = 0L;
            }
        }
        private bool _LoadAllPrescription = true;
        public bool LoadAllPrescription
        {
            get
            {
                return _LoadAllPrescription;
            }
            set
            {
                _LoadAllPrescription = value;
            }
        }
        int ccurrow = 0;

        private void LoadPrescriptionFilterHistory()
        {
            if (PatientId != 0L)
            {
                DataGrid.Rows.Clear();
                IList<ConsultationNote> ConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientId((long)PatientId);

                if (ConsultationNote != null && ConsultationNote.Count > 0)
                {
                    int i = 0;
                    DateTime PrescrpDateTime = Global.getTransactionDate().AddMonths(-1);
                    if (LoadAllPrescription)
                    {
                        PrescrpDateTime = Global.getTransactionDate().AddYears(-100);
                    }
                    foreach (ConsultationNote Note in ConsultationNote.Where(d => d.Date > PrescrpDateTime))
                    {
                        if (Note.IsDischarged)
                        {
                            IList<DischargePrescription> DischargePrescription = DischargeNoteManager.Instance.ListDischargePrescriptionByPatientIpId((long)Note.InPatientAdmissionId!);
                            if (DischargePrescription != null && DischargePrescription.Count > 0)
                            {
                                string PrescriptionsName = string.Empty;
                                long PRescriberId = 0L;
                                foreach (DischargePrescription DisPrescription in DischargePrescription)
                                {
                                    Product Product = DisPrescription.Prescription.ProductId != null ? CatalogProductManager.Instance.GetProductInfoById((long)DisPrescription.Prescription.ProductId) : null!;
                                    PrescriptionsName = string.IsNullOrEmpty(PrescriptionsName) ? (Product == null ? DisPrescription.Prescription.CustomProduct : Product.Name) : PrescriptionsName + ",\n" + (Product == null ? DisPrescription.Prescription.CustomProduct : Product.Name);
                                }
                                if (DischargePrescription.Count > 0)
                                {
                                    Prescription lPrescription = DischargePrescription.Last().Prescription;
                                    if (lPrescription != null)
                                    {
                                        PRescriberId = lPrescription.PrescribedByDoctorId != null ? (long)lPrescription.PrescribedByDoctorId : 0L;
                                        DataGrid.Rows.Add(1);
                                        DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.SELECT].Value = i + 1;
                                        DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.DATE].Value = Note.Date.ToString(Global.Company.DateFormat) + " " + Note.Date.ToShortTimeString();
                                        DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.NOS].Value = lPrescription.PrescriptionNumber;
                                        if (PRescriberId != 0L)
                                        {
                                            Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById(PRescriberId);
                                            if (Emp != null)
                                            {
                                                DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value = Emp.Name;
                                                DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.EID].Value = PRescriberId;
                                            }
                                            else
                                            {
                                                DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value = lPrescription.ToString();
                                                //DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.EID].Value = Global.User.UserId;
                                            }
                                        }
                                        else
                                        {
                                            DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value = lPrescription.ToString();
                                            //DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.EID].Value = Global.User.UserId;
                                        }
                                        DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.NOTEID].Value = Note.Id;
                                        DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.DETAIL].Value = PrescriptionsName;
                                        DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.IsDischarge].Value = true;
                                        i++;
                                    }
                                }
                            }
                            DataGrid.ClearSelection();
                        }
                        else
                        {
                            if (HiddenNoteId != Note.Id)
                            {
                                IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(Note.Id);
                                if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                                {
                                    string PrescriptionDetails = string.Empty;
                                    long PRescriberId = 0L;
                                    foreach (ConsultedPrescription ConsPres in lConsultedPrescription)
                                    {
                                        Product Product = ConsPres.Prescription.ProductId != null ? CatalogProductManager.Instance.GetProductInfoById((long)ConsPres.Prescription.ProductId) : null!;
                                        PrescriptionDetails = string.IsNullOrEmpty(PrescriptionDetails) ? (Product != null ? Product.Name : ConsPres.CustomPrescription) : PrescriptionDetails + ", " + (Product != null ? Product.Name : ConsPres.CustomPrescription);
                                    }
                                    if (lConsultedPrescription.Count > 0)
                                    {
                                        Prescription lPrescription = lConsultedPrescription.Last().Prescription;
                                        Prescription llPrescription = lConsultedPrescription.First().Prescription;
                                        if (lPrescription != null)
                                        {
                                            PRescriberId = lPrescription.PrescribedByDoctorId != null ? (long)lPrescription.PrescribedByDoctorId : 0L;
                                            DataGrid.Rows.Add(1);
                                            DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.SELECT].Value = i + 1;
                                            DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.DATE].Value = Note.Date.ToString(Global.Company.DateFormat) + " " + Note.Date.ToShortTimeString();
                                            DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.NOS].Value = llPrescription.PrescriptionNumber;
                                            if (PRescriberId != 0L)
                                            {
                                                Employee Emp = EmployeeManager.Instance.GetEmployeeInfoById(PRescriberId);
                                                if (Emp != null)
                                                {
                                                    DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value = Emp.Name;
                                                    DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.EID].Value = PRescriberId;
                                                }
                                                else
                                                {
                                                    DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value = lPrescription.ToString();
                                                    // DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.EID].Value = Global.User.UserId;
                                                }
                                            }
                                            else
                                            {
                                                DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value = lPrescription.ToString();
                                                //DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.EID].Value = Global.User.UserId;
                                            }

                                            DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.NOTEID].Value = Note.Id;
                                            DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.DETAIL].Value = PrescriptionDetails;
                                            DataGrid.Rows[i].Cells[(int)PrescriptionHistoryGridColumn.IsDischarge].Value = false;
                                            i++;
                                        }
                                    }
                                }
                            }
                            DataGrid.ClearSelection();
                        }
                    }
                }
                if (ccurrow > 0)
                {
                    DataGrid.CurrentCell = DataGrid[(int)PrescriptionHistoryGridColumn.DATE, ccurrow];
                }
                GridRows = GetRowCount();
            }
        }
        private void PrescriptionHistory_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.Width - DATAGRID_WIDTH_DIFF < MIN_WIDTH)
            {
                this.Width = MIN_WIDTH;
            }
            else
            {
                DataGrid.Width = this.Width - DATAGRID_WIDTH_DIFF;
                DataGrid.Columns[4].Width = DataGrid.Width - (DATAGRID_DESC_COLUMN_DIFF + DATAGRID_PRESCRIBEDBY_DIFF + DATAGRID_NOS_COLUMN_DIFF + DATAGRID_COLUM_ADJST_DIFF);                 //DataGrid.Columns[2].Width = DataGrid.Width - DATAGRID_DESC_COLUMN_DIFF;
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
            if (e.RowIndex >= 0 && RowSelection == true)
            {
                DataGrid.Rows[e.RowIndex].Selected = true;
                int selectedRowIndex = e.RowIndex;
                HandleRowSelection(e.RowIndex);

            }
            else if (RowSelection == false)
            {
                RowSelection = true;                
            }           
        }

        private void DataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        public int GetRowCount()
        {
            return DataGrid.Rows.Count;
        }
        public DataGridViewRow GetSelectedRow()
        {
            if (DataGrid.SelectedRows.Count > 0)
            {
                return DataGrid.SelectedRows[0];
            }
            return null!;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (DataGrid.CurrentRow != null)
                {
                    int currentRowIndex = DataGrid.CurrentRow.Index;
                    int totalRows = DataGrid.Rows.Count;

                    // Handling Tab key press
                    if (keyData == Keys.Tab)
                    {
                        ReverseTab = false;
                        if (currentRowIndex < totalRows - 1)
                        {
                            GridRowIndex = currentRowIndex + 1;
                            DataGrid.CurrentCell = DataGrid[0, GridRowIndex];

                            DataGrid_CellClick(DataGrid, new DataGridViewCellEventArgs(0, DataGrid.CurrentCell.RowIndex));
                        }
                        else if (currentRowIndex == totalRows - 1)
                        {
                            LastRow = true;

                            var mainForm = this.FindForm() as FormConsulting;
                            if (mainForm != null)
                            {
                                mainForm.NavigateToTabControl(3);
                                return true;
                            }
                        }

                        DataGrid.CurrentCell.Selected = true;
                        DataGrid.Select();
                        return true; // Indicate that the key has been handled
                    }

                    // Handling Shift+Tab key press
                    if (keyData == (Keys.Shift | Keys.Tab))
                    {
                        ReverseTab = true;
                        if (currentRowIndex > 0)
                        {
                            GridRowIndex = currentRowIndex - 1;
                            DataGrid.CurrentCell = DataGrid[0, GridRowIndex];

                            DataGrid_CellClick(DataGrid, new DataGridViewCellEventArgs(0, DataGrid.CurrentCell.RowIndex));
                        }
                        else
                        {
                            LastRow = false;

                            var mainForm = this.FindForm() as FormConsulting;
                            if (mainForm != null)
                            {
                                ReverseTab = false;
                                mainForm.NavigateToTabControl(0);
                                return true;
                            }

                            DataGrid.CurrentCell = DataGrid[0, currentRowIndex];
                        }

                        DataGrid.CurrentCell.Selected = true;
                        DataGrid.Select();
                        return true; // Indicate that the key has been handled
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DataGrid_Enter(object sender, EventArgs e)
        {
            if (DataGrid.Rows.Count > 0)
            {
                int rowIndex = 0;

                if (ReverseTab)
                {
                    rowIndex = DataGrid.Rows.Count - 1;
                }

                DataGrid.CurrentCell = DataGrid.Rows[rowIndex].Cells[0];
                DataGrid.Rows[rowIndex].Selected = true;
                var cellEventArgs = new DataGridViewCellEventArgs(0, rowIndex);
                DataGrid_CellClick(DataGrid, cellEventArgs);
            }
        }

        private void HandleRowSelection(int rowIndex)
        {
            if (rowIndex == DataGrid.RowCount - 1)
            {
                LastRow = true;
            }
            else
            {
                LastRow = false;
            }

            var currentRow = DataGrid.Rows[rowIndex];

            NoteId = long.Parse(currentRow.Cells[(int)PrescriptionHistoryGridColumn.NOTEID].Value.ToString()!);
            IsDischarge = (bool)currentRow.Cells[(int)PrescriptionHistoryGridColumn.IsDischarge].Value;
            RefNos = currentRow.Cells[(int)PrescriptionHistoryGridColumn.NOS].Value == null ? 0L :
                     currentRow.Cells[(int)PrescriptionHistoryGridColumn.NOS].Value.ToString() == "" ? 0L :
                     long.Parse(currentRow.Cells[(int)PrescriptionHistoryGridColumn.NOS].Value.ToString()!);
            RefeBy = currentRow.Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value == null ? string.Empty :
                     currentRow.Cells[(int)PrescriptionHistoryGridColumn.AUTHOR].Value.ToString()!;
            EmpId = (currentRow.Cells[(int)PrescriptionHistoryGridColumn.EID].Value != null && !string.IsNullOrEmpty(currentRow.Cells[(int)PrescriptionHistoryGridColumn.EID].Value.ToString())) ?
                    long.Parse(currentRow.Cells[(int)PrescriptionHistoryGridColumn.EID].Value.ToString()!) : null;

            this.OnLoad(EventArgs.Empty); // Calling OnLoad method to refresh/load the data as needed
        }

        private void PrescriptionHistory_Enter(object sender, EventArgs e)
        {
            if (DataGrid.Rows.Count > 0)
            {
                //DataGrid.ClearSelection(); // Clear any previous selection
                int rowIndex = 0;
                if (ReverseTab)
                {
                    rowIndex = DataGrid.Rows.Count - 1;
                }
                if (RowSelection == true)
                {
                    rowIndex = SelectedRowIndex;
                    RowSelection = false;
                }
                else if (RowSelection == false)
                {
                    rowIndex = 0;
                    DataGrid.ClearSelection();
                }

                DataGrid.CurrentCell = DataGrid.Rows[rowIndex].Cells[0];
                DataGrid.Rows[rowIndex].Selected = true;
                var cellEventArgs = new DataGridViewCellEventArgs(0, rowIndex);
                DataGrid_CellClick(DataGrid, cellEventArgs);
            }
        }

        private void DataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                LastSelectedRowIndex = e.RowIndex;
            }
        }

        private void DataGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                LastSelectedRowIndex = e.RowIndex;
            }
        }
        public void SelectDataGrid_CellClick(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < DataGrid.RowCount)
            {
                DataGrid.CurrentCell = DataGrid[0, rowIndex];

                //DataGrid_CellClick(DataGrid, new DataGridViewCellEventArgs(0, DataGrid.CurrentCell.RowIndex));
                DataGrid.CurrentCell.Selected = true;
                //DataGrid.Select();
                return;
            }
        }
        public void FocusOnPrescriptionRow(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < DataGrid.Rows.Count)
            {
                //DataGrid.ClearSelection(); // Clear previous selection

                // Select the specified row
                DataGrid.Rows[rowIndex].Selected = true;

                // Set the current cell to ensure focus is placed on the specified row
                DataGrid.CurrentCell = DataGrid.Rows[rowIndex].Cells[0];

                // Set focus to the DataGrid
                DataGrid.Focus();
            }
        }
    }
}
