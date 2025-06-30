using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ExcelDataReader;
using fa.api.Hms;
using fa.libraries.utils;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.views.controls;
using Fa.api.Hms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fa.views.hms.masters.upload
{
    public partial class MedicalTestFileUpload : Form
    {
        public bool FileUploadComplete;
        public bool PauseProcessing;
        public bool IsBreak = false;
        public static string RefreshMedicalTestConfirmText = "Are you sure you want to delete the previous medical test data? This action cannot be undone.";
        public MedicalTestFileUpload()
        {
            InitializeComponent();
        }
        private void MedicalTestFileUpload_Load(object sender, EventArgs e)
        {
            FileUploadMedicalTest.RefreshallDataVisibility();
        }
        private void FileUploadMedicalTest_OnClickUpload(object sender, EventArgs e)
        {
            string FileName = FileUploadMedicalTest.FileName();
            bool HasHeader = FileUploadMedicalTest.HasHeader();
            bool IsColumnValid = true;
            bool refreshAllData = FileUploadMedicalTest.RefreshAllData();
            bool isProcessed = false;
            string[] MedicalTestHeader = new string[] { "MedicalTestCategory", "CategoryDisplayName", "CategoryDescription", "IsSubMedicalTestCategory", "ParentCategory", "TestName", "DisplayAs", "TestCode", "TestShortName", "TestDescription", "SampleRequirement", "Active", "Reason", "HasElement", "Keywords", "ElementCode", "ElementName", "ElementShortName", "Class", "SubClass", "UOM", "UomDescription", "RangeFrom", "RangeTo", "SingleValue", "TimetoResult", "ElementDescription" };
            Stopwatch stopw = new Stopwatch();
            using (var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    FileUploadComplete = true;
                    int rowcount = reader.RowCount;
                    stopw.Start();
                    this.UseWaitCursor = false;
                    Cursor.Current = Cursors.WaitCursor;
                    FileUploadMedicalTest.SetProgressMax(rowcount);
                    int row = 1;
                    do
                    {
                        while (reader.Read())
                        {
                            if (PauseProcessing == true)
                            {
                                this.UseWaitCursor = false;
                                FileUploadMedicalTest.AddAccessLog("Paused");
                                string message = "Do you want to exit from FileUpload Processing?";
                                string title = "Exit FileUpload";
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    FileUploadMedicalTest.StopProcessing = true;
                                    this.Invoke((new Action(() => this.Close())));
                                }
                                else
                                {
                                    this.UseWaitCursor = true;
                                    FileUploadMedicalTest.AddAccessLog("Continuing");
                                    PauseProcessing = false;
                                    FileUploadMedicalTest.StopProcessing = false;
                                    FileUploadMedicalTest.EnableExitButton(true);
                                    continue;
                                }
                            }
                            if (FileUploadMedicalTest.StopProcessing == false)
                            {
                                string TestName = "";
                                string TestDisplayAs = "";
                                string TestDescription = "";
                                string Reason = "";
                                string CategoryName = "";
                                string CategoryDisplayAs = "";
                                string CategoryDescription = "";
                                long CategoryId = 0L;
                                long UomId = 0L;
                                int fc = 0;
                                try
                                {
                                    if (MedicalTestHeader.Length == reader.FieldCount)
                                    {
                                        if (row == 1 && HasHeader)
                                        {
                                            foreach (string sh in MedicalTestHeader)
                                            {
                                                if (sh.Trim() != reader.GetValue(fc).ToString()!.Trim())
                                                {
                                                    IsColumnValid = false;
                                                }
                                                fc++;
                                            }
                                            row++;
                                            continue;
                                        }
                                        else if (row == 1 && !HasHeader)
                                        {
                                            foreach (string sh in MedicalTestHeader)
                                            {
                                                var value = reader.GetValue(fc);
                                                if (sh == value.ToString())
                                                {
                                                    IsColumnValid = false;
                                                    fc++;
                                                }
                                                else
                                                {
                                                    IsColumnValid = true;
                                                    break;
                                                }
                                            }
                                            if (!IsColumnValid)
                                            {
                                                FileUploadMedicalTest.AddErrorLog("Row No - " + row + ": Please check the file have headers");
                                                IsColumnValid = true;
                                                row++;
                                                continue;
                                            }
                                        }
                                        if (IsColumnValid)
                                        {
                                            if (refreshAllData && !isProcessed)
                                            {
                                                Cursor.Current = Cursors.WaitCursor;
                                                isProcessed = true;
                                                IList<MedicalTestCategory> medicalTestCategorys = MedicalTestManager.Instance.ListMedicalTestCategoryByCompanyId(Global.Company.CompanyId);
                                                if (medicalTestCategorys != null && medicalTestCategorys.Count > 0)
                                                {
                                                    foreach (MedicalTestCategory medicalTestCategory in medicalTestCategorys)
                                                    {
                                                        bool testliked = false;
                                                        List<MedicalTestCategory> ChildCategorys = MedicalTestManager.Instance.GetChildMedicalTestCategoryByParentId(medicalTestCategory.Id, Global.Company.CompanyId);
                                                        List<MedicalTest> medicalTests = null!;
                                                        if (ChildCategorys != null && ChildCategorys.Count > 0)
                                                        {
                                                            foreach (MedicalTestCategory childCategory in ChildCategorys)
                                                            {
                                                                medicalTests = MedicalTestManager.Instance.GetMedicalTestByCategoryId(childCategory.Id, Global.Company.CompanyId);
                                                                foreach (var medicalTest in medicalTests)
                                                                {
                                                                    if (IsAnyMedicalTestLinkedToConsultation(medicalTest))
                                                                    {
                                                                        MedicalTestManager.Instance.DeleteMedicalTest(medicalTest.Id);
                                                                    }
                                                                    else
                                                                    {
                                                                        testliked = true;
                                                                    }
                                                                }
                                                                if (!testliked)
                                                                {
                                                                    MedicalTestManager.Instance.DeleteMedicalTestCategory(childCategory.Id);
                                                                }
                                                            }
                                                        }
                                                        medicalTests = MedicalTestManager.Instance.GetMedicalTestByCategoryId(medicalTestCategory.Id, Global.Company.CompanyId);
                                                        foreach (var medicalTest in medicalTests)
                                                        {
                                                            if (IsAnyMedicalTestLinkedToConsultation(medicalTest))
                                                            {
                                                                MedicalTestManager.Instance.DeleteMedicalTest(medicalTest.Id);
                                                            }
                                                            else
                                                            {
                                                                testliked = true;
                                                            }
                                                        }
                                                        if (!testliked)
                                                        {
                                                            MedicalTestManager.Instance.DeleteMedicalTestCategory(medicalTestCategory.Id);
                                                        }
                                                    }
                                                }
                                                Cursor.Current = Cursors.Default;
                                            }
                                            FileUploadMedicalTest.IncrementProgress();
                                            CategoryName = reader.GetValue(0) != null ? reader.GetValue(0).ToString()!.Trim() : string.Empty;
                                            if (CategoryName.Length > 30)
                                            {
                                                CategoryName = CategoryName.Substring(0, 30);
                                            }
                                            CategoryDisplayAs = reader.GetValue(1) != null ? reader.GetValue(1).ToString()!.Trim() : string.Empty;
                                            CategoryDescription = reader.GetValue(2) != null ? reader.GetValue(2).ToString()!.Trim() : string.Empty;
                                            dynamic subvalue = reader.GetValue(3);
                                            bool IsSubMedicalTestCategory = (subvalue == null ? true : subvalue is bool ? reader.GetBoolean(3) : true);
                                            string ParentCategory = reader.GetValue(4) != null ? reader.GetValue(4).ToString()!.Trim() : string.Empty;
                                            TestName = reader.GetValue(5) != null ? reader.GetValue(5).ToString()!.Trim() : string.Empty;
                                            TestDisplayAs = reader.GetValue(6) != null ? reader.GetValue(6).ToString()!.Trim() : string.Empty;
                                            string TestCode = reader.GetValue(7) != null ? reader.GetValue(7).ToString()!.Trim() : string.Empty;
                                            string TestShortName = reader.GetValue(8) != null ? reader.GetValue(8).ToString()!.Trim() : string.Empty;
                                            TestDescription = reader.GetValue(9) != null ? reader.GetValue(9).ToString()!.Trim() : string.Empty;
                                            string SampleRequirement = reader.GetValue(10) != null ? reader.GetValue(10).ToString()!.Trim() : string.Empty;
                                            dynamic value = reader.GetValue(11);
                                            bool IsActive = (value == null ? true : value is bool ? reader.GetBoolean(11) : true);
                                            Reason = reader.GetValue(12) != null ? reader.GetValue(12).ToString()!.Trim() : string.Empty;
                                            bool HasElement = reader.GetValue(13) != null ? (reader.GetValue(13) is bool ? reader.GetBoolean(13) : true) : true;
                                            var KeywordsInString = reader.GetValue(14) != null ? reader.GetValue(14).ToString()!.Trim() : string.Empty;
                                            string ElementCode = reader.GetValue(15) != null ? reader.GetValue(15).ToString()!.Trim() : string.Empty;
                                            string ElementName = reader.GetValue(16) != null ? reader.GetValue(16).ToString()!.Trim() : string.Empty;
                                            string ElementShortName = reader.GetValue(17) != null ? reader.GetValue(17).ToString()!.Trim() : string.Empty;
                                            string Class = reader.GetValue(18) != null ? reader.GetValue(18).ToString()!.Trim() : string.Empty;
                                            string SubClass = reader.GetValue(19) != null ? reader.GetValue(19).ToString()!.Trim() : string.Empty;

                                            string UomName = "";
                                            UomName = reader.GetValue(20) != null ? reader.GetValue(20).ToString()!.Trim() : string.Empty;

                                            string UomDescription = "";
                                            UomDescription = reader.GetValue(21) != null ? reader.GetValue(21).ToString()!.Trim() : string.Empty;
                                            string RangeFrom = reader.GetValue(22) != null ? reader.GetValue(22).ToString()!.Trim() : string.Empty;
                                            string RangeTo = reader.GetValue(23) != null ? reader.GetValue(23).ToString()!.Trim() : string.Empty;
                                            string SingleValue = reader.GetValue(24) != null ? reader.GetValue(24).ToString()!.Trim() : string.Empty;
                                            string TimeToResult = reader.GetValue(25) != null ? reader.GetValue(25).ToString()!.Trim() : string.Empty;
                                            string ElementDescription = reader.GetValue(26) != null ? reader.GetValue(26).ToString()!.Trim() : string.Empty;

                                            if (CategoryName != string.Empty)
                                            {
                                                MedicalTestCategory lMedicalTestCategoryByName = MedicalTestManager.Instance.GetMedicalTestCategoryByName(CategoryName, Global.Company.CompanyId);
                                                if (lMedicalTestCategoryByName != null)
                                                {
                                                    CategoryId = lMedicalTestCategoryByName.Id;
                                                }
                                                else
                                                {
                                                    MedicalTestCategory lMedicalTestCategory = new MedicalTestCategory();
                                                    lMedicalTestCategory.Id = 0L;
                                                    lMedicalTestCategory.Name = CategoryName;
                                                    lMedicalTestCategory.DisplayAs = CategoryDisplayAs.ToString();
                                                    lMedicalTestCategory.Description = CategoryDescription.ToString();
                                                    lMedicalTestCategory.CompanyId = Global.Company.CompanyId;
                                                    lMedicalTestCategory.IsSubMedicalTestCategory = IsSubMedicalTestCategory;
                                                    if (IsSubMedicalTestCategory && !string.IsNullOrEmpty(ParentCategory))
                                                    {
                                                        MedicalTestCategory lParentCategoryByName = MedicalTestManager.Instance.GetMedicalTestCategoryByName(ParentCategory, Global.Company.CompanyId);
                                                        if (lParentCategoryByName != null)
                                                        {
                                                            MedicalTestManager.Instance.UpdateMedicalTestCategory(lParentCategoryByName);
                                                            lMedicalTestCategory.ParentMedicalTestCategoryId = lParentCategoryByName.Id;
                                                        }
                                                        else
                                                        {
                                                            MedicalTestCategory ParentMedicalTestCategory = new MedicalTestCategory();
                                                            {
                                                                ParentMedicalTestCategory.Name = ParentCategory;
                                                                ParentMedicalTestCategory.CompanyId = Global.Company.CompanyId;
                                                            }
                                                            lMedicalTestCategory.ParentMedicalTestCategory = ParentMedicalTestCategory;
                                                        }
                                                    }
                                                    MedicalTestCategory lMedicalTestCategoryFromDB = null!;
                                                    lMedicalTestCategoryFromDB = MedicalTestManager.Instance.AddMedicalTestCategory(lMedicalTestCategory);
                                                    CategoryId = lMedicalTestCategoryFromDB.Id;
                                                }
                                            }
                                            else
                                            {
                                                CategoryName = "General";
                                                MedicalTestCategory MedicalTestCategoryByName = MedicalTestManager.Instance.GetMedicalTestCategoryByName(CategoryName, Global.Company.CompanyId);
                                                if (MedicalTestCategoryByName != null)
                                                {
                                                    CategoryId = MedicalTestCategoryByName.Id;
                                                }
                                                else
                                                {
                                                    MedicalTestCategory lMedicalTestCategory = new MedicalTestCategory();
                                                    lMedicalTestCategory.Name = CategoryName;
                                                    lMedicalTestCategory.Description = "";
                                                    lMedicalTestCategory.DisplayAs = CategoryName;
                                                    lMedicalTestCategory.IsSubMedicalTestCategory = IsSubMedicalTestCategory;
                                                    lMedicalTestCategory.CompanyId = Global.Company.CompanyId;

                                                    MedicalTestCategory lMedicalTestCategoryFromDB = null!;
                                                    lMedicalTestCategoryFromDB = MedicalTestManager.Instance.AddMedicalTestCategory(lMedicalTestCategory);
                                                    CategoryId = lMedicalTestCategoryFromDB.Id;
                                                }
                                            }
                                            if (TestName != null && !string.IsNullOrEmpty(TestName))
                                            {
                                                MedicalTest MTest = new MedicalTest();
                                                {
                                                    MTest.Id = 0L;
                                                    MTest.CompanyId = Global.Company.CompanyId;
                                                    MTest.Name = TestName;
                                                    MTest.DisplayAs = TestDisplayAs;
                                                    MTest.TestCode = TestCode;
                                                    MTest.TestShortName = TestShortName;
                                                    MTest.Description = TestDescription;
                                                    MTest.SampleRequirement = SampleRequirement;
                                                    MTest.Reason = Reason;
                                                    if (IsActive)
                                                    {
                                                        MTest.IsActive = IsActive;
                                                        MTest.Reason = "";
                                                    }
                                                    else
                                                    {
                                                        MTest.IsActive = string.IsNullOrEmpty(Reason) ? true : false;
                                                    }
                                                    MTest.HasElement = HasElement;
                                                    MTest.MedicalTestCategoryId = CategoryId;
                                                }
                                                if (KeywordsInString != null)
                                                {
                                                    string[] KeywordsArray = KeywordsInString.ToString()!.Trim().Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
                                                    foreach (string keyword in KeywordsArray)
                                                    {
                                                        if (string.IsNullOrEmpty(keyword))
                                                        {
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            MedicalTestKeyword keyWord = new MedicalTestKeyword
                                                            {
                                                                Text = keyword.Trim(),
                                                                CompanyId = Global.Company.CompanyId
                                                            };
                                                            if (MTest.Keywords == null)
                                                            {
                                                                MTest.Keywords = new List<MedicalTestKeyword>();
                                                            }
                                                            MTest.Keywords.Add(keyWord);
                                                        }
                                                    }
                                                }
                                                MedicalTest MTById = null!;
                                                if (MedicalTestManager.Instance.MedicalTestNameUniqueById(MTest))
                                                {
                                                    MTById = MedicalTestManager.Instance.AddMedicalTest(MTest);
                                                }
                                                else
                                                {
                                                    MTById = MedicalTestManager.Instance.GetMedicalTestByName(MTest);
                                                    if (MTById != null)
                                                    {
                                                        MTest.Id = MTById.Id;
                                                        MedicalTestManager.Instance.UpdateMedicalTestFromUpload(MTest);
                                                    }
                                                }
                                                MedicalTestUOM MTUById = null!;
                                                if (!string.IsNullOrEmpty(UomName))
                                                {
                                                    MedicalTestUOM UOM = MedicalTestManager.Instance.GetMedicalTestUOMByName(UomName, Global.Company.CompanyId);
                                                    if (UOM == null)
                                                    {
                                                        MedicalTestUOM MTU = new MedicalTestUOM
                                                        {
                                                            Id = 0L,
                                                            CompanyId = Global.Company.CompanyId,
                                                            Name = UomName,
                                                            Discription = UomDescription.ToString()
                                                        };
                                                        MTUById = MedicalTestManager.Instance.AddMedicalTestUom(MTU);
                                                        UomId = MTUById.Id;
                                                    }
                                                    else
                                                    {
                                                        UomId = UOM.Id;
                                                    }
                                                }
                                                if (HasElement && ElementName != null && !string.IsNullOrEmpty(ElementName))
                                                {
                                                    MedicalTestElement MTE = new MedicalTestElement
                                                    {
                                                        Id = 0L,
                                                        MedicalTestId = MTById!.Id,
                                                        CompanyId = Global.Company.CompanyId,
                                                        ElementCode = ElementCode,
                                                        Name = ElementName.ToString(),
                                                        ElementShortName = ElementShortName,
                                                        Class = Class,
                                                        SubClass = SubClass,
                                                        UomId = !string.IsNullOrEmpty(UomName) ? UomId : null,
                                                        RangeFrom = RangeFrom,
                                                        RangeTo = RangeTo,
                                                        SingleValue = SingleValue,
                                                        ResultDuration = TimeToResult,
                                                        Description = ElementDescription
                                                    };
                                                    if (MedicalTestManager.Instance.MedicalTestElementNameUniqueById(MTE))
                                                    {
                                                        MedicalTestElement MTEById = MedicalTestManager.Instance.AddMedicalTestElement(MTE);
                                                    }
                                                    else
                                                    {
                                                        MedicalTestElement MTEById = MedicalTestManager.Instance.GetMedicalTestElementByName(MTE);
                                                        if (MTEById != null)
                                                        {
                                                            MTE.Id = MTEById.Id;
                                                            MedicalTestManager.Instance.UpdateMedicalTestElementFromUpload(MTE);
                                                        }
                                                    }
                                                }
                                                if ((IsActive && Reason != "") || (!IsActive && Reason == ""))
                                                {
                                                    if (HasHeader)
                                                    {
                                                        FileUploadMedicalTest.AddAccessLog("Processing Row No : " + (row - 1) + " - " + "Please check the Active and Reason Column");
                                                        FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        row++;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        FileUploadMedicalTest.AddAccessLog("Processing Row No : " + row + " - " + "Please check the Active and Reason Column");
                                                        FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));                                         //FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.ToString());
                                                        row++;
                                                        continue;
                                                    }
                                                }
                                                if (HasHeader)
                                                {
                                                    FileUploadMedicalTest.AddAccessLog("Processing Row No : " + (row - 1) + " - " + TestName);
                                                    FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                }
                                                else
                                                {
                                                    FileUploadMedicalTest.AddAccessLog("Processing Row No : " + row + " - " + TestName);
                                                    FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));                                         //FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (HasHeader)
                                                {
                                                    FileUploadMedicalTest.AddErrorLog("Row No - " + (row - 1) + " Empty Name");
                                                    FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                }
                                                else
                                                {
                                                    FileUploadMedicalTest.AddErrorLog("Row No - " + row + " Empty Name");
                                                    FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            FileUploadMedicalTest.AddAccessLog(FileUploadMedicalTest.FileUploadMismatchColoumn);
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        FileUploadComplete = false;
                                        IsBreak = true;
                                        FileUploadMedicalTest.AddAccessLog(FileUploadMedicalTest.FileUploadMismatchColoumn);
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.HResult.ToString() == "-2146233080")
                                    {
                                        FileUploadComplete = false;
                                        FileUploadMedicalTest.AddAccessLog("Cancelled");
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;//from the Form/Window instance
                                        throw new IndexOutOfRangeException("-2146233080", ex);
                                    }
                                    else
                                    {
                                        if (HasHeader)
                                        {
                                            FileUploadMedicalTest.AddErrorLog("Row No : " + (row - 1) + " - " + TestName + " : " + ex.InnerException!.Message);
                                            FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                        else
                                        {
                                            FileUploadMedicalTest.AddErrorLog("Row No : " + row + " - " + TestName + " : " + ex.InnerException!.Message);
                                            FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                    }
                                }
                                row++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    } while (reader.NextResult());
                    if (IsBreak == false) { FileUploadMedicalTest.CompleteIncrementProgress(row); } else { FileUploadMedicalTest.CompleteIncrementProgress(0); }
                    FileUploadMedicalTest.CompleteIncrementProgress(row);
                    FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                    stopw.Stop();
                    if (FileUploadMedicalTest.StopProcessing == true)
                    {
                        FileUploadMedicalTest.AddAccessLog("Cancelled");
                    }
                    else
                    {
                        FileUploadMedicalTest.AddAccessLog("Finished");
                    }
                    this.UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private bool IsAnyMedicalTestLinkedToConsultation(MedicalTest medicalTest)
        {
            var consultedTest = ConsultationNoteManager.Instance.GetConsultedLabTestsByMedicalTestId(medicalTest.Id);
            if (consultedTest == null)
            {
                return true;
            }
            return false;
        }
        private void ExitFileUploadProcess()
        {
            if (FileUploadMedicalTest.WorkFlow == true)
            {
                FileUploadMedicalTest.EnableExitButton(false);
                PauseProcessing = true;
            }
            else
            {
                this.Close();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F10))
            {
                ExitFileUploadProcess();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FileUploadMedicalTest_OnClickExit(object sender, EventArgs e)
        {
            ExitFileUploadProcess();
        }

        private void MedicalTestFileUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FileUploadMedicalTest.WorkFlow == true)
            {
                FileUploadMedicalTest.ExitFileUpload();
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void FileUploadMedicalTest_ShowDialog(object sender, EventArgs e)
        {
            if (FileUploadMedicalTest.RefreshAllData())
            {
                DialogResult confirmation = MessageBox.Show(string.Format(RefreshMedicalTestConfirmText), "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (confirmation == DialogResult.No)
                {
                    FileUploadMedicalTest.ChangeCheckeState();
                }
            }
        }
    }
}
