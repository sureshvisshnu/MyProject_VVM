using fa.api.Hms;
using fa.libraries.utils;
using fa.model.hms.common;
using fa.model.Hms.Master;
using fa.views.utils.Hms;
using Fa.api.Hms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using fa.model.Accounting.Masters;

namespace fa.views.hms.patient
{
    public enum GridViewLabTestResultElementInfo
    {
        SNO, NAME, UOM, VARIATION, RANGETYPE, RESULT, RANGEVALUE, RANGEFROM, RANGETO, ID, TESTID
    }
    public partial class FormLabTestResults : FormPatientBase
    {
        public static string SaveSuccessMsg = "Saved success.";
        public static string EnterLabtestElementErrorMsg = "Please enter {0}";
        public static string UpLoadFileErrorMsg = "Please Upload less than 5mb";
        long PatientId = 0L;
        public LabTestAttachment LabTestAttachment;
        FormPatientBase parent = null;
        public FormLabTestResults(object sender)
        {
            InitializeComponent();
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
            GridViewLabTestResultLabtestElementInfo.RowTemplate.Height = 20;
        }
        private void FormLabTestResults_Load(object sender, EventArgs e)
        {
            PatientId = long.Parse(parent.PatientIdTransport.Text);
            loadLabTestHistory();
        }
        private void loadLabTestHistory()
        {
            GridViewLabTestResultLabTestHistory.PatientId = PatientId;
        }
        private void GridViewLabTestHistory_Load(object sender, EventArgs e)
        {
            BtnLabTestResultLabTestPrintRequisition.Enabled = false;
            BtnLabTestResultLabTestImgSave.Enabled = false;
            BtnLabTestResultLabTestImgCancel.Enabled = false;
            BtnLabTestResultLabTestElementCancel.Enabled = false;
            BtnLabTestResultLabTestElementSave.Enabled = false;
            if (GridViewLabTestResultLabTestHistory.LTestId != 0L)
            {
                LoadLabTestDetails(GridViewLabTestResultLabTestHistory.LTestId);
                LoadLabTestAttachment(GridViewLabTestResultLabTestHistory.LTestId);
            }

        }
        private void LoadLabTestDetails(long LabTestId)
        {
            GridViewLabTestResultLabtestElementInfo.Rows.Clear();
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
                            int newRowIdx = GridViewLabTestResultLabtestElementInfo.Rows.Add();
                            DataGridViewRow newRow = GridViewLabTestResultLabtestElementInfo.Rows[newRowIdx];

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

                    BtnLabTestResultLabTestElementCancel.Enabled = true;
                    BtnLabTestResultLabTestElementSave.Enabled = true;
                    BtnLabTestResultLabTestPrintRequisition.Enabled = true;
                }
            }
        }
        private bool ValidateTestElementDetail()
        {
            StatusLabelLabTestResultErrorMsg.Text = string.Empty;
            foreach (DataGridViewRow row in GridViewLabTestResultLabtestElementInfo.Rows)
            {
                int j = 2;
                if (row.Cells[j].Value == null || string.IsNullOrEmpty(row.Cells[j].FormattedValue.ToString()))
                {
                    return true;
                }
                for (int i = 3; i < 6; i++)
                {
                    if (i == 4 || i == 5 || i == 3) { continue; }
                    if (row.Cells[i].Value == null || string.IsNullOrEmpty(row.Cells[i].Value.ToString()))
                    {
                        StatusLabelLabTestResultErrorMsg.Text = string.Format(EnterLabtestElementErrorMsg, GridViewLabTestResultLabtestElementInfo.Columns[i].HeaderText);
                        GridViewLabTestResultLabtestElementInfo.CurrentCell = GridViewLabTestResultLabtestElementInfo[i, row.Index];
                        GridViewLabTestResultLabtestElementInfo.BeginEdit(true);
                        return false;
                    }
                }
            }
            return true;
        }
        private void BtnSaveConsLabTestElement_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            if (GridViewLabTestResultLabtestElementInfo.Rows.Count > 0)
            {
                if (ValidateTestElementDetail())
                {
                    IList<ConsultedLabTestElements> lConsultedLabTestElements = new List<ConsultedLabTestElements>();
                    foreach (DataGridViewRow row in GridViewLabTestResultLabtestElementInfo.Rows)
                    {
                        ConsultedLabTestElements ConsultedLabTestElements = new ConsultedLabTestElements();
                        if (row.Cells[(int)GridViewLabTestResultElementInfo.ID].Value != null)
                        {
                            long Id = long.Parse(row.Cells[(int)GridViewLabTestResultElementInfo.ID].Value.ToString()!);
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
                        else if (row.Cells[(int)GridViewLabTestResultElementInfo.TESTID].Value != null)
                        {
                            long TestId = long.Parse(row.Cells[(int)GridViewLabTestResultElementInfo.TESTID].Value.ToString()!);
                            MedicalTestElement Element = MedicalTestManager.Instance.GetMedicalTestElementById(TestId, Global.Company.CompanyId);
                            if (Element != null)
                            {
                                ConsultedLabTestElements.ConsLabTestId = GridViewLabTestResultLabTestHistory.LTestId;
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
                    StatusLabelLabTestResultErrorMsg.Text = SaveSuccessMsg;
                }
            }
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
        private void GridViewElementInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SNO].ReadOnly = true;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.NAME].ReadOnly = true;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.UOM].ReadOnly = true;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].ReadOnly = false;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RESULT].ReadOnly = false;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].ReadOnly = false;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SINGLEVALUE].ReadOnly = true;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGEFROM].ReadOnly = true;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.RANGETO].ReadOnly = true;
            GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.ID].ReadOnly = true;

            if (GridViewLabTestResultLabtestElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS && (GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value == null || string.IsNullOrEmpty(GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].Value.ToString())))
            {
                GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.SUBCLASS].ReadOnly = true;
            }
            if (GridViewLabTestResultLabtestElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS && (GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value == null || string.IsNullOrEmpty(GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].Value.ToString())))
            {
                GridViewLabTestResultLabtestElementInfo.Rows[e.RowIndex].Cells[(int)MedicalLabTestElementsGridColumn.CLASS].ReadOnly = true;
            }
        }
        private void BtnCancelConsLabTestElement_Click(object sender, EventArgs e)
        {
            GridViewLabTestHistory_Load(sender, e);
        }
        private void BtnLabTestPrintRequisition_Click(object sender, EventArgs e)
        {
            if (GridViewLabTestResultLabTestHistory.LTestId != 0L && PatientId != 0L)
            {
                LabTestPrinting LabTestPrinting = new LabTestPrinting();
                LabTestPrinting.ExportOrPrintToFile(GridViewLabTestResultLabTestHistory.LTestId, PatientId, "LabTest Result's", "pdf", true);
            }
        }
        private void LoadLabTestAttachment(long ConsLabTestId)
        {
            PictureBoxLabTestResultImage.Image = null;
            GridViewLabTestResultLabTestImage.Rows.Clear();
            GridViewLabTestResultLabTestImage.Rows.Add();
            IList<LabTestAttachment> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestAttachmentByLabtestId(ConsLabTestId);
            if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
            {
                GridViewLabTestResultLabTestImage.Rows.Add(lConsultedLabTestElements.Count);
                int i = 0;
                foreach (LabTestAttachment LabtestAttachment in lConsultedLabTestElements)
                {
                    GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                    GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value = LabtestAttachment.FileName;
                    GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.DESC].Value = LabtestAttachment.Description;
                    ButtonToggle(true, i);
                    GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value = LabtestAttachment.Id;
                    GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value = LabtestAttachment.Attachment;
                    GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.TYPE].Value = LabtestAttachment.FileType;
                    i++;
                }
                GridViewLabTestResultLabTestImage.CurrentCell = GridViewLabTestResultLabTestImage.Rows[0].Cells[0];
                GridViewLabTestImage_CellEnter(this.GridViewLabTestResultLabTestImage, new DataGridViewCellEventArgs(0, 0));
                BtnLabTestResultLabTestImgSave.Enabled = true;
                BtnLabTestResultLabTestImgCancel.Enabled = true;
            }
            else
            {
                EnableViewer();
            }
        }
        private void ButtonToggle(bool btnStatus, int cellIndex)
        {
            if (btnStatus)
            {
                GridViewLabTestResultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "\u2B73";
                GridViewLabTestResultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                GridViewLabTestResultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "Download";
            }
            else
            {
                GridViewLabTestResultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Value = "+";
                GridViewLabTestResultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].Style.Font = new Font("Verdana", 14, FontStyle.Bold);
                GridViewLabTestResultLabTestImage.Rows[cellIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText = "";
            }
        }
        private void GridViewLabTestImage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StatusLabelLabTestResultErrorMsg.Text = "";
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE)
                {
                    UploadDownload();
                }
                else if (e.ColumnIndex == (int)AddFileUploadGridColumn.REMOVE && e.RowIndex != GridViewLabTestResultLabTestImage.Rows.Count - 1)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete row " + GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].Value.ToString() + "?", "Delete Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (e.RowIndex == 0 && GridViewLabTestResultLabTestImage.Rows.Count <= 1)
                        {
                            GridViewLabTestResultLabTestImage.Rows.RemoveAt(e.RowIndex);
                            GridViewLabTestResultLabTestImage.Rows.Add();
                        }
                        else
                        {
                            GridViewLabTestResultLabTestImage.Rows.RemoveAt(e.RowIndex);
                            for (int i = 0; i < GridViewLabTestResultLabTestImage.Rows.Count - 1; i++)
                            {
                                GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
                            }
                        }
                        ReSequence();
                    }
                }
            }
        }
        private void UploadDownload()
        {
            if (GridViewLabTestResultLabTestImage.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE && GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText == "Edit")
            {
                LabTestAttachment = null;
                FormUploadDocumentWithPreview FormUploadDocumentWithPreview = new FormUploadDocumentWithPreview(this);
                FormUploadDocumentWithPreview.ShowDialog();
                if (LabTestAttachment != null)
                {
                    GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.ID].Value = null;
                    GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value = LabTestAttachment.FileType;
                    GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.SNO].Value = GridViewLabTestResultLabTestImage.Rows.Count;
                    GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value = LabTestAttachment.FileName;
                    GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.DESC].Value = LabTestAttachment.Description;
                    GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value = LabTestAttachment.Attachment;
                    ButtonToggle(true, GridViewLabTestResultLabTestImage.CurrentRow.Index);
                    GridViewLabTestResultLabTestImage.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    int Index = GridViewLabTestResultLabTestImage.CurrentRow.Index;
                    GridViewLabTestResultLabTestImage.Rows.Add();
                    GridViewLabTestResultLabTestImage.CurrentCell = GridViewLabTestResultLabTestImage.Rows[Index].Cells[0];
                    GridViewLabTestImage_CellEnter(this.GridViewLabTestResultLabTestImage, new DataGridViewCellEventArgs(0, Index));
                }
            }
            else if (GridViewLabTestResultLabTestImage.CurrentCell.ColumnIndex == (int)AddFileUploadGridColumn.CHOOSE && GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.CHOOSE].ToolTipText == "Download")
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString() + ComboUtils.GetExtension((FileType)GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value);
                    saveFileDialog.DefaultExt = ComboUtils.GetExtension((FileType)GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value);
                    saveFileDialog.AddExtension = true;
                    if (DialogResult.OK == saveFileDialog.ShowDialog())
                    {
                        byte[] array = (byte[])GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value;
                        File.WriteAllBytes(saveFileDialog.FileName /*+ Path.GetExtension(saveFileDialog.FileName)*/, array);
                    }
                }
            }
        }
        private void GridViewLabTestImage_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].Value = GridViewLabTestResultLabTestImage.Rows.Count;
        }
        private void ReSequence()
        {
            for (int i = 0; i < GridViewLabTestResultLabTestImage.Rows.Count; i++)
            {
                GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.SNO].Value = i + 1;
            }
        }
        private void BtnLabTestImgSave_Click(object sender, EventArgs e)
        {
            StatusLabelLabTestResultErrorMsg.Text = string.Empty;
            IList<LabTestAttachment> LabTestAttachment = new List<LabTestAttachment>();
            for (int i = 0; i < GridViewLabTestResultLabTestImage.Rows.Count - 1; i++)
            {
                LabTestAttachment lLabTestAttachment = new LabTestAttachment();
                if (GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value != null)
                {
                    LabTestAttachment llLabTestAttachment = ConsultationNoteManager.Instance.GetConsLabTestAttachmentById(long.Parse(GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.ID].Value.ToString()));
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
                    lLabTestAttachment.FileType = (FileType)GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.TYPE].Value;
                    lLabTestAttachment.FileName = GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString();
                    lLabTestAttachment.Description = GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.DESC].Value.ToString();
                    lLabTestAttachment.Attachment = (byte[])GridViewLabTestResultLabTestImage.Rows[i].Cells[(int)AddFileUploadGridColumn.PATH].Value;
                }
                lLabTestAttachment.ConsLabTestId = GridViewLabTestResultLabTestHistory.LTestId;
                LabTestAttachment.Add(lLabTestAttachment);
            }
            ConsultationNoteManager.Instance.UpdateConsultedLabTestAttachment(LabTestAttachment, GridViewLabTestResultLabTestHistory.LTestId);
            StatusLabelLabTestResultErrorMsg.Text = SaveSuccessMsg;
            BtnLabTestResultLabTestImgCancel.Enabled = false;
        }
        private void BtnLabTestImgCancel_Click(object sender, EventArgs e)
        {
            LoadLabTestAttachment(GridViewLabTestResultLabTestHistory.LTestId);
            PictureBoxLabTestResultImage.BackgroundImage = null;
        }
        private void EnableViewer()
        {
            PictureBoxLabTestResultImage.Visible = false;
            PictureBoxLabTestResultImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            PictureBoxLabTestResultImage.Image = null;
            DocBrowserLabtestDocument.LoadDocument("about:blank");
            PdfDocumentViewLabtestDocument.Refresh();
            DocBrowserLabtestDocument.Visible = false;
            PdfDocumentViewLabtestDocument.Visible = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F9))
            {
                if (TabControlLabtestResult.SelectedTab == TabLabtestResultLabtestDetails)
                {
                    BtnLabTestResultLabTestPrintRequisition.PerformClick();
                }
            }
            else if (keyData == (Keys.F8))
            {
                if (TabControlLabtestResult.SelectedTab == TabLabtestResultLabtestImageDocument)
                {
                    BtnLabTestResultLabTestImgSave.PerformClick();
                }
                else
                {
                    BtnLabTestResultLabTestElementSave.PerformClick();
                }
            }
            else if (keyData == (Keys.Escape))
            {
                if (TabControlLabtestResult.SelectedTab == TabLabtestResultLabtestImageDocument)
                {
                    BtnLabTestResultLabTestImgCancel.PerformClick();
                }
                else
                {
                    BtnLabTestResultLabTestElementCancel.PerformClick();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void GridViewLabTestImage_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.NAME].ReadOnly = true;
            GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.DESC].ReadOnly = true;
            GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.SNO].ReadOnly = true;
            GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.CHOOSE].ReadOnly = true;
            GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.REMOVE].ReadOnly = true;
            if (GridViewLabTestResultLabTestImage.Rows.Count > 1)
            {
                BtnLabTestResultLabTestImgSave.Enabled = true;
                BtnLabTestResultLabTestImgCancel.Enabled = true;
            }
            EnableViewer();
            if ((e.ColumnIndex <= (int)AddFileUploadGridColumn.CHOOSE) && e.RowIndex != GridViewLabTestResultLabTestImage.Rows.Count - 1)
            {
                PictureBoxLabTestResultImage.BackgroundImage = null;
                if (e.RowIndex > -1)
                {
                    if (GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.ID].Value != null)
                    {
                        LabTestAttachment LabTestAttachment = ConsultationNoteManager.Instance.GetConsLabTestAttachmentById(long.Parse(GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.ID].Value.ToString()));
                        if (LabTestAttachment != null)
                        {
                            if (LabTestAttachment.FileType == FileType.JPEG || LabTestAttachment.FileType == FileType.JPG || LabTestAttachment.FileType == FileType.PNG)
                            {
                                PictureBoxLabTestResultImage.Visible = true;
                                MemoryStream Stream = new MemoryStream(LabTestAttachment.Attachment);
                                PictureBoxLabTestResultImage.Image = System.Drawing.Image.FromStream(Stream);
                                PictureBoxLabTestResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
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
                                        DocBrowserLabtestDocument.Visible = true;
                                        DocBrowserLabtestDocument.LoadDocument(Path.GetTempPath() + LabTestAttachment.FileName + ComboUtils.GetExtension(LabTestAttachment.FileType));
                                    }
                                    else
                                    {
                                        PdfDocumentViewLabtestDocument.Visible = true;
                                        PdfDocumentViewLabtestDocument.Load(Path.GetTempPath() + LabTestAttachment.FileName + ComboUtils.GetExtension(LabTestAttachment.FileType));
                                    }
                                }
                                catch (Exception exc)
                                {
                                    Console.WriteLine(exc.HResult);
                                }
                            }
                        }
                    }
                    if (GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.PATH].Value != null
                        && !string.IsNullOrEmpty(GridViewLabTestResultLabTestImage.Rows[e.RowIndex].Cells[(int)AddFileUploadGridColumn.PATH].Value.ToString()))
                    {
                        FileType FileType = (FileType)GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.TYPE].Value;
                        string Filename = GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.NAME].Value.ToString()!;
                        byte[] array = (byte[])GridViewLabTestResultLabTestImage.CurrentRow.Cells[(int)AddFileUploadGridColumn.PATH].Value;
                        if (FileType == FileType.JPEG || FileType == FileType.JPG || FileType == FileType.PNG)
                        {
                            PictureBoxLabTestResultImage.Visible = true;
                            MemoryStream Stream = new MemoryStream(array);
                            PictureBoxLabTestResultImage.Image = System.Drawing.Image.FromStream(Stream);
                            PictureBoxLabTestResultImage.SizeMode = PictureBoxSizeMode.StretchImage;
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
                                    DocBrowserLabtestDocument.Visible = true;
                                    DocBrowserLabtestDocument.LoadDocument(Path.GetTempPath() + Filename + ComboUtils.GetExtension(FileType));
                                }
                                else
                                {
                                    PdfDocumentViewLabtestDocument.Visible = true;
                                    PdfDocumentViewLabtestDocument.Load(Path.GetTempPath() + Filename + ComboUtils.GetExtension(FileType));
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

        private void GridViewLabTestResultLabtestElementInfo_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                var currentCell = GridViewLabTestResultLabtestElementInfo[e.ColumnIndex, e.RowIndex];
                if (currentCell.EditedFormattedValue != null)
                {
                    currentCell.Value = currentCell.EditedFormattedValue;
                }
            }
        }

        private void GridViewLabTestResultLabtestElementInfo_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                GridViewLabTestResultLabtestElementInfo.CommitEdit(DataGridViewDataErrorContexts.Commit);
                GridViewLabTestResultLabtestElementInfo.CurrentCell.Value = GridViewLabTestResultLabtestElementInfo.CurrentCell.EditedFormattedValue;
            }
        }

        private void GridViewLabTestResultLabtestElementInfo_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (GridViewLabTestResultLabtestElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                ComboBox? classComboBox = e.Control as ComboBox;
                if (classComboBox != null && GridViewLabTestResultLabtestElementInfo.CurrentRow.Cells[(int)MedicalLabTestElementsGridColumn.ID].Value != null)
                {
                    var row = GridViewLabTestResultLabtestElementInfo.CurrentRow;
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

                    classComboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    classComboBox.FlatStyle = FlatStyle.Flat;

                    classComboBox!.SelectedIndexChanged -= ClassComboBox_SelectedIndexChanged;
                    classComboBox.SelectedIndexChanged += ClassComboBox_SelectedIndexChanged;

                    classComboBox.BackColor = SystemColors.Window;
                    classComboBox.ForeColor = SystemColors.WindowText;
                }
            }
            else if (GridViewLabTestResultLabtestElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
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
            if (GridViewLabTestResultLabtestElementInfo.CurrentRow != null && GridViewLabTestResultLabtestElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.SUBCLASS)
            {
                var subClassComboBox = sender as ComboBox;
                var selectedSubClass = subClassComboBox?.SelectedItem?.ToString();

                if (selectedSubClass != null)
                {
                    var row = GridViewLabTestResultLabtestElementInfo.CurrentRow;
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
            if (GridViewLabTestResultLabtestElementInfo.CurrentRow != null && GridViewLabTestResultLabtestElementInfo.CurrentCell.ColumnIndex == (int)MedicalLabTestElementsGridColumn.CLASS)
            {
                var ClassComboBox = sender as ComboBox;
                var selectedClass = ClassComboBox?.SelectedItem?.ToString()!;
                var classvalue = GridViewLabTestResultLabtestElementInfo.CurrentCell.EditedFormattedValue.ToString();
                var row = GridViewLabTestResultLabtestElementInfo.CurrentRow;
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

        private void GridViewLabTestResultLabtestElementInfo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.ForeColor = Color.Black;
                e.CellStyle.SelectionBackColor = Color.White;
                e.CellStyle.SelectionForeColor = Color.Black;
            }
        }

        private void GridViewLabTestResultLabtestElementInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}
