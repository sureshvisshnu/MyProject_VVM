using ExcelDataReader;
using fa;
using fa.api.Accounting;
using fa.api.Hms;
using fa.model.Common;
using fa.model.Hms.Master;
using Fa.views.controls.hms;
using Fa.views.utils.Hms;
using Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.hms.patient
{
    public partial class FormPatientUpload : Form
    {
        public bool FileUploadComplete;
        public bool PauseProcessing;
        public bool IsBreak = false;
        public FormPatientUpload()
        {
            InitializeComponent();
        }
        private void PatientFileUpload_OnClickUpload(object sender, EventArgs e)
        {
            string FileName = patientFileUpload.FileName();
            bool HasHeader = patientFileUpload.HasHeader();
            string SoftwareFrom = patientFileUpload.SoftwareType();
            bool OverWritePatient = patientFileUpload.overWritePatient();
            bool IsColumnValid = true;
            string[] PatientDetailHeaderFromEquals = new string[] { "PACNAME", "REGNO", "DATE", "SEX", "DOB", "AGE", "GUAR_NAME", "RELATION", "ADDR1", "ADDR2", "ADDR3", "ADDR4" };
            string[] PatientDetailHeaderFromTherapia = new string[] { "PatientName", "PatientNumber", "Gender", "DOB", "Age", "BloodGroup", "Occupation", "TaxId", "Income", "PatientAddress", "PatientState", "PatientPincode", "PhoneNo", "MobileNo", "GaurdianName", "GaurdianGender", "GaurdianDOB", "GaurdianAddress", "GaurdianState", "GaurdianPincode", "GaurdianPhone", "GaurdianMobile", "GaurdianRelationship", "ResponsibleParty", "EmergencyConName", "EmergencyConGender", "EmergencyConAddress", "EmergencyConState", "EmergencyConPincode", "EmergencyConRelation", "EmergencyConPhone", "EmergencyConMobile", "InsuranceName", "GroupNo", "PolicyNo", "Insurer", "InsurerRelation", "InsurerAddress", "InsurerState", "InsurerPincode", "InsurancePhoneNo" };
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
                    patientFileUpload.SetProgressMax(rowcount);
                    int row = 1;
                    do
                    {
                        while (reader.Read())
                        {
                            if (PauseProcessing == true)
                            {
                                this.UseWaitCursor = false;
                                patientFileUpload.AddAccessLog("Paused");
                                string message = "Do you want to exit from FileUpload Processing?";
                                string title = "Exit FileUpload";
                                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                                if (result == DialogResult.Yes)
                                {
                                    patientFileUpload.StopProcessing = true;
                                    this.Invoke((new Action(() => this.Close())));
                                }
                                else
                                {
                                    this.UseWaitCursor = true;
                                    patientFileUpload.AddAccessLog("Continuing");
                                    PauseProcessing = false;
                                    patientFileUpload.StopProcessing = false;
                                    patientFileUpload.EnableExitButton(true);
                                    continue;
                                }
                            }
                            if (patientFileUpload.StopProcessing == false)
                            {
                                int cols = 0;
                                string PatientName = string.Empty;
                                string PatientNumber = string.Empty;
                                string Gender = string.Empty;
                                string DateOfBirth = string.Empty;
                                try
                                {
                                    if (SoftwareFrom == "Equals")
                                    {
                                        string CreatedDate = string.Empty;
                                        double Age = 0;
                                        string GuardianName = string.Empty;
                                        string Relationship = string.Empty;
                                        string Address1 = string.Empty;
                                        string Address2 = string.Empty;
                                        string Address3 = string.Empty;
                                        string Address4 = string.Empty;
                                        int fieldCount = reader.FieldCount;

                                        string[] HeadersFromExcel = null!;
                                        bool allHeadersPresent = true;

                                        if (row == 1)
                                        {
                                            HeadersFromExcel = new string[fieldCount];

                                            for (int col = 1; col <= fieldCount; col++)
                                            {
                                                HeadersFromExcel[col - 1] = reader.GetValue(cols).ToString()!.Trim();
                                                cols++;
                                            }
                                            allHeadersPresent = PatientDetailHeaderFromEquals.All(header => HeadersFromExcel.Contains(header));
                                            if (!allHeadersPresent)
                                            {
                                                FileUploadComplete = false;
                                                IsBreak = true;
                                                patientFileUpload.AddAccessLog(patientFileUpload.FileUploadMismatchColoumn);
                                                Cursor.Current = Cursors.Default;
                                                this.UseWaitCursor = false;
                                                break;
                                            }
                                        }
                                        if (row == 1 && HasHeader)
                                        {
                                            row++;
                                            continue;
                                        }
                                        else if (row == 1 && !HasHeader)
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            patientFileUpload.AddAccessLog(patientFileUpload.CheckBoxUncheckMessage);
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            break;
                                        }
                                        patientFileUpload.IncrementProgress();
                                        PatientName = reader.GetValue(5) != null ? reader.GetValue(5).ToString()!.Trim() : string.Empty;
                                        if (PatientName.Length > 30)
                                        {
                                            PatientName = PatientName.Substring(0, 30);
                                        }
                                        PatientNumber = reader.GetValue(3) != null ? reader.GetValue(3).ToString()!.Trim() : string.Empty;
                                        CreatedDate = reader.GetValue(2) != null ? reader.GetValue(2).ToString()!.Trim() : string.Empty;
                                        Gender = reader.GetValue(16) != null ? reader.GetValue(16).ToString()!.Trim() : string.Empty;
                                        DateOfBirth = reader.GetValue(15) != null ? reader.GetValue(15).ToString()!.Trim() : string.Empty;
                                        Age = !reader.IsDBNull(9) ? reader.GetDouble(9) : 0;
                                        GuardianName = reader.GetValue(8) != null ? reader.GetValue(8).ToString()!.Replace("Mr. ", "Mr.").Trim() : string.Empty;
                                        Relationship = reader.GetValue(10) != null ? reader.GetValue(10).ToString()!.Trim().ToUpper() : string.Empty;
                                        Address1 = reader.GetValue(11) != null ? reader.GetValue(11).ToString()!.Trim() : string.Empty;
                                        Address2 = reader.GetValue(12) != null ? reader.GetValue(12).ToString()!.Trim() : string.Empty;
                                        Address3 = reader.GetValue(13) != null ? reader.GetValue(13).ToString()!.Trim() : string.Empty;
                                        Address4 = reader.GetValue(14) != null ? reader.GetValue(14).ToString()!.Trim() : string.Empty;

                                        if (!string.IsNullOrEmpty(PatientName))
                                        {
                                            List<string> address1 = ProcessExcelCell(Address1);
                                            List<string> address2 = ProcessExcelCell(Address2);
                                            List<string> address3 = ProcessExcelCell(Address3);
                                            List<string> address4 = ProcessExcelCell(Address4);
                                            Address lAddress = new Address();
                                            {
                                                lAddress.AddressLine1 = address1[0].IsNumeric() ? (address1[1].IsNumeric() ? "" : address1[1]) : address1[0];
                                                lAddress.AddressLine2 = address2[0].IsNumeric() ? (address2[1].IsNumeric() ? "" : address2[1]) : address2[0];
                                                lAddress.CityOrTown = address3[0].IsNumeric() ? (address3[1].IsNumeric() ? "" : address3[1]) : address3[0];
                                                lAddress.StatesId = Global.Company.Address.StatesId;
                                            }
                                            ContactInfo lcontactInfo = new ContactInfo();
                                            {
                                                lcontactInfo.Mobile = address1[0].IsNumeric() ? address1[0] : (address1[1].IsNumeric() ? address1[1] : "");
                                                if (string.IsNullOrEmpty(lcontactInfo.Mobile))
                                                {
                                                    lcontactInfo.Mobile = address2[0].IsNumeric() ? address2[0] : (address2[1].IsNumeric() ? address2[1] : "");
                                                }
                                                else
                                                {
                                                    lcontactInfo.Phone = address2[0].IsNumeric() ? address2[0] : (address2[1].IsNumeric() ? address2[1] : "");
                                                }
                                                if (string.IsNullOrEmpty(lcontactInfo.Mobile))
                                                {
                                                    lcontactInfo.Mobile = address3[0].IsNumeric() ? address3[0] : (address3[1].IsNumeric() ? address3[1] : "");
                                                }
                                                else
                                                {
                                                    lcontactInfo.Phone = address3[0].IsNumeric() ? address3[0] : (address3[1].IsNumeric() ? address3[1] : "");
                                                }
                                                if (string.IsNullOrEmpty(lcontactInfo.Mobile))
                                                {
                                                    lcontactInfo.Mobile = address4[0].IsNumeric() ? address4[0] : (address4[1].IsNumeric() ? address4[1] : "");
                                                }
                                                else
                                                {
                                                    lcontactInfo.Phone = address4[0].IsNumeric() ? address4[0] : (address4[1].IsNumeric() ? address4[1] : "");
                                                }
                                            }
                                            string[] splitPatientName = PatientName.Split(' ');
                                            Patient patient = new Patient();
                                            {
                                                patient.Id = 0;
                                                patient.FirstName = splitPatientName[0];
                                                patient.LastName = splitPatientName.Length > 1 ? splitPatientName[1] : (Gender != null ? (Gender == "M" ? "Mr." : "Mrs.") : "Mr.");
                                                patient.PatientNumber = PatientNumber;
                                                patient.CreatedDate = DateTime.Parse(CreatedDate);
                                                patient.Gender = Gender == "M" ? fa.model.Hms.Master.Gender.MALE : fa.model.Hms.Master.Gender.FEMALE;
                                                if (Age > 0)
                                                {
                                                    DateOfBirth = DateTime.Today.AddYears(-(int)Age).ToString();
                                                }
                                                patient.DateOfBirth = DateTime.Parse(DateOfBirth);
                                                patient.Address = lAddress;
                                                patient.ContactInfo = lcontactInfo;
                                                patient.CompanyId = Global.Company.CompanyId;
                                            }
                                            Patient AddOrUpdatePatient = null!;
                                            DateTime PatientDateofBirth = DateTime.TryParse(DateOfBirth, out DateTime result) ? result : DateTime.Now;
                                            if (PatientManager.Instance.CheckPatientUniqueByPatientName(patient.Name, PatientDateofBirth, PatientNumber, Global.Company.CompanyId, Global.Company.DateFormat))
                                            {
                                                AddOrUpdatePatient = PatientManager.Instance.AddPatient(patient);
                                            }
                                            else
                                            {
                                                if (!OverWritePatient)
                                                {
                                                    AddOrUpdatePatient = PatientManager.Instance.AddPatient(patient);
                                                }
                                                else
                                                {
                                                    Patient lpatient = PatientManager.Instance.GetPatientByPatientNumber(patient.Name, PatientDateofBirth, PatientNumber, Global.Company.CompanyId, Global.Company.DateFormat);
                                                    if (lpatient != null)
                                                    {
                                                        patient.Id = lpatient.Id;
                                                        if (lpatient.AddressId > 0)
                                                        {
                                                            lAddress.AddressId = (long)lpatient.AddressId;
                                                            patient.AddressId = AddressManager.Instance.UpdateAddress(lAddress).AddressId;
                                                        }
                                                        patient.ContactInfoId = lpatient.ContactInfoId;
                                                        AddOrUpdatePatient = PatientManager.Instance.UpdatePatient(patient);
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(GuardianName))
                                            {
                                                string[] splitGuardianName = GuardianName.Split(' ');
                                                Guardian lGuardian = new Guardian();
                                                {
                                                    lGuardian.Id = 0;
                                                    lGuardian.PatientId = AddOrUpdatePatient.Id;
                                                    lGuardian.FirstName = splitGuardianName[0];
                                                    lGuardian.LastName = splitGuardianName.Length > 1 ? (!string.IsNullOrEmpty(splitGuardianName[1]) ? (splitGuardianName.Length > 2 ? (!string.IsNullOrEmpty(splitGuardianName[2]) ? (splitGuardianName[1] + " " + splitGuardianName[2]) : splitGuardianName[1]) : splitGuardianName[1]) : "Mr.") : "Mr.";
                                                    lGuardian.RelationShip = !string.IsNullOrEmpty(Relationship) ? (Relationship == "MOTHER" ? RelationShip.MOTHER : Relationship == "FATHER" ? RelationShip.FATHER : Relationship == "GRANDPARENT" ? RelationShip.GRANDPARENT : Relationship == "GAURDIAN" ? RelationShip.GAURDIAN : Relationship == "FRIEND" ? RelationShip.FRIEND : Relationship == "SELF" ? RelationShip.SELF : Relationship == "HUSBAND" ? RelationShip.HUSBAND : RelationShip.SPOUSE) : RelationShip.GAURDIAN;
                                                    lGuardian.Address = lAddress;
                                                    lGuardian.ContactInfo = lcontactInfo;
                                                    lGuardian.CompanyId = Global.Company.CompanyId;
                                                }
                                                Guardian AddOrUpdateGuardian = null!;
                                                if (GuardianManager.Instance.CheckGuardiantuniqueByName(lGuardian.Name, AddOrUpdatePatient.Id, lGuardian.RelationShip, Global.Company.CompanyId))
                                                {
                                                    lGuardian.Address.AddressId = 0;
                                                    lGuardian.ContactInfo.Id = 0;
                                                    AddOrUpdateGuardian = GuardianManager.Instance.AddGuardian(lGuardian);
                                                }
                                                else
                                                {
                                                    Guardian guardian = GuardianManager.Instance.GetGuardianByName(lGuardian.Name, AddOrUpdatePatient.Id, lGuardian.RelationShip, Global.Company.CompanyId);
                                                    lGuardian.Id = guardian.Id;
                                                    if (guardian.AddressId > 0)
                                                    {
                                                        lAddress.AddressId = (long)guardian.AddressId;
                                                        lGuardian.AddressId = AddressManager.Instance.UpdateAddress(lAddress).AddressId;
                                                    }
                                                    lGuardian.ContactInfoId = guardian.ContactInfoId;
                                                    AddOrUpdateGuardian = GuardianManager.Instance.UpdateGuardian(lGuardian);
                                                }
                                            }
                                            if (HasHeader)
                                            {
                                                patientFileUpload.AddAccessLog("Processing Row No : " + row + " - " + PatientName);
                                                patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                            }
                                            else
                                            {
                                                patientFileUpload.AddAccessLog("Processing Row No : " + (row - 1) + " - " + PatientName);
                                                patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));                                         //FileUploadMedicalTest.AddTimingLog(stopw.Elapsed.ToString());
                                            }
                                        }
                                        else
                                        {
                                            if (HasHeader)
                                            {
                                                patientFileUpload.AddErrorLog("Row No - " + row + " Empty Name");
                                                patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                            }
                                            else
                                            {
                                                patientFileUpload.AddErrorLog("Row No - " + (row - 1) + " Empty Name");
                                                patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                            }
                                        }
                                    }
                                    else if (SoftwareFrom == "None")
                                    {
                                        if (PatientDetailHeaderFromTherapia.Length == reader.FieldCount)
                                        {
                                            if (row == 1 && HasHeader)
                                            {
                                                foreach (string sh in PatientDetailHeaderFromTherapia)
                                                {
                                                    if (sh.Trim() != reader.GetValue(cols).ToString()!.Trim())
                                                    {
                                                        IsColumnValid = false;
                                                    }
                                                    cols++;
                                                }
                                                row++;
                                                continue;
                                            }
                                            else if (row == 1 && !HasHeader)
                                            {
                                                foreach (string sh in PatientDetailHeaderFromTherapia)
                                                {
                                                    var value = reader.GetValue(cols);
                                                    if (sh == value.ToString())
                                                    {
                                                        IsColumnValid = false;
                                                        cols++;
                                                    }
                                                    else
                                                    {
                                                        IsColumnValid = true;
                                                        break;
                                                    }
                                                }
                                                if (!IsColumnValid)
                                                {
                                                    patientFileUpload.AddErrorLog("Row No - " + row + ": Please check the file have headers");
                                                    IsColumnValid = true;
                                                    row++;
                                                    continue;
                                                }
                                            }
                                            if (IsColumnValid)
                                            {
                                                patientFileUpload.IncrementProgress();
                                                PatientName = reader.GetValue(0) != null ? reader.GetValue(0).ToString()!.Trim() : string.Empty;
                                                if (PatientName.Length > 30)
                                                {
                                                    PatientName = PatientName.Substring(0, 30);
                                                }
                                                PatientNumber = reader.GetValue(1) != null ? reader.GetValue(1).ToString()!.Trim() : string.Empty;
                                                Gender = reader.GetValue(2) != null ? reader.GetValue(2).ToString()!.Trim().ToUpper() : string.Empty;
                                                DateOfBirth = reader.GetValue(3) != null ? reader.GetValue(3).ToString()!.Trim() : string.Empty;
                                                var Page = reader.GetValue(4) != null ? reader.GetValue(4).ToString()!.Trim() : string.Empty;
                                                int PatientAge = int.TryParse(Page, out int age) ? age : 0;
                                                string BloodGroup = reader.GetValue(5) != null ? reader.GetValue(5).ToString()!.Trim() : string.Empty;
                                                string Occupation = reader.GetValue(6) != null ? reader.GetValue(6).ToString()!.Trim() : string.Empty;
                                                string TaxId = reader.GetValue(7) != null ? reader.GetValue(7).ToString()!.Trim() : string.Empty;
                                                string Income = reader.GetValue(8) != null ? reader.GetValue(8).ToString()!.Trim() : string.Empty;
                                                string PatientAddress = reader.GetValue(9) != null ? reader.GetValue(9).ToString()!.Trim() : string.Empty;
                                                string PatientState = reader.GetValue(10) != null ? reader.GetValue(10).ToString()!.Trim() : string.Empty;
                                                string PatientPincode = reader.GetValue(11) != null ? reader.GetValue(11).ToString()!.Trim() : string.Empty;
                                                string PhoneNo = reader.GetValue(12) != null ? reader.GetValue(12).ToString()!.Trim() : string.Empty;
                                                string MobileNo = reader.GetValue(13) != null ? reader.GetValue(13).ToString()!.Trim() : string.Empty;

                                                string GaurdianName = reader.GetValue(14) != null ? reader.GetValue(14).ToString()!.Trim() : string.Empty;
                                                string GaurdianGender = reader.GetValue(15) != null ? reader.GetValue(15).ToString()!.Trim() : string.Empty;
                                                string GaurdianDOB = reader.GetValue(16) != null ? reader.GetValue(16).ToString()!.Trim() : string.Empty;
                                                string GaurdianAddress = reader.GetValue(17) != null ? reader.GetValue(17).ToString()!.Trim() : string.Empty;
                                                string GaurdianState = reader.GetValue(18) != null ? reader.GetValue(18).ToString()!.Trim() : string.Empty;
                                                string GaurdianPincode = reader.GetValue(19) != null ? reader.GetValue(19).ToString()!.Trim() : string.Empty;
                                                string GaurdianPhone = reader.GetValue(20) != null ? reader.GetValue(20).ToString()!.Trim() : string.Empty;
                                                string GaurdianMoblie = reader.GetValue(21) != null ? reader.GetValue(21).ToString()!.Trim() : string.Empty;
                                                string GaurdianRelationship = reader.GetValue(22) != null ? reader.GetValue(22).ToString()!.Trim() : string.Empty;
                                                string ResponsibleParty = reader.GetValue(23) != null ? reader.GetValue(23).ToString()!.Trim() : string.Empty;

                                                string EmergencyConName = reader.GetValue(24) != null ? reader.GetValue(24).ToString()!.Trim() : string.Empty;
                                                string EmergencyConGender = reader.GetValue(25) != null ? reader.GetValue(25).ToString()!.Trim() : string.Empty;
                                                string EmergencyConAddress = reader.GetValue(26) != null ? reader.GetValue(26).ToString()!.Trim() : string.Empty;
                                                string EmergencyConState = reader.GetValue(27) != null ? reader.GetValue(27).ToString()!.Trim() : string.Empty;
                                                string EmergencyConPincode = reader.GetValue(28) != null ? reader.GetValue(28).ToString()!.Trim() : string.Empty;
                                                string EmergencyConRelation = reader.GetValue(29) != null ? reader.GetValue(29).ToString()!.Trim() : string.Empty;
                                                string EmergencyConPhone = reader.GetValue(30) != null ? reader.GetValue(30).ToString()!.Trim() : string.Empty;
                                                string EmergencyConMobile = reader.GetValue(31) != null ? reader.GetValue(31).ToString()!.Trim() : string.Empty;

                                                string InsuranceName = reader.GetValue(32) != null ? reader.GetValue(32).ToString()!.Trim() : string.Empty;
                                                string GroupNo = reader.GetValue(33) != null ? reader.GetValue(33).ToString()!.Trim() : string.Empty;
                                                string PolicyNo = reader.GetValue(34) != null ? reader.GetValue(34).ToString()!.Trim() : string.Empty;
                                                string Insurer = reader.GetValue(35) != null ? reader.GetValue(35).ToString()!.Trim() : string.Empty;
                                                string InsurerRelation = reader.GetValue(36) != null ? reader.GetValue(36).ToString()!.Trim() : string.Empty;
                                                string InsurerAddress = reader.GetValue(37) != null ? reader.GetValue(37).ToString()!.Trim() : string.Empty;
                                                string InsurerState = reader.GetValue(38) != null ? reader.GetValue(38).ToString()!.Trim() : string.Empty;
                                                string InsurerPincode = reader.GetValue(39) != null ? reader.GetValue(39).ToString()!.Trim() : string.Empty;
                                                string InsurerPhone = reader.GetValue(40) != null ? reader.GetValue(40).ToString()!.Trim() : string.Empty;

                                                if (!string.IsNullOrEmpty(PatientName))
                                                {
                                                    string[] PAddress = PatientAddress.Split(',');
                                                    Address lAddress = new Address();
                                                    {
                                                        long? stateId = StateManager.Instance.GetStateByName(PatientState, Global.Company.CountryId);

                                                        int length = PAddress.Length;

                                                        if (length > 0)
                                                        {
                                                            if (length > 4)
                                                            {
                                                                lAddress.AddressLine1 = string.Join(", ", PAddress.Take(length - 3)).Trim();
                                                                lAddress.AddressLine2 = PAddress[length - 3]?.Trim() ?? string.Empty;
                                                                lAddress.CityOrTown = PAddress[length - 2]?.Trim() ?? string.Empty;
                                                                lAddress.District = PAddress[length - 1]?.Trim() ?? string.Empty;
                                                            }
                                                            else
                                                            {
                                                                lAddress.AddressLine1 = PAddress.ElementAtOrDefault(0)?.Trim() ?? string.Empty;
                                                                lAddress.AddressLine2 = PAddress.ElementAtOrDefault(1)?.Trim() ?? string.Empty;
                                                                lAddress.CityOrTown = PAddress.ElementAtOrDefault(2)?.Trim() ?? string.Empty;
                                                                lAddress.District = PAddress.ElementAtOrDefault(3)?.Trim() ?? string.Empty;
                                                            }
                                                        }
                                                        lAddress.PinCode = (!string.IsNullOrEmpty(PatientPincode) && PatientPincode.IsNumeric()) ? PatientPincode : string.Empty;
                                                        lAddress.StatesId = stateId != null ? stateId : Global.Company.Address.StatesId;
                                                    }
                                                    ContactInfo PcontactInfo = new ContactInfo();
                                                    {
                                                        PcontactInfo.Phone = PhoneNo;
                                                        PcontactInfo.Mobile = MobileNo;
                                                    }
                                                    string[] splitPatientName = PatientName.Split(' ');
                                                    Patient ThrPatient = new Patient();
                                                    {
                                                        ThrPatient.Id = 0;
                                                        ThrPatient.FirstName = splitPatientName.ElementAtOrDefault(0) ?? string.Empty;
                                                        if (splitPatientName.Length > 1)
                                                        {
                                                            if (splitPatientName[1].Length == 1)
                                                            {
                                                                ThrPatient.MiddleInitial = splitPatientName[1];
                                                                ThrPatient.LastName = string.Join(" ", splitPatientName.Skip(2));
                                                            }
                                                            else
                                                            {
                                                                ThrPatient.MiddleInitial = string.Empty;
                                                                ThrPatient.LastName = string.Join(" ", splitPatientName.Skip(1));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ThrPatient.MiddleInitial = string.Empty;
                                                            ThrPatient.LastName = Gender == "MALE" ? "Mr. " : (Gender == "FEMALE" ? "Mrs. " : (Gender == "M" ? "Mr. " : (Gender == "F" ? "Mrs. " : "Mr. ")));
                                                        }
                                                        ThrPatient.PatientNumber = PatientNumber;
                                                        ThrPatient.Gender = Gender == "MALE" ? fa.model.Hms.Master.Gender.MALE : (Gender == "FEMALE" ? fa.model.Hms.Master.Gender.FEMALE : (Gender == "M" ? fa.model.Hms.Master.Gender.MALE : (Gender == "F" ? fa.model.Hms.Master.Gender.FEMALE : fa.model.Hms.Master.Gender.OTHERS)));
                                                        if (PatientAge > 0)
                                                        {
                                                            if (string.IsNullOrEmpty(DateOfBirth))
                                                            {
                                                                DateOfBirth = DateTime.Today.AddYears(-(int)PatientAge).ToString();
                                                            }
                                                        }
                                                        ThrPatient.DateOfBirth = DateTime.TryParse(DateOfBirth, out DateTime DOB) ? DOB : DateTime.Now.Date;
                                                        ThrPatient.Bloodgroup = BloodGroup;
                                                        ThrPatient.Occupation = Occupation;
                                                        ThrPatient.TaxId = TaxId;
                                                        ThrPatient.Income = Income;
                                                        ThrPatient.Address = lAddress;
                                                        ThrPatient.ContactInfo = PcontactInfo;
                                                        ThrPatient.CompanyId = Global.Company.CompanyId;

                                                        Patient AddOrUpdatePatient = null!;
                                                        DateTime PatientDateofBirth = DateTime.TryParse(DateOfBirth, out DateTime result) ? result : DateTime.Now;
                                                        if (PatientManager.Instance.CheckPatientUniqueByPatientName(ThrPatient.Name, PatientDateofBirth, PatientNumber, Global.Company.CompanyId, Global.Company.DateFormat))
                                                        {
                                                            ThrPatient.Address.AddressId = 0;
                                                            ThrPatient.ContactInfo.Id = 0;
                                                            AddOrUpdatePatient = PatientManager.Instance.AddPatient(ThrPatient);
                                                        }
                                                        else
                                                        {
                                                            if (!OverWritePatient)
                                                            {
                                                                AddOrUpdatePatient = PatientManager.Instance.AddPatient(ThrPatient);
                                                            }
                                                            else
                                                            {
                                                                Patient lpatient = PatientManager.Instance.GetPatientByPatientNumber(ThrPatient.Name, PatientDateofBirth, PatientNumber, Global.Company.CompanyId, Global.Company.DateFormat);
                                                                if (lpatient != null)
                                                                {
                                                                    if (lpatient.AddressId > 0)
                                                                    {
                                                                        lAddress.AddressId = (long)lpatient.AddressId;
                                                                        ThrPatient.AddressId = AddressManager.Instance.UpdateAddress(lAddress).AddressId;
                                                                    }
                                                                    ThrPatient.Id = lpatient.Id;
                                                                    ThrPatient.ContactInfoId = lpatient.ContactInfoId;
                                                                    AddOrUpdatePatient = PatientManager.Instance.UpdatePatient(ThrPatient);
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(GaurdianName))
                                                        {
                                                            string[] splitGuardianName = GaurdianName.Split(' ');
                                                            Guardian lGuardian = new Guardian();
                                                            {
                                                                lGuardian.Id = 0;
                                                                lGuardian.PatientId = AddOrUpdatePatient.Id;
                                                                lGuardian.FirstName = splitGuardianName.ElementAtOrDefault(0) ?? string.Empty;
                                                                if (splitGuardianName.Length > 1)
                                                                {
                                                                    if (splitGuardianName[1].Length == 1)
                                                                    {
                                                                        lGuardian.MiddleInitial = splitGuardianName[1];
                                                                        lGuardian.LastName = string.Join(" ", splitGuardianName.Skip(2));
                                                                    }
                                                                    else
                                                                    {
                                                                        lGuardian.MiddleInitial = string.Empty;
                                                                        lGuardian.LastName = string.Join(" ", splitGuardianName.Skip(1));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lGuardian.MiddleInitial = "";
                                                                    lGuardian.LastName = !string.IsNullOrEmpty(GaurdianGender) ? (GaurdianGender == "MALE" ? "Mr. " : (GaurdianGender == "FEMALE" ? "Mrs. " : (GaurdianGender == "M" ? "Mr. " : (GaurdianGender == "F" ? "Mrs. " : "Mr. ")))) : "Mr.";
                                                                }
                                                                lGuardian.Gender = !string.IsNullOrEmpty(GaurdianGender) ? (GaurdianGender == "MALE" ? fa.model.Hms.Master.Gender.MALE : (GaurdianGender == "FEMALE" ? fa.model.Hms.Master.Gender.FEMALE : (GaurdianGender == "M" ? fa.model.Hms.Master.Gender.MALE : (GaurdianGender == "F" ? fa.model.Hms.Master.Gender.FEMALE : fa.model.Hms.Master.Gender.OTHERS)))) : fa.model.Hms.Master.Gender.MALE;
                                                                lGuardian.DateOfBirth = DateTime.TryParse(GaurdianDOB, out DateTime GDOB) ? GDOB : DateTime.Now.Date;
                                                                string[] GAddress = GaurdianAddress.Split(',');
                                                                Address GaurdAddress = new Address();
                                                                {
                                                                    long? stateId = StateManager.Instance.GetStateByName(GaurdianState, Global.Company.CountryId);

                                                                    int length = GAddress.Length;
                                                                    if (length > 0)
                                                                    {
                                                                        if (length > 4)
                                                                        {
                                                                            GaurdAddress.AddressLine1 = string.Join(", ", GAddress.Take(length - 3)).Trim();
                                                                            GaurdAddress.AddressLine2 = GAddress[length - 3]?.Trim() ?? string.Empty;
                                                                            GaurdAddress.CityOrTown = GAddress[length - 2]?.Trim() ?? string.Empty;
                                                                            GaurdAddress.District = GAddress[length - 1]?.Trim() ?? string.Empty;
                                                                        }
                                                                        else
                                                                        {
                                                                            GaurdAddress.AddressLine1 = GAddress.ElementAtOrDefault(0)?.Trim() ?? string.Empty;
                                                                            GaurdAddress.AddressLine2 = GAddress.ElementAtOrDefault(1)?.Trim() ?? string.Empty;
                                                                            GaurdAddress.CityOrTown = GAddress.ElementAtOrDefault(2)?.Trim() ?? string.Empty;
                                                                            GaurdAddress.District = GAddress.ElementAtOrDefault(3)?.Trim() ?? string.Empty;
                                                                        }
                                                                    }
                                                                    GaurdAddress.PinCode = (!string.IsNullOrEmpty(GaurdianPincode) && GaurdianPincode.IsNumeric()) ? GaurdianPincode : string.Empty;
                                                                    GaurdAddress.StatesId = stateId != null ? stateId : Global.Company.Address.StatesId;
                                                                }
                                                                ContactInfo GcontactInfo = new ContactInfo();
                                                                {
                                                                    GcontactInfo.Phone = GaurdianPhone;
                                                                    GcontactInfo.Mobile = GaurdianMoblie;
                                                                }
                                                                lGuardian.Address = GaurdAddress;
                                                                lGuardian.ContactInfo = GcontactInfo;
                                                                lGuardian.RelationShip = !string.IsNullOrEmpty(GaurdianRelationship) ? (GaurdianRelationship == "MOTHER" ? RelationShip.MOTHER : GaurdianRelationship == "FATHER" ? RelationShip.FATHER : GaurdianRelationship == "GRANDPARENT" ? RelationShip.GRANDPARENT : GaurdianRelationship == "GAURDIAN" ? RelationShip.GAURDIAN : GaurdianRelationship == "FRIEND" ? RelationShip.FRIEND : GaurdianRelationship == "SELF" ? RelationShip.SELF : GaurdianRelationship == "HUSBAND" ? RelationShip.HUSBAND : RelationShip.SPOUSE) : RelationShip.GAURDIAN;
                                                                lGuardian.CompanyId = Global.Company.CompanyId;

                                                                Guardian AddOrUpdateGuardian = null!;
                                                                if (GuardianManager.Instance.CheckGuardiantuniqueByName(lGuardian.Name, AddOrUpdatePatient.Id, lGuardian.RelationShip, Global.Company.CompanyId))
                                                                {
                                                                    lGuardian.Address.AddressId = 0;
                                                                    lGuardian.ContactInfo.Id = 0;
                                                                    AddOrUpdateGuardian = GuardianManager.Instance.AddGuardian(lGuardian);
                                                                }
                                                                else
                                                                {
                                                                    Guardian guardian = GuardianManager.Instance.GetGuardianByName(lGuardian.Name, AddOrUpdatePatient.Id, lGuardian.RelationShip, Global.Company.CompanyId);
                                                                    lGuardian.Id = guardian.Id;
                                                                    if (guardian.AddressId > 0)
                                                                    {
                                                                        GaurdAddress.AddressId = (long)guardian.AddressId;
                                                                        lGuardian.AddressId = AddressManager.Instance.UpdateAddress(GaurdAddress).AddressId;
                                                                    }
                                                                    lGuardian.ContactInfoId = guardian.ContactInfoId;
                                                                    AddOrUpdateGuardian = GuardianManager.Instance.UpdateGuardian(lGuardian);
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(EmergencyConName))
                                                        {
                                                            string[] splitEmergencyConName = EmergencyConName.Split(' ');
                                                            EmergencyContact lemergencyContact = new EmergencyContact();
                                                            {
                                                                lemergencyContact.Id = 0;
                                                                lemergencyContact.PatientId = AddOrUpdatePatient.Id;
                                                                lemergencyContact.FirstName = splitEmergencyConName.ElementAtOrDefault(0) ?? string.Empty;
                                                                if (splitEmergencyConName.Length > 1)
                                                                {
                                                                    if (splitEmergencyConName[1].Length == 1)
                                                                    {
                                                                        lemergencyContact.MiddleInitial = splitEmergencyConName[1];
                                                                        lemergencyContact.LastName = string.Join(" ", splitEmergencyConName.Skip(2));
                                                                    }
                                                                    else
                                                                    {
                                                                        lemergencyContact.MiddleInitial = string.Empty;
                                                                        lemergencyContact.LastName = string.Join(" ", splitEmergencyConName.Skip(1));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    lemergencyContact.MiddleInitial = "";
                                                                    lemergencyContact.LastName = !string.IsNullOrEmpty(EmergencyConGender) ? (EmergencyConGender == "MALE" ? "Mr. " : (EmergencyConGender == "FEMALE" ? "Mrs. " : (EmergencyConGender == "M" ? "Mr. " : (EmergencyConGender == "F" ? "Mrs. " : "Mr. ")))) : "Mr.";
                                                                }
                                                                lemergencyContact.Gender = EmergencyConGender == "MALE" ? fa.model.Hms.Master.Gender.MALE : (EmergencyConGender == "FEMALE" ? fa.model.Hms.Master.Gender.FEMALE : (EmergencyConGender == "M" ? fa.model.Hms.Master.Gender.MALE : (EmergencyConGender == "F" ? fa.model.Hms.Master.Gender.FEMALE : fa.model.Hms.Master.Gender.OTHERS)));
                                                                string[] EAddress = EmergencyConAddress.Split(',');
                                                                Address lEmergAddress = new Address();
                                                                {
                                                                    long? stateId = StateManager.Instance.GetStateByName(EmergencyConState, Global.Company.CountryId);

                                                                    int length = EAddress.Length;
                                                                    if (length > 0)
                                                                    {
                                                                        if (length > 4)
                                                                        {
                                                                            lEmergAddress.AddressLine1 = string.Join(", ", EAddress.Take(length - 3)).Trim();
                                                                            lEmergAddress.AddressLine2 = EAddress[length - 3]?.Trim() ?? string.Empty;
                                                                            lEmergAddress.CityOrTown = EAddress[length - 2]?.Trim() ?? string.Empty;
                                                                            lEmergAddress.District = EAddress[length - 1]?.Trim() ?? string.Empty;
                                                                        }
                                                                        else
                                                                        {
                                                                            lEmergAddress.AddressLine1 = EAddress.ElementAtOrDefault(0)?.Trim() ?? string.Empty;
                                                                            lEmergAddress.AddressLine2 = EAddress.ElementAtOrDefault(1)?.Trim() ?? string.Empty;
                                                                            lEmergAddress.CityOrTown = EAddress.ElementAtOrDefault(2)?.Trim() ?? string.Empty;
                                                                            lEmergAddress.District = EAddress.ElementAtOrDefault(3)?.Trim() ?? string.Empty;
                                                                        }
                                                                    }
                                                                    lEmergAddress.PinCode = (!string.IsNullOrEmpty(EmergencyConPincode) && EmergencyConPincode.IsNumeric()) ? EmergencyConPincode : string.Empty;
                                                                    lEmergAddress.StatesId = stateId != null ? stateId : Global.Company.Address.StatesId;
                                                                }
                                                                ContactInfo EcontactInfo = new ContactInfo();
                                                                {
                                                                    EcontactInfo.Phone = EmergencyConPhone;
                                                                    EcontactInfo.Mobile = EmergencyConMobile;
                                                                }
                                                                lemergencyContact.Address = lEmergAddress;
                                                                lemergencyContact.ContactInfo = EcontactInfo;
                                                                lemergencyContact.RelationShip = !string.IsNullOrEmpty(EmergencyConRelation) ? (EmergencyConRelation == "MOTHER" ? RelationShip.MOTHER : EmergencyConRelation == "FATHER" ? RelationShip.FATHER : EmergencyConRelation == "GRANDPARENT" ? RelationShip.GRANDPARENT : EmergencyConRelation == "GAURDIAN" ? RelationShip.GAURDIAN : EmergencyConRelation == "FRIEND" ? RelationShip.FRIEND : EmergencyConRelation == "SELF" ? RelationShip.SELF : GaurdianRelationship == "HUSBAND" ? RelationShip.HUSBAND : RelationShip.SPOUSE) : RelationShip.GAURDIAN;
                                                                lemergencyContact.CompanyId = Global.Company.CompanyId;

                                                                EmergencyContact AddOrUpdateEmergencyContact = null!;
                                                                if (EmergencyContactManager.Instance.CheckEmergencyContactUniqueByName(lemergencyContact.Name, AddOrUpdatePatient.Id, lemergencyContact.RelationShip, Global.Company.CompanyId))
                                                                {
                                                                    lemergencyContact.Address.AddressId = 0;
                                                                    lemergencyContact.ContactInfo.Id = 0;
                                                                    AddOrUpdateEmergencyContact = EmergencyContactManager.Instance.AddEmergencyContact(lemergencyContact);
                                                                }
                                                                else
                                                                {
                                                                    EmergencyContact lemergencyContactFromDB = EmergencyContactManager.Instance.GetEmergencyContactByName(lemergencyContact.Name, AddOrUpdatePatient.Id, lemergencyContact.RelationShip, Global.Company.CompanyId);
                                                                    lemergencyContact.Id = lemergencyContactFromDB.Id;
                                                                    if (lemergencyContactFromDB.AddressId > 0)
                                                                    {
                                                                        lEmergAddress.AddressId = (long)lemergencyContactFromDB.AddressId;
                                                                        lemergencyContact.AddressId = AddressManager.Instance.UpdateAddress(lEmergAddress).AddressId;
                                                                    }
                                                                    lemergencyContact.ContactInfoId = lemergencyContactFromDB.ContactInfoId;
                                                                    AddOrUpdateEmergencyContact = EmergencyContactManager.Instance.UpdateEmergencyContact(lemergencyContact);
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(InsuranceName) && !string.IsNullOrEmpty(PolicyNo))
                                                        {
                                                            long? InsuranceHolderId = PersonManager.Instance.GetInsuranceHolderInfoByName(Insurer, Global.Company.CompanyId);
                                                            InsuranceInfo insuranceInfo = new InsuranceInfo();
                                                            {
                                                                insuranceInfo.InsuranceName = InsuranceName;
                                                                insuranceInfo.PatientId = AddOrUpdatePatient.Id;
                                                                insuranceInfo.GroupNumber = GroupNo;
                                                                insuranceInfo.PolicyNumber = PolicyNo;
                                                                insuranceInfo.IsPrimary = true;
                                                                insuranceInfo.IsActive = true;
                                                                if (InsuranceHolderId != null && InsuranceHolderId > 0)
                                                                {
                                                                    insuranceInfo.InsuranceHolderId = (long)InsuranceHolderId!;
                                                                }

                                                                string[] IAddress = InsurerAddress.Split(',');
                                                                Address InsureAddress = new Address();
                                                                {
                                                                    long? stateId = StateManager.Instance.GetStateByName(InsurerState, Global.Company.CountryId);

                                                                    int length = IAddress.Length;
                                                                    if (length > 0)
                                                                    {
                                                                        if (length > 4)
                                                                        {
                                                                            InsureAddress.AddressLine1 = string.Join(", ", IAddress.Take(length - 3)).Trim();
                                                                            InsureAddress.AddressLine2 = IAddress[length - 3]?.Trim() ?? string.Empty;
                                                                            InsureAddress.CityOrTown = IAddress[length - 2]?.Trim() ?? string.Empty;
                                                                            InsureAddress.District = IAddress[length - 1]?.Trim() ?? string.Empty;
                                                                        }
                                                                        else
                                                                        {
                                                                            InsureAddress.AddressLine1 = IAddress.ElementAtOrDefault(0)?.Trim() ?? string.Empty;
                                                                            InsureAddress.AddressLine2 = IAddress.ElementAtOrDefault(1)?.Trim() ?? string.Empty;
                                                                            InsureAddress.CityOrTown = IAddress.ElementAtOrDefault(2)?.Trim() ?? string.Empty;
                                                                            InsureAddress.District = IAddress.ElementAtOrDefault(3)?.Trim() ?? string.Empty;
                                                                        }
                                                                    }
                                                                    InsureAddress.PinCode = (!string.IsNullOrEmpty(InsurerPincode) && InsurerPincode.IsNumeric()) ? InsurerPincode : string.Empty;
                                                                    InsureAddress.StatesId = stateId != null ? stateId : Global.Company.Address.StatesId;
                                                                }
                                                                ContactInfo InsureContactInfo = new ContactInfo();
                                                                {
                                                                    InsureContactInfo.Phone = InsurerPhone;
                                                                }
                                                                insuranceInfo.EmployerAddress = InsureAddress;
                                                                insuranceInfo.EmployerContactInfo = InsureContactInfo;

                                                                insuranceInfo.InsuranceHolderRelationShip = !string.IsNullOrEmpty(InsurerRelation) ? (InsurerRelation == "MOTHER" ? RelationShip.MOTHER : InsurerRelation == "FATHER" ? RelationShip.FATHER : InsurerRelation == "GRANDPARENT" ? RelationShip.GRANDPARENT : InsurerRelation == "GAURDIAN" ? RelationShip.GAURDIAN : InsurerRelation == "FRIEND" ? RelationShip.FRIEND : InsurerRelation == "SELF" ? RelationShip.SELF : GaurdianRelationship == "HUSBAND" ? RelationShip.HUSBAND : RelationShip.SPOUSE) : RelationShip.GAURDIAN;
                                                                insuranceInfo.CompanyId = Global.Company.CompanyId;

                                                                InsuranceInfo AddOrUpdateInsuranceInfo = null!;
                                                                if (InsuranceInfoManager.Instance.CheckInsuranceInfoUniqueByName(InsuranceName, PolicyNo, AddOrUpdatePatient.Id, Global.Company.CompanyId))
                                                                {
                                                                    AddOrUpdateInsuranceInfo = InsuranceInfoManager.Instance.AddInsuranceInfo(insuranceInfo);
                                                                }
                                                                else
                                                                {
                                                                    InsuranceInfo lInsuranceInfoFromDB = InsuranceInfoManager.Instance.GetInsuranceInfoByName(InsuranceName, PolicyNo, AddOrUpdatePatient.Id, Global.Company.CompanyId);
                                                                    insuranceInfo.Id = lInsuranceInfoFromDB.Id;
                                                                    if (lInsuranceInfoFromDB.EmployerAddressId > 0)
                                                                    {
                                                                        InsureAddress.AddressId = (long)lInsuranceInfoFromDB.EmployerAddressId;
                                                                        insuranceInfo.EmployerAddressId = AddressManager.Instance.UpdateAddress(InsureAddress).AddressId;
                                                                    }
                                                                    insuranceInfo.EmployerContactInfoId = lInsuranceInfoFromDB.EmployerContactInfoId;
                                                                    AddOrUpdateInsuranceInfo = InsuranceInfoManager.Instance.UpdateInsuranceInfo(insuranceInfo);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (HasHeader)
                                                    {
                                                        patientFileUpload.AddAccessLog("Processing Row No : " + row + " - " + PatientName);
                                                        patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else
                                                    {
                                                        patientFileUpload.AddAccessLog("Processing Row No : " + (row - 1) + " - " + PatientName);
                                                        patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                                else
                                                {
                                                    if (HasHeader)
                                                    {
                                                        patientFileUpload.AddErrorLog("Row No - " + row + " Empty Name");
                                                        patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                    else
                                                    {
                                                        patientFileUpload.AddErrorLog("Row No - " + (row - 1) + " Empty Name");
                                                        patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            FileUploadComplete = false;
                                            IsBreak = true;
                                            patientFileUpload.AddAccessLog(patientFileUpload.FileUploadMismatchColoumn);
                                            Cursor.Current = Cursors.Default;
                                            this.UseWaitCursor = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        FileUploadComplete = false;
                                        IsBreak = true;
                                        patientFileUpload.AddAccessLog(patientFileUpload.SoftwareMisMatchMessage);
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
                                        patientFileUpload.AddAccessLog("Cancelled");
                                        Cursor.Current = Cursors.Default;
                                        this.UseWaitCursor = false;//from the Form/Window instance
                                        throw new IndexOutOfRangeException("-2146233080", ex);
                                    }
                                    else
                                    {
                                        if (HasHeader)
                                        {
                                            patientFileUpload.AddErrorLog("Row No : " + row + " - " + PatientName + " : " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                                            patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                        else
                                        {
                                            patientFileUpload.AddErrorLog("Row No : " + (row - 1) + " - " + PatientName + " : " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                                            patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                                        }
                                    }
                                }
                                row++;
                            }
                        }
                    }
                    while (reader.NextResult());
                    if (IsBreak == false) { patientFileUpload.CompleteIncrementProgress(row); } else { patientFileUpload.CompleteIncrementProgress(0); }
                    patientFileUpload.CompleteIncrementProgress(row);
                    patientFileUpload.AddTimingLog(stopw.Elapsed.Hours.ToString("00") + "." + stopw.Elapsed.Minutes.ToString("00") + "." + stopw.Elapsed.Seconds.ToString("00"));
                    stopw.Stop();
                    if (patientFileUpload.StopProcessing == true)
                    {
                        patientFileUpload.AddAccessLog("Cancelled");
                    }
                    else
                    {
                        patientFileUpload.AddAccessLog("Finished");
                    }
                    this.UseWaitCursor = false;
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public List<string> ProcessExcelCell(string cellValue)
        {
            List<string> address = new List<string>();
            string phonePattern = @"\b\d{10,}\b";
            var regex = new Regex(phonePattern);

            var match = regex.Match(cellValue);
            if (match.Success)
            {
                address.Add(match.Value.ToString());
                address.Add(cellValue.Replace(match.Value, "").Trim());
            }
            else
            {
                address.Add(cellValue.Trim());
                address.Add(string.Empty);
            }
            return address;
        }

        private void ExitFileUploadProcess()
        {
            if (patientFileUpload.WorkFlow == true)
            {
                patientFileUpload.EnableExitButton(false);
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
            if (patientFileUpload.WorkFlow == true)
            {
                patientFileUpload.ExitFileUpload();
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void patientFileUpload_OnClickDownload(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PatientExportFile patientExportFile = new PatientExportFile();
            patientExportFile.GenerateFile(Global.Company.CompanyId);
        }
    }
}
