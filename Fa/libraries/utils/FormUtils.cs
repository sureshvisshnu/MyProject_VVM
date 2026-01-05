using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using fa.api.Accounting;
using fa.api.catalog;
using fa.api.Hms;
using fa.api.Log;
using fa.api.OrderManagement;
using fa.api.System;
using fa.api.UserProfile;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Common;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.model.System;
using fa.model.UserProfile;
using fa.views.controls;
using fa.views.controls.ComboTreeView;
using Fa.api.Hms;
using FADataAccessLibrary.Api.Hms;
using FADataAccessLibrary.Api.OrderManagement;
using FADataAccessLibrary.Model.Hms.Master;
using FADataAccessLibrary.Model.Purchase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VisioForge.Libs.NAudio;

namespace fa.libraries.utils
{
    public static class DataGridUtils
    {
        public static float readColumValueAsFloat(DataGridView DataGridItem, int row, int column)
        {
            try
            {
                return float.Parse(DataGridItem.Rows[row].Cells[column].Value.ToString()!);
            }
            catch (Exception e)
            {
                Logger.LogError(new Exception("Could not read data column", e));
                return 0F;
            }
        }
    }
    public class FileUtils
    {
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart!.SharedStringTablePart!;
            if (cell.CellValue == null)
            {
                return "";
            }
            string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart!.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        public static string GetCellDateValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart!.SharedStringTablePart!;
            if (cell.CellValue == null)
            {
                return "";
            }
            var cellValue = cell.CellValue;
            var value = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if (cellValue != null)
            {
                double d = double.Parse(cellValue.InnerText);
                DateTime conv = DateTime.FromOADate(d);
                return conv.ToString(Global.Company.DateFormat);
            }
            else
            {
                return "";
            }
        }
        public static int? GetColumnIndexFromName(string columnName)
        {
            string name = columnName;
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }
            return number;
        }
        public static string GetColumnName(string cellReference)
        {
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
        public static string GetWriteFileName(string FileNames, string Title, string ProposedFileName)
        {
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.Filter = FileNames;
            SaveFileDialog1.Title = Title;
            SaveFileDialog1.FileName = ProposedFileName;
            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(SaveFileDialog1.FileName))
                {
                    return SaveFileDialog1.FileName;
                }
            }
            return null!;
        }
        public static bool DeleteFileIfExist(string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                    return true;
                }
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
            return false;
        }
    }
    public static class ComboUtils
    {
        public static string GetExtension(FileType FileType)
        {
            if (FileType == FileType.DOC)
            {
                return ".DOC";
            }
            else if (FileType == FileType.DOCX)
            {
                return ".DOCX";
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
            else if (FileType == FileType.GIF)
            {
                return ".GIF";
            }
            else if (FileType == FileType.JFIF)
            {
                return ".JFIF";
            }
            else if (FileType == FileType.BMP)
            {
                return ".BMP";
            }
            else if (FileType == FileType.PCX)
            {
                return ".PCX";
            }
            else if (FileType == FileType.TGA)
            {
                return ".TGA";
            }
            else if (FileType == FileType.CR2)
            {
                return ".CR2";
            }
            else if (FileType == FileType.NEF)
            {
                return ".NEF";
            }
            else if (FileType == FileType.ORF)
            {
                return ".ORF";
            }
            else if (FileType == FileType.SR2)
            {
                return ".SR2";
            }
            else if (FileType == FileType.DWG)
            {
                return ".DWG";
            }
            else if (FileType == FileType.DXF)
            {
                return ".DXF";
            }
            else if (FileType == FileType.TPL)
            {
                return ".TPL";
            }
            else if (FileType == FileType.CVX)
            {
                return ".CVX";
            }
            else if (FileType == FileType.CNV)
            {
                return ".CNV";
            }
            else if (FileType == FileType.CVI)
            {
                return ".CVI";
            }
            else if (FileType == FileType.PSD)
            {
                return ".PSD";
            }
            else if (FileType == FileType.TIFF)
            {
                return ".TIFF";
            }
            else if (FileType == FileType.EPS)
            {
                return ".EPS";
            }
            else if (FileType == FileType.AI)
            {
                return ".AI";
            }
            else if (FileType == FileType.INDD)
            {
                return ".INDD";
            }
            else if (FileType == FileType.RAW)
            {
                return ".RAW";
            }
            return ".RAW";
        }
        public static FileType GetFileType(string Extension)
        {
            if (Extension.ToUpper() == ".DOC")
            {
                return FileType.DOC;
            }
            else if (Extension.ToUpper() == ".DOCX")
            {
                return FileType.DOCX;
            }
            else if (Extension.ToUpper() == ".PDF")
            {
                return FileType.PDF;
            }
            else if (Extension.ToUpper() == ".JPEG")
            {
                return FileType.JPEG;
            }
            else if (Extension.ToUpper() == ".JPG")
            {
                return FileType.JPG;
            }
            else if (Extension.ToUpper() == ".PNG")
            {
                return FileType.PNG;
            }
            else if (Extension.ToUpper() == ".GIF")
            {
                return FileType.GIF;
            }
            else if (Extension.ToUpper() == ".JFIF")
            {
                return FileType.JFIF;
            }
            else if (Extension.ToUpper() == ".BMP")
            {
                return FileType.BMP;
            }
            else if (Extension.ToUpper() == ".PCX")
            {
                return FileType.PCX;
            }
            else if (Extension.ToUpper() == ".TGA")
            {
                return FileType.TGA;
            }
            else if (Extension.ToUpper() == ".CR2")
            {
                return FileType.CR2;
            }
            else if (Extension.ToUpper() == ".NEF")
            {
                return FileType.NEF;
            }
            else if (Extension.ToUpper() == ".ORF")
            {
                return FileType.ORF;
            }
            else if (Extension.ToUpper() == ".SR2")
            {
                return FileType.SR2;
            }
            else if (Extension.ToUpper() == ".DWG")
            {
                return FileType.DWG;
            }
            else if (Extension.ToUpper() == ".DXF")
            {
                return FileType.DXF;
            }
            else if (Extension.ToUpper() == ".TPL")
            {
                return FileType.TPL;
            }
            else if (Extension.ToUpper() == ".CVX")
            {
                return FileType.CVX;
            }
            else if (Extension.ToUpper() == ".CNV")
            {
                return FileType.CNV;
            }
            else if (Extension.ToUpper() == ".CVI")
            {
                return FileType.CVI;
            }
            else if (Extension.ToUpper() == ".PSD")
            {
                return FileType.PSD;
            }
            else if (Extension.ToUpper() == ".TIFF")
            {
                return FileType.TIFF;
            }
            else if (Extension.ToUpper() == ".EPS")
            {
                return FileType.EPS;
            }
            else if (Extension.ToUpper() == ".AI")
            {
                return FileType.AI;
            }
            else if (Extension.ToUpper() == ".INDD")
            {
                return FileType.INDD;
            }
            else if (Extension.ToUpper() == ".RAW")
            {
                return FileType.RAW;
            }
            return FileType.RAW;
        }
        public static string GetTypeDescription(TransactionType hemType)
        {
            string s = null!;
            switch (hemType)
            {
                case TransactionType.REGISTRATION_FEE:
                    s = "Registration";
                    break;
                case TransactionType.CONSULTATION_FEE:
                    s = "Consultation";
                    break;
                case TransactionType.MEDICALPROCEDURE_FEE:
                    s = "Procedure";
                    break;
                case TransactionType.ROOM_RENT:
                    s = "Room rent";
                    break;
                case TransactionType.LAB_FEE:
                    s = "Lab";
                    break;
                case TransactionType.PHARMACY_FEE:
                    s = "Parmacy";
                    break;
                case TransactionType.MISCELLENEOUS:
                    s = "Others";
                    break;
                case TransactionType.ROOM_CLEANING_CHARGE:
                    s = "Room cleaning";
                    break;
                default:
                    break;
            }
            return s;
        }
        public static TransactionType GetType(string hemType)
        {
            TransactionType s = TransactionType.CONSULTATION_FEE;
            switch (hemType)
            {
                case "Registration":
                    s = TransactionType.REGISTRATION_FEE;
                    break;
                case "Procedure":
                    s = TransactionType.CONSULTATION_FEE;
                    break;
                case "Consultation":
                    s = TransactionType.MEDICALPROCEDURE_FEE;
                    break;
                case "Room rent":
                    s = TransactionType.ROOM_RENT;
                    break;
                case "Lab":
                    s = TransactionType.LAB_FEE;
                    break;
                case "Parmacy":
                    s = TransactionType.PHARMACY_FEE;
                    break;
                case "Others":
                    s = TransactionType.MISCELLENEOUS;
                    break;
                case "Room cleaning":
                    s = TransactionType.ROOM_CLEANING_CHARGE;
                    break;
                default:
                    break;
            }
            return s;
        }
        public static string GetEntryTypeName(EntryType Type)
        {
            string s = null!;
            switch (Type)
            {
                case EntryType.INVOICE:
                    s = "Invoice";
                    break;
                case EntryType.BILL:
                    s = "Bill";
                    break;
                case EntryType.RECEIPT:
                    s = "Receipt";
                    break;
                case EntryType.PAYMENT:
                    s = "Payment";
                    break;
                case EntryType.CREDIT_NOTE:
                    s = "Credit Note";
                    break;
                case EntryType.DEBIT_NOTE:
                    s = "Debit Note";
                    break;
                case EntryType.EXPENSE:
                    s = "Expense";
                    break;
                case EntryType.JOURNAL:
                    s = "Journal";
                    break;
                case EntryType.PURCHASE:
                    s = "Purchase Entry";
                    break;
                case EntryType.PURCHASE_RETURN:
                    s = "Purchase Return";
                    break;
                case EntryType.PURCHASE_ORDER:
                    s = "Purchase Order";
                    break;
                case EntryType.SALES:
                    s = "Sale Entry";
                    break;
                case EntryType.SALES_QUOTE:
                    s = "Sale Quote";
                    break;
                case EntryType.SALES_RETURN:
                    s = "Sale Return";
                    break;
                case EntryType.STOCK_IN:
                    s = "Stock IN";
                    break;
                case EntryType.STOCK_OUT:
                    s = "Stock Out";
                    break;
                case EntryType.PATIENT_FEE_RECEIPT:
                    s = "Patient Receipt";
                    break;
                case EntryType.OP_TOKEN:
                    s = "Op Token";
                    break;
                case EntryType.PRESCRIPTION:
                    s = "Prescription";
                    break;
                case EntryType.PATIENT_INVOICE:
                    s = "Patient Invoice";
                    break;
                case EntryType.STOCK_OPENING:
                    s = "Opening Stock";
                    break;
                case EntryType.STOCK_PURCHASE:
                    s = "Purchase Stock";
                    break;
                case EntryType.STOCK_SALE:
                    s = "Sale Stock";
                    break;
                case EntryType.STOCK_SALERETURN:
                    s = "Sale Return Stock";
                    break;
                case EntryType.STOCK_PURCHASERETURN:
                    s = "Purchase Return Stock";
                    break;
                case EntryType.STOCK_ADJUSTMENT:
                    s = "Adjustment Stock";
                    break;
                case EntryType.STOCK_DAMAGE:
                    s = "Damaged Stock";
                    break;
                case EntryType.STOCK_TOPATIENT:
                    s = "Stock ToPatient";
                    break;
                case EntryType.STOCK_REQUEST:
                    s = "Stock Request";
                    break;
                case EntryType.PATIENT_ID:
                    s = "Patient ID";
                    break;
                case EntryType.OP_ID:
                    s = "Patient OP Number";
                    break;
                case EntryType.IP_ID:
                    s = "Patient IP Number";
                    break;
                default:
                    break;
            }
            return s;
        }

        public static List<string> GetAvailablePrinter()
        {
            List<string> printerList = new();

            try
            {
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    printerList.Add(printer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load printers.\n" + ex.Message);
            }

            return printerList;
        }


        public static List<string> GetAvailablePrinterxx()
        {
            var PrinterList = new List<string>();

            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
            foreach (var printer in printerQuery.Get())
            {
                var name = printer.Properties["Name"].Value;
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var isNetworkPrinter = (bool)printer.GetPropertyValue("Network");
                var CurrentUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');
                PrinterList.Add(name.ToString()!);
            }
            return PrinterList;
        }
        public static void InitializeStockLocationComboTree(ToolStripComboTree SelectCombobox, long CompanyId)
        {
            HospitalInventoryManager HospitalInventoryManager = HospitalInventoryManager.Instance;
            IList<InventoryLocation> InventoryLocations = HospitalInventoryManager.ListAllInventoryLocation(CompanyId).ToArray<InventoryLocation>();
            if (InventoryLocations.Count > 0)
            {
                foreach (var InventoryLocation in InventoryLocations)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = InventoryLocation.Id.ToString();
                    parent.Text = InventoryLocation.Name;
                    SelectCombobox.Nodes.Add(parent);
                }
            }
        }
        public static void InitializeStockLocationCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            HospitalInventoryManager HospitalInventoryManager = HospitalInventoryManager.Instance;
            IList<InventoryLocation> InventoryLocations = HospitalInventoryManager.ListAllInventoryLocation(CompanyId).ToArray<InventoryLocation>();
            SelectBox.Nodes.Clear();
            if (InventoryLocations.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var InventoryLocation in InventoryLocations)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = InventoryLocation.Id.ToString();
                parent.Text = InventoryLocation.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializePatientCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            PatientManager PatientManager = PatientManager.Instance;
            IList<Patient> PatientInfo = PatientManager.ListAllPatient(CompanyId).ToArray<Patient>();
            if (PatientInfo.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var Patient in PatientInfo)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Patient.Id.ToString();
                parent.Text = Patient.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeEmployeeCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId, string Employees)
        {
            List<Employee> lemployee = (List<Employee>)EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Global.Company.CompanyId, Employees);
            SelectBox.Nodes.Clear();
            if (lemployee.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var employee in lemployee)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = employee.Id.ToString();
                parent.Text = employee.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeDoctorCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            List<Employee> lemployee = (List<Employee>)EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Global.Company.CompanyId, "Doctor");
            SelectBox.Nodes.Clear();
            if (lemployee.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var employee in lemployee)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = employee.Id.ToString();
                parent.Text = employee.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeNurseCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            List<Employee> lemployee = (List<Employee>)EmployeeManager.Instance.ListEmployeeByCompanyIdTitle(Global.Company.CompanyId, "Nurse");
            SelectBox.Nodes.Clear();
            if (lemployee.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var employee in lemployee)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = employee.Id.ToString();
                parent.Text = employee.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllWardCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<Ward> lWardInfo = WardManager.Instance.ListWardByCompanyId(CompanyId).ToArray<Ward>();
            SelectBox.Nodes.Clear();
            if (lWardInfo.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var WardInfos in lWardInfo)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = WardInfos.Id.ToString();
                parent.Text = WardInfos.Name;
                SelectBox.TreeNodes = parent;
            }

        }
        public static void InitializeAllInsuranceCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<InsuranceInfo> lInsuranceInfo = InsuranceInfoManager.Instance.ListAllActiveInsuranceInfoByCompanyId(CompanyId).ToArray<InsuranceInfo>();
            SelectBox.Nodes.Clear();
            if (lInsuranceInfo.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var InsuranceInfos in lInsuranceInfo)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = InsuranceInfos.Id.ToString();
                parent.Text = InsuranceInfos.InsuranceName;
                SelectBox.TreeNodes = parent;
            }

        }
        public static void InitializeAllDepartmentCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            DepartmentManager DepartmentManager = DepartmentManager.Instance;
            IList<Department> lDepartment = DepartmentManager.Instance.ListDepartmentByCompanyId(CompanyId).ToArray<Department>();
            SelectBox.Nodes.Clear();
            if (lDepartment.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var Departments in lDepartment)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Departments.Id.ToString();
                parent.Text = Departments.Name;
                SelectBox.TreeNodes = parent;
            }

        }
        public static void InitializeAllDiagnosisCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<Symptom> lSymptoms = SymptomsManager.Instance.ListSymptomByCompanyId(CompanyId);
            SelectBox.Nodes.Clear();
            if (lSymptoms.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var Departments in lSymptoms)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Departments.Id.ToString();
                parent.Text = Departments.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializePatientTypeCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<Patient> lConsultation = PatientManager.Instance.ListAllPatient(CompanyId).ToArray<Patient>();
            if (SelectBox != null && SelectBox.Nodes != null)
            {
                SelectBox.Nodes.Clear();
                ComboTreeNodeCollection nodeCollection = new ComboTreeNodeCollection();
                if (lConsultation.Count > 1)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = "All";
                    parent.Text = "All";
                    nodeCollection.Add(parent);
                }
                foreach (var Consultation in lConsultation)
                {
                    ComboTreeNode parent = new ComboTreeNode
                    {
                        Name = Consultation.Id.ToString(),
                        Text = Consultation.Name + " (" + Consultation.PatientNumber.ToString() + ")"
                    };
                    nodeCollection.Add(parent);
                }
                SelectBox.Nodes.AddRange(nodeCollection);
            }
        }
        public static void InitializeLabTestCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<MedicalTest> lMedicalTest = MedicalTestManager.Instance.GetMedicalTestsByCompanyId(CompanyId).ToArray<MedicalTest>();
            if (SelectBox != null && SelectBox.Nodes != null)
            {
                SelectBox.Nodes.Clear();
                if (lMedicalTest.Count > 1)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = "All";
                    parent.Text = "All";
                    SelectBox.TreeNodes = parent;
                }
                foreach (var MedicalTest in lMedicalTest)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = MedicalTest.Id.ToString();
                    parent.Text = MedicalTest.Name;
                    SelectBox.TreeNodes = parent;
                }
            }
        }
        public static void InitializeConsultantTypeCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<User> Consultant = UserManager.Instance.ListAllUserForCounslting(Global.Company.BusinessType).ToList<User>();
            SelectBox.Nodes.Clear();
            if (Consultant.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var PatientLedger in Consultant)
            {
                ComboTreeNode node = new ComboTreeNode();
                node.Name = PatientLedger.UserId.ToString();
                node.Text = PatientLedger.Name.ToString();
                SelectBox.TreeNodes = node;
            }
        }
        public static void InitializeFiscalYearCombo(ComboBoxSwapTextBox SelectBox, long CompanyId)
        {
            List<string> year = new List<string>();
            List<IdSpace> lIdSpace = CompanyManager.Instance.ListIdSpaceByCompanyForFiscalYear(CompanyId);
            int StartYear = lIdSpace.OrderByDescending(x => x.YearStartDate).First().YearStartDate.Year;
            year.Add((StartYear + 1).ToString());
            for (int i = StartYear - 1; i > StartYear - 11; i--)
            {
                if (lIdSpace.FirstOrDefault(x => x.YearStartDate.Year == i) == null)
                {
                    year.Add(i.ToString());
                }
            }
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(year.ToArray<string>());
            SelectBox.SelectedIndex = SelectBox.FindStringExact((StartYear + 1).ToString());
        }
        public static void InitializeYearCombo(ComboBoxSwapTextBox SelectBox, Company Company)
        {
            List<string> year = new List<string>();
            List<IdSpace> lIdSpace = CompanyManager.Instance.ListIdSpaceByCompanyForFiscalYear(Company.CompanyId);
            DateTime Start = CompanyManager.getFiscalYearStartDate(Company).Date;
            DateTime End = CompanyManager.getFiscalYearEndDate(Company).Date;

            foreach (IdSpace IdSpace in lIdSpace.OrderByDescending(x => x.YearStartDate))
            {
                string yearString = IdSpace.YearStartDate.Year.ToString() + (Company.AccountingStartDate != 1 ? "-" + IdSpace.YearEndDate.Year.ToString() : "");
                year.Add(yearString);
            }

            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(year.ToArray<string>());

            string searchString = Start.Year.ToString() + (Company.AccountingStartDate != 1 ? "-" + End.Year.ToString() : "");
            int selectedIndex = SelectBox.FindStringExact(searchString);

            if (selectedIndex == -1)
            {
                selectedIndex = 0;
            }

            SelectBox.SelectedIndex = selectedIndex;
        }
        public static void InitializeAllMedicalProcedures(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<MedicalProcedure> Procedures = MedicalProcedureManager.Instance.ListActiveMedicalProcedureByCompanyId(Global.Company.CompanyId);
            SelectBox.Nodes.Clear();
            foreach (var lProcedure in Procedures)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = lProcedure.Id.ToString();
                parent.Text = lProcedure.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public class LocationType
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
        public static void InitializeInventoryLocationType(ComboBox SelectBox)
        {
            SelectBox.Items.Clear();
            SelectBox.ValueMember = "Id";
            SelectBox.DisplayMember = "Name";
            if (Global.Company.BusinessType == model.Accounting.Masters.BuisnessType.Hospital)
            {
                SelectBox.Items.AddRange(new[] {
                new LocationType() { Name = "Ward", Id = 0 },
                new LocationType() { Name = "Store", Id = 1 },
                new LocationType() { Name = "Godown", Id = 2 },
                new LocationType() { Name = "Nurshing Station", Id = 3 },
                new LocationType() { Name = "Pharmacy", Id = 4 }
             });
            }
            else if (Global.Company.BusinessType == model.Accounting.Masters.BuisnessType.Wholesale
                || Global.Company.BusinessType == model.Accounting.Masters.BuisnessType.Retail)
            {
                SelectBox.Items.AddRange(new[] {
                 new LocationType() { Name = "Store", Id = 1 },
                new LocationType() { Name = "Godown", Id = 2 }
             });
            }
            else if (Global.Company.BusinessType == model.Accounting.Masters.BuisnessType.Pharmacy)
            {
                SelectBox.Items.AddRange(new[] {
                new LocationType() { Name = "Store", Id = 1 },
                new LocationType() { Name = "Godown", Id = 2 },
                new LocationType() { Name = "Pharmacy", Id = 4 }
             });
            }
        }
        public static void InitializeConsultationCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<Consultation> Consultation = ConsultationManager.Instance.ListConsultationByCompanyId(Global.Company.CompanyId);
            IList<MedicalProcedure> procedures = MedicalProcedureManager.Instance.ListMedicalProcedureByCompanyId(Global.Company.CompanyId);
            SelectBox.Nodes.Clear();
            if (Consultation.Count > 1 || (Consultation.Count > 0 && procedures.Count > 0))
            {
                ComboTreeNode All = new ComboTreeNode();
                All.Name = "All";
                All.Text = "All";
                SelectBox.TreeNodes = All;
            }
            foreach (var lConsultation in Consultation)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = lConsultation.Id.ToString();
                parent.Text = lConsultation.Name;
                SelectBox.TreeNodes = parent;
            }
            if (procedures != null && procedures.Count > 0)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "Procedure";
                parent.Text = "Procedure Fee";
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllAccountComboForLedger(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            AccountManager AccountManager = AccountManager.Instance;
            IList<Account> Account = AccountManager.GetAllAccountsByCompanyId(CompanyId).ToArray<Account>();
            SelectBox.Nodes.Clear();
            if (Account.Count > 1)
            {
                ComboTreeNode All = new ComboTreeNode();
                All.Name = "All";
                All.Text = "All";
                SelectBox.TreeNodes = All;
            }
            foreach (var lAccount in Account)
            {
                if (lAccount.ParentAccountId == null)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = lAccount.Id.ToString();
                    parent.Text = lAccount.Name;
                    SelectBox.TreeNodes = parent;
                    foreach (var llAccount in Account.Where(x => x.ParentAccountId == lAccount.Id))
                    {
                        ComboTreeNode child = new ComboTreeNode();
                        child.Name = llAccount.Id.ToString();
                        child.Text = llAccount.Name;
                        parent.Nodes.Add(child);
                    }
                }
            }
        }
        public static void InitializeScheduleCombo(ComboBox SelectBox)
        {
            List<string> ProductSchedules = new List<string>
            {
                "Schedule-B",
                "Schedule-C, C1",
                "Schedule-D",
                "Schedule-D I",
                "Schedule-D II",
                "Schedule-F I",
                "Schedule-F II",
                "Schedule-F III",
                "Schedule-FF",
                "Schedule-G",
                "Schedule-H",
                "Schedule-H1",
                "Schedule-H 2",
                "Schedule-J",
                "Schedule-K",
                "Schedule-L1",
                "Schedule-M",
                "Schedule-M I",
                "Schedule-N",
                "Schedule-O",
                "Schedule-P",
                "Schedule-P1",
                "Schedule-Q",
                "Schedule-R",
                "Schedule-R I",
                "Schedule-S",
                "Schedule-T",
                "Schedule-TA",
                "Schedule-U",
                "Schedule-U I",
                "Schedule-V",
                "Schedule-X",
                "Schedule-Y"};
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(ProductSchedules.ToArray<string>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializePaperFormatCombo(ComboBox SelectBox)
        {
            PaperFormatManager PaperFormatManager = PaperFormatManager.Instance;
            SelectBox.Items.Clear();
            if (SelectBox.Name == "ComboBoxCompanySalePaperFormat")
            {
                SelectBox.Items.AddRange(PaperFormatManager.ListPrintPaperFormat().Where(x => x.Name != "A5 PORTRAIT").ToArray<PrintPaperFormat>());
            }
            else if (SelectBox.Name == "ComboBoxCompanyPrescriptionPaperFormat")
            {
                SelectBox.Items.AddRange(PaperFormatManager.ListPrintPaperFormat().Where(x => x.Name == "A4 PORTRAIT").ToArray<PrintPaperFormat>());
            }
            else
            {
                SelectBox.Items.AddRange(PaperFormatManager.ListPrintPaperFormat().Where(x => x.Name == "A5 LANDSCAPE").ToArray<PrintPaperFormat>());
            }
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeStockLocationCombo(ComboBox SelectBox, long CompanyId)
        {
            HospitalInventoryManager HospitalInventoryManager = HospitalInventoryManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(HospitalInventoryManager.ListAllInventoryLocation(CompanyId).ToArray<InventoryLocation>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeStockLocationCombo(ToolStripComboBox SelectBox, long CompanyId)
        {
            HospitalInventoryManager HospitalInventoryManager = HospitalInventoryManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(HospitalInventoryManager.ListAllInventoryLocation(CompanyId).ToArray<InventoryLocation>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeStockLocationComboWithAll(ToolStripComboBox SelectBox, long CompanyId)
        {
            HospitalInventoryManager HospitalInventoryManager = HospitalInventoryManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.Add("All");
            SelectBox.Items.AddRange(HospitalInventoryManager.ListAllInventoryLocation(CompanyId).ToArray<InventoryLocation>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeStockLocationComboWithEmpty(ComboBox SelectBox, long CompanyId)
        {
            HospitalInventoryManager HospitalInventoryManager = HospitalInventoryManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.Add("");
            SelectBox.Items.AddRange(HospitalInventoryManager.ListAllInventoryLocation(CompanyId).ToArray<InventoryLocation>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeCategeoryParentCombo(ComboBox SelectBox, long CompanyId)
        {
            CategoryManager CategoryManager = CategoryManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CategoryManager.ListCategoryByCompanyId(CompanyId).ToArray<Category>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeProductCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            IList<Product> CatalogItems = CatalogProductManager.ListProductByCompanyId(CompanyId).ToArray<Product>();
            SelectBox.Nodes.Clear();
            if (CatalogItems.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var lCatalogItems in CatalogItems)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = lCatalogItems.Id.ToString();
                parent.Text = lCatalogItems.Name + " " + "(" + lCatalogItems.MaterialId + ")";
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeBedTypeCombo(ComboBox SelectBox, long CompanyId)
        {
            BedManager BedManager = BedManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(BedManager.ListBedTypeByCompanyId(CompanyId).ToArray<BedType>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeWardBedCombo(ComboBox SelectBox, long WardId)
        {
            WardManager WardManager = WardManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(WardManager.ListWardBedByWardId(WardId).ToArray<Bed>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeWardCombo(ComboBox SelectBox, long CompanyId)
        {
            WardManager WardManager = WardManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(WardManager.ListWardByCompanyId(CompanyId).ToArray<Ward>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeWardComboWithAll(ToolStripComboBox SelectBox, long CompanyId)
        {
            WardManager WardManager = WardManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.Add("All");
            SelectBox.Items.AddRange(WardManager.ListWardByCompanyId(CompanyId).ToArray<Ward>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeWardCombo(ToolStripComboBox SelectBox, long CompanyId)
        {
            WardManager WardManager = WardManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(WardManager.ListWardByCompanyId(CompanyId).ToArray<Ward>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeDoctorCombo(ComboBox SelectBox, long CompanyId)
        {
            EmployeeManager EmployeeManager = EmployeeManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(EmployeeManager.ListEmployeeByCompanyIdTitle(CompanyId, "Doctor").ToArray<Employee>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeNurseCombo(ComboBox SelectBox, long CompanyId)
        {
            EmployeeManager EmployeeManager = EmployeeManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(EmployeeManager.ListEmployeeByCompanyIdTitle(CompanyId, "Nurse").ToArray<Employee>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeInsurerCombo(ComboBox SelectBox, long PatientId)
        {
            PersonManager PersonManager = PersonManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.ResetText();
            SelectBox.Items.AddRange(PersonManager.ListAllInsurerPersonByCompanyId(Global.Company.CompanyId, PatientId).ToArray<Person>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializePatientInsurerCombo(ComboBox SelectBox, long PatientId, string InsuranceCoverage)
        {
            PersonManager PersonManager = PersonManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.ResetText();
            SelectBox.Items.AddRange(InsuranceInfoManager.Instance.ListAllInsuranceInfoByPatientId(PatientId, true, InsuranceCoverage).ToArray<InsuranceInfo>()); // SelectBox.Items.AddRange(InsuranceInfoManager.Instance.ListAllActiveInsuranceInfoByPatientId(PatientId).ToArray<InsuranceInfo>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeResponsiblePartyCombo(ComboBox SelectBox, long PatientId)
        {
            GuardianManager GuardianManager = GuardianManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.ResetText();
            SelectBox.Items.AddRange(GuardianManager.ListAllGuardianByPatientId(PatientId).ToArray<Guardian>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeDepartmentCombo(ComboBox SelectBox, long CompanyId)
        {
            DepartmentManager DepartmentManager = DepartmentManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(DepartmentManager.ListDepartmentByCompanyId(CompanyId).ToArray<Department>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeParentDepartmentCombo(ComboBox SelectBox, long CompanyId)
        {
            DepartmentManager DepartmentManager = DepartmentManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(DepartmentManager.ListParentDepartmentByCompanyId(CompanyId).ToArray<Department>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeTitleCombo(ComboBox SelectBox, long CompanyId, SoftwareType softwareType)
        {
            TitleManager TitleManager = TitleManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(TitleManager.ListTitleByCompanyId(CompanyId, softwareType).ToArray<Title>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeSymptomCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            SymptomsManager SymptomsManager = SymptomsManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(SymptomsManager.ListSymptomCategoryByCompanyId(CompanyId).ToArray<SymptomCategory>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeParentSymptomCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            SymptomsManager SymptomsManager = SymptomsManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(SymptomsManager.ListParentSymptomCategoryByCompanyId(CompanyId).ToArray<SymptomCategory>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllergieCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            QuestionManager AllergieManager = QuestionManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AllergieManager.ListAllergieCategoryByCompanyId(CompanyId).ToArray<AllergieCategory>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeParentAllergieCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            QuestionManager AllergieManager = QuestionManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AllergieManager.ListParentAllergieCategoryByCompanyId(CompanyId).ToArray<AllergieCategory>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeMedicalTestCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            MedicalTestManager MedicalTestManager = MedicalTestManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(MedicalTestManager.ListMedicalTestCategoryByCompanyId(CompanyId).ToArray<MedicalTestCategory>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeMedicalTestSampleRequirementCombo(ComboBox SelectBox, long CompanyId)
        {
            MedicalTestManager MedicalTestManager = MedicalTestManager.Instance;
            SelectBox.Items.Clear();
            string[] Samples = MedicalTestManager.ListMedicalTestSampleByCompanyId(CompanyId).Where(sample => sample != null).ToArray<string>();
            if (Samples.Length > 0)
            {
                SelectBox.Items.AddRange(Samples);
            }
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeParentMedicalTestCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            MedicalTestManager MedicalTestManager = MedicalTestManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(MedicalTestManager.ListParentMedicalTestCategoryByCompanyId(CompanyId).ToArray<MedicalTestCategory>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeMedicalProcedureCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            MedicalProcedureManager MedicalProcedureManager = MedicalProcedureManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(MedicalProcedureManager.ListMedicalProcedureCategoryByCompanyId(CompanyId).ToArray<MedicalProcedureCategory>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeParentMedicalProcedureCategoryCombo(ComboBox SelectBox, long CompanyId)
        {
            MedicalProcedureManager MedicalProcedureManager = MedicalProcedureManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(MedicalProcedureManager.ListParentMedicalProcedureCategoryByCompanyId(CompanyId).ToArray<MedicalProcedureCategory>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeCostCenterComboByUserAccess(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            UserManager UserManager = UserManager.Instance;
            IList<CostCenter> CostCenter = (UserManager.GetAccessibleCostCenter(Global.User, CompanyId).ToArray<CostCenter>());
            SelectBox.Nodes.Clear();
            if (CostCenter.Count > 1)
            {
                ComboTreeNode All = new ComboTreeNode();
                All.Name = "All";
                All.Text = "All";
                SelectBox.TreeNodes = All;
            }
            foreach (var lCostCenter in CostCenter)
            {
                if (lCostCenter != null)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = lCostCenter.CostCenterId.ToString();
                    parent.Text = lCostCenter.Name;
                    SelectBox.TreeNodes = parent;
                }
            }
        }

        public static void InitializeBankAccountCombo(ComboBox SelectBox, long CompanyId)
        {
            AccountManager AccountManager = AccountManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountManager.ListAllAccountByGroupIds(CompanyId, Global.BankAccountInReceipt).ToArray<Account>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeCreditCardAccountCombo(ComboBox SelectBox, long CompanyId)
        {
            AccountManager AccountManager = AccountManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountManager.ListAllAccountByGroupIds(CompanyId, new long?[] { 701 }).ToArray<Account>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeReceiptCreditAccountCombo(ComboBox SelectBox, long CompanyId)
        {
            AccountManager AccountManager = AccountManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountManager.ListAllCreditDebitAccount(CompanyId).ToArray<Account>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeCostCenterCombo(ComboBox SelectBox, long CompanyId)
        {
            UserManager UserManager = UserManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(UserManager.GetAccessibleCostCenter(Global.User, CompanyId).ToArray<CostCenter>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeCostCenterComboByUserAccess(ToolStripComboBox SelectBox, long CompanyId)
        {
            UserManager UserManager = UserManager.Instance;
            IList<CostCenter> CostCenter = (UserManager.GetAccessibleCostCenter(Global.User, CompanyId).ToArray<CostCenter>());
            SelectBox.Items.Clear();
            if (CostCenter.Count > 0)
            {
                SelectBox.Items.Add("All");
                SelectBox.Items.AddRange(CostCenter.ToArray<CostCenter>());
            }
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeReferedCombo(ComboBox SelectBox)
        {
            ReferedManager ReferedManager = ReferedManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(ReferedManager.GetAllRefered(Global.Company.CompanyId).ToArray<Refered>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeReferedCombo(ToolStripComboBox SelectBox)
        {
            ReferedManager ReferedManager = ReferedManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(ReferedManager.GetAllRefered(Global.Company.CompanyId).ToArray<Refered>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeStateCombo(ComboBox SelectBox, long CountryId)
        {
            StateManager StateManager = StateManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(StateManager.GetAllStates(CountryId).ToArray<State>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeStateByCountryCombo(ComboBox SelectBox, long CountryId)
        {
            StateManager StateManager = StateManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(StateManager.ListAllStatesByCountry(CountryId).ToArray<State>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeCountryCombo(ComboBox SelectBox)
        {
            CountryManager CountryManager = CountryManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CountryManager.ListAllCountry().ToArray<Country>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAccountMethodCombo(ComboBox SelectBox)
        {
            AccountingMethodManager AccountingMethodManager = AccountingMethodManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountingMethodManager.GetAllAccountingMethod().ToArray<AccountingMethod>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeCurrencyCombo(ComboBox SelectBox)
        {
            fa.api.Accounting.CurrencyManager CurrencyManager = fa.api.Accounting.CurrencyManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CurrencyManager.GetAllCurrencies().ToArray<Currency>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeCompanyTypeCombo(ComboBox SelectBox)
        {
            CompanyTypeManager CompanyTypeManager = CompanyTypeManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CompanyTypeManager.GetAllCompanyType().ToArray<CompanyType>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeBusinessTypeCompanyCombo(ComboBox SelectBox, List<Company> Companies, SoftwareType filterSoftwareType)
        {
            SelectBox.Items.Clear();

            if (Companies != null)
            {
                List<Company> SelectCompanies = Companies.Where(company => company.BusinessType != BuisnessType.Hospital).ToList();
                if (filterSoftwareType == SoftwareType.MEDICARE)
                {
                    SelectCompanies = Companies.Where(company => company.BusinessType == BuisnessType.Hospital).ToList();
                }
                SelectBox.Items.AddRange(SelectCompanies.ToArray());

            }

            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeCompanyCombo(ComboBox SelectBox, List<Company> Companies)
        {
            SelectBox.Items.Clear();
            if (Companies != null)
            {
                SelectBox.Items.AddRange(Companies.ToArray<Company>());
            }
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeCustomerCombo(ComboBox SelectBox, long CompanyId)
        {
            CustomerManager CustomerManager = CustomerManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CustomerManager.ListParentCustomerByCompanyId(CompanyId).ToArray<Customer>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllCustomerCombo(ComboBox SelectBox, long CompanyId)
        {
            CustomerManager CustomerManager = CustomerManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CustomerManager.ListCustomerByCompanyId(CompanyId).ToArray<Customer>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeSupplierCombo(ComboBox SelectBox, long CompanyId)
        {
            SupplierManager SupplierManager = SupplierManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(SupplierManager.ListParentSupplierByCompanyId(CompanyId).ToArray<Supplier>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeAllSupplierCombo(ComboBox SelectBox, long CompanyId)
        {
            SupplierManager SupplierManager = SupplierManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(SupplierManager.ListSupplierByCompanyId(CompanyId).ToArray<Supplier>());
            SelectBox.SelectedIndex = -1;
        }


        public static void InitializePaymentMethodCombo(ComboBox SelectBox, long CompanyId)
        {
            PaymentMethodManager PaymentMethodManager = PaymentMethodManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(PaymentMethodManager.GetAllPaymentMethodByCompanyId(CompanyId).ToArray<PaymentMethod>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializePaymentTermCombo(ComboBox SelectBox, long CompanyId)
        {
            PaymentTermManager PaymentTermManager = PaymentTermManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(PaymentTermManager.GetAllPaymentTermByCompanyId(CompanyId).ToArray<PaymentTerm>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeAllAccountCombo(ComboBox SelectBox, long CompanyId)
        {
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountManager.Instance.GetAllGeneralAccountsByCompanyId(CompanyId).ToArray<Account>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAccountCombo(ComboBox SelectBox, long CompanyId)
        {
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountManager.Instance.ListParentAccountByCompanyId(CompanyId).ToArray<Account>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAccountGroupCombo(ComboBox SelectBox)
        {
            AccountGroupManager AccountGroupManager = AccountGroupManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountGroupManager.GetParentAccountGroup().ToArray<AccountGroup>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAccountDetailTypeCombo(ComboBox SelectBox, long AccountGroupId)
        {
            AccountGroupManager AccountGroupManager = AccountGroupManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(AccountGroupManager.ListAccountGroupById(AccountGroupId).ToArray<AccountGroup>());
            if (SelectBox.Items.Count > 0) { SelectBox.SelectedIndex = 0; }
        }
        public static void InitializeAlltManufactureCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            IList<CatalogItem> CatalogItems = CatalogProductManager.Instance.ListAllManufacture(CompanyId);

            var distinctManufacturerNames = CatalogItems.Select(item => item.Manufacturer).Distinct().ToArray();

            SelectBox.Nodes.Clear();
            if (distinctManufacturerNames.Length > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }

            foreach (var manufacturerName in distinctManufacturerNames)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = CatalogItems.First(item => item.Manufacturer == manufacturerName).Id.ToString();
                parent.Text = manufacturerName;
                SelectBox.TreeNodes = parent;
            }
        }

        public static void InitializeAllSupplierFromAccount(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            SupplierManager SupplierManager = SupplierManager.Instance;
            IList<Supplier> Suppliers = SupplierManager.ListSupplierByCompanyId(CompanyId);

            var distinctSupplierNames = Suppliers.Select(supplier => supplier.Name).Distinct().ToArray();

            SelectBox.Nodes.Clear();
            if (distinctSupplierNames.Length > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }

            foreach (var supplierName in distinctSupplierNames)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Suppliers.First(supplier => supplier.Name == supplierName).Id.ToString();
                parent.Text = supplierName;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllSupplierComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            SupplierManager SupplierManager = SupplierManager.Instance;
            IList<CatalogItem> CatalogItems = CatalogProductManager.Instance.ListAllSupplier(CompanyId);

            var distinctSupplierNames = CatalogItems.Select(item => item.SupplierName).Distinct().ToArray();

            SelectBox.Nodes.Clear();
            if (distinctSupplierNames.Length > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }

            foreach (var supplierName in distinctSupplierNames)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = CatalogItems.First(supplier => supplier.SupplierName == supplierName).Id.ToString();
                parent.Text = supplierName;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllRackNumberComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            IList<Product> RackInfos = CatalogProductManager.ListAllRackNumber(CompanyId);

            var distinctRackNumbers = RackInfos.Select(rack => rack.RackNumber).Distinct().ToArray();

            SelectBox.Nodes.Clear();
            if (distinctRackNumbers.Length > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }

            foreach (var rackNumber in distinctRackNumbers)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = RackInfos.First(info => info.RackNumber == rackNumber).Id.ToString();
                parent.Text = rackNumber;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllUniqueProductManufactureCombo(ComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CatalogProductManager.GetAllUniqueProductManufacture(CompanyId).ToArray<string>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllUniqueProductSupplierCombo(ComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(SupplierManager.Instance.GetAllSupplier(CompanyId).ToArray<Supplier>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllUniqueRackNumberCombo(ComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CatalogProductManager.GetAllUniqueRackNumber(CompanyId).ToArray<string>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllUniquePurchaseUOMCombo(ComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CatalogProductManager.GetAllUniquePurchaseUOM(CompanyId).ToArray<string>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllUniqueRetailUOMCombo(ComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CatalogProductManager.GetAllUniqueRetailUOM(CompanyId).ToArray<string>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllUniqueWholeSaleUOMCombo(ComboBox SelectBox, long CompanyId)
        {
            CatalogProductManager CatalogProductManager = CatalogProductManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(CatalogProductManager.GetAllUniqueWholeSaleUOM(CompanyId).ToArray<string>());
            SelectBox.SelectedIndex = -1;
        }

        public static void InitializeInvoiceMappingTempletCombo(ComboBox SelectBox, long CompanyId)
        {
            InvoiceMappingManager DepartmentManager = InvoiceMappingManager.Instance;
            SelectBox.Items.Clear();
            SelectBox.Items.AddRange(InvoiceMappingManager.Instance.GetAllInvoiceMappingTemplate(CompanyId).ToArray<string>());
            SelectBox.SelectedIndex = -1;
        }
        public static void InitializeAllItemComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CatalogItemManager catalogItemManager = CatalogItemManager.Instance;
            IList<CatalogItem> CatalogItems = CatalogItemManager.Instance.ListItemsByCompanyId(CompanyId).ToArray<CatalogItem>();
            SelectBox.Nodes.Clear();
            if (CatalogItems.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var Item in CatalogItems)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Item.Id.ToString();
                parent.Text = Item.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllCategoryCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CategoryManager CategoryManager = CategoryManager.Instance;
            IList<Category> Categorys = CategoryManager.Instance.ListCategoryByCompanyId(CompanyId).ToArray<Category>();
            SelectBox.Nodes.Clear();
            if (Categorys.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var Category in Categorys)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Category.Id.ToString();
                parent.Text = Category.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllSupplierCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            SupplierManager SupplierManager = SupplierManager.Instance;
            IList<Supplier> Suppliers = SupplierManager.Instance.ListSupplierByCompanyId(CompanyId).ToArray<Supplier>();
            IList<Customer> Customers = CustomerManager.Instance.ListCustomerByCompanyId(CompanyId).ToArray<Customer>();

            SelectBox.Nodes.Clear();
            if (Suppliers.Count > 0 || Customers.Count > 0)
            {
                if ((Suppliers.Count > 1 && Customers.Count == 0) || (Suppliers.Count == 0 && Customers.Count > 1) || (Suppliers.Count >= 1 && Customers.Count >= 1))
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = "All";
                    parent.Text = "All";
                    SelectBox.TreeNodes = parent;
                }
            }
            foreach (var Supplier in Suppliers)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Supplier.Id.ToString();
                parent.Text = Supplier.Name;
                SelectBox.TreeNodes = parent;
            }
            foreach (var Customer in Customers)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Customer.Id.ToString();
                parent.Text = Customer.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllCustomerCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            IList<Customer> Customers = CustomerManager.Instance.ListCustomerByCompanyId(CompanyId).ToArray<Customer>();

            SelectBox.Nodes.Clear();
            if (Customers.Count > 0)
            {
                if ((Customers.Count == 0) || (Customers.Count > 1))
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = "All";
                    parent.Text = "All";
                    SelectBox.TreeNodes = parent;
                }
            }
            foreach (var Customer in Customers)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = Customer.Id.ToString();
                parent.Text = Customer.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllPFamilyCombo(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CatalogProductFamilyManager catalogProductFamilyManager = CatalogProductFamilyManager.Instance;
            IList<ProductFamily> ProductFamilies = CatalogProductFamilyManager.Instance.ListProductFamilyByCompanyId(CompanyId).ToArray<ProductFamily>();
            SelectBox.Nodes.Clear();
            if (ProductFamilies.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var ProductFamily in ProductFamilies)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = ProductFamily.Id.ToString();
                parent.Text = ProductFamily.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeAllDistinctFamilyProductComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CatalogProductFamilyManager FamilyManager = CatalogProductFamilyManager.Instance;
            IList<CatalogItem> FamilyProducts = FamilyManager.ListProductFamilyByCompanyId(CompanyId).ToArray<CatalogItem>();

            if (FamilyProducts.Count > 0)
            {
                ComboTreeNode allNode = new ComboTreeNode();
                allNode.Name = "All";
                allNode.Text = "All";

                // Group products by family name
                var groupedProducts = FamilyProducts
                    .GroupBy(pf => pf.Name)
                    .Select(group => new
                    {
                        FamilyName = group.Key,
                        // ProductIds = group.Select(pf => pf.Id)  // Commented out to exclude IDs
                    });

                foreach (var familyGroup in groupedProducts)
                {
                    ComboTreeNode familyNode = new ComboTreeNode();
                    familyNode.Name = familyGroup.FamilyName;
                    familyNode.Text = familyGroup.FamilyName;

                    allNode.Nodes.Add(familyNode);
                }

                SelectBox.TreeNodes = allNode;
            }
        }

        // Helper method to get product IDs by family name
        private static IEnumerable<long> GetProductIdsByFamilyName(IList<CatalogItem> familyProducts, string familyName)
        {
            return familyProducts
                .Where(pf => pf.Name == familyName)
                .Select(pf => pf.Id);
        }
        public static void InitializeAllFamilyProductComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CategoryManager CategoryManager = CategoryManager.Instance;
            IList<CatalogItem> FamilyProducts = CatalogProductFamilyManager.Instance.ListProductFamilyByCompanyId(CompanyId).ToArray<CatalogItem>();
            SelectBox.Nodes.Clear();
            if (FamilyProducts.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var FamilyProduct in FamilyProducts)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = FamilyProduct.Id.ToString();
                parent.Text = FamilyProduct.Name;
                SelectBox.TreeNodes = parent;
            }
        }
        public static void InitializeReferedComboBox(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            ReferedManager ReferedManager = ReferedManager.Instance;
            IList<Refered> refereds = ReferedManager.ListAllRefered(Global.Company.CompanyId).ToArray<Refered>();
            SelectBox.Nodes.Clear();
            if (refereds.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var lrefereds in refereds)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = lrefereds.Id.ToString();
                parent.Text = lrefereds.Name;
                SelectBox.Nodes.Add(parent);
            }
        }

        public static void InitializeCustomerComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CustomerManager CustomerManager = CustomerManager.Instance;
            IList<Customer> Customer = CustomerManager.GetAllCustomer(CompanyId).ToArray<Customer>();
            SelectBox.Nodes.Clear();
            if (Customer.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var lCustomer in Customer)
            {
                if (lCustomer.ParentAccountId == null)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = lCustomer.Id.ToString();
                    parent.Text = lCustomer.Name;
                    SelectBox.Nodes.Add(parent);
                    foreach (var llCustomer in Customer.Where(x => x.ParentAccountId == lCustomer.Id))
                    {
                        ComboTreeNode child = new ComboTreeNode();
                        child.Name = llCustomer.Id.ToString();
                        child.Text = llCustomer.Name;
                        parent.Nodes.Add(child);
                    }
                }
            }
        }
        public static void InitializeVendorsComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CustomerManager CustomerManager = CustomerManager.Instance;
            List<AccountHelperData> Accounts = AccountHelper.GetHelpData(0, true, true, false, false, "", Global.Company);
            SelectBox.Nodes.Clear();
            if (Accounts.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var lAccount in Accounts)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = lAccount.Id.ToString();
                parent.Text = lAccount.Name;
                SelectBox.Nodes.Add(parent);
            }
        }
        public static void InitializeCustomerComboForReturn(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CustomerManager CustomerManager = CustomerManager.Instance;
            List<string> Customer = SalesManager.Instance.GetCustomerFromSaleEntry(Global.Company.CompanyId).ToList<string>();
            SelectBox.Nodes.Clear();
            if (Customer.Count > 1)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = "All";
                parent.Text = "All";
                SelectBox.TreeNodes = parent;
            }
            foreach (var lCustomer in Customer)
            {
                ComboTreeNode parent = new ComboTreeNode();
                parent.Name = lCustomer;
                parent.Text = lCustomer;
                SelectBox.Nodes.Add(parent);
            }
        }
        public static void InitializeAreaComboForReport(ToolstripCheckedTreeComboBox SelectBox, long CompanyId)
        {
            CustomerManager CustomerManager = CustomerManager.Instance;

            IList<string> addresses = AddressManager.Instance.GetCustomerCompanyRelatedAddresses(Global.Company);
            HashSet<string> uniqueAddresses = new HashSet<string>(addresses, StringComparer.OrdinalIgnoreCase);

            SelectBox.Nodes.Clear();

            if (uniqueAddresses.Count > 1)
            {
                ComboTreeNode allNode = new ComboTreeNode();
                allNode.Name = "All";
                allNode.Text = "All";
                SelectBox.Nodes.Add(allNode);
            }

            ComboTreeNode unknownNode = new ComboTreeNode();
            unknownNode.Name = "UnKnown Area";
            unknownNode.Text = "UnKnown Area";
            SelectBox.Nodes.Add(unknownNode);

            foreach (var address in uniqueAddresses)
            {
                ComboTreeNode addressNode = new ComboTreeNode();
                addressNode.Name = address;
                addressNode.Text = address;
                SelectBox.Nodes.Add(addressNode);
            }
        }
        public static void InitializeCustomerComboForReport(ToolStripComboTree SelectBox, long CompanyId)
        {
            CustomerManager CustomerManager = CustomerManager.Instance;
            IList<Customer> Customer = CustomerManager.GetAllCustomer(CompanyId).ToArray<Customer>();
            SelectBox.Nodes.Clear();
            foreach (var lCustomer in Customer)
            {
                if (lCustomer.ParentAccountId == null)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = lCustomer.Id.ToString();
                    parent.Text = lCustomer.Name;
                    SelectBox.Nodes.Add(parent);
                    foreach (var llCustomer in Customer.Where(x => x.ParentAccountId == lCustomer.Id))
                    {
                        ComboTreeNode child = new ComboTreeNode();
                        child.Name = llCustomer.Id.ToString();
                        child.Text = llCustomer.Name;
                        parent.Nodes.Add(child);
                    }
                }
            }
        }
        public static void InitializeSupplierComboForReport(ToolStripComboTree SelectBox, long CompanyId)
        {
            SupplierManager SupplierManager = SupplierManager.Instance;
            IList<Supplier> Supplier = SupplierManager.GetAllSupplier(CompanyId).ToArray<Supplier>();
            SelectBox.Nodes.Clear();
            foreach (var lSupplier in Supplier)
            {
                if (lSupplier.ParentAccountId == null)
                {
                    ComboTreeNode parent = new ComboTreeNode();
                    parent.Name = lSupplier.Id.ToString();
                    parent.Text = lSupplier.Name;
                    SelectBox.Nodes.Add(parent);
                    foreach (var llCustomer in Supplier.Where(x => x.ParentAccountId == lSupplier.Id))
                    {
                        ComboTreeNode child = new ComboTreeNode();
                        child.Name = llCustomer.Id.ToString();
                        child.Text = llCustomer.Name;
                        parent.Nodes.Add(child);
                    }
                }
            }
        }
        public static void InitializeAllCategoryComboForReport(ToolStripComboTree SelectBox, long CompanyId)
        {
            CategoryManager CategoryManager = CategoryManager.Instance;
            IList<CatalogItem> CatalogItems = CategoryManager.ListCategoryByCompanyId(CompanyId).ToArray<CatalogItem>();
            if (CatalogItems.Count > 0)
            {
                foreach (var CatalogItem in CatalogItems)
                {
                    if (CatalogItem.Type == CatalogItemType.CATEGORY && CatalogItem.ParentId == null)
                    {
                        ComboTreeNode parent = new ComboTreeNode();
                        parent.Name = CatalogItem.Id.ToString();
                        parent.Text = CatalogItem.Name;
                        SelectBox.Nodes.Add(parent);
                        LoadChildNodes(parent, CatalogItem.Id, CatalogItems);
                    }
                }
            }
        }
        public static void InitializeAllFamilyProductComboForReport(ToolStripComboTree SelectBox, long CompanyId)
        {
            CategoryManager CategoryManager = CategoryManager.Instance;
            IList<CatalogItem> CatalogItems = CategoryManager.ListParentCategoryByCompanyId(CompanyId).ToArray<CatalogItem>();
            if (CatalogItems.Count > 0)
            {
                foreach (var CatalogItem in CatalogItems)
                {
                    if (CatalogItem.Type == CatalogItemType.CATEGORY && CatalogItem.ParentId == null)
                    {
                        ComboTreeNode parent = new ComboTreeNode();
                        parent.Name = CatalogItem.Id.ToString();
                        parent.Text = CatalogItem.Name;
                        SelectBox.Nodes.Add(parent);
                        LoadChildNodes(parent, CatalogItem.Id, CatalogItems);
                    }
                }
            }
        }
        private static void LoadChildNodes(ComboTreeNode ParentNode, long ParentId, IList<CatalogItem> CatalogItems)
        {
            foreach (var CatalogItem in CatalogItems)
            {
                if (CatalogItem.Type != CatalogItemType.PRODUCT && CatalogItem.ParentId == ParentId)
                {
                    if (CatalogItem.Type == CatalogItemType.CATEGORY)
                    {
                        ComboTreeNode ChildNode = ParentNode.Nodes.Add(CatalogItem.Id.ToString(), CatalogItem.Name);
                        LoadChildNodes(ChildNode, CatalogItem.Id, CatalogItems);
                    }

                }
            }
        }
    }
    class FormUtils
    {
        public static void ResetForm(System.Windows.Forms.Control form)
        {
            foreach (System.Windows.Forms.Control control in form.Controls)
            {
                if (control is GroupBox)
                {
                    ResetForm(control);
                }
                else if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    textBox.ResetText();
                }
                else if (control is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)control;
                    comboBox.ResetText();
                }

                else if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.ResetText();
                }

                else if (control is ListBox)
                {
                    ListBox listBox = (ListBox)control;
                    listBox.ResetText();
                }
                else if (control is CheckedListBox)
                {
                    CheckedListBox checkedlistBox = (CheckedListBox)control;
                    checkedlistBox.ResetText();
                }
            }
        }
    }
}
