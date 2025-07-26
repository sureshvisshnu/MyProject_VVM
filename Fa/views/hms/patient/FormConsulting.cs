using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using fa.model.Hms.common;
using fa.api.Hms;
using fa.model.hms.common;
using fa.views.hms.helper;
using fa.model.Hms.Master;
using Fa.api.Hms;
using fa.model.Catalog;
using fa.api.catalog;
using fa.views.utils.Hms;
using System.IO;
using fa.model.Hms.Ip;
using fa.views.hms.ip;
using fa.model.Hms.Op;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using fa.common;
using fa.libraries.utils;
using System.Diagnostics;
using VisioForge.MediaFramework.Helpers;
using NPOI.SS.Formula.Functions;
using Standard;
using VisioForge.Libs.MediaFoundation.OPM;
using fa.views.controls.grid;
using fa.api.utils;
using Fa.views.hms.helper;
using fa.views.controls.hms;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Google.Protobuf.WellKnownTypes;
using Enum = System.Enum;
using System.Runtime.CompilerServices;
using System.Speech.Recognition;
using Fa.views.utils.Report.Hms;

namespace fa.views.hms.patient
{
    public enum AddFileUploadGridColumn
    {
        SNO, NAME, DESC, CHOOSE, REMOVE, PATH, TYPE, ID
    }
    public enum ConsultationNotesGridColumn
    {
        DATE, IPOP, CONSULTATIONNOTES, SYMPTOM, ADDSYMPTOM, PRESCRIPTION, ADDPRESCRIPTION, LABTEST, ADDLABTEST, PROCEDURE, ADDPROCEDURE, CONSULTATION, FEE, ADDCONSULTATION, REMOVE, CONSULTINGNOTESID, SYMPTOMSIDS, PRESCRIPTIONIDS, LABTESTIDS, CONSULTATIONIDS, CONSULTATIONFEES, LABTESTELEMENTIDS, PROCEDUREIDS, PROCEDUREDISC, PROCEDUREFEES, PRESCRIPTIONNAME, TOTAL, DOSAGEDAYS, BEFOREAFTER, INTERVAL, MORNING, AFTERNOON, EVENING, NIGHT, ADDITONALNOTES, OPID, IPID, ISDISCHARGED, SYMPTOMDISC, CONSULTATIONDISC, ISVISITCOMPLETE, INVOICED, LABTESTFEES, LABTESTDESC
    }
    public enum PrescriptionDetailsGridColumn
    {
        NAME, TOTAL, DAYS, BEFOREAFTER, HOURS, MORNING, AFTERNOON, EVENING, NIGHT, ADDNOTES, PID
    }
    public enum MedicalLabTestElementsGridColumn
    {
        SNO, NAME, UOM, CLASS, SUBCLASS, RESULT, SINGLEVALUE, RANGEFROM, RANGETO, ID, TESTID
    }
    public enum ProcedureInfoGridColumn
    {
        SLNO, PDATE, NAME, DISC, REQBY, STAT, PERFMBY, PERFMON, NOTE, FEES, PID
    }
    public partial class FormConsulting : FormPatientBase
    {
        public static string DoNotAllowToEditInvoiced = "Not allowed to edit, prescription/procedure/fee/medical test has been sent to invoice";
        public static string DoNotAllowToDeleteDispatchedToMedicalMsg = "Could not delete, prescription has been sent to pharmacy already";
        public static string DoNotAllowToEditDispatchedToMedicalMsg = "Not allowed to edit, prescription has been sent to pharmacy already";
        public static string DoNotAllowToDeleteOtherConsultantConsultationMsg = "You do not have permission to delete this note";
        public static string DoNotAllowToDeleteDischargeConsultationMsg = "You cannot delete this consultation notes as the patient has been discharged";
        public static string DoNotAllowToDeleteInvoicedConsultationMsg = "You cannot delete, invoice generated for the consultation notes.";

        public static string SaveSuccessMsg = "Saved...";
        public static string DoNotAllowToEditOtherConsultantConsultationMsg = "You do not have permission to edit this note";
        public static string EnterConsultationDetailErrorMsg = "Please enter a note";
        public static string SelectConsultationSymptomErrorMsg = "Please select diagnostics details";
        public static string EnterConsultationNoteErrorMsg = "Consultation Note is required, Please add a note";

        public static string SelectPrescripedMedicienPreferedAfterorBeforeFoodErrorMsg = "Please select prescriped medicine to be taken after/before food";
        public static string EnterPrescripedMedicienDosageErrorMsg = "Please enter the dosage details";
        public static string EnterPrescripedMedicienTakenDaysErrorMsg = "Please enter the number of days for the medication";
        public static string SelectPrescripedMedicienTakenTimeDelayErrorMsg = "Please select the interval for the medication";
        public static string SelectPrescripedMedicienTakenTimeErrorMsg = "Please select the interval for the medication";
        public static string PrescriptionSendMedicalMsg = "Prescriptions has been sent to medical";
        public static string PrescriptionTimeIntervalErrorMsg = "Can't have a breakup for Time Interval";

        public static string EnterLabtestElementErrorMsg = "Please enter {0}";
        public static string UpLoadFileErrorMsg = "The file is too large, maximum size for the file is 5 MB";

        public static string PatientVisitErrorMsg = "This Patient completed is OP Visit";
        public static string EnterConsultationPrescriberErrorMsg = "Please select authorizes prescriber.";
        public static string UpdateFileErrorMsg = "An error occurred: {0} Error";

        long PatientId = 0L;
        public Patient PatientDetail = null!;
        public long? PatientOpId = null;
        public long? PatientIPId = null;
        public long? LastConsultedNoteId = null;
        public PatientTypes PatientType;
        public static int id;
        public static string selectedItems = "";
        public static string selectedPItems = "";
        public bool RecordEnter = false;
        private bool isRowAdding = true;
        private object DocFile = null!;
        private string PatientChartFilePath;
        private string PatientChartFileName;
        public LabTestAttachment LabTestAttachment;
        private bool shouldExecuteEditingControlShowing = false;
        FormPatientBase parent = null!;

        private List<string> popupList;
        private System.Windows.Forms.ListView popupListView;
        private List<string> autoCompleteList = new List<string>();

        private System.Windows.Forms.TextBox currentTextBox;
        private System.Windows.Forms.TextBox editingTextBox;

        private bool isKeyboardInput = false;
        private bool isArrowKeyInput = false;

        private DataGridViewCell PreviousCell = null!;

        private LabTestHistory labTestHistoryControl;
        private PrescriptionHistory PrescriptionHistoryControl;
        private PatientVitalEntry patientVitalEntry;
        private OutPatientVisit OPVisitInfo;
        private InPatientVisit IPVisitInfo;

        public int LabHistoryRowCount = 0;
        public int LabHistoryRowIndex = 0;
        private bool LabHistoryIsLastRow = false;

        public int PrescriptionHistoryRowCount = 0;
        public int PrescriptionHistoryRowIndex = 0;
        private bool PrescriptionHistoryIsLastRow = false;

        public int OPVisitRowCount = 0;
        public int OPVisitRowIndex = 0;
        private bool OPVisitIsLastRow = false;
        private bool OPVisitIsFirstRow = false;

        public int IPVisitRowCount = 0;
        public int IPVisitRowIndex = 0;
        private bool IPVisitIsLastRow = false;
        private bool IPVisitIsFirstRow = false;
        private int previousTabIndex = 0;

        private bool MedicationTabSelected = false;
        private bool PatientLabTestSelected = false;
        private bool PatientLabTestDetailSelected = false;

        private Dictionary<string, int> MedicalHistorycolumnWidths;

        private bool GridViewProcedureInfoIsLastRow = false;

        private bool IsPatientVitalEntryIsFocused = false;
        private bool IsMedicalHistoryTabFocused = false;
        private bool IsPatientVisitEntryFocused = false;
        private bool IsConsultantNotesFocused = false;
        private bool IsPatientPrescriptionFocused = false;
        private bool IsPatientLabTestFocused = false;
        private bool IsPatientChartFocused = false;
        private bool IsPatientConsultNoteFocused = false;
        private bool IsMedicalProcedureFocused = false;

        private bool MedicalHistoryGridWidthChanged = false;
        private bool PrescriptionHistoryDataSaved = false;

        private bool PreventGridViewPatientHistoryEnterEvent = false;
        private Dictionary<(int rowIndex, int colIndex), bool> cellTagDictionary;


        private System.Windows.Forms.Timer searchDelayTimer;
        private int searchDelayMilliseconds = 300;
        public DialogResult isConsulted = DialogResult.None;

        public FormConsulting(object sender)
        {
            this.Cursor = Cursors.WaitCursor;
            InitializeComponent();
            BtnPrescriptionSendMedical.Visible = !Global.Company.HasProductCatalog ? false : true;
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }

            searchDelayTimer = new System.Windows.Forms.Timer();
            searchDelayTimer.Interval = searchDelayMilliseconds;
            searchDelayTimer.Tick += SearchDelayTimer_Tick;

            this.KeyPreview = true;
            this.MouseClick += new MouseEventHandler(FormConsulting_MouseClick!);

            GridViewNote.MouseClick += new MouseEventHandler(GridViewNote_MouseClick!);
            GridViewNote.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(ConsultationNotesGrid_EditingControlShowing!);
            GridViewNote.RowPostPaint += new DataGridViewRowPostPaintEventHandler(GridViewNote_RowPostPaint!);
            this.GridViewNote.KeyDown += new KeyEventHandler(this.GridViewNote_KeyDown!);
            this.GridViewNote.PreviewKeyDown += new PreviewKeyDownEventHandler(this.GridViewNote_PreviewKeyDown!);

            GridViewPrescriptionDetail.KeyDown += GridViewPrescriptionDetail_KeyDown!;
            BtnPrescriptionSave.KeyDown += BtnPrescription_KeyDown!;
            BtnPrescriptionPreview.KeyDown += BtnPrescription_KeyDown!;
            BtnPrescriptionPrint.KeyDown += BtnPrescription_KeyDown!;
            BtnPrescriptionSendMedical.KeyDown += BtnPrescription_KeyDown!;
            BtnPrescriptionCancel.KeyDown += BtnPrescription_KeyDown!;



            InitializePopupListView();

            labTestHistoryControl = new LabTestHistory();
            PrescriptionHistoryControl = new PrescriptionHistory();
            patientVitalEntry = new PatientVitalEntry();
            OPVisitInfo = new OutPatientVisit();
            IPVisitInfo = new InPatientVisit();


            PatientVitalHistory.RowDeleted += PatientVitalHistory_RowDeleted;
            popupListView!.MouseClick += new MouseEventHandler(PopupListView_MouseClick!);
            //TabControlConsult.SelectedIndexChanged += TabControlConsult_SelectedIndexChanged!;

            this.Cursor = Cursors.Default;

            //GridViewLabTestElementInformation.RowTemplate.Height = 20;

        }

        private void InitializePopupListView()
        {
            popupListView = new System.Windows.Forms.ListView
            {
                View = View.List,
                FullRowSelect = true,
                Width = 363,
                Height = 200,
                Visible = false
            };

            popupListView.KeyDown += PopupListView_KeyDown!;
            popupListView.DoubleClick += PopupListView_DoubleClick!;

            this.Controls.Add(popupListView);
        }
        private void FormConsulting_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            BtnConsultLabTestPrintRequisition.Visible = false;
            PatientId = parent != null ? long.Parse(parent.PatientIdTransport.Text) : long.Parse(PatientIdTransport.Text);
            LoadConsultingData(PatientId);
            PopulateAutoCompleteList();
            this.Cursor = Cursors.Default;
            ResetDirtyFlag();

            GridViewPrescriptionHistory.Leave += Control_Leave!;
            GridViewPrescriptionDetail.Leave += Control_Leave!;
            BtnPrescriptionSave.Leave += Control_Leave!;
            BtnPrescriptionPrint.Leave += Control_Leave!;
            BtnPrescriptionPreview.Leave += Control_Leave!;
            BtnPrescriptionSendMedical.Leave += Control_Leave!;
            BtnPrescriptionCancel.Leave += Control_Leave!;

            this.formIsDirty = false;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewNote.Columns["Fee"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
            GridViewNote.Rows[GridViewNote.Rows.Count - 1].Selected = true;
            GridViewNote.CurrentCell = GridViewNote.Rows[GridViewNote.Rows.Count - 1].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES];
        }

        private void LoadConsultingData(long Id)
        {
            BtnCompleteConsultation.Enabled = false;
            ConsultPatientInfo.Type = PatientType;
            ConsultPatientInfo.PatientId = Id;
            LoadIpOpVisit();
            PatientVitalEntry.PatientId = Id;
            PatientVitalHistory.PatientId = Id;
            GraphChartControlVital.PatientId = Id;
            LoadNotesbyFilter();
            loadPrescriptionHistory();
            loadLabTestHistory();
            LoadProcedureHistory();
            //LoadPatientChart();
            if (!MedicalHistoryGridWidthChanged) { MedicalHistoryInitialColumnWidths(); }
            LoadMedicalHistory();
            LoadMedicalHistoryData();
            LockAllGridViewCells();
            cellTagDictionary = CreateCellTagDictionary();
            UnLockGridViewSelectedCells();
            if (PatientIPId != null)
            {
                LinkDischargePatient.Visible = true;
            }
            if (PatientOpId != null)
            {
                BtnCompleteConsultation.Enabled = true;
            }
            EnableForm();
        }
        private void EnableForm()
        {
            //if (Global.User.IsSuperAdmin == false)
            {
                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                if (Patient != null && Patient.IsDeceased)
                {
                    LockScreen();
                }
            }
        }
        private void LoadIpOpVisit()
        {
            Cursor.Current = Cursors.WaitCursor;
            ConsultInPatientVisit.PatientId = PatientId;
            IPVisitRowCount = ConsultInPatientVisit.GridRows;
            ConsultOutPatientVisit.PatientId = PatientId;
            OPVisitRowCount = ConsultOutPatientVisit.GridRows;
            Cursor.Current = Cursors.Default;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {

            PatientVitalEntry.Clear();
            PatientVitalEntry.Select();
            ConsultedNoteErrMsg.Text = " ";
        }
        private void BtnSaveVitals_Click(object sender, EventArgs e)
        {
            SaveVitals(sender, e, true);
        }
        private void SaveVitals(object sender, EventArgs e, bool loadChart)
        {
            Cursor.Current = Cursors.WaitCursor;
            ConsultedNoteErrMsg.Text = "";
            if (PatientVitalEntry.ValidatePatientVitalEntry())
            {
                if (PatientId == 0)
                {
                    ConsultedNoteErrMsg.Text = "Error: PatientId is not assigned.";
                    Cursor.Current = Cursors.Default;
                    return;
                }

                Vital vital = PatientVitalEntry.GetVitalDetails();
                vital.OpRegistrationId = PatientOpId;
                if (vital.Id == 0L)
                {
                    VitalEntryManager.Instance.AddVital(vital);
                }
                else
                {
                    VitalEntryManager.Instance.UpdateVital(vital);
                }
                PatientVitalEntry.Clear();
                PatientVitalHistory.PatientId = PatientId;
                ConsultedNoteErrMsg.Text = SaveSuccessMsg;
            }
            else
            {
                ConsultedNoteErrMsg.Text = PatientVitalEntry.ErrorMsg();
                Cursor.Current = Cursors.Default;
                return;
            }

            GraphChartControlVital.PatientId = PatientId;
            Cursor.Current = Cursors.Default;
        }

        private void BtnSaveVitals_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;

                if (e.Shift)
                {
                    PatientVitalEntry.FocusIndex = 9;
                    PatientVitalEntry.PatientId = PatientId;
                    PatientVitalEntry.Focus();
                }
                else
                {
                    BtnResetVital.Select();
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;

            if (keyData == (Keys.F4))
            {
                BtnPrescriptionPreview.PerformClick();
            }
            if (keyData == (Keys.F9))
            {
                if (TabControlConsult.SelectedTab == TabControlConsult.TabPages["TabLab"])
                {
                    if (TabControlLabtest.SelectedTab == TabConsultLabtestDetails)
                    {
                        BtnConsultLabTestPrintRequisition.PerformClick();
                    }
                }
                if (TabControlConsult.SelectedTab == TabControlConsult.TabPages["TabMedication"])
                {
                    BtnPrescriptionPrint.PerformClick();
                }
                return true;
            }
            if (keyData == (Keys.F8))
            {
                if (TabControlConsult.SelectedIndex == 0)
                {
                    BtnNotesSave.PerformClick();
                }
                if (TabControlConsult.SelectedIndex == 1)
                {
                    BtnPrescriptionSave.PerformClick();
                }
                if (TabControlConsult.SelectedIndex == 2)
                {
                    if (TabControlLabtest.SelectedTab == TabConsultLabtestImage)
                    {
                        BtnLabTestImgSave.PerformClick();
                    }
                    else
                    {
                        BtnConsultLabTestElementSave.PerformClick();
                    }
                }
                if (TabControlConsult.SelectedIndex == 3)
                {
                    BtnSaveProcedureInfo.PerformClick();
                }
                if (TabControlConsult.SelectedIndex == 4)
                {
                    BtnSaveVitals.PerformClick();
                }
                else if (TabControlConsult.SelectedIndex == 6)
                {
                    BtnMedicalHistorySave.PerformClick();
                }
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                if (TabControlConsult.SelectedIndex == 0)
                {
                    BtnNoteCancel.PerformClick();
                }
                else if (TabControlConsult.SelectedIndex == 1)
                {
                    BtnPrescriptionCancel.PerformClick();
                }
                else if (TabControlConsult.SelectedIndex == 2)
                {
                    if (TabControlLabtest.SelectedTab == TabConsultLabtestImage)
                    {
                        BtnLabTestImgCancel.PerformClick();
                    }
                    else
                    {
                        BtnConsultLabTestElementCancel.PerformClick();
                    }
                }
                else if (TabControlConsult.SelectedIndex == 3)
                {
                    BtnCancelProcedureInfo.PerformClick();
                }
                else if (TabControlConsult.SelectedIndex == 4)
                {
                    BtnResetVital.PerformClick();
                }
                else if (TabControlConsult.SelectedIndex == 6)
                {
                    BtnMedicalHistoryCancel.PerformClick();
                }
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnNoteExit.PerformClick();
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == CheckBoxLoadAllNotes && LinkDischargePatient.Visible)
            {
                BtnNoteExit.Focus();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnCancelProcedureInfo)
            {
                BtnSaveProcedureInfo.Focus();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == LinkDischargePatient)
            {
                CheckBoxLoadAllNotes.Focus();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnNoteExit)
            {
                if (BtnCompleteConsultation.Enabled)
                {
                    BtnCompleteConsultation.Focus();
                    return true;
                }
                else
                {
                    BtnNoteCancel.Focus();
                    return true;
                }
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnCompleteConsultation)
            {
                BtnNoteCancel.Focus();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnNoteCancel)
            {
                BtnNotesSave.Focus();
                return true;
            }
            if (keyData == Keys.Tab && popupListView.Visible)
            {
                PopupListView_KeyDown(popupListView, new KeyEventArgs(Keys.Tab));
                return true;
            }

            if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
            {
                // Handle Tab key
                if ((keyData & Keys.KeyCode) == Keys.Tab)
                {
                    if ((keyData & Keys.Shift) == Keys.Shift)
                    {
                        // Handle Shift + Tab
                        HandleTabControlShiftTab(keyData);
                    }
                    else if ((keyData & Keys.Control) == Keys.Control)
                    {
                        // Handle Control + Tab
                        HandleTabControlSwitch();
                    }
                    else
                    {
                        // Handle Tab
                        HandleTabControlTab(keyData);
                    }
                    return true;
                }
                if (keyData == Keys.End && TabControlConsult.SelectedIndex == 0)
                {
                    HandleEndKey();
                    return true;
                }
                if (TabControlConsult.SelectedIndex == 0)
                {
                    if ((keyData & Keys.KeyCode) == Keys.Enter || (keyData & Keys.KeyCode) == Keys.Space)
                    {
                        if (popupListView.Visible)
                        {
                            if (isKeyboardInput)
                            {
                                PopupListView_KeyDown(popupListView, new KeyEventArgs(Keys.Tab));
                                return true;
                            }
                        }
                        if (GridViewNote.CurrentCell != null)
                        {
                            int rowIndex = GridViewNote.CurrentCell.RowIndex;
                            int columnIndex = GridViewNote.CurrentCell.ColumnIndex;

                            // Allow space key in the specified column
                            if (keyData == Keys.Space && columnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
                            {
                                return base.ProcessCmdKey(ref msg, keyData); // Allow normal space input
                            }
                            HidePopupListView();
                            SimulateCellContentClick(rowIndex, columnIndex);
                            return true;
                        }
                    }
                }
                if (TabControlConsult.SelectedIndex == 6)
                {
                    if (keyData == Keys.Down || keyData == Keys.Up || keyData == Keys.Left || keyData == Keys.Right)
                    {
                        HandleKeyPressForPatientMedicalHistory(keyData);
                        return true;
                    }
                }
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnNotesSave)
            {
                int Rows = GridViewNote.Rows.Count - 1;
                GridViewNote.Focus();
                GridViewNote.CurrentCell = GridViewNote[13, Rows];
                GridViewNote.BeginEdit(true);
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnPrescriptionSave)
            {
                int Rows = GridViewPrescriptionDetail.Rows.Count - 1;
                GridViewPrescriptionDetail.Focus();
                GridViewPrescriptionDetail.CurrentCell = GridViewNote[0, Rows];
                GridViewPrescriptionDetail.BeginEdit(true);
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift) && ActiveControl == BtnSaveProcedureInfo)
            {
                int Rows = GridViewProcedureInfo.Rows.Count - 1;
                SelectEntireRow(Rows);
                return true;
            }
            if (GridViewNote.CurrentCell != null && keyData == Keys.Down && GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                if (popupListView.Visible && popupListView.Items.Count > 0)
                {
                    popupListView.Focus();
                    popupListView.Items[0].Selected = true;
                }
            }
            if (GridViewNote.CurrentCell != null && GridViewNote.Focused)
            {
                ConsultationNote Note = null!;
                if (GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value != null)
                {
                    Note = ConsultationNoteManager.Instance.GetConsultationNoteById(long.Parse(GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value.ToString()!));
                }
                if (GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value == null || (Note != null && Note.ConsultantId == Global.User.UserId))
                {
                    if ((keyData == Keys.F2))
                    {
                        if (Note == null || (Note != null && !Note.IsPrescriptionDispatchedForMedical))
                        {
                            if (Note == null || Note != null && !Note.IsInvoiced)
                            {
                                if ((keyData == Keys.F2) && GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.SYMPTOM)
                                {
                                    FormSelectSymptom formSelectSymptom = new FormSelectSymptom(this);
                                    formSelectSymptom.SelectedSymptomsids = (DataGridViewComboBoxCell)GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value;
                                    formSelectSymptom.SelectedSymptomsNames = GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value != null ? GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value.ToString()! : string.Empty;
                                    formSelectSymptom.SelectedSymptomsDiscp = (DataGridViewComboBoxCell)GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value;
                                    formSelectSymptom.ShowDialog();
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value = formSelectSymptom.SelectedSymptomsids;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value = formSelectSymptom.SelectedSymptomsNames;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value = formSelectSymptom.SelectedSymptomsDiscp;
                                    return true;
                                }
                                else if ((keyData == Keys.F2) && GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.PRESCRIPTION)
                                {
                                    FormSelectPrescriptions formSelectPrescription = new FormSelectPrescriptions(this);
                                    formSelectPrescription.SelectedPrescriptionIds = (DataGridViewComboBoxCell)GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value;
                                    formSelectPrescription.SelectedPrescriptionsNames = GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value != null ? GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value.ToString()! : string.Empty;
                                    formSelectPrescription.ShowDialog();
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value = formSelectPrescription.SelectedPrescriptionIds;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value = formSelectPrescription.SelectedPrescriptionsNames;
                                    return true;
                                }
                                else if ((keyData == Keys.F2) && GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.LABTEST)
                                {
                                    FormSelectLabTest FormSelectLabTest = new FormSelectLabTest(this);
                                    FormSelectLabTest.SelectedLabTestsDisc = (DataGridViewComboBoxCell)GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value;
                                    FormSelectLabTest.SelectedLabTestsFees = (DataGridViewComboBoxCell)GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value;
                                    FormSelectLabTest.SelectedLabTestsids = (DataGridViewComboBoxCell)GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value;
                                    FormSelectLabTest.SelectedLabTestsElementids = (DataGridViewComboBoxCell)GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value;
                                    FormSelectLabTest.SelectedLabTestsNames = GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value != null ? GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value.ToString()! : string.Empty;
                                    FormSelectLabTest.ShowDialog();
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value = FormSelectLabTest.SelectedLabTestsids;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value = FormSelectLabTest.SelectedLabTestsElementids;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.LABTEST].Value = FormSelectLabTest.SelectedLabTestsNames;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value = FormSelectLabTest.SelectedLabTestsDisc;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value = FormSelectLabTest.SelectedLabTestsFees;

                                    return true;
                                }
                                else if ((keyData == Keys.F2) && GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.FEE)
                                {
                                    FormSelectConsultation FormSelectConsultation = new FormSelectConsultation(this);
                                    FormSelectConsultation.SelectedConsultationsids = (DataGridViewComboBoxCell)GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value;
                                    FormSelectConsultation.SelectedConsultationsFees = (DataGridViewComboBoxCell)GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value;
                                    FormSelectConsultation.SelectedConsultationsDiscrp = (DataGridViewComboBoxCell)GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value;
                                    FormSelectConsultation.SelectedConsultationsNames = GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value != null ? GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value.ToString()! : string.Empty;
                                    FormSelectConsultation.ShowDialog();
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value = FormSelectConsultation.SelectedConsultationsids;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value = FormSelectConsultation.SelectedConsultationsNames;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value = FormSelectConsultation.SelectedConsultationsFees;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.FEE].Value = TotalFee(FormSelectConsultation.SelectedConsultationsFees);
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value = FormSelectConsultation.SelectedConsultationsDiscrp;
                                    return true;
                                }
                                else if ((keyData == Keys.F2) && GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.PROCEDURE)
                                {
                                    FormSelectProcedures FormSelectProcedures = new FormSelectProcedures(this);
                                    FormSelectProcedures.SelectedProcedureIds = (DataGridViewComboBoxCell)GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value;
                                    FormSelectProcedures.SelectedProcedureDisc = (DataGridViewComboBoxCell)GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value;
                                    FormSelectProcedures.SelectedProcedureFees = (DataGridViewComboBoxCell)GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value;
                                    FormSelectProcedures.SelectedProcedureNames = GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value != null ? GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value.ToString()! : string.Empty;
                                    FormSelectProcedures.ShowDialog();
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value = FormSelectProcedures.SelectedProcedureIds;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value = FormSelectProcedures.SelectedProcedureNames;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value = FormSelectProcedures.SelectedProcedureDisc;
                                    GridViewNote.CurrentRow.Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value = FormSelectProcedures.SelectedProcedureFees;
                                    return true;
                                }
                            }
                            else
                            {
                                MessageBox.Show(DoNotAllowToEditInvoiced);
                            }
                        }
                        else
                        {
                            MessageBox.Show(DoNotAllowToEditDispatchedToMedicalMsg);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(DoNotAllowToEditOtherConsultantConsultationMsg);
                }
            }
            if (GridViewConsultLabTestImage.CurrentRow != null && GridViewConsultLabTestImage.Focused)
            {
                if (keyData == (Keys.Tab) && GridViewConsultLabTestImage.CurrentRow.Index > -1)
                {
                    if (GridViewConsultLabTestImage.CurrentRow.Index != GridViewConsultLabTestImage.Rows.Count - 1)
                    {

                        GridViewConsultLabTestImage.Select();
                        GridViewConsultLabTestImage.CurrentCell = GridViewConsultLabTestImage[0, GridViewConsultLabTestImage.CurrentRow.Index + 1];
                        GridViewConsultLabTestImage.CurrentCell.Selected = true;
                    }
                }
                if (keyData == (Keys.Shift | Keys.Tab) && GridViewConsultLabTestImage.CurrentRow.Index > -1)
                {
                    if (GridViewConsultLabTestImage.CurrentRow.Index != 0)
                    {
                        GridViewConsultLabTestImage.Select();
                        GridViewConsultLabTestImage.CurrentCell = GridViewConsultLabTestImage[0, GridViewConsultLabTestImage.CurrentRow.Index - 1];
                        GridViewConsultLabTestImage.CurrentCell.Selected = true;
                    }

                }
            }
            ConsultedNoteErrMsg.Text = PatientVitalEntry.ErrorMsg();
            if (TabControlConsult.SelectedTab.Name == "TabVitals")
            {
                ConsultedNoteErrMsg.Text = PatientVitalEntry.ErrorMsg();
            }
            else
            {
                ConsultedNoteErrMsg.Text = " ";
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RemovePrescriptionandLabtestByNote(long NoteId)
        {
            BtnPrescriptionPrint.Enabled = false;
            BtnPrescriptionPreview.Enabled = false;
            GridViewPrescriptionDetail.Rows.Clear();
            GridViewPrescriptionHistory.HiddenNoteId = NoteId;
            if (CheckBoxLoadAllNotes.Checked)
            {
                GridViewPrescriptionHistory.LoadAllPrescription = true;

            }
            else
            {
                GridViewPrescriptionHistory.LoadAllPrescription = false;
            }
            BtnConsultLabTestPrintRequisition.Enabled = false;
            BtnLabTestPrintRequisition.Enabled = false;
            GridViewLabTestElementInformation.Rows.Clear();
            GridViewLabTestHistory.HiddenNoteId = NoteId;
        }
        int Rowindex = -1;
        private void ConsultationNotesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ConsultedNoteErrMsg.Text = string.Empty;
            if (e.ColumnIndex != (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                HidePopupListView();
                isKeyboardInput = false;
            }
            if (e.RowIndex > -1 && e.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES && (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value && GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value.ToString() == "Patient got Discharged")
            {
                FormDischargePatient FormDischargePatient = new FormDischargePatient
                {
                    PatientId = PatientId,
                    PatientIpId = (long)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPID].Value
                };
                FormDischargePatient.ShowDialog();
            }
            if (e.RowIndex > -1 && !GridViewNote.ReadOnly)
            {
                bool GridViewLock = false;
                if ((string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value == "IP")
                {
                    if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value == null || !(bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value)
                    {
                        GridViewLock = true;
                    }
                }
                else if ((string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value == "OP")
                {
                    if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value == null || !(bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value)
                    {
                        GridViewLock = true;
                    }
                }
                else
                {
                    GridViewLock = true;
                }
                if (GridViewLock)
                {
                    int Index = e.RowIndex;
                    ConsultationNote Note = null!;
                    if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value != null)
                    {
                        Note = ConsultationNoteManager.Instance.GetConsultationNoteById(long.Parse(GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value.ToString()!));
                    }
                    if (e.ColumnIndex == (int)ConsultationNotesGridColumn.REMOVE && (GridViewNote.Rows.Count - 1) != e.RowIndex)
                    {
                        if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value != null)
                        {
                            Note = ConsultationNoteManager.Instance.GetConsultationNoteById(long.Parse(GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value.ToString()!));
                            if (Note != null && Note.IsInvoiced)
                            {
                                MessageBox.Show(DoNotAllowToDeleteInvoicedConsultationMsg);
                                return;
                            }
                            if (Note != null && Note.ConsultantId != Global.User.UserId)
                            {
                                MessageBox.Show(DoNotAllowToDeleteOtherConsultantConsultationMsg);
                                return;
                            }
                            if (Note != null && Note.ConsultantId == Global.User.UserId && Note.IsPrescriptionDispatchedForMedical)
                            {
                                MessageBox.Show(DoNotAllowToDeleteDispatchedToMedicalMsg);
                                return;
                            }
                            if (Note != null)
                            {
                                InPatientAdmission InPatientAdmission = null!;
                                if (Note.InPatientAdmissionId != null)
                                {
                                    InPatientAdmission = IpManager.Instance.GetInPatientAdmissionById((long)Note.InPatientAdmissionId);
                                }
                                else if (Note.OpRegistrationId != null)
                                {
                                    InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByOpId((long)Note.OpRegistrationId);
                                }

                                if (InPatientAdmission != null && InPatientAdmission.Status == InPatientStatus.DISCHARGED)
                                {
                                    MessageBox.Show(DoNotAllowToDeleteDischargeConsultationMsg);
                                    return;
                                }

                            }
                        }
                        DialogResult Result = MessageBox.Show("Do you want to delete  " + GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.DATE].Value.ToString() + " Consultation Notes?", "Delete Confirm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (Result == DialogResult.Yes)
                        {
                            if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value != null)
                            {
                                RemovePrescriptionandLabtestByNote((long)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value);
                            }
                            GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                            GridViewNote.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                    if (Note == null || (Note != null && !Note.IsPrescriptionDispatchedForMedical))
                    {
                        if (Note == null || Note != null && !Note.IsInvoiced)
                        {
                            if (e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDSYMPTOM)
                            {
                                Index = e.RowIndex;
                                FormSelectSymptom formSelectSymptom = new FormSelectSymptom(this);
                                formSelectSymptom.SelectedSymptomsids = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value;
                                formSelectSymptom.SelectedSymptomsNames = GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value != null ? GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value.ToString()! : string.Empty;
                                formSelectSymptom.SelectedSymptomsDiscp = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value;
                                formSelectSymptom.isDirty = this.formIsDirty;
                                formSelectSymptom.ShowDialog();

                                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                                && (!string.IsNullOrEmpty(formSelectSymptom.SelectedSymptomsNames)))
                                {
                                    GridViewNote.Rows.Add();
                                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                }
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value = formSelectSymptom.SelectedSymptomsNames;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value = formSelectSymptom.SelectedSymptomsids;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value = formSelectSymptom.SelectedSymptomsDiscp;
                                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDPRESCRIPTION, Index];
                                if (formSelectSymptom.Oldids.Count == 0 && formSelectSymptom.Newids.Count == 0)
                                {
                                    this.formIsDirty = formSelectSymptom.isDirty;
                                }
                                else if (formSelectSymptom.Oldids.Count == formSelectSymptom.Newids.Count)
                                {
                                    List<long> test = new List<long>();
                                    test.AddRange(formSelectSymptom.Oldids);
                                    test.AddRange(formSelectSymptom.Newids);
                                    if (test.Distinct().ToList().Count == formSelectSymptom.Newids.Count)
                                    {
                                        this.formIsDirty = formSelectSymptom.isDirty;
                                    }
                                    else
                                    {
                                        this.formIsDirty = true;
                                    }

                                }
                                else
                                {
                                    this.formIsDirty = true;
                                }

                                return;
                            }
                            else if (e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDPRESCRIPTION)
                            {
                                Index = e.RowIndex;
                                FormSelectPrescriptions formSelectPrescriptions = new FormSelectPrescriptions(this);
                                formSelectPrescriptions.SelectedPrescriptionIds = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value;
                                formSelectPrescriptions.SelectedPrescriptionsName = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONNAME].Value;
                                formSelectPrescriptions.SelectedPrescriptionsDosage = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.TOTAL].Value;
                                formSelectPrescriptions.SelectedPrescriptionsDosageDays = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value;
                                formSelectPrescriptions.SelectedPrescriptionsInterval = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value;
                                formSelectPrescriptions.SelectedPrescriptionsBeforeAfter = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value;
                                formSelectPrescriptions.SelectedPrescriptionsMorning = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.MORNING].Value;
                                formSelectPrescriptions.SelectedPrescriptionsAfterNoon = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value;
                                formSelectPrescriptions.SelectedPrescriptionsEvening = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.EVENING].Value;
                                formSelectPrescriptions.SelectedPrescriptionsNight = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.NIGHT].Value;
                                formSelectPrescriptions.SelectedPrescriptionsNotes = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value;
                                formSelectPrescriptions.SelectedPrescriptionsNames = GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value != null ? GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value.ToString()! : string.Empty;
                                formSelectPrescriptions.isDirty = this.formIsDirty;
                                formSelectPrescriptions.ShowDialog();

                                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                                && (!string.IsNullOrEmpty(formSelectPrescriptions.SelectedPrescriptionsNames)))
                                {
                                    GridViewNote.Rows.Add();
                                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                }
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value = formSelectPrescriptions.SelectedPrescriptionIds;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONNAME].Value = formSelectPrescriptions.SelectedPrescriptionsName;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value = formSelectPrescriptions.SelectedPrescriptionsNames;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.TOTAL].Value = formSelectPrescriptions.SelectedPrescriptionsDosage;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value = formSelectPrescriptions.SelectedPrescriptionsDosageDays;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value = formSelectPrescriptions.SelectedPrescriptionsInterval;

                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.MORNING].Value = formSelectPrescriptions.SelectedPrescriptionsMorning;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value = formSelectPrescriptions.SelectedPrescriptionsAfterNoon;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.EVENING].Value = formSelectPrescriptions.SelectedPrescriptionsEvening;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.NIGHT].Value = formSelectPrescriptions.SelectedPrescriptionsNight;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value = formSelectPrescriptions.SelectedPrescriptionsBeforeAfter;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value = formSelectPrescriptions.SelectedPrescriptionIds;

                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value = formSelectPrescriptions.SelectedPrescriptionsNotes;
                                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDLABTEST, Index];
                                if (formSelectPrescriptions.Oldids.Count == 0 && formSelectPrescriptions.Newids.Count == 0)
                                {
                                    this.formIsDirty = formSelectPrescriptions.isDirty;
                                }
                                else if (formSelectPrescriptions.Oldids.Count == formSelectPrescriptions.Newids.Count)
                                {
                                    List<long> test = new List<long>();
                                    test.AddRange(formSelectPrescriptions.Oldids);
                                    test.AddRange(formSelectPrescriptions.Newids);
                                    if (test.Distinct().ToList().Count == formSelectPrescriptions.Newids.Count)
                                    {
                                        this.formIsDirty = formSelectPrescriptions.isDirty;
                                    }
                                    else
                                    {
                                        this.formIsDirty = true;
                                    }

                                }
                                else
                                {
                                    this.formIsDirty = true;
                                }
                                return;
                            }
                            else if (e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDLABTEST)
                            {
                                Index = e.RowIndex;
                                FormSelectLabTest FormSelectLabTest = new FormSelectLabTest(this);
                                FormSelectLabTest.SelectedLabTestsids = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value;
                                FormSelectLabTest.SelectedLabTestsDisc = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value;
                                FormSelectLabTest.SelectedLabTestsFees = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value;
                                FormSelectLabTest.SelectedLabTestsElementids = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value;
                                FormSelectLabTest.SelectedLabTestsNames = GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value != null ? GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value.ToString()! : string.Empty;
                                FormSelectLabTest.isDirty = this.formIsDirty;
                                FormSelectLabTest.ShowDialog();

                                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                                && (!string.IsNullOrEmpty(FormSelectLabTest.SelectedLabTestsNames)))
                                {
                                    GridViewNote.Rows.Add();
                                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                }
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value = FormSelectLabTest.SelectedLabTestsids;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value = FormSelectLabTest.SelectedLabTestsDisc;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value = FormSelectLabTest.SelectedLabTestsFees;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value = FormSelectLabTest.SelectedLabTestsElementids;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value = FormSelectLabTest.SelectedLabTestsNames;
                                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDPROCEDURE, Index];
                                if (FormSelectLabTest.Oldids.Count == 0 && FormSelectLabTest.Newids.Count == 0)
                                {
                                    this.formIsDirty = FormSelectLabTest.isDirty;
                                }
                                else if (FormSelectLabTest.Oldids.Count == FormSelectLabTest.Newids.Count)
                                {
                                    List<long> test = new List<long>();
                                    test.AddRange(FormSelectLabTest.Oldids);
                                    test.AddRange(FormSelectLabTest.Newids);
                                    if (test.Distinct().ToList().Count == FormSelectLabTest.Newids.Count)
                                    {
                                        this.formIsDirty = FormSelectLabTest.isDirty;
                                    }
                                    else
                                    {
                                        this.formIsDirty = true;
                                    }

                                }
                                else
                                {
                                    this.formIsDirty = true;
                                }
                                return;
                            }
                            else if (e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDCONSULTATION)
                            {
                                Index = e.RowIndex;
                                FormSelectConsultation FormSelectConsultation = new FormSelectConsultation(this);
                                FormSelectConsultation.SelectedConsultationsids = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value;
                                FormSelectConsultation.SelectedConsultationsFees = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value;
                                FormSelectConsultation.SelectedConsultationsDiscrp = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value;
                                FormSelectConsultation.SelectedConsultationsNames = GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value != null ? GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value.ToString()! : string.Empty;
                                FormSelectConsultation.PatientOpId = PatientOpId;
                                FormSelectConsultation.isDirty = this.formIsDirty;
                                FormSelectConsultation.ShowDialog();

                                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                                && (!string.IsNullOrEmpty(FormSelectConsultation.SelectedConsultationsNames)))
                                {
                                    GridViewNote.Rows.Add();
                                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                }
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value = FormSelectConsultation.SelectedConsultationsids;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value = FormSelectConsultation.SelectedConsultationsNames;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value = FormSelectConsultation.SelectedConsultationsFees;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.FEE].Value = TotalFee(FormSelectConsultation.SelectedConsultationsFees);
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value = FormSelectConsultation.SelectedConsultationsDiscrp;
                                if (GridViewNote.CurrentCell!.RowIndex == GridViewNote.Rows.Count - 1 && GridViewNote.CurrentCell.RowIndex != Index)
                                {
                                    GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, Index + 1];
                                }
                                if (FormSelectConsultation.Oldids.Count == 0 && FormSelectConsultation.Newids.Count == 0)
                                {
                                    this.formIsDirty = FormSelectConsultation.isDirty;
                                }
                                else if (FormSelectConsultation.Oldids.Count == FormSelectConsultation.Newids.Count)
                                {
                                    List<long> test = new List<long>();
                                    test.AddRange(FormSelectConsultation.Oldids);
                                    test.AddRange(FormSelectConsultation.Newids);
                                    if (test.Distinct().ToList().Count == FormSelectConsultation.Newids.Count)
                                    {
                                        this.formIsDirty = FormSelectConsultation.isDirty;
                                    }
                                    else
                                    {
                                        this.formIsDirty = true;
                                    }

                                }
                                else
                                {
                                    this.formIsDirty = true;
                                }
                                return;
                            }
                            else if (e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDPROCEDURE)
                            {
                                Index = e.RowIndex;
                                FormSelectProcedures FormSelectProcedures = new FormSelectProcedures(this);
                                FormSelectProcedures.SelectedProcedureIds = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value;
                                FormSelectProcedures.SelectedProcedureDisc = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value;
                                FormSelectProcedures.SelectedProcedureFees = (DataGridViewComboBoxCell)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value;
                                FormSelectProcedures.SelectedProcedureNames = GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value != null ? GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value.ToString()! : string.Empty;
                                FormSelectProcedures.isDirty = this.formIsDirty;
                                FormSelectProcedures.ShowDialog();

                                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                                && (!string.IsNullOrEmpty(FormSelectProcedures.SelectedProcedureNames)))
                                {
                                    GridViewNote.Rows.Add();
                                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                }
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value = FormSelectProcedures.SelectedProcedureIds;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value = FormSelectProcedures.SelectedProcedureNames;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value = FormSelectProcedures.SelectedProcedureDisc;
                                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value = FormSelectProcedures.SelectedProcedureFees;
                                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDCONSULTATION, Index];
                                if (FormSelectProcedures.Oldids.Count == 0 && FormSelectProcedures.Newids.Count == 0)
                                {
                                    this.formIsDirty = FormSelectProcedures.isDirty;
                                }
                                else if (FormSelectProcedures.Oldids.Count == FormSelectProcedures.Newids.Count)
                                {
                                    List<long> test = new List<long>();
                                    test.AddRange(FormSelectProcedures.Oldids);
                                    test.AddRange(FormSelectProcedures.Newids);
                                    if (test.Distinct().ToList().Count == FormSelectProcedures.Newids.Count)
                                    {
                                        this.formIsDirty = FormSelectProcedures.isDirty;
                                    }
                                    else
                                    {
                                        this.formIsDirty = true;
                                    }

                                }
                                else
                                {
                                    this.formIsDirty = true;
                                }
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(DoNotAllowToEditInvoiced);
                        }
                    }
                    else
                    {
                        MessageBox.Show(DoNotAllowToEditDispatchedToMedicalMsg);
                    }
                }
            }
        }
        private double TotalFee(DataGridViewComboBoxCell SelectedConsultationsFees)
        {
            double Fee = 0.00;
            if (SelectedConsultationsFees != null && SelectedConsultationsFees.Items.Count > 0)
            {
                foreach (var fee in SelectedConsultationsFees.Items)
                {
                    Fee = Fee + double.Parse(fee.ToString()!);
                }
            }
            return Fee;
        }

        private bool ValidateNote()
        {
            ConsultedNoteErrMsg.Text = string.Empty;
            if (GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, 0].Value == null || string.IsNullOrEmpty(GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, 0].Value.ToString()))
            {
                GridViewNote.BeginInvoke(new MethodInvoker(delegate ()
                {
                    TabControlConsult.SelectedTab = ConsultNotesTab;
                    GridViewNote.Focus();
                    GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, 0];
                    GridViewNote.BeginEdit(true);
                    ConsultedNoteErrMsg.Text = EnterConsultationNoteErrorMsg;
                }));
                return false;
            }
            for (int i = 0; i < GridViewNote.Rows.Count - 1; i++)
            {
                if (GridViewNote.Rows[i].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value == null
                    || string.IsNullOrEmpty(GridViewNote.Rows[i].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value.ToString()))
                {
                    GridViewNote.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        TabControlConsult.SelectedTab = ConsultNotesTab;
                        GridViewNote.Focus();
                        GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, i];
                        GridViewNote.BeginEdit(true);
                        ConsultedNoteErrMsg.Text = EnterConsultationNoteErrorMsg;
                    }));
                    return false;
                }
            }
            return true;
        }
        private void SaveNote(object sender, EventArgs e, bool IsConsult)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            if (PatientOpId == null)
            {
                Registration Registration = OpManager.Instance.GetPatientInOpQueue(PatientId, Global.getTransactionDate());
                if (Registration != null)
                {
                    MessageBox.Show("Patient " + Registration.Patient.Name + " doesn't complete OP");
                    return;
                }
            }
            ConsultedNoteErrMsg.Text = "";
            SaveConsultationNotes(IsConsult, false);
            //PrescriptionSave(sender, e, false);
            //LabTestImgSave(sender, e, false);
            //SaveProcedureInfo(sender, e, false);
            //SaveVitals(sender, e, false);
            //MedicalHistorySave(sender, e, false);

            LoadNotesbyFilter();
            loadPrescriptionHistory();
            loadLabTestHistory();
            LoadIpOpVisit();
            LoadProcedureHistory();
            //LoadPatientChart();
            if (PatientOpId != null)
            {
                BtnCompleteConsultation.Enabled = true;
            }
            System.Windows.Forms.Cursor.Current = Cursors.Default;
            RecordEnter = true;
            this.formIsDirty = false;
            isConsulted = DialogResult.OK;
        }
        private void SaveConsultationNotes(bool IsConsult, bool isFromCheckboxEvent)
        {
            Cursor.Current = Cursors.WaitCursor;
            //try
            //{
            int i = 0;
            IList<ConsultationNote> lConsultationNote = new List<ConsultationNote>();
            foreach (DataGridViewRow row in GridViewNote.Rows)
            {
                if (i < (GridViewNote.Rows.Count - 1) &&
                    (row.Cells[(int)ConsultationNotesGridColumn.IPOP].Value == null || (row.Cells[(int)ConsultationNotesGridColumn.IPOP].Value.ToString() == "IP" && row.Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value != null &&
                    !((bool)row.Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value)) ||
                    (row.Cells[(int)ConsultationNotesGridColumn.IPOP].Value.ToString() == "OP" && row.Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value != null &&
                    !((bool)row.Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value))))
                {
                    ConsultationNote ConsultationNote = new ConsultationNote();
                    ConsultationNote.IsInvoiced = row.Cells[(int)ConsultationNotesGridColumn.INVOICED].Value != null ? (bool)row.Cells[(int)ConsultationNotesGridColumn.INVOICED].Value : false;
                    ConsultationNote.Id = row.Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value == null ? 0L : long.Parse(row.Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value.ToString()!);
                    ConsultationNote.CompanyId = Global.Company.CompanyId;
                    ConsultationNote.Date = ((DateTime)row.Cells[(int)ConsultationNotesGridColumn.DATE].Value).Date.Add(DateTime.Now.TimeOfDay);
                    long n = 0;
                    //if (long.TryParse(row.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value.ToString(), out n))
                    //{
                    //    ConsultationNote note = ConsultationNoteManager.Instance.GetConsultationNoteById((long.Parse(row.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value.ToString()!)));
                    //    ConsultationNote.Note = note.Note.ToString();
                    //}
                    //else
                    //{
                    ConsultationNote.Note = row.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value.ToString();
                    //}
                    ConsultationNote.PatientId = PatientId;
                    ConsultationNote.ConsultantId = Global.User.UserId;
                    ConsultationNote.IsPrescriptionDone = false;
                    ConsultationNote.IsPrescriptionDispatchedForMedical = false;
                    ConsultationNote.InPatientAdmissionId = (row.Cells[(int)ConsultationNotesGridColumn.IPID].Value == null && ConsultationNote.Id == 0L) ? PatientIPId : row.Cells[(int)ConsultationNotesGridColumn.IPID].Value != null ? long.Parse(row.Cells[(int)ConsultationNotesGridColumn.IPID].Value.ToString()!) : (long?)row.Cells[(int)ConsultationNotesGridColumn.IPID].Value;
                    ConsultationNote.OpRegistrationId = (row.Cells[(int)ConsultationNotesGridColumn.OPID].Value == null && ConsultationNote.Id == 0L) ? PatientOpId : row.Cells[(int)ConsultationNotesGridColumn.OPID].Value != null ? long.Parse(row.Cells[(int)ConsultationNotesGridColumn.OPID].Value.ToString()!) : (long?)row.Cells[(int)ConsultationNotesGridColumn.OPID].Value;
                    ConsultationNote.Fees = double.Parse(row.Cells[(int)ConsultationNotesGridColumn.FEE].Value.ToString()!);

                    //Symptoms
                    DataGridViewComboBoxCell SymptomsIds = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value;
                    DataGridViewComboBoxCell SymptomDisc = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value;

                    ConsultationNote.ConsultedSymptom = new List<ConsultedSymptom>();
                    if (SymptomsIds != null)
                    {
                        int cs = 0;
                        foreach (var Symp in SymptomsIds.Items)
                        {
                            ConsultedSymptom ConsultedSymptom = new ConsultedSymptom();
                            ConsultedSymptom.SymptomId = long.Parse(Symp.ToString()!);
                            ConsultedSymptom.CompanyId = Global.Company.CompanyId;
                            ConsultedSymptom.Description = SymptomDisc.Items[cs].ToString();
                            ConsultationNote.ConsultedSymptom.Add(ConsultedSymptom);
                            cs++;
                        }
                    }
                    //Prescription
                    DataGridViewComboBoxCell PrescriptionIds = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value;
                    string PrescriptionsNames = row.Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value != null ? row.Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value.ToString()! : string.Empty;
                    DataGridViewComboBoxCell PrescriptionsDosage = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.TOTAL].Value;
                    DataGridViewComboBoxCell PrescriptionsDosageDays = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value;
                    DataGridViewComboBoxCell PrescriptionsBeforeAfter = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value;// (TakeDosage)Enum.Parse(typeof(TakeDosage), (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value.ToString(), true);//
                    DataGridViewComboBoxCell PrescriptionsInterval = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value;
                    DataGridViewComboBoxCell PrescriptionsMorning = (DataGridViewComboBoxCell)(row.Cells[(int)ConsultationNotesGridColumn.MORNING].Value); //!= null ? row.Cells[(int)ConsultationNotesGridColumn.MORNING].Value : false);
                    DataGridViewComboBoxCell PrescriptionsAfterNoon = (DataGridViewComboBoxCell)(row.Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value); //!= null ? row.Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value : false);
                    DataGridViewComboBoxCell PrescriptionsEvening = (DataGridViewComboBoxCell)(row.Cells[(int)ConsultationNotesGridColumn.EVENING].Value); // != null ? row.Cells[(int)ConsultationNotesGridColumn.EVENING].Value : false);
                    DataGridViewComboBoxCell PrescriptionsNight = (DataGridViewComboBoxCell)(row.Cells[(int)ConsultationNotesGridColumn.NIGHT].Value); //  != null ? row.Cells[(int)ConsultationNotesGridColumn.NIGHT].Value : false);
                    DataGridViewComboBoxCell PrescriptionsNotes = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value;
                    ConsultationNote.ConsultedPrescription = new List<ConsultedPrescription>();
                    if (PrescriptionIds != null)
                    {
                        string Refno = null!;
                        if (ConsultationNote.Id == 0)
                        {
                            Refno = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PRESCRIPTION, ConsultationNote.Date);
                        }
                        else
                        {
                            ConsultedPrescription ConsultedPrescription = ConsultationNoteManager.Instance.GetConPrescriptionByConsultationNoteId(ConsultationNote.Id);
                            if (ConsultedPrescription != null)
                            {
                                Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById((long)ConsultedPrescription.PrescriptionId!);
                                if (PrescriptionFromDB.PrescriptionNumber == null || string.IsNullOrEmpty(PrescriptionFromDB.PrescriptionNumber))
                                {
                                    Refno = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PRESCRIPTION, ConsultationNote.Date);
                                }
                                else
                                {
                                    Refno = PrescriptionFromDB.PrescriptionNumber;
                                }
                            }
                            else if (PrescriptionIds != null)
                            {
                                Refno = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PRESCRIPTION, ConsultationNote.Date);
                            }
                        }
                        int cp = 0;
                        foreach (var Prod in PrescriptionIds.Items)
                        {
                            Prescription Prescription = new Prescription();
                            Prescription.ProductId = Prod.ToString().IsNumeric() ? long.Parse(Prod.ToString()!) : null;
                            Prescription.IsCustomProduct = Prescription.ProductId == null ? true : false;
                            Prescription.CustomProduct = Prescription.ProductId == null ? Prod.ToString() : string.Empty;
                            Prescription.PrescriptionNumber = Refno;
                            if (PrescriptionsDosage.Items[cp].ToString() != null)
                            {
                                Prescription.Total = PrescriptionsDosage.Items[cp].ToString();
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value != null && PrescriptionsDosageDays.Items.Count > 0)
                            {
                                Prescription.Days = PrescriptionsDosageDays.Items[cp].ToString();
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value != null && PrescriptionsBeforeAfter.Items.Count > 0)
                            {
                                Prescription.TakeDosage = (TakeDosage)Enum.Parse(typeof(TakeDosage), PrescriptionsBeforeAfter.Items[cp].ToString()!, true);
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value != null && PrescriptionsInterval.Items.Count > 0)
                            {
                                Prescription.Hours = PrescriptionsInterval.Items[cp].ToString();
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.MORNING].Value != null && PrescriptionsMorning.Items.Count > 0)
                            {
                                Prescription.Morning = PrescriptionsMorning.Items[cp].ToString()!; // Convert.ToBoolean(row.Cells[(int)ConsultationNotesGridColumn.MORNING].Value);
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value != null && PrescriptionsAfterNoon.Items.Count > 0)
                            {
                                Prescription.Afternoon = PrescriptionsAfterNoon.Items[cp].ToString()!;
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.EVENING].Value != null && PrescriptionsEvening.Items.Count > 0)
                            {
                                Prescription.Evening = PrescriptionsEvening.Items[cp].ToString()!;
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.NIGHT].Value != null && PrescriptionsNight.Items.Count > 0)
                            {
                                Prescription.Night = PrescriptionsNight.Items[cp].ToString()!;
                            }
                            if (row.Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value != null && PrescriptionsNotes.Items.Count > 0)
                            {
                                Prescription.AdditionalNotes = PrescriptionsNotes.Items[cp].ToString();
                            }
                            ConsultedPrescription ConsultedPrescription = new ConsultedPrescription();
                            ConsultedPrescription.CompanyId = Global.Company.CompanyId;
                            ConsultedPrescription.Prescription = Prescription;
                            ConsultedPrescription.IsCustomPrescription = Prescription.IsCustomProduct;
                            ConsultedPrescription.CustomPrescription = Prescription.CustomProduct;
                            ConsultationNote.ConsultedPrescription.Add(ConsultedPrescription);
                            cp++;
                        }
                    }

                    //LabTest
                    DataGridViewComboBoxCell LabtestIds = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value;
                    DataGridViewComboBoxCell LabtestElementIds = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value;
                    DataGridViewComboBoxCell LabtestDisc = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value;
                    DataGridViewComboBoxCell LabtestFees = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value;
                    ConsultationNote.ConsultedLabTest = new List<ConsultedLabTest>();
                    if (LabtestIds != null)
                    {
                        int lt = 0;
                        ConsultationNote.ConsultedLabTest = new List<ConsultedLabTest>();
                        foreach (var Lab in LabtestIds.Items)
                        {
                            ConsultedLabTest ConsultedLabTest = new ConsultedLabTest();
                            ConsultedLabTest.CompanyId = Global.Company.CompanyId;
                            MedicalTest MedicalTest = MedicalTestManager.Instance.GetMedicalTestById(long.Parse(Lab.ToString()!));
                            if (MedicalTest != null)
                            {
                                ConsultedLabTest.MedicalTestId = MedicalTest.Id;
                                ConsultedLabTest.HasElement = MedicalTest.HasElement;
                                ConsultedLabTest.RequestedById = Global.User.UserId;
                                ConsultedLabTest.RequestedOn = (DateTime)row.Cells[(int)ConsultationNotesGridColumn.DATE].Value;
                                ;
                                if (LabtestDisc.Items[lt].ToString() != null)
                                {
                                    ConsultedLabTest.Description = LabtestDisc.Items[lt].ToString();
                                }
                                else
                                {
                                    ConsultedLabTest.Description = MedicalTest.Description;
                                }
                                ConsultedLabTest.Name = MedicalTest.Name;
                                if (MedicalTest.HasElement && MedicalTest.TestElements.Count > 0 && LabtestElementIds.Items.Count > 0)
                                {
                                    ConsultedLabTest.ConsultedLabTestElements = new List<ConsultedLabTestElements>();
                                    foreach (var Element in LabtestElementIds.Items)
                                    {
                                        MedicalTestElement MElement = MedicalTest.TestElements.FirstOrDefault(x => x.Id.ToString() == Element.ToString())!;
                                        if (MElement != null)
                                        {
                                            ConsultedLabTestElements ConsultedLabTestElements = new ConsultedLabTestElements();
                                            ConsultedLabTestElements.CompanyId = Global.Company.CompanyId;
                                            ConsultedLabTestElements.Name = MElement.Name;
                                            ConsultedLabTestElements.UomId = MElement.UomId;
                                            ConsultedLabTestElements.Uom = MElement.Uom;
                                            ConsultedLabTestElements.Class = MElement.Class;
                                            ConsultedLabTestElements.SubClass = MElement.SubClass;
                                            ConsultedLabTestElements.RangeFrom = MElement.RangeFrom;
                                            ConsultedLabTestElements.RangeTo = MElement.RangeTo;
                                            ConsultedLabTestElements.SingleValue = MElement.SingleValue;
                                            ConsultedLabTestElements.MedicalTestElementId = MElement.Id;
                                            ConsultedLabTest.ConsultedLabTestElements.Add(ConsultedLabTestElements);
                                        }
                                    }
                                }
                                ConsultationNote.ConsultedLabTest.Add(ConsultedLabTest);
                                lt++;
                            }
                        }
                    }

                    // MedicalProcedure
                    DataGridViewComboBoxCell ProcedureIds = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value;
                    DataGridViewComboBoxCell ProcedureDisc = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value;
                    DataGridViewComboBoxCell ProcedureFees = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value;
                    ConsultationNote.ConsultedProcedure = new List<ConsultedProcedure>();
                    if (ProcedureIds != null)
                    {
                        int ip = 0;
                        foreach (var Proced in ProcedureIds.Items)
                        {
                            MedicalProcedure MedicalProcedureDetl = new MedicalProcedure();
                            MedicalProcedureDetl.CompanyId = Global.Company.CompanyId;
                            MedicalProcedureDetl.Id = long.Parse(Proced.ToString()!);

                            MedicalProcedure MedicalProcedure = MedicalProcedureManager.Instance.GetMedicalProcedureById(long.Parse(Proced.ToString()!));
                            if (MedicalProcedure != null)
                            {
                                ConsultedProcedure ConsultedProcedure = new ConsultedProcedure();
                                ConsultedProcedure.CompanyId = Global.Company.CompanyId;
                                ConsultedProcedure.MedicalProcedureId = MedicalProcedure.Id;
                                ConsultedProcedure.Name = MedicalProcedure.Name;
                                if (ProcedureDisc.Items[ip].ToString() != null)
                                {
                                    ConsultedProcedure.Description = ProcedureDisc.Items[ip].ToString();
                                }
                                else
                                {
                                    ConsultedProcedure.Description = MedicalProcedure.Description;
                                }
                                ConsultedProcedure.Note = string.Empty;
                                ConsultedProcedure.ProStatus = ProcedureStatus.REQUESTED;
                                ConsultedProcedure.Date = (DateTime)row.Cells[(int)ConsultationNotesGridColumn.DATE].Value; // Global.getTransactionDate(); //DateTime.Now;                                
                                ConsultedProcedure.RequestedById = Global.User.UserId;
                                ConsultedProcedure.RequestedOn = Global.getTransactionDate(); //DateTime.Now;
                                ConsultedProcedure.Fees = double.Parse(ProcedureFees.Items[ip].ToString()!);
                                ConsultationNote.ConsultedProcedure.Add(ConsultedProcedure);
                                ip++;
                            }
                        }
                    }
                    DataGridViewComboBoxCell ConsultationIds = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value;
                    DataGridViewComboBoxCell ConsultationFees = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value;
                    DataGridViewComboBoxCell ConsultationDiscrp = (DataGridViewComboBoxCell)row.Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value;
                    ConsultationNote.ConsultedConsultationFee = new List<ConsultedConsultationFee>();
                    if (ConsultationIds != null && ConsultationFees != null)
                    {
                        //ConsultationNote.ConsultedConsultationFee = new List<ConsultedConsultationFee>();
                        int c = 0;
                        foreach (var Cons in ConsultationIds.Items)
                        {
                            Consultation Consultation = ConsultationManager.Instance.GetConsultationById(long.Parse(Cons.ToString()!));
                            if (Consultation != null)
                            {
                                ConsultedConsultationFee ConsultedConsultation = new ConsultedConsultationFee();
                                ConsultedConsultation.CompanyId = Global.Company.CompanyId;
                                ConsultedConsultation.ConsultationId = long.Parse(Cons.ToString()!);
                                if (ConsultationDiscrp.Items[c].ToString() != null)
                                {
                                    ConsultedConsultation.Description = ConsultationDiscrp.Items[c].ToString();
                                }
                                else
                                {
                                    ConsultedConsultation.Description = Consultation.Discription;
                                }
                                ConsultedConsultation.Name = Consultation.Name;
                                ConsultedConsultation.Fee = double.Parse(ConsultationFees.Items[c].ToString()!);
                                ConsultedConsultation.ConsultantId = Global.User.UserId;
                                ConsultedConsultation.Date = (DateTime)row.Cells[(int)ConsultationNotesGridColumn.DATE].Value; // Global.getTransactionDate(); //DateTime.Now;                                
                                if (GetLastUpdatedFee(ConsultedConsultation.ConsultationId) != double.Parse(ConsultationFees.Items[c].ToString()!))
                                {
                                    ConsultedDoctorConsultationFee DoctorConsultation = new ConsultedDoctorConsultationFee();
                                    DoctorConsultation.Fee = double.Parse(ConsultationFees.Items[c].ToString()!);
                                    DoctorConsultation.ConsultationId = long.Parse(Cons.ToString()!);
                                    DoctorConsultation.ConsultantId = Global.User.UserId;
                                    DoctorConsultation.CompanyId = Global.Company.CompanyId;
                                    DoctorConsultationManager.Instance.AddDoctorConsultation(DoctorConsultation);
                                    ConsultedConsultation.IsOverrideFee = true;
                                }
                                ConsultationNote.ConsultedConsultationFee.Add(ConsultedConsultation);
                                c++;
                            }
                        }
                    }
                    lConsultationNote.Add(ConsultationNote);
                }
                i++;
            }
            if (CheckBoxLoadAllNotes.Checked)
            {
                ConsultationNoteManager.Instance.ManageConsultationNote(lConsultationNote, PatientId, Global.getTransactionDate(), false, IsConsult, PatientOpId, isFromCheckboxEvent);
            }
            else
            {
                ConsultationNoteManager.Instance.ManageConsultationNote(lConsultationNote, PatientId, Global.getTransactionDate(), true, IsConsult, PatientOpId, isFromCheckboxEvent);
            }
            if (PatientOpId == null)
            {
                Registration Registration = OpManager.Instance.GetConsultedOpByPatientId(PatientId, Global.getTransactionDate());
                if (Registration != null)
                {
                    PatientOpId = Registration.Id;
                }
            }
            Cursor.Current = Cursors.Default;
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("" + e);
            //}
            ConsultedNoteErrMsg.Text = SaveSuccessMsg;
        }
        private void loadPrescriptionHistory()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (CheckBoxLoadAllNotes.Checked)
            {
                GridViewPrescriptionHistory.LoadAllPrescription = true;

            }
            else
            {
                GridViewPrescriptionHistory.LoadAllPrescription = false;
            }
            GridViewPrescriptionHistory.PatientId = PatientId;
            PrescriptionHistoryRowCount = GridViewPrescriptionHistory.GridRows;
            Cursor.Current = Cursors.Default;
        }
        private void loadLabTestHistory()
        {
            Cursor.Current = Cursors.WaitCursor;
            if (CheckBoxLoadAllNotes.Checked)
            {
                GridViewLabTestHistory.LoadAllLabTest = true;
            }
            else
            {
                GridViewLabTestHistory.LoadAllLabTest = false;
            }
            GridViewLabTestHistory.PatientId = PatientId;
            LabHistoryRowCount = GridViewLabTestHistory.GridRows;
            Cursor.Current = Cursors.Default;
        }

        private void LoadProcedureHistory()
        {
            Cursor.Current = Cursors.WaitCursor;
            GridViewProcedureInfo.Rows.Clear();
            IList<ConsultationNote> ConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientId((long)PatientId);
            if (ConsultationNote != null && ConsultationNote.Count > 0)
            {
                DateTime ProcedureDateTime = Global.getTransactionDate().AddMonths(-1);
                if (CheckBoxLoadAllNotes.Checked)
                {
                    ProcedureDateTime = Global.getTransactionDate().AddYears(-100);
                }
                int p = 0;
                foreach (ConsultationNote ConsultationNotes in ConsultationNote.Where(d => d.Date > ProcedureDateTime))
                {
                    IList<ConsultedProcedure> ConsultedProceduresDetl = ConsultationNoteManager.Instance.ListProcedureByNoteId(ConsultationNotes.Id);
                    if (ConsultedProceduresDetl != null && ConsultedProceduresDetl.Count > 0)
                    {
                        string ProcedureDetails = string.Empty;

                        foreach (ConsultedProcedure ConsltProcedure in ConsultedProceduresDetl)
                        {
                            GridViewProcedureInfo.Rows.Add(1);
                            IList<ConsultedProcedureHistory> ConsultedProcedureHistory = ConsultationNoteManager.Instance.ListConsultedProcedureHistoryById(ConsltProcedure.ConsultedProcedureId, Global.Company.CompanyId);
                            GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.SLNO].Value = p + 1;
                            GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.PDATE].Value = ConsltProcedure.Date;
                            GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.NAME].Value = ConsltProcedure.MedicalProcedure.Name;
                            GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.DISC].Value = ConsltProcedure.Description;
                            GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.FEES].Value = ConsltProcedure.Fees.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));
                            GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.PID].Value = ConsltProcedure.ConsultedProcedureId;
                            GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.REQBY].Value = ConsltProcedure.RequestedBy != null ? ConsltProcedure.RequestedBy.Name : "";
                            if (ConsultedProcedureHistory != null && ConsultedProcedureHistory.Count > 0)
                            {
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.PERFMBY].Value = ConsultedProcedureHistory.Last().PerformedBy != null ? ConsultedProcedureHistory.Last().PerformedBy.Name : "";
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.PERFMON].Value = ConsultedProcedureHistory.Last().PerformOn;
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.NOTE].Value = ConsultedProcedureHistory.Last().Note;
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.STAT].Value = ConsultedProcedureHistory.Last().ProStatus.ToString();
                            }
                            else
                            {
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.PERFMBY].Value = ConsltProcedure.PerformedBy != null ? ConsltProcedure.PerformedBy.Name : "";
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.PERFMON].Value = ConsltProcedure.PerformOn;
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.NOTE].Value = ConsltProcedure.Note;
                                GridViewProcedureInfo.Rows[p].Cells[(int)ProcedureInfoGridColumn.STAT].Value = ConsltProcedure.ProStatus.ToString();
                            }

                            p++;
                        }
                    }

                }
            }
            Cursor.Current = Cursors.Default;
        }
        private double GetLastUpdatedFee(long ConsId)
        {
            double Fee = 0.00;
            ConsultedDoctorConsultationFee DoctorConsultation = DoctorConsultationManager.Instance.GetLatestDoctorConsultationByConsultationsandConsultantId(ConsId, Global.User.UserId);
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

        private void ConsultationNotesGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (GridViewNote.CurrentCell.ColumnIndex == 2 && e.Control is System.Windows.Forms.TextBox textBox)
            {
                editingTextBox = textBox;
                editingTextBox.TextChanged -= TextBox_TextChanged!;
                editingTextBox.TextChanged += TextBox_TextChanged!;
                editingTextBox.KeyDown -= TextBox_KeyDown!;
                editingTextBox.KeyDown += TextBox_KeyDown!;
                editingTextBox.MouseDown -= TextBox_MouseDown!;
                editingTextBox.MouseDown += TextBox_MouseDown!;

                textBox.TextAlign = HorizontalAlignment.Left;

                isKeyboardInput = false;
            }
            e.CellStyle.BackColor = Color.White;
            e.CellStyle.ForeColor = Color.Black;
            e.CellStyle.SelectionBackColor = Color.White;
            e.CellStyle.SelectionForeColor = Color.Black;

        }

        private void GridViewNote_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                if (GridViewNote.CurrentCell.EditedFormattedValue != null)
                {
                    if (isRowAdding)
                    {
                        GridViewNote.CurrentCell.Value = GridViewNote.CurrentCell.EditedFormattedValue;
                    }
                }
            }
        }
        private void btnCancelNote_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("Do you really want to Cancel  " + " Consultation Notes?", "Cancel Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            CancelConsulting();
        }
        private void CancelConsulting()
        {
            HidePopupListView();
            LoadNotesbyFilter();
            loadPrescriptionHistory();
            loadLabTestHistory();
            ConsultedNoteErrMsg.Text = "";
            this.formIsDirty = false;
        }
        private void GridViewNote_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.DATE].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.REMOVE].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ADDSYMPTOM].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ADDPRESCRIPTION].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTEST].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ADDLABTEST].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ADDCONSULTATION].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.FEE].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ADDPROCEDURE].ReadOnly = true;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].ReadOnly = true;

            if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value != null)
            {
                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteById(long.Parse(GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value.ToString()!));
                if (Note != null && Note.ConsultantId == Global.User.UserId)
                {
                    GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].ReadOnly = false;
                }
            }
            if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value == null)
            {
                GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].ReadOnly = false;
            }
            if ((string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value == "IP")
            {
                GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].ReadOnly = (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value ? (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value : (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.INVOICED].Value;
            }
            else if ((string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value == "OP")
            {
                GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].ReadOnly = (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value ? (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value : (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.INVOICED].Value;
            }
        }
        private void GridViewNote_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == (int)ConsultationNotesGridColumn.DATE)
            {

            }
        }
        private void GridViewPrescriptionDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (e.ColumnIndex == (int)PrescriptionDetailsGridColumn.HOURS)
                {
                    if (GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && !string.IsNullOrWhiteSpace(GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                    {
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = true;
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = true;
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = true;
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = true;
                    }
                    else
                    {
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = false;
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = false;
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = false;
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = false;
                    }
                }

                if (e.ColumnIndex == (int)PrescriptionDetailsGridColumn.MORNING
                    || e.ColumnIndex == (int)PrescriptionDetailsGridColumn.AFTERNOON
                    || e.ColumnIndex == (int)PrescriptionDetailsGridColumn.EVENING
                    || e.ColumnIndex == (int)PrescriptionDetailsGridColumn.NIGHT)
                {
                    if (GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.HOURS].ReadOnly = false;
                    }
                    else
                    {
                        GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.HOURS].ReadOnly = false;
                    }
                }
                else
                {
                    GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.HOURS].ReadOnly = false;
                }
            }
        }
        private void GridViewPrescriptionHistory_Load(object sender, EventArgs e)
        {
            GridViewPrescriptionDetail.Rows.Clear();
            ConsultedNoteErrMsg.Text = "";
            BtnPrescriptionSendMedical.Enabled = false;
            BtnPrescriptionPrint.Enabled = false;
            BtnPrescriptionPreview.Enabled = false;
            if (GridViewPrescriptionDetail.ContainsFocus)
            {
                if (GridViewPrescriptionDetail.RowCount > 0)
                {
                    GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[0].Cells[0];
                }
            }
            else if (GridViewPrescriptionHistory.ContainsFocus)
            {
                if (GridViewPrescriptionHistory.NoteId != 0L)
                {


                    LoadPrescriptionDetails(GridViewPrescriptionHistory.NoteId, GridViewPrescriptionHistory.IsDischarge);
                    PrescriptionHistoryRowIndex = GridViewPrescriptionHistory.GridRowIndex;
                    PrescriptionHistoryIsLastRow = GridViewPrescriptionHistory.LastRow;
                    if (PrescriptionHistoryIsLastRow == true)
                    {
                        if (PrescriptionHistoryRowIndex == PrescriptionHistoryRowCount)
                        {
                            if (GridViewPrescriptionDetail.RowCount > 0)
                            {
                                GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[0].Cells[0];
                            }
                            else if (BtnPrescriptionSave.Enabled)
                            {
                                BtnPrescriptionSave.Select();
                            }
                            else
                            {
                                PatientLabTestSelected = true;
                                MoveToTab(2, LabTab);
                            }
                            GridViewPrescriptionHistory.LastRow = false;
                        }
                    }

                }
                else
                {
                    BtnPrescriptionSave.Enabled = false;
                    BtnPrescriptionCancel.Enabled = false;
                }
                EnableForm();
            }
            else if (GridViewPrescriptionHistory.RowSelection == false)
            {

                if (!PrescriptionHistoryDataSaved)
                {
                    GridViewPrescriptionDetail.Rows.Clear();
                }
                else if (PrescriptionHistoryDataSaved)
                {
                    GridViewPrescriptionHistory.RowSelection = true;
                    if (GridViewPrescriptionHistory.GridRows > 0)
                    {
                        int currentRowIndex = GridViewPrescriptionHistory.LastSelectedRowIndex;// GridViewPrescriptionHistory.GridRowIndex;

                        GridViewPrescriptionHistory.SelectDataGrid_CellClick(currentRowIndex);
                    }
                    PrescriptionHistoryDataSaved = false;
                }
            }
            else if (GridViewPrescriptionHistory.RowSelection == true)
            {
                GridViewPrescriptionHistory.FocusOnPrescriptionRow(GridViewPrescriptionHistory.LastSelectedRowIndex);
            }
            this.formIsDirty = false;
        }
        private void LoadPrescriptionDetails(long NoteId, bool Isdischarge)
        {
            GridViewPrescriptionDetail.Rows.Clear();
            BtnPrescriptionSave.Enabled = !Isdischarge;
            BtnPrescriptionCancel.Enabled = !Isdischarge;

            if (Isdischarge)
            {
                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteById(NoteId);
                IList<DischargePrescription> lDischargePrescription = DischargeNoteManager.Instance.ListDischargePrescriptionByPatientIpId((long)Note.InPatientAdmissionId!);
                if (lDischargePrescription != null && lDischargePrescription.Count > 0)
                {
                    if (Note != null)
                    {
                        BtnPrescriptionSendMedical.Enabled = Note.IsPrescriptionDispatchedForMedical ? false : Note.IsPrescriptionDone ? true : false;
                        BtnPrescriptionCancel.Enabled = BtnPrescriptionSave.Enabled = Note.IsPrescriptionDispatchedForMedical ? false : true;
                        BtnPrescriptionPrint.Enabled = BtnPrescriptionPreview.Enabled = Note.IsPrescriptionDone ? true : false;
                    }

                    int i = 0;
                    foreach (DischargePrescription DisPres in lDischargePrescription)
                    {
                        GridViewPrescriptionDetail.Rows.Add();
                        Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById(DisPres.PrescriptionId);
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NAME].Value = (bool)PrescriptionFromDB.IsCustomProduct! ? PrescriptionFromDB.CustomProduct : PrescriptionFromDB.Product.Name;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.TOTAL].Value = PrescriptionFromDB.Total;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.DAYS].Value = PrescriptionFromDB.Days;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.HOURS].Value = PrescriptionFromDB.Hours;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.MORNING].Value = PrescriptionFromDB.Morning;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].Value = PrescriptionFromDB.Afternoon;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.EVENING].Value = PrescriptionFromDB.Evening;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].Value = PrescriptionFromDB.Night;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER].Value = PrescriptionFromDB.TakeDosage;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.PID].Value = PrescriptionFromDB.Id;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.ADDNOTES].Value = PrescriptionFromDB.AdditionalNotes;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NAME].ReadOnly = true;
                        if (!string.IsNullOrWhiteSpace(PrescriptionFromDB.Hours))
                        {
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = true;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = true;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = true;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = true;
                        }
                        else
                        {
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = false;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = false;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = false;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = false;
                        }
                        i++;
                    }
                }
            }
            else
            {
                IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(NoteId);
                if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                {
                    ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteById(NoteId);
                    if (Note != null)
                    {
                        BtnPrescriptionSendMedical.Enabled = Note.IsPrescriptionDispatchedForMedical ? false : Note.IsPrescriptionDone ? true : false;
                        BtnPrescriptionCancel.Enabled = BtnPrescriptionSave.Enabled = Note.IsPrescriptionDispatchedForMedical ? false : true;
                        BtnPrescriptionPrint.Enabled = BtnPrescriptionPreview.Enabled = Note.IsPrescriptionDone ? true : false;
                    }

                    int i = 0;
                    foreach (ConsultedPrescription ConsPres in lConsultedPrescription)
                    {
                        GridViewPrescriptionDetail.Rows.Add();
                        Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById((long)ConsPres.PrescriptionId!);
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NAME].Value = PrescriptionFromDB.Product == null ? PrescriptionFromDB.CustomProduct : PrescriptionFromDB.Product.Name;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.TOTAL].Value = PrescriptionFromDB.Total;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.DAYS].Value = PrescriptionFromDB.Days;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.HOURS].Value = PrescriptionFromDB.Hours;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.MORNING].Value = PrescriptionFromDB.Morning;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].Value = PrescriptionFromDB.Afternoon;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.EVENING].Value = PrescriptionFromDB.Evening;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].Value = PrescriptionFromDB.Night;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER].Value = PrescriptionFromDB.TakeDosage;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.ADDNOTES].Value = PrescriptionFromDB.AdditionalNotes;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.PID].Value = PrescriptionFromDB.Id;
                        GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NAME].ReadOnly = true;
                        if (!string.IsNullOrWhiteSpace(PrescriptionFromDB.Hours))
                        {
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = true;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = true;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = true;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = true;
                        }
                        else
                        {
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = false;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = false;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = false;
                            GridViewPrescriptionDetail.Rows[i].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = false;
                        }
                        if (Note?.IsPrescriptionDispatchedForMedical == true)
                        {
                            GridViewPrescriptionDetail.Rows[i].ReadOnly = true;
                        }
                        i++;
                    }
                }
            }

        }

        private void BtnPrescriptionSave_Click(object sender, EventArgs e)
        {
            int lastSelectedRowIndex = GridViewPrescriptionHistory.LastSelectedRowIndex;

            //PrescriptionSave(sender, e, true);
            //if (lastSelectedRowIndex >= 0 && lastSelectedRowIndex < GridViewPrescriptionHistory.GridRows)
            //{
            //    PrescriptionHistoryDataSaved = true;
            //    GridViewPrescriptionHistory.RowSelection = true;
            //    GridViewPrescriptionHistory.SelectedRowIndex = lastSelectedRowIndex;
            //   // int currentRowIndex = GridViewPrescriptionHistory.LastSelectedRowIndex;// GridViewPrescriptionHistory.GridRowIndex;

            //    GridViewPrescriptionHistory.SelectDataGrid_CellClick(lastSelectedRowIndex);
            //    //return;
            //}
            //// Capture the last selected row index

            // Call the method that saves the prescription data
            PrescriptionSave(sender, e, true);
            if (lastSelectedRowIndex >= 0 && lastSelectedRowIndex < GridViewPrescriptionHistory.GridRows)
            {
                PrescriptionHistoryDataSaved = true;
                GridViewPrescriptionHistory.RowSelection = true;
                GridViewPrescriptionHistory.SelectedRowIndex = lastSelectedRowIndex;
                GridViewPrescriptionHistory.SelectDataGrid_CellClick(lastSelectedRowIndex);
            }
        }
        private void PrescriptionSave(object sender, EventArgs e, bool loadChart)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            ConsultedNoteErrMsg.Text = "";
            if (GridViewPrescriptionDetail.Rows.Count > 0)
            {
                //if (ValidatePresDetail())
                //{
                IList<Prescription> lprescription = new List<Prescription>();
                Patient lPatientInfo = PatientManager.Instance.GetPatientById(PatientId);
                foreach (DataGridViewRow row in GridViewPrescriptionDetail.Rows)
                {
                    Prescription Prescription = new Prescription();
                    Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById(long.Parse(row.Cells[(int)PrescriptionDetailsGridColumn.PID].Value.ToString()!));
                    if (PrescriptionFromDB != null)
                    {
                        Prescription.CustomProduct = PrescriptionFromDB.CustomProduct;
                        Prescription.IsCustomProduct = PrescriptionFromDB.IsCustomProduct;
                        Prescription.Id = PrescriptionFromDB.Id;
                        Prescription.ProductId = PrescriptionFromDB.ProductId;
                        Prescription.Total = row.Cells[(int)PrescriptionDetailsGridColumn.TOTAL].Value != null ? row.Cells[(int)PrescriptionDetailsGridColumn.TOTAL].Value.ToString() : "";
                        Prescription.Days = row.Cells[(int)PrescriptionDetailsGridColumn.DAYS].Value != null ? row.Cells[(int)PrescriptionDetailsGridColumn.DAYS].Value.ToString()! : "";
                        Prescription.Morning = row.Cells[(int)PrescriptionDetailsGridColumn.MORNING].Value != null ? row.Cells[(int)PrescriptionDetailsGridColumn.MORNING].Value.ToString()! : "";
                        Prescription.Afternoon = row.Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].Value != null ? row.Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].Value.ToString()! : "";
                        Prescription.Evening = row.Cells[(int)PrescriptionDetailsGridColumn.EVENING].Value != null ? row.Cells[(int)PrescriptionDetailsGridColumn.EVENING].Value.ToString()! : "";
                        Prescription.Night = row.Cells[(int)PrescriptionDetailsGridColumn.NIGHT].Value != null ? row.Cells[(int)PrescriptionDetailsGridColumn.NIGHT].Value.ToString()! : "";
                        Prescription.Hours = row.Cells[(int)PrescriptionDetailsGridColumn.HOURS].Value == null ? string.Empty : string.IsNullOrWhiteSpace(row.Cells[(int)PrescriptionDetailsGridColumn.HOURS].Value.ToString()) ? string.Empty : row.Cells[(int)PrescriptionDetailsGridColumn.HOURS].Value.ToString();
                        Prescription.TakeDosage = (TakeDosage)Enum.Parse(typeof(TakeDosage), row.Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER].Value.ToString()!, true);
                        if (PrescriptionFromDB.PrescribedByDoctorId == null)
                        {
                            if (lPatientInfo != null)
                            {
                                if (lPatientInfo.GetStatus(Global.getTransactionDate()) == "In OP")
                                {
                                    Registration OpRegistration = OpManager.Instance.GetlastOPRecord((long)lPatientInfo.Id, lPatientInfo.CompanyId);
                                    Prescription.PrescribedByDoctorId = OpRegistration.RequestedDoctor != null ? OpRegistration.RequestedDoctor.Id : GridViewPrescriptionHistory.EmpId;
                                }
                                else if (lPatientInfo.GetStatus(Global.getTransactionDate()) == "In IP")
                                {
                                    InPatientAdmission InPatientAdmissions = IpManager.Instance.GetAdmittedInPatientAdmissionByPatientId((long)lPatientInfo.Id);
                                    long IpAdmitId = InPatientAdmissions.Id;
                                    MedicalTeam IpMedicalTeam = MedicalTeamManager.Instance.GetInPatientMedicalTeambyAdmissionId(IpAdmitId);
                                    Prescription.PrescribedByDoctorId = IpMedicalTeam.PrimaryDoctor != null ? IpMedicalTeam.PrimaryDoctor.Id : GridViewPrescriptionHistory.EmpId;
                                }
                                else
                                {
                                    Prescription.PrescribedByDoctorId = GridViewPrescriptionHistory.EmpId;
                                }
                            }
                            else
                            {
                                Prescription.PrescribedByDoctorId = GridViewPrescriptionHistory.EmpId;
                            }
                        }
                        else
                        {
                            Prescription.PrescribedByDoctorId = PrescriptionFromDB.PrescribedByDoctorId;
                        }
                        Prescription.AdditionalNotes = row.Cells[(int)PrescriptionDetailsGridColumn.ADDNOTES].Value == null ? string.Empty : row.Cells[(int)PrescriptionDetailsGridColumn.ADDNOTES].Value.ToString();
                        Prescription.PrescriptionNumber = PrescriptionFromDB.PrescriptionNumber;

                        lprescription.Add(Prescription);
                    }
                }
                ConsultationNoteManager.Instance.UpdatePrescription(lprescription, GridViewPrescriptionHistory.NoteId);
                //GridViewPrescriptionHistory_Load(sender, e);
                //if (loadChart)
                //{
                //loadPrescriptionHistory();
                //LoadPatientChart();
                //}
                //}
            }
            LoadConsultedPrescriptionbyFilter();
            ConsultedNoteErrMsg.Text = SaveSuccessMsg;
            this.formIsDirty = false;
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void LoadConsultedPrescriptionbyFilter()
        {
            int row = 0;
            IList<ConsultationNote> ListConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientId(PatientId);
            DateTime ConsltdateTime = Global.getTransactionDate().AddMonths(-1);
            if (CheckBoxLoadAllNotes.Checked)
            {
                ConsltdateTime = Global.getTransactionDate().AddYears(-100);
            }
            foreach (ConsultationNote ConsultationNote in ListConsultationNote.Where(d => d.Date > ConsltdateTime))
            {
                DataGridViewComboBoxCell SelectedPrescriptionsName = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedPrescriptionsids = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedPrescriptionsDosages = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedPrescriptionsDosageDay = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedPrescriptionsIntervals = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedPrescriptionsBeforeorAfter = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedMorningPrescriptions = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedAfterNoonPrescriptions = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedEveningPrescriptions = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedNightPrescriptions = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedPrescriptionsAdditonalNotes = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell SelectedMorn = new DataGridViewComboBoxCell();

                IList<ConsultedPrescription> ConsultedPrescriptions = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(ConsultationNote.Id);
                if (ConsultedPrescriptions.Count > 0)
                {

                    string PrescriptionsName = string.Empty;
                    foreach (ConsultedPrescription ConsultedPrescription in ConsultedPrescriptions)
                    {
                        Product Product = ConsultedPrescription.Prescription.ProductId != null ? CatalogProductManager.Instance.GetProductInfoById((long)ConsultedPrescription.Prescription.ProductId) : null!;
                        SelectedPrescriptionsName.Items.Add(Product != null ? Product.Name : ConsultedPrescription.CustomPrescription);
                        SelectedPrescriptionsids.Items.Add(Product != null ? Product.Id : ConsultedPrescription.CustomPrescription);
                        PrescriptionsName = string.IsNullOrEmpty(PrescriptionsName) ? (Product != null ? Product.Name : ConsultedPrescription.CustomPrescription) : PrescriptionsName + ",\n" + (Product != null ? Product.Name : ConsultedPrescription.CustomPrescription);
                        SelectedPrescriptionsDosages.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Total) ? "" : ConsultedPrescription.Prescription.Total);
                        SelectedPrescriptionsDosageDay.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Days.ToString()) ? "" : ConsultedPrescription.Prescription.Days.ToString());
                        SelectedPrescriptionsBeforeorAfter.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.TakeDosage.ToString()) ? "" : ConsultedPrescription.Prescription.TakeDosage.ToString());
                        SelectedPrescriptionsIntervals.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Hours) ? "" : ConsultedPrescription.Prescription.Hours);
                        SelectedMorn.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Morning) ? "" : ConsultedPrescription.Prescription.Morning);
                        SelectedMorningPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Morning) ? "" : ConsultedPrescription.Prescription.Morning);
                        SelectedAfterNoonPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Afternoon) ? "" : ConsultedPrescription.Prescription.Afternoon);
                        SelectedEveningPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Evening) ? "" : ConsultedPrescription.Prescription.Evening);
                        SelectedNightPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Night) ? "" : ConsultedPrescription.Prescription.Night);
                        SelectedPrescriptionsAdditonalNotes.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.AdditionalNotes) ? "" : ConsultedPrescription.Prescription.AdditionalNotes);
                    }
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value = SelectedPrescriptionsids;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value = PrescriptionsName;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONNAME].Value = SelectedPrescriptionsName;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.TOTAL].Value = SelectedPrescriptionsDosages;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value = SelectedPrescriptionsDosageDay;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value = SelectedPrescriptionsBeforeorAfter;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value = SelectedPrescriptionsIntervals;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.MORNING].Value = SelectedMorningPrescriptions;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value = SelectedAfterNoonPrescriptions;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.EVENING].Value = SelectedEveningPrescriptions;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.NIGHT].Value = SelectedNightPrescriptions;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value = SelectedPrescriptionsAdditonalNotes;

                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    row++;
                }
            }
        }

        private void GridViewPrescriptionDetail_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var Value = from Enum en in Enum.GetValues(typeof(TakeDosage)) select new { Id = en, Name = en.ToString().Replace("None", " ") };

            (GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.DataSource = null;
            (GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.DataSource = Value.ToList();
            (GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.ValueMember = "Id";
            (GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
            (GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.BEFOREAFTER] as DataGridViewComboBoxCell)!.AutoComplete = true;
            GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.MORNING].Value = "";
            GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].Value = "";
            GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.EVENING].Value = "";
            GridViewPrescriptionDetail.Rows[e.RowIndex].Cells[(int)PrescriptionDetailsGridColumn.NIGHT].Value = "";
        }

        private void GridViewLabTestHistory_Load(object sender, EventArgs e)
        {
            LabHistoryRowIndex = 0;
            BtnLabTestImgSave.Enabled = false;
            BtnLabTestImgCancel.Enabled = false;
            BtnConsultLabTestElementCancel.Enabled = false;
            BtnConsultLabTestElementSave.Enabled = false;
            BtnConsultLabTestPrintRequisition.Enabled = false;
            if(GridViewLabTestHistory.GridRows > 0)
            {
                BtnLabTestPrintRequisition.Enabled = true;
            }
            else
            {
                BtnLabTestPrintRequisition.Enabled = false;
            }
            if (GridViewLabTestHistory.LTestId != 0L)
            {
                BtnLabTestPrintRequisition.Enabled = true;
                LoadLabTestDetails(GridViewLabTestHistory.LTestId);
                LoadLabTestAttachment(GridViewLabTestHistory.LTestId);
                LabHistoryRowIndex = GridViewLabTestHistory.GridRowIndex;
                LabHistoryIsLastRow = GridViewLabTestHistory.LastRow;
                if (LabHistoryIsLastRow == true)
                {
                    if (LabHistoryRowIndex == LabHistoryRowCount)
                    {
                        if (BtnLabTestImgSave.Enabled)
                        {
                            BtnLabTestImgSave.Select();
                        }
                        else
                        {
                            MoveToTab(3, ProcedureTab);
                            ProcedureTab.Focus();

                            if (GridViewProcedureInfo.Rows.Count > 0)
                            {
                                GridViewProcedureInfo.CurrentCell = GridViewProcedureInfo.Rows[0].Cells[0]; // Set the CurrentCell to the first cell
                                GridViewProcedureInfo.Focus();
                                SelectEntireRow(0);
                                GridViewProcedureInfo.BeginEdit(true);
                            }
                        }
                        GridViewLabTestHistory.LastRow = false;
                    }
                }
            }
            EnableForm();
            this.formIsDirty = false;
        }
        // Your existing LoadLabTestDetails method with updates for variation display
        private void LoadLabTestDetails(long LabTestId)
        {
            GridViewLabTestElementInformation.Rows.Clear();
            ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(LabTestId);

            if (consultedLabTest != null)
            {
                IList<MedicalTestElement> lLabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(consultedLabTest.MedicalTestId);
                IList<ConsultedLabTestElements> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestElementsByLabtestId(LabTestId);

                if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
                {
                    Dictionary<string, DataGridViewRow> rowMap = new Dictionary<string, DataGridViewRow>();

                    foreach (ConsultedLabTestElements consElement in lConsultedLabTestElements)
                    {
                        string name = consElement.Name;

                        if (rowMap.ContainsKey(name))
                        {
                            var comboBoxCell = (DataGridViewComboBoxCell)rowMap[name].Cells[(int)MedicalLabTestElementsGridColumn.CLASS];
                            
                            if (!comboBoxCell.Items.Contains(consElement.Class))
                            {
                                comboBoxCell.Items.Add(consElement.Class);

                                comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                                comboBoxCell.FlatStyle = FlatStyle.Flat;
                            }

                            rowMap[name].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value = consElement.Class;
                        }
                        else
                        {
                            int newRowIdx = GridViewLabTestElementInformation.Rows.Add();
                            DataGridViewRow newRow = GridViewLabTestElementInformation.Rows[newRowIdx];

                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.SNO].Value = newRowIdx + 1;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.NAME].Value = name;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.UOM].Value = consElement.Uom != null ? consElement.Uom.Name.ToString() : "";
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value = consElement.Class;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value = consElement.ResultDescription;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value = consElement.SubClass;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Value = consElement.SingleValue;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value = consElement.RangeFrom;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value = consElement.RangeTo;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value = consElement.Id;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value = LabTestId;

                            DataGridViewComboBoxCell ClasscomboBoxCell = new DataGridViewComboBoxCell();
                            ClasscomboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                            ClasscomboBoxCell.FlatStyle = FlatStyle.Flat;

                            foreach (string Class in lLabTestElements.Select(x => x.Class).Distinct())
                            {
                                if (!string.IsNullOrEmpty(Class))
                                {
                                    ClasscomboBoxCell.Items.Add(Class);
                                }
                            }
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.CLASS] = ClasscomboBoxCell;

                            DataGridViewComboBoxCell SubClasscomboBoxCell = new DataGridViewComboBoxCell();
                            SubClasscomboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                            SubClasscomboBoxCell.FlatStyle = FlatStyle.Flat;

                            IList<MedicalTestElement> elements = lLabTestElements.Where(x => x.Class == consElement.Class).Distinct().Where(subClass => subClass != null).ToList();

                            foreach (MedicalTestElement element in elements)
                            {
                                if (!string.IsNullOrEmpty(element.SubClass))
                                {
                                    SubClasscomboBoxCell.Items.Add(element.SubClass);
                                }
                            }
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS] = SubClasscomboBoxCell;

                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value = consElement.Class;
                            newRow.Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value = consElement.SubClass;

                            rowMap[name] = newRow;
                        }
                    }

                    BtnConsultLabTestElementCancel.Enabled = true;
                    BtnConsultLabTestElementSave.Enabled = true;
                    BtnConsultLabTestPrintRequisition.Enabled = true;
                    BtnLabTestPrintRequisition.Enabled = true;
                }
            }
        }

        //private void LoadLabTestDetailsMadeChanges(long LabTestId)
        //{
        //    GridViewLabTestElementInformation.Rows.Clear();
        //    ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(LabTestId);

        //    if (consultedLabTest != null)
        //    {
        //        IList<MedicalTestElement> lLabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(consultedLabTest.MedicalTestId);
        //        IList<ConsultedLabTestElements> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestElementsByLabtestId(LabTestId);

        //        if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
        //        {
        //            // Dictionary to track rows by name and store variations
        //            Dictionary<string, DataGridViewRow> rowMap = new Dictionary<string, DataGridViewRow>();

        //            foreach (ConsultedLabTestElements consElement in lConsultedLabTestElements)
        //            {
        //                string name = consElement.Name;

        //                // Check if the name already exists in the row map
        //                if (rowMap.ContainsKey(name))
        //                {
        //                    // Reuse the existing row's ComboBox for variations
        //                    var comboBoxCell = (DataGridViewComboBoxCell)rowMap[name].Cells[(int)MedicalLabTestElementsGridColumn.VARIATION];

        //                    // Add variation only if it doesn't already exist in the ComboBox
        //                    if (!comboBoxCell.Items.Contains(consElement.Variation))
        //                    {
        //                        comboBoxCell.Items.Add(consElement.Variation);
        //                    }

        //                    // Set the selected value to the variation
        //                    rowMap[name].Cells[(int)MedicalLabTestElementsGridColumn.VARIATION].Value = consElement.Variation;
        //                }
        //                else
        //                {
        //                    // Create a new row for this name
        //                    int newRowIdx = GridViewLabTestElementInformation.Rows.Add();
        //                    DataGridViewRow newRow = GridViewLabTestElementInformation.Rows[newRowIdx];

        //                    // Set cell values for the new row
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.SNO].Value = newRowIdx + 1;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.NAME].Value = name;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.UOM].Value = consElement.Uom.Name.ToString();
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.VARIATION].Value = consElement.Variation;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value = consElement.ResultDescription;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.RANGETYPE].Value = consElement.Type;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value = consElement.ValueFrom;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value = consElement.ValueTo;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value = consElement.Id;
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value = consElement.MedicalTestElementId;

        //                    // Create a ComboBoxCell for the VARIATION column
        //                    DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();

        //                    // Populate ComboBoxCell with all variations from lLabTestElements
        //                    foreach (MedicalTestElement MedicalElement in lLabTestElements)
        //                    {
        //                        comboBoxCell.Items.Add(MedicalElement.Class);
        //                    }

        //                    // Ensure that the variation from ConsultedLabTestElements is added to the ComboBox items
        //                    if (!comboBoxCell.Items.Contains(consElement.Variation))
        //                    {
        //                        comboBoxCell.Items.Add(consElement.Variation);
        //                    }

        //                    // Set the ComboBoxCell DisplayStyle to allow a dropdown
        //                    //comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;

        //                    // Assign the ComboBoxCell to the VARIATION column
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.VARIATION] = comboBoxCell;

        //                    // Set the value for the variation column to ensure it displays the selected item
        //                    newRow.Cells[(int)MedicalLabTestElementsGridColumn.VARIATION].Value = consElement.Variation;

        //                    // Store the row in the dictionary for later reuse
        //                    rowMap[name] = newRow;
        //                }
        //            }

        //            // Enable the necessary buttons after loading the data
        //            BtnConsultLabTestElementCancel.Enabled = true;
        //            BtnConsultLabTestElementSave.Enabled = true;
        //            BtnConsultLabTestPrintRequisition.Enabled = true;
        //        }
        //    }
        //}

        //private void LoadLabTestDetails29092024(long LabTestId)
        //{
        //    GridViewLabTestElementInformation.Rows.Clear();
        //    ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(LabTestId);
        //    if (consultedLabTest != null)
        //    {
        //        IList<MedicalTestElement> lLabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(consultedLabTest.MedicalTestId);
        //        IList<ConsultedLabTestElements> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestElementsByLabtestId(LabTestId);
        //        int i = 0;
        //        if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
        //        {
        //            string PrescriptionDetails = string.Empty;
        //            foreach (ConsultedLabTestElements ConsElement in lConsultedLabTestElements)
        //            {
        //                //if (lLabTestElements != null && lLabTestElements.Count > 0)
        //                //{
        //                //    MedicalTestElement medicalTestElement = null!;
        //                //    medicalTestElement = lLabTestElements.FirstOrDefault(x => x.Id != ConsElement.MedicalTestElementId)!;
        //                //    if (medicalTestElement != null)
        //                //    {
        //                //        lLabTestElements.Remove(medicalTestElement);
        //                //    }
        //                //}
        //                GridViewLabTestElementInformation.Rows.Add();
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.SNO].Value = i + 1;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.NAME].Value = ConsElement.Name;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.VARIATION].Value = ConsElement.Variation;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value = ConsElement.ResultDescription;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGETYPE].Value = ConsElement.Type;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value = ConsElement.ValueFrom;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value = ConsElement.ValueTo;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.ID].Value = ConsElement.Id;
        //                GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value = ConsElement.MedicalTestElementId;

        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Style.BackColor = Color.LightGray;
        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.NAME].Style.SelectionBackColor = Color.LightGray;
        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.VARIATION].Style.BackColor = Color.LightGray;
        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGETYPE].Style.BackColor = Color.LightGray;
        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Style.BackColor = Color.LightGray;
        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Style.BackColor = Color.LightGray;
        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.SNO].Style.BackColor = Color.LightGray;
        //                //GridViewLabTestElementInformation.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.SNO].Style.BackColor = Color.LightGray;


        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_ABS].Value = ConsElement.LowAbsolute;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_NOR].Value = ConsElement.LowNormal;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_CRI].Value = ConsElement.LowCritical;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_OBSERVED].Value = ConsElement.ObservedLow;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.DESCRIPTION].Value = ConsElement.ResultDescription;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.ID].Value = ConsElement.Id;

        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.UOM].Style.BackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.UOM].Style.SelectionBackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_ABS].Style.BackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_NOR].Style.BackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_CRI].Style.BackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_ABS].Style.BackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_NOR].Style.BackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_CRI].Style.BackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_ABS].Style.SelectionBackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_NOR].Style.SelectionBackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_CRI].Style.SelectionBackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_ABS].Style.SelectionBackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_NOR].Style.SelectionBackColor = Color.LightGray;
        //                //GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_CRI].Style.SelectionBackColor = Color.LightGray;
        //                i++;

        //            }
        //            BtnConsultLabTestElementCancel.Enabled = true;
        //            BtnConsultLabTestElementSave.Enabled = true;
        //            BtnConsultLabTestPrintRequisition.Enabled = true;
        //        }
        //        //if (lLabTestElements != null && lLabTestElements.Count > 0)
        //        //{
        //        //    foreach (MedicalTestElement Element in lLabTestElements)
        //        //    {
        //        //        GridViewConsultElementInfo.Rows.Add();
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.SNO].Value = i + 1;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.NAME].Value = Element.Name;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.UOM].Value = Element.UomId;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_ABS].Value = Element.HiAbsolute;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_NOR].Value = Element.HiNormal;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_CRI].Value = Element.HiCritical;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_ABS].Value = Element.LowAbsolute;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_NOR].Value = Element.LowNormal;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_CRI].Value = Element.LowCritical;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.DESCRIPTION].Value = Element.ResultDescription;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.TESTID].Value = Element.Id;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_OBSERVED].Value = 0;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_OBSERVED].Value = 0;

        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.UOM].Style.BackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.UOM].Style.SelectionBackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_ABS].Style.BackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_NOR].Style.BackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_CRI].Style.BackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_ABS].Style.BackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_NOR].Style.BackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_CRI].Style.BackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_ABS].Style.SelectionBackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_NOR].Style.SelectionBackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.HI_CRI].Style.SelectionBackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_ABS].Style.SelectionBackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_NOR].Style.SelectionBackColor = Color.LightGray;
        //        //        GridViewConsultElementInfo.Rows[i].Cells[(int)ConsLabTestElementsGridColumn.LOW_CRI].Style.SelectionBackColor = Color.LightGray;
        //        //        i++;
        //        //    }
        //        //    BtnConsultLabTestElementCancel.Enabled = true;
        //        //    BtnConsultLabTestElementSave.Enabled = true;
        //        //    BtnConsultLabTestPrintRequisition.Enabled = true;
        //        //}
        //    }
        //}
        private bool ValidateProceduresInfo()
        {
            return true;
        }
        private bool ValidateTestElementDetail()
        {
            int rcount = GridViewLabTestElementInformation.Rows.Count;
            ConsultedNoteErrMsg.Text = string.Empty;
            foreach (DataGridViewRow row in GridViewLabTestElementInformation.Rows)
            {
                if (row.Index + 1 != rcount)
                {
                    if (row.Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value == null || string.IsNullOrEmpty(row.Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value.ToString()))
                    {
                        for (int i = 3; i < 6; i++)
                        {
                            if (i == 4 || i == 5 || i == 3) { continue; }
                            if (row.Cells[i].Value == null || string.IsNullOrEmpty(row.Cells[i].Value.ToString()) || row.Cells[i].Value.ToString()!.Equals("0"))
                            {
                                ConsultedNoteErrMsg.Text = string.Format(EnterLabtestElementErrorMsg, GridViewLabTestElementInformation.Columns[i].HeaderText);
                                GridViewLabTestElementInformation.CurrentCell = GridViewLabTestElementInformation[i, row.Index];
                                GridViewLabTestElementInformation.BeginEdit(true);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        private void BtnSaveConsLabTestElement_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            ConsultedNoteErrMsg.Text = "";

            if (GridViewLabTestElementInformation.Rows.Count > 0)
            {
                if (ValidateTestElementDetail()) // MedicalLabTestElementsGridColumn
                {
                    IList<ConsultedLabTestElements> lConsultedLabTestElements = new List<ConsultedLabTestElements>();

                    foreach (DataGridViewRow row in GridViewLabTestElementInformation.Rows)
                    {
                        ConsultedLabTestElements ConsultedLabTestElements = new ConsultedLabTestElements();

                        if (row.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value != null)
                        {
                            long Id = long.Parse(row.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value.ToString()!);
                            ConsultedLabTestElements ConsultedLabTestElementsFromDB = ConsultationNoteManager.Instance.GetConsultedLabTestElementsById(Id);

                            if (ConsultedLabTestElementsFromDB != null)
                            {
                                // Populate fields from the database
                                ConsultedLabTestElements.ConsLabTestId = ConsultedLabTestElementsFromDB.ConsLabTestId;
                                ConsultedLabTestElements.CompanyId = Global.Company.CompanyId;
                                ConsultedLabTestElements.Id = Id;
                                ConsultedLabTestElements.MedicalTestElementId = ConsultedLabTestElementsFromDB.MedicalTestElementId;
                                ConsultedLabTestElements.Name = ConsultedLabTestElementsFromDB.Name;
                                ConsultedLabTestElements.UomId = ConsultedLabTestElementsFromDB.MedicalTestElement.UomId;

                                // Retrieve the selected value from ComboBoxCell
                                ConsultedLabTestElements.Class = row.Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value?.ToString() ?? "";
                                ConsultedLabTestElements.SubClass = row.Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value?.ToString() ?? "";
                                ConsultedLabTestElements.ResultDescription = row.Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value?.ToString() ?? "";

                                ConsultedLabTestElements.SingleValue = row.Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Value?.ToString() ?? "";
                                ConsultedLabTestElements.RangeFrom = row.Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value?.ToString() ?? "";
                                ConsultedLabTestElements.RangeTo = row.Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value?.ToString() ?? "";

                                lConsultedLabTestElements.Add(ConsultedLabTestElements);
                            }
                        }
                        else if (row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value != null)
                        {
                            long TestId = long.Parse(row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value.ToString()!);
                            MedicalTestElement Element = MedicalTestManager.Instance.GetMedicalTestElementById(TestId, Global.Company.CompanyId);

                            if (Element != null)
                            {
                                // Populate fields for new entries
                                ConsultedLabTestElements.ConsLabTestId = GridViewLabTestHistory.LTestId;
                                ConsultedLabTestElements.CompanyId = Global.Company.CompanyId;
                                ConsultedLabTestElements.Id = 0L; // New entry
                                ConsultedLabTestElements.MedicalTestElementId = Element.Id;
                                ConsultedLabTestElements.Name = Element.Name;

                                // Retrieve the selected value from ComboBoxCell
                                ConsultedLabTestElements.Uom = (MedicalTestUOM)row.Cells[(int)MedicalLabTestElementsGridColumn.UOM].Value;
                                ConsultedLabTestElements.Class = row.Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value?.ToString() ?? "";
                                ConsultedLabTestElements.SingleValue = row.Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Value?.ToString() ?? "";
                                ConsultedLabTestElements.ResultDescription = row.Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value?.ToString() ?? "";
                                lConsultedLabTestElements.Add(ConsultedLabTestElements);
                            }
                        }
                    }

                    // Save the updated elements to the database
                    ConsultationNoteManager.Instance.UpdateConsultedLabTestElements(lConsultedLabTestElements);

                    // Refresh the DataGridView
                    GridViewLabTestHistory_Load(sender, e);

                    // Reload the patient chart
                    //LoadPatientChart();

                    // Display success message
                    ConsultedNoteErrMsg.Text = SaveSuccessMsg;
                }
            }

            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
        private void GridViewPrescriptionDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewNote_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewNote_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.DATE].Value = Global.getTransactionDate();
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value = null;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.FEE].Value = "0.00";
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value = false;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value = false;
            GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.INVOICED].Value = false;
        }
        private void BtnPrescriptionPreview_Click(object sender, EventArgs e)
        {
            string PrintPaperFormat = GetPrintPaperFormat();
            if (GridViewPrescriptionHistory.NoteId != 0L && PatientId != 0L)
            {
                PrescriptionPrinting PrescriptionPrinting = new PrescriptionPrinting();
                PrescriptionPrinting.ExportOrPrintToFileA4andA5Format(GridViewPrescriptionHistory.NoteId, PatientId, GridViewPrescriptionHistory.RefNos, GridViewPrescriptionHistory.RefeBy, false, "PRESCRIPTION", "pdf", false, ByteImg(CheckedBoxNew), ByteImg(UnCheckedBoxNew), PrintPaperFormat);
            }
        }
        private void BtnPrescriptionPrint_Click(object sender, EventArgs e)
        {
            string PrintPaperFormat = GetPrintPaperFormat();
            if (GridViewPrescriptionHistory.NoteId != 0L && PatientId != 0L)
            {
                PrescriptionPrinting PrescriptionPrinting = new PrescriptionPrinting();
                PrescriptionPrinting.ExportOrPrintToFileA4andA5Format(GridViewPrescriptionHistory.NoteId, PatientId, GridViewPrescriptionHistory.RefNos, GridViewPrescriptionHistory.RefeBy, false, "PRESCRIPTION", "pdf", true, ByteImg(CheckedBoxNew), ByteImg(UnCheckedBoxNew), PrintPaperFormat);
            }
        }
        public string GetPrintPaperFormat()
        {
            DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();
            string PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.PRESCRIPTION)!.PrintPaperFormat.Name;
            return PrintPaper;
        }
        private byte[] ByteImg(PictureBox PictureBox)
        {
            MemoryStream Stream = new MemoryStream();
            PictureBox.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] Img = Stream.ToArray();
            return Img;
        }
        private void BtnLabTestPrintRequisition_Click(object sender, EventArgs e)
        {
            if (GridViewLabTestHistory.LTestId != 0L && PatientId != 0L)
            {
                LabTestPrinting LabTestPrinting = new LabTestPrinting();
                LabTestPrinting.ExportOrPrintToFile(GridViewLabTestHistory.LTestId, PatientId, "LabTest Result's", "pdf", true);
            }
        }
        private void LoadLabTestAttachment(long ConsLabTestId)
        {
            LapTestPictureBox.Image = null;
            GridViewConsultLabTestImage.Rows.Clear();
            GridViewConsultLabTestImage.Rows.Add();
            IList<LabTestAttachment> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestAttachmentByLabtestId(ConsLabTestId);
            if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
            {
                GridViewConsultLabTestImage.Rows.Add(lConsultedLabTestElements.Count);
                int i = 0;
                foreach (LabTestAttachment LabtestAttachment in lConsultedLabTestElements)
                {
                    GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                    GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value = LabtestAttachment.FileName;
                    GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.DESC].Value = LabtestAttachment.Description;
                    ButtonToggle(true, i);
                    GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value = LabtestAttachment.Id;
                    GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value = LabtestAttachment.Attachment;
                    GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.TYPE].Value = LabtestAttachment.FileType;
                    i++;
                }
                GridViewConsultLabTestImage.CurrentCell = GridViewConsultLabTestImage.Rows[0].Cells[0];
                GridViewLabTestImage_CellEnter(this.GridViewConsultLabTestImage, new DataGridViewCellEventArgs(0, 0));
                BtnLabTestImgSave.Enabled = true;
                BtnLabTestImgCancel.Enabled = true;
            }
            else
            {
                EnableViewer();
            }
            this.formIsDirty = false;
        }
        private void ButtonToggle(bool btnStatus, int cellIndex)
        {
            if (btnStatus)
            {
                GridViewConsultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "\u2B73";
                GridViewConsultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                GridViewConsultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "Download";
            }
            else
            {
                GridViewConsultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "+";
                GridViewConsultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                GridViewConsultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "";
            }
        }
        private void GridViewLabTestImage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE)
                {
                    UploadDownload();
                }
                else if (e.ColumnIndex == (int)AddFileUploadGridColumn.REMOVE && e.RowIndex != GridViewConsultLabTestImage.Rows.Count - 1)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].Value.ToString() + "?", "Delete Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (e.RowIndex == 0 && GridViewConsultLabTestImage.Rows.Count <= 1)
                        {
                            GridViewConsultLabTestImage.Rows.RemoveAt(e.RowIndex);
                            GridViewConsultLabTestImage.Rows.Add();
                        }
                        else
                        {
                            GridViewConsultLabTestImage.Rows.RemoveAt(e.RowIndex);
                            for (int i = 0; i < GridViewConsultLabTestImage.Rows.Count - 1; i++)
                            {
                                GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                            }
                        }
                        ReSequence();
                    }
                }

            }
        }
        private void UploadDownload()
        {
            if (GridViewConsultLabTestImage.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE && GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText == ("Edit"))
            {
                LabTestAttachment = null!;
                FormUploadDocumentWithPreview FormUploadDocumentWithPreview = new FormUploadDocumentWithPreview(this);
                FormUploadDocumentWithPreview.ShowDialog();
                if (LabTestAttachment != null)
                {
                    GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.ID].Value = null;
                    GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value = LabTestAttachment.FileType;
                    GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.SNO].Value = GridViewConsultLabTestImage.Rows.Count;
                    GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value = LabTestAttachment.FileName;
                    GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.DESC].Value = LabTestAttachment.Description;
                    GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value = LabTestAttachment.Attachment;
                    ButtonToggle(true, GridViewConsultLabTestImage.CurrentRow.Index);
                    GridViewConsultLabTestImage.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    int Index = GridViewConsultLabTestImage.CurrentRow.Index;
                    GridViewConsultLabTestImage.Rows.Add();
                    GridViewConsultLabTestImage.CurrentCell = GridViewConsultLabTestImage.Rows[Index].Cells[0];
                    GridViewLabTestImage_CellEnter(this.GridViewConsultLabTestImage, new DataGridViewCellEventArgs(0, Index));
                }
            }
            else if (GridViewConsultLabTestImage.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE && GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText == "Download")
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString() + ComboUtils.GetExtension((FileType)GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value);
                    saveFileDialog.DefaultExt = Path.GetExtension(GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString());
                    saveFileDialog.AddExtension = true;
                    if (DialogResult.OK == saveFileDialog.ShowDialog())
                    {
                        byte[] array = (byte[])GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value;
                        File.WriteAllBytes(saveFileDialog.FileName + Path.GetExtension(saveFileDialog.FileName), array);
                    }
                }
            }
        }
        private void GridViewLabTestImage_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].Value = GridViewConsultLabTestImage.Rows.Count;
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewConsultLabTestImage.Rows.Count; i++)
            {
                GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
            }
        }
        private void BtnLabTestImgSave_Click(object sender, EventArgs e)
        {
            LabTestImgSave(sender, e, true);
        }
        private void LabTestImgSave(object sender, EventArgs e, bool loadChart)
        {
            Cursor.Current = Cursors.WaitCursor;
            ConsultedNoteErrMsg.Text = "";
            IList<LabTestAttachment> LabTestAttachment = new List<LabTestAttachment>();
            for (int i = 0; i < GridViewConsultLabTestImage.Rows.Count - 1; i++)
            {
                LabTestAttachment lLabTestAttachment = new LabTestAttachment();
                if (GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value != null)
                {
                    LabTestAttachment llLabTestAttachment = ConsultationNoteManager.Instance.GetConsLabTestAttachmentById(long.Parse(GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value.ToString()));
                    if (llLabTestAttachment != null)
                    {
                        lLabTestAttachment.FileType = llLabTestAttachment.FileType;
                        lLabTestAttachment.FileName = llLabTestAttachment.FileName;
                        lLabTestAttachment.Attachment = llLabTestAttachment.Attachment;
                        lLabTestAttachment.Description = llLabTestAttachment.Description;
                    }
                }
                else
                {
                    lLabTestAttachment.FileType = (FileType)GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.TYPE].Value;
                    lLabTestAttachment.FileName = GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString();
                    lLabTestAttachment.Description = GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.DESC].Value.ToString();
                    lLabTestAttachment.Attachment = (byte[])GridViewConsultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value;
                }
                lLabTestAttachment.ConsLabTestId = GridViewLabTestHistory.LTestId;
                LabTestAttachment.Add(lLabTestAttachment);
            }
            ConsultationNoteManager.Instance.UpdateConsultedLabTestAttachment(LabTestAttachment, GridViewLabTestHistory.LTestId);
            ConsultedNoteErrMsg.Text = SaveSuccessMsg;
            //if (loadChart)
            //{
            //    LoadPatientChart();
            //}
            Cursor.Current = Cursors.Default;
        }
        private void BtnLabTestImgCancel_Click(object sender, EventArgs e)
        {
            LoadLabTestAttachment(GridViewLabTestHistory.LTestId);
            LapTestPictureBox.BackgroundImage = null;
        }
        private void EnableViewer()
        {
            LapTestPictureBox.Visible = true;
            LapTestPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            LapTestPictureBox.Image = null;
            DocBrowserPatient.LoadDocument("about:blank");
            pdfDocumentView1.Refresh();
            DocBrowserPatient.Visible = false;
            pdfDocumentView1.Visible = false;
        }
        private void GridViewLabTestImage_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.NAME].ReadOnly = true;
            GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.DESC].ReadOnly = true;
            GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].ReadOnly = true;
            GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ReadOnly = true;
            GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.REMOVE].ReadOnly = true;
            if (GridViewConsultLabTestImage.Rows.Count > 1)
            {
                BtnLabTestImgSave.Enabled = true;
                BtnLabTestImgCancel.Enabled = true;
            }
            EnableViewer();
            if ((e.ColumnIndex <= (int)AddFileUploadGridColumn.CHOOSE) && e.RowIndex != GridViewConsultLabTestImage.Rows.Count - 1)
            {
                LapTestPictureBox.BackgroundImage = null;
                if (e.RowIndex > -1)
                {
                    if (GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.ID].Value != null)
                    {
                        LabTestAttachment LabTestAttachment = ConsultationNoteManager.Instance.GetConsLabTestAttachmentById(long.Parse(GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.ID].Value.ToString()));
                        if (LabTestAttachment != null)
                        {
                            if (LabTestAttachment.FileType == FileType.JPEG || LabTestAttachment.FileType == FileType.JPG || LabTestAttachment.FileType == FileType.PNG)
                            {
                                LapTestPictureBox.Visible = true;
                                MemoryStream Stream = new MemoryStream(LabTestAttachment.Attachment);
                                LapTestPictureBox.Image = System.Drawing.Image.FromStream(Stream);
                                LapTestPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            else
                            {
                                try
                                {
                                    string Filename = LabTestAttachment.FileName;
                                    if (File.Exists(Path.Combine(Path.GetTempPath(), Filename + ComboUtils.GetExtension(LabTestAttachment.FileType))))
                                    {
                                        try
                                        {
                                            File.Delete(Path.Combine(Path.GetTempPath(), Filename + ComboUtils.GetExtension(LabTestAttachment.FileType)));
                                        }
                                        catch (Exception exc)
                                        {
                                            Console.WriteLine(exc.HResult);
                                            for (int i = 1; i < 1000; i++)
                                            {
                                                Filename = LabTestAttachment.FileName + i.ToString();
                                                if (!File.Exists(Path.Combine(Path.GetTempPath(), Filename + ComboUtils.GetExtension(LabTestAttachment.FileType))))
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    FileStream stream = new FileStream((Path.GetTempPath() + Filename + ComboUtils.GetExtension(LabTestAttachment.FileType)), FileMode.CreateNew);
                                    BinaryWriter writer = new BinaryWriter(stream);
                                    if (LabTestAttachment.Attachment != null)
                                    {
                                        writer.Write(LabTestAttachment.Attachment, 0, LabTestAttachment.Attachment.Length);
                                        writer.Close();

                                        if (LabTestAttachment.FileType == FileType.DOC || LabTestAttachment.FileType == FileType.DOCX)
                                        {
                                            DocBrowserPatient.Visible = true;
                                            DocBrowserPatient.LoadDocument(Path.GetTempPath() + LabTestAttachment.FileName + ComboUtils.GetExtension(LabTestAttachment.FileType));

                                        }
                                        else
                                        {
                                            pdfDocumentView1.Visible = true;
                                            pdfDocumentView1.Load(Path.GetTempPath() + LabTestAttachment.FileName + ComboUtils.GetExtension(LabTestAttachment.FileType));
                                        }
                                    }
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine(exc.HResult);
                                }

                            }
                        }
                    }
                    if (GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.PATH].Value != null
                        && !string.IsNullOrEmpty(GridViewConsultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.PATH].Value.ToString()))
                    {
                        FileType FileType = (FileType)GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value;
                        string Filename = GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString()!;
                        byte[] array = (byte[])GridViewConsultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value;

                        if (FileType == FileType.JPEG || FileType == FileType.JPG || FileType == FileType.PNG)
                        {
                            LapTestPictureBox.Visible = true;
                            MemoryStream Stream = new MemoryStream(array);
                            LapTestPictureBox.Image = System.Drawing.Image.FromStream(Stream);
                            LapTestPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        else
                        {
                            try
                            {
                                if (File.Exists(Path.Combine(Path.GetTempPath(), Filename + ComboUtils.GetExtension(FileType))))
                                {
                                    try
                                    {
                                        File.Delete(Path.Combine(Path.GetTempPath(), Filename + ComboUtils.GetExtension(FileType)));
                                    }
                                    catch (Exception exc)
                                    {
                                        Console.WriteLine(exc.HResult);
                                        for (int i = 1; i < 1000; i++)
                                        {
                                            if (!File.Exists(Path.Combine(Path.GetTempPath(), Filename + ComboUtils.GetExtension(FileType))))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                FileStream stream = new FileStream((Path.GetTempPath() + Filename + ComboUtils.GetExtension(FileType)), FileMode.CreateNew);
                                BinaryWriter writer = new BinaryWriter(stream);
                                writer.Write(array, 0, array.Length);
                                writer.Close();

                                if (FileType == FileType.DOC || FileType == FileType.DOCX)
                                {
                                    DocBrowserPatient.Visible = true;
                                    DocBrowserPatient.LoadDocument(Path.GetTempPath() + Filename + ComboUtils.GetExtension(FileType));

                                }
                                else
                                {
                                    pdfDocumentView1.Visible = true;
                                    pdfDocumentView1.Load(Path.GetTempPath() + Filename + ComboUtils.GetExtension(FileType));
                                }
                            }
                            catch (Exception exc)
                            {
                                Console.WriteLine(exc.HResult);
                            }

                        }

                    }
                }
            }
        }
        private void BtnCancelConsLabTestElement_Click(object sender, EventArgs e)
        {
            ConsultedNoteErrMsg.Text = string.Empty;
            GridViewLabTestHistory_Load(sender, e);
        }
        private void BtnPrescriptionSendMedical_Click(object sender, EventArgs e)
        {
            ConsultedNoteErrMsg.Text = string.Empty;
            DialogResult Result = MessageBox.Show("You cannot modify the prescription once it is sent to the pharmacy.\nDo you want to continue?.", "Save Confirm",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Result == DialogResult.Yes)
            {
                if (GridViewPrescriptionHistory.NoteId != 0L)
                {
                    ConsultationNoteManager.Instance.UpdateNoteForPrescription(GridViewPrescriptionHistory.NoteId);
                    GridViewPrescriptionHistory_Load(sender, e);
                    ConsultedNoteErrMsg.Text = PrescriptionSendMedicalMsg;
                }
            }
        }
        private void PatientVitalHistory_DoubleClick(object sender, EventArgs e)
        {
            if (PatientVitalHistory.VitalsId != 0L)
            {
                PatientVitalEntry.VitalId = (long)PatientVitalHistory.VitalsId!;
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)sender;
            string item = cb.Text;
            if (GridViewPrescriptionDetail.CurrentCell.ColumnIndex == (int)PrescriptionDetailsGridColumn.HOURS)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = false;
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = false;
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = false;
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = false;
                }
                else
                {
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.MORNING].Value = "";
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].Value = "";
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.EVENING].Value = "";
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.NIGHT].Value = "";

                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.MORNING].ReadOnly = true;
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.AFTERNOON].ReadOnly = true;
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.EVENING].ReadOnly = true;
                    GridViewPrescriptionDetail.CurrentRow.Cells[(int)PrescriptionDetailsGridColumn.NIGHT].ReadOnly = true;
                }
            }
        }
        private void GridViewPrescriptionDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            System.Windows.Forms.ComboBox? combo = e.Control as System.Windows.Forms.ComboBox;
            if (GridViewPrescriptionDetail.CurrentCell.ColumnIndex == (int)PrescriptionDetailsGridColumn.HOURS)
            {
                combo!.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged!);
                combo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged!);
            }
        }
        private void BtnPrescriptionCancel_Click(object sender, EventArgs e)
        {
            GridViewPrescriptionHistory_Load(sender, e);
        }
        private void linkLabelUpdatePatient_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (PatientId > 0)
            {
                try
                {
                    using (PatientRegistration patientRegistration = new PatientRegistration(this))
                    {
                        patientRegistration.CreatePatientOnLoad = true;
                        patientRegistration.PatientId = PatientId;
                        patientRegistration.ShowDialog();
                        LoadConsultingData(PatientId);

                    }
                }
                catch (Exception ex)
                {
                    ConsultedNoteErrMsg.Text = string.Format(UpdateFileErrorMsg, ex.Message);
                }
            }
        }
        private void LinkDischargePatient_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (PatientId > 0 && PatientIPId != null)
            {
                FormDischargePatient DischargePatient = new FormDischargePatient
                {
                    PatientId = PatientId,
                    PatientIpId = (long)PatientIPId
                };
                DischargePatient.ShowDialog();
                this.Cursor = Cursors.WaitCursor;
                ChangePatientStatus();
                LoadConsultingData(PatientId);
                this.Cursor = Cursors.Default;
            }
        }
        private void ChangePatientStatus()
        {
            InPatientAdmission InPatientAdmission = IpManager.Instance.GetInPatientAdmissionById((long)PatientIPId);
            if (InPatientAdmission != null && InPatientAdmission.Status == InPatientStatus.DISCHARGED)
            {
                PatientIPId = null;
                PatientOpId = null;
            }
        }
        private void CheckBoxLoadAllNotes_CheckedChanged(object sender, EventArgs e)
        {
            if (this.formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes. Do you want to save ?", "Save Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.Yes)
                {
                    if (!ValidateNote())
                    {
                        return;
                    }
                    SaveConsultationNotes(true, true);
                }
            }
            this.Cursor = Cursors.WaitCursor;
            PatientId = parent != null ? long.Parse(parent.PatientIdTransport.Text) : long.Parse(PatientIdTransport.Text);
            LoadConsultingData(PatientId);
            this.formIsDirty = false;
            this.Cursor = Cursors.Default;
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }
        public void LoadNotesbyFilter()
        {
            Cursor.Current = Cursors.WaitCursor;
            GridViewNote.Rows.Clear();
            IList<ConsultationNote> ListConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientId(PatientId);
            IList<DischargeNote> ListDischargeNote = DischargeNoteManager.Instance.ListDischargeNoteByPatientId(PatientId);
            int row = 0;
            DateTime ConsltdateTime = Global.getTransactionDate().AddMonths(-1);
            if (CheckBoxLoadAllNotes.Checked)
            {
                ConsltdateTime = Global.getTransactionDate().AddYears(-100);
            }
            foreach (ConsultationNote ConsultationNote in ListConsultationNote.Where(d => d.Date > ConsltdateTime))
            {
                GridViewNote.Rows.Add();
                LastConsultedNoteId = ConsultationNote.Id;
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.DATE].Value = ConsultationNote.Date;
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value = ConsultationNote.Note;
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value = ConsultationNote.Id;
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.OPID].Value = ConsultationNote.OpRegistrationId;
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.IPID].Value = ConsultationNote.InPatientAdmissionId;
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.IPOP].Value = ConsultationNote.InPatientAdmissionId != null ? "IP" : "OP";
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value = ConsultationNote.IsDischarged;
                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.INVOICED].Value = ConsultationNote.IsInvoiced;
                if (GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.IPOP].Value.ToString() == "IP")
                {
                    {
                        if (GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.IPID].Value != null)
                        {
                            InPatientAdmission InPatient = IpManager.Instance.GetInPatientAdmissionById((long)GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.IPID].Value);
                            if (InPatient != null && InPatient.Status == InPatientStatus.DISCHARGED)
                            {
                                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value = true;
                            }
                            else
                            {
                                GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value = false;
                            }
                        }
                    }
                }
                else if (GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.IPOP].Value.ToString() == "OP")
                {
                    if (GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.OPID].Value != null)
                    {
                        Registration OutPatient = OpManager.Instance.GetRegisterByRegId((long)GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.OPID].Value);
                        if (OutPatient != null && OutPatient.Status == fa.model.Hms.Op.Status.COMPLETED)
                        {
                            GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value = true;

                        }
                        else
                        {
                            GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value = false;
                        }
                    }
                }

                IList<ConsultedSymptom> ConsultedSymptoms = ConsultationNoteManager.Instance.ListSymptomByNoteId(ConsultationNote.Id);
                if (ConsultedSymptoms.Count > 0)
                {
                    DataGridViewComboBoxCell SelectedSymptomsids = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedSymptomsDesc = new DataGridViewComboBoxCell();

                    string SymptomsName = string.Empty;
                    foreach (ConsultedSymptom ConsultedSymptom in ConsultedSymptoms)
                    {
                        SelectedSymptomsids.Items.Add(ConsultedSymptom.SymptomId);
                        SymptomsName = string.IsNullOrEmpty(SymptomsName) ? ConsultedSymptom.Symptom.Name : SymptomsName + ",\n" + ConsultedSymptom.Symptom.Name;
                        SelectedSymptomsDesc.Items.Add(string.IsNullOrEmpty(ConsultedSymptom.Description) ? "" : ConsultedSymptom.Description);

                    }
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value = SelectedSymptomsids;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value = SymptomsName;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value = SelectedSymptomsDesc;

                }

                if (ConsultationNote.IsDischarged)
                {
                    IList<DischargePrescription> DischargePrescription = DischargeNoteManager.Instance.ListDischargePrescriptionByPatientIpId((long)ConsultationNote.InPatientAdmissionId);
                    if (DischargePrescription != null && DischargePrescription.Count > 0)
                    {
                        string PrescriptionsName = string.Empty;
                        foreach (DischargePrescription DisPrescription in DischargePrescription)
                        {
                            Product Product = DisPrescription.Prescription.ProductId != null ? CatalogProductManager.Instance.GetProductInfoById((long)DisPrescription.Prescription.ProductId) : null!;
                            PrescriptionsName = string.IsNullOrEmpty(PrescriptionsName) ? (Product == null ? DisPrescription.Prescription.CustomProduct : Product.Name) : PrescriptionsName + ",\n" + (Product == null ? DisPrescription.Prescription.CustomProduct : Product.Name);
                        }
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value = PrescriptionsName;
                    }
                }
                else
                {
                    DataGridViewComboBoxCell SelectedPrescriptionsName = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedPrescriptionsids = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedPrescriptionsDosages = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedPrescriptionsDosageDay = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedPrescriptionsIntervals = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedPrescriptionsBeforeorAfter = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedMorningPrescriptions = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedAfterNoonPrescriptions = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedEveningPrescriptions = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedNightPrescriptions = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedPrescriptionsAdditonalNotes = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedMorn = new DataGridViewComboBoxCell();

                    IList<ConsultedPrescription> ConsultedPrescriptions = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(ConsultationNote.Id);
                    if (ConsultedPrescriptions.Count > 0)
                    {

                        string PrescriptionsName = string.Empty;
                        foreach (ConsultedPrescription ConsultedPrescription in ConsultedPrescriptions)
                        {
                            Product Product = ConsultedPrescription.Prescription.ProductId != null ? CatalogProductManager.Instance.GetProductInfoById((long)ConsultedPrescription.Prescription.ProductId) : null!;
                            SelectedPrescriptionsName.Items.Add(Product != null ? Product.Name : ConsultedPrescription.CustomPrescription);
                            SelectedPrescriptionsids.Items.Add(Product != null ? Product.Id : ConsultedPrescription.CustomPrescription);
                            PrescriptionsName = string.IsNullOrEmpty(PrescriptionsName) ? (Product != null ? Product.Name : ConsultedPrescription.CustomPrescription) : PrescriptionsName + ",\n" + (Product != null ? Product.Name : ConsultedPrescription.CustomPrescription);
                            SelectedPrescriptionsDosages.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Total) ? "" : ConsultedPrescription.Prescription.Total);
                            SelectedPrescriptionsDosageDay.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Days.ToString()) ? "" : ConsultedPrescription.Prescription.Days.ToString());
                            SelectedPrescriptionsBeforeorAfter.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.TakeDosage.ToString()) ? "" : ConsultedPrescription.Prescription.TakeDosage.ToString());
                            SelectedPrescriptionsIntervals.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Hours) ? "" : ConsultedPrescription.Prescription.Hours);
                            SelectedMorn.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Morning) ? "" : ConsultedPrescription.Prescription.Morning);
                            SelectedMorningPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Morning) ? "" : ConsultedPrescription.Prescription.Morning);
                            SelectedAfterNoonPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Afternoon) ? "" : ConsultedPrescription.Prescription.Afternoon);
                            SelectedEveningPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Evening) ? "" : ConsultedPrescription.Prescription.Evening);
                            SelectedNightPrescriptions.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.Night) ? "" : ConsultedPrescription.Prescription.Night);
                            SelectedPrescriptionsAdditonalNotes.Items.Add(string.IsNullOrEmpty(ConsultedPrescription.Prescription.AdditionalNotes) ? "" : ConsultedPrescription.Prescription.AdditionalNotes);
                        }
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value = SelectedPrescriptionsids;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value = PrescriptionsName;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONNAME].Value = SelectedPrescriptionsName;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.TOTAL].Value = SelectedPrescriptionsDosages;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value = SelectedPrescriptionsDosageDay;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value = SelectedPrescriptionsBeforeorAfter;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value = SelectedPrescriptionsIntervals;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.MORNING].Value = SelectedMorningPrescriptions;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value = SelectedAfterNoonPrescriptions;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.EVENING].Value = SelectedEveningPrescriptions;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.NIGHT].Value = SelectedNightPrescriptions;
                        GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value = SelectedPrescriptionsAdditonalNotes;
                    }
                }
                //labtest
                IList<ConsultedLabTest> ConsultedLabTests = ConsultationNoteManager.Instance.ListLabTestByNoteId(ConsultationNote.Id);
                if (ConsultedLabTests.Count > 0)
                {
                    DataGridViewComboBoxCell SelectedLabTestsids = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedLabTestsdesc = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedLabTestsfee = new DataGridViewComboBoxCell();

                    DataGridViewComboBoxCell SelectedLabTestsElementsids = new DataGridViewComboBoxCell();

                    string LabTestsName = string.Empty;
                    foreach (ConsultedLabTest ConsultedLabTest in ConsultedLabTests)
                    {
                        if (ConsultedLabTest.ConsultedLabTestElements != null && ConsultedLabTest.ConsultedLabTestElements.Count > 0)
                        {
                            foreach (ConsultedLabTestElements ConsultedLabTestElement in ConsultedLabTest.ConsultedLabTestElements)
                            {
                                SelectedLabTestsElementsids.Items.Add(ConsultedLabTestElement.MedicalTestElementId);
                            }
                        }
                        SelectedLabTestsids.Items.Add(ConsultedLabTest.MedicalTest.Id);
                        SelectedLabTestsdesc.Items.Add(ConsultedLabTest.MedicalTest.Description);
                        //SelectedLabTestsfee.Items.Add(ConsultedLabTest.MedicalTest.Fee);
                        LabTestsName = string.IsNullOrEmpty(LabTestsName) ? ConsultedLabTest.MedicalTest.Name : LabTestsName + ",\n" + ConsultedLabTest.MedicalTest.Name;
                    }
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value = SelectedLabTestsElementsids;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value = SelectedLabTestsids;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value = SelectedLabTestsdesc;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value = SelectedLabTestsfee;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value = LabTestsName;
                }
                //consultation
                IList<ConsultedConsultationFee> ConsultedConsultations = ConsultationNoteManager.Instance.ListConsultationByNoteId(ConsultationNote.Id);
                if (ConsultedConsultations.Count > 0)
                {
                    DataGridViewComboBoxCell SelectedConsultationsids = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedConsultationsFee = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedConsultationsDiscp = new DataGridViewComboBoxCell();
                    string ConsultationsName = string.Empty;
                    foreach (ConsultedConsultationFee ConsultedConsultation in ConsultedConsultations)
                    {
                        SelectedConsultationsids.Items.Add(ConsultedConsultation.Consultation.Id.ToString());
                        //SelectedConsultationsFee.Items.Add(ConsultedConsultation.IsOverrideFee ? GetLastUpdatedFee(ConsultedConsultation.Consultation.Id) : ConsultedConsultation.Fee);
                        SelectedConsultationsFee.Items.Add(ConsultedConsultation.Fee);
                        ConsultationsName = string.IsNullOrEmpty(ConsultationsName) ? ConsultedConsultation.Consultation.Name : ConsultationsName + ",\n" + ConsultedConsultation.Consultation.Name;
                        SelectedConsultationsDiscp.Items.Add(string.IsNullOrEmpty(ConsultedConsultation.Description) ? "" : ConsultedConsultation.Description);
                    }
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value = SelectedConsultationsids;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value = ConsultationsName;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value = SelectedConsultationsDiscp;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value = SelectedConsultationsFee;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.FEE].Value = TotalFee(SelectedConsultationsFee);

                }
                // Procedure
                IList<ConsultedProcedure> ConsultedProcedures = ConsultationNoteManager.Instance.ListProcedureByNoteId(ConsultationNote.Id);
                if (ConsultedProcedures != null && ConsultedProcedures.Count > 0)
                {
                    DataGridViewComboBoxCell SelectedProcedureids = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedProcedureFee = new DataGridViewComboBoxCell();
                    DataGridViewComboBoxCell SelectedProcedureDesc = new DataGridViewComboBoxCell();
                    string ProcedureName = string.Empty;
                    foreach (ConsultedProcedure ConsultedProcedure in ConsultedProcedures)
                    {
                        SelectedProcedureids.Items.Add(ConsultedProcedure.MedicalProcedure.Id);
                        SelectedProcedureFee.Items.Add(double.IsNaN(ConsultedProcedure.Fees) ? 0 : ConsultedProcedure.Fees);
                        SelectedProcedureDesc.Items.Add(string.IsNullOrEmpty(ConsultedProcedure.Description) ? "" : ConsultedProcedure.Description);
                        ProcedureName = string.IsNullOrEmpty(ProcedureName) ? ConsultedProcedure.MedicalProcedure.Name : ProcedureName + ",\n" + ConsultedProcedure.MedicalProcedure.Name;
                    }
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value = SelectedProcedureids;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value = ProcedureName;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value = SelectedProcedureDesc;
                    GridViewNote.Rows[row].Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value = SelectedProcedureFee;
                }
                GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                row++;
            }
            GridViewNote.FirstDisplayedScrollingRowIndex = GridViewNote.RowCount - 1;
            Cursor.Current = Cursors.Default;
        }
        private void GridViewNote_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (((e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDSYMPTOM ||
                e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDPRESCRIPTION ||
                e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDLABTEST ||
                e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDPROCEDURE ||
                e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDCONSULTATION ||
                e.ColumnIndex == (int)ConsultationNotesGridColumn.REMOVE) &&
                (((((GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value == null || !(bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value) && (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value != null && (string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value != "IP")) && ((GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value == null || !(bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value) && (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value != null && (string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value != "OP")))) ||
                ((((GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value == null || !(bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value) && (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value != null && (string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value == "IP")) || ((GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value == null || !(bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value) && (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value != null && (string)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.IPOP].Value == "OP")) && !GridViewNote.ReadOnly)))) ||
                (e.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES && (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value != null) && GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value != null && GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value.ToString() == "Patient got Discharged"))
                {
                    GridViewNote.Cursor = Cursors.Hand;
                }
                else if (e.RowIndex == GridViewNote.Rows.Count - 1)
                {
                    if (e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDSYMPTOM || e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDPRESCRIPTION ||
                        e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDLABTEST || e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDPROCEDURE || e.ColumnIndex == (int)ConsultationNotesGridColumn.ADDCONSULTATION)
                    {
                        GridViewNote.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        GridViewNote.Cursor = Cursors.Default;
                    }
                }
                else
                {
                    GridViewNote.Cursor = Cursors.Default;
                }
            }
        }
        private void GridViewNote_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            GridViewNote.Cursor = Cursors.Default;
        }
        private void GridViewProcedureInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void GridViewProcedureInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.SLNO].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.PDATE].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.NAME].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.DISC].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.STAT].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.FEES].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.PID].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.PERFMBY].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.PERFMON].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.NOTE].ReadOnly = true;
            GridViewProcedureInfo.Rows[e.RowIndex].Cells[(int)ProcedureInfoGridColumn.REQBY].ReadOnly = true;
            if (e.RowIndex == GridViewProcedureInfo.RowCount - 1)
            {
                GridViewProcedureInfoIsLastRow = true;
            }
            else
            {
                GridViewProcedureInfoIsLastRow = false;
            }
        }
        private void BtnCancelProcedureInfo_Click(object sender, EventArgs e)
        {
            LoadProcedureHistory();
        }
        private void BtnSaveProcedureInfo_Click(object sender, EventArgs e)
        {
            SaveProcedureInfo(sender, e, true);
        }
        private void SaveProcedureInfo(object sender, EventArgs e, bool loadChart)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            ConsultedNoteErrMsg.Text = "";
            if (GridViewProcedureInfo.Rows.Count > 0)
            {
                if (ValidateProceduresInfo())
                {
                    IList<ConsultedProcedure> lProcedure = new List<ConsultedProcedure>();
                    foreach (DataGridViewRow row in GridViewProcedureInfo.Rows)
                    {
                        ConsultedProcedure ConsultedProcedures = new ConsultedProcedure();
                        ConsultedProcedure ConsultedProceduresFromDB = ConsultationNoteManager.Instance.GetConsultedProceduresById(long.Parse(row.Cells[(int)ProcedureInfoGridColumn.PID].Value.ToString()));
                        if (ConsultedProceduresFromDB != null)
                        {
                            ConsultedProcedures.ProStatus = ConsultedProceduresFromDB.ProStatus;
                            ConsultedProcedures.Date = ConsultedProceduresFromDB.Date;
                            ConsultedProcedures.Name = ConsultedProceduresFromDB.Name;
                            ConsultedProcedures.CompanyId = Global.Company.CompanyId;
                            ConsultedProcedures.Description = ConsultedProceduresFromDB.Description;
                            ConsultedProcedures.PerformedById = ConsultedProceduresFromDB.PerformedById;
                            ConsultedProcedures.RequestedById = ConsultedProceduresFromDB.RequestedById;
                            ConsultedProcedures.RequestedOn = ConsultedProceduresFromDB.RequestedOn;
                            ConsultedProcedures.PerformOn = ConsultedProceduresFromDB.PerformOn;
                            ConsultedProcedures.MedicalProcedureId = ConsultedProceduresFromDB.MedicalProcedureId;
                            ConsultedProcedures.Fees = ConsultedProceduresFromDB.Fees;
                            ConsultedProcedures.ConsultedProcedureId = ConsultedProceduresFromDB.ConsultedProcedureId;
                            lProcedure.Add(ConsultedProcedures);
                        }
                        lProcedure.Add(ConsultedProcedures);
                    }
                    ConsultationNoteManager.Instance.UpdateConsultedMedicalProcedure(lProcedure);
                    if (loadChart)
                    {
                        LoadProcedureHistory();
                        // LoadPatientChart();
                    }
                    ConsultedNoteErrMsg.Text = SaveSuccessMsg;
                }
            }
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
        private void LockScreen()
        {
            GridViewNote.ReadOnly = true;
            BtnNotesSave.Enabled = false;
            BtnPrescriptionSave.Enabled = false;
            BtnPrescriptionSendMedical.Enabled = false;
            BtnConsultLabTestElementSave.Enabled = false;
            BtnLabTestImgSave.Enabled = false;
            BtnSaveVitals.Enabled = false;
            BtnSaveProcedureInfo.Enabled = false;
        }
        private void GridViewNote_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value != null && (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISDISCHARGED].Value == true
                || GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.INVOICED].Value != null && (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.INVOICED].Value == true
                || GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value != null && (bool)GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.ISVISITCOMPLETE].Value == true)
            {
                e.CellStyle.BackColor = Color.Gainsboro;
                GridViewNote.ClearSelection();
            }
            if (e.ColumnIndex == GridViewNote.Columns["CONSULTATIONNOTES"].Index && e.Value != null)
            {
                var cell = GridViewNote[e.ColumnIndex, e.RowIndex];
                cell.Style.WrapMode = DataGridViewTriState.True;
            }
        }

        private void GridViewProcedureInfo_DoubleClick(object sender, EventArgs e)
        {
            if (GridViewProcedureInfo.CurrentRow != null)
            {
                FormPatientProcedure PatientProcedure = new FormPatientProcedure(this);
                PatientProcedure.PatientId = PatientId;
                PatientProcedure.ProcedureId = long.Parse(GridViewProcedureInfo.CurrentRow.Cells[(int)ProcedureInfoGridColumn.PID].Value.ToString()!);
                PatientProcedure.ShowDialog();
                LoadProcedureHistory();
            }
        }
        private void BtnCompleteConsultation_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            if (ValidateNote())
            {
                SaveNote(sender, e, true);
                this.formIsDirty = false;
                this.Close();
            }
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
        private void BtnNoteExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnNotesSave_Click(object sender, EventArgs e)
        {
            if (ValidateNote())
            {
                SaveNote(sender, e, false);
            }
        }

        private void GridViewPrescriptionDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }
        private void GridViewNote_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (isRowAdding && e.ColumnIndex == 2 && GridViewNote.CurrentCell.RowIndex == GridViewNote.Rows.Count - 1)
            {
                if (GridViewNote.CurrentCell.EditedFormattedValue != null && GridViewNote.CurrentCell.EditedFormattedValue.ToString() != "")
                {
                    int index = e.RowIndex;
                    string formattedValue = GridViewNote.CurrentCell.EditedFormattedValue.ToString()!;
                    BeginInvoke(new Action(() =>
                    {
                        GridViewNote.Rows.Add();
                        isRowAdding = false;
                        GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        GridViewNote.Rows[index].Cells[2].Value = formattedValue;
                        GridViewNote.Rows[index + 1].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Value = "";
                        GridViewNote.CurrentCell = GridViewNote[4, index];
                        isRowAdding = true;
                    }));
                    isRowAdding = false;
                }
            }
            if (e.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                if (e.RowIndex == GridViewNote.Rows.Count - 1)
                {
                    this.formIsDirty = false;
                }
            }
        }
        private void GridViewNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Shift)
            {
                HandleShiftTabKeyForGridViewNote();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                HandleTabKeyForGridViewNote();
                e.Handled = true;
            }

            if (e.KeyCode == Keys.End)
            {
                HandleEndKey();
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                int ColumnIndex = GridViewNote.CurrentCell.ColumnIndex;
                int RowIndex = GridViewNote.CurrentCell.RowIndex;
                ConsultationNote Note = null!;
                if (GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value != null)
                {
                    Note = ConsultationNoteManager.Instance.GetConsultationNoteById(long.Parse(GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value.ToString()!));
                }
                if (GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value == null || (Note != null && Note.ConsultantId == Global.User.UserId)) //(GridViewNote.Rows[e.RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTINGNOTESID].Value == null || (Note != null && Note.ConsultantId == Global.User.UserId) || Global.User.IsSuperAdmin == true)
                {
                    if (Note == null || (Note != null && !Note.IsPrescriptionDispatchedForMedical))
                    {
                        if (Note == null || Note != null && !Note.IsInvoiced)
                        {
                            e.SuppressKeyPress = true;
                            HandleEnterOrSpaceKey(ColumnIndex, RowIndex);
                            e.Handled = true;
                        }
                        else
                        {
                            MessageBox.Show(DoNotAllowToEditInvoiced);
                        }
                    }
                    else
                    {
                        MessageBox.Show(DoNotAllowToEditDispatchedToMedicalMsg);
                    }
                }
                else
                {
                    MessageBox.Show(DoNotAllowToEditOtherConsultantConsultationMsg);
                }
            }
        }
        private void HandleEnterOrSpaceKey(int columnIndex, int RowIndex)
        {
            // Handle the enter or space key logic based on the column index
            if (columnIndex == (int)ConsultationNotesGridColumn.ADDSYMPTOM)
            {
                int Index = RowIndex;
                FormSelectSymptom formSelectSymptom = new FormSelectSymptom(this);
                formSelectSymptom.SelectedSymptomsids = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value;
                formSelectSymptom.SelectedSymptomsNames = GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value != null ? GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value.ToString()! : string.Empty;
                formSelectSymptom.SelectedSymptomsDiscp = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value;
                formSelectSymptom.ShowDialog();

                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                && (!string.IsNullOrEmpty(formSelectSymptom.SelectedSymptomsNames)))
                {
                    GridViewNote.Rows.Add();
                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.SYMPTOM].Value = formSelectSymptom.SelectedSymptomsNames;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.SYMPTOMSIDS].Value = formSelectSymptom.SelectedSymptomsids;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.SYMPTOMDISC].Value = formSelectSymptom.SelectedSymptomsDiscp;
                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDPRESCRIPTION, Index];
            }
            else if (columnIndex == (int)ConsultationNotesGridColumn.ADDPRESCRIPTION)
            {
                int Index = RowIndex;
                FormSelectPrescriptions formSelectPrescriptions = new FormSelectPrescriptions(this);
                formSelectPrescriptions.SelectedPrescriptionIds = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value;
                formSelectPrescriptions.SelectedPrescriptionsName = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONNAME].Value;
                formSelectPrescriptions.SelectedPrescriptionsDosage = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.TOTAL].Value;
                formSelectPrescriptions.SelectedPrescriptionsDosageDays = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value;
                formSelectPrescriptions.SelectedPrescriptionsInterval = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value;
                formSelectPrescriptions.SelectedPrescriptionsBeforeAfter = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value;
                formSelectPrescriptions.SelectedPrescriptionsMorning = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.MORNING].Value;
                formSelectPrescriptions.SelectedPrescriptionsAfterNoon = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value;
                formSelectPrescriptions.SelectedPrescriptionsEvening = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.EVENING].Value;
                formSelectPrescriptions.SelectedPrescriptionsNight = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.NIGHT].Value;
                formSelectPrescriptions.SelectedPrescriptionsNotes = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value;
                formSelectPrescriptions.SelectedPrescriptionsNames = GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value != null ? GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value.ToString()! : string.Empty;
                formSelectPrescriptions.ShowDialog();

                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                && (!string.IsNullOrEmpty(formSelectPrescriptions.SelectedPrescriptionsNames)))
                {
                    GridViewNote.Rows.Add();
                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value = formSelectPrescriptions.SelectedPrescriptionIds;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONNAME].Value = formSelectPrescriptions.SelectedPrescriptionsName;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTION].Value = formSelectPrescriptions.SelectedPrescriptionsNames;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.TOTAL].Value = formSelectPrescriptions.SelectedPrescriptionsDosage;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.DOSAGEDAYS].Value = formSelectPrescriptions.SelectedPrescriptionsDosageDays;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.INTERVAL].Value = formSelectPrescriptions.SelectedPrescriptionsInterval;

                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.MORNING].Value = formSelectPrescriptions.SelectedPrescriptionsMorning;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.AFTERNOON].Value = formSelectPrescriptions.SelectedPrescriptionsAfterNoon;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.EVENING].Value = formSelectPrescriptions.SelectedPrescriptionsEvening;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.NIGHT].Value = formSelectPrescriptions.SelectedPrescriptionsNight;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.BEFOREAFTER].Value = formSelectPrescriptions.SelectedPrescriptionsBeforeAfter;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PRESCRIPTIONIDS].Value = formSelectPrescriptions.SelectedPrescriptionIds;

                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.ADDITONALNOTES].Value = formSelectPrescriptions.SelectedPrescriptionsNotes;
                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDLABTEST, Index];
            }
            else if (columnIndex == (int)ConsultationNotesGridColumn.ADDLABTEST)
            {
                int Index = RowIndex;
                FormSelectLabTest FormSelectLabTest = new FormSelectLabTest(this);
                FormSelectLabTest.SelectedLabTestsids = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value;
                FormSelectLabTest.SelectedLabTestsDisc = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value;
                FormSelectLabTest.SelectedLabTestsFees = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value;
                FormSelectLabTest.SelectedLabTestsElementids = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value;
                FormSelectLabTest.SelectedLabTestsNames = GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value != null ? GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value.ToString()! : string.Empty;
                FormSelectLabTest.ShowDialog();

                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                && (!string.IsNullOrEmpty(FormSelectLabTest.SelectedLabTestsNames)))
                {
                    GridViewNote.Rows.Add();
                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTIDS].Value = FormSelectLabTest.SelectedLabTestsids;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTDESC].Value = FormSelectLabTest.SelectedLabTestsDisc;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTFEES].Value = FormSelectLabTest.SelectedLabTestsFees;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTESTELEMENTIDS].Value = FormSelectLabTest.SelectedLabTestsElementids;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.LABTEST].Value = FormSelectLabTest.SelectedLabTestsNames;
                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDPROCEDURE, Index];
            }
            else if (columnIndex == (int)ConsultationNotesGridColumn.ADDCONSULTATION)
            {
                int Index = RowIndex;
                FormSelectConsultation FormSelectConsultation = new FormSelectConsultation(this);
                FormSelectConsultation.SelectedConsultationsids = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value;
                FormSelectConsultation.SelectedConsultationsFees = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value;
                FormSelectConsultation.SelectedConsultationsDiscrp = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value;
                FormSelectConsultation.SelectedConsultationsNames = GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value != null ? GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value.ToString()! : string.Empty;
                FormSelectConsultation.PatientOpId = PatientOpId;
                FormSelectConsultation.ShowDialog();

                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                && (!string.IsNullOrEmpty(FormSelectConsultation.SelectedConsultationsNames)))
                {
                    GridViewNote.Rows.Add();
                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONIDS].Value = FormSelectConsultation.SelectedConsultationsids;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATION].Value = FormSelectConsultation.SelectedConsultationsNames;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONFEES].Value = FormSelectConsultation.SelectedConsultationsFees;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.FEE].Value = TotalFee(FormSelectConsultation.SelectedConsultationsFees);
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONDISC].Value = FormSelectConsultation.SelectedConsultationsDiscrp;
                if (GridViewNote.CurrentCell!.RowIndex == GridViewNote.Rows.Count - 1 && GridViewNote.CurrentCell.RowIndex != Index)
                {
                    GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, Index + 1];
                }
            }
            else if (columnIndex == (int)ConsultationNotesGridColumn.ADDPROCEDURE)
            {
                int Index = RowIndex;
                FormSelectProcedures FormSelectProcedures = new FormSelectProcedures(this);
                FormSelectProcedures.SelectedProcedureIds = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value;
                FormSelectProcedures.SelectedProcedureDisc = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value;
                FormSelectProcedures.SelectedProcedureFees = (DataGridViewComboBoxCell)GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value;
                FormSelectProcedures.SelectedProcedureNames = GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value != null ? GridViewNote.Rows[RowIndex].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value.ToString()! : string.Empty;
                FormSelectProcedures.ShowDialog();

                if (GridViewNote.CurrentCell != null && ((GridViewNote.Rows.Count - 1) == GridViewNote.CurrentCell.RowIndex)
                && (!string.IsNullOrEmpty(FormSelectProcedures.SelectedProcedureNames)))
                {
                    GridViewNote.Rows.Add();
                    GridViewNote.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDUREIDS].Value = FormSelectProcedures.SelectedProcedureIds;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDURE].Value = FormSelectProcedures.SelectedProcedureNames;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDUREDISC].Value = FormSelectProcedures.SelectedProcedureDisc;
                GridViewNote.Rows[Index].Cells[(int)ConsultationNotesGridColumn.PROCEDUREFEES].Value = FormSelectProcedures.SelectedProcedureFees;
                GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.ADDCONSULTATION, Index];
            }
        }
        private void LinkDischargePatient_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!e.Shift && e.KeyCode == Keys.Tab)
            {
                linkLabelUpdatePatient.Focus();
                e.IsInputKey = true;
            }
        }
        private void linkLabelUpdatePatient_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!e.Shift && e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                int Rows = GridViewNote.Rows.Count;
                TabControlConsult.SelectedTab = ConsultNotesTab;
                for (int i = 0; i < Rows; i++)
                {
                    if (GridViewNote.Rows[i].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].ReadOnly == false)
                    {
                        GridViewNote.Focus();
                        GridViewNote.CurrentCell = GridViewNote[2, i];
                        GridViewNote.BeginEdit(true);
                        break;
                    }
                }
            }
            if (e.Shift && e.KeyCode == Keys.Tab)
            {
                if (LinkDischargePatient.Visible)
                {
                    LinkDischargePatient.Focus();
                    e.IsInputKey = true;
                }
                else
                {
                    CheckBoxLoadAllNotes.Focus();
                }
            }
        }
        private void CheckBoxLoadAllNotes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (!e.Shift && e.KeyCode == Keys.Tab)
            {
                if (LinkDischargePatient.Visible)
                {
                    LinkDischargePatient.Focus();
                    e.IsInputKey = true;
                }
                else
                {
                    linkLabelUpdatePatient.Focus();
                }
            }
            if (e.Shift && e.KeyCode == Keys.Tab)
            {
                BtnNoteExit.Focus();
                e.IsInputKey = true;
            }
        }

        private void LoadMedicalHistory()
        {
            bool flag = false;
            GridViewPatientHistory.Rows.Clear();
            int i = 0;
            IList<PatientHistoryQuestionGroup> PatientHistoryQuestionGroupInfo = PatientMedicalHistoryManager.Instance.ListAllPatientHistoryQuestionGroup();

            if (PatientHistoryQuestionGroupInfo.Count > 0)
            {
                foreach (PatientHistoryQuestionGroup lPatientHistoryQuestionGroup in PatientHistoryQuestionGroupInfo.OrderBy(x => x.Name))
                {
                    IList<PatientHistoryQuestion> PatientHistoryQuestionInfo = PatientMedicalHistoryManager.Instance.ListAllPatientHistoryQuestionsByQuestionGroupId(lPatientHistoryQuestionGroup.Id);
                    if (PatientHistoryQuestionInfo.Count > 0)
                    {
                        int j = 2;
                        GridViewPatientHistory.Rows.Add(1);
                        GridViewPatientHistory.Rows[i].Cells[0].Value = lPatientHistoryQuestionGroup.Id;
                        GridViewPatientHistory.Rows[i].Cells[9].Value = lPatientHistoryQuestionGroup;
                        i++;
                        GridViewPatientHistory.Rows.Add(1);

                        foreach (PatientHistoryQuestion lPatientHistoryQuestion in PatientHistoryQuestionInfo)
                        {
                            flag = true;
                            GridViewPatientHistory.Rows[i].Cells[j].Value = lPatientHistoryQuestion;

                            // Tag all cells to false by default
                            if (GridViewPatientHistory.Rows[i].Cells[j - 1].Tag == null || (bool)GridViewPatientHistory.Rows[i].Cells[j - 1].Tag == false)
                            {
                                GridViewPatientHistory.Rows[i].Cells[j - 1].ReadOnly = true;
                                GridViewPatientHistory.Rows[i].Cells[j - 1].Tag = false;
                            }

                            if (lPatientHistoryQuestion.AdditionalNotes)
                            {
                                i++;
                                if (GridViewPatientHistory.Rows.Count == i)
                                {
                                    GridViewPatientHistory.Rows.Add(1);
                                }

                                GridViewPatientHistory.Rows[i].Cells[j - 1] = new DataGridViewTextBoxCell();
                                GridViewPatientHistory.Rows[i].Cells[j - 1].Value = "";
                                GridViewPatientHistory.Rows[i].Cells[j - 1].ReadOnly = true;

                                // Dynamically mark the appropriate cells for AdditionalNotes
                                //if ((j == 2 && i == 15) || (j == 4 && i == 15) || (j == 4 && i == 21))
                                //{
                                //    GridViewPatientHistory.Rows[i].Cells[j].Tag = true;
                                //    //GridViewPatientHistory.Rows[i].Cells[j].Tag = 101;
                                //}

                                DataGridViewAdvancedBorderStyle newStyle = new DataGridViewAdvancedBorderStyle
                                {
                                    Right = DataGridViewAdvancedCellBorderStyle.InsetDouble,
                                    Left = DataGridViewAdvancedCellBorderStyle.InsetDouble,
                                    Bottom = DataGridViewAdvancedCellBorderStyle.InsetDouble,
                                    Top = DataGridViewAdvancedCellBorderStyle.InsetDouble
                                };

                                DataGridViewAdvancedBorderStyle newStylePlaceholder = new DataGridViewAdvancedBorderStyle
                                {
                                    Right = DataGridViewAdvancedCellBorderStyle.InsetDouble,
                                    Left = DataGridViewAdvancedCellBorderStyle.InsetDouble,
                                    Bottom = DataGridViewAdvancedCellBorderStyle.InsetDouble,
                                    Top = DataGridViewAdvancedCellBorderStyle.InsetDouble
                                };

                                GridViewPatientHistory.Rows[i].Cells[j].AdjustCellBorderStyle(newStyle, newStylePlaceholder, true, true, true, true);
                                GridViewPatientHistory.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                                GridViewPatientHistory.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                                GridViewPatientHistory.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightGray;
                                GridViewPatientHistory.Rows[i].Cells[j].Style.SelectionForeColor = Color.Black;

                                // Adjust the column width based on text length
                                int textLength = lPatientHistoryQuestion.AdditionalNotesCaption!.Length;
                                GridViewPatientHistory.Columns[j - 1].Width = Math.Max(GridViewPatientHistory.Columns[j - 1].Width, (int)(textLength * 7.5)); // Assuming approx. 7.5 pixels per character

                                i--;
                            }

                            j += 2;
                            if (j > 8)
                            {
                                i = GridViewPatientHistory.Rows.Count;
                                j = 2;
                                GridViewPatientHistory.Rows.Add(1);
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            i = GridViewPatientHistory.Rows.Count;
                            GridViewPatientHistory.Rows.Add(1);
                            i++;
                        }
                    }
                }
            }

            // Apply the stored column widths
            ApplyColumnWidths();

            if (GridViewPatientHistory.Rows.Count > 0)
            {
                GridViewPatientHistory.EndEdit();
                for (int k = 0; k < GridViewPatientHistory.Rows.Count; k++)
                {
                    for (int l = 2; l < 9; l += 2)
                    {
                        // Ensure that cells are only marked false if not already marked true
                        if (GridViewPatientHistory.Rows[k].Cells[l].Value == null && GridViewPatientHistory.Rows[k].Cells[l - 1].GetType() == typeof(DataGridViewCheckBoxCell))
                        {
                            if (GridViewPatientHistory.Rows[k].Cells[l - 1].Tag == null || (bool)GridViewPatientHistory.Rows[k].Cells[l - 1].Tag == false)
                            {
                                GridViewPatientHistory.Rows[k].Cells[l - 1] = new DataGridViewTextBoxCell();
                                GridViewPatientHistory.Rows[k].Cells[l - 1].ReadOnly = true;
                                GridViewPatientHistory.Rows[k].Cells[l - 1].Tag = false;
                            }
                        }
                    }
                    if (GridViewPatientHistory.Rows[k].Cells[0].Value != null)
                    {
                        // Additional logic if needed
                    }
                }
            }
        }

        private void LoadMedicalHistoryData()
        {
            if (PatientId != 0L)
            {
                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                if (Patient != null)
                {
                    IList<PatientPreMedicalHistory> PatientPreMedicalHistory = PatientMedicalHistoryManager.Instance.ListAllPatientPreMedicalHistoryPatientId(PatientId);
                    if (PatientPreMedicalHistory.Count > 0)
                    {
                        foreach (DataGridViewRow rows in GridViewPatientHistory.Rows)
                        {
                            foreach (DataGridViewCell Cell in rows.Cells)
                            {
                                if (Cell.GetType() == typeof(DataGridViewCheckBoxCell))
                                {
                                    Cell.ReadOnly = true;

                                    // Check if the next cell contains a PatientHistoryQuestion
                                    if (GridViewPatientHistory.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex + 1].Value is PatientHistoryQuestion question)
                                    {
                                        PatientPreMedicalHistory History = PatientPreMedicalHistory.FirstOrDefault(x => x.HistoryItemId == question.Id)!;

                                        if (History != null)
                                        {
                                            GridViewPatientHistory.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value = true;

                                            if (question.AdditionalNotes)
                                            {
                                                GridViewPatientHistory.Rows[Cell.RowIndex + 1].Cells[Cell.ColumnIndex + 1].Value = History.AdditionalValue;
                                                GridViewPatientHistory.Rows[Cell.RowIndex + 1].Cells[Cell.ColumnIndex + 1].ReadOnly = false;
                                            }
                                            else
                                            {
                                                GridViewPatientHistory.Rows[Cell.RowIndex + 1].Cells[Cell.ColumnIndex + 1].ReadOnly = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (Cell.RowIndex > 0 &&
                                        Cell.ColumnIndex > 0 &&
                                        GridViewPatientHistory.Rows[Cell.RowIndex - 1].Cells[Cell.ColumnIndex - 1].GetType() == typeof(DataGridViewCheckBoxCell))
                                    {
                                        var cellValue = GridViewPatientHistory.Rows[Cell.RowIndex - 1].Cells[Cell.ColumnIndex - 1].Value;

                                        // Check if the cell value is not null and is of type boolean
                                        if (cellValue is bool isChecked && isChecked)
                                        {
                                            // Allow editing if the checkbox is checked
                                            Cell.ReadOnly = false;
                                        }
                                        else
                                        {
                                            // Otherwise, set it to read-only
                                            Cell.ReadOnly = true;
                                        }
                                    }
                                    else
                                    {
                                        Cell.ReadOnly = true;
                                    }


                                }
                            }
                        }
                    }
                    TextBoxOtherNotes.Text = Patient.OtherNotes;
                }
            }
        }

        private void GridViewPatientHistory_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewPatientHistory.Rows[e.RowIndex].Cells[1].Value = false;
            GridViewPatientHistory.Rows[e.RowIndex].Cells[3].Value = false;
            GridViewPatientHistory.Rows[e.RowIndex].Cells[5].Value = false;
            GridViewPatientHistory.Rows[e.RowIndex].Cells[7].Value = false;
        }
        public bool GridViewPatientHistoryChecked = false;
        private void GridViewPatientHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewPatientHistory.Enabled)
            {
                if (GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell)
                {
                    if (e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 5 || e.ColumnIndex == 7)
                    {
                        // Toggle the checkbox value
                        bool isChecked = !(bool)GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                        GridViewPatientHistoryChecked = true;
                        GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = isChecked;

                        GridViewPatientHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);

                        PatientHistoryQuestion? question = GridViewPatientHistory.Rows[e.RowIndex].Cells[9].Value as PatientHistoryQuestion;

                        if (question != null && question.AdditionalNotes)
                        {
                            GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].ReadOnly = !isChecked;
                            if (isChecked)
                            {
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory[e.ColumnIndex + 1, e.RowIndex + 1];
                                GridViewPatientHistory.BeginEdit(true);
                            }
                            else
                            {
                                GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value = null;
                            }
                        }
                        else
                        {
                            GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].ReadOnly = true;
                        }

                        PatientHistoryQuestion oldQuestion = PatientMedicalHistoryManager.Instance.GetPatientHistoryQuestionById(
                            ((PatientHistoryQuestion)GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value).Id
                        );

                        if (oldQuestion != null && oldQuestion.AdditionalNotes)
                        {
                            if (!isChecked)
                            {
                                GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].ReadOnly = false;
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory[e.ColumnIndex + 1, e.RowIndex + 1];
                                GridViewPatientHistory.BeginEdit(true);
                            }
                            else
                            {
                                GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value = null;
                                GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].ReadOnly = true;
                            }
                        }
                        else
                        {
                            GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].ReadOnly = true;
                        }

                        ToggleCheckBoxStateAtCurrentCell(e.RowIndex, e.ColumnIndex);
                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    else
                    {
                        GridViewPatientHistoryChecked = false;
                    }
                }
            }
        }

        private void BtnMedicalHistorySave_Click(object sender, EventArgs e)
        {
            MedicalHistorySave(sender, e, true);
        }
        private void MedicalHistorySave(object sender, EventArgs e, bool loadChart)
        {
            Cursor.Current = Cursors.WaitCursor;
            ConsultedNoteErrMsg.Text = "";
            if (GridViewPatientHistory.Rows.Count > 0)
            {
                Patient Patient = new Patient();
                Patient.Id = PatientId;
                Patient.OtherNotes = TextBoxOtherNotes.Text;
                PatientManager.Instance.UpdateFromCossultingPatient(Patient);
                Patient.History = new List<PatientPreMedicalHistory>();
                foreach (DataGridViewRow rows in GridViewPatientHistory.Rows)
                {
                    foreach (DataGridViewCell Cell in rows.Cells)
                    {
                        if (Cell.GetType() == typeof(DataGridViewCheckBoxCell))
                        {
                            if ((bool)Cell.Value)
                            {
                                PatientPreMedicalHistory lPatientPreMedicalHistory = new PatientPreMedicalHistory();
                                lPatientPreMedicalHistory.CompanyId = Global.Company.CompanyId;
                                lPatientPreMedicalHistory.PatientId = PatientId;
                                lPatientPreMedicalHistory.HistoryItemId = ((PatientHistoryQuestion)GridViewPatientHistory.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex + 1].Value).Id;
                                if (((PatientHistoryQuestion)GridViewPatientHistory.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex + 1].Value).AdditionalNotes)
                                {
                                    if (GridViewPatientHistory.Rows[Cell.RowIndex + 1].Cells[Cell.ColumnIndex + 1] != null && GridViewPatientHistory.Rows[Cell.RowIndex + 1].Cells[Cell.ColumnIndex + 1].Value != null)
                                    {
                                        lPatientPreMedicalHistory.AdditionalValue = GridViewPatientHistory.Rows[Cell.RowIndex + 1].Cells[Cell.ColumnIndex + 1].Value.ToString();
                                    }
                                }
                                Patient.History.Add(lPatientPreMedicalHistory);
                            }
                        }
                    }
                }
                PatientMedicalHistoryManager.Instance.AddPatientPreMedicalHistory(Patient);
            }
            LoadMedicalHistory();
            LoadMedicalHistoryData();
            LockAllGridViewCells();
            UnLockGridViewSelectedCells();
            //if (loadChart)
            //{
            //    LoadPatientChart();
            //}
            ConsultedNoteErrMsg.Text = "Saved.";
            this.formIsDirty = false;
            Cursor.Current = Cursors.Default;
        }

        private void GridViewPatientHistory_Enter(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = false;
            if (!PreventGridViewPatientHistoryEnterEvent)
            {
                GridViewPatientHistory.Focus();
                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[1].Cells[1];
                GridViewPatientHistory.Select();
            }
            PreventGridViewPatientHistoryEnterEvent = false;
        }

        private void GridViewPatientHistory_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is System.Windows.Forms.TextBox textBox)
            {
                textBox.KeyPress -= GridViewPatientHistory_KeyPress!;
                textBox.KeyPress += GridViewPatientHistory_KeyPress!;
                textBox.AcceptsTab = true;
            }
        }

        private void GridViewPatientHistory_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void LoadPatientChart()
        {
            Cursor.Current = Cursors.WaitCursor;
            string Extension = ".pdf";
            List<string> Pages = new List<string>();
            Pages.Add("Prescription History");
            Pages.Add("LabTest History");
            MemoryStream Stream = new MemoryStream();
            PictureBoxMedicalHis.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] CheckedImg = Stream.ToArray();

            Stream = new MemoryStream();
            PictureBoxMedHisUnChecked.Image.Save(Stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] UnCheckedImg = Stream.ToArray();

            PatientChartPrinting PatientChartPrinting = new PatientChartPrinting()
            {
                PatientId = parent != null ? long.Parse(parent.PatientIdTransport.Text) : long.Parse(PatientIdTransport.Text),
                Pages = Pages,
                CheckedImg = CheckedImg,
                UnCheckedImg = UnCheckedImg,
                IncludeFullChart = true,
                IsConsuting = true
            };
            PatientChartPrinting.GenerateChart();

            PatientChartFileName = PatientChartPrinting.ConsultingFileName!;
            PatientChartFilePath = PatientChartPrinting.ConsultingFilePath!;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(PatientChartFileName);

            DocFile = ReadImageFile(PatientChartFilePath + PatientChartFileName);

            byte[] Attachment = (byte[])DocFile;

            PatientChartDocBrowser.LoadDocument("about:blank");
            pdfDocumentView2.Refresh();
            pdfDocumentView2.Visible = false;
            PatientChartDocBrowser.Visible = false;

            try
            {
                if (File.Exists(Path.Combine(PatientChartFilePath, fileNameWithoutExtension + Extension)))
                {
                    try
                    {
                        File.Delete(Path.Combine(PatientChartFilePath, fileNameWithoutExtension + Extension));
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.HResult);
                        for (int i = 1; i < 1000; i++)
                        {
                            fileNameWithoutExtension = fileNameWithoutExtension + i.ToString();
                            if (!File.Exists(Path.Combine(PatientChartFilePath, fileNameWithoutExtension + Extension)))
                            {
                                break;
                            }
                        }
                    }
                }
                FileStream stream = new FileStream((PatientChartFilePath + fileNameWithoutExtension + Extension), FileMode.CreateNew);
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(Attachment, 0, Attachment.Length);
                writer.Close();

                pdfDocumentView2.Visible = true;
                pdfDocumentView2.Load(PatientChartFilePath + fileNameWithoutExtension + Extension);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.HResult);
            }
        }
        public static byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null!;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }

        private void GridViewPatientHistory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (GridViewPatientHistory.Rows[e.RowIndex].Cells[0].Value != null && e.RowIndex > -1)
            {
                System.Drawing.Rectangle rowBounds = new System.Drawing.Rectangle(
            0, e.RowBounds.Top,
            this.GridViewPatientHistory.Columns.GetColumnsWidth(
                DataGridViewElementStates.Visible) -
            this.GridViewPatientHistory.HorizontalScrollingOffset,
            e.RowBounds.Height);
                System.Drawing.Font drawFont = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
                string rr = GridViewPatientHistory.Rows[e.RowIndex].Cells[9].Value.ToString()!;
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                e.Graphics.DrawString(rr, drawFont, drawBrush, rowBounds);
            }
        }

        private void GridViewPatientHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void BtnMedicalHistoryCancel_Click(object sender, EventArgs e)
        {
            LoadMedicalHistory();
            LoadMedicalHistoryData();
            LockAllGridViewCells();
            UnLockGridViewSelectedCells();
            //LockGridViewCells();
            ConsultedNoteErrMsg.Text = string.Empty;
        }

        private void FormConsulting_Click(object sender, EventArgs e)
        {
            HidePopupListView();
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                isKeyboardInput = false;
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    isKeyboardInput = false;
                }
                else
                {
                    isKeyboardInput = true;
                }
            }

            if (e.KeyCode == Keys.Down && popupListView.Visible && popupListView.Items.Count > 0)
            {
                popupListView.Focus();
                popupListView.Items[0].Selected = true;
                e.Handled = true;
            }
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (GridViewNote.CurrentCell.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                string typedText = (sender as System.Windows.Forms.TextBox)!.Text.Trim();

                if (isKeyboardInput)
                {
                    // Restart the timer every time the text changes
                    searchDelayTimer.Stop();
                    if (typedText.Length > 2)
                    {
                        searchDelayTimer.Start();
                    }
                    else
                    {
                        HidePopupListView();
                    }
                }
                else
                {
                    HidePopupListView();
                }
                isKeyboardInput = false;
            }
        }

        private void ShowPopupListView()
        {
            var cellRectangle = GridViewNote.GetCellDisplayRectangle(GridViewNote.CurrentCell.ColumnIndex, GridViewNote.CurrentCell.RowIndex, true);
            var column3Rect = GridViewNote.GetColumnDisplayRectangle((int)ConsultationNotesGridColumn.CONSULTATIONNOTES, true);
            popupListView.Location = new Point(column3Rect.X + GridViewNote.Location.X + 207, cellRectangle.Bottom + GridViewNote.Location.Y + 27);

            popupListView.Width = GridViewNote.Columns[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES].Width;
            popupListView.Visible = true;
            popupListView.BringToFront();
        }

        private void HidePopupListView()
        {
            if (popupListView.Visible)
            {
                isKeyboardInput = false;
                popupListView.Visible = false;
            }
        }

        private void PopupListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && popupListView.SelectedIndices.Count == 0)
            {
                GridViewNote.Focus();
                GridViewNote.CurrentCell = GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[2];
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && popupListView.SelectedItems.Count > 0)
            {
                HandlePopupListViewSelection();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;

                if (popupListView.SelectedItems.Count > 0)
                {
                    var selectedItemText = popupListView.SelectedItems[0].Text;

                    InsertTextIntoGridViewNoteColumn(selectedItemText, 2);
                }
                else
                {
                    HandleTabKeyForGridViewNote();
                }
            }
        }
        private void PopupListView_DoubleClick(object sender, EventArgs e)
        {
            HandlePopupListViewSelection();
        }
        private void PopulateAutoCompleteList()
        {
            IList<ConsultationNote> lConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByCompanyId(Global.Company.CompanyId);
            if (lConsultationNote != null)
            {
                foreach (ConsultationNote consultationNote in lConsultationNote)
                {
                    if (consultationNote != null && !string.IsNullOrEmpty(consultationNote.Note))
                    {
                        autoCompleteList.Add(consultationNote.Note);
                    }
                }
            }
        }

        private void GridViewNote_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (GridViewNote.CurrentCell != null && GridViewNote.CurrentCell.ColumnIndex == 2 && e.KeyCode == Keys.Down)
            {
                e.IsInputKey = true;
            }
        }
        private void HandlePopupListViewSelection()
        {
            if (popupListView.SelectedItems.Count > 0)
            {
                var selectedItem = popupListView.SelectedItems[0];
                if (selectedItem != null)
                {
                    GridViewNote.CurrentCell.Value = selectedItem.Text;

                    GridViewNote.Focus();
                    GridViewNote.CurrentCell = GridViewNote.Rows[GridViewNote.CurrentCell.RowIndex].Cells[2];
                    GridViewNote.BeginEdit(true);

                    if (GridViewNote.EditingControl is System.Windows.Forms.TextBox textBox)
                    {
                        textBox.SelectionStart = textBox.Text.Length;
                        textBox.SelectionLength = 0;
                    }

                    HidePopupListView();
                }
            }
            else
            {
                HidePopupListView();
            }
        }

        private void HandleTabKeyForGridViewNote()
        {
            int maxColumnIndex = GridViewNote.ColumnCount - 1;

            HidePopupListView();
            int currentColumnIndex = GridViewNote.CurrentCell.ColumnIndex;
            int currentRowIndex = GridViewNote.CurrentCell.RowIndex;
            int newColumnIndex;

            if (GridViewNote.ContainsFocus)
            {
                if (currentColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES + 1)
                {
                    if (popupListView.Visible == false)
                    {
                        newColumnIndex = 4;
                    }
                    else
                    {
                        newColumnIndex = 2;
                    }
                }
                else
                {
                    newColumnIndex = currentColumnIndex + 2;
                    if (currentColumnIndex == 1)
                    {
                        newColumnIndex++;
                    }
                    else if (currentColumnIndex == 10)
                    {
                        newColumnIndex++;
                    }
                    else if (currentColumnIndex == 13)
                    {
                        newColumnIndex--;
                    }
                }

                if (newColumnIndex <= 14)
                {
                    if (newColumnIndex >= 0 && newColumnIndex < GridViewNote.Columns.Count && GridViewNote.Columns[newColumnIndex].Visible)
                    {
                        if (currentColumnIndex == (int)ConsultationNotesGridColumn.PRESCRIPTION ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.PROCEDURE ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.SYMPTOM ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.LABTEST ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.FEE)
                        {
                            GridViewNote.CurrentCell = GridViewNote.Rows[currentRowIndex].Cells[currentColumnIndex + 1];
                        }
                        else
                        {
                            GridViewNote.CurrentCell = GridViewNote.Rows[currentRowIndex].Cells[newColumnIndex];
                        }
                    }
                    else
                    {
                        if (currentColumnIndex == (int)ConsultationNotesGridColumn.PRESCRIPTION ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.PROCEDURE ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.SYMPTOM ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.LABTEST ||
                            currentColumnIndex == (int)ConsultationNotesGridColumn.FEE)
                        {
                            GridViewNote.CurrentCell = GridViewNote.Rows[currentRowIndex].Cells[currentColumnIndex + 1];
                        }
                    }
                }
                else
                {
                    int nextRowIndex = currentRowIndex + 1;
                    if (nextRowIndex < GridViewNote.RowCount)
                    {
                        GridViewNote.CurrentCell = GridViewNote.Rows[nextRowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES];
                    }
                    else
                    {
                        if (!IsColumnEmpty(GridViewNote.Rows[currentRowIndex], (int)ConsultationNotesGridColumn.CONSULTATIONNOTES))
                        {
                            GridViewNote.Rows.Add();
                            GridViewNote.CurrentCell = GridViewNote.Rows[nextRowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES];
                        }
                        else
                        {
                            if (nextRowIndex == GridViewNote.RowCount)
                            {
                                BtnNotesSave.Select();
                            }
                            else
                            {
                                GridViewNote.Select();
                                GridViewNote.BeginInvoke(new MethodInvoker(delegate ()
                                {
                                    GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, currentRowIndex];
                                }));
                            }

                        }
                    }
                }
            }
            else
            {
                HandleButtonKeyForGridViewNote(false);
            }
        }
        private void HandleShiftTabKeyForGridViewNote()
        {
            int maxColumnIndex = GridViewNote.ColumnCount - 1;
            isKeyboardInput = false;
            HidePopupListView();

            int currentColumnIndex = GridViewNote.CurrentCell.ColumnIndex;
            int currentRowIndex = GridViewNote.CurrentCell.RowIndex;
            int newColumnIndex;

            if (GridViewNote.ContainsFocus)
            {
                if (currentColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
                {
                    newColumnIndex = 14;
                }
                else
                {
                    newColumnIndex = currentColumnIndex - 2;
                    if (currentColumnIndex == 14)
                    {
                        newColumnIndex++; // Adjust for column 14
                    }
                    else if (currentColumnIndex == 13)
                    {
                        newColumnIndex--; // Adjust for column 13
                    }
                }
                if (currentColumnIndex == 2)
                {
                    if (currentRowIndex > 0)
                    {
                        GridViewNote.CurrentCell = GridViewNote.Rows[currentRowIndex - 1].Cells[newColumnIndex];
                    }
                }
                else
                {
                    if (newColumnIndex > 1)
                    {
                        GridViewNote.CurrentCell = GridViewNote.Rows[currentRowIndex].Cells[newColumnIndex];
                    }
                    else if (newColumnIndex == 1)
                    {
                        GridViewNote.CurrentCell = GridViewNote.Rows[currentRowIndex].Cells[2];
                    }
                    else
                    {
                        int prevRowIndex = currentRowIndex - 1;
                        if (prevRowIndex >= 0)
                        {
                            GridViewNote.CurrentCell = GridViewNote.Rows[prevRowIndex].Cells[14];
                        }
                        else
                        {
                            // If there is no previous row, handle accordingly (e.g., stay in the same cell, show an error, etc.)
                            if (!IsColumnEmpty(GridViewNote.Rows[currentRowIndex], (int)ConsultationNotesGridColumn.CONSULTATIONNOTES))
                            {
                                //currentRowIndex = 1;
                                if (currentRowIndex == 0)
                                {
                                    MoveToTab(7, PatientChatTab);
                                }
                                else
                                {
                                    GridViewNote.CurrentCell = GridViewNote.Rows[currentRowIndex].Cells[maxColumnIndex];
                                }
                            }
                            else
                            {
                                GridViewNote.Select();
                                GridViewNote.BeginInvoke(new MethodInvoker(delegate ()
                                {
                                    GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES, currentRowIndex];
                                }));
                            }
                        }
                    }
                }
            }
            else
            {
                HandleButtonKeyForGridViewNote(true);
            }
        }
        private void HandleButtonKeyForGridViewNote(bool isShiftTab)
        {
            int maxColumnIndex = GridViewNote.ColumnCount - 1;
            int LastRowIndex = GridViewNote.RowCount - 1;
            if (!isShiftTab)
            {
                if (ActiveControl == BtnNotesSave)
                {
                    BtnNoteCancel.Select();
                }
                else if (ActiveControl == BtnNoteCancel)
                {
                    BtnCompleteConsultation.Select();
                }
                else if (ActiveControl == BtnCompleteConsultation)
                {
                    BtnNoteExit.Select();
                }
                else if (ActiveControl == BtnNoteExit)
                {
                    IsPatientPrescriptionFocused = true;
                    GridViewPrescriptionHistory.RowSelection = true;
                    GridViewPrescriptionHistory.SelectedRowIndex = 0;
                    MoveToTab(1, MedicationTab);
                    MedicationTab.Select();
                    MedicationTab.Focus();
                }
                else
                {
                    BtnNotesSave.Select();
                }
            }
            else
            {
                if (ActiveControl == BtnNoteExit)
                {
                    BtnCompleteConsultation.Select();
                }
                else if (ActiveControl == BtnCompleteConsultation)
                {
                    BtnNoteCancel.Select();
                }
                else if (ActiveControl == BtnNoteCancel)
                {
                    BtnNotesSave.Select();
                }
                else if (ActiveControl == BtnNotesSave)
                {
                    GridViewNote.Select();
                    GridViewNote.BeginInvoke(new MethodInvoker(delegate ()
                    {
                        GridViewNote.CurrentCell = GridViewNote[(int)ConsultationNotesGridColumn.REMOVE, LastRowIndex];
                    }));
                }
                else
                {
                    BtnNoteExit.Select();
                }
            }
        }

        private bool IsColumnEmpty(DataGridViewRow row, int columnIndex)
        {
            var cellValue = row.Cells[columnIndex].Value;
            return cellValue == null || string.IsNullOrEmpty(cellValue.ToString());
        }

        private void HandleEndKey()
        {
            if (GridViewNote.CurrentCell is DataGridViewTextBoxCell)
            {
                GridViewNote.BeginEdit(true);

                if (GridViewNote.EditingControl is System.Windows.Forms.TextBox textBox)
                {
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectionLength = 0;
                }
            }
        }
        private void FocusGridViewNoteColumn(int columnIndex)
        {
            if (GridViewNote.CurrentRow != null)
            {
                HidePopupListView();
                GridViewNote.CurrentCell = GridViewNote.CurrentRow.Cells[columnIndex];
                GridViewNote.BeginEdit(true);
                GridViewNote.Focus();
            }
        }

        private void InsertTextIntoGridViewNoteColumn(string text, int columnIndex)
        {
            if (GridViewNote.CurrentRow != null)
            {
                GridViewNote.CurrentRow.Cells[columnIndex].Value = text;
                FocusGridViewNoteColumn(4);
            }
        }

        private void LoadSuggestionNoteAndUpdatePopup(string typedText)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DataTable searchResults = ConsultationNoteManager.Instance.ListNotesSuggesionsByCompanyId(Global.Company.CompanyId, typedText);

                if (searchResults != null && searchResults.Rows.Count > 0)
                {
                    UpdatePopupListView(searchResults);
                }
                else
                {
                    HidePopupListView();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                HidePopupListView();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void UpdatePopupListView(DataTable searchResults)
        {
            popupListView.Items.Clear();

            foreach (DataRow row in searchResults.Rows)
            {
                var item = new ListViewItem(row["Note"].ToString());
                popupListView.Items.Add(item);
            }

            if (popupListView.Items.Count > 0)
            {

                ShowPopupListView();
            }
            else
            {
                HidePopupListView();
            }
        }
        private void GridViewNote_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var row = GridViewNote.Rows[e.RowIndex];
            var preferredHeight = row.GetPreferredHeight(e.RowIndex, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true);

            if (row.Height != preferredHeight)
            {
                row.Height = preferredHeight;
            }
        }

        private void FormConsulting_MouseClick(object sender, MouseEventArgs e)
        {
            HidePopupListView();
        }

        private void GridViewNote_MouseClick(object sender, MouseEventArgs e)
        {
            var hitNoteColumnInfo = GridViewNote.HitTest(e.X, e.Y);

            if (hitNoteColumnInfo.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES)
            {
                HandleSecondColumnFunctionality(hitNoteColumnInfo.RowIndex);
            }
            else
            {
                HidePopupListView();
            }
        }
        private void PopupListView_MouseClick(object sender, MouseEventArgs e)
        {
            // popupListView.Visible = true;
            // Do nothing to prevent hiding the popupListView
        }
        private void HandleSecondColumnFunctionality(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < GridViewNote.Rows.Count)
            {
                GridViewNote.CurrentCell = GridViewNote.Rows[rowIndex].Cells[(int)ConsultationNotesGridColumn.CONSULTATIONNOTES];
                GridViewNote.BeginEdit(true);
            }
        }
        private void FormConsulting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show("There are unsaved changes, Do you want to Exit?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void TabControlConsult_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConsultedNoteErrMsg.Text = string.Empty;
            if (TabControlConsult.SelectedTab != ConsultNotesTab)
            {
                HidePopupListView();
            }
            if (TabControlConsult.SelectedTab == PatientChatTab)
            {
                LoadPatientChart();
            }
        }
        private void SetTagForSpecificCells()
        {
            var specificCells = new List<(int RowIndex, int ColumnIndex)>
            {
                (21, 4),
                (15, 4),
                (15, 2)
            };

            foreach (var (rowIndex, columnIndex) in specificCells)
            {
                if (rowIndex < GridViewPatientHistory.RowCount && columnIndex < GridViewPatientHistory.ColumnCount)
                {
                    GridViewPatientHistory.Rows[rowIndex].Cells[columnIndex].Tag = true;
                }
            }
        }
        private void HandleTabKeyForPatientMedicalHistory()
        {
            SetTagForSpecificCells();

            cellTagDictionary = CreateCellTagDictionary();
            if (BtnMedicalHistorySave.ContainsFocus || BtnMedicalHistoryCancel.ContainsFocus)
            {
                HandleTabKeyForGridViewPatientHistoryBtn(false);
                return; // Exit early if one of the buttons has focus
            }
            if (MedicalHistoryTab.Focused)
            {
                IsMedicalHistoryTabFocused = false;

                GridViewPatientHistory.Focus();
                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[1].Cells[1];
                return;
            }

            int maxColumnIndex = GridViewPatientHistory.ColumnCount - 1;
            int maxRowIndex = GridViewPatientHistory.Rows.Count - 1;

            int currentColumnIndex = GridViewPatientHistory.CurrentCell.ColumnIndex;
            int currentRowIndex = GridViewPatientHistory.CurrentCell.RowIndex;

            int SelectedColumIndex = currentColumnIndex;
            int newColumnIndex = currentColumnIndex + 2; // Skip one column
            int newRowIndex = currentRowIndex;
            int RowIndexNew;

            while (newColumnIndex > maxColumnIndex || !GridViewPatientHistory.Columns[newColumnIndex].Visible)
            {
                newColumnIndex += 2;
                if (newColumnIndex > maxColumnIndex)
                {
                    newColumnIndex = 1;
                    newRowIndex++;

                    if (newRowIndex > maxRowIndex)
                    {
                        TextBoxOtherNotes.Select();
                        return;
                    }
                }
            }

            if (currentColumnIndex == 0 || currentColumnIndex == 2)
            {
                newColumnIndex = currentColumnIndex + 2;
                if (newColumnIndex > maxColumnIndex)
                {
                    newColumnIndex = 1;
                    newRowIndex++;
                    if (newRowIndex > maxRowIndex)
                    {
                        TextBoxOtherNotes.Select();
                        return;
                    }
                }
                else if (currentColumnIndex > 0 &&
                        GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex - 1] is DataGridViewCheckBoxCell prevCheckBoxCell &&
                        prevCheckBoxCell.Value is bool prevChecked && prevChecked &&
                        cellTagDictionary.TryGetValue((newRowIndex + 1, currentColumnIndex), out bool nextTagValue) && nextTagValue)
                {
                    if (newRowIndex + 1 <= maxRowIndex)
                    {
                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex + 1].Cells[currentColumnIndex];
                    }
                }
                else if ((currentColumnIndex > 0 && cellTagDictionary.TryGetValue((newRowIndex, currentColumnIndex), out bool CurrentTagValue) && CurrentTagValue))
                {
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex - 1].Cells[currentColumnIndex + 1];
                }
                else if (currentColumnIndex > 0 && GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex + 1] is DataGridViewCheckBoxCell)
                {
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex].Cells[currentColumnIndex + 1];
                }
                else
                {
                    if (currentRowIndex == GridViewPatientHistory.Rows.Count - 1 &&
                        currentColumnIndex == GridViewPatientHistory.Columns.Count - 1)
                    {
                        TextBoxOtherNotes.Select();
                    }
                    else
                    {
                        bool checkboxFound = false;

                        for (int col = currentColumnIndex + 1; col < GridViewPatientHistory.Columns.Count; col++)
                        {
                            if (GridViewPatientHistory.Rows[currentRowIndex].Cells[col] is DataGridViewCheckBoxCell)
                            {
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[col];
                                checkboxFound = true;
                                break;
                            }
                        }

                        if (!checkboxFound)
                        {
                            for (int row = currentRowIndex + 1; row < GridViewPatientHistory.Rows.Count; row++)
                            {
                                for (int col = 0; col < GridViewPatientHistory.Columns.Count; col++)
                                {
                                    if (GridViewPatientHistory.Rows[row].Cells[col] is DataGridViewCheckBoxCell)
                                    {
                                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[row].Cells[col];
                                        checkboxFound = true;
                                        break;
                                    }
                                }
                                if (checkboxFound) break;
                            }
                        }
                    }
                }

            }
            else if (GridViewPatientHistory.CurrentCell is DataGridViewCheckBoxCell checkBoxCell && checkBoxCell.Value is bool isChecked && isChecked &&
            cellTagDictionary.TryGetValue((newRowIndex + 1, currentColumnIndex + 1), out bool tagValue) && tagValue)
            {
                if (newRowIndex + 1 <= maxRowIndex && currentColumnIndex + 1 <= maxColumnIndex)
                {
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex + 1].Cells[currentColumnIndex + 1];
                }
            }
            else if (currentColumnIndex > 0 &&
                        GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex - 1] is DataGridViewCheckBoxCell prevCheckBoxCell &&
                        prevCheckBoxCell.Value is bool prevChecked && prevChecked &&
                        cellTagDictionary.TryGetValue((newRowIndex + 1, currentColumnIndex), out bool nextTagValue) && nextTagValue)
            {
                if (newRowIndex + 1 <= maxRowIndex)
                {
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex + 1].Cells[currentColumnIndex];
                }
            }
            else
            {
                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex].Cells[newColumnIndex];

                if (GridViewPatientHistory.CurrentCell.GetType() == typeof(DataGridViewTextBoxCell))
                {
                    if (currentColumnIndex != 0)
                    {
                        if (currentRowIndex == 0) { RowIndexNew = 1; } else { RowIndexNew = currentRowIndex; }
                        if (GridViewPatientHistory.Rows[RowIndexNew - 1].Cells[currentColumnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
                        {
                            if (RowIndexNew + 1 <= maxRowIndex && GridViewPatientHistory.Rows[RowIndexNew + 1].Cells[0].Value == null)
                            {
                                if (newRowIndex == currentRowIndex && currentColumnIndex == 5)
                                {
                                    TextBoxOtherNotes.Select();
                                    return;
                                }
                                else
                                {
                                    if (ActiveControl == GridViewPatientHistory)
                                    {
                                        GridViewPatientHistory.Focus();
                                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex + 3].Cells[1];
                                    }
                                    else if (ActiveControl == TextBoxOtherNotes)
                                    {
                                        BtnMedicalHistorySave.Select();
                                    }
                                    else if (ActiveControl == BtnMedicalHistorySave)
                                    {
                                        BtnMedicalHistoryCancel.Select();
                                    }
                                    else if (ActiveControl == BtnMedicalHistoryCancel)
                                    {
                                        MoveToTab(7, PatientChatTab);
                                    }
                                }
                            }
                            else
                            {
                                GridViewPatientHistory.Focus();
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex + 2].Cells[1];
                            }
                        }
                        else if ((currentColumnIndex > 0 && cellTagDictionary.TryGetValue((newRowIndex, currentColumnIndex), out bool CurrentTagValue) && CurrentTagValue))
                        {
                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex - 1].Cells[currentColumnIndex + 1];
                        }
                        else
                        {
                            bool checkBoxFoundInSameRow = false;
                            for (int b = SelectedColumIndex + 1; b <= maxColumnIndex; b++)
                            {
                                if (GridViewPatientHistory.Rows[currentRowIndex].Cells[b] is DataGridViewCheckBoxCell)
                                {
                                    newColumnIndex = b;
                                    checkBoxFoundInSameRow = true;
                                    break;
                                }
                            }
                            if (checkBoxFoundInSameRow)
                            {
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[newColumnIndex];
                            }
                            else
                            {
                                bool checkBoxFoundInNextRows = false;
                                for (int j = currentRowIndex + 1; j <= maxRowIndex; j++)
                                {
                                    for (int k = 0; k <= maxColumnIndex; k++)
                                    {
                                        if (GridViewPatientHistory.Rows[j].Cells[k] is DataGridViewCheckBoxCell)
                                        {
                                            newColumnIndex = k;
                                            newRowIndex = j;
                                            checkBoxFoundInNextRows = true;
                                            break;
                                        }
                                    }
                                    if (checkBoxFoundInNextRows)
                                    {
                                        break;
                                    }
                                }
                                if (checkBoxFoundInNextRows)
                                {
                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex].Cells[newColumnIndex];
                                }
                                else
                                {
                                    for (int j = RowIndexNew - 1; j < GridViewPatientHistory.Rows.Count; j++)
                                    {
                                        for (int k = currentColumnIndex; k < GridViewPatientHistory.Columns.Count - 2; k++)
                                        {
                                            if (GridViewPatientHistory.Rows[j].Cells[k].GetType() == typeof(DataGridViewCheckBoxCell))
                                            {
                                                GridViewPatientHistory.Focus();
                                                if (currentColumnIndex == 0 || currentColumnIndex == 2)
                                                {
                                                    if (currentRowIndex + 1 <= maxRowIndex && GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[0].Value == null)
                                                    {
                                                        if (GridViewPatientHistory.Rows[j].Cells[0].GetType() == typeof(DataGridViewCheckBoxCell))
                                                        {
                                                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j + 1].Cells[currentColumnIndex + 1];
                                                        }
                                                        else
                                                        {
                                                            bool checkBoxFoundInRow = false;
                                                            for (int i = SelectedColumIndex + 1; i <= maxColumnIndex; i++)
                                                            {
                                                                if (GridViewPatientHistory.Rows[currentRowIndex].Cells[i] is DataGridViewCheckBoxCell)
                                                                {
                                                                    newColumnIndex = i;
                                                                    checkBoxFoundInRow = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (checkBoxFoundInRow)
                                                            {
                                                                if (GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[0].Value == null && GridViewPatientHistory.Rows[currentRowIndex + 2].Cells[0].Value == null)
                                                                {
                                                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex].Cells[newColumnIndex];
                                                                }
                                                                else if (GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[0].Value == null)
                                                                {
                                                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex].Cells[newColumnIndex];
                                                                }
                                                                else
                                                                {
                                                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j + 2].Cells[currentColumnIndex + 1];
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[0].Value == null && GridViewPatientHistory.Rows[currentRowIndex + 2].Cells[0].Value == null)
                                                                {
                                                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex + 1].Cells[currentColumnIndex + 1];
                                                                }
                                                                else if (GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[0].Value == null)
                                                                {
                                                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex + 3].Cells[currentColumnIndex + 1];
                                                                }
                                                                else
                                                                {
                                                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j + 2].Cells[currentColumnIndex + 1];
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j + 2].Cells[currentColumnIndex + 2];
                                                    }
                                                }
                                                else
                                                {
                                                    bool checkBoxFoundInRow = false;
                                                    for (int i = SelectedColumIndex + 1; i <= maxColumnIndex; i++)
                                                    {
                                                        if (GridViewPatientHistory.Rows[currentRowIndex].Cells[i] is DataGridViewCheckBoxCell)
                                                        {
                                                            newColumnIndex = i;
                                                            checkBoxFoundInRow = true;
                                                            break;
                                                        }
                                                    }
                                                    if (checkBoxFoundInRow)
                                                    {
                                                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex].Cells[newColumnIndex];
                                                    }
                                                    else
                                                    {
                                                        bool checkBoxFoundInNextNewRows = false;
                                                        for (int nri = currentRowIndex + 1; nri <= maxRowIndex; nri++)
                                                        {
                                                            for (int nci = 0; nci <= maxColumnIndex; nci++)
                                                            {
                                                                if (GridViewPatientHistory.Rows[nri].Cells[nci] is DataGridViewCheckBoxCell)
                                                                {
                                                                    newColumnIndex = nci;
                                                                    newRowIndex = nri;
                                                                    checkBoxFoundInNextNewRows = true;
                                                                    break;
                                                                }
                                                            }
                                                            if (checkBoxFoundInNextNewRows)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                        if (checkBoxFoundInNextNewRows)
                                                        {
                                                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[newRowIndex].Cells[newColumnIndex];
                                                        }
                                                        else
                                                        {
                                                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j + 1].Cells[0];
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void HandleShiftTabKeyForPatientMedicalHistory()
        {
            if (ActiveControl == BtnMedicalHistorySave || ActiveControl == BtnMedicalHistoryCancel)
            {
                HandleTabKeyForGridViewPatientHistoryBtn(true);
            }
            else if (ActiveControl == TextBoxOtherNotes)
            {
                if (GridViewPatientHistory.RowCount > 0)
                {
                    DataGridViewCheckBoxCell lastCheckBoxCell = null!;
                    int lastCheckBoxColumnIndex = -1;

                    for (int rowIndex = GridViewPatientHistory.RowCount - 1; rowIndex >= 0; rowIndex--)
                    {
                        for (int colIndex = GridViewPatientHistory.ColumnCount - 1; colIndex >= 0; colIndex--)
                        {
                            if (GridViewPatientHistory.Rows[rowIndex].Cells[colIndex] is DataGridViewCheckBoxCell)
                            {
                                lastCheckBoxCell = (DataGridViewCheckBoxCell)GridViewPatientHistory.Rows[rowIndex].Cells[colIndex];
                                lastCheckBoxColumnIndex = colIndex;
                                break;
                            }
                        }

                        if (lastCheckBoxCell != null && lastCheckBoxColumnIndex != -1)
                        {
                            PreventGridViewPatientHistoryEnterEvent = true;
                            GridViewPatientHistory.CurrentCell = lastCheckBoxCell;
                            GridViewPatientHistory.Focus();
                            break;
                        }
                    }
                }
            }
            else
            {
                int currentColumnIndex = GridViewPatientHistory.CurrentCell.ColumnIndex;
                int currentRowIndex = GridViewPatientHistory.CurrentCell.RowIndex;
                // Check if the current control is TextBoxOtherNotes

                if (ActiveControl is System.Windows.Forms.TextBox)
                {
                    // Move focus to the last DataGridViewCheckBoxCell in GridViewPatientHistory
                    if ((currentColumnIndex > 0 && cellTagDictionary.TryGetValue((currentRowIndex, currentColumnIndex), out bool CurrentTagValue) && CurrentTagValue))
                    {
                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex - 1].Cells[currentColumnIndex - 1];
                    }
                    else
                    {
                        MoveToLastCheckBoxCellInGridView();
                    }
                    return;
                }

                // Ensure the DataGridView has a selected cell
                if (GridViewPatientHistory.CurrentCell == null)
                {
                    // If no cell is selected, focus on the first checkbox cell
                    SelectFirstCheckBoxOrTextBox();
                    return;
                }

                // Check if the current cell is in the first row and first column
                if (currentRowIndex == 1 && currentColumnIndex == 1)
                {
                    // Move to MedicationTab if on the first row and first column
                    IsMedicalHistoryTabFocused = true;
                    MoveToTab(6, MedicalHistoryTab);
                    MedicalHistoryTab.Focus();
                    return;
                }

                // Move to the previous column with DataGridViewCheckBoxCell type
                while (true)
                {
                    for (int columnIndex = currentColumnIndex - 1; columnIndex >= 0; columnIndex--)
                    {
                        if (GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex] is DataGridViewCheckBoxCell)
                        {
                            if (((currentColumnIndex == 3 && currentRowIndex == 14)) && (GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[currentColumnIndex - 1].ReadOnly != true)
                              || ((currentColumnIndex == 5 && currentRowIndex == 20)) && (GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[currentColumnIndex - 1].ReadOnly != true)
                              || ((currentColumnIndex == 5 && currentRowIndex == 14)) && (GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[currentColumnIndex - 1].ReadOnly != true))
                            {
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex + 1].Cells[currentColumnIndex - 1];
                            }
                            else
                            {
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex];
                            }
                            return;
                        }
                    }

                    // Move to the previous row if no DataGridViewCheckBoxCell type is found in the current row
                    currentRowIndex--;

                    // If the row index is less than 0, move to MedicationTab
                    if (currentRowIndex < 0)
                    {
                        IsMedicalHistoryTabFocused = true;
                        MoveToTab(6, MedicalHistoryTab);
                        MedicalHistoryTab.Focus();
                        return;
                    }

                    // Move to the last DataGridViewCheckBoxCell in the previous row
                    currentColumnIndex = GridViewPatientHistory.ColumnCount - 1;
                    for (int columnIndex = currentColumnIndex; columnIndex >= 0; columnIndex--)
                    {
                        if (GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex] is DataGridViewCheckBoxCell)
                        {
                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex];
                            return;
                        }
                    }
                }
            }

        }

        private void MoveToLastCheckBoxCellInGridView()
        {
            int maxRowIndex = GridViewPatientHistory.Rows.Count - 1;

            // Iterate through the last row to find the last DataGridViewCheckBoxCell
            for (int columnIndex = GridViewPatientHistory.Columns.Count - 1; columnIndex >= 0; columnIndex--)
            {
                if (GridViewPatientHistory.Rows[maxRowIndex].Cells[columnIndex] is DataGridViewCheckBoxCell)
                {
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[maxRowIndex].Cells[columnIndex];
                    return;
                }
            }
        }

        private void SelectFirstCheckBoxOrTextBox()
        {
            // Search for the first DataGridViewCheckBoxCell in the first row
            foreach (DataGridViewRow row in GridViewPatientHistory.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell is DataGridViewCheckBoxCell)
                    {
                        GridViewPatientHistory.CurrentCell = cell;
                        return;
                    }
                }
            }
            // If no checkbox cell is found, focus on TextBoxOtherNotes
            TextBoxOtherNotes.Select();
        }

        private void GridViewPatientHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Shift)
            {
                HandleShiftTabKeyForPatientMedicalHistory();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                HandleTabKeyForPatientMedicalHistory();
                e.Handled = true;
            }
        }

        private void GridViewPatientHistory_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //if (e.KeyCode == Keys.Down)
            //{
            //   // e.IsInputKey = true;
            //}
        }
        public int rowCount = 0;
        private void HandleTabControlTab(Keys keyData)
        {
            switch (TabControlConsult.SelectedIndex)
            {
                case 0:
                    if (ConsultNotesTab.Focused || ConsultNotesTab.ContainsFocus)
                    {
                        HandleTabKeyForGridViewNote();
                    }
                    else
                    {
                        if (GridViewNote.Rows.Count > 0)
                        {
                            ConsultNotesTab.Focus();
                            GridViewNote.Focus();
                            GridViewNote.CurrentCell = GridViewNote[2, 0];
                            GridViewNote.BeginEdit(true);
                        }
                        else
                        {
                            IsPatientPrescriptionFocused = true;
                            MoveToTab(1, MedicationTab);
                            MedicationTab.Focus();
                        }
                    }
                    break;
                case 1:
                    if (keyData == Keys.Tab)
                    {
                        if (MedicationTab.Focused && MedicationTab.ContainsFocus)
                        {
                            if (PrescriptionHistoryRowCount > 0)
                            {
                                GridViewPrescriptionHistory.Focus();
                                if (GridViewPrescriptionHistory.Focused)
                                {
                                    MedicationTabSelected = false;
                                    return;
                                }
                            }
                            else
                            {
                                PatientLabTestSelected = true;
                                MoveToTab(2, LabTab);
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    LabTab.Select();
                                    LabTab.Focus();
                                });
                            }
                        }
                        else if (IsPatientPrescriptionFocused == true)
                        {
                            if (GridViewPrescriptionHistory.ContainsFocus)
                            {
                                if (PrescriptionHistoryRowCount > 0)
                                {
                                    IsPatientPrescriptionFocused = false;
                                    MedicationTabSelected = false;
                                    GridViewPrescriptionHistory.Focus();
                                    HandleTabKeyForGridViewPrescriptionHistory();
                                }
                            }
                            else if (GridViewPrescriptionDetail.ContainsFocus)
                            {
                                HandleTabKeyForGridViewPrescriptionDetail(false);
                            }
                            else
                            {
                                HandleTabKeyForGridViewPrescriptionBtn(false);
                            }
                        }
                        else if (GridViewPrescriptionHistory.ContainsFocus)
                        {
                            HandleTabKeyForGridViewPrescriptionHistory();
                        }
                        else if (GridViewPrescriptionDetail.ContainsFocus)
                        {
                            HandleTabKeyForGridViewPrescriptionDetail(false);
                        }
                        else if (BtnPrescriptionSave.ContainsFocus || BtnPrescriptionCancel.ContainsFocus
                            || BtnPrescriptionPreview.ContainsFocus || BtnPrescriptionPrint.ContainsFocus
                            || BtnPrescriptionSendMedical.ContainsFocus)
                        {
                            HandleTabKeyForGridViewPrescriptionBtn(false); // Normal Tab                            
                        }
                        else
                        {
                            BtnPrescriptionSave.Focus();
                        }
                    }
                    else
                    {
                        PatientLabTestSelected = true;
                        MoveToTab(2, LabTab);
                    }
                    break;
                case 2:
                    if (LabHistoryRowCount > 0)
                    {
                        if (PatientLabTestSelected == true)
                        {
                            LabHistoryIsLastRow = GridViewLabTestHistory.LastRow;
                            if (LabHistoryIsLastRow == true)
                            {
                                GridViewLabTestHistory.LastRow = false;
                                if (GridViewLabTestElementInformation.Rows.Count > 0)
                                {
                                    PatientLabTestSelected = false;
                                    GridViewLabTestElementInformation.CurrentCell = GridViewLabTestElementInformation[0, 0];
                                    GridViewLabTestElementInformation.Focus();
                                }
                                else
                                {
                                    MoveToTab(3, ProcedureTab);
                                    SelectEntireRow(0);
                                }
                            }
                            else
                            {
                                GridViewLabTestHistory.Focus();
                            }
                        }
                        else if (PatientLabTestSelected == false)
                        {
                            HandleTabKeyForGridViewLabTestElementInformationDetail(false);
                        }
                        else
                        {
                            MoveToTab(3, ProcedureTab);
                            SelectEntireRow(0);
                        }
                    }
                    else
                    {
                        MoveToTab(3, ProcedureTab);
                        SelectEntireRow(0);
                    }
                    break;
                case 3:
                    if (ProcedureTab.Focused || ProcedureTab.ContainsFocus)
                    {
                        if (GridViewProcedureInfo.Rows.Count > 0)
                        {
                            if (GridViewProcedureInfoIsLastRow == true)
                            {
                                HandleTabKeyForGridViewProcedureBtn(false);
                            }
                            else
                            {
                                GridViewProcedureInfo.Focus();
                                HandleTabKeyForGridViewProcedureInfo(keyData);
                            }
                        }
                        else
                        {
                            IsPatientVitalEntryIsFocused = false;
                            MoveToTab(4, VitalsTab);
                        }
                    }
                    else
                    {
                        if (GridViewProcedureInfo.Rows.Count > 0)
                        {
                            HandleTabKeyForGridViewProcedureInfo(keyData);
                        }
                        else
                        {
                            IsPatientVitalEntryIsFocused = false;
                            MoveToTab(4, VitalsTab);
                        }
                    }
                    break;
                case 4:
                    if (VitalsTab.Focused || VitalsTab.ContainsFocus)
                    {
                        PatientVitalEntry.PatientId = PatientId;
                        if (PatientVitalEntry.Focused || PatientVitalEntry.ContainsFocus)
                        {
                            // Ensure PatientVitalEntry handles the tab key correctly
                            return;
                        }
                        else
                        {
                            PatientVitalEntry.Focus();
                        }
                    }
                    else if (GridViewProcedureInfoIsLastRow == false)
                    {
                        PatientVitalEntry.PatientId = PatientId;
                        PatientVitalEntry.Focus();
                    }
                    else if (BtnResetVital.Focused)
                    {
                        MoveToTab(5, VisitTab);
                    }
                    break;
                case 5:
                    if (OPVisitRowCount > 0)
                    {
                        OPVisitIsLastRow = ConsultOutPatientVisit.LastRow;
                        if (OPVisitIsLastRow)
                        {
                            if (IPVisitRowCount > 0)
                            {
                                IPVisitIsLastRow = ConsultInPatientVisit.LastRow;
                                if (IPVisitIsLastRow)
                                {
                                    // Reset LastRow flags
                                    ConsultOutPatientVisit.LastRow = false;
                                    ConsultInPatientVisit.LastRow = false;
                                    MoveToTab(6, MedicalHistoryTab);
                                }
                                else
                                {
                                    ConsultInPatientVisit.Focus();
                                }
                            }
                            else
                            {
                                IsMedicalHistoryTabFocused = true;
                                ConsultOutPatientVisit.LastRow = false;
                                ConsultInPatientVisit.LastRow = false;
                                MoveToTab(6, MedicalHistoryTab);
                            }
                        }
                        else
                        {
                            ConsultOutPatientVisit.Focus();
                        }
                    }
                    else if (IPVisitRowCount > 0)
                    {
                        IPVisitIsLastRow = ConsultInPatientVisit.LastRow;
                        if (IPVisitIsLastRow)
                        {
                            IsMedicalHistoryTabFocused = true;
                            ConsultOutPatientVisit.LastRow = false;
                            ConsultInPatientVisit.LastRow = false;
                            MoveToTab(6, MedicalHistoryTab);
                        }
                        else
                        {
                            ConsultInPatientVisit.Focus();
                        }
                    }
                    else
                    {
                        IsMedicalHistoryTabFocused = true;
                        ConsultOutPatientVisit.LastRow = false;
                        ConsultInPatientVisit.LastRow = false;
                        MoveToTab(6, MedicalHistoryTab);
                    }
                    break;
                case 6:
                    HandleTabKeyForPatientMedicalHistory();
                    break;
            }
        }

        private void HandleTabControlShiftTab(Keys keyData)
        {
            switch (TabControlConsult.SelectedIndex)
            {
                case 7:
                    if (BtnMedicalHistorySave.Enabled)
                    {
                        IsMedicalHistoryTabFocused = false;
                        MoveToTab(6, MedicalHistoryTab);
                        BtnMedicalHistoryCancel.Focus();
                        if (!BtnMedicalHistoryCancel.Focused)
                        {
                            BtnMedicalHistoryCancel.Focus();
                        }
                    }
                    else
                    {
                        IsMedicalHistoryTabFocused = true;
                        MoveToTab(6, MedicalHistoryTab);
                        MedicalHistoryTab.Focus();
                    }
                    break;
                case 6:
                    if (!IsMedicalHistoryTabFocused)
                    {
                        HandleShiftTabKeyForPatientMedicalHistory();
                    }
                    else if (BtnMedicalHistorySave.ContainsFocus || BtnMedicalHistoryCancel.ContainsFocus)
                    {
                        HandleTabKeyForGridViewPatientHistoryBtn(true);
                    }
                    else
                    {
                        IsPatientVisitEntryFocused = true;
                        MoveToTab(5, VisitTab);
                    }
                    break;
                case 5:
                    if (BtnSaveVitals.Enabled)
                    {
                        IsPatientVitalEntryIsFocused = true;
                        PatientVitalEntry.FocusIndex = 0;
                        MoveToTab(4, VitalsTab);
                        BtnResetVital.Focus();
                    }
                    else
                    {
                        IsPatientVitalEntryIsFocused = true;
                        MoveToTab(4, VitalsTab);
                        VitalsTab.Focus();
                    }
                    break;
                case 4:
                    if (IsPatientVitalEntryIsFocused)
                    {
                        if (BtnCancelProcedureInfo.Enabled)
                        {
                            MoveToTab(3, ProcedureTab);
                            BtnCancelProcedureInfo.Focus();
                        }
                        else
                        {
                            MoveToTab(3, ProcedureTab);
                            ProcedureTab.Select();
                            ProcedureTab.Focus();
                        }

                    }
                    else if (!IsPatientVitalEntryIsFocused)
                    {
                        HandleTabKeyForGridViewVitalEntryBtn(true);
                    }
                    break;
                case 3:
                    if (IsMedicalProcedureFocused == true)
                    {
                        HandleTabKeyForGridViewProcedureInfo(keyData);
                    }
                    else if (GridViewProcedureInfoIsLastRow == true)
                    {
                        HandleTabKeyForGridViewProcedureBtn(true);
                    }
                    else
                    {
                        HandleTabKeyForGridViewProcedureInfo(keyData);
                    }
                    break;
                case 2:
                    if (LabHistoryRowCount > 0)
                    {
                        LabHistoryIsLastRow = GridViewLabTestHistory.LastRow;
                        GridViewLabTestHistory.ReverseTab = true;
                        GridViewLabTestHistory.Focus();
                    }
                    else
                    {
                        if (IsPatientLabTestFocused == true)
                        {
                            if (BtnPrescriptionCancel.Enabled)
                            {
                                MoveToTab(1, MedicationTab);
                                this.Invoke((MethodInvoker)delegate
                                {
                                    BtnPrescriptionCancel.Focus();
                                    if (!BtnPrescriptionCancel.Focused)
                                    {
                                        BtnMedicalHistoryCancel.Focus();
                                    }
                                });
                            }
                            else
                            {
                                MoveToTab(1, MedicationTab);
                            }
                        }
                        else
                        {
                            if (BtnPrescriptionCancel.Enabled)
                            {
                                IsPatientPrescriptionFocused = false;
                                MoveToTab(1, MedicationTab);
                                BtnPrescriptionCancel.Focus();
                            }
                            else
                            {
                                IsPatientPrescriptionFocused = true;
                                MoveToTab(1, MedicationTab);
                            }
                        }
                    }
                    break;
                case 1:
                    if (IsPatientPrescriptionFocused)
                    {
                        if (BtnPrescriptionCancel.Focused || BtnPrescriptionSave.Focused)
                        {
                            HandleTabKeyForGridViewPrescriptionBtn(true);
                        }
                        else if (GridViewPrescriptionHistory.ContainsFocus)
                        {
                            HandleShiftTabKeyForGridViewPrescriptionHistory(true);
                        }
                        else if (GridViewPrescriptionDetail.ContainsFocus)
                        {
                            HandleTabKeyForGridViewPrescriptionDetail(true);
                        }
                        else
                        {
                            MoveToTab(0, ConsultNotesTab);
                            if (BtnNoteExit.Enabled)
                            {
                                IsPatientConsultNoteFocused = false;
                                MoveToTab(0, ConsultNotesTab);
                                BtnNoteExit.Focus();
                            }
                            else
                            {
                                IsPatientConsultNoteFocused = false;
                                MoveToTab(0, ConsultNotesTab);
                            }
                        }
                    }
                    else if (GridViewPrescriptionHistory.ContainsFocus)
                    {
                        HandleShiftTabKeyForGridViewPrescriptionHistory(true);
                    }
                    else if (GridViewPrescriptionDetail.ContainsFocus)
                    {
                        HandleTabKeyForGridViewPrescriptionDetail(true);
                    }
                    else if (BtnPrescriptionSave.ContainsFocus || BtnPrescriptionCancel.ContainsFocus
                        || BtnPrescriptionPreview.ContainsFocus || BtnPrescriptionPrint.ContainsFocus
                        || BtnPrescriptionSendMedical.ContainsFocus)
                    {
                        HandleTabKeyForGridViewPrescriptionBtn(true); // Shift + Tab
                    }
                    break;
                case 0:
                    if (IsPatientConsultNoteFocused)
                    {
                        HandleShiftTabKeyForGridViewNote();
                    }
                    else
                    {
                        HandleButtonKeyForGridViewNote(true);
                    }
                    break;
            }
        }
        private void HandleTabControlSwitch()
        {
            int tabCount = TabControlConsult.TabCount;
            int currentIndex = TabControlConsult.SelectedIndex;
            int newIndex = (currentIndex + 1) % tabCount; // Move to the next tab, wrap around to the first tab if at the end

            TabControlConsult.SelectedIndex = newIndex;
            SelectTab(newIndex);
        }

        private void SelectTab(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    ConsultNotesTab.Select();
                    break;
                case 1:
                    MedicationTab.Select();
                    break;
                case 2:
                    LabTab.Select();
                    break;
                case 3:
                    ProcedureTab.Select();
                    break;
                case 4:
                    VitalsTab.Select();
                    break;
                case 5:
                    VisitTab.Select();
                    break;
                case 6:
                    MedicalHistoryTab.Select();
                    break;
                case 7:
                    PatientChatTab.Select();
                    break;
            }
        }
        public void NavigateToTabControl(int TabSelect)
        {
            switch (TabSelect)
            {
                case 0:
                    MoveToTab(0, ConsultNotesTab);
                    if (BtnNoteExit.Enabled)
                    {
                        BtnNoteExit.Focus();
                    }
                    break;

                case 1:
                    //if (GridViewLabTestHistory.LastRow == false)
                    //{
                    //    GridViewPrescriptionHistory.ReverseTab = true;
                    //}
                    //else
                    //{
                    //    GridViewPrescriptionHistory.ReverseTab = false;
                    //}
                    MoveToTab(1, MedicationTab);
                    if (BtnPrescriptionCancel.Enabled)
                    {
                        BtnPrescriptionCancel.Focus();
                    }
                    else
                    {
                        GridViewPrescriptionDetail.Focus();
                    }
                    break;
                case 2:
                    IsPatientVitalEntryIsFocused = false;

                    if (BtnSaveProcedureInfo.Enabled)
                    {
                        MoveToTab(3, ProcedureTab);
                        BtnCancelProcedureInfo.Focus();
                    }
                    else
                    {
                        MoveToTab(3, ProcedureTab);
                        ProcedureTab.Select();
                        ProcedureTab.Focus();
                    }
                    break;
                case 3:
                    if (GridViewPrescriptionDetail.Rows.Count > 0)
                    {
                        GridViewPrescriptionDetail.Focus();
                    }
                    else
                    {
                        if (BtnPrescriptionSave.Enabled)
                        {
                            BtnPrescriptionSave.Focus();
                        }
                        else
                        {
                            PatientLabTestSelected = true;
                            MoveToTab(2, LabTab);
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                LabTab.Select();
                                LabTab.Focus();
                            });
                        }
                    }
                    break;
                case 4:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(TabSelect), "Invalid tab selection."); // Handle unexpected values
            }
        }

        private void MoveToTab(int tabIndex, Control tabControl)
        {
            TabControlConsult.SelectedIndex = tabIndex;
            tabControl.Select();
            tabControl.Focus();
        }
        private void PatientVitalHistory_RowDeleted(object sender, EventArgs e)
        {
            PatientVitalEntry.Clear();
        }
        private void SelectEntireRow(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < GridViewProcedureInfo.Rows.Count)
            {
                GridViewProcedureInfo.CurrentCell = GridViewProcedureInfo[0, rowIndex];

                GridViewProcedureInfo.ClearSelection();

                GridViewProcedureInfo.Rows[rowIndex].Selected = true;
            }
        }
        private void HandleTabKeyForGridViewProcedureInfo(Keys keyData)
        {
            if (GridViewProcedureInfo.CurrentCell != null)
            {
                // if (GridViewProcedureInfoIsLastRow == true)
                if (GridViewProcedureInfo.Rows.Count > 0)
                {
                    int currentRowIndex = GridViewProcedureInfo.CurrentCell.RowIndex;
                    int currentColumnIndex = GridViewProcedureInfo.CurrentCell.ColumnIndex;
                    int totalRows = GridViewProcedureInfo.Rows.Count;

                    if (keyData == Keys.Tab)
                    {
                        if (currentRowIndex < totalRows - 1)
                        {
                            GridViewProcedureInfo.CurrentCell = GridViewProcedureInfo[0, currentRowIndex + 1];
                        }
                        else
                        {
                            BtnSaveProcedureInfo.Focus();
                        }
                    }
                    else if (keyData == (Keys.Shift | Keys.Tab))
                    {
                        if (currentRowIndex > 0)
                        {
                            GridViewProcedureInfo.CurrentCell = GridViewProcedureInfo[0, currentRowIndex - 1];
                        }
                        else if (currentRowIndex == 0)
                        {
                            IsPatientLabTestFocused = false;
                            if (BtnLabTestImgCancel.Enabled)
                            {
                                BtnLabTestImgCancel.Focus();
                                PatientLabTestSelected = true;
                                MoveToTab(2, LabTab);
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    LabTab.Select();
                                    LabTab.Focus();
                                });
                            }
                            else
                            {
                                PatientLabTestSelected = true;
                                MoveToTab(2, LabTab);
                                this.BeginInvoke((MethodInvoker)delegate
                                {
                                    LabTab.Select();
                                    LabTab.Focus();
                                });
                            }
                        }
                    }
                }
                else if (GridViewProcedureInfo.Rows.Count == 0)
                {

                    IsPatientLabTestFocused = false;
                    if (BtnLabTestImgCancel.Enabled)
                    {
                        BtnLabTestImgCancel.Focus();
                        PatientLabTestSelected = true;
                        MoveToTab(2, LabTab);
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            LabTab.Select();
                            LabTab.Focus();
                        });
                    }
                    else
                    {
                        PatientLabTestSelected = true;
                        MoveToTab(2, LabTab);
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            LabTab.Select();
                            LabTab.Focus();
                        });
                    }
                }
            }
            else
            {
                if (GridViewProcedureInfo.Rows.Count == 0)
                {

                    IsPatientLabTestFocused = false;
                    if (BtnLabTestImgCancel.Enabled)
                    {
                        BtnLabTestImgCancel.Focus();
                        PatientLabTestSelected = true;
                        MoveToTab(2, LabTab);
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            LabTab.Select();
                            LabTab.Focus();
                        });
                    }
                    else
                    {
                        PatientLabTestSelected = true;
                        MoveToTab(2, LabTab);
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            LabTab.Select();
                            LabTab.Focus();
                        });
                    }
                }
            }
        }
        private void HandleTabKeyForGridViewProcedureBtn(bool isShiftTab)
        {
            if (!isShiftTab) // Handle Tab key logic
            {
                if (ActiveControl == BtnSaveProcedureInfo)
                {
                    BtnCancelProcedureInfo.Select();
                }
                else if (ActiveControl == BtnCancelProcedureInfo)
                {
                    IsPatientVitalEntryIsFocused = true;
                    PatientVitalEntry.FocusIndex = 0;
                    MoveToTab(4, VitalsTab);
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        VitalsTab.Select();
                        VitalsTab.Focus();
                    });
                }
                else
                {
                    BtnSaveProcedureInfo.Select();
                }
            }
            else // Handle Shift + Tab key logic
            {
                int currentRowIndex = GridViewProcedureInfo.CurrentCell.RowIndex;
                int totalRows = GridViewProcedureInfo.Rows.Count;

                if (GridViewProcedureInfoIsLastRow == true)
                {
                    if (currentRowIndex == 0)
                    {
                        if (BtnLabTestImgCancel.Enabled)
                        {
                            IsPatientLabTestFocused = true;
                            BtnLabTestImgCancel.Focus();
                            PatientLabTestSelected = true;
                            MoveToTab(2, LabTab);
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                LabTab.Select();
                                LabTab.Focus();
                            });
                        }
                        else
                        {
                            IsPatientLabTestFocused = true;
                            PatientLabTestSelected = true;
                            MoveToTab(2, LabTab);
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                LabTab.Select();
                                LabTab.Focus();
                            });
                        }
                    }
                    else
                    {
                        GridViewProcedureInfo.CurrentCell = GridViewProcedureInfo[0, currentRowIndex - 1];
                    }
                }
                else if (ActiveControl == BtnSaveProcedureInfo)
                {
                    GridViewProcedureInfo.Select();
                    GridViewProcedureInfo.CurrentCell = GridViewProcedureInfo[0, totalRows - 1];
                }
                else if (ActiveControl == BtnCancelProcedureInfo)
                {
                    BtnSaveProcedureInfo.Select();
                }
                else if (ActiveControl == GridViewProcedureInfo)
                {
                    if (GridViewProcedureInfo.Rows.Count == 1)
                    {
                        if (BtnLabTestImgCancel.Enabled)
                        {
                            IsPatientLabTestFocused = true;
                            BtnLabTestImgCancel.Focus();
                            PatientLabTestSelected = true;
                            MoveToTab(2, LabTab);
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                LabTab.Select();
                                LabTab.Focus();
                            });
                        }
                        else
                        {
                            IsPatientLabTestFocused = true;
                            PatientLabTestSelected = true;
                            MoveToTab(2, LabTab);
                            this.BeginInvoke((MethodInvoker)delegate
                            {
                                LabTab.Select();
                                LabTab.Focus();
                            });
                        }
                    }
                    else
                    {
                        GridViewProcedureInfo.CurrentCell = GridViewProcedureInfo[0, currentRowIndex - 1];
                    }
                }
            }
        }

        private void BtnResetVital_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;

                if (e.Shift)
                {
                    BtnSaveVitals.Select();
                }
                else
                {
                    MoveToTab(5, VisitTab);
                }
            }
        }
        private void SimulateCellContentClick(int rowIndex, int columnIndex)
        {
            HidePopupListView();
            var e = new DataGridViewCellEventArgs(columnIndex, rowIndex);
            ConsultationNotesGrid_CellContentClick(this, e);
        }
        private void HandleKeyPressForPatientMedicalHistory(Keys keyData)
        {
            int maxColumnIndex = GridViewPatientHistory.ColumnCount - 1;
            int maxRowIndex = GridViewPatientHistory.Rows.Count - 1;

            int currentColumnIndex = GridViewPatientHistory.CurrentCell.ColumnIndex;
            int currentRowIndex = GridViewPatientHistory.CurrentCell.RowIndex;

            switch (keyData)
            {
                case Keys.Down:
                    HandleDownKeyForPatientMedicalHistory(ref currentRowIndex, maxRowIndex, maxColumnIndex);
                    break;

                case Keys.Up:
                    HandleUpKeyForPatientMedicalHistory(ref currentRowIndex, maxRowIndex, maxColumnIndex);
                    break;

                case Keys.Right:
                    HandleTabKeyForPatientMedicalHistory();
                    break;

                case Keys.Left:
                    HandleShiftTabKeyForPatientMedicalHistory();
                    break;
            }
        }

        private void HandleDownKeyForPatientMedicalHistory(ref int currentRowIndex, int maxRowIndex, int maxColumnIndex)
        {
            int currentColumnIndex = GridViewPatientHistory.CurrentCell.ColumnIndex;

            while (true)
            {
                currentRowIndex++;
                if (currentRowIndex > maxRowIndex)
                {
                    TextBoxOtherNotes.Select();
                    return;
                }

                if (currentColumnIndex <= maxColumnIndex &&
                    GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex];
                    return;
                }

                for (int columnIndex = 0; columnIndex <= maxColumnIndex; columnIndex++)
                {
                    if (GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
                    {
                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex];
                        return;
                    }
                }
            }
        }

        private void HandleUpKeyForPatientMedicalHistory(ref int currentRowIndex, int maxRowIndex, int maxColumnIndex)
        {
            int currentColumnIndex = GridViewPatientHistory.CurrentCell.ColumnIndex;

            while (true)
            {
                currentRowIndex--;
                if (currentRowIndex < 0)
                {
                    ConsultOutPatientVisit.LastRow = false;
                    ConsultInPatientVisit.LastRow = false;
                    MoveToTab(6, MedicalHistoryTab);
                    return;
                }

                if (currentColumnIndex <= maxColumnIndex &&
                    GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex];
                    return;
                }

                for (int columnIndex = maxColumnIndex; columnIndex >= 0; columnIndex--)
                {
                    if (GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
                    {
                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[columnIndex];
                        return;
                    }
                }
            }
        }

        private void GridViewPatientHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            IsMedicalHistoryTabFocused = true;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var currentCell = GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Check if the current clicked cell is a CheckBox cell
                if (currentCell is DataGridViewCheckBoxCell)
                {
                    GridViewPatientHistory.CurrentCell = currentCell;
                    ToggleCheckBoxStateAtCurrentCell(e.RowIndex, e.ColumnIndex);

                    // Allow editing on the next cell based on the dictionary
                    if (e.RowIndex + 1 < GridViewPatientHistory.Rows.Count && e.ColumnIndex + 1 < GridViewPatientHistory.Columns.Count)
                    {
                        var nextCell = GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1];
                        var CurrentCheckBoxCell = GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                        if (cellTagDictionary.TryGetValue((e.RowIndex + 1, e.ColumnIndex + 1), out bool tagValue) && tagValue)
                        {
                            nextCell.ReadOnly = false;
                            //if (CurrentCheckBoxCell != null && CurrentCheckBoxCell.Value is bool isChecked && isChecked)
                            //{
                            //    //GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1];
                            //    //GridViewPatientHistory.BeginEdit(true);
                            //}
                        }
                    }
                }
                else if (currentCell is DataGridViewTextBoxCell)
                {
                    bool canEdit = false;

                    if (e.ColumnIndex > 0)
                    {

                        var previousCell = GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex - 1];

                        // Check if the previous cell is a DataGridViewCheckBoxCell
                        if (previousCell is DataGridViewCheckBoxCell)
                        {
                            GridViewPatientHistory.CurrentCell = previousCell;
                            ToggleCheckBoxStateAtPreviousCell(e.RowIndex, e.ColumnIndex);

                            if (e.RowIndex + 1 < GridViewPatientHistory.Rows.Count && e.ColumnIndex < GridViewPatientHistory.Columns.Count)
                            {
                                var sameCell = GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex];
                                var CurrentCheckBoxCell = GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex - 1] as DataGridViewCheckBoxCell;
                                if (cellTagDictionary.TryGetValue((e.RowIndex + 1, e.ColumnIndex), out bool nextTagValue) && nextTagValue)
                                {
                                    sameCell.ReadOnly = false;
                                    if (CurrentCheckBoxCell != null && CurrentCheckBoxCell.Value is bool isChecked && isChecked)
                                    {
                                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex];
                                        GridViewPatientHistory.BeginEdit(true);
                                    }
                                }
                            }
                        }
                        SetTagForSpecificCells();

                        cellTagDictionary = CreateCellTagDictionary();
                        // Directly check if the value in cellTagDictionary is true for the given row and column
                        if (cellTagDictionary.ContainsKey((e.RowIndex, e.ColumnIndex)) && cellTagDictionary[(e.RowIndex, e.ColumnIndex)])
                        {
                            if (e.RowIndex > 0 && e.ColumnIndex > 0)
                            {
                                var previousCheckBoxCell = GridViewPatientHistory.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1] as DataGridViewCheckBoxCell;

                                if (previousCheckBoxCell != null && previousCheckBoxCell.Value is bool isChecked && isChecked)
                                {
                                    canEdit = true;
                                }
                            }
                        }

                        currentCell.ReadOnly = !canEdit;

                        if (!currentCell.ReadOnly)
                        {
                            GridViewPatientHistory.CurrentCell = currentCell;
                            GridViewPatientHistory.BeginEdit(true);
                        }
                    }
                    else
                    {
                        // Check for next DataGridViewCheckBoxCell and focus
                    }
                }
                else
                {
                    GridViewPatientHistory.CurrentCell = currentCell;
                }
            }
        }
        private void ToggleCheckBoxStateAtPreviousCell(int currentRowIndex, int currentColumnIndex)
        {
            bool operationPerformed = false;

            if (currentColumnIndex > 0 &&
                GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex - 1] is DataGridViewCheckBoxCell previousCheckBoxCell)
            {
                // Check if the previous checkbox is checked
                bool isChecked = previousCheckBoxCell.Value is true;

                previousCheckBoxCell.Value = !isChecked;

                GridViewPatientHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);
                operationPerformed = true;
            }

            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex];

            if (currentRowIndex == GridViewPatientHistory.Rows.Count - 1 &&
                currentColumnIndex == GridViewPatientHistory.Columns.Count - 1 && operationPerformed)
            {
                TextBoxOtherNotes.Select();
            }
        }

        private void ToggleCheckBoxStateAtCurrentCell(int currentRowIndex, int currentColumnIndex)
        {
            bool operationPerformed = false;

            // Check if the current cell is a DataGridViewCheckBoxCell
            if (GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex] is DataGridViewCheckBoxCell currentCheckBoxCell)
            {
                bool isChecked = (bool)currentCheckBoxCell.Value;
                currentCheckBoxCell.Value = !isChecked;
                GridViewPatientHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);
                operationPerformed = true;
            }
            // If the current cell is not a DataGridViewCheckBoxCell, check the previous cell
            else if (currentColumnIndex > 0 && GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex - 1] is DataGridViewCheckBoxCell previousCheckBoxCell)
            {
                bool isChecked = (bool)previousCheckBoxCell.Value;
                previousCheckBoxCell.Value = !isChecked;
                GridViewPatientHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);
                operationPerformed = true;
            }

            // Keep the focus on the initially clicked cell
            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[currentRowIndex].Cells[currentColumnIndex];

            // If the current cell is the last row and last column, focus on TextBoxOtherNotes
            if (currentRowIndex == GridViewPatientHistory.Rows.Count - 1 && currentColumnIndex == GridViewPatientHistory.Columns.Count - 1 && operationPerformed)
            {
                TextBoxOtherNotes.Select();
            }
        }

        private void GridViewPrescriptionDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (GridViewPrescriptionDetail.CurrentCell != null && GridViewPrescriptionDetail.RowCount > 0)
            {
                int currentRowIndex = GridViewPrescriptionDetail.CurrentCell.RowIndex;

                if (e.KeyCode == Keys.Tab)
                {
                    if (currentRowIndex < GridViewPrescriptionDetail.RowCount - 1)
                    {
                        // Move to the next row
                        GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[currentRowIndex + 1].Cells[0];
                        GridViewPrescriptionDetail.Rows[currentRowIndex + 1].Selected = true;
                        e.SuppressKeyPress = true;
                    }
                    else
                    {
                        BtnPrescriptionSave.Focus();
                        e.SuppressKeyPress = true;
                    }
                }
            }
        }

        private void HandleTabKeyForGridViewPrescriptionDetail(bool isShiftTab)
        {
            if (GridViewPrescriptionDetail.CurrentCell == null) return;

            int maxColumnIndex = GridViewPrescriptionDetail.ColumnCount - 1;
            int currentRowIndex = GridViewPrescriptionDetail.CurrentCell.RowIndex;
            int currentColumnIndex = GridViewPrescriptionDetail.CurrentCell.ColumnIndex;

            int newColumnIndex = isShiftTab ? currentColumnIndex - 1 : currentColumnIndex + 1;

            if (isShiftTab)
            {
                if (newColumnIndex < 0)
                {
                    if (currentRowIndex == 0)
                    {
                        GridViewPrescriptionHistory.ReverseTab = true;
                        GridViewPrescriptionHistory.Focus();
                        if (GridViewPrescriptionHistory.Focused)
                        {
                            MedicationTabSelected = false;
                            return;
                        }
                    }
                    else if (currentRowIndex > 0)
                    {
                        GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[currentRowIndex - 1].Cells[maxColumnIndex - 1];
                    }
                }
                else
                {
                    while (newColumnIndex >= 0 && !GridViewPrescriptionDetail.Columns[newColumnIndex].Visible)
                    {
                        newColumnIndex--;
                    }
                    if (newColumnIndex >= 0)
                    {
                        GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[currentRowIndex].Cells[newColumnIndex];
                    }
                }
            }
            else
            {
                if (currentRowIndex == GridViewPrescriptionDetail.RowCount - 1 && currentColumnIndex == maxColumnIndex)
                {
                    BtnPrescriptionSave.Focus();
                }
                else if (newColumnIndex > maxColumnIndex)
                {
                    int nextRowIndex = currentRowIndex + 1;
                    if (nextRowIndex < GridViewPrescriptionDetail.RowCount)
                    {
                        GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[nextRowIndex].Cells[0];
                    }
                    else
                    {
                        BtnPrescriptionSave.Focus();
                    }
                }
                else
                {
                    while (newColumnIndex <= maxColumnIndex && !GridViewPrescriptionDetail.Columns[newColumnIndex].Visible)
                    {
                        newColumnIndex++;
                    }
                    if (newColumnIndex <= maxColumnIndex)
                    {
                        GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[currentRowIndex].Cells[newColumnIndex];
                    }
                    else if (newColumnIndex > maxColumnIndex)
                    {
                        if (currentRowIndex == GridViewPrescriptionDetail.RowCount - 1)
                        {
                            BtnPrescriptionSave.Focus();
                        }
                        else
                        {
                            GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[currentRowIndex + 1].Cells[0];
                        }
                    }
                }
            }
        }

        private void HandleShiftTabKeyForGridViewPrescriptionHistory(bool isShiftTab)
        {
            // Check if the focus is currently on GridViewPrescriptionHistory
            if (GridViewPrescriptionHistory.ContainsFocus)
            {
                if (PrescriptionHistoryRowCount > 0)
                {
                    int currentRowIndex = GridViewPrescriptionHistory.GridRowIndex; // Assuming it's a DataGridView
                    bool isFirstRow = !GridViewPrescriptionHistory.LastRow;
                    bool isLastRow = GridViewPrescriptionHistory.LastRow;
                    if (isShiftTab)
                    {
                        if (isFirstRow)
                        {
                            IsPatientConsultNoteFocused = false;
                            MoveToTab(0, ConsultNotesTab);
                            if (BtnNoteCancel.Enabled)
                            {
                                BtnNoteCancel.Focus();
                            }
                        }
                        else
                        {
                            int previousRowIndex = currentRowIndex - 1;
                            GridViewPrescriptionHistory.GridRowIndex = previousRowIndex;

                            var selectedRow = GridViewPrescriptionHistory.GridRowIndex;
                            if (selectedRow != null)
                            {
                                GridViewPrescriptionHistory_Load(GridViewPrescriptionHistory, EventArgs.Empty);
                            }
                        }
                    }
                    else
                    {
                        // Handle Tab logic
                        if (isLastRow)
                        {
                            BtnPrescriptionSave.Focus(); // Focus on BtnPrescriptionSave if it's the last row
                        }
                        else
                        {
                            int nextRowIndex = currentRowIndex + 1;
                            GridViewPrescriptionHistory.GridRowIndex = nextRowIndex; // Focus on the next row's first cell

                            GridViewPrescriptionHistory_Load(GridViewPrescriptionHistory, EventArgs.Empty);
                        }
                    }
                }
            }
            IsPatientPrescriptionFocused = false;
            MedicationTabSelected = false;
        }

        private void HandleTabKeyForGridViewPrescriptionHistory()
        {
            // Check if the focus is currently on GridViewPrescriptionHistory
            if (GridViewPrescriptionHistory.ContainsFocus)
            {
                if (PrescriptionHistoryRowCount > 0)
                {
                    PrescriptionHistoryIsLastRow = GridViewPrescriptionHistory.LastRow;

                    if (PrescriptionHistoryIsLastRow)
                    {
                        if (GridViewPrescriptionDetail.RowCount > 0)
                        {
                            GridViewPrescriptionDetail.Focus();
                        }
                        else
                        {
                            GridViewPrescriptionHistory.LastRow = false;
                            PatientLabTestSelected = true;
                            MoveToTab(2, LabTab);
                        }
                    }
                    else
                    {
                        GridViewPrescriptionHistory.Focus();
                        if (GridViewPrescriptionHistory.GetRowCount() > 0)
                        {
                            var selectedRow = GridViewPrescriptionHistory.GetSelectedRow();
                            if (selectedRow != null)
                            {
                                selectedRow.Selected = true;
                                GridViewPrescriptionHistory_Load(GridViewPrescriptionHistory, EventArgs.Empty);
                            }
                        }
                    }
                }
            }
            MedicationTabSelected = false;
        }
        private void HandleTabKeyForGridViewLabTestElementInformationDetail(bool isShiftTab)
        {

            // Check if the current cell is null
            if (GridViewLabTestElementInformation.CurrentCell == null) return;

            // Get the maximum column index and current cell indices
            int maxColumnIndex = GridViewLabTestElementInformation.ColumnCount - 1;
            int currentRowIndex = GridViewLabTestElementInformation.CurrentCell.RowIndex;
            int currentColumnIndex = GridViewLabTestElementInformation.CurrentCell.ColumnIndex;

            // Calculate the new column index based on whether Shift+Tab or Tab was pressed
            int newColumnIndex = isShiftTab ? currentColumnIndex - 1 : currentColumnIndex + 1;

            if (isShiftTab)
            {
                // Handle Shift+Tab (reverse tabbing)
                if (newColumnIndex < 0)
                {
                    if (currentRowIndex == 0)
                    {
                        // If it's the first row and first column, move focus to another control (e.g., GridViewLabTestHistory)
                        GridViewLabTestHistory.ReverseTab = true;
                        GridViewLabTestHistory.Focus();
                        if (GridViewLabTestHistory.Focused)
                        {
                            MedicationTabSelected = false;
                            return;
                        }
                    }
                    else if (currentRowIndex > 0)
                    {
                        // Move to the last column of the previous row
                        GridViewLabTestElementInformation.CurrentCell = GridViewLabTestElementInformation.Rows[currentRowIndex - 1].Cells[maxColumnIndex - 1];
                    }
                }
                else
                {
                    // Find the previous visible column
                    while (newColumnIndex >= 0 && !GridViewLabTestElementInformation.Columns[newColumnIndex].Visible)
                    {
                        newColumnIndex--;
                    }
                    if (newColumnIndex >= 0)
                    {
                        GridViewLabTestElementInformation.CurrentCell = GridViewLabTestElementInformation.Rows[currentRowIndex].Cells[newColumnIndex];
                    }
                }
            }
            else
            {
                if (BtnConsultLabTestElementSave.Focused)
                {
                    if (BtnLabTestPrintRequisition.Enabled)
                    {
                        BtnLabTestPrintRequisition.Focus();
                    }
                    else
                    {
                        BtnConsultLabTestElementCancel.Focus();
                    }
                }
                else if (BtnLabTestPrintRequisition.Focused)
                {
                    BtnConsultLabTestElementCancel.Focus();
                }
                else if (BtnConsultLabTestElementCancel.Focused)
                {
                    MoveToTab(3, ProcedureTab);
                    ProcedureTab.Select();
                }
                // Handle Tab (forward tabbing)
                else if (currentRowIndex == GridViewLabTestElementInformation.RowCount - 1 && currentColumnIndex == maxColumnIndex)
                {
                    BtnConsultLabTestElementSave.Focus(); // Move focus to the Save button if at the last cell
                }
                else if (newColumnIndex > maxColumnIndex)
                {
                    // Move to the first column of the next row
                    int nextRowIndex = currentRowIndex + 1;
                    if (nextRowIndex < GridViewLabTestElementInformation.RowCount)
                    {
                        GridViewLabTestElementInformation.CurrentCell = GridViewLabTestElementInformation.Rows[nextRowIndex].Cells[0];
                    }
                    else
                    {
                        BtnConsultLabTestElementSave.Focus(); // Move focus to the Save button if there are no more rows
                    }
                }
                else
                {
                    // Find the next visible column
                    while (newColumnIndex <= maxColumnIndex && !GridViewLabTestElementInformation.Columns[newColumnIndex].Visible)
                    {
                        newColumnIndex++;
                    }
                    if (newColumnIndex <= maxColumnIndex)
                    {
                        GridViewLabTestElementInformation.CurrentCell = GridViewLabTestElementInformation.Rows[currentRowIndex].Cells[newColumnIndex];
                    }
                    else if (newColumnIndex > maxColumnIndex)
                    {
                        // Move to the first column of the next row if needed
                        if (currentRowIndex == GridViewLabTestElementInformation.RowCount - 1)
                        {
                            BtnConsultLabTestElementSave.Focus(); // Focus the Save button if no more rows are left
                        }
                        else
                        {
                            GridViewLabTestElementInformation.CurrentCell = GridViewLabTestElementInformation.Rows[currentRowIndex + 1].Cells[0];
                        }
                    }
                }
            }
        }

        private void BtnPrescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                System.Windows.Forms.Button? currentButton = sender as System.Windows.Forms.Button;

                if (currentButton == BtnPrescriptionSave)
                {
                    BtnPrescriptionPreview.Focus();
                }
                else if (currentButton == BtnPrescriptionPreview)
                {
                    BtnPrescriptionPrint.Focus();
                }
                else if (currentButton == BtnPrescriptionPrint)
                {
                    BtnPrescriptionSendMedical.Focus();
                }
                else if (currentButton == BtnPrescriptionSendMedical)
                {
                    BtnPrescriptionCancel.Focus();
                }
                else if (currentButton == BtnPrescriptionCancel)
                {
                    LabTab.Focus();
                }

                e.SuppressKeyPress = true;
            }
        }

        private void UpdateMedicationTabSelected()
        {
            if (GridViewPrescriptionHistory.LastSelectedRowIndex == 0)
            {
                IsPatientPrescriptionFocused = true;
            }
            else
            {
                IsPatientPrescriptionFocused = false;
            }
            MedicationTabSelected = !(GridViewPrescriptionHistory.Focused ||
                                      GridViewPrescriptionDetail.Focused ||
                                      BtnPrescriptionSave.Focused ||
                                      BtnPrescriptionPrint.Focused ||
                                      BtnPrescriptionPreview.Focused ||
                                      BtnPrescriptionSendMedical.Focused ||
                                      BtnPrescriptionCancel.Focused);
        }
        private void Control_Leave(object sender, EventArgs e)
        {
            UpdateMedicationTabSelected();
        }
        private void MedicalHistoryInitialColumnWidths()
        {
            MedicalHistorycolumnWidths = new Dictionary<string, int>();

            foreach (DataGridViewColumn column in GridViewPatientHistory.Columns)
            {
                if (column is DataGridViewCheckBoxColumn checkBoxColumn)
                {
                    int adjustedWidth = GridViewPatientHistory.RowTemplate.Height - 4; // Slightly less width than height
                    MedicalHistorycolumnWidths[column.Name] = adjustedWidth;
                }
                else
                {
                    int adjustedWidth = column.Width + (int)(column.Width * 0.75); // Increase other cells' width by half
                    MedicalHistorycolumnWidths[column.Name] = adjustedWidth;
                }
            }
            MedicalHistoryGridWidthChanged = true;
        }

        // Method to apply the stored column widths
        private void ApplyColumnWidths()
        {
            foreach (DataGridViewColumn column in GridViewPatientHistory.Columns)
            {
                if (MedicalHistorycolumnWidths.TryGetValue(column.Name, out int width))
                {
                    column.Width = width;
                }
            }
        }
        private void SearchDelayTimer_Tick(object? sender, EventArgs e)
        {
            // Stop the timer to avoid multiple triggers
            searchDelayTimer.Stop();

            if (GridViewNote.EditingControl is System.Windows.Forms.TextBox editingTextBox)
            {
                string typedText = editingTextBox.Text.Trim();

                if (typedText.Length > 2)
                {
                    LoadSuggestionNoteAndUpdatePopup(typedText);
                }
                else
                {
                    HidePopupListView();
                }
            }
            else
            {
                HidePopupListView();
            }
        }

        private void HandleTabKeyForGridViewPrescriptionBtn(bool isShiftTab)
        {
            if (isShiftTab)
            {
                // Handle Shift + Tab navigation through the buttons
                if (ActiveControl == BtnPrescriptionCancel)
                {
                    if (BtnPrescriptionSendMedical.Enabled)
                    {
                        BtnPrescriptionSendMedical.Focus();
                    }
                    else
                    {
                        BtnPrescriptionSave.Focus();
                    }
                }
                else if (ActiveControl == BtnPrescriptionSendMedical)
                {
                    BtnPrescriptionPrint.Focus();
                }
                else if (ActiveControl == BtnPrescriptionPrint)
                {
                    BtnPrescriptionPreview.Focus();
                }
                else if (ActiveControl == BtnPrescriptionPreview)
                {
                    BtnPrescriptionSave.Focus();
                }
                else if (ActiveControl == BtnPrescriptionSave)
                {
                    if (GridViewPrescriptionDetail.RowCount > 0)
                    {
                        int lastRowIndex = GridViewPrescriptionDetail.RowCount - 1;

                        GridViewPrescriptionDetail.CurrentCell = GridViewPrescriptionDetail.Rows[lastRowIndex].Cells[9];
                        GridViewPrescriptionDetail.Focus();
                    }
                    else if (GridViewPrescriptionHistory.GridRows > 0)
                    {
                        GridViewPrescriptionHistory.RowSelection = true;
                        int currentRowIndex = GridViewPrescriptionHistory.LastSelectedRowIndex;// GridViewPrescriptionHistory.GridRowIndex;

                        //GridViewPrescriptionHistory.SelectDataGrid_CellClick(currentRowIndex);
                        GridViewPrescriptionHistory.Focus();
                        PrescriptionHistoryDataSaved = false;
                    }
                    else
                    {
                        MoveToTab(0, ConsultNotesTab);
                        if (BtnNoteExit.Enabled)
                        {
                            IsPatientConsultNoteFocused = false;
                            MoveToTab(0, ConsultNotesTab);
                            BtnNoteExit.Focus();
                        }
                        else
                        {
                            IsPatientConsultNoteFocused = false;
                            MoveToTab(0, ConsultNotesTab);
                        }
                    }
                }
            }
            else
            {
                // If focus is not on GridViewPrescriptionHistory, navigate through the buttons
                if (ActiveControl == BtnPrescriptionSave)
                {
                    if (BtnPrescriptionPreview.Enabled)
                    {
                        BtnPrescriptionPreview.Focus();
                    }
                    else
                    {
                        BtnPrescriptionCancel.Focus();
                    }
                }
                else if (ActiveControl == BtnPrescriptionPreview)
                {
                    BtnPrescriptionPrint.Focus();
                }
                else if (ActiveControl == BtnPrescriptionPrint)
                {
                    BtnPrescriptionSendMedical.Focus();
                }
                else if (ActiveControl == BtnPrescriptionSendMedical)
                {
                    BtnPrescriptionCancel.Focus();
                }
                else if (ActiveControl == BtnPrescriptionCancel)
                {
                    PatientLabTestSelected = true;
                    MoveToTab(2, LabTab);
                }
            }

            MedicationTabSelected = false;
        }
        private void HandleTabKeyForGridViewPatientHistoryBtn(bool isShiftTab)
        {
            if (isShiftTab)
            {
                // Handle Shift + Tab navigation
                if (ActiveControl == BtnMedicalHistorySave)
                {
                    TextBoxOtherNotes.Select();
                    TextBoxOtherNotes.Focus();
                    if (!string.IsNullOrEmpty(TextBoxOtherNotes.Text))
                    {
                        TextBoxOtherNotes.SelectAll(); // Select all text if there's any
                    }
                }
                else if (IsMedicalHistoryTabFocused)
                {
                    MoveToTab(5, VisitTab);
                }
                else if (ActiveControl == BtnMedicalHistoryCancel)
                {
                    BtnMedicalHistorySave.Focus();
                }
                else if (ActiveControl == TextBoxOtherNotes)
                {
                    if (GridViewPatientHistory.RowCount > 0)
                    {
                        DataGridViewCheckBoxCell lastCheckBoxCell = null!;
                        int lastCheckBoxColumnIndex = -1;

                        for (int rowIndex = GridViewPatientHistory.RowCount - 1; rowIndex >= 0; rowIndex--)
                        {
                            for (int colIndex = GridViewPatientHistory.ColumnCount - 1; colIndex >= 0; colIndex--)
                            {
                                if (GridViewPatientHistory.Rows[rowIndex].Cells[colIndex] is DataGridViewCheckBoxCell)
                                {
                                    lastCheckBoxCell = (DataGridViewCheckBoxCell)GridViewPatientHistory.Rows[rowIndex].Cells[colIndex];
                                    lastCheckBoxColumnIndex = colIndex;
                                    break;
                                }
                            }

                            if (lastCheckBoxCell != null && lastCheckBoxColumnIndex != -1)
                            {
                                PreventGridViewPatientHistoryEnterEvent = true;
                                GridViewPatientHistory.CurrentCell = lastCheckBoxCell;
                                GridViewPatientHistory.Focus();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (ActiveControl == BtnMedicalHistorySave)
                {
                    BtnMedicalHistoryCancel.Focus();
                }
                else if (ActiveControl == BtnMedicalHistoryCancel)
                {
                    MoveToTab(7, PatientChatTab);
                }
            }

            MedicationTabSelected = false;
        }
        private void HandleTabKeyForGridViewVitalEntryBtn(bool isShiftTab)
        {
            if (isShiftTab)
            {
                // Handle Shift + Tab navigation
                if (ActiveControl == BtnSaveVitals)
                {
                    BtnResetVital.Focus();
                }
                else if (IsPatientVitalEntryIsFocused)
                {
                    MoveToTab(4, ProcedureTab);
                }
                else if (ActiveControl == BtnResetVital)
                {
                    PatientVitalEntry.Focus();
                }
                else if (ActiveControl == PatientVitalEntry)
                {
                    IsPatientVitalEntryIsFocused = false;

                    if (BtnSaveProcedureInfo.Enabled)
                    {
                        MoveToTab(3, ProcedureTab);
                        BtnCancelProcedureInfo.Focus();
                    }
                    else
                    {
                        MoveToTab(3, ProcedureTab);
                        ProcedureTab.Select();
                        ProcedureTab.Focus();
                    }
                }
                else
                {
                    if (!IsPatientVitalEntryIsFocused)
                    {
                        IsPatientVitalEntryIsFocused = false;

                        if (BtnSaveProcedureInfo.Enabled)
                        {
                            MoveToTab(3, ProcedureTab);
                            BtnCancelProcedureInfo.Focus();
                        }
                        else
                        {
                            MoveToTab(3, ProcedureTab);
                            ProcedureTab.Select();
                            ProcedureTab.Focus();
                        }
                    }
                }
            }
            else
            {
                if (ActiveControl == BtnMedicalHistorySave)
                {
                    BtnMedicalHistoryCancel.Focus();
                }
                else if (ActiveControl == BtnMedicalHistoryCancel)
                {
                    MoveToTab(7, PatientChatTab);
                }
            }

            MedicationTabSelected = false;
        }

        private void UnLockGridViewSelectedCells()
        {
            int trueCount = 0;

            foreach (DataGridViewRow row in GridViewPatientHistory.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Tag is bool tagValue && tagValue == true)
                    {
                        cell.ReadOnly = false;
                        trueCount++;
                    }
                }
            }
        }

        private void LockAllGridViewCells()
        {
            for (int rowIndex = 0; rowIndex < GridViewPatientHistory.Rows.Count; rowIndex++)
            {
                if (GridViewPatientHistory.Rows[rowIndex].IsNewRow) continue;

                for (int columnIndex = 0; columnIndex < GridViewPatientHistory.Columns.Count; columnIndex++)
                {
                    DataGridViewCell cell = GridViewPatientHistory.Rows[rowIndex].Cells[columnIndex];

                    cell.ReadOnly = true;
                }
            }
        }

        private Dictionary<(int rowIndex, int colIndex), bool> CreateCellTagDictionary()
        {
            var cellTagDictionary = new Dictionary<(int rowIndex, int colIndex), bool>();

            for (int rowIndex = 0; rowIndex < GridViewPatientHistory.Rows.Count; rowIndex++)
            {
                if (GridViewPatientHistory.Rows[rowIndex].IsNewRow)
                    continue;

                for (int colIndex = 0; colIndex < GridViewPatientHistory.Columns.Count; colIndex++)
                {
                    if (rowIndex == 21 && (colIndex == 4 || colIndex == 1))
                    {

                    }
                    var cell = GridViewPatientHistory.Rows[rowIndex].Cells[colIndex];

                    bool tagValue = cell.Tag is bool tag ? tag : false;

                    cellTagDictionary[(rowIndex, colIndex)] = tagValue;
                }
            }

            return cellTagDictionary;
        }

        private void MedicalHistoryTab_Enter(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = true;
        }

        private void MedicalHistoryTab_Leave(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = false;
        }

        private void BtnMedicalHistorySave_Enter(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = false;
        }

        private void BtnMedicalHistoryCancel_Enter(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = false;
        }

        private void TextBoxOtherNotes_Click(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = false;
        }

        private void TextBoxOtherNotes_Enter(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = false;
        }

        private void MedicalHistoryTab_Click(object sender, EventArgs e)
        {
            IsMedicalHistoryTabFocused = true;
        }

        private void GridViewPatientHistory_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            IsMedicalHistoryTabFocused = false;
        }


        private void TabControlConsult_Click(object sender, EventArgs e)
        {
            if (TabControlConsult.SelectedTab == ConsultNotesTab)
            {
                IsPatientConsultNoteFocused = true;

            }
            else if (TabControlConsult.SelectedTab == MedicalHistoryTab)
            {
                IsMedicalHistoryTabFocused = true;
            }
            else if (TabControlConsult.SelectedTab == VitalsTab)
            {
                IsPatientVitalEntryIsFocused = true;

            }
            else if (TabControlConsult.SelectedTab == VisitTab)
            {
                IsPatientVisitEntryFocused = true;

            }
            else if (TabControlConsult.SelectedTab == ProcedureTab)
            {
                IsMedicalProcedureFocused = true;

            }
            else if (TabControlConsult.SelectedTab == MedicationTab)
            {
                IsPatientPrescriptionFocused = true;

            }
            else if (TabControlConsult.SelectedTab == LabTab)
            {
                IsPatientLabTestFocused = true;

            }
            else if (TabControlConsult.SelectedTab == PatientChatTab)
            {
                IsPatientChartFocused = true;

            }
        }

        private void TabControlConsult_Enter(object sender, EventArgs e)
        {
            if (TabControlConsult.SelectedTab == ConsultNotesTab)
            {
                IsPatientConsultNoteFocused = true;

            }
            else if (TabControlConsult.SelectedTab == MedicalHistoryTab)
            {
                IsMedicalHistoryTabFocused = true;
            }
            else if (TabControlConsult.SelectedTab == VitalsTab)
            {
                IsPatientVitalEntryIsFocused = true;

            }
            else if (TabControlConsult.SelectedTab == VisitTab)
            {
                IsPatientVisitEntryFocused = true;

            }
            else if (TabControlConsult.SelectedTab == ProcedureTab)
            {
                IsMedicalProcedureFocused = true;

            }
            else if (TabControlConsult.SelectedTab == MedicationTab)
            {
                IsPatientPrescriptionFocused = true;

            }
            else if (TabControlConsult.SelectedTab == LabTab)
            {
                IsPatientLabTestFocused = true;

            }
            else if (TabControlConsult.SelectedTab == PatientChatTab)
            {
                IsPatientChartFocused = true;

            }
        }

        private void GridViewNote_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == (int)ConsultationNotesGridColumn.CONSULTATIONNOTES && e.RowIndex >= 0)
            {
                string cellText = e.Value?.ToString() ?? string.Empty;

                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                Rectangle textRect = e.CellBounds;
                textRect.X += e.CellStyle.Padding.Left;
                textRect.Y += e.CellStyle.Padding.Top;
                textRect.Width -= e.CellStyle.Padding.Right;
                textRect.Height -= e.CellStyle.Padding.Bottom;

                TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.PreserveGraphicsClipping;

                int currentXPosition = textRect.X;
                int currentYPosition = textRect.Y;
                int startIndex = 0;
                int lineHeight = TextRenderer.MeasureText(" ", e.CellStyle.Font).Height;

                while (startIndex < cellText.Length)
                {
                    int startParen = cellText.IndexOf("{", startIndex);

                    if (startParen == -1)
                    {
                        string remainingText = cellText.Substring(startIndex);
                        DrawTextWithWrapping(e.Graphics, remainingText, e.CellStyle.Font, e.CellStyle.ForeColor, textRect, ref currentXPosition, ref currentYPosition, lineHeight, flags);
                        break;
                    }

                    string beforeParenText = cellText.Substring(startIndex, startParen - startIndex);
                    DrawTextWithWrapping(e.Graphics, beforeParenText, e.CellStyle.Font, e.CellStyle.ForeColor, textRect, ref currentXPosition, ref currentYPosition, lineHeight, flags);

                    int endParen = cellText.IndexOf("}", startParen);
                    if (endParen == -1) endParen = cellText.Length - 1;

                    string openBrace = "{";
                    DrawTextWithWrapping(e.Graphics, openBrace, e.CellStyle.Font, Color.White, textRect, ref currentXPosition, ref currentYPosition, lineHeight, flags);

                    string parenText = cellText.Substring(startParen + 1, endParen - startParen - 1);
                    DrawTextWithWrapping(e.Graphics, parenText, e.CellStyle.Font, Color.Red, textRect, ref currentXPosition, ref currentYPosition, lineHeight, flags);

                    string closeBrace = "}";
                    DrawTextWithWrapping(e.Graphics, closeBrace, e.CellStyle.Font, Color.White, textRect, ref currentXPosition, ref currentYPosition, lineHeight, flags);

                    startIndex = endParen + 1;
                }
                e.Handled = true;
            }
        }

        private void DrawTextWithWrapping(Graphics graphics, string text, Font font, Color color, Rectangle textRect, ref int currentXPosition, ref int currentYPosition, int lineHeight, TextFormatFlags flags)
        {
            string[] words = text.Split();

            foreach (string word in words)
            {
                SizeF wordSize = graphics.MeasureString(word, font);

                if (currentXPosition + (int)wordSize.Width > textRect.Right)
                {
                    currentXPosition = textRect.X;
                    currentYPosition += lineHeight;
                }

                if (currentYPosition + lineHeight > textRect.Bottom)
                    break;

                TextRenderer.DrawText(graphics, word, font, new Point(currentXPosition, currentYPosition), color, flags);
                currentXPosition += (int)wordSize.Width;
            }
        }

        private void GridViewPrescriptionHistory_Enter(object sender, EventArgs e)
        {
            GridViewPrescriptionHistory.RowClickedData = true;
        }

        private void GridViewElementInformation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void GridViewLabTestElementInformation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                //var row = GridViewLabTestElementInformation.Rows[e.RowIndex];
                //long TestId = Convert.ToInt64(row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value);
                //MedicalTestElement Element = MedicalTestManager.Instance.GetMedicalTestElementById(TestId);

                //(GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.DataSource = null;
                //(GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.DataSource = Element.Name;
                //(GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.ValueMember = "ID";
                //(GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM] as DataGridViewComboBoxCell)!.DisplayMember = "Name";
            }
        }

        private void GridViewLabTestElementInformation_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SNO].ReadOnly = true;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.NAME].ReadOnly = true;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM].ReadOnly = true;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].ReadOnly = false;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RESULT].ReadOnly = false;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].ReadOnly = false;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].ReadOnly = true;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].ReadOnly = true;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].ReadOnly = true;
            GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.ID].ReadOnly = true;
            if (GridViewLabTestElementInformation.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS && (GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value == null || string.IsNullOrEmpty(GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value.ToString())))
            {
                GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].ReadOnly = true;
            }
            if (GridViewLabTestElementInformation.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS && (GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value == null || string.IsNullOrEmpty(GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value.ToString())))
            {
                GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].ReadOnly = true;
            }
        }

        private void GridViewLabTestElementInformation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                GridViewLabTestElementInformation.BeginEdit(true);

                //var row = GridViewLabTestElementInformation.Rows[e.RowIndex];
                //long TestId = Convert.ToInt64(row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value);

                //MedicalTestElement Element = MedicalTestManager.Instance.GetMedicalTestElementById(TestId);
                //if (Element != null)
                //{
                //    GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGETYPE].Value = Element.SubClass;
                //    GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGEVALUE].Value = Element.SingleValue;
                //    GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value = Element.RangeFrom;
                //    GridViewLabTestElementInformation.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value = Element.RangeTo;
                //}
            }
        }

        private void ComboBoxVariation_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)sender;
            string item = cb.Text;
            if (GridViewLabTestElementInformation.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    GridViewLabTestElementInformation.CurrentRow.Cells[(int)MedicalLabTestElementsGridColumn.CLASS].ReadOnly = false;
                }
            }
        }

        private void GridViewLabTestElementInformation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
            this.formIsDirty = false;
        }

        private void TabControlConsult_Deselecting(object? sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show("Do you really want to Save " + " Consultation Notes?", "Save Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.No)
                    {
                        CancelConsulting();
                        this.formIsDirty = false;
                        return;
                    }
                    if (Result == DialogResult.Yes)
                    {
                        BtnNotesSave_Click(this, null);
                    }
                }
                this.formIsDirty = false;
            }
            else if (e.TabPageIndex == 1)
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show("Do you really want to Save " + " Prescrption?", "Save Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.No)
                    {
                        BtnPrescriptionCancel_Click(this, null);
                        this.formIsDirty = false;
                        return;
                    }
                    if (Result == DialogResult.Yes)
                    {
                        BtnPrescriptionSave_Click(this, null);
                    }
                }
                this.formIsDirty = false;
            }
            else if (e.TabPageIndex == 2)
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show("Do you really want to Save " + " Lab Test?", "Save Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.No)
                    {
                        BtnCancelConsLabTestElement_Click(this, null);
                        this.formIsDirty = false;
                        return;
                    }
                    if (Result == DialogResult.Yes)
                    {
                        BtnSaveConsLabTestElement_Click(this, null);
                    }
                }
                this.formIsDirty = false;
            }
            else if (e.TabPageIndex == 3)
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show("Do you really want to Save " + "Procedure?", "Save Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.No)
                    {
                        BtnCancelProcedureInfo_Click(this, null);
                        this.formIsDirty = false;
                        return;
                    }
                    if (Result == DialogResult.Yes)
                    {
                        BtnSaveProcedureInfo_Click(this, null);
                    }
                }
                this.formIsDirty = false;
            }
            else if (e.TabPageIndex == 4)
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show("Do you really want to Save " + "Vitals?", "Save Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.No)
                    {
                        BtnReset_Click(this, null);
                        this.formIsDirty = false;
                        return;
                    }
                    if (Result == DialogResult.Yes)
                    {
                        BtnSaveVitals_Click(this, null);
                    }
                }
                this.formIsDirty = false;
            }
            else if (e.TabPageIndex == 6)
            {
                if (formIsDirty)
                {
                    DialogResult Result = MessageBox.Show("Do you really want to Save " + "Medical History?", "Save Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.No)
                    {
                        BtnMedicalHistoryCancel_Click(this, null);
                        this.formIsDirty = false;
                        return;
                    }
                    if (Result == DialogResult.Yes)
                    {
                        BtnMedicalHistorySave_Click(this, null);
                    }
                }
                this.formIsDirty = false;
            }
        }
        private void GridViewLabTestElementInformation_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (GridViewLabTestElementInformation.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                ComboBox? classComboBox = e.Control as ComboBox;
                if (classComboBox != null && GridViewLabTestElementInformation.CurrentRow.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value != null)
                {
                    var row = GridViewLabTestElementInformation.CurrentRow;
                    long LabTestId = Convert.ToInt64(row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value);
                    ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(LabTestId);
                    IList<MedicalTestElement> lLabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(consultedLabTest.MedicalTestId);

                    if (row.Cells[(int)MedicalLabTestElementsGridColumn.CLASS] is DataGridViewComboBoxCell classCell)
                    {
                        classCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                        classCell.FlatStyle = FlatStyle.Flat;

                        classCell.Items.Clear();
                        foreach (string Class in lLabTestElements.Select(x => x.Class).Distinct())
                        {
                            if (!string.IsNullOrEmpty(Class))
                            {
                                classCell.Items.Add(Class);
                            }
                        }
                    }
                    classComboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    classComboBox.FlatStyle = FlatStyle.Flat;

                    classComboBox.SelectedIndexChanged -= ClassComboBox_SelectedIndexChanged;
                    classComboBox.SelectedIndexChanged += ClassComboBox_SelectedIndexChanged;

                    classComboBox.BackColor = SystemColors.Window;
                    classComboBox.ForeColor = SystemColors.WindowText;
                }
            }
            else if (GridViewLabTestElementInformation.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                ComboBox? SubclassComboBox = e.Control as ComboBox;
                if (SubclassComboBox != null)
                {
                    SubclassComboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    SubclassComboBox.FlatStyle = FlatStyle.Flat;

                    SubclassComboBox.SelectedIndexChanged -= SubClassComboBox_SelectedIndexChanged;
                    SubclassComboBox.SelectedIndexChanged += SubClassComboBox_SelectedIndexChanged;
                }
            }
        }

        private void SubClassComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (GridViewLabTestElementInformation.CurrentRow != null && GridViewLabTestElementInformation.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                var subClassComboBox = sender as ComboBox;
                var selectedSubClass = subClassComboBox?.SelectedItem?.ToString();

                if (selectedSubClass != null)
                {
                    var row = GridViewLabTestElementInformation.CurrentRow;
                    long LabTestId = Convert.ToInt64(row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value);

                    ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(LabTestId);
                    IList<MedicalTestElement> lLabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(consultedLabTest.MedicalTestId);

                    MedicalTestElement selectedElement = lLabTestElements.FirstOrDefault(x => x.SubClass == selectedSubClass)!;

                    if (selectedElement != null)
                    {
                        row.Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Value = selectedElement.SingleValue;
                        row.Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value = selectedElement.RangeFrom;
                        row.Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value = selectedElement.RangeTo;
                    }
                }
            }
        }

        private void ClassComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (GridViewLabTestElementInformation.CurrentRow != null && GridViewLabTestElementInformation.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                var ClassComboBox = sender as ComboBox;
                var selectedClass = ClassComboBox?.SelectedItem?.ToString()!;
                var classvalue = GridViewLabTestElementInformation.CurrentCell.EditedFormattedValue.ToString();
                var row = GridViewLabTestElementInformation.CurrentRow;
                long LabTestId = Convert.ToInt64(row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value);

                ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(LabTestId);
                IList<MedicalTestElement> lLabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(consultedLabTest.MedicalTestId);

                DataGridViewComboBoxCell SubClasscomboBoxCell = new DataGridViewComboBoxCell();
                SubClasscomboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                SubClasscomboBoxCell.FlatStyle = FlatStyle.Flat;

                IList<MedicalTestElement> elements = lLabTestElements.Where(x => x.Class == classvalue).Distinct().Where(subClass => subClass != null).ToList();
                foreach (MedicalTestElement element in elements)
                {
                    if (!string.IsNullOrEmpty(element.SubClass))
                    {
                        SubClasscomboBoxCell.Items.Add(element.SubClass);
                    }
                }
                row.Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS] = SubClasscomboBoxCell;

                if (consultedLabTest != null && consultedLabTest.ConsultedLabTestElements.Count > 0)
                {
                    long ConLabTestElementId = Convert.ToInt64(row.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value);
                    ConsultedLabTestElements ConsultedLabTestElements = ConsultationNoteManager.Instance.GetConsultedLabTestElementsById(ConLabTestElementId);
                    MedicalTestElement selectedElement = lLabTestElements.FirstOrDefault(x => x.Class == selectedClass && x.Name == ConsultedLabTestElements.Name && x.SubClass == ConsultedLabTestElements.SubClass)!;

                    if (selectedElement != null)
                    {
                        row.Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value = selectedElement.SubClass;
                        row.Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Value = selectedElement.SingleValue;
                        row.Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value = selectedElement.RangeFrom;
                        row.Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value = selectedElement.RangeTo;
                    }
                    else
                    {
                        selectedElement = lLabTestElements.FirstOrDefault(x => x.Class == selectedClass && x.Name == ConsultedLabTestElements.Name)!;
                        if (selectedElement != null)
                        {
                            row.Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value = selectedElement.SubClass;
                            row.Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Value = selectedElement.SingleValue;
                            row.Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Value = selectedElement.RangeFrom;
                            row.Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Value = selectedElement.RangeTo;
                        }
                    }
                }
            }
        }
        private void GridViewLabTestElementInformation_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                GridViewLabTestElementInformation.CommitEdit(DataGridViewDataErrorContexts.Commit);
                GridViewLabTestElementInformation.CurrentCell.Value = GridViewLabTestElementInformation.CurrentCell.EditedFormattedValue;
            }
        }

        private void GridViewLabTestElementInformation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                var currentCell = GridViewLabTestElementInformation[e.ColumnIndex, e.RowIndex];
                if (currentCell.EditedFormattedValue != null)
                {
                    currentCell.Value = currentCell.EditedFormattedValue;
                }
            }
        }

        private void BtnLabTestPrintRequisition_Click_1(object sender, EventArgs e)
        {
            if (GridViewLabTestHistory.LTestId != 0L && PatientId != 0L)
            {
                LabtestRequisitionPrint LabtestRequisitionPrint = new LabtestRequisitionPrint();
                LabtestRequisitionPrint.ExportOrPrintToFile(GridViewLabTestHistory.LTestId, PatientId, "LabTest Requisition", "pdf", true);
            }
        }
    }
}
