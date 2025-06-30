using fa;
using fa.api.Hms;
using fa.api.utils;
using fa.model.Common;
using fa.model.Hms.Master;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.views.utils.Hms
{
    public class PatientExportFile
    {
        public void GenerateFile(long CompanyId)
        {
            Cursor.Current = Cursors.WaitCursor;
            int i = 0;

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Patient Data");

            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            hStyle.FillPattern = FillPattern.SolidForeground;
            hStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
            hStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;

            IRow headerRow = sheet.CreateRow(i++);
            headerRow.CreateCell(0).SetCellValue("PatientName");
            headerRow.CreateCell(1).SetCellValue("PatientNumber");
            headerRow.CreateCell(2).SetCellValue("Gender");
            headerRow.CreateCell(3).SetCellValue("DOB");
            headerRow.CreateCell(4).SetCellValue("Age");
            headerRow.CreateCell(5).SetCellValue("BloodGroup");
            headerRow.CreateCell(6).SetCellValue("Occupation");
            headerRow.CreateCell(7).SetCellValue("TaxId");
            headerRow.CreateCell(8).SetCellValue("Income");
            headerRow.CreateCell(9).SetCellValue("PatientAddress");
            headerRow.CreateCell(10).SetCellValue("PatientState");
            headerRow.CreateCell(11).SetCellValue("PatientPincode");
            headerRow.CreateCell(12).SetCellValue("PhoneNo");
            headerRow.CreateCell(13).SetCellValue("MobileNo");

            headerRow.CreateCell(14).SetCellValue("GaurdianName");
            headerRow.CreateCell(15).SetCellValue("GaurdianGender");
            headerRow.CreateCell(16).SetCellValue("GaurdianDOB");
            headerRow.CreateCell(17).SetCellValue("GaurdianAddress");
            headerRow.CreateCell(18).SetCellValue("GaurdianState");
            headerRow.CreateCell(19).SetCellValue("GaurdianPincode");
            headerRow.CreateCell(20).SetCellValue("GaurdianPhone");
            headerRow.CreateCell(21).SetCellValue("GaurdianMobile");
            headerRow.CreateCell(22).SetCellValue("GaurdianRelationship");
            headerRow.CreateCell(23).SetCellValue("ResponsibleParty");

            headerRow.CreateCell(24).SetCellValue("EmergencyConName");
            headerRow.CreateCell(25).SetCellValue("EmergencyConGender");
            headerRow.CreateCell(26).SetCellValue("EmergencyConAddress");
            headerRow.CreateCell(27).SetCellValue("EmergencyConState");
            headerRow.CreateCell(28).SetCellValue("EmergencyConPincode");
            headerRow.CreateCell(29).SetCellValue("EmergencyConRelation");
            headerRow.CreateCell(30).SetCellValue("EmergencyConPhone");
            headerRow.CreateCell(31).SetCellValue("EmergencyConMobile");

            headerRow.CreateCell(32).SetCellValue("InsuranceName");
            headerRow.CreateCell(33).SetCellValue("GroupNo");
            headerRow.CreateCell(34).SetCellValue("PolicyNo");
            headerRow.CreateCell(35).SetCellValue("Insurer");
            headerRow.CreateCell(36).SetCellValue("InsurerRelation");
            headerRow.CreateCell(37).SetCellValue("InsurerAddress");
            headerRow.CreateCell(38).SetCellValue("InsurerState");
            headerRow.CreateCell(39).SetCellValue("InsurerPincode");
            headerRow.CreateCell(40).SetCellValue("InsurancePhoneNo");

            foreach (var cell in headerRow.Cells)
            {
                cell.CellStyle = hStyle;
            }
            IList<Patient> PatientFromDB = PatientManager.Instance.ListAllPatient(Global.Company.CompanyId);
            if (PatientFromDB != null && PatientFromDB.Count > 0)
            {
                foreach (Patient lPatient in PatientFromDB.OrderBy(x => x.Name))
                {
                    List<Guardian> guardians = lPatient.Guardians?.ToList() ?? new List<Guardian>();
                    List<EmergencyContact> emergencyContacts = lPatient.EmergencyContact?.ToList() ?? new List<EmergencyContact>();
                    List<InsuranceInfo> insurances = InsuranceInfoManager.Instance.GetInsuranceInfoByPatientId(lPatient.Id).ToList() ?? new List<InsuranceInfo>();

                    int maxRows = Math.Max(1, Math.Max(guardians.Count, Math.Max(emergencyContacts.Count, insurances.Count)));

                    for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
                    {
                        IRow dataRow = sheet.CreateRow(i++);

                        dataRow.CreateCell(0).SetCellValue(lPatient.Name ?? "");
                        dataRow.CreateCell(1).SetCellValue(lPatient.PatientNumber ?? "");
                        dataRow.CreateCell(2).SetCellValue(lPatient.Gender.ToString() ?? "");
                        dataRow.CreateCell(3).SetCellValue(lPatient.DateOfBirth.ToString(Global.Company.DateFormat) ?? "");
                        dataRow.CreateCell(4).SetCellValue(lPatient.Age.ToString() ?? "");
                        dataRow.CreateCell(5).SetCellValue(lPatient.Bloodgroup?.ToString() ?? "");
                        dataRow.CreateCell(6).SetCellValue(lPatient.Occupation?.ToString() ?? "");
                        dataRow.CreateCell(7).SetCellValue(lPatient.TaxId?.ToString() ?? "");
                        dataRow.CreateCell(8).SetCellValue(Decimal.TryParse(lPatient.Income, out decimal incomeValue) ? incomeValue.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)): "0.00");
                        if (lPatient.AddressId != null)
                        {
                            Address PAddress = PatientManager.Instance.GetAddressById(lPatient.AddressId);
                            if (PAddress != null)
                            {
                                string fullAddress = string.Join(", ", new[]
                                {
                                    PAddress.AddressLine1, PAddress.AddressLine2, PAddress.CityOrTown,
                                    PAddress.District
                                }.Where(part => !string.IsNullOrEmpty(part)));

                                dataRow.CreateCell(9).SetCellValue(fullAddress);
                                dataRow.CreateCell(10).SetCellValue(PAddress.State?.Name ?? "");
                                dataRow.CreateCell(11).SetCellValue(PAddress.PinCode?.ToString() ?? "");
                            }
                        }
                        dataRow.CreateCell(12).SetCellValue(lPatient.ContactInfo?.Phone ?? "");
                        dataRow.CreateCell(13).SetCellValue(lPatient.ContactInfo?.Mobile ?? "");

                        if (rowIndex < guardians.Count)
                        {
                            var guardian = guardians[rowIndex];
                            dataRow.CreateCell(14).SetCellValue(guardian.Name ?? "");
                            dataRow.CreateCell(15).SetCellValue(guardian.Gender.ToString() ?? "");
                            dataRow.CreateCell(16).SetCellValue(guardian.DateOfBirth.Year < 1900 ? "" : guardian.DateOfBirth.ToString(Global.Company.DateFormat));

                            if (guardian.AddressId != null)
                            {
                                Address GaudAddress = PatientManager.Instance.GetAddressById(guardian.AddressId);
                                if (GaudAddress != null)
                                {
                                    string fullAddress = string.Join(", ", new[]
                                    {
                                        GaudAddress.AddressLine1, GaudAddress.AddressLine2, GaudAddress.CityOrTown,
                                        GaudAddress.District
                                    }.Where(part => !string.IsNullOrEmpty(part)));
                                    dataRow.CreateCell(17).SetCellValue(fullAddress);
                                    dataRow.CreateCell(18).SetCellValue(GaudAddress.State?.Name ?? "");
                                    dataRow.CreateCell(19).SetCellValue(GaudAddress.PinCode?.ToString() ?? "");
                                }
                            }
                            dataRow.CreateCell(20).SetCellValue(guardian.ContactInfo?.Phone ?? "");
                            dataRow.CreateCell(21).SetCellValue(guardian.ContactInfo?.Mobile ?? "");
                            dataRow.CreateCell(22).SetCellValue(guardian.RelationShip.ToString() ?? "");
                            dataRow.CreateCell(23).SetCellValue(lPatient.ResponsibleParty != null ? lPatient.ResponsibleParty.Name?.ToString() : "");
                        }

                        if (rowIndex < emergencyContacts.Count)
                        {
                            var emergencyContact = emergencyContacts[rowIndex];
                            dataRow.CreateCell(24).SetCellValue(emergencyContact.Name ?? "");
                            dataRow.CreateCell(25).SetCellValue(emergencyContact.Gender.ToString() ?? "");

                            if (emergencyContact.AddressId != null)
                            {
                                Address EmergAddress = PatientManager.Instance.GetAddressById(emergencyContact.AddressId);
                                if (EmergAddress != null)
                                {
                                    string fullAddress = string.Join(", ", new[]
                                    {
                                        EmergAddress.AddressLine1, EmergAddress.AddressLine2, EmergAddress.CityOrTown,
                                        EmergAddress.District
                                    }.Where(part => !string.IsNullOrEmpty(part)));

                                    dataRow.CreateCell(26).SetCellValue(fullAddress);
                                    dataRow.CreateCell(27).SetCellValue(EmergAddress.State?.Name ?? "");
                                    dataRow.CreateCell(28).SetCellValue(EmergAddress.PinCode?.ToString() ?? "");
                                }
                            }
                            dataRow.CreateCell(29).SetCellValue(emergencyContact.RelationShip.ToString() ?? "");
                            dataRow.CreateCell(30).SetCellValue(emergencyContact.ContactInfo?.Phone ?? "");
                            dataRow.CreateCell(31).SetCellValue(emergencyContact.ContactInfo?.Mobile ?? "");
                        }

                        if (rowIndex < insurances.Count)
                        {
                            var insurance = insurances[rowIndex];
                            dataRow.CreateCell(32).SetCellValue(insurance.InsuranceName ?? "");
                            dataRow.CreateCell(33).SetCellValue(insurance.GroupNumber ?? "");
                            dataRow.CreateCell(34).SetCellValue(insurance.PolicyNumber ?? "");
                            dataRow.CreateCell(35).SetCellValue(insurance.InsuranceHolder?.Name ?? "");
                            dataRow.CreateCell(36).SetCellValue(insurance.InsuranceHolderRelationShip.ToString() ?? "");
                            if (insurance.EmployerAddressId != null)
                            {
                                Address InsurerAddress = PatientManager.Instance.GetAddressById(insurance.EmployerAddressId);
                                if (InsurerAddress != null)
                                {
                                    string fullAddress = string.Join(", ", new[]
                                    {
                                        InsurerAddress.AddressLine1, InsurerAddress.AddressLine2, InsurerAddress.CityOrTown,
                                        InsurerAddress.District
                                    }.Where(part => !string.IsNullOrEmpty(part)));

                                    dataRow.CreateCell(37).SetCellValue(fullAddress);
                                    dataRow.CreateCell(38).SetCellValue(InsurerAddress.State?.Name ?? "");
                                    dataRow.CreateCell(39).SetCellValue(InsurerAddress.PinCode?.ToString() ?? "");
                                }
                            }
                            dataRow.CreateCell(40).SetCellValue(insurance.EmployerContactInfo?.Phone?.Trim('-') ?? "");
                        }
                    }
                }
            }
            for (int columnIndex = 0; columnIndex < 40; columnIndex++)
            {
                sheet.AutoSizeColumn(columnIndex);
            }

            SaveFileDialog sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfDlg.Filter = "Excel Workbook (*.xls;*.xlsx)|*.xls;*.xlsx|All Files (*.*)|*.*";
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "PatientMaster_" + DateTime.Now.ToString(Global.Company.DateFormat).Replace("/", "-");

            if (sfDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(sfDlg.FileName, FileMode.Create))
                    {
                        workbook.Write(fs);
                    }
                    if (MessageBox.Show("File saved successfully! \nDo you want to open the file?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo { FileName = sfDlg.FileName, UseShellExecute = true });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save the file: " + ex.Message);
                }
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
