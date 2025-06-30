using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using fa.views.hms.patient;
using fa.model.Hms.Master;
using fa.api.utils;
using fa.model.Common;
using fa.libraries.Validation;
using fa.api.Hms;
using fa.views.controls;
using fa.api.Accounting;
using fa.libraries.utils;
using fa.model.Hms.Op;
using fa.views.hms.ip;
using fa.views.hms.op;
using System.IO;
using fa.model.Hms.Ip;
using fa.model.hms.common;
using fa.views.catalog;
using fa.model.Catalog;
using VisioForge.MediaFramework.ONVIF;
using DateTime = System.DateTime;
using VisioForge.Libs.MediaFoundation.OPM;
using fa.api.catalog;
using DocumentFormat.OpenXml.Drawing;
using Path = System.IO.Path;
using Color = System.Drawing.Color;
using System.Text.RegularExpressions;
using fa.model.Accounting.Masters;
using Fa.views.hms.patient;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace fa.views.hms
{
    public partial class PatientRegistration : FormPatientBase
    {
        public static string EnterFirstNameErrorMsg = "Please enter first name";
        public static string EnterLastNameErrorMsg = "Please enter last name";
        public static string SelectGenderErrorMsg = "Please select gender";
        public static string EnterDOBErrorMsg = "Please enter date of birth";
        public static string EnterValidDOBErrorMsg = "Please enter valid date";
        public static string SelectRelationshipErrorMsg = "Please select relationship";
        public static string SelectDocumentErrorMsg = "Plesae select the documents category.";
        public static string SaveSuccessMsg = "Saved";
        public static string DoNotAllowToDeleteMsg = "Can't delete the patient, the patient is in use";
        public static string DoNotAllowToDeleteGuardianMsg = "Error in Delete : Do not delete the {0} {1} this person as responsible party";
        public static string DoNotAllowToDeleteEmergencyContactMsg = "Error in Delete";
        public static string DoNotAllowToDeleteInsuranceMsg = "Error in Delete";
        public static string PatientAsInPatientMsg = "This patient is in IP @ {0}, {1}";
        public static string PatientAsOutPatientMsg = "This patient is in OP @ {0}";
        public static string SaveConfirmText = "Do you want to save the current changes?";
        public static string PatientDoNotHaveOpErrorMsg = "The patient {0} do not have any OP registration";
        public static string DoNotAllowToSave_PatientInIpMsg = "{0} has been admitted as in-patient, could not create new IP.";
        public static string UploadStatusFileSizeError = "File size large cant Upload it";
        public static string UploadStatusDocument = "No Document Found";
        public static string SelectStatusDocument = "No document selected";
        public static string ChooseStateErrorMsg = "Please choose state";
        public static string StatusDocumentFormat = "Document format not supported for preview";
        public static string CameraErrorMsg = "Failed to open camera occupied by another device or faulty device.";
        public static string InvalidIDErrorMsg = "Invalid document ID";
        public static string DisplayingErrorMsg = "Error displaying {0}";

        public bool CreatePatientOnLoad = false;
        private bool shouldExecuteEditingControlShowing = false;
        public long PatientId = 0L;
        public LabTestAttachment? LabTestAttachment;

        GuardianManager GuardianManager = null!;
        EmergencyContactManager EmergancyContactManager = null!;
        InsuranceInfoManager InsuranceInfoManager = null!;
        PatientMedicalHistoryManager PatientMedicalHistoryManager = null!;
        OpManager OpManager = null!;
        IpManager IpManager = null!;
        PatientManager PatientManager = null!;
        AddressManager AddressManager = null!;
        ContactInfoManager ContactInfoManager = null!;
        DateValidation DateValidation = null!;
        KeypressValidation KeypressValidation = null!;
        FormPatientBase parent = null!;
        CountryManager countryManager = null!;
        public PatientRegistration(object sender)
        {
            if (sender is FormPatientBase)
            {
                parent = (FormPatientBase)sender;
            }
            else
                parent = null!;
            InitializeComponent();
            GuardianManager = GuardianManager.Instance;
            EmergancyContactManager = EmergencyContactManager.Instance;
            InsuranceInfoManager = InsuranceInfoManager.Instance;
            OpManager = OpManager.Instance;
            IpManager = IpManager.Instance;
            PatientManager = PatientManager.Instance;
            DateValidation = DateValidation.Instance;
            AddressManager = AddressManager.Instance;
            ContactInfoManager = ContactInfoManager.Instance;
            KeypressValidation = KeypressValidation.Instance;
            PatientMedicalHistoryManager = PatientMedicalHistoryManager.Instance;
            excludedObjects = new string[] { "PatientVisitControlIpOP", "GridViewOutPatient", "GridViewInPatient", "GridViewInsuranceInfo", "GridViewGuardianInfo", "GridViewEmergencyContact" };
            countryManager = CountryManager.Instance;
            Country country = new Country();
            this.TabControlPatient.SelectedIndexChanged += new System.EventHandler(this.PatientDetails_SelectedIndexChanged!);

            //PatientPhotoControl.CameraCaptureFailed += (sender, errorMessage) =>
            //{
            //    PatientErrorMsg.Text = CameraErrorMsg;
            //};
        }
        private void PatientRegistration_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ResetForm();
                GroupBoxPatientAddress.CountryId = (long)Global.Company.CountryId!;
                GroupBoxPatientAddress.StateId = (long)Global.Company.Address.StatesId!;
                TextBoxPatientId.ResetText();
                EnableForm(true);
                if (CreatePatientOnLoad)
                {
                    ResetDirtyFlag();
                    if (PatientId != 0L)
                    {
                        TextBoxPatientId.Text = PatientId.ToString();
                        LoadPatientInfo();
                        EnableForm(false);
                        BtnNew.Visible = false;
                        BtnDelete.Visible = false;
                        BtnPrint.Visible = false;
                        BtnPatientPrintSticker.Visible = false;
                        BtnOpVisit.Visible = false;
                        BtnOPSticker.Visible = false;
                        BtnIpVisit.Visible = false;
                        BtnIPSticker.Visible = false;
                        buttonImport.Visible = true;
                        buttonImport.Location = new System.Drawing.Point(862, 590);
                    }
                    else
                    {
                        BtnNew_Click(sender, e);
                    }
                    BtnSearchPatient.Visible = false;
                }
                LoadMedicalHistory();
                LoadMedicalHistoryData();
                TabControlPatient.SelectedTab = PatientInfoTab;
                TextBoxPatientFirstName.Select();
                ResetDirtyFlag();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void LoadPatientInfo()
        {
            Patient PatientFromDB = PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text));
            if (PatientFromDB != null)
            {
                if (DateUtils.ValidDate(PatientFromDB.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                {
                    TextBoxPatientAge.Text = PatientFromDB.Age.ToString();
                    DateTimePickerPatientDob.Date = (DateTime)DateUtils.ToDate(PatientFromDB.DateOfBirth.Date.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
                }
                TextBoxPatientFirstName.Text = PatientFromDB.FirstName;
                TextBoxPatientId.Text = PatientFromDB.Id.ToString();
                TextBoxPatientIncome.Text = double.Parse(PatientFromDB.Income).ToString(TextUtils.DecimalPlace(TextBoxPatientIncome.Decimals));
                TextBoxPatientInitial.Text = PatientFromDB.MiddleInitial;
                TextBoxPatientLastName.Text = PatientFromDB.LastName;
                TextBoxPatientOccupation.Text = PatientFromDB.Occupation;
                TextBoxPatientBloodGroup.Text = PatientFromDB.Bloodgroup;
                TextBoxPatientTaxId.Text = PatientFromDB.TaxId;
                RbtPatientGender.Gender = (GenderSelection)PatientFromDB.Gender + 1;
                if (PatientFromDB.Photo != null)
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream(PatientFromDB.Photo))
                        {
                            PatientPhotoControl.Photo = System.Drawing.Image.FromStream(stream);
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine("Invalid image format: " + ex.Message);
                    }
                }
                GroupBoxPatientAddress.CountryId = (long)Global.Company.CountryId!;
                if (PatientFromDB.Address != null)
                {
                    Address Address = PatientFromDB.Address;
                    GroupBoxPatientAddress.AddressLine1 = Address.AddressLine1;
                    GroupBoxPatientAddress.AddressLine2 = Address.AddressLine2;
                    GroupBoxPatientAddress.CityName = Address.CityOrTown;
                    GroupBoxPatientAddress.DistrictName = Address.District;
                    GroupBoxPatientAddress.PinCode = Address.PinCode;
                    GroupBoxPatientAddress.StateId = Address.StatesId != null ? (long)Address.StatesId : 0L;
                }
                if (PatientFromDB.ContactInfo != null)
                {
                    ContactInfo lContactInfo = PatientFromDB.ContactInfo;
                    TextBoxPatientPhone.Text = lContactInfo.Phone;
                    TextBoxPatientMobile.Text = lContactInfo.Mobile;
                }
                LoadPatientDocumentCategory();
                LoadInsuranceDocument();
                LoadGuardians();
                LoadEmergencyCont();
                LoadInsuranceInfo();
                LoadMedicalHistory();
                LoadMedicalHistoryData();
                LoadOutPatient();
                LoadInPatient();
                ResetDirtyFlag();
                TextBoxOtherNotes.Text = PatientFromDB.OtherNotes;
                PatientNumberPatient.PatientNumber = PatientFromDB.PatientNumber;
                PatientVisitControlIpOP.PatientId = PatientFromDB.Id.ToString();
                LinkLabelPatientIdEdit.Enabled = true;
                GridViewPatientHistory.Enabled = true;
                this.formIsDirty = false;
            }
        }
        private void LoadGuardians()
        {
            GridViewGuardianInfo.Rows.Clear();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                IList<Guardian> Guardian = GuardianManager.ListAllGuardianByPatientId(long.Parse(TextBoxPatientId.Text));
                if (Guardian.Count > 0)
                {
                    GridViewGuardianInfo.Rows.Add(Guardian.Count);
                    int i = 0;
                    foreach (var lGuardian in Guardian)
                    {
                        GridViewGuardianInfo.Rows[i].Cells[0].Value = lGuardian?.Name ?? string.Empty;

                        if (lGuardian?.DateOfBirth != null && DateUtils.ValidDate_TillCurrentDate(lGuardian.DateOfBirth.ToString(Global.Company.DateFormat), Global.Company.DateFormat))
                        {
                            GridViewGuardianInfo.Rows[i].Cells[1].Value = lGuardian.DateOfBirth.ToShortDateString();
                        }
                        else
                        {
                            GridViewGuardianInfo.Rows[i].Cells[1].Value = DateTime.Now.Date;
                        }
                        GridViewGuardianInfo.Rows[i].Cells[2].Value = lGuardian?.Address?.FullAddress ?? string.Empty;
                        GridViewGuardianInfo.Rows[i].Cells[3].Value = lGuardian?.ContactInfo?.Phone?.Trim('-') ?? string.Empty;
                        GridViewGuardianInfo.Rows[i].Cells[4].Value = lGuardian?.RelationShip.ToString();
                        GridViewGuardianInfo.Rows[i].Cells[5].Value = ImageListPatient?.Images.Count > 0 ? ImageListPatient.Images[0] : null;
                        GridViewGuardianInfo.Rows[i].Cells[6].Value = "X";
                        GridViewGuardianInfo.Rows[i].Cells[7].Value = lGuardian?.Id;
                        i++;
                    }
                    loadResposiblePart();
                }
            }
        }
        private void loadResposiblePart()
        {
            ComboUtils.InitializeResponsiblePartyCombo(ComboBoxSwapTextBoxPatientResponsibleParty, long.Parse(TextBoxPatientId.Text));
            Patient PatientFromDB = PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text));
            if (PatientFromDB.ResponsibleParty != null) { ComboBoxSwapTextBoxPatientResponsibleParty.SelectedIndex = ComboBoxSwapTextBoxPatientResponsibleParty.FindStringExact(PatientFromDB.ResponsibleParty.Name); }
        }
        private void LoadEmergencyCont()
        {
            GridViewEmergencyContact.Rows.Clear();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                IList<EmergencyContact> EmergencyContact = EmergancyContactManager.ListAllEmergencyContactByPatientId(long.Parse(TextBoxPatientId.Text));
                if (EmergencyContact.Count > 0)
                {
                    GridViewEmergencyContact.Rows.Add(EmergencyContact.Count);
                    int i = 0;
                    foreach (var lEmergencyContact in EmergencyContact)
                    {
                        GridViewEmergencyContact.Rows[i].Cells[0].Value = lEmergencyContact?.Name ?? string.Empty;
                        GridViewEmergencyContact.Rows[i].Cells[1].Value = lEmergencyContact?.RelationShip.ToString();
                        GridViewEmergencyContact.Rows[i].Cells[2].Value = lEmergencyContact?.ContactInfo?.Phone?.Trim('-') ?? string.Empty;
                        GridViewEmergencyContact.Rows[i].Cells[3].Value = (ImageListPatient.Images.Count > 0) ? ImageListPatient.Images[0] : null;
                        GridViewEmergencyContact.Rows[i].Cells[4].Value = "X";
                        GridViewEmergencyContact.Rows[i].Cells[5].Value = lEmergencyContact?.Id ?? 0;
                        i++;
                    }
                }
            }
        }
        private void LoadInsuranceInfo()
        {
            GridViewInsuranceInfo.Rows.Clear();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                IList<InsuranceInfo> InsuranceInfo = InsuranceInfoManager.ListAllInsuranceInfoByPatientId(long.Parse(TextBoxPatientId.Text), false, "ALL");
                if (InsuranceInfo.Count > 0)
                {
                    GridViewInsuranceInfo.Rows.Add(InsuranceInfo.Count);
                    int i = 0;
                    foreach (var lInsuranceInfo in InsuranceInfo)
                    {
                        var insuranceHolder = PersonManager.Instance.GetPersonById(lInsuranceInfo?.InsuranceHolderId ?? 0);
                        GridViewInsuranceInfo.Rows[i].Cells[0].Value = insuranceHolder?.Name ?? string.Empty;
                        GridViewInsuranceInfo.Rows[i].Cells[1].Value = lInsuranceInfo?.InsuranceHolderRelationShip.ToString();
                        GridViewInsuranceInfo.Rows[i].Cells[2].Value = lInsuranceInfo?.InsuranceName ?? string.Empty;
                        GridViewInsuranceInfo.Rows[i].Cells[3].Value = lInsuranceInfo?.EmployerContactInfo?.Phone?.Trim('-') ?? string.Empty;
                        GridViewInsuranceInfo.Rows[i].Cells[4].Value = lInsuranceInfo?.GroupNumber ?? string.Empty;
                        GridViewInsuranceInfo.Rows[i].Cells[5].Value = lInsuranceInfo?.PolicyNumber ?? string.Empty;
                        GridViewInsuranceInfo.Rows[i].Cells[6].Value = lInsuranceInfo?.IsPrimary ?? false;
                        GridViewInsuranceInfo.Rows[i].Cells[7].Value = (lInsuranceInfo?.IsActive == true ? "Active" : "Inactive");
                        GridViewInsuranceInfo.Rows[i].Cells[8].Value = (lInsuranceInfo?.IPInsuranceCoverage == true && lInsuranceInfo.OPInsuranceCoverage == true ? "IP & OP" : lInsuranceInfo!.IPInsuranceCoverage == false && lInsuranceInfo.OPInsuranceCoverage == false ? "NA" : lInsuranceInfo.IPInsuranceCoverage == true ? "IP" : lInsuranceInfo.OPInsuranceCoverage == true ? "OP" : "NA");
                        GridViewInsuranceInfo.Rows[i].Cells[9].Value = (ImageListPatient.Images.Count > 0) ? ImageListPatient.Images[0] : null;
                        GridViewInsuranceInfo.Rows[i].Cells[10].Value = "X";
                        GridViewInsuranceInfo.Rows[i].Cells[11].Value = lInsuranceInfo?.Id ?? 0;
                        i++;
                    }
                }
            }
        }
        private void LoadInsuranceDocument()
        {
            TreeViewInsurance.Nodes.Clear();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                IList<PatientDocument> lPatientDocument = DocumentManager.Instance.ListPatientDocumentByCategoryId(4, long.Parse(TextBoxPatientId.Text));
                if (lPatientDocument != null)
                {
                    foreach (var PatientDocument in lPatientDocument)
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = PatientDocument.FileName + GetExtension(PatientDocument.FileType);
                        treeNode.Name = PatientDocument.Id.ToString();
                        treeNode.ContextMenuStrip = contextDeleteDocument;
                        TreeViewInsurance.Nodes.Add(treeNode);
                    }
                }
            }
        }
        private void LoadMedicalHistory()
        {
            bool flag = false;
            GridViewPatientHistory.Rows.Clear();
            int i = 0;
            IList<PatientHistoryQuestionGroup> PatientHistoryQuestionGroupInfo = PatientMedicalHistoryManager.ListAllPatientHistoryQuestionGroup();
            if (PatientHistoryQuestionGroupInfo.Count > 0)
            {
                foreach (PatientHistoryQuestionGroup lPatientHistoryQuestionGroup in PatientHistoryQuestionGroupInfo.OrderBy(x => x.Name))
                {
                    IList<PatientHistoryQuestion> PatientHistoryQuestionInfo = PatientMedicalHistoryManager.ListAllPatientHistoryQuestionsByQuestionGroupId(lPatientHistoryQuestionGroup.Id);
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
                            if (lPatientHistoryQuestion.AdditionalNotes)
                            {
                                i++;
                                if (GridViewPatientHistory.Rows.Count == i)
                                {
                                    GridViewPatientHistory.Rows.Add(1);
                                }
                                GridViewPatientHistory.Rows[i].Cells[j - 1] = new DataGridViewTextBoxCell();
                                GridViewPatientHistory.Rows[i].Cells[j - 1].Value = lPatientHistoryQuestion.AdditionalNotesCaption;
                                GridViewPatientHistory.Rows[i].Cells[j - 1].ReadOnly = true;
                                DataGridViewAdvancedBorderStyle newStyle = new DataGridViewAdvancedBorderStyle();
                                newStyle.Right = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                                newStyle.Left = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                                newStyle.Bottom = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                                newStyle.Top = DataGridViewAdvancedCellBorderStyle.InsetDouble;

                                DataGridViewAdvancedBorderStyle newStylePlaceholder = new DataGridViewAdvancedBorderStyle();
                                newStylePlaceholder.Right = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                                newStylePlaceholder.Left = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                                newStylePlaceholder.Bottom = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                                newStylePlaceholder.Top = DataGridViewAdvancedCellBorderStyle.InsetDouble;
                                GridViewPatientHistory.Rows[i].Cells[j].AdjustCellBorderStyle(newStyle, newStylePlaceholder, true, true, true, true);
                                GridViewPatientHistory.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                                GridViewPatientHistory.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                                GridViewPatientHistory.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightGray;
                                GridViewPatientHistory.Rows[i].Cells[j].Style.SelectionForeColor = Color.Black;
                                i--;
                            }
                            j = j + 2;
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
            if (GridViewPatientHistory.Rows.Count > 0)
            {
                GridViewPatientHistory.EndEdit();
                for (int k = 0; k < GridViewPatientHistory.Rows.Count; k++)
                {
                    for (int l = 2; l < 9; l = l + 2)
                    {
                        if (GridViewPatientHistory.Rows[k].Cells[l].Value == null && GridViewPatientHistory.Rows[k].Cells[l - 1].GetType() == typeof(DataGridViewCheckBoxCell))
                        {
                            GridViewPatientHistory.Rows[k].Cells[l - 1] = new DataGridViewTextBoxCell();
                            GridViewPatientHistory.Rows[k].Cells[l - 1].ReadOnly = true;
                        }
                    }
                    if (GridViewPatientHistory.Rows[k].Cells[0].Value != null)
                    {

                    }
                }
            }
        }
        private void LoadMedicalHistoryData()
        {
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                IList<PatientPreMedicalHistory> PatientPreMedicalHistory = PatientMedicalHistoryManager.ListAllPatientPreMedicalHistoryPatientId(long.Parse(TextBoxPatientId.Text));
                if (PatientPreMedicalHistory.Count > 0)
                {
                    foreach (DataGridViewRow rows in GridViewPatientHistory.Rows)
                    {
                        foreach (DataGridViewCell Cell in rows.Cells)
                        {
                            if (Cell.GetType() == typeof(DataGridViewCheckBoxCell))
                            {
                                Cell.ReadOnly = true;
                                PatientPreMedicalHistory History = PatientPreMedicalHistory.FirstOrDefault(x => x.HistoryItemId == ((PatientHistoryQuestion)GridViewPatientHistory.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex + 1].Value).Id)!;
                                if (History != null)
                                {
                                    GridViewPatientHistory.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value = true;
                                    if (((PatientHistoryQuestion)GridViewPatientHistory.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex + 1].Value).AdditionalNotes)
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
                            else
                            {
                                if (!(Cell.RowIndex > 0 &&
                                    Cell.ColumnIndex > 0 &&
                                    GridViewPatientHistory.Rows[Cell.RowIndex - 1].Cells[Cell.ColumnIndex - 1].GetType() == typeof(DataGridViewCheckBoxCell) &&
                                    (bool)GridViewPatientHistory.Rows[Cell.RowIndex - 1].Cells[Cell.ColumnIndex - 1].Value == true))
                                {
                                    Cell.ReadOnly = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void LoadPatientDocumentCategory()
        {
            TreeViewPatientDocument.Nodes.Clear();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                LoadPatientDocument();
            }
            else
            {
                IList<DocumentCategory> lPatientDocumentCategory = DocumentManager.Instance.ListPatientDocumentCategoryByCompanyId(Global.Company.CompanyId);
                if (lPatientDocumentCategory != null)
                {
                    foreach (var PatientDocumentCategory in lPatientDocumentCategory)
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = PatientDocumentCategory.Name;
                        treeNode.Name = PatientDocumentCategory.Id.ToString();
                        treeNode.ContextMenuStrip = contextUploadDocument;
                        TreeViewPatientDocument.Nodes.Add(treeNode);
                    }
                }
            }
        }
        private void LoadPatientDocument()
        {
            Cursor.Current = Cursors.WaitCursor;
            string FilterString = "";
            TreeViewPatientDocument.Nodes.Clear();
            IList<DocumentCategory> lPatientDocumentCategory = DocumentManager.Instance.ListPatientDocumentCategoryByCompanyId(Global.Company.CompanyId);
            if (lPatientDocumentCategory != null && lPatientDocumentCategory.Count > 0)
            {
                foreach (var DocumentCategory in lPatientDocumentCategory)
                {
                    TreeNode treeNode = new TreeNode();
                    treeNode.Text = DocumentCategory.ToString();
                    treeNode.Name = DocumentCategory.Id.ToString();
                    treeNode.ImageIndex = 0;
                    treeNode.ContextMenuStrip = contextUploadDocument;
                    TreeViewPatientDocument.Nodes.Add(treeNode);
                    IList<PatientDocument> lPatientDocument = DocumentManager.Instance.ListPatientDocumentByCategoryId(DocumentCategory.Id, long.Parse(TextBoxPatientId.Text));
                    if (lPatientDocument != null)
                    {
                        foreach (var PatientDocument in lPatientDocument)
                        {
                            TreeNode ChildNode = new TreeNode();
                            ChildNode.Text = PatientDocument.FileName + GetExtension(PatientDocument.FileType);
                            ChildNode.Name = (PatientDocument.Id.ToString() + "@");
                            ChildNode.ContextMenuStrip = contextDeleteDocument;
                            treeNode.Nodes.Add(ChildNode);
                        }
                    }
                }
            }
            else
            {
                if (TreeViewPatientDocument.Nodes.Count > 0)
                {
                    PatientErrorMsg.Text = "";
                }
                else
                {
                    PatientErrorMsg.Text = string.Format(UploadStatusDocument, FilterString);
                }
            }
            if (TreeViewPatientDocument.Nodes.Count > 0)
            {
                TreeViewPatientDocument.SelectedNode = TreeViewPatientDocument.Nodes[0];
            }
            Cursor.Current = Cursors.Default;
        }
        private void GridViewPatientHistory_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            GridViewPatientHistory.Rows[e.RowIndex].Cells[1].Value = false;
            GridViewPatientHistory.Rows[e.RowIndex].Cells[3].Value = false;
            GridViewPatientHistory.Rows[e.RowIndex].Cells[5].Value = false;
            GridViewPatientHistory.Rows[e.RowIndex].Cells[7].Value = false;
        }
        private void GridViewPatientHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && GridViewPatientHistory.Enabled)
            {
                if (GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    if (e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 5 || e.ColumnIndex == 7)
                    {
                        GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !(bool)GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                        GridViewPatientHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        PatientHistoryQuestion PatientHistoryQuestion = PatientMedicalHistoryManager.GetPatientHistoryQuestionById(((PatientHistoryQuestion)GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value).Id);
                        if (PatientHistoryQuestion.AdditionalNotes)
                        {
                            if ((bool)GridViewPatientHistory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
                            {
                                GridViewPatientHistory.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].ReadOnly = false;
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory[e.ColumnIndex + 1, e.RowIndex + 1];
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
                    }
                }
            }
        }

        private void LoadOutPatient()
        {
            GridViewOutPatient.Clear();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                GridViewOutPatient.PatientId = long.Parse(TextBoxPatientId.Text);
                ResetIpOp();
            }
        }
        private void LoadInPatient()
        {
            GridViewInPatient.Clear();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                GridViewInPatient.PatientId = long.Parse(TextBoxPatientId.Text);
                ResetIpOp();
            }
        }
        private Patient GetPatientFromForm()
        {
            Patient lPatient = new Patient();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                lPatient.Id = Convert.ToInt64(TextBoxPatientId.Text);
            }
            else
            {
                lPatient.Id = 0L;
            }
            lPatient.FirstName = TextBoxPatientFirstName.Text;
            lPatient.MiddleInitial = TextBoxPatientInitial.Text;
            lPatient.LastName = TextBoxPatientLastName.Text;
            lPatient.Gender = (Gender)RbtPatientGender.Gender - 1;
            if (DateUtils.ValidDate_TillCurrentDate(((DateTime)DateTimePickerPatientDob.Date!).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                lPatient.DateOfBirth = ((DateTime)DateUtils.ToDate(((DateTime)DateTimePickerPatientDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)!).Date;
            }
            lPatient.TaxId = TextBoxPatientTaxId.Text;
            lPatient.Occupation = TextBoxPatientOccupation.Text;
            lPatient.Bloodgroup = TextBoxPatientBloodGroup.Text;
            lPatient.Income = TextBoxPatientIncome.Text;
            lPatient.CompanyId = Global.Company.CompanyId;
            if (ComboBoxSwapTextBoxPatientResponsibleParty.SelectedIndex > -1)
            {
                lPatient.ResponsiblePartyId = ((Guardian)ComboBoxSwapTextBoxPatientResponsibleParty.Items[ComboBoxSwapTextBoxPatientResponsibleParty.SelectedIndex]).Id;
            }
            if (PatientPhotoControl.Photo != null)
            {
                try
                {
                    using (Bitmap bmp = new Bitmap(PatientPhotoControl.Photo))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            bmp.Save(stream, ImageFormat.Jpeg);
                            lPatient.Photo = stream.ToArray();
                        }
                    }
                }
                catch (ExternalException ex)
                {
                    Console.WriteLine("Invalid image format: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Invalid image format: " + ex.Message);
                }
            }
            lPatient.OtherNotes = TextBoxOtherNotes.Text.ToString().Trim();
            lPatient.Address = GetPatientAddressFromForm();
            lPatient.ContactInfo = GetPatientContactInfoFromForm();
            return lPatient;
        }
        private Address GetPatientAddressFromForm()
        {
            Address lAddress = new Address();
            lAddress.AddressLine1 = GroupBoxPatientAddress.AddressLine1.Trim();
            lAddress.AddressLine2 = GroupBoxPatientAddress.AddressLine2.Trim();
            lAddress.CityOrTown = GroupBoxPatientAddress.CityName.Trim();
            lAddress.District = GroupBoxPatientAddress.DistrictName.Trim();
            lAddress.PinCode = GroupBoxPatientAddress.PinCode.Trim();
            lAddress.StatesId = GroupBoxPatientAddress?.StateId != null ? GroupBoxPatientAddress.StateId : 0L;
            return lAddress;
        }
        private ContactInfo GetPatientContactInfoFromForm()
        {
            ContactInfo lContactInfo = new ContactInfo();
            lContactInfo.Phone = TextBoxPatientPhone.Text.Replace("-", "");
            lContactInfo.Mobile = TextBoxPatientMobile.Text.Replace("-", "");
            return lContactInfo;
        }
        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (formIsDirty)
            {
                DialogResult Result = MessageBox.Show(SaveConfirmText, "Confirm",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);
                if (Result == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnSave_Click(sender, e);
                    }
                }
                if (Result == DialogResult.Cancel)
                {
                    TextBoxPatientFirstName.Select();
                    return;
                }
            }
            ResetForm();
            TextBoxPatientId.ResetText();
            EnableForm(true);
            GroupBoxPatientAddress.CountryId = (long)Global.Company.CountryId!;
            GroupBoxPatientAddress.StateId = (long)Global.Company.Address.StatesId!;
            TabControlPatient.SelectedTab = PatientInfoTab;
            TextBoxPatientFirstName.Select();
            ResetDirtyFlag();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (validate())
            {
                Patient lPatient = GetPatientFromForm();
                Patient PatientFromDB = null!;
                if (lPatient.Id == 0)
                {
                    //lPatient.PatientNumber = PatientIdManager.FeachPatientID(Global.Company.CompanyId, Global.getTransactionDate().Date);
                    lPatient.PatientNumber = CompanyManager.Instance.GetIdSpace(Global.Company, EntryType.PATIENT_ID, Global.getTransactionDate().Date);
                    PatientFromDB = PatientManager.AddPatient(lPatient);
                    //PatientIdManager.GenerateNextPatientID(Global.Company.CompanyId, Global.getTransactionDate().Date);
                    if (CreatePatientOnLoad)
                    {
                        if (parent != null)
                        {
                            parent.PatientIdTransport.Text = PatientFromDB.Id.ToString();
                        }
                        this.formIsDirty = false;
                        this.Hide();
                        this.Close();
                    }
                    TextBoxPatientId.Text = PatientFromDB.Id.ToString();
                }
                else
                {
                    lPatient.Address = GetPatientAddressFromForm();
                    lPatient.ContactInfo = GetPatientContactInfoFromForm();
                    Patient PatientById = PatientManager.GetPatientById(lPatient.Id);
                    if (PatientById != null)
                    {
                        lPatient.PatientNumber = PatientById.PatientNumber;
                        lPatient.ContactInfoId = lPatient.ContactInfo.Id = (long)PatientById.ContactInfoId!;
                        lPatient.AddressId = lPatient.Address.AddressId = (long)PatientById.AddressId!;
                        AddressManager.UpdateAddress(lPatient.Address);
                        ContactInfoManager.UpdateContactInfo(lPatient.ContactInfo);
                        PatientFromDB = PatientManager.UpdatePatient(lPatient);
                        TextBoxPatientId.Text = PatientFromDB.Id.ToString();
                    }
                    if (GridViewPatientHistory.Rows.Count > 0)
                    {
                        Patient Patient = new Patient();
                        Patient.Id = PatientFromDB.Id;
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
                                        lPatientPreMedicalHistory.PatientId = PatientFromDB.Id;
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
                        PatientMedicalHistoryManager.AddPatientPreMedicalHistory(Patient);
                    }
                }
                ResetForm();
                TextBoxPatientId.Text = PatientFromDB.Id.ToString();
                LoadPatientInfo();
                EnableForm(false);
                PatientErrorMsg.Text = SaveSuccessMsg;
                TabControlPatient.SelectedTab = PatientInfoTab;
                ResetDirtyFlag();
                PatientNumberPatient.PatientNumber = PatientFromDB.PatientNumber;
                PatientVisitControlIpOP.PatientId = PatientFromDB.Id.ToString();
                if (CreatePatientOnLoad && PatientId != 0L)
                {
                    this.Close();
                }
                TextBoxPatientFirstName.Select();
                Cursor.Current = Cursors.Default;
            }
        }
        private Boolean validate()
        {
            PatientErrorMsg.Text = "";
            if (string.IsNullOrEmpty(TextBoxPatientFirstName.Text.Trim()))
            {
                PatientErrorMsg.Text = EnterFirstNameErrorMsg;
                TabControlPatient.SelectedTab = PatientInfoTab;
                TextBoxPatientFirstName.Select();
                return false;
            }
            if (string.IsNullOrEmpty(TextBoxPatientLastName.Text.Trim()))
            {
                PatientErrorMsg.Text = EnterLastNameErrorMsg;
                TabControlPatient.SelectedTab = PatientInfoTab;
                TextBoxPatientLastName.Select();
                return false;
            }
            if (RbtPatientGender.Gender == GenderSelection.None)
            {
                PatientErrorMsg.Text = SelectGenderErrorMsg;
                TabControlPatient.SelectedTab = PatientInfoTab;
                RbtPatientGender.Select();
                RbtPatientGender.Gender = GenderSelection.None;
                return false;
            }
            if (DateTimePickerPatientDob.Date == null || !DateUtils.ValidDate(((DateTime)DateTimePickerPatientDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                PatientErrorMsg.Text = EnterValidDOBErrorMsg;
                DateTimePickerPatientDob.Focus();
                return false;
            }
            if (!DateUtils.ValidDate_TillCurrentDate(((DateTime)DateTimePickerPatientDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                PatientErrorMsg.Text = EnterDOBErrorMsg;
                DateTimePickerPatientDob.Focus();
                return false;
            }
            if (GroupBoxPatientAddress.StateId == 0L)
            {
                PatientErrorMsg.Text = ChooseStateErrorMsg;
                GroupBoxPatientAddress.selected_field(Fields.state);
                return false;
            }

            return true;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (this.formIsDirty)
            {
                if (MessageBox.Show("Do you want to save the changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnSave.PerformClick();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            if (CreatePatientOnLoad)
            {
                this.formIsDirty = false;
                this.Close();
            }
            ResetForm();
            TextBoxPatientId.ResetText();
            EnableForm(true);
            TabControlPatient.SelectedTab = PatientInfoTab;
            ResetDirtyFlag();
            TextBoxPatientFirstName.Select();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Do you want to delete Patient " + TextBoxPatientFirstName.Text + " " + TextBoxPatientInitial.Text + " " + TextBoxPatientLastName.Text + "?", "Delete Confirm",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (Result == DialogResult.Yes)
            {
                if (PatientManager.DeletePatient(long.Parse(TextBoxPatientId.Text)))
                {
                    ResetForm();
                    EnableForm(true);
                    ResetDirtyFlag();
                    TabControlPatient.SelectedTab = PatientInfoTab;
                    TextBoxPatientId.ResetText();
                    TextBoxPatientFirstName.Select();
                }
                else
                {
                    PatientErrorMsg.Text = DoNotAllowToDeleteMsg;
                }
            }
        }
        private void BtnAddGuardian_Click(object sender, EventArgs e)
        {
            FormParentOrGuardian FormAddGuardian = new FormParentOrGuardian(this);
            FormAddGuardian.GuardianId = null;
            FormAddGuardian.TextBoxPatientId.Text = TextBoxPatientId.Text;
            FormAddGuardian.ShowDialog();
            LoadGuardians();
        }
        private void BtnAddInsurance_Click(object sender, EventArgs e)
        {
            FormInsurance FormInsurance = new FormInsurance(this);
            FormInsurance.InsuranceId = null;
            FormInsurance.TextBoxPatientId.Text = TextBoxPatientId.Text;
            FormInsurance.ShowDialog();
            LoadInsuranceInfo();
        }
        private void BtnAddEmergencyContact_Click(object sender, EventArgs e)
        {
            FormEmergencyContact FormEmergencyContact = new FormEmergencyContact(this);
            FormEmergencyContact.EmergencyContId = null;
            FormEmergencyContact.TextBoxPatientId.Text = TextBoxPatientId.Text;
            FormEmergencyContact.ShowDialog();
            LoadEmergencyCont();
        }
        private void BtnSearchPatient_Click(object sender, EventArgs e)
        {
            FormPatientSearch FormPatientSearch = new FormPatientSearch(this);
            FormPatientSearch.ShowDialog();
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                ResetForm();
                LoadPatientInfo();
                EnableForm(false);
            }
            else
            {
                ResetDirtyFlag();
            }
            TabControlPatient.SelectedTab = PatientInfoTab;
            TextBoxPatientFirstName.Select();
        }
        private void BtnIpVisit_Click(object sender, EventArgs e)
        {
            Registration RegistrationFromDB = OpManager.Instance.GetOPRecordIfAdmitted(long.Parse(TextBoxPatientId.Text));
            if (RegistrationFromDB == null)
            {
                Registration lRegistrationFromDB = OpManager.Instance.GetPatientInOpQueue(long.Parse(TextBoxPatientId.Text), Global.getTransactionDate());
                if (lRegistrationFromDB != null)
                {
                    this.PatientIdTransport.Text = TextBoxPatientId.Text;
                    FormIPRegistration FormIPRegistration = new FormIPRegistration(this);
                    FormIPRegistration.OpId = lRegistrationFromDB.Id;
                    FormIPRegistration.ShowDialog();
                    PatientVisitControlIpOP.PatientId = TextBoxPatientId.Text;
                    LoadInPatient();
                }
                else
                {
                    MessageBox.Show(string.Format(PatientDoNotHaveOpErrorMsg, PatientManager.GetPatientById(long.Parse(TextBoxPatientId.Text)).Name), "Warning");
                }
            }
            else
            {
                MessageBox.Show(string.Format(DoNotAllowToSave_PatientInIpMsg, RegistrationFromDB.Patient.Name), "Warning");
            }
        }
        private void BtnOpVisit_Click(object sender, EventArgs e)
        {
            this.PatientIdTransport.Text = TextBoxPatientId.Text;
            FormOPRegistration FormOPRegistration = new FormOPRegistration(this);
            FormOPRegistration.CreateOpRegisterOnLoad = true;
            FormOPRegistration.PatientIdTransport.Text = PatientIdTransport.Text;
            FormOPRegistration.ShowDialog();
            PatientVisitControlIpOP.PatientId = TextBoxPatientId.Text;
            LoadOutPatient();
        }
        private void ResetForm()
        {
            BtnIpVisit.Text = "IP Visit [F6]";
            BtnOpVisit.Text = "OP Visit [F5]";
            PatientPhotoControl.Clear();
            PatientNumberPatient.PatientNumber = "000000000000";
            LinkLabelPatientIdEdit.Enabled = false;
            PatientVisitControlIpOP.PatientId = "";
            PatientErrorMsg.Text = "";
            TextBoxPatientAge.ResetText();
            DateTimePickerPatientDob.Format = Global.Company.DateFormat;
            DateTimePickerPatientDob.MaxDate = (DateTime)DateUtils.ToDate(DateTime.Now.ToString(Global.Company.DateFormat), Global.Company.DateFormat)!;
            DateTimePickerPatientDob.Reset();
            TextBoxPatientFirstName.ResetText();
            TextBoxPatientIncome.Decimals = Global.Company.PrimaryCurrency.RoundingPrecision;
            TextBoxPatientIncome.Text = TextUtils.DecimalPlace(TextBoxPatientIncome.Decimals);
            TextBoxPatientInitial.ResetText();
            TextBoxPatientLastName.ResetText();
            TextBoxPatientMobile.ResetText();
            TextBoxPatientOccupation.ResetText();
            TextBoxPatientBloodGroup.ResetText();
            TextBoxPatientPhone.ResetText();
            TextBoxPatientTaxId.ResetText();
            GroupBoxPatientAddress.Clear();
            GroupBoxPatientAddress.StateId = (long)Global.Company.Address.StatesId!;
            RbtPatientGender.Gender = GenderSelection.Male;
            ComboBoxSwapTextBoxPatientResponsibleParty.SelectedIndex = -1;
            GridViewEmergencyContact.Rows.Clear();
            GridViewGuardianInfo.Rows.Clear();
            GridViewInsuranceInfo.Rows.Clear();
            TreeViewPatientDocument.Nodes.Clear();
            TreeViewInsurance.Nodes.Clear();
            TextBoxOtherNotes.ResetText();
            LoadMedicalHistory();
            LoadPatientDocumentCategory();
            GridViewInPatient.Clear();
            GridViewOutPatient.Clear();
            PatientPhotoControl.Clear();
        }
        private void EnableForm(Boolean enable)
        {
            if (enable)
            {
                BtnNew.Enabled = !enable;
                BtnDelete.Enabled = !enable;
                BtnIpVisit.Enabled = !enable;
                BtnIPSticker.Enabled = !enable;
                BtnOpVisit.Enabled = !enable;
                BtnOPSticker.Enabled = !enable;
                BtnPrint.Enabled = !enable;
                BtnPatientPrintSticker.Enabled = !enable;
                BtnSave.Enabled = enable;
                BtnCancel.Enabled = enable;
                BtnAddGuardian.Enabled = !enable;
                BtnAddEmergencyContact.Enabled = !enable;
                BtnAddInsurance.Enabled = !enable;
                GridViewPatientHistory.Enabled = !enable;
                TextBoxOtherNotes.Enabled = !enable;
                TreeViewPatientDocument.Enabled = !enable;
                TreeViewInsurance.Enabled = !enable;
                BtnInsuranceScan.Enabled = !enable;
                BtnInsuranceUpload.Enabled = !enable;
                BtnPatientScan.Enabled = !enable;
                BtnPatientUpload.Enabled = !enable;
                GroupBoxPatientAddress.ReadOnly = !enable;
                GroupBoxPatientAddress.TabStop = enable;
            }
            else
            {
                BtnNew.Enabled = !enable;
                BtnDelete.Enabled = !enable;
                BtnIpVisit.Enabled = !enable;
                BtnIPSticker.Enabled = !enable;
                BtnOpVisit.Enabled = !enable;
                BtnOPSticker.Enabled = !enable;
                BtnPrint.Enabled = !enable;
                BtnPatientPrintSticker.Enabled = !enable;
                BtnSave.Enabled = !enable;
                BtnCancel.Enabled = !enable;
                BtnAddGuardian.Enabled = !enable;
                BtnAddEmergencyContact.Enabled = !enable;
                BtnAddInsurance.Enabled = !enable;
                //GridViewPatientHistory.Enabled = !enable;
                TextBoxOtherNotes.Enabled = !enable;
                TreeViewPatientDocument.Enabled = !enable;
                TreeViewInsurance.Enabled = !enable;
                GroupBoxPatientAddress.ReadOnly = enable;
                GroupBoxPatientAddress.TabStop = enable;
                DocumentCategory DocumentCategory = DocumentManager.Instance.GetDocumentCategoryById(4);
                if (DocumentCategory != null)
                {
                    BtnInsuranceScan.Enabled = !enable;
                    BtnInsuranceUpload.Enabled = !enable;
                }
                if (TreeViewPatientDocument.Nodes != null && TreeViewPatientDocument.Nodes.Count > 0 && TreeViewPatientDocument.SelectedNode != null)
                {
                    BtnPatientScan.Enabled = !enable;
                    BtnPatientUpload.Enabled = !enable;
                }
                ResetIpOp();
            }
        }
        private void ResetIpOp()
        {
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                BtnIpVisit.Enabled = true;
                BtnIPSticker.Enabled = false;
                BtnOpVisit.Enabled = true;
                BtnOPSticker.Enabled = false;
                Registration RegistrationFromDB = OpManager.Instance.GetOPRecordIfAdmitted(long.Parse(TextBoxPatientId.Text));
                Registration lRegistrationFromDB = OpManager.Instance.GetPatientInOpQueue(long.Parse(TextBoxPatientId.Text), Global.getTransactionDate());
                if (RegistrationFromDB != null)
                {
                    BtnIpVisit.Enabled = false;
                    BtnOpVisit.Enabled = false;
                    BtnIPSticker.Enabled = true;
                    //BtnOPSticker.Enabled = true;
                    BtnIpVisit.Text = "In Patient";
                    BtnOpVisit.Text = "In Patient";
                    InPatientAdmission Admission = IpManager.GetAdmittedInPatientAdmissionByPatientId((long)RegistrationFromDB.PatientId!);
                    if (Admission != null)
                    {
                        InPatientLocation Location = IpManager.GetInPatientLocationbyAdmissionId(Admission.Id);
                        if (Location != null)
                        {
                            PatientErrorMsg.Text = string.Format(PatientAsInPatientMsg, Location.Ward.Name, Location.Bed.Name);
                        }
                    }
                }
                else if (lRegistrationFromDB != null)
                {
                    BtnOPSticker.Enabled = true;
                    BtnOpVisit.Enabled = false;
                    BtnOpVisit.Text = "In OP Queue";
                    PatientErrorMsg.Text = string.Format(PatientAsOutPatientMsg, lRegistrationFromDB.TockenNo);
                }
            }
        }
        private void GridViewGuardianInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 5)
                {
                    FormParentOrGuardian FormAddGuardian = new FormParentOrGuardian(this);
                    FormAddGuardian.GuardianId = long.Parse(GridViewGuardianInfo.CurrentRow.Cells[7].Value.ToString()!);
                    FormAddGuardian.ShowDialog();
                    LoadGuardians();
                }
                if (e.ColumnIndex == 6)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete Guardian " + GridViewGuardianInfo.Rows[e.RowIndex].Cells[0].Value.ToString() + "?", "Delete Confirm",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (GuardianManager.DeleteGuardian((long)GridViewGuardianInfo.CurrentRow.Cells[7].Value))
                        {
                            LoadGuardians();
                        }
                        else
                        {
                            PatientErrorMsg.Text = string.Format(DoNotAllowToDeleteGuardianMsg, GridViewGuardianInfo.CurrentRow.Cells[4].Value.ToString(), GridViewGuardianInfo.CurrentRow.Cells[0].Value.ToString());
                        }
                    }
                }
            }
        }
        private void GridViewEmergencyContact_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 3)
                {
                    FormEmergencyContact FormEmergencyContact = new FormEmergencyContact(this);
                    FormEmergencyContact.EmergencyContId = long.Parse(GridViewEmergencyContact.CurrentRow.Cells[5].Value.ToString()!);
                    FormEmergencyContact.ShowDialog();
                    LoadEmergencyCont();
                }
                if (e.ColumnIndex == 4)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete Emergency Contact " + GridViewEmergencyContact.Rows[e.RowIndex].Cells[0].Value.ToString() + "?", "Delete Confirm",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (EmergancyContactManager.DeleteEmergencyContact((long)GridViewEmergencyContact.CurrentRow.Cells[5].Value))
                        {
                            LoadEmergencyCont();
                        }
                        else
                        {
                            PatientErrorMsg.Text = DoNotAllowToDeleteEmergencyContactMsg;
                        }
                    }
                }
            }
        }
        private void GridViewInsuranceInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 9)
                {
                    long InsuranceId = 0;
                    FormInsurance FormInsurance = new FormInsurance(this);
                    FormInsurance.TextBoxPatientId.Text = TextBoxPatientId.Text;
                    FormInsurance.InsuranceId = InsuranceId = long.Parse(GridViewInsuranceInfo.CurrentRow.Cells[11].Value.ToString()!);
                    FormInsurance.ShowDialog();
                    LoadInsuranceInfo();
                    FocusedRowRetrivedByInsuranceId(InsuranceId);
                }
                if (e.ColumnIndex == 10)
                {
                    DialogResult Result = MessageBox.Show("Do you want to delete Insurance Info " + GridViewInsuranceInfo.Rows[e.RowIndex].Cells[2].Value.ToString() + "?", "Delete Confirm",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Result == DialogResult.Yes)
                    {
                        if (InsuranceInfoManager.DeleteInsuranceInfo((long)GridViewInsuranceInfo.CurrentRow.Cells[11].Value))
                        {
                            LoadInsuranceInfo();
                        }
                        else
                        {
                            PatientErrorMsg.Text = DoNotAllowToDeleteInsuranceMsg;
                        }
                    }
                }
            }
        }
        public void FocusedRowRetrivedByInsuranceId(long InsuranceId)
        {
            foreach (DataGridViewRow Row in GridViewInsuranceInfo.Rows)
            {
                if (Row.Cells[11].Value.ToString() == InsuranceId.ToString())
                {
                    GridViewInsuranceInfo.Select();
                    GridViewInsuranceInfo.CurrentCell = GridViewInsuranceInfo[0, Row.Index];
                    GridViewInsuranceInfo.CurrentCell.Selected = true;
                }
            }
        }
        private void GridViewGuardianInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private void BtnAddGuardian_EnabledChanged(object sender, EventArgs e)
        {
            ComboBoxSwapTextBoxPatientResponsibleParty.Visible = BtnAddGuardian.Enabled;
        }
        private void ComboBoxSwapTextBoxPatientResponsibleParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.ComboBoxSwapTextBoxPatientResponsibleParty.DroppedDown = false;
        }
        private void DateTimePickerPatientDob_Leave(object sender, EventArgs e)
        {
            if (DateTimePickerPatientDob.Date != null && DateUtils.ValidDate_TillCurrentDate(((DateTime)DateTimePickerPatientDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat))
            {
                TextBoxPatientAge.Text = DateUtils.ComputeAge(DateTime.Now, (DateTime)DateUtils.ToDate(((DateTime)DateTimePickerPatientDob.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat)!).ToString();
            }
            else
            {
                TextBoxPatientAge.ResetText();
            }
        }
        private void BtnAddEmergencyContact_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab)
            {
                PatientPhotoControl.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                BtnAddGuardian.Select();
            }
        }
        private void BtnAddInsurance_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnInsuranceUpload.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlPatient.SelectedTab = PatientInfoTab;
                BtnPatientPrintSticker.Focus();
            }
        }
        private void GridViewPatientHistory_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            int CurrColumn = GridViewPatientHistory.CurrentCell.ColumnIndex;
            int CurrRow = GridViewPatientHistory.CurrentCell.RowIndex;
            int Totalrow = GridViewPatientHistory.Rows.Count;
            int Totalcolumn = GridViewPatientHistory.Columns.Count;

            if (e.KeyCode == Keys.Tab)
            {
                if (e.Modifiers != Keys.Shift)
                {
                    if (GridViewPatientHistory.CurrentCell.GetType() == typeof(DataGridViewCheckBoxCell) && GridViewPatientHistory.CurrentCell.Value != null)
                    {
                        if (GridViewPatientHistory.CurrentCell.GetType() == typeof(DataGridViewCheckBoxCell))
                        {
                            if ((bool)GridViewPatientHistory.Rows[CurrRow].Cells[CurrColumn].Value == true)
                            {
                                if (GridViewPatientHistory.Rows[CurrRow + 1].Cells[CurrColumn + 1].ReadOnly == false)
                                {
                                    GridViewPatientHistory.Focus();
                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[CurrRow + 1].Cells[CurrColumn];
                                    shouldExecuteEditingControlShowing = false;
                                    return;
                                }
                                else
                                {
                                    for (int j = CurrRow; j < Totalrow; j++)
                                    {
                                        for (int k = CurrColumn + 1; k < Totalcolumn - 2; k++)
                                        {
                                            if (GridViewPatientHistory.Rows[j].Cells[k].GetType() == typeof(DataGridViewCheckBoxCell))
                                            {
                                                GridViewPatientHistory.Focus();
                                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j].Cells[k - 1];
                                                return;
                                            }
                                        }
                                        CurrColumn = 0;
                                    }
                                    TextBoxOtherNotes.Focus();
                                }
                            }
                            else
                            {
                                for (int j = CurrRow; j < Totalrow; j++)
                                {
                                    for (int k = CurrColumn + 1; k < Totalcolumn - 2; k++)
                                    {
                                        if (GridViewPatientHistory.Rows[j].Cells[k].GetType() == typeof(DataGridViewCheckBoxCell))
                                        {
                                            GridViewPatientHistory.Focus();
                                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j].Cells[k - 1];
                                            return;
                                        }
                                    }
                                    CurrColumn = 0;
                                }
                                TextBoxOtherNotes.Focus();
                            }
                        }
                    }
                }
                else
                {
                    if ((CurrColumn == 1 && CurrRow == 1) || (CurrColumn == 0 && CurrRow == 0))
                    {
                        TabControlPatient.SelectedTab = PatientInsuranceDetailsTab;
                        BtnInsuranceUpload.Focus();
                        return;
                    }
                    if (GridViewPatientHistory.CurrentCell.GetType() == typeof(DataGridViewCheckBoxCell))
                    {
                        if (CurrColumn != 1)
                        {
                            if ((bool)GridViewPatientHistory.Rows[CurrRow].Cells[CurrColumn - 2].Value == true)
                            {
                                if (GridViewPatientHistory.Rows[CurrRow + 1].Cells[CurrColumn - 1].ReadOnly == false)
                                {
                                    shouldExecuteEditingControlShowing = false;
                                    GridViewPatientHistory.Focus();
                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[CurrRow + 1].Cells[CurrColumn];
                                    return;
                                }
                                else
                                {
                                    for (int j = CurrRow; j >= 1; j--)
                                    {
                                        for (int k = CurrColumn - 1; k >= 0; k--)
                                        {
                                            if (GridViewPatientHistory.Rows[j].Cells[k].GetType() == typeof(DataGridViewCheckBoxCell))
                                            {
                                                GridViewPatientHistory.Focus();
                                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j].Cells[k + 1];
                                                return;
                                            }
                                        }
                                        CurrColumn = 8;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = CurrRow; j >= 1; j--)
                                {
                                    for (int k = CurrColumn - 1; k >= 0; k--)
                                    {
                                        if (GridViewPatientHistory.Rows[j].Cells[k].GetType() == typeof(DataGridViewCheckBoxCell))
                                        {
                                            GridViewPatientHistory.Focus();
                                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j].Cells[k + 1];
                                            return;
                                        }
                                    }
                                    CurrColumn = 8;
                                }
                            }
                        }
                        else
                        {
                            for (int j = CurrRow; j >= 1; j--)
                            {
                                for (int k = CurrColumn - 1; k >= 0; k--)
                                {
                                    if (GridViewPatientHistory.Rows[j].Cells[k].GetType() == typeof(DataGridViewCheckBoxCell))
                                    {
                                        GridViewPatientHistory.Focus();
                                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j].Cells[k + 1];
                                        return;
                                    }
                                }
                                CurrColumn = 8;
                            }
                        }
                    }
                }
            }
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F2))
            {
                BtnSearchPatient.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F3))
            {
                BtnNew.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F4))
            {
                BtnDelete.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F5))
            {
                BtnOpVisit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F6))
            {
                BtnIpVisit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.F9))
            {
                BtnPrint.PerformClick();
                return true;
            }
            if (keyData == Keys.F8 || (keyData == Keys.Enter && BtnSave.Focused))
            {
                BtnSave_Click(this, null!);
                return true;
            }
            else if (keyData == (Keys.F10))
            {
                BtnExit.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Escape))
            {
                BtnCancel.PerformClick();
                return true;
            }
            else if (keyData == Keys.Tab && TextBoxOtherNotes.Focused)
            {
                TabControlPatient.SelectedTab = PatientDocumenttab;
                BtnPatientUpload.Focus();
                return true;
            }
            else if (keyData == (Keys.Shift | Keys.Tab) && TextBoxOtherNotes.Focused)
            {
                int TRow = GridViewPatientHistory.Rows.Count;
                int TColumn = GridViewPatientHistory.Columns.Count;
                TabControlPatient.SelectedTab = PatientHistoryTab;
                GridViewPatientHistory.Focus();
                for (int j = TRow - 1; j >= 1; j--)
                {
                    for (int k = TColumn - 1; k >= 0; k--)
                    {
                        if (GridViewPatientHistory.Rows[j].Cells[k].GetType() == typeof(DataGridViewCheckBoxCell))
                        {
                            GridViewPatientHistory.Focus();
                            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[j].Cells[k];
                            return true;
                        }
                    }
                    TColumn = 9;
                }
            }
            if (keyData == Keys.Tab && BtnSave.Focused && TabControlPatient.SelectedTab == PatientInfoTab)
            {
                TextBoxPatientFirstName.Focus();
                return true;
            }
            if (keyData == (Keys.Shift | Keys.Tab) && TextBoxPatientFirstName.Focused && TabControlPatient.SelectedTab == PatientInfoTab)
            {
                BtnSave.Focus();
                return true;
            }
            try
            {
                if (keyData == Keys.Tab && ActiveControl == TextBoxPatientIncome && GroupBoxPatientAddress.Enabled)
                {
                    GroupBoxPatientAddress.Focus();
                    ActiveControl = GroupBoxPatientAddress;
                    return true;
                }
                if ((keyData & (Keys.Tab | Keys.Shift)) == (Keys.Tab | Keys.Shift) && ActiveControl == TextBoxPatientIncome)
                {
                    TextBoxPatientOccupation.Focus();
                    return true;
                }
                if ((keyData & (Keys.Tab | Keys.Shift)) == (Keys.Tab | Keys.Shift) && ActiveControl == PatientPhotoControl)
                {
                    if (BtnAddEmergencyContact.Enabled)
                    {
                        BtnAddEmergencyContact.Focus();
                    }
                    else
                    {
                        GroupBoxPatientAddress.selected_field(Fields.pin);
                    }
                    return true;
                }
                if (keyData == Keys.Tab && BtnExit.Focused == true && TabControlPatient.SelectedTab == PatientInfoTab)
                {
                    TabControlPatient.SelectedIndex = 1;
                    return true;
                }
                else if (keyData == Keys.Tab && BtnExit.Focused == true && TabControlPatient.SelectedTab == PatientInsuranceDetailsTab)
                {
                    TabControlPatient.SelectedIndex = 2;
                    GridViewPatientHistory.Rows[1].Cells[1].Selected = true;
                    return true;
                }
                else if (keyData == Keys.Tab && BtnExit.Focused == true && TabControlPatient.SelectedTab == PatientHistoryTab)
                {
                    TabControlPatient.SelectedIndex = 3;
                    return true;
                }
                else if (keyData == Keys.Tab && BtnExit.Focused == true && TabControlPatient.SelectedTab == HospitalVisitsTab)
                {
                    TabControlPatient.SelectedIndex = 4;
                    return true;
                }
                else if (keyData == Keys.Tab && BtnExit.Focused == true && TabControlPatient.SelectedTab == PatientDocumenttab)
                {
                    TabControlPatient.SelectedIndex = 0;
                    return true;
                }
            }
            catch
            {

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void ResetDirtyFlag()
        {
            formIsDirty = false;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override void PatientIdTransportReload(object sender, EventArgs e)
        {
            TextBoxPatientId.Text = PatientIdTransport.Text;
            if (!string.IsNullOrEmpty(TextBoxPatientId.Text))
            {
                LoadPatientInfo();
            }
        }
        private void PatientRegistration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formIsDirty)
            {
                if (MessageBox.Show("Do you want to save the changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (validate())
                    {
                        BtnSave.PerformClick();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            if (PatientPhotoControl.capture != null && !PatientPhotoControl.capture.IsDisposed)
            {
                Application.Idle -= PatientPhotoControl.Streaming!;
                PatientPhotoControl.capture.Release();
                PatientPhotoControl.capture.Dispose();
                PatientPhotoControl.isCameraRunning = false;
            }
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            FormChartDetails FormChartDetails = new FormChartDetails();
            FormChartDetails.TextBoxPatientId.Text = TextBoxPatientId.Text;
            FormChartDetails.ShowDialog();
        }
        private void BtnPatientUpload_Click(object sender, EventArgs e)
        {
            PatientDocumentCategoryId = 0L;
            PatientDocumentId = 0L;
            if (TreeViewPatientDocument.SelectedNode != null)
            {
                FormUploadDocumentWithPreview FormUploadDocumentWithPreview = new FormUploadDocumentWithPreview(this);
                if (FormUploadDocumentWithPreview != null)
                {
                    FormUploadDocumentWithPreview.PatientId = long.Parse(TextBoxPatientId.Text);
                    FormUploadDocumentWithPreview.CategoryId = TreeViewPatientDocument.SelectedNode.Parent == null ? long.Parse(TreeViewPatientDocument.SelectedNode.Name) : GetParenNode(TreeViewPatientDocument.SelectedNode.Parent);
                    FormUploadDocumentWithPreview.ShowDialog();
                    PatientDocumentCategoryId = FormUploadDocumentWithPreview.CategoryId;
                    PatientDocumentId = FormUploadDocumentWithPreview.PatientDocumentId;
                    LoadInsuranceDocument();
                    LoadPatientDocumentCategory();
                    TreeViewPatientDocument.Focus();
                    if (PatientDocumentCategoryId != 0L)
                    {
                        LastNodeFollow(TreeViewPatientDocument.Nodes);
                    }
                    EnableForm(false);
                }
            }
            else
            {
                PatientErrorMsg.Text = SelectDocumentErrorMsg;
            }
        }
        private long GetParenNode(TreeNode Nodes)
        {
            long CatId = 0L;
            if (Nodes.Parent != null)
            {
                GetParenNode(Nodes.Parent);
            }
            else
            {
                CatId = long.Parse(Nodes.Name);
            }
            return CatId;
        }
        public static void DeleteDirectory(string FolderPath)
        {
            if (!Directory.Exists(FolderPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(FolderPath);
            }
            else
            {
                if (Directory.Exists(FolderPath))
                {
                    foreach (string file in Directory.GetFiles(FolderPath))
                    {
                        File.Delete(file);
                    }
                    foreach (string directory in Directory.GetDirectories(FolderPath))
                    {
                        DeleteDirectory(directory);
                    }
                }
            }
        }
        private void TreeViewPatientDocument_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientErrorMsg.Text = "";
            TreeNode node = e.Node!;
            node.SelectedImageIndex = node.ImageIndex;
            BtnPatientUpload.Enabled = true;
            PictureBoxDocumentPreview.Image = null;
            DocBrowserPatient.LoadDocument("about:blank");
            pdfDocumentView1.Refresh();
            PictureBoxDocumentPreview.Visible = false;
            DocBrowserPatient.Visible = false;
            pdfDocumentView1.Visible = false;
            if (e.Node!.Name.EndsWith("@"))
            {
                PatientDocument PatientDocument = DocumentManager.Instance.GetPatientDocumentById(long.Parse(e.Node.Name.Trim('@')));
                if (PatientDocument != null)
                {
                    if (PatientDocument.FileType == FileType.RAW || PatientDocument.FileType == FileType.JPG || PatientDocument.FileType == FileType.PNG || PatientDocument.FileType == FileType.GIF ||
                        PatientDocument.FileType == FileType.CVX || PatientDocument.FileType == FileType.CNV || PatientDocument.FileType == FileType.CVI || PatientDocument.FileType == FileType.EPS ||
                        PatientDocument.FileType == FileType.BMP || PatientDocument.FileType == FileType.PCX || PatientDocument.FileType == FileType.TGA || PatientDocument.FileType == FileType.TPL ||
                        PatientDocument.FileType == FileType.NEF || PatientDocument.FileType == FileType.ORF || PatientDocument.FileType == FileType.DXF || PatientDocument.FileType == FileType.DWG ||
                        PatientDocument.FileType == FileType.PSD || PatientDocument.FileType == FileType.CR2 || PatientDocument.FileType == FileType.SR2 || PatientDocument.FileType == FileType.AI ||
                        PatientDocument.FileType == FileType.INDD || PatientDocument.FileType == FileType.TIFF || PatientDocument.FileType == FileType.JPEG || PatientDocument.FileType == FileType.JFIF)
                    {
                        PictureBoxDocumentPreview.Visible = true;
                        MemoryStream Stream = new MemoryStream(PatientDocument.File);
                        PictureBoxDocumentPreview.Image = System.Drawing.Image.FromStream(Stream);
                        PictureBoxDocumentPreview.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        try
                        {
                            string Filename = PatientDocument.FileName;
                            if (File.Exists(Path.Combine(Path.GetTempPath(), Filename + GetExtension(PatientDocument.FileType))))
                            {
                                try
                                {
                                    File.Delete(Path.Combine(Path.GetTempPath(), Filename + GetExtension(PatientDocument.FileType)));
                                }
                                catch (Exception exc)
                                {
                                    for (int i = 1; i < 1000; i++)
                                    {
                                        Filename = PatientDocument.FileName + i.ToString();
                                        if (!File.Exists(Path.Combine(Path.GetTempPath(), Filename + GetExtension(PatientDocument.FileType))))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            FileStream stream = new FileStream((Path.GetTempPath() + Filename + GetExtension(PatientDocument.FileType)), FileMode.CreateNew);
                            BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write(PatientDocument.File, 0, PatientDocument.File.Length);
                            writer.Close();
                            if (PatientDocument.FileType == FileType.PDF)
                            {
                                pdfDocumentView1.Visible = true;
                                pdfDocumentView1.ZoomMode = Syncfusion.Windows.Forms.PdfViewer.ZoomMode.FitWidth;
                                pdfDocumentView1.Load(Path.GetTempPath() + PatientDocument.FileName + GetExtension(PatientDocument.FileType));
                            }
                            else
                            {
                                PatientErrorMsg.Text = UploadStatusDocument;
                            }
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.HResult);
                        }
                    }
                }
            }
            else if (e.Node.Name.EndsWith("#"))
            {
                LabTestAttachment LabTestAttachment = ConsultationNoteManager.Instance.GetConsLabTestAttachmentById(long.Parse(e.Node.Name.Trim('#')));
                if (LabTestAttachment != null)
                {
                    if (LabTestAttachment.FileType == FileType.JPEG || LabTestAttachment.FileType == FileType.JPG || LabTestAttachment.FileType == FileType.PNG)
                    {
                        PictureBoxDocumentPreview.Visible = true;
                        MemoryStream Stream = new MemoryStream(LabTestAttachment.Attachment);
                        PictureBoxDocumentPreview.Image = System.Drawing.Image.FromStream(Stream);
                        PictureBoxDocumentPreview.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        try
                        {
                            string Filename = LabTestAttachment.FileName;
                            if (File.Exists(Path.Combine(Path.GetTempPath(), Filename + GetExtension(LabTestAttachment.FileType))))
                            {
                                try
                                {
                                    File.Delete(Path.Combine(Path.GetTempPath(), Filename + GetExtension(LabTestAttachment.FileType)));
                                }
                                catch (Exception exc)
                                {
                                    for (int i = 1; i < 1000; i++)
                                    {
                                        Filename = LabTestAttachment.FileName + i.ToString();
                                        if (!File.Exists(Path.Combine(Path.GetTempPath(), Filename + GetExtension(LabTestAttachment.FileType))))
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            FileStream stream = new FileStream(Path.GetTempPath() + Filename + GetExtension(LabTestAttachment.FileType), FileMode.CreateNew);
                            BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write(LabTestAttachment.Attachment, 0, LabTestAttachment.Attachment.Length);
                            writer.Close();
                            if (LabTestAttachment.FileType == FileType.PDF)
                            {
                                pdfDocumentView1.Visible = true;
                                pdfDocumentView1.ZoomMode = Syncfusion.Windows.Forms.PdfViewer.ZoomMode.FitWidth;
                                pdfDocumentView1.Load(Path.GetTempPath() + LabTestAttachment.FileName + GetExtension(LabTestAttachment.FileType));
                            }
                            else
                            {
                                PatientErrorMsg.Text = UploadStatusDocument;
                            }
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.HResult);
                        }
                    }
                }
            }
            else
            {
                PictureBoxDocumentPreview.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }
        private string GetExtension(FileType FileType)
        {
            if (FileType == FileType.DOC)
            {
                return ".DOC";
            }
            else if (FileType == FileType.DOCX)
            {
                return ".docx";
            }
            else if (FileType == FileType.PDF)
            {
                return ".PDF";
            }
            else if (FileType == FileType.JPEG)
            {
                return ".JPEG";
            }
            else if (FileType == FileType.JPG)
            {
                return ".JPG";
            }
            else if (FileType == FileType.PNG)
            {
                return ".PNG";
            }
            return ".PNG";
        }
        private void BtnInsuranceUpload_Click(object sender, EventArgs e)
        {
            FormUploadDocumentWithPreview FormUploadDocumentWithPreview = new FormUploadDocumentWithPreview(this);
            FormUploadDocumentWithPreview.PatientId = long.Parse(TextBoxPatientId.Text);
            FormUploadDocumentWithPreview.CategoryId = 4;
            FormUploadDocumentWithPreview.ShowDialog();
            LoadPatientDocumentCategory();
            LoadInsuranceDocument();
            TreeViewInsurance.Select();
        }
        private void TreeViewInsurance_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                PatientErrorMsg.Text = "";
                TreeNode node = e.Node!;
                node.SelectedImageIndex = node.ImageIndex;
                PictureBoxInsurance.Image = null;
                DocBrowserInsurance.LoadDocument("about:blank");
                PdfDocumentViewInsurance.Refresh();
                PdfDocumentViewInsurance.Visible = false;
                PictureBoxInsurance.Visible = false;
                DocBrowserInsurance.Visible = false;
                if (e.Node != null)
                {
                    try
                    {
                        long documentId;
                        if (!long.TryParse(e.Node.Name.Trim('@'), out documentId))
                        {
                            PatientErrorMsg.Text = InvalidIDErrorMsg;
                            return;
                        }

                        PatientDocument PatientDocument = DocumentManager.Instance.GetPatientDocumentById(documentId);
                        if (PatientDocument != null)
                        {
                            var supportedImageFormats = new List<FileType>
                                {
                                    FileType.JPEG, FileType.JPG, FileType.PNG, FileType.GIF, FileType.BMP, FileType.TIFF,
                                    FileType.RAW, FileType.CVX, FileType.CNV, FileType.CVI, FileType.EPS, FileType.PCX,
                                    FileType.TGA, FileType.TPL, FileType.NEF, FileType.ORF, FileType.DXF, FileType.DWG,
                                    FileType.PSD, FileType.CR2, FileType.SR2, FileType.AI, FileType.INDD, FileType.JFIF
                                };

                            if (supportedImageFormats.Contains(PatientDocument.FileType))
                            {
                                try
                                {
                                    PictureBoxInsurance.Visible = true;
                                    using (MemoryStream Stream = new MemoryStream(PatientDocument.File))
                                    {
                                        PictureBoxInsurance.Image = System.Drawing.Image.FromStream(Stream);
                                        PictureBoxInsurance.SizeMode = PictureBoxSizeMode.StretchImage;
                                    }
                                }
                                catch (Exception imgEx)
                                {
                                    PictureBoxInsurance.Visible = false;
                                    PatientErrorMsg.Text = string.Format(DisplayingErrorMsg, "image") + ": " + imgEx.Message;
                                }
                            }
                            else if (PatientDocument.FileType == FileType.PDF)
                            {
                                try
                                {
                                    string tempFilePath = GetTempFilePath(PatientDocument);
                                    using (FileStream stream = new FileStream(tempFilePath, FileMode.CreateNew))
                                    using (BinaryWriter writer = new BinaryWriter(stream))
                                    {
                                        writer.Write(PatientDocument.File, 0, PatientDocument.File.Length);
                                    }

                                    PdfDocumentViewInsurance.Visible = true;
                                    PdfDocumentViewInsurance.ZoomMode = Syncfusion.Windows.Forms.PdfViewer.ZoomMode.FitWidth;
                                    PdfDocumentViewInsurance.Load(tempFilePath);
                                }
                                catch (Exception pdfEx)
                                {
                                    PdfDocumentViewInsurance.Visible = false;
                                    PatientErrorMsg.Text = string.Format(DisplayingErrorMsg, "PDF") + ": " + pdfEx.Message;
                                }
                            }
                            else
                            {
                                PatientErrorMsg.Text = StatusDocumentFormat;
                            }
                        }
                        else
                        {
                            PatientErrorMsg.Text = UploadStatusDocument;
                        }
                    }
                    catch (Exception ex)
                    {
                        PatientErrorMsg.Text = $"Error loading document: {ex.Message}";
                        Console.WriteLine($"Error: {ex}");
                    }
                }
                else
                {
                    PictureBoxInsurance.Visible = true;
                    PatientErrorMsg.Text = SelectStatusDocument;
                }

            }
            catch
            {

            }
            Cursor.Current = Cursors.Default;
        }
        private string GetTempFilePath(PatientDocument document)
        {
            string baseFilename = document.FileName;
            string extension = GetExtension(document.FileType);
            string tempPath = Path.GetTempPath();
            string filePath = Path.Combine(tempPath, baseFilename + extension);

            for (int i = 0; i < 1000; i++)
            {
                if (!File.Exists(filePath))
                {
                    return filePath;
                }
                filePath = Path.Combine(tempPath, $"{baseFilename}_{i}{extension}");
            }
            throw new Exception("Could not create unique temporary file name");
        }
        private void DeleteDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientErrorMsg.Text = string.Empty;
            if (TabControlPatient.SelectedTab == PatientDocumenttab)
            {
                if (TreeViewPatientDocument.SelectedNode != null && TreeViewPatientDocument.SelectedNode.Name.Contains('@'))
                {
                    PatientDocument PatientDocument = DocumentManager.Instance.GetPatientDocumentById(long.Parse(TreeViewPatientDocument.SelectedNode.Name.Trim('@')));
                    if (PatientDocument != null)
                    {
                        DialogResult Result = MessageBox.Show(("Do you want to delete the document " + PatientDocument.FileName + "."), "Delete Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            DocumentManager.Instance.DeletePatientDocument(long.Parse(TreeViewPatientDocument.SelectedNode.Name.Trim('@')));
                            PictureBoxDocumentPreview.Image = null;
                            LoadPatientDocumentCategory();
                            LoadInsuranceDocument();
                        }
                        TreeViewPatientDocument.Select();
                    }
                }
            }
            else
            {
                if (TreeViewInsurance.SelectedNode != null)
                {
                    PatientDocument PatientDocument = DocumentManager.Instance.GetPatientDocumentById(long.Parse(TreeViewInsurance.SelectedNode.Name));
                    if (PatientDocument != null)
                    {
                        DialogResult Result = MessageBox.Show(("Do you want to delete the document " + PatientDocument.FileName + "."), "Delete Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        if (Result == DialogResult.Yes)
                        {
                            DocumentManager.Instance.DeletePatientDocument(PatientDocument.Id);
                            PictureBoxInsurance.Image = null;
                            LoadPatientDocumentCategory();
                            LoadInsuranceDocument();
                        }
                        TreeViewInsurance.Select();
                    }
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void UploadDocToolStripMenu_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (TreeViewPatientDocument.SelectedNode != null && !TreeViewPatientDocument.SelectedNode.Name.Contains('@'))
            {
                DocumentCategory DocumentCategory = DocumentManager.Instance.GetDocumentCategoryById(long.Parse(TreeViewPatientDocument.SelectedNode.Name));
                if (DocumentCategory != null)
                {
                    BtnPatientUpload_Click(sender, e);
                }
            }
            else
            {
                PatientErrorMsg.Text = SelectDocumentErrorMsg;
            }
            Cursor.Current = Cursors.Default;
        }
        private void TreeViewPatientDocument_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewPatientDocument.SelectedNode = e.Node;
        }
        private void TreeViewInsurance_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewInsurance.SelectedNode = e.Node;
        }
        private void TreeViewPatientDocument_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }
        private void TreeViewPatientDocument_DragOver(object sender, DragEventArgs e)
        {
            System.Drawing.Point targetPoint = TreeViewPatientDocument.PointToClient(new System.Drawing.Point(e.X, e.Y));
            TreeViewPatientDocument.SelectedNode = TreeViewPatientDocument.GetNodeAt(targetPoint);
        }
        private void TreeViewPatientDocument_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }
        private void TreeViewPatientDocument_DragDrop(object sender, DragEventArgs e)
        {
            System.Drawing.Point targetPoint = TreeViewPatientDocument.PointToClient(new System.Drawing.Point(e.X, e.Y));
            TreeNode targetNode = TreeViewPatientDocument.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data!.GetData(typeof(TreeNode));
            if (draggedNode != null && !draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode) && targetNode != null && !targetNode.Name.EndsWith("@")
                && !targetNode.Name.EndsWith("#") && !draggedNode.Name.EndsWith("#"))
            {
                if (e.Effect == DragDropEffects.Move && draggedNode.Name.EndsWith("@"))
                {
                    PatientDocument PatientDocument = DocumentManager.Instance.GetPatientDocumentById(long.Parse(draggedNode.Name.Trim('@')));
                    if (PatientDocument != null)
                    {
                        PatientDocument.PatientDocumentCategoryId = long.Parse(targetNode.Name);
                        if (DocumentManager.Instance.CheckFileNameExists(PatientDocument.PatientDocumentCategoryId, PatientDocument.PatientId, PatientDocument.FileName))
                        {
                            DocumentManager.Instance.UpdatePatientDocument(PatientDocument);
                            draggedNode.Remove();
                            targetNode.Nodes.Add(draggedNode);
                            LoadInsuranceDocument();
                        }
                        else
                        {
                            PatientErrorMsg.Text = "The file name " + PatientDocument.FileName + GetExtension(PatientDocument.FileType) + " exists in target category.";
                        }
                    }
                }
                targetNode.Expand();
            }
        }
        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if (node2 == null) return false;
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;
            return ContainsNode(node1, node2.Parent);
        }
        bool FlagEntered;
        private void TextBoxPatientFirstName_MouseUp(object sender, MouseEventArgs e)
        {
            if ((sender as TextBox)!.SelectedText == "" && !FlagEntered)
            {
                (sender as TextBox)!.SelectAll();
                FlagEntered = true;
            }
        }
        private void TextBoxPatientFirstName_Leave(object sender, EventArgs e)
        {
            FlagEntered = false;
        }
        private bool TxtFocus(String input, TextBox sender)
        {
            if (input.Length < 0)
            {
                return false;
            }
            else
            {
                sender.Focus();
                sender.SelectionStart = 0;
                sender.SelectionLength = sender.Text.Length;
                return true;
            }
        }
        private void TextBoxPatientLastName_Click(object sender, EventArgs e)
        {
            TxtFocus(TextBoxPatientLastName.Text, TextBoxPatientLastName);
        }
        private void TextBoxPatientMobile_Click(object sender, EventArgs e)
        {
            TextBoxPatientMobile.Focus();
            TextBoxPatientMobile.SelectionStart = 0;
            TextBoxPatientMobile.SelectionLength = TextBoxPatientMobile.Text.Length;
        }
        private void TextBoxPatientTaxId_Click(object sender, EventArgs e)
        {
            TextBoxPatientTaxId.Select();
        }
        private void TextBoxPatientOccupation_Click(object sender, EventArgs e)
        {
            TextBoxPatientOccupation.Select();
        }
        private void TextBoxPatientBloodGroup_Click(object sender, EventArgs e)
        {
            TextBoxPatientBloodGroup.Select();
        }
        private void TextBoxPatientIncome_Click(object sender, EventArgs e)
        {
            TxtFocus(TextBoxPatientIncome.Text, TextBoxPatientIncome);
        }
        private void TextBoxPatientAge_Click(object sender, EventArgs e)
        {
            TxtFocus(TextBoxPatientAge.Text, TextBoxPatientAge);
        }
        private void LinkLabelPatientIdEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormPatientIdEdit PatientIdEdit = new FormPatientIdEdit(this);
            PatientIdEdit.PatientNo = PatientNumberPatient.PatientNumber;
            PatientIdEdit.PatientNumber = PatientNumberPatient.PatientNumber;
            PatientIdEdit.ShowDialog();
            if (PatientIdEdit.PatientNumberUpdated == true)
            {
                PatientNumberPatient.PatientNumber = PatientIdEdit.PatientNo;
            }
        }
        private void PatientDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControlPatient.SelectedIndex == 0)
            {
                TextBoxPatientFirstName.Focus();
            }
            if (TabControlPatient.SelectedIndex == 1)
            {
                BtnAddInsurance.Focus();
            }
            if (TabControlPatient.SelectedIndex == 2)
            {
                GridViewPatientHistory.Rows[1].Cells[1].Selected = true;
                GridViewPatientHistory.Focus();
            }
            if (TabControlPatient.SelectedIndex == 3)
            {
                GridViewOutPatient.Focus();
            }
            if (TabControlPatient.SelectedIndex == 4)
            {
                if (BtnPatientUpload.Enabled)
                {
                    BtnPatientUpload.Focus();
                }
                TreeViewPatientDocument.Select();
            }
        }
        private void BtnInsuranceUpload_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab)
            {
                if (TabControlPatient.SelectedTab == PatientInsuranceDetailsTab)
                {
                    TabControlPatient.SelectedTab = PatientHistoryTab;
                    GridViewPatientHistory.Focus();
                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[1].Cells[1];
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlPatient.SelectedTab = PatientInsuranceDetailsTab;
                BtnAddInsurance.Focus();
            }
        }
        private void LinkLabelPatientIdEdit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPatientPrintSticker.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                PatientPhotoControl.Select();
            }
        }

        private void GridViewPatientHistory_Enter(object sender, EventArgs e)
        {
            GridViewPatientHistory.Focus();
            GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[1].Cells[1];
            //GridViewPatientHistory.Select();
        }

        private void BtnPatientPrintSticker_Click(object sender, EventArgs e)
        {
            FormCatalogBarCodePrint printForm = new FormCatalogBarCodePrint(this);
            printForm.PatientId = long.Parse(TextBoxPatientId.Text);
            printForm.ShowDialog();
        }

        private void GroupBoxPatientAddress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                e.IsInputKey = true;
                if (BtnAddGuardian.Enabled)
                {
                    BtnAddGuardian.Select();
                }
                else
                {
                    BtnSave.Select();
                }
            }
        }

        private void BtnSave_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab && e.Modifiers != Keys.Shift)
            {
                TabControlPatient.SelectedTab = PatientInfoTab;
                TextBoxPatientFirstName.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                if (TabControlPatient.SelectedTab == PatientInfoTab && ActiveControl == BtnSave)
                {
                    if (BtnPatientPrintSticker.Enabled)
                    {
                        BtnPatientPrintSticker.Focus();
                    }
                    else
                    {
                        BtnPatientUpload.Focus();
                    }
                }
            }
        }

        private void ComboBoxSwapTextBoxPatientResponsibleParty_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnAddGuardian.Select();
            }
            if (e.KeyCode == Keys.Tab && e.Modifiers == Keys.Shift)
            {
                GroupBoxPatientAddress.selected_field(Fields.pin);
            }
        }
        private void BtnPatientUpload_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                TabControlPatient.SelectedTab = PatientInfoTab;
                BtnSave.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlPatient.SelectedTab = PatientHistoryTab;
                TextBoxOtherNotes.Focus();
            }
        }

        private void TreeViewPatientDocument_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true;
                BtnPatientUpload.Select();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TabControlPatient.SelectedTab = PatientInsuranceDetailsTab;
                BtnInsuranceUpload.Select();
            }
        }

        long PatientDocumentId = 0L;
        long PatientDocumentCategoryId = 0L;

        private void LastNodeFollow(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Name.Contains(PatientDocumentCategoryId.ToString()))
                {
                    if (node.Nodes != null && node.Nodes.Count > 0)
                    {
                        foreach (TreeNode lnode in node.Nodes)
                        {
                            if (lnode.Name.Contains(PatientDocumentId.ToString() + "@"))
                            {
                                node.ExpandAll();
                                node.TreeView.SelectedNode = lnode;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void GroupBoxPatientAddress_PreviewKeyDown_1(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab)
            {
                if (!BtnAddGuardian.Enabled)
                {
                    PatientPhotoControl.Focus();
                }
                else
                {
                    ComboBoxSwapTextBoxPatientResponsibleParty.Focus();
                }
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                TextBoxPatientIncome.Focus();
            }
        }
        private void BtnAddGuardian_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
            if (e.KeyCode == Keys.Tab)
            {
                BtnAddEmergencyContact.Focus();
            }
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Tab)
            {
                ComboBoxSwapTextBoxPatientResponsibleParty.Focus();
            }
        }

        private void GridViewPatientHistory_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox)
            {
                textBox.KeyPress += GridViewPatientHistory_KeyPress!;
                textBox.AcceptsTab = true;
            }
        }

        private void GridViewPatientHistory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (shouldExecuteEditingControlShowing)
            {
                int Totalrow = GridViewPatientHistory.Rows.Count;
                int Totalcolumn = GridViewPatientHistory.Columns.Count;
                int CurrColumn = GridViewPatientHistory.CurrentCell.ColumnIndex;
                int CurrRow = GridViewPatientHistory.CurrentCell.RowIndex;
                if (e.KeyChar == (char)Keys.Tab)
                {
                    if (CurrRow > 0)
                    {
                        if (ModifierKeys.HasFlag(Keys.Shift))
                        {
                            if (GridViewPatientHistory.CurrentCell.GetType() == typeof(DataGridViewTextBoxCell))
                            {
                                GridViewPatientHistory.Focus();
                                GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[CurrRow - 1].Cells[CurrColumn];
                            }
                            e.Handled = true;
                        }
                        else
                        {
                            if (CurrColumn != 0)
                            {
                                if (GridViewPatientHistory.CurrentCell.GetType() == typeof(DataGridViewTextBoxCell))
                                {
                                    if (GridViewPatientHistory.Rows[CurrRow - 1].Cells[CurrColumn].GetType() == typeof(DataGridViewCheckBoxCell))
                                    {
                                        GridViewPatientHistory.Focus();
                                        GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[CurrRow - 1].Cells[CurrColumn];
                                    }
                                    else
                                    {
                                        TextBoxOtherNotes.Focus();
                                    }
                                }
                            }
                            else
                            {
                                if (GridViewPatientHistory.Rows[CurrRow].Cells[CurrColumn + 1].GetType() == typeof(DataGridViewCheckBoxCell))
                                {
                                    GridViewPatientHistory.Focus();
                                    GridViewPatientHistory.CurrentCell = GridViewPatientHistory.Rows[CurrRow].Cells[CurrColumn + 1];
                                }
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
            shouldExecuteEditingControlShowing = true;
            if (e.KeyChar == (char)Keys.Escape && TabControlPatient.SelectedTab == PatientHistoryTab)
            {
                BtnCancel_Click(this, null!);
            }
        }

        private void BtnOPSticker_Click(object sender, EventArgs e)
        {
            FormCatalogBarCodePrint printForm = new FormCatalogBarCodePrint(this);
            printForm.PatientId = long.Parse(TextBoxPatientId.Text);
            printForm.IsOP = true;
            printForm.IsIP = false;
            printForm.ShowDialog();
        }

        private void BtnIPSticker_Click(object sender, EventArgs e)
        {
            FormCatalogBarCodePrint printForm = new FormCatalogBarCodePrint(this);
            printForm.PatientId = long.Parse(TextBoxPatientId.Text);
            printForm.IsOP = false;
            printForm.IsIP = true;
            printForm.ShowDialog();
        }
        private void buttonImport_Click(object sender, EventArgs e)
        {
            FormPatientUpload formPatientUpload = new FormPatientUpload();
            formPatientUpload.ShowDialog();
        }

        private void BtnAddGuardian_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(sender, e);
        }

        private void BtnAddEmergencyContact_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(sender, e);
        }
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                BtnSearchPatient.PerformClick();
            }
            else if (e.KeyCode == Keys.F3)
            {
                BtnNew.PerformClick();
            }
            else if (e.KeyCode == Keys.F4)
            {
                BtnDelete.PerformClick();
            }
            else if (e.KeyCode == Keys.F5)
            {
                BtnOpVisit.PerformClick();
            }
            else if (e.KeyCode == Keys.F6)
            {
                BtnIpVisit.PerformClick();
            }
            else if (e.KeyCode == Keys.F9)
            {
                BtnPrint.PerformClick();
            }
            else if (e.KeyCode == Keys.F8 || (e.KeyCode == Keys.Enter && BtnSave.Focused))
            {
                BtnSave_Click(this, null!);
            }
            else if (e.KeyCode == Keys.F10)
            {
                BtnExit.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnCancel.PerformClick();
            }
        }
    }
}

