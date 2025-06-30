using ClosedXML;
using ExcelDataReader;
using fa.model.Hms.Master;
using Fa.api.Hms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using VisioForge.MediaFramework.Helpers;

namespace fa.views.hms.masters.upload
{
    public partial class DiagonosisFileUpload : Form
    {
        public bool FileUploadComplete;
        public bool PauseProcessing;
        public bool IsBreak = false;

        public DiagonosisFileUpload()
        {
            InitializeComponent();
        }
        private void ExitFileUploadProcess()
        {
            if (FileUploadDiagonosis.WorkFlow == true)
            {
                FileUploadDiagonosis.EnableExitButton(false);
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
        private void FileUploadDiagonosis_OnClickExit(object sender, EventArgs e)
        {
            ExitFileUploadProcess();
        }
        private void FileUploadDiagonosis_OnClickUpload(object sender, EventArgs e)
        {
            string FileName = FileUploadDiagonosis.FileName();
            bool HasHeader = FileUploadDiagonosis.HasHeader();
            bool IsColumnValid = true;
            string[] SymptomHeader = new string[] { "SymptomCategory", "CategoryDisplayName", "CategoryDescription", "IsSubSymptomCategory", "ParentCategoryId", "SymptomCode", "Name", "DisplayName", "Description", "Is Active", "Reason For InActive", "Keywords" };
            Stopwatch stopw = new Stopwatch();
            IList<Symptom> llSymptom = SymptomsManager.Instance.ListSymptomByCompanyId(Global.Company.CompanyId);
            using (var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    FileUploadComplete = true;
                    int rowcount = reader.RowCount;
                    stopw.Start();
                    FileUploadDiagonosis.SetProgressMax(rowcount);
                    int row = 1;
                    int rowno = 1;
                    do
                    {
                        while (reader.Read())
                        {
                            List<string> firstRow = new List<string>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                firstRow.Add(reader.GetValue(i)?.ToString() ?? "");
                            }

                            bool CheckedHeader = SymptomHeader.SequenceEqual(firstRow, StringComparer.OrdinalIgnoreCase);
                            if (rowno == 1)
                            {

                                if (HasHeader)
                                {
                                    if (!CheckedHeader)
                                    {
                                        FileUploadComplete = false;
                                        IsBreak = true;
                                        FileUploadDiagonosis.AddAccessLog(FileUploadDiagonosis.FileUploadNoHeader);
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;
                                        return;
                                    }
                                    Console.WriteLine("Header validation passed. Continuing processing...");
                                }
                                else if (CheckedHeader)
                                {
                                    FileUploadComplete = false;
                                    IsBreak = true;
                                    FileUploadDiagonosis.AddAccessLog(FileUploadDiagonosis.FileUploadHasHeader);
                                    Cursor.Current = Cursors.Default;
                                    this.UseWaitCursor = false;
                                    return;
                                }
                            }
                            rowno++;
                            if (PauseProcessing == true)
                            {
                                this.UseWaitCursor = false;
                                FileUploadDiagonosis.AddAccessLog(FileUploadDiagonosis.FileUploaPaused);
                                string message = "Do you want to exit from FileUpload Processing?";
                                string title = "Exit FileUpload";
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    FileUploadDiagonosis.StopProcessing = true;
                                    this.Invoke((new Action(() => this.Close())));
                                }
                                else
                                {
                                    this.UseWaitCursor = true;
                                    FileUploadDiagonosis.AddAccessLog(FileUploadDiagonosis.FileUploaContinuing);
                                    PauseProcessing = false;
                                    FileUploadDiagonosis.StopProcessing = false;
                                    FileUploadDiagonosis.EnableExitButton(true);
                                    continue;
                                }
                            }
                            if (FileUploadDiagonosis.StopProcessing == false)
                            {
                                // FileUploadDiagonosis.IncrementProgress();
                                string SCode = "";
                                string Name = "";
                                string CategoryName = "";
                                string DisplayName = "";
                                string Description = "";
                                string Reason = "";
                                string CategoryDisplayName = "";
                                string CategoryDescription = "";
                                long CategoryId = 0L;
                                int fc = 0;
                                try
                                {
                                    if (SymptomHeader.Length == reader.FieldCount)
                                    {
                                        if (row == 1 && HasHeader)
                                        {
                                            foreach (string sh in SymptomHeader)
                                            {
                                                if (sh != reader.GetValue(fc).ToString())
                                                {
                                                    IsColumnValid = false;
                                                    IsBreak = true;
                                                }
                                                fc++;
                                            }
                                            row++;
                                            continue;
                                        }
                                        if (IsColumnValid)
                                        {
                                            FileUploadDiagonosis.IncrementProgress();
                                            CategoryName = reader.GetValue(0) != null ? reader.GetValue(0).ToString()!.Trim() : string.Empty;
                                            if (CategoryName.Length > 30)
                                            {
                                                CategoryName = CategoryName.Substring(0, 30);
                                            }
                                            CategoryDisplayName = reader.GetValue(1) != null ? reader.GetValue(1).ToString()!.Trim() : string.Empty; ;
                                            CategoryDescription = reader.GetValue(2) != null ? reader.GetValue(2).ToString()!.Trim() : string.Empty; ;
                                            dynamic subvalue = reader.GetValue(3);
                                            bool IsSubCategory = subvalue == null ? true : subvalue is bool ? (bool)subvalue
                                                    : subvalue is string substringValue ? substringValue.Equals("true", StringComparison.OrdinalIgnoreCase) || substringValue.Equals("yes", StringComparison.OrdinalIgnoreCase) : true;
                                            var ParentCategoryId = reader.GetValue(4);

                                            object scvalue = reader.GetValue(5);
                                            SCode = scvalue?.ToString() ?? "";

                                            Name = reader.GetValue(6)?.ToString()?.Trim() ?? "";

                                            DisplayName = reader.GetValue(7)?.ToString() ?? "";

                                            Description = reader.GetValue(8)?.ToString() ?? "";

                                            dynamic value = reader.GetValue(9);
                                            bool IsActive = value == null ? true : value is bool ? (bool)value
                                                    : value is string stringValue ? stringValue.Equals("true", StringComparison.OrdinalIgnoreCase) || stringValue.Equals("yes", StringComparison.OrdinalIgnoreCase) : true;
                                            Reason = reader.GetValue(10) != null ? reader.GetValue(10).ToString()!.Trim() : string.Empty; ;
                                            var KeywordsInString = reader.GetValue(11);

                                            if (CategoryName != string.Empty)
                                            {
                                                SymptomCategory lSymptomCategoryByName = SymptomsManager.Instance.GetSymptomCategoryByName(CategoryName, Global.Company.CompanyId);
                                                if (lSymptomCategoryByName != null)
                                                {
                                                    CategoryId = lSymptomCategoryByName.Id;
                                                }
                                                else
                                                {
                                                    SymptomCategory lSymptomCategory = new SymptomCategory();
                                                    lSymptomCategory.Id = 0L;
                                                    lSymptomCategory.Name = CategoryName;
                                                    lSymptomCategory.Discription = CategoryDescription.ToString();
                                                    lSymptomCategory.DisplayAs = CategoryDisplayName.ToString();
                                                    lSymptomCategory.CompanyId = Global.Company.CompanyId;
                                                    lSymptomCategory.IsSubSymptomCategory = false;
                                                    //lSymptomCategory.ParentSymptomCategoryId = long.Parse(ParentCategoryId.ToString());
                                                    SymptomCategory lSymptomCategoryFromDB = null!;
                                                    lSymptomCategoryFromDB = SymptomsManager.Instance.AddSymptomCategory(lSymptomCategory);

                                                    CategoryId = lSymptomCategoryFromDB.Id;

                                                }
                                            }
                                            else
                                            {
                                                CategoryName = "General";
                                                SymptomCategory SymptomCategoryByName = SymptomsManager.Instance.GetSymptomCategoryByName(CategoryName, Global.Company.CompanyId);
                                                if (SymptomCategoryByName != null)
                                                {
                                                    CategoryId = SymptomCategoryByName.Id;
                                                }
                                                else
                                                {
                                                    SymptomCategory lSymptomCategory = new SymptomCategory();
                                                    lSymptomCategory.Name = CategoryName;
                                                    lSymptomCategory.Discription = "";
                                                    lSymptomCategory.DisplayAs = CategoryName;
                                                    lSymptomCategory.IsSubSymptomCategory = false;
                                                    lSymptomCategory.CompanyId = Global.Company.CompanyId;

                                                    SymptomCategory lSymptomCategoryFromDB = null;
                                                    lSymptomCategoryFromDB = SymptomsManager.Instance.AddSymptomCategory(lSymptomCategory);
                                                    CategoryId = lSymptomCategoryFromDB.Id;
                                                }
                                            }

                                            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(SCode))
                                            {
                                                long SympId = 0L;
                                                Symptom GSympById = SymptomsManager.Instance.GetSymptomByNameCode(Name, SCode, Global.Company.CompanyId);
                                                if (GSympById != null)
                                                {
                                                    SympId = GSympById.Id;
                                                }
                                                Symptom Symptoms = new Symptom();
                                                {
                                                    Symptoms.Id = SympId;
                                                    Symptoms.CompanyId = Global.Company.CompanyId;
                                                    Symptoms.SymptomCode = SCode;
                                                    Symptoms.Name = Name;
                                                    Symptoms.DisplayAs = DisplayName;
                                                    Symptoms.Discription = Description;
                                                    Symptoms.ReasonForInactive = Reason;
                                                    if (IsActive)
                                                    {
                                                        Symptoms.IsActive = IsActive;
                                                        Symptoms.ReasonForInactive = "";
                                                    }
                                                    else
                                                    {
                                                        Symptoms.IsActive = string.IsNullOrEmpty(Reason) ? true : false;
                                                    }
                                                    Symptoms.SymptomCategoryId = CategoryId;
                                                };

                                                Symptoms.Codes = (llSymptom == null || llSymptom.Count == 0 ? 1 : (llSymptom.OrderByDescending(x => x.Id).First().Id + 1)).ToString();

                                                if (KeywordsInString != null)
                                                {
                                                    string[] KeywordsArray = KeywordsInString.ToString()!
                                                        .Split(',')
                                                        .Select(k => k.Trim())
                                                        .Where(k => !string.IsNullOrEmpty(k))
                                                        .Distinct()
                                                        .ToArray();

                                                    if (Symptoms.Keywords == null)
                                                    {
                                                        Symptoms.Keywords = new List<SymptomKeyword>();
                                                    }

                                                    HashSet<string> existingKeywords = new HashSet<string>(
                                                        Symptoms.Keywords.Select(k => k.Text), StringComparer.OrdinalIgnoreCase);

                                                    foreach (string keyword in KeywordsArray)
                                                    {
                                                        if (!existingKeywords.Contains(keyword))
                                                        {
                                                            Symptoms.Keywords.Add(new SymptomKeyword
                                                            {
                                                                Text = keyword,
                                                                CompanyId = Global.Company.CompanyId,
                                                            });

                                                            existingKeywords.Add(keyword);
                                                        }
                                                    }
                                                }

                                                Symptom SymptomById = null!;
                                                if (SymptomsManager.Instance.GetSymptomByNameCodeUniqueById(Symptoms))
                                                {
                                                    if (Symptoms.Id == 0)
                                                    {
                                                        SymptomById = SymptomsManager.Instance.AddSymptom(Symptoms);
                                                    }
                                                    else
                                                    {
                                                        SymptomById = SymptomsManager.Instance.UpdateSymptom(Symptoms);
                                                    }
                                                    if ((IsActive && Reason != "") || (!IsActive && Reason == ""))
                                                    {
                                                        if (HasHeader)
                                                        {
                                                            FileUploadDiagonosis.AddAccessLog("Processing Row No : " + (row - 1) + " - " + "Please check the Active and Reason Column");
                                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));                                         //FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.ToString("00.00.00"));
                                                        }
                                                        else
                                                        {
                                                            FileUploadDiagonosis.AddAccessLog("Processing Row No : " + row + " - " + "Please check the Active and Reason Column");
                                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        row++;
                                                        continue;
                                                    }
                                                    if (HasHeader)
                                                    {
                                                        FileUploadDiagonosis.AddAccessLog("Processing Row No : " + (row - 1) + " - " + Name);
                                                        FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));                                         //FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.ToString("00.00.00"));
                                                    }
                                                    else
                                                    {
                                                        FileUploadDiagonosis.AddAccessLog("Processing Row No : " + row + " - " + Name);
                                                        FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                                else
                                                {
                                                    Symptom lSymptomByName = SymptomsManager.Instance.GetSymptomByName(Symptoms.Name, Global.Company.CompanyId);
                                                    if (lSymptomByName != null)
                                                    {
                                                        if (HasHeader)
                                                        {
                                                            FileUploadDiagonosis.AddErrorLog("Row No - " + (row - 1) + " Name : " + Name + " already Exists ");
                                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        else
                                                        {
                                                            FileUploadDiagonosis.AddErrorLog("Row No - " + row + " Name : " + Name + " already Exists ");
                                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (HasHeader)
                                                        {
                                                            FileUploadDiagonosis.AddErrorLog("Row No - " + (row - 1) + " Symptom Code : " + SCode + " already Exists ");
                                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        else
                                                        {
                                                            FileUploadDiagonosis.AddErrorLog("Row No - " + row + " Symptom Code : " + SCode + " already Exists ");
                                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (HasHeader)
                                                {
                                                    if (string.IsNullOrWhiteSpace(Name))
                                                    {
                                                        FileUploadDiagonosis.AddErrorLog("Row No - " + (row - 1) + " Empty Name");
                                                        FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else if (string.IsNullOrWhiteSpace(SCode))
                                                    {
                                                        FileUploadDiagonosis.AddErrorLog("Row No - " + (row - 1) + " Empty Code");
                                                        FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrWhiteSpace(Name))
                                                    {
                                                        FileUploadDiagonosis.AddErrorLog("Row No - " + row + " Empty Name");
                                                        FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else if (string.IsNullOrWhiteSpace(SCode))
                                                    {
                                                        FileUploadDiagonosis.AddErrorLog("Row No - " + row + " Empty Code");
                                                        FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            FileUploadDiagonosis.AddAccessLog(FileUploadDiagonosis.FileUploadMismatchColoumn);
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            FileUploadDiagonosis.StopProcessing = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        FileUploadComplete = false;
                                        IsBreak = true;
                                        FileUploadDiagonosis.AddAccessLog(FileUploadDiagonosis.FileUploadMismatchColoumn);
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;
                                        FileUploadDiagonosis.StopProcessing = true;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.HResult.ToString() == "-2146233080")
                                    {
                                        FileUploadComplete = false;
                                        FileUploadDiagonosis.AddAccessLog("Cancelled");
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;//from the Form/Window instance
                                        throw new IndexOutOfRangeException("-2146233080", ex);
                                    }
                                    else
                                    {
                                        if (HasHeader)
                                        {
                                            FileUploadDiagonosis.AddErrorLog("Row No : " + (row - 1) + " - " + Name + " : " + ex.InnerException!.Message);
                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                        else
                                        {
                                            FileUploadDiagonosis.AddErrorLog("Row No : " + row + " - " + Name + " : " + ex.InnerException.Message);
                                            FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                    }
                                }
                                row++;
                            }
                            else
                            {
                                break;
                            }
                            this.UseWaitCursor = false;
                        }
                    } while (reader.NextResult());
                    if (IsBreak == false)
                    {

                        FileUploadDiagonosis.CompleteIncrementProgress(row);
                    }
                    else { FileUploadDiagonosis.CompleteIncrementProgress(0); }
                    FileUploadDiagonosis.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                    stopw.Stop();
                    if (FileUploadDiagonosis.StopProcessing == true)
                    {
                        FileUploadDiagonosis.AddAccessLog("Cancelled");
                    }
                    else
                    {
                        FileUploadDiagonosis.CompleteIncrementProgress(row);
                        FileUploadDiagonosis.AddAccessLog("Finished");
                    }
                    this.UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private void DiagonosisFileUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FileUploadDiagonosis.WorkFlow == true)
            {
                FileUploadDiagonosis.ExitFileUpload();
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
