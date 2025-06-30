using ExcelDataReader;
using fa.model.Hms.Master;
using Fa.api.Hms;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WinFormsMessage = System.Windows.Forms.Message;


namespace fa.views.hms.masters.upload
{
    public partial class ProcedureFileUpload : Form
    {
        public bool FileUploadComplete;
        public bool PauseProcessing;
        public bool IsBreak = false;

        public ProcedureFileUpload()
        {
            InitializeComponent();
        }
        private void fileUpload1_OnClickUpload_1(object sender, EventArgs e)
        {
            string FileName = fileUpload1.FileName();
            bool HasHeader = fileUpload1.HasHeader();
            bool feildColumns = false;
            bool IsColumnValid = true;
            string[] MedicalProcedureHeader = new string[] { "MedicalProcedureCategory", "CategoryDisplayName", "CategoryDescription", "IsSubMedicalProcedureCategory", "ParentCategoryId", "ProcedureCode", "Name", "DisplayName", "Description", "Is Active", "Reason For InActive", "Fee", "Keywords" };
            Stopwatch stopw = new Stopwatch();
            using (var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    FileUploadComplete = true;
                    int rowcount = reader.RowCount;
                    stopw.Start();
                    this.UseWaitCursor = true;
                    Cursor.Current = Cursors.WaitCursor;
                    fileUpload1.SetProgressMax(rowcount);
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

                            bool CheckedHeader = MedicalProcedureHeader.SequenceEqual(firstRow, StringComparer.OrdinalIgnoreCase);
                            if (rowno == 1)
                            {

                                if (HasHeader)
                                {
                                    if (!CheckedHeader)
                                    {
                                        FileUploadComplete = false;
                                        IsBreak = true;
                                        fileUpload1.AddAccessLog(fileUpload1.FileUploadNoHeader);
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
                                    fileUpload1.AddAccessLog(fileUpload1.FileUploadHasHeader);
                                    Cursor.Current = Cursors.Default;
                                    this.UseWaitCursor = false;
                                    return;
                                }
                            }
                            rowno++;
                            if (fileUpload1.PauseWaitCursor == true)
                            {
                                this.UseWaitCursor = false;
                            }
                            else
                            {
                                this.UseWaitCursor = true;
                            }
                            if (PauseProcessing == true)
                            {
                                this.UseWaitCursor = false;
                                fileUpload1.AddAccessLog(fileUpload1.FileUploaPaused);
                                string message = "Do you want to exit from FileUpload Processing?";
                                string title = "Exit FileUpload";
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    fileUpload1.StopProcessing = true;
                                    this.Invoke((new Action(() => this.Close())));
                                }
                                else
                                {
                                    this.UseWaitCursor = true;
                                    fileUpload1.AddAccessLog(fileUpload1.FileUploaContinuing);
                                    PauseProcessing = false;
                                    fileUpload1.StopProcessing = false;
                                    fileUpload1.EnableExitButton(true);
                                    continue;
                                }
                            }

                            if (fileUpload1.StopProcessing == false)
                            {

                                string CategoryName = "";
                                string Name = "";
                                string PCode = "";
                                long CategoryId = 0L;
                                int fc = 0;
                                try
                                {
                                    if (MedicalProcedureHeader.Length == reader.FieldCount)
                                    {
                                        feildColumns = true;
                                        if (row == 1 && HasHeader)
                                        {
                                            foreach (string sh in MedicalProcedureHeader)
                                            {
                                                if (sh != reader.GetValue(fc).ToString())
                                                {
                                                    IsColumnValid = false;
                                                }
                                                fc++;
                                            }
                                            row++;
                                            continue;
                                        }
                                        if (IsColumnValid)
                                        {
                                            fileUpload1.IncrementProgress();
                                            CategoryName = reader.GetValue(0) != null ? reader.GetValue(0).ToString()!.Trim() : string.Empty;
                                            if (CategoryName.Length > 30)
                                            {
                                                CategoryName = CategoryName.Substring(0, 30);
                                            }
                                            string CategoryDisplayName = reader.GetString(1) != null ? reader.GetString(1) : string.Empty;
                                            string CategoryDescription = reader.GetString(2) != null ? reader.GetString(2) : string.Empty;
                                            dynamic subvalue = reader.GetValue(3);
                                            bool IsSubMedicalProcedureCategory = (subvalue == null ? true : subvalue is bool ? reader.GetBoolean(3) : true);
                                            object pcvalue = reader.GetValue(5);
                                            PCode = pcvalue?.ToString() ?? "";

                                            Name = reader.GetValue(6)?.ToString()?.Trim() ?? "";

                                            string DisplayName = reader.GetValue(7)?.ToString() ?? "";

                                            string Description = reader.GetValue(8)?.ToString() ?? "";

                                            dynamic value = reader.GetValue(9);
                                            bool IsActive = value == null ? true : value is bool ? (bool)value
                                                    : value is string stringValue ? stringValue.Equals("true", StringComparison.OrdinalIgnoreCase) || stringValue.Equals("yes", StringComparison.OrdinalIgnoreCase) : true;
                                            string Reason = reader.GetValue(10) != null && !IsActive ? reader.GetValue(10).ToString()! : string.Empty;
                                            value = reader.GetValue(11);
                                            double Fee = (value == null ? 0 : value is double ? reader.GetDouble(11) : 0); // Double.TryParse(value, out retNum);
                                            string KeywordsInString = string.Empty;
                                            if (reader.FieldCount > 12)
                                            {
                                                KeywordsInString = reader.GetValue(12) != null ? reader.GetValue(12).ToString()! : string.Empty;
                                            }

                                            if (CategoryName != string.Empty)
                                            {
                                                MedicalProcedureCategory lMedicalProcedureCategoryByName = MedicalProcedureManager.Instance.GetMedicalProcedureCategoryByName(CategoryName, Global.Company.CompanyId);
                                                if (lMedicalProcedureCategoryByName != null)
                                                {
                                                    CategoryId = lMedicalProcedureCategoryByName.Id;
                                                }
                                                else
                                                {
                                                    MedicalProcedureCategory lMedicalProcedureCategory = new MedicalProcedureCategory();
                                                    lMedicalProcedureCategory.Id = 0L;
                                                    lMedicalProcedureCategory.Name = CategoryName;
                                                    lMedicalProcedureCategory.Description = CategoryDescription;
                                                    lMedicalProcedureCategory.DisplayAs = CategoryDisplayName;
                                                    lMedicalProcedureCategory.CompanyId = Global.Company.CompanyId;
                                                    lMedicalProcedureCategory.IsSubMedicalProcedureCategory = false;

                                                    MedicalProcedureCategory lMedicalProcedureCategoryFromDB = null!;
                                                    lMedicalProcedureCategoryFromDB = MedicalProcedureManager.Instance.AddMedicalProcedureCategory(lMedicalProcedureCategory);

                                                    CategoryId = lMedicalProcedureCategoryFromDB.Id;

                                                }
                                            }
                                            else
                                            {
                                                CategoryName = "General";
                                                MedicalProcedureCategory MedicalProcedureCategoryByName = MedicalProcedureManager.Instance.GetMedicalProcedureCategoryByName(CategoryName, Global.Company.CompanyId);
                                                if (MedicalProcedureCategoryByName != null)
                                                {
                                                    CategoryId = MedicalProcedureCategoryByName.Id;
                                                }
                                                else
                                                {
                                                    MedicalProcedureCategory lMedicalProcedureCategory = new MedicalProcedureCategory();
                                                    lMedicalProcedureCategory.Name = CategoryName;
                                                    lMedicalProcedureCategory.Description = "";
                                                    lMedicalProcedureCategory.DisplayAs = CategoryName;
                                                    lMedicalProcedureCategory.IsSubMedicalProcedureCategory = false;
                                                    lMedicalProcedureCategory.CompanyId = Global.Company.CompanyId;

                                                    MedicalProcedureCategory lMedicalProcedureCategoryFromDB = null!;
                                                    lMedicalProcedureCategoryFromDB = MedicalProcedureManager.Instance.AddMedicalProcedureCategory(lMedicalProcedureCategory);
                                                    CategoryId = lMedicalProcedureCategoryFromDB.Id;
                                                }
                                            }

                                            if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(PCode))
                                            {
                                                long MPId = 0L;
                                                MedicalProcedure GMPById = MedicalProcedureManager.Instance.GetMedicalProcedureByCodeName(Name, PCode, Global.Company.CompanyId);
                                                if (GMPById != null)
                                                {
                                                    MPId = GMPById.Id;
                                                }

                                                MedicalProcedure MP = new model.Hms.Master.MedicalProcedure();
                                                {
                                                    MP.Id = MPId;
                                                    MP.CompanyId = Global.Company.CompanyId;
                                                    MP.ProcedureCode = PCode;
                                                    MP.Name = Name;
                                                    MP.Description = Description;
                                                    MP.DisplayAs = DisplayName;
                                                    MP.Reason = Reason;
                                                    if (IsActive)
                                                    {
                                                        MP.IsActive = IsActive;
                                                        MP.Reason = "";
                                                    }
                                                    else
                                                    {
                                                        MP.IsActive = string.IsNullOrEmpty(Reason) ? true : false;
                                                    }
                                                    MP.Fee = Fee;
                                                    MP.MedicalProcedureCategoryId = CategoryId;
                                                };

                                                if (!string.IsNullOrEmpty(KeywordsInString))
                                                {
                                                    string[] KeywordsArray = KeywordsInString
                                                        .Split(',')
                                                        .Select(k => k.Trim())
                                                        .Where(k => !string.IsNullOrEmpty(k))
                                                        .Distinct()
                                                        .ToArray();

                                                    if (MP.Keywords == null)
                                                    {
                                                        MP.Keywords = new List<MedicalProcedureKeyword>();
                                                    }

                                                    HashSet<string> existingKeywords = new HashSet<string>(
                                                        MP.Keywords.Select(k => k.Text), StringComparer.OrdinalIgnoreCase);

                                                    foreach (string keyword in KeywordsArray)
                                                    {
                                                        if (!existingKeywords.Contains(keyword))
                                                        {
                                                            MP.Keywords.Add(new MedicalProcedureKeyword
                                                            {
                                                                Text = keyword,
                                                                CompanyId = Global.Company.CompanyId,
                                                                MedicalProcedure = MP
                                                            });

                                                            existingKeywords.Add(keyword);
                                                        }
                                                    }
                                                }

                                                MedicalProcedure MPById = null!;
                                                if (MedicalProcedureManager.Instance.MedicalProcedureCodeNameUniqueById(MP))
                                                {
                                                    if (MP.Id == 0)
                                                    {
                                                        MPById = MedicalProcedureManager.Instance.AddMedicalProcedure(MP);
                                                    }
                                                    else
                                                    {
                                                        MPById = MedicalProcedureManager.Instance.UpdateAndSaveProcedure(MP);
                                                    }
                                                    if ((IsActive && Reason != "") || (!IsActive && Reason == ""))
                                                    {
                                                        if (HasHeader)
                                                        {
                                                            fileUpload1.AddAccessLog("Processing Row No : " + (row - 1) + " - " + "Please check the Active and Reason Column");
                                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        else
                                                        {
                                                            fileUpload1.AddAccessLog("Processing Row No : " + row + " - " + "Please check the Active and Reason Column");
                                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        row++;
                                                        continue;
                                                    }
                                                    if (HasHeader)
                                                    {
                                                        fileUpload1.AddAccessLog("Processing Row No : " + (row - 1) + " - " + Name);
                                                        fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else
                                                    {
                                                        fileUpload1.AddAccessLog("Processing Row No : " + row + " - " + Name);
                                                        fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                                else
                                                {
                                                    MedicalProcedure MedicalProcedures = MedicalProcedureManager.Instance.GetMedicalProcedureByName(MP.Name, Global.Company.CompanyId);
                                                    if (MedicalProcedures != null)
                                                    {
                                                        if (HasHeader)
                                                        {
                                                            fileUpload1.AddErrorLog("Row No - " + (row - 1) + " Name : " + Name + " already Exists ");
                                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        else
                                                        {
                                                            fileUpload1.AddErrorLog("Row No - " + row + " Name : " + Name + " already Exists ");
                                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (HasHeader)
                                                        {
                                                            fileUpload1.AddErrorLog("Row No - " + (row - 1) + " Procedure Code : " + PCode + " already Exists ");
                                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                        }
                                                        else
                                                        {
                                                            fileUpload1.AddErrorLog("Row No - " + row + " Procedure Code : " + PCode + " already Exists ");
                                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
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
                                                        fileUpload1.AddErrorLog("Row No - " + (row - 1) + " Empty Name");
                                                        fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else if (string.IsNullOrWhiteSpace(PCode))
                                                    {
                                                        fileUpload1.AddErrorLog("Row No - " + (row - 1) + " Empty Code");
                                                        fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrWhiteSpace(Name))
                                                    {
                                                        fileUpload1.AddErrorLog("Row No - " + row + " Empty Name");
                                                        fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else if (string.IsNullOrWhiteSpace(PCode))
                                                    {
                                                        fileUpload1.AddErrorLog("Row No - " + row + " Empty Code");
                                                        fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            fileUpload1.AddAccessLog(fileUpload1.FileUploadMismatchColoumn);
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (feildColumns == false)
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            fileUpload1.AddAccessLog(fileUpload1.FileUploadMismatchNofColoumn);
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.HResult.ToString() == "-2146233080")
                                    {
                                        FileUploadComplete = false;
                                        fileUpload1.AddAccessLog("Cancelled");
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;//from the Form/Window instance
                                        throw new IndexOutOfRangeException("-2146233080", ex);
                                    }
                                    else
                                    {
                                        if (HasHeader)
                                        {
                                            if (ex.InnerException != null)
                                            {
                                                fileUpload1.AddErrorLog("Row No : " + (row - 1) + " - " + Name + " : " + ex.InnerException.Message);
                                            }
                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                        else
                                        {
                                            fileUpload1.AddErrorLog("Row No : " + row + " - " + Name + " : " + ex.InnerException!.Message);
                                            fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
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
                    if (IsBreak == false) { fileUpload1.CompleteIncrementProgress(row); } else { fileUpload1.CompleteIncrementProgress(0); }
                    fileUpload1.CompleteIncrementProgress(row);
                    fileUpload1.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                    stopw.Stop();
                    if (fileUpload1.StopProcessing == true)
                    {
                        fileUpload1.AddAccessLog("Cancelled");
                    }
                    else
                    {
                        fileUpload1.CompleteIncrementProgress(row);
                        fileUpload1.AddAccessLog("Finished");
                    }
                    Cursor.Current = Cursors.Default;
                    this.UseWaitCursor = false;
                }
            }
        }
        private void ExitFileUploadProcess()
        {
            if (fileUpload1.WorkFlow == true)
            {
                fileUpload1.EnableExitButton(false);
                PauseProcessing = true;
            }
            else
            {
                this.Close();
            }
        }
        private void fileUpload1_OnClickExit(object sender, EventArgs e)
        {
            ExitFileUploadProcess();
        }
        protected override bool ProcessCmdKey(ref WinFormsMessage msg, Keys keyData)
        {
            if (keyData == (Keys.F10))
            {
                ExitFileUploadProcess();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ProcedureFileUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileUpload1.WorkFlow == true)
            {
                fileUpload1.ExitFileUpload();
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
