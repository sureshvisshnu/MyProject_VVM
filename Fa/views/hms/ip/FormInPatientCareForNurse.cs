using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.model.Hms.common;
using fa.api.Hms;
using fa.model.Hms.Master;
using fa.api.utils;
using fa.views.hms.patient;
using fa.common;
using fa.model.hms.common;
using fa.views.controls.hms;
using Fa.api.Hms;
using fa.libraries.utils;
using fa.views.utils.Hms;
using fa.model.Accounting.Masters;
using fa.views.controls.ComboTreeView;
using fa.report.Hms;
using fa.views.controls.grid;
using VisioForge.Libs.MediaFoundation.OPM;
using System.Globalization;

namespace fa.views.hms.ip
{
    public partial class FormInPatientCareForNurse : FormPatientBase
    {
        public static string SaveSuccessMsg = "Saved success.";
        public static string EnterLabtestElementErrorMsg = "Please enter {0}";
        public static string UpLoadFileErrorMsg = "Please Upload less than 5mb";
        public static string ChooseStatusErrorMsg = "Please select status.";
        public static string EnterValidDateErrorMsg = "Please enter valid date.";
        public static string NoRecordErrorMsg = "Record Not found.";
        public static string NoRecordByErrorMsg = "{0} Record Not found.";
        public long PatientId = 0;
        public long PatientIpId = 0L;
        public LabTestAttachment? LabTestAttachment;
        public FormInPatientCareForNurse()
        {
            InitializeComponent();
            GridViewElementInfo.RowTemplate.Height = 20;
            DataGridViewCurrencyColumn currencyColumn = (DataGridViewCurrencyColumn)GridViewInpatientCareProcedureInfo.Columns["Fees"];
            if (int.TryParse(Global.Company.PrimaryCurrency.RoundingPrecision.ToString(), out int decimalPlaces)) currencyColumn.DecimalPlaces = decimalPlaces;
        }

        private void FormInPatientCareForNurse_Load(object sender, EventArgs e)
        {
            LoadInpatientData(PatientId);
            StatusLabelInpatientCareErrorMsg.Text = "";
        }

        private void BtnIPVitalEntrySave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (VitalEntryInpatientCare.VitalEntryValidationResult())
            {
                Vital vital = VitalEntryInpatientCare.GetVitalDetails();
                if (vital.Weight != 0)
                {
                    vital.InPatientAdmissionId = PatientIpId;
                    if (vital.Id == 0L)
                    {
                        VitalEntryManager.Instance.AddVital(vital);
                    }
                    else
                    {
                        VitalEntryManager.Instance.UpdateVital(vital);
                    }
                    VitalEntryInpatientCare.Clear();
                    VitalHistoryInpatientCare.PatientId = PatientId;
                    StatusLabelInpatientCareErrorMsg.Text = SaveSuccessMsg;

                }
                else { return; }
            }
            else
            {
                VitalEntryInpatientCare.Focus();
                StatusLabelInpatientCareErrorMsg.Text = VitalEntryInpatientCare.ErrorMsg();
            }
            VitalGraphChartInpatientCare.PatientId = PatientId;
            Cursor.Current = Cursors.Default;
        }
        private void BtnIPVitalEntryCancel_Click(object sender, EventArgs e)
        {
            StatusLabelInpatientCareErrorMsg.Text = "";
            VitalEntryInpatientCare.Clear();
            VitalEntryInpatientCare.Select();
        }

        private void loadLabTestHistory()
        {
            GridViewInpatientCareLabTestHistory.PatientId = PatientId;
        }
        private void GridViewLabTestHistory_Load(object sender, EventArgs e)
        {
            BtnInpatientCareLabtestPrintRequisition.Enabled = false;
            BtnInpatientCareImgSave.Enabled = false;
            BtnInpatientCareImgAdd.Enabled = false;
            BtnInpatientCareImgCancel.Enabled = false;
            BtnInpatientCareLabTestElementCancel.Enabled = false;
            BtnInpatientCareLabTestElementSave.Enabled = false;
            StatusLabelInpatientCareErrorMsg.Text = "";
            if (GridViewInpatientCareLabTestHistory.LTestId != 0L)
            {
                BtnInpatientCareImgAdd.Enabled = true;
                LoadLabTestDetails(GridViewInpatientCareLabTestHistory.LTestId);
                LoadLabTestAttachment(GridViewInpatientCareLabTestHistory.LTestId);
            }
        }
        private void LoadLabTestDetails(long LabTestId)
        {
            int i = 0;
            GridViewElementInfo.Rows.Clear();
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
                            comboBoxCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                            comboBoxCell.FlatStyle = FlatStyle.Flat;
                            if (!comboBoxCell.Items.Contains(consElement.Class))
                            {
                                comboBoxCell.Items.Add(consElement.Class);
                            }
                            rowMap[name].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value = consElement.Class;
                        }
                        else
                        {
                            int newRowIdx = GridViewElementInfo.Rows.Add();
                            DataGridViewRow newRow = GridViewElementInfo.Rows[newRowIdx];

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

                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.NAME].Style.BackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.NAME].Style.SelectionBackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.UOM].Style.BackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.UOM].Style.SelectionBackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Style.BackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Style.BackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Style.BackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Style.SelectionBackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].Style.SelectionBackColor = Color.LightGray;
                            GridViewElementInfo.Rows[i].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].Style.SelectionBackColor = Color.LightGray;
                            i++;
                        }
                    }
                    BtnInpatientCareLabTestElementCancel.Enabled = true;
                    BtnInpatientCareLabTestElementSave.Enabled = true;
                    BtnInpatientCareLabtestPrintRequisition.Enabled = true;
                }
            }
            else
            {

            }
        }
        private void LoadLabTestAttachment(long ConsLabTestId)
        {
            PictureBoxInpatientCareImage.Image = null;
            GridViewInpatientCareImage.Rows.Clear();
            GridViewInpatientCareImage.Rows.Add();
            IList<LabTestAttachment> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestAttachmentByLabtestId(ConsLabTestId);
            if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
            {
                GridViewInpatientCareImage.Rows.Add(lConsultedLabTestElements.Count);
                int i = 0;
                foreach (LabTestAttachment LabtestAttachment in lConsultedLabTestElements)
                {
                    GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                    GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value = LabtestAttachment.FileName;
                    GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.DESC].Value = LabtestAttachment.Description;
                    ButtonToggle(true, i);
                    GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value = LabtestAttachment.Id;
                    GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value = LabtestAttachment.Attachment;
                    GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.TYPE].Value = LabtestAttachment.FileType;
                    i++;
                }
                GridViewInpatientCareImage.CurrentCell = GridViewInpatientCareImage.Rows[0].Cells[0];
                GridViewLabTestImage_CellEnter(this.GridViewInpatientCareImage, new DataGridViewCellEventArgs(0, 0));
                BtnInpatientCareImgSave.Enabled = true;
                BtnInpatientCareImgAdd.Enabled = true;
                BtnInpatientCareImgCancel.Enabled = true;
            }
            else
            {
                EnableViewer();
            }
        }
        private bool ValidateTestElementDetail()
        {
            StatusLabelInpatientCareErrorMsg.Text = string.Empty;
            foreach (DataGridViewRow row in GridViewElementInfo.Rows)
            {
                DataGridViewComboBoxCell? comboBoxCell = GridViewElementInfo.Rows[GridViewElementInfo.CurrentCell.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM] as DataGridViewComboBoxCell;
                if (comboBoxCell != null)
                {
                    if (comboBoxCell.Value == null || string.IsNullOrEmpty(comboBoxCell.FormattedValue.ToString()))
                    {
                        return true;
                    }
                }
                for (int i = 3; i < 6; i++)
                {
                    if (i == 4 || i == 5 || i == 3) { continue; }
                    if (row.Cells[i].Value == null || string.IsNullOrEmpty(row.Cells[i].Value.ToString()) || double.Parse(row.Cells[i].Value.ToString()!) == 0)
                    {
                        StatusLabelInpatientCareErrorMsg.Text = string.Format(EnterLabtestElementErrorMsg, GridViewElementInfo.Columns[i].HeaderText);
                        GridViewElementInfo.CurrentCell = GridViewElementInfo[i, row.Index];
                        GridViewElementInfo.BeginEdit(true);
                        return false;
                    }
                }
            }
            return true;
        }

        private void GridViewElementInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SNO].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.NAME].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].ReadOnly = false;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RESULT].ReadOnly = false;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].ReadOnly = false;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].ReadOnly = true;
            GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.ID].ReadOnly = true;

            if (GridViewElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS && (GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value == null || string.IsNullOrEmpty(GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value.ToString())))
            {
                GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].ReadOnly = true;
            }
            if (GridViewElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS && (GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value == null || string.IsNullOrEmpty(GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value.ToString())))
            {
                GridViewElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].ReadOnly = true;
            }
        }
        private void ButtonToggle(bool btnStatus, int cellIndex)
        {
            if (btnStatus)
            {
                GridViewInpatientCareImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "\u2B73";
                GridViewInpatientCareImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                GridViewInpatientCareImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "Download";
            }
            else
            {
                GridViewInpatientCareImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "+";
                GridViewInpatientCareImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                GridViewInpatientCareImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "";
            }
        }

        private void GridViewLabTestImage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                StatusLabelInpatientCareErrorMsg.Text = "";
                if (e.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE)
                {
                    UploadDownload();
                }
                else if (e.ColumnIndex == (int)AddFileUploadGridColumn.REMOVE && e.RowIndex != GridViewInpatientCareImage.Rows.Count - 1)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].Value.ToString() + "?", "Delete Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (e.RowIndex == 0 && GridViewInpatientCareImage.Rows.Count <= 1)
                        {
                            GridViewInpatientCareImage.Rows.RemoveAt(e.RowIndex);
                            GridViewInpatientCareImage.Rows.Add();
                        }
                        else
                        {
                            GridViewInpatientCareImage.Rows.RemoveAt(e.RowIndex);
                            for (int i = 0; i < GridViewInpatientCareImage.Rows.Count - 1; i++)
                            {
                                GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                            }
                        }
                        ReSequence();
                    }
                }
            }
        }
        private void UploadDownload()
        {
            if (GridViewInpatientCareImage.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE && GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText == "Edit")
            {
                LabTestAttachment = null!;
                FormUploadDocumentWithPreview FormUploadDocumentWithPreview = new FormUploadDocumentWithPreview(this);
                FormUploadDocumentWithPreview.ShowDialog();
                if (LabTestAttachment != null)
                {
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.ID].Value = null;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value = LabTestAttachment.FileType;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.SNO].Value = GridViewInpatientCareImage.Rows.Count;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value = LabTestAttachment.FileName;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.DESC].Value = LabTestAttachment.Description;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value = LabTestAttachment.Attachment;
                    ButtonToggle(true, GridViewInpatientCareImage.CurrentRow.Index);
                    GridViewInpatientCareImage.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    int Index = GridViewInpatientCareImage.CurrentRow.Index;
                    GridViewInpatientCareImage.Rows.Add();
                    GridViewInpatientCareImage.CurrentCell = GridViewInpatientCareImage.Rows[Index].Cells[0];
                    GridViewLabTestImage_CellEnter(this.GridViewInpatientCareImage, new DataGridViewCellEventArgs(0, Index));
                }
            }
            else if (GridViewInpatientCareImage.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE && GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText == "Download")
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString() + ComboUtils.GetExtension((FileType)GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value);
                    saveFileDialog.DefaultExt = ComboUtils.GetExtension((FileType)GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value);
                    saveFileDialog.AddExtension = true;
                    if (DialogResult.OK == saveFileDialog.ShowDialog())
                    {
                        byte[] array = (byte[])GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value;
                        File.WriteAllBytes(saveFileDialog.FileName /*+ Path.GetExtension(saveFileDialog.FileName)*/, array);
                    }
                }
            }
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewInpatientCareImage.Rows.Count; i++)
            {
                GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
            }
        }

        private void GridViewLabTestImage_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].Value = GridViewInpatientCareImage.Rows.Count;
        }
        private void EnableViewer()
        {
            PictureBoxInpatientCareImage.Visible = false;
            PictureBoxInpatientCareImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            PictureBoxInpatientCareImage.Image = null;
            DocBrowserInpatientCareDocument.LoadDocument("about:blank");
            PdfDocumentViewInpatientCareDocument.Refresh();
            DocBrowserInpatientCareDocument.Visible = false;
            PdfDocumentViewInpatientCareDocument.Visible = false;
        }

        private void GridViewLabTestImage_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.NAME].ReadOnly = true;
            GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.DESC].ReadOnly = true;
            GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].ReadOnly = true;
            GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ReadOnly = true;
            GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.REMOVE].ReadOnly = true;
            if (GridViewInpatientCareImage.Rows.Count > 1)
            {
                BtnInpatientCareImgSave.Enabled = true;
                BtnInpatientCareImgAdd.Enabled = true;
                BtnInpatientCareImgCancel.Enabled = true;
            }
            EnableViewer();
            if ((e.ColumnIndex <= (int)AddFileUploadGridColumn.CHOOSE) && e.RowIndex != GridViewInpatientCareImage.Rows.Count - 1)
            {
                PictureBoxInpatientCareImage.BackgroundImage = null;
                if (e.RowIndex > -1)
                {
                    if (GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.ID].Value != null)
                    {
                        LabTestAttachment LabTestAttachment = ConsultationNoteManager.Instance.GetConsLabTestAttachmentById(long.Parse(GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.ID].Value.ToString()));
                        if (LabTestAttachment != null)
                        {
                            if (LabTestAttachment.FileType == FileType.JPEG || LabTestAttachment.FileType == FileType.JPG || LabTestAttachment.FileType == FileType.PNG)
                            {
                                PictureBoxInpatientCareImage.Visible = true;
                                MemoryStream Stream = new MemoryStream(LabTestAttachment.Attachment);
                                PictureBoxInpatientCareImage.Image = System.Drawing.Image.FromStream(Stream);
                                PictureBoxInpatientCareImage.SizeMode = PictureBoxSizeMode.StretchImage;
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
                                    writer.Write(LabTestAttachment.Attachment, 0, LabTestAttachment.Attachment.Length);
                                    writer.Close();
                                    if (LabTestAttachment.FileType == FileType.DOC || LabTestAttachment.FileType == FileType.DOCX)
                                    {
                                        DocBrowserInpatientCareDocument.Visible = true;
                                        DocBrowserInpatientCareDocument.LoadDocument(Path.GetTempPath() + LabTestAttachment.FileName + ComboUtils.GetExtension(LabTestAttachment.FileType));
                                    }
                                    else
                                    {
                                        PdfDocumentViewInpatientCareDocument.Visible = true;
                                        PdfDocumentViewInpatientCareDocument.Load(Path.GetTempPath() + LabTestAttachment.FileName + ComboUtils.GetExtension(LabTestAttachment.FileType));
                                    }
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine(exc.HResult);
                                }
                            }
                        }
                    }
                    if (GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.PATH].Value != null
                        && !string.IsNullOrEmpty(GridViewInpatientCareImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.PATH].Value.ToString()))
                    {
                        FileType FileType = (FileType)GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value;
                        string Filename = GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString();
                        byte[] array = (byte[])GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value;
                        if (FileType == FileType.JPEG || FileType == FileType.JPG || FileType == FileType.PNG)
                        {
                            PictureBoxInpatientCareImage.Visible = true;
                            MemoryStream Stream = new MemoryStream(array);
                            PictureBoxInpatientCareImage.Image = System.Drawing.Image.FromStream(Stream);
                            PictureBoxInpatientCareImage.SizeMode = PictureBoxSizeMode.StretchImage;
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
                                    DocBrowserInpatientCareDocument.Visible = true;
                                    DocBrowserInpatientCareDocument.LoadDocument(Path.GetTempPath() + Filename + ComboUtils.GetExtension(FileType));
                                }
                                else
                                {
                                    PdfDocumentViewInpatientCareDocument.Visible = true;
                                    PdfDocumentViewInpatientCareDocument.Load(Path.GetTempPath() + Filename + ComboUtils.GetExtension(FileType));
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

        private void BtnSaveConsLabTestElement_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            if (GridViewElementInfo.Rows.Count > 0)
            {
                if (ValidateTestElementDetail())
                {
                    StatusLabelInpatientCareErrorMsg.Text = string.Empty;
                    IList<ConsultedLabTestElements> lConsultedLabTestElements = new List<ConsultedLabTestElements>();
                    foreach (DataGridViewRow row in GridViewElementInfo.Rows)
                    {
                        ConsultedLabTestElements ConsultedLabTestElements = new ConsultedLabTestElements();
                        if (row.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value != null)
                        {
                            long Id = long.Parse(row.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value.ToString()!);
                            ConsultedLabTestElements ConsultedLabTestElementsFromDB = ConsultationNoteManager.Instance.GetConsultedLabTestElementsById(Id);
                            if (ConsultedLabTestElementsFromDB != null)
                            {
                                ConsultedLabTestElements.ConsLabTestId = ConsultedLabTestElementsFromDB.ConsLabTestId;
                                ConsultedLabTestElements.CompanyId = Global.Company.CompanyId;
                                ConsultedLabTestElements.Id = Id;
                                ConsultedLabTestElements.MedicalTestElementId = ConsultedLabTestElementsFromDB.MedicalTestElementId;
                                ConsultedLabTestElements.Name = ConsultedLabTestElementsFromDB.Name;
                                ConsultedLabTestElements.UomId = ConsultedLabTestElementsFromDB.MedicalTestElement.UomId;
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
                                ConsultedLabTestElements.ConsLabTestId = GridViewInpatientCareLabTestHistory.LTestId;
                                ConsultedLabTestElements.CompanyId = Global.Company.CompanyId;
                                ConsultedLabTestElements.Id = 0L;
                                ConsultedLabTestElements.MedicalTestElementId = Element.Id;
                                ConsultedLabTestElements.Name = Element.Name;
                                ConsultedLabTestElements.Uom = (MedicalTestUOM)row.Cells[(int)MedicalLabTestElementsGridColumn.UOM].Value;
                                ConsultedLabTestElements.Class = row.Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value?.ToString() ?? "";
                                ConsultedLabTestElements.SingleValue = row.Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].Value?.ToString() ?? "";
                                ConsultedLabTestElements.ResultDescription = row.Cells[(int)MedicalLabTestElementsGridColumn.RESULT].Value?.ToString() ?? "";
                                lConsultedLabTestElements.Add(ConsultedLabTestElements);
                            }
                        }
                    }
                    ConsultationNoteManager.Instance.UpdateConsultedLabTestElements(lConsultedLabTestElements);
                    GridViewLabTestHistory_Load(sender, e);
                    StatusLabelInpatientCareErrorMsg.Text = SaveSuccessMsg;
                }
            }
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void BtnCancelConsLabTestElement_Click(object sender, EventArgs e)
        {
            GridViewLabTestHistory_Load(sender, e);
        }

        private void BtnLabTestImgSave_Click(object sender, EventArgs e)
        {
            StatusLabelInpatientCareErrorMsg.Text = string.Empty;
            IList<LabTestAttachment> LabTestAttachment = new List<LabTestAttachment>();
            for (int i = 0; i < GridViewInpatientCareImage.Rows.Count - 1; i++)
            {
                LabTestAttachment lLabTestAttachment = new LabTestAttachment();
                if (GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value != null)
                {
                    LabTestAttachment llLabTestAttachment = ConsultationNoteManager.Instance.GetConsLabTestAttachmentById(long.Parse(GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value.ToString()!));
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
                    lLabTestAttachment.FileType = (FileType)GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.TYPE].Value;
                    lLabTestAttachment.FileName = GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString();
                    lLabTestAttachment.Description = GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.DESC].Value.ToString();
                    lLabTestAttachment.Attachment = (byte[])GridViewInpatientCareImage.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value;
                }
                lLabTestAttachment.ConsLabTestId = GridViewInpatientCareLabTestHistory.LTestId;
                LabTestAttachment.Add(lLabTestAttachment);
            }
            ConsultationNoteManager.Instance.UpdateConsultedLabTestAttachment(LabTestAttachment, GridViewInpatientCareLabTestHistory.LTestId);
            StatusLabelInpatientCareErrorMsg.Text = SaveSuccessMsg;
        }

        private void BtnLabTestImgCancel_Click(object sender, EventArgs e)
        {
            StatusLabelInpatientCareErrorMsg.Text = "";
            LoadLabTestAttachment(GridViewInpatientCareLabTestHistory.LTestId);
            PictureBoxInpatientCareImage.BackgroundImage = null;
        }

        private void BtnLabTestPrintRequisition_Click(object sender, EventArgs e)
        {
            if (GridViewInpatientCareLabTestHistory.LTestId != 0L && PatientId != 0L)
            {
                LabTestPrinting LabTestPrinting = new LabTestPrinting();
                LabTestPrinting.ExportOrPrintToFile(GridViewInpatientCareLabTestHistory.LTestId, PatientId, "LabTest Result's", "pdf", true);
            }
            BtnInpatientCareLabTestElementSave.Focus();
        }

        private void BtnProcedureFilter_Click(object sender, EventArgs e)
        {
            LoadPatientProcedure();
        }
        private void ResetForm()
        {
            StatusLabelInpatientCareErrorMsg.Text = "";
            GridViewInpatientCareProcedureInfo.Rows.Clear();
            DateTimePickerInpatientCareProcedureFromDate.Format = Global.Company.DateFormat;
            DateTimePickerInpatientCareProcedureFromDate.Date = Global.getTransactionDate().AddDays(-30);
            DateTimePickerInpatientCareProcedureToDate.Format = Global.Company.DateFormat;
            DateTimePickerInpatientCareProcedureToDate.Date = Global.getTransactionDate();
            EnableButton(false);
        }
        private void EnableButton(bool Enable)
        {
            BtnInpatientCareProcedureUpdate.Enabled = Enable;
        }
        private List<ProcedureStatus> GetStatus()
        {
            List<ProcedureStatus> Status = new List<ProcedureStatus>();
            foreach (ComboTreeNode node in ComboBoxInpatientCareProcedureStatus.Nodes)
            {
                if (node.Checked)
                {
                    if (node.Text == "Requested")
                    {
                        Status.Add(ProcedureStatus.REQUESTED);
                    }
                    else if (node.Text == "In Progress")
                    {
                        Status.Add(ProcedureStatus.INPROGRESS);
                    }
                    else if (node.Text == "Cancelled")
                    {
                        Status.Add(ProcedureStatus.CANCEL);
                    }
                    else
                    {
                        Status.Add(ProcedureStatus.COMPLETED);
                    }
                }
            }
            return Status;
        }
        private bool ValidateForm()
        {
            if (GetStatus().Count == 0)
            {
                StatusLabelInpatientCareErrorMsg.Text = ChooseStatusErrorMsg;
                ComboBoxInpatientCareProcedureStatus.Focus();
                return false;
            }
            if (DateTimePickerInpatientCareProcedureFromDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerInpatientCareProcedureFromDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                StatusLabelInpatientCareErrorMsg.Text = EnterValidDateErrorMsg;
                DateTimePickerInpatientCareProcedureFromDate.Focus();
                return false;
            }
            if (DateTimePickerInpatientCareProcedureToDate.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerInpatientCareProcedureToDate.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                StatusLabelInpatientCareErrorMsg.Text = EnterValidDateErrorMsg;
                DateTimePickerInpatientCareProcedureToDate.Focus();
                return false;
            }
            return true;
        }
        private void LoadPatientProcedure()
        {
            StatusLabelInpatientCareErrorMsg.Text = "";
            GridViewInpatientCareProcedureInfo.Rows.Clear();
            if (ValidateForm())
            {
                RptConsultedProcedure RptConsultedProcedure = new RptConsultedProcedure();
                RptConsultedProcedure.PatientId = PatientId;
                RptConsultedProcedure.FromDate = (DateTime)DateTimePickerInpatientCareProcedureFromDate.Date!;
                RptConsultedProcedure.ToDate = (DateTime)DateTimePickerInpatientCareProcedureToDate.Date!;
                RptConsultedProcedure.CompanyId = Global.Company.CompanyId;
                RptConsultedProcedure.ProceduresStatuses = GetStatus();
                List<ProcedureStatus> Status = GetStatus();
                RptConsultedProcedure.GenerateReport();
                if (RptConsultedProcedure.LineItems.Count > 0)
                {
                    IList<ConsultedProcedureLineItem> FilteredLineItems = RptConsultedProcedure.LineItems.Where(x => Status.Contains(x.Status)).ToList();
                    if (FilteredLineItems.Count > 0)
                    {
                        EnableButton(true);
                        int i = 0;
                        foreach (ConsultedProcedureLineItem Item in FilteredLineItems)
                        {
                            GridViewInpatientCareProcedureInfo.Rows.Add();
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.SNO].Value = i + 1;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.DATE].Value = Item.Date;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.NAME].Value = Item.Name;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.DESC].Value = Item.Desc;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.REQUBY].Value = Item.RequestedBy;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.STATUS].Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Item.Status.ToString().ToLower());
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.PERBY].Value = Item.PerformBy;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.PERON].Value = Item.PerformOn != null ? Item.PerformOn : null;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.NOTE].Value = Item.Note;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.FEE].Value = Item.Fee;
                            GridViewInpatientCareProcedureInfo.Rows[i].Cells[(int)ConsultedProcedureTableColumn.ID].Value = Item.Id;
                            i++;
                        }
                    }
                    else
                    {
                        StatusLabelInpatientCareErrorMsg.Text = string.Format(NoRecordByErrorMsg, "Procedure ");
                    }
                }
                else
                {
                    StatusLabelInpatientCareErrorMsg.Text = string.Format(NoRecordByErrorMsg, "Procedure ");
                }
            }
        }

        private void GridViewProcedureInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                PerformUpload();
            }
        }

        private void BtnProcedureUpdate_Click(object sender, EventArgs e)
        {
            PerformUpload();
        }
        private void PerformUpload()
        {
            if (GridViewInpatientCareProcedureInfo.CurrentRow != null)
            {
                long CPId = long.Parse(GridViewInpatientCareProcedureInfo.CurrentRow.Cells[(int)ConsultedProcedureTableColumn.ID].Value.ToString()!);
                FormPatientProcedure PatientProcedure = new FormPatientProcedure(this);
                PatientProcedure.ProcedureId = CPId;
                PatientProcedure.PatientId = ConsultationNoteManager.Instance.GetConsultedProceduresById(CPId).ConsultationNote.PatientId;
                PatientProcedure.PatientType = PatientTypes.ImPatient;
                PatientProcedure.ShowDialog();
                ResetForm();
                LoadPatientProcedure();
            }
        }

        private void GridViewProcedureInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void BtnInpatientCareImgAdd_Click(object sender, EventArgs e)
        {
            if (GridViewInpatientCareImage.Rows.Count - 1 == GridViewInpatientCareImage.CurrentRow.Index)
            {
                LabTestAttachment = null;
                FormUploadDocumentWithPreview FormUploadDocumentWithPreview = new FormUploadDocumentWithPreview(this);
                FormUploadDocumentWithPreview.ShowDialog();
                if (LabTestAttachment != null)
                {
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.ID].Value = null;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value = LabTestAttachment.FileType;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.SNO].Value = GridViewInpatientCareImage.Rows.Count;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value = LabTestAttachment.FileName;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.DESC].Value = LabTestAttachment.Description;
                    GridViewInpatientCareImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value = LabTestAttachment.Attachment;
                    ButtonToggle(true, GridViewInpatientCareImage.CurrentRow.Index);
                    GridViewInpatientCareImage.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    int Index = GridViewInpatientCareImage.CurrentRow.Index;
                    GridViewInpatientCareImage.Rows.Add();
                    GridViewInpatientCareImage.CurrentCell = GridViewInpatientCareImage.Rows[Index].Cells[0];
                    GridViewLabTestImage_CellEnter(this.GridViewInpatientCareImage, new DataGridViewCellEventArgs(0, Index));
                }
            }
        }

        private void PatientLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (PatientId > 0)
            {
                StatusLabelInpatientCareErrorMsg.Text = "";
                PatientRegistration PatientRegistration = new PatientRegistration(this);
                PatientRegistration.CreatePatientOnLoad = true;
                PatientRegistration.PatientId = PatientId;
                PatientRegistration.ShowDialog();
                LoadInpatientData(PatientId);
            }
        }
        private void LoadInpatientData(long InPatientId)
        {
            PatientInfoMinInpatientCare.Type = PatientTypes.InPatient;
            PatientInfoMinInpatientCare.PatientId = InPatientId;
            VitalHistoryInpatientCare.PatientId = InPatientId;
            VitalEntryInpatientCare.PatientId = InPatientId;
            ResetForm();
            LoadPatientProcedure();
            VitalGraphChartInpatientCare.PatientId = InPatientId;
            loadLabTestHistory();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F3)
            {
                BtnInpatientCareProcedureUpdate.PerformClick();
            }
            else if (keyData == Keys.F9)
            {
                BtnInpatientCareLabtestPrintRequisition.PerformClick();
            }
            else if (keyData == Keys.Escape)
            {
                if (tabControl1.SelectedTab == LabTestResults && TabControlInpatientCareLabtest.SelectedTab == TabInPatientCareLabtestImageDocument)
                {
                    BtnInpatientCareImgCancel.PerformClick();
                }
                else if (tabControl1.SelectedTab == LabTestResults && TabControlInpatientCareLabtest.SelectedTab == TabInPatientCareLabtestDetails)
                {
                    BtnInpatientCareLabTestElementCancel.PerformClick();
                }
            }
            else if (keyData == Keys.F8)
            {
                if (tabControl1.SelectedTab == LabTestResults && TabControlInpatientCareLabtest.SelectedTab == TabInPatientCareLabtestImageDocument)
                {
                    BtnInpatientCareImgSave.PerformClick();
                }
                else if (tabControl1.SelectedTab == LabTestResults && TabControlInpatientCareLabtest.SelectedTab == TabInPatientCareLabtestDetails)
                {
                    BtnInpatientCareLabTestElementSave.PerformClick();
                }
            }
            StatusLabelInpatientCareErrorMsg.Text = VitalEntryInpatientCare.ErrorMsg();
            if (tabControl1.SelectedTab.Name == "VitalEntry")
            {
                StatusLabelInpatientCareErrorMsg.Text = VitalEntryInpatientCare.ErrorMsg();
            }
            else
            {
                StatusLabelInpatientCareErrorMsg.Text = " ";
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void VitalHistoryInpatientCare_DoubleClick(object sender, EventArgs e)
        {
            StatusLabelInpatientCareErrorMsg.Text = "";
            if (VitalHistoryInpatientCare.VitalsId != 0L)
            {
                VitalEntryInpatientCare.VitalId = (long)VitalHistoryInpatientCare.VitalsId!;
            }
        }

        private void GridViewElementInfo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                var currentCell = GridViewElementInfo[e.ColumnIndex, e.RowIndex];
                if (currentCell.EditedFormattedValue != null)
                {
                    currentCell.Value = currentCell.EditedFormattedValue;
                }
            }
        }

        private void GridViewElementInfo_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                GridViewElementInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
                GridViewElementInfo.CurrentCell.Value = GridViewElementInfo.CurrentCell.EditedFormattedValue;
            }
        }

        private void GridViewElementInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (GridViewElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                ComboBox? classComboBox = e.Control as ComboBox;
                if (classComboBox != null && GridViewElementInfo.CurrentRow.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value != null)
                {
                    classComboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    classComboBox.FlatStyle = FlatStyle.Flat;

                    var row = GridViewElementInfo.CurrentRow;
                    long LabTestId = Convert.ToInt64(row.Cells[(int)MedicalLabTestElementsGridColumn.TESTID].Value);
                    ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(LabTestId);
                    IList<MedicalTestElement> lLabTestElements = MedicalTestManager.Instance.ListMedicalTestElementByMedicalTestId(consultedLabTest.MedicalTestId);

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
                    row.Cells[(int)MedicalLabTestElementsGridColumn.CLASS] = ClasscomboBoxCell;

                    classComboBox!.SelectedIndexChanged -= ClassComboBox_SelectedIndexChanged;
                    classComboBox.SelectedIndexChanged += ClassComboBox_SelectedIndexChanged;

                    classComboBox.BackColor = SystemColors.Window;
                    classComboBox.ForeColor = SystemColors.WindowText;
                }
            }
            if (GridViewElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                ComboBox? classComboBox = e.Control as ComboBox;
                if (classComboBox != null)
                {
                    classComboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    classComboBox.FlatStyle = FlatStyle.Flat;

                    classComboBox.SelectedIndexChanged -= SubClassComboBox_SelectedIndexChanged;
                    classComboBox.SelectedIndexChanged += SubClassComboBox_SelectedIndexChanged;
                }
            }
        }
        private void ClassComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (GridViewElementInfo.CurrentRow != null && GridViewElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                var ClassComboBox = sender as ComboBox;
                var selectedClass = ClassComboBox?.SelectedItem?.ToString()!;
                var classvalue = GridViewElementInfo.CurrentCell.EditedFormattedValue.ToString();
                var row = GridViewElementInfo.CurrentRow;
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
        private void SubClassComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (GridViewElementInfo.CurrentRow != null && GridViewElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                var subClassComboBox = sender as ComboBox;
                var selectedSubClass = subClassComboBox?.SelectedItem?.ToString();

                if (selectedSubClass != null)
                {
                    var row = GridViewElementInfo.CurrentRow;
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
        private void GridViewElementInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }
        private void GridViewElementInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            StatusLabelInpatientCareErrorMsg.Text = "";
        }

        private void VitalHistoryInpatientCare_Enter(object sender, EventArgs e)
        {
            StatusLabelInpatientCareErrorMsg.Text = "";
        }
    }
}
