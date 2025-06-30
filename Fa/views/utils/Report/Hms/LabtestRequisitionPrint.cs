using fa.api.Hms;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.hms.common;
using fa.views.utils.Common;
using fa.model.Hms.Master;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.views.hms.patient;
using Rectangle = iTextSharp.text.Rectangle;
using ScottPlot.MarkerShapes;
using fa.model.hms.config;
using DocumentFormat.OpenXml.Bibliography;
using Fa.api.Hms;
using DocumentFormat.OpenXml.ExtendedProperties;
using fa.api.Accounting;
using Company = fa.model.Accounting.Masters.Company;
using fa.api.UserProfile;
using fa.model.Employee;
using fa.model.Hms.Ip;
using fa.model.Hms.Op;
using fa.model.UserProfile;
using fa;
using fa.views.utils;

namespace Fa.views.utils.Report.Hms
{
    internal class LabtestRequisitionPrint
    {
        public bool ExportOrPrintToFile(long ConsultedLabTestId, long PatientId, string ReportName, string fileExtension, bool isPrint)
        {
            try
            {
                switch (fileExtension.ToLower())
                {
                    case "xls":
                        break;
                    case "pdf":
                        var DataTable = DataGridViewDataTable(ConsultedLabTestId);
                        if (DataTable != null)
                        {
                            GenerateLabTestPrint(DataTable, PatientId, ConsultedLabTestId, isPrint, ReportName);
                            break;
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("File Error Please Contact System Admin");
                Console.WriteLine(e.ToString());
            }
            return true;
        }
        static readonly String[] LabTestColumn = new String[]
        {
            "Parameters", "Value", "Unit", "Biological Ref Range", "ID", "MedicalTestName"
        };
        public static DataTable DataGridViewDataTable(long ConsLabTestId)
        {
            DataTable LabTestTable = new DataTable();
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            LabTestTable.Columns.Add(LabTestColumn[(int)ConsLabTestElementsGridColumn.PARAMETERS], typeof(string));
            //LabTestTable.Columns.Add(LabTestColumn[(int)ConsLabTestElementsGridColumn.VALUE], typeof(string));
            //LabTestTable.Columns.Add(LabTestColumn[(int)ConsLabTestElementsGridColumn.UOM], typeof(string));
            //LabTestTable.Columns.Add(LabTestColumn[(int)ConsLabTestElementsGridColumn.BIO_REF_RANGE], typeof(string));

            DataRow PrescriptionList = null!;
            {
                IList<ConsultedLabTestElements> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestElementsByLabtestId(ConsLabTestId);
                ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetMedicalTestsByConsultedLabTestId(ConsLabTestId);
                IList<MedicalTestElement> MTElements = null!;

                if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
                {
                    foreach (ConsultedLabTestElements ConsLabTestEle in lConsultedLabTestElements)
                    {
                        PrescriptionList = LabTestTable.NewRow();
                        PrescriptionList[LabTestColumn[(int)ConsLabTestElementsGridColumn.PARAMETERS]] = ConsLabTestEle.Name;
                        //PrescriptionList[LabTestColumn[(int)ConsLabTestElementsGridColumn.VALUE]] = ConsLabTestEle.ResultDescription ?? "";
                        //PrescriptionList[LabTestColumn[(int)ConsLabTestElementsGridColumn.UOM]] = ConsLabTestEle?.Uom?.Name ?? "";

                        //if (ConsLabTestEle!.SingleValue != null)
                        //{
                        //    PrescriptionList[LabTestColumn[(int)ConsLabTestElementsGridColumn.BIO_REF_RANGE]] = string.IsNullOrEmpty(ConsLabTestEle.SingleValue) ?
                        //        ((string.IsNullOrEmpty(ConsLabTestEle.Class) && string.IsNullOrEmpty(ConsLabTestEle.SubClass)) ? " - " : $"{ConsLabTestEle.Class} {ConsLabTestEle.SubClass} : {ConsLabTestEle.RangeFrom} - {ConsLabTestEle.RangeTo}") :
                        //        $"{ConsLabTestEle.Class} {ConsLabTestEle.SubClass} : {ConsLabTestEle.SingleValue}";
                        //}
                        //LabTestTable.Rows.Add(PrescriptionList);
                        //MedicalTestElement elements = MedicalTestManager.Instance.GetMedicalTestElementById(ConsLabTestEle.MedicalTestElementId, Global.Company.CompanyId);
                        //MTElements = MedicalTestManager.Instance.GetMedicalTestElementByElementCode(elements.ElementCode, Global.Company.CompanyId);

                        //var filteredMTElements = MTElements.Where(x => !lConsultedLabTestElements.Any(c => c.Class == x.Class && c.SubClass == x.SubClass)).ToList();

                        //foreach (MedicalTestElement MTElement in filteredMTElements)
                        //{
                        //    PrescriptionList = LabTestTable.NewRow();
                        //    PrescriptionList[LabTestColumn[(int)ConsLabTestElementsGridColumn.UOM]] = MTElement?.Uom?.Name ?? "";
                        //    PrescriptionList[LabTestColumn[(int)ConsLabTestElementsGridColumn.BIO_REF_RANGE]] = string.IsNullOrEmpty(MTElement!.SingleValue) ? $"{MTElement.Class} {MTElement.SubClass} : {MTElement.RangeFrom} - {MTElement.RangeTo}" : $"{MTElement.Class} {MTElement.SubClass} : {MTElement.SingleValue}";

                        //    LabTestTable.Rows.Add(PrescriptionList);
                        //}
                    }
                }
            }
            return LabTestTable;
        }
        public void GenerateLabTestPrint(DataTable DataTable, long PatientId, long ConsultedLabTestId, bool isPrint, string ReportName)
        {
            Cursor.Current = Cursors.WaitCursor;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A5LandscapeHeight = 300;
                Document pdfDoc = new Document(PageSize.A5.Rotate(), -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                HospitalConfiguration configuration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Patient.CompanyId);
                ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetMedicalTestsByConsultedLabTestId(ConsultedLabTestId);
                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteById(consultedLabTest.ConsultationNoteId);
                User user = UserManager.Instance.GetUserByLogin(Global.User.Login);
                Employee EmployeeDetails = null;
                if ((user.IsSuperAdmin) || (!user.IsSuperAdmin && user.EmployeeId == null))
                {
                    if (Note.InPatientAdmissionId == null)
                    {
                        if (Note.OpRegistrationId.HasValue)
                        {
                            Registration registration = OpManager.Instance.GetOpRegistrationById(Note.OpRegistrationId.Value);
                            if (registration != null && registration.RequestedDoctorId != null)
                            {
                                EmployeeDetails = EmployeeManager.Instance.GetEmployeeInfoById(registration.RequestedDoctorId.Value);
                            }
                        }
                    }
                    else if (Note.InPatientAdmissionId.HasValue)
                    {
                        InPatientAdmission inPatientAdmission = IpManager.Instance.GetInPatientAdmissionById(Note.InPatientAdmissionId.Value);
                        if (inPatientAdmission != null && inPatientAdmission.CurrentMedicalTeam.PrimaryDoctorId != null)
                        {
                            EmployeeDetails = EmployeeManager.Instance.GetEmployeeInfoById(inPatientAdmission.CurrentMedicalTeam.PrimaryDoctorId.Value);
                        }
                    }
                }
                else
                {
                    if (user.EmployeeId != null)
                    {
                        EmployeeDetails = EmployeeManager.Instance.GetEmployeeInfoById(user.EmployeeId.Value);
                    }
                }

                if (Patient != null)
                {
                    PdfPageHeader PdfHeader = new PdfPageHeader()
                    {
                        IsMainHeader = true,
                        Islogo = true,
                        IsAddress = true,
                        IsPhone = true,
                        IsEmail = true,
                        IsWebsite = true,
                        IsLicenceInfo = true,
                        ReportLine1 = ReportName,
                    };
                    PdfPTable HTable = PdfHeader.PageHeader();

                    PdfHeader = new PdfPageHeader()
                    {
                        IsMainHeader = false,
                        Islogo = true,
                        IsAddress = true,
                        IsPhone = false,
                        IsEmail = false,
                        IsWebsite = false,
                        IsLicenceInfo = false,
                        ReportLine1 = ReportName,
                    };
                    PdfPTable MiniHTable = PdfHeader.PageHeader();

                    PdfPTable PatientDetailsTable = PatientDetailsHeading(Patient, ConsultedLabTestId);
                    PdfPTable LabTestHeadingTable = LabTesHeading(ConsultedLabTestId);
                    PdfPTable LabTestElementHeadingTable = LabTesElementHeading();
                    PdfPTable LabTestMainTable = LabTestMainTableRow(ConsultedLabTestId);
                    PdfPTable LabTestDummyTable = LabTestDummyTableRow();

                    pdfDoc.Add(HTable);
                    pdfDoc.Add(PatientDetailsTable);
                    pdfDoc.Add(LabTestHeadingTable);
                    pdfDoc.Add(LabTestElementHeadingTable);
                    pdfDoc.Add(LabTestMainTable);
                    pdfDoc.Add(LabTestDummyTable);

                    Cursor.Current = Cursors.WaitCursor;
                    AddDigitalSignature(configuration, pdfDoc, writer, EmployeeDetails);
                    pdfDoc.Close();

                    PdfFooter PdfFooter = new PdfFooter();
                    PdfFooter.IsReport = true;
                    PdfFooter.IsDate = true;
                    PdfFooter.Text = string.Empty;
                    PdfFooter.IsPageNumber = true;
                    PdfFooter.IsA5LandScape = true;
                    PdfFooter.PdfFile = myMemoryStream.ToArray();
                    byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                    myMemoryStream.Close();

                    Cursor.Current = Cursors.WaitCursor;
                    PdfGeneration PdfGeneration = new PdfGeneration();
                    PdfGeneration.IsPrint = isPrint;
                    PdfGeneration.FileName = "LabTest";
                    PdfGeneration.PdfFile = PdfFileWithFooter;
                    PdfGeneration.SavePdfFile();
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        public enum ConsLabTestElementsGridColumn
        {
            PARAMETERS, VALUE, UOM, BIO_REF_RANGE
        }
        public void AddDigitalSignature(HospitalConfiguration configuration, Document pdfDoc, PdfWriter writer, Employee EmployeeDetails)
        {
            PdfPTable signatureTable = new PdfPTable(2);
            signatureTable.TotalWidth = pdfDoc.PageSize.Width - 50f;
            signatureTable.LockedWidth = true;

            float[] sigWidths = new float[] { 75f, 25f };
            signatureTable.SetWidths(sigWidths);

            // Add Footer Details Cell
            PdfPCell footerCell;
            if (configuration.IsFooterDetailsDisplayOnLabtest)
            {
                if (!string.IsNullOrEmpty(configuration.FooterDetailsLabtest))
                {
                    footerCell = new PdfPCell(new Phrase(configuration.FooterDetailsLabtest, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                }
                else
                {
                    footerCell = new PdfPCell(new Phrase(""));
                }
            }
            else
            {
                footerCell = new PdfPCell(new Phrase(""));
            }
            footerCell.Border = Rectangle.NO_BORDER;
            footerCell.PaddingTop = 25f;
            footerCell.HorizontalAlignment = Element.ALIGN_LEFT;
            footerCell.MinimumHeight = 20f;
            signatureTable.AddCell(footerCell);

            // Add Signature Cell
            PdfPCell signatureCell = new PdfPCell();
            signatureCell.Border = Rectangle.NO_BORDER;
            signatureCell.MinimumHeight = 20f;
            signatureCell.HorizontalAlignment = Element.ALIGN_CENTER;

            if (configuration.IsDigitalSignatureDisplayOnLabtest)
            {
                if (EmployeeDetails != null && EmployeeDetails.DigitalSignature != null)
                {
                    iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(EmployeeDetails.DigitalSignature);
                    signatureImage.ScaleToFit(100f, 50f);
                    signatureImage.Alignment = Element.ALIGN_CENTER;
                    signatureCell.AddElement(signatureImage);
                }
            }
            if (configuration.IsSignatureNameDisplayOnLabtest)
            {
                if (EmployeeDetails != null && !string.IsNullOrEmpty(EmployeeDetails.DisplayAs))
                {
                    string signatureName = EmployeeDetails.DisplayAs.Length > 75
                                            ? EmployeeDetails.DisplayAs.Substring(0, 75) + "..."
                                            : EmployeeDetails.DisplayAs;

                    PdfPTable nestedTable = new PdfPTable(1);
                    nestedTable.WidthPercentage = 100;

                    Paragraph signatureNameParagraph = new Paragraph(signatureName, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"));
                    signatureNameParagraph.Alignment = Element.ALIGN_CENTER;

                    PdfPCell signatureNameCell = new PdfPCell(signatureNameParagraph);
                    signatureNameCell.Border = Rectangle.NO_BORDER;
                    signatureNameCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    signatureNameCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    nestedTable.AddCell(signatureNameCell);

                    signatureCell.AddElement(nestedTable);
                }
            }
            signatureTable.AddCell(signatureCell);

            PdfContentByte cb = writer.DirectContent;
            signatureTable.WriteSelectedRows(0, -1, 30f, 80f, cb);
        }
        private PdfPTable LabTesHeading(long ConsLabTestId)
        {
            ConsultedLabTest ConsultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(ConsLabTestId);

            PdfPTable ReportMainTable = new PdfPTable(6);
            float[] widths = new float[] { 155f, 40f, 40f, 50f, 60f, 60f };
            ReportMainTable.SetWidths(widths);
            ReportMainTable.AddCell(PdfDataAlignment.CreateDummyRow("", 6));
            ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellLab(ConsultedLabTest.MedicalTest.Name, 6));
            return ReportMainTable;
        }

        private PdfPTable LabTesElementHeading()
        {
            PdfPTable HeadTable = new PdfPTable(4);
            float[] widths = new float[] { 50f, 13f, 12f, 25f };
            HeadTable.SetWidths(widths);

            PdfPCell HeadCell = new PdfPCell();

            HeadCell = new PdfPCell(new Phrase("Element Name", PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = 0.7f;
            HeadCell.PaddingBottom = 4;
            HeadCell.BorderColor = BaseColor.BLACK;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = 0.7f;
            HeadCell.PaddingBottom = 4;
            HeadCell.BorderColor = BaseColor.BLACK;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = 0.7f;
            HeadCell.PaddingBottom = 4;
            HeadCell.BorderColor = BaseColor.BLACK;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderWidthRight = (float)BorderStyle.None;
            HeadCell.BorderWidthLeft = (float)BorderStyle.None;
            HeadCell.BorderWidthTop = (float)BorderStyle.None;
            HeadCell.BorderWidthBottom = 0.7f;
            HeadCell.PaddingBottom = 4;
            HeadCell.BorderColor = BaseColor.BLACK;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }
        public static PdfPTable PatientDetailsHeading(Patient Patient, long ConsultedLabTestId)
        {
            PdfPTable MainHeadTable = new PdfPTable(2);
            MainHeadTable.SetTotalWidth(new float[] { 65, 35 });
            MainHeadTable.AddCell(PdfDataAlignment.CreateLabTestHeadingHeading("LABORATORY TEST REQUISITION", 10));

            int HeadColumns = 2;
            float[] HeadWidths = new float[] { 15f, 50f };
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            HeadTable.SetWidths(HeadWidths);
            PdfPCell HeadCell = new PdfPCell();

            var PatientName = "Patient Name         :     ";
            var PatientDetails = "";
            PatientDetails = Patient.Name;

            HeadCell = new PdfPCell(new Phrase(PatientName, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientHeading = "Age / Gender         : ";
            PatientDetails = Patient.Age + " Years / " + Patient.Gender.ToString();

            HeadCell = new PdfPCell(new Phrase(PatientHeading, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientHeadingPhone = "Phone                    : ";
            if (Patient.ContactInfo != null)
            {
                PatientDetails = !string.IsNullOrEmpty(Patient.ContactInfo.Phone) ? Patient.ContactInfo.Phone : !string.IsNullOrEmpty(Patient.ContactInfo.Mobile) ? Patient.ContactInfo.Mobile : "";
            }

            HeadCell = new PdfPCell(new Phrase(PatientHeadingPhone, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var patientAddress = "Patient Address     :     ";
            PatientDetails = "";
            if (Patient.Address != null && !string.IsNullOrEmpty(Patient.Address.FullAddress))
            {
                PatientDetails = Patient.Address.FullAddressInSingleLine;
            }
            HeadCell = new PdfPCell(new Phrase(patientAddress, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            MainHeadTable.AddCell(HeadTable);

            HeadColumns = 2;
            HeadWidths = new float[] { 20f, 22f };
            HeadTable = new PdfPTable(HeadColumns);
            HeadTable.SetWidths(HeadWidths);
            ConsultedLabTest ConsultedLabTest = ConsultationNoteManager.Instance.GetConsultedLabTestsById(ConsultedLabTestId);
            ConsultationNote consultationNote = ConsultationNoteManager.Instance.GetConsultationNoteById(ConsultedLabTest.ConsultationNoteId);
            var Date = "Date                            :     ";
            PatientDetails = "";
            PatientDetails = consultationNote.Date.ToString(Global.Company.DateFormat);
            HeadCell = new PdfPCell(new Phrase(Date, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            InPatientAdmission inPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByPatientId(Patient.Id);
            Registration registration = OpManager.Instance.GetRegisterByPatientId(Patient.Id);
            if (inPatientAdmission != null)
            {
                var patientOPId = "Patient Op Number     :     ";
                PatientDetails = "";
                PatientDetails = registration.PatientOPNumber;
                HeadCell = new PdfPCell(new Phrase(patientOPId, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.UseVariableBorders = true;
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Colspan = 1;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadTable.AddCell(HeadCell);

                HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                HeadCell.UseVariableBorders = true;
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Colspan = 1;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadTable.AddCell(HeadCell);

                var patientIpId = "Patient Ip Number       :     ";
                PatientDetails = "";
                PatientDetails = inPatientAdmission.PatientIPNumber;
                HeadCell = new PdfPCell(new Phrase(patientIpId, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.UseVariableBorders = true;
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Colspan = 1;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadTable.AddCell(HeadCell);

                HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                HeadCell.UseVariableBorders = true;
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Colspan = 1;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadTable.AddCell(HeadCell);
            }
            else
            {
                if (registration != null)
                {
                    var patientOPId = "Patient Op Number     :     ";
                    PatientDetails = "";
                    PatientDetails = registration.PatientOPNumber;
                    HeadCell = new PdfPCell(new Phrase(patientOPId, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.UseVariableBorders = true;
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.Colspan = 1;
                    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    HeadTable.AddCell(HeadCell);

                    HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    HeadCell.UseVariableBorders = true;
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.Colspan = 1;
                    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    HeadTable.AddCell(HeadCell);
                }
            }

            if (inPatientAdmission != null)
            {
                if(inPatientAdmission.CurrentMedicalTeam.PrimaryDoctorId != null)
                {
                    Employee employee = EmployeeManager.Instance.GetEmployeeInfoById(inPatientAdmission.CurrentMedicalTeam.PrimaryDoctorId.Value);
                    if (employee != null)
                    {
                        var patientOPId = "Doctor Name               :     ";
                        PatientDetails = "";
                        PatientDetails = employee.Name;
                        HeadCell = new PdfPCell(new Phrase(patientOPId, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        HeadCell.UseVariableBorders = true;
                        HeadCell.BorderColor = BaseColor.WHITE;
                        HeadCell.Colspan = 1;
                        HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        HeadTable.AddCell(HeadCell);

                        HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                        HeadCell.UseVariableBorders = true;
                        HeadCell.BorderColor = BaseColor.WHITE;
                        HeadCell.Colspan = 1;
                        HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        HeadTable.AddCell(HeadCell);
                    }
                }
            }
            else
            {
                if (registration != null && registration.RequestedDoctorId != null)
                {
                    Employee employee = EmployeeManager.Instance.GetEmployeeInfoById(registration.RequestedDoctorId.Value);
                    if (employee != null)
                    {
                        var patientOPId = "Doctor Name               :     ";
                        PatientDetails = "";
                        PatientDetails = employee.Name;
                        HeadCell = new PdfPCell(new Phrase(patientOPId, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        HeadCell.UseVariableBorders = true;
                        HeadCell.BorderColor = BaseColor.WHITE;
                        HeadCell.Colspan = 1;
                        HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        HeadTable.AddCell(HeadCell);

                        HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                        HeadCell.UseVariableBorders = true;
                        HeadCell.BorderColor = BaseColor.WHITE;
                        HeadCell.Colspan = 1;
                        HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        HeadTable.AddCell(HeadCell);
                    }
                }
            }

            var PatientId = "Patient Number           : ";
            PatientDetails = Patient.PatientNumber;

            HeadCell = new PdfPCell(new Phrase(PatientId, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientBarcode = "";

            MemoryStream Barcode = new MemoryStream();
            var Image = BarCode.GenerateImageBarcode1(Patient.PatientNumber);
            Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
            image.ScaleAbsoluteHeight(15);
            image.ScaleAbsoluteWidth(110);

            HeadCell = new PdfPCell(new Phrase(PatientBarcode, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.PaddingTop = 2;
            HeadCell.PaddingBottom = 2;
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(image);
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.PaddingTop = 2;
            HeadCell.PaddingBottom = 2;
            HeadCell.Colspan = 2;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            MainHeadTable.AddCell(HeadTable);
            return MainHeadTable;
        }

        public static PdfPTable LabTestMainTableRow(long ConsultNoteId)
        {
            PdfPTable MainTableRow = new PdfPTable(1);
            MainTableRow.SetTotalWidth(new float[] { 100f });
            MainTableRow.DefaultCell.Border = (int)BorderStyle.None;

            IList<ConsultedLabTestElements> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestElementsByLabtestId(ConsultNoteId);
            ConsultedLabTest consultedLabTest = ConsultationNoteManager.Instance.GetMedicalTestsByConsultedLabTestId(ConsultNoteId);
            IList<MedicalTestElement> MTElements = null!;
            if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
            {
                foreach (ConsultedLabTestElements ConsLabTestEle in lConsultedLabTestElements)
                {
                    PdfPCell HeadCell = new PdfPCell();

                    MedicalTestElement elements = MedicalTestManager.Instance.GetMedicalTestElementById(ConsLabTestEle.MedicalTestElementId, Global.Company.CompanyId);
                    MTElements = MedicalTestManager.Instance.GetMedicalTestElementByElementCode(elements.ElementCode, Global.Company.CompanyId);
                    var filteredMTElements = MTElements.Where(x => !lConsultedLabTestElements.Any(c => c.Name == x.Name && c.Class == x.Class && c.SubClass == x.SubClass)).ToList();

                    int Rowspan = filteredMTElements.Count == 0 ? 1 : (filteredMTElements.Count + 1);

                    var AddressData = ConsLabTestEle.Name;
                    var RptLine2 = new Chunk(elements.Description, PdfDataAlignment.GetFont("Font_Normal_Italic_5_Black"));
                    RptLine2.SetTextRise(-3);
                    RptLine2.setLineHeight(5);
                    Paragraph AddressDataParagraph = new Paragraph(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
                    AddressDataParagraph.Alignment = Element.ALIGN_LEFT;
                    Paragraph ReportLine2paragraph = new Paragraph(RptLine2);
                    ReportLine2paragraph.Alignment = Element.ALIGN_LEFT;

                    HeadCell = new PdfPCell();
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
                    HeadCell.Rowspan = Rowspan;
                    HeadCell.Colspan = 1;
                    HeadCell.PaddingTop = -1;
                    HeadCell.AddElement(AddressDataParagraph);
                    HeadCell.AddElement(ReportLine2paragraph);
                    MainTableRow.AddCell(HeadCell);

                    HeadCell = new PdfPCell(new Phrase( "", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.UseVariableBorders = true;
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.Rowspan = 1;
                    HeadCell.Colspan = 1;
                    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    MainTableRow.AddCell(HeadCell);

                    HeadCell = new PdfPCell(new Phrase( "", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    HeadCell.UseVariableBorders = true;
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.Rowspan = 1;
                    HeadCell.Colspan = 1;
                    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    MainTableRow.AddCell(HeadCell);

                    //string BioRefRange = string.IsNullOrEmpty(ConsLabTestEle!.SingleValue) ?
                    //            ((string.IsNullOrEmpty(ConsLabTestEle.Class) && string.IsNullOrEmpty(ConsLabTestEle.SubClass)) ? ((string.IsNullOrEmpty(ConsLabTestEle.RangeFrom) && string.IsNullOrEmpty(ConsLabTestEle.RangeTo) ? " - " : $"{ConsLabTestEle.RangeFrom} - {ConsLabTestEle.RangeTo}")) : $"{ConsLabTestEle.Class} {ConsLabTestEle.SubClass} : {ConsLabTestEle.RangeFrom} - {ConsLabTestEle.RangeTo}") :
                    //            ((string.IsNullOrEmpty(ConsLabTestEle.Class) && string.IsNullOrEmpty(ConsLabTestEle.SubClass)) ? ConsLabTestEle.SingleValue : $"{ConsLabTestEle.Class} {ConsLabTestEle.SubClass} : {ConsLabTestEle.SingleValue}");
                    //if (!string.IsNullOrEmpty(BioRefRange.Replace("-", "").Trim()))
                    //{
                    //    PdfPTable BioRefRangeColumn = new PdfPTable(2);
                    //    BioRefRangeColumn.SetWidths(new float[] { 16f, 9f });
                    //    BioRefRangeColumn.DefaultCell.Border = (int)BorderStyle.None;
                    //    BioRefRangeColumn.DefaultCell.BorderColor = BaseColor.WHITE;

                    //    string[] splitedRangeValue = BioRefRange.Split(':');

                    //    HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //    HeadCell.UseVariableBorders = true;
                    //    HeadCell.BorderColor = BaseColor.WHITE;
                    //    HeadCell.Border = Rectangle.NO_BORDER;
                    //    HeadCell.PaddingTop = 0;
                    //    HeadCell.Colspan = 1;
                    //    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    BioRefRangeColumn.AddCell(HeadCell);

                    //    HeadCell = new PdfPCell(new Phrase( "", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //    HeadCell.UseVariableBorders = true;
                    //    HeadCell.BorderColor = BaseColor.WHITE;
                    //    HeadCell.Border = Rectangle.NO_BORDER;
                    //    HeadCell.PaddingTop = 0;
                    //    HeadCell.Colspan = 1;
                    //    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    BioRefRangeColumn.AddCell(HeadCell);

                    //    MainTableRow.AddCell(BioRefRangeColumn);
                    //}
                    //else
                    //{
                    //    HeadCell = new PdfPCell(new Phrase(" ", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //    HeadCell.UseVariableBorders = true;
                    //    HeadCell.BorderColor = BaseColor.WHITE;
                    //    HeadCell.PaddingTop = 0;
                    //    HeadCell.Colspan = 1;
                    //    HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    MainTableRow.AddCell(HeadCell);
                    //}
                    //if (filteredMTElements != null && filteredMTElements.Count > 0)
                    //{
                    //    foreach (MedicalTestElement MTElement in filteredMTElements)
                    //    {
                    //        HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //        HeadCell.UseVariableBorders = true;
                    //        HeadCell.BorderColor = BaseColor.WHITE;
                    //        HeadCell.PaddingTop = (float)BorderStyle.None;
                    //        HeadCell.Colspan = 1;
                    //        HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        MainTableRow.AddCell(HeadCell);

                    //        HeadCell = new PdfPCell(new Phrase( "", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //        HeadCell.UseVariableBorders = true;
                    //        HeadCell.BorderColor = BaseColor.WHITE;
                    //        HeadCell.PaddingTop = (float)BorderStyle.None;
                    //        HeadCell.Colspan = 1;
                    //        HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //        MainTableRow.AddCell(HeadCell);

                    //        BioRefRange = string.IsNullOrEmpty(MTElement!.SingleValue) ? $"{MTElement.Class} {MTElement.SubClass} : {MTElement.RangeFrom} - {MTElement.RangeTo}" : $"{MTElement.Class} {MTElement.SubClass} : {MTElement.SingleValue}";

                    //        if (!string.IsNullOrEmpty(BioRefRange))
                    //        {
                    //            PdfPTable BioRefRangeColumn = new PdfPTable(2);
                    //            BioRefRangeColumn.SetWidths(new float[] { 16f, 9f });
                    //            BioRefRangeColumn.DefaultCell.Border = (int)BorderStyle.None;
                    //            BioRefRangeColumn.DefaultCell.BorderColor = BaseColor.RED;
                    //            BioRefRangeColumn.DefaultCell.PaddingTop = (int)BorderStyle.None;

                    //            string[] splitedRangeValue = BioRefRange.Split(':');

                    //            HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //            HeadCell.UseVariableBorders = true;
                    //            HeadCell.BorderColor = BaseColor.WHITE;
                    //            HeadCell.Border = Rectangle.NO_BORDER;
                    //            HeadCell.PaddingTop = -1;
                    //            HeadCell.PaddingBottom = 2;
                    //            HeadCell.Colspan = 1;
                    //            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //            BioRefRangeColumn.AddCell(HeadCell);

                    //            HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //            HeadCell.UseVariableBorders = true;
                    //            HeadCell.BorderColor = BaseColor.WHITE;
                    //            HeadCell.Border = Rectangle.NO_BORDER;
                    //            HeadCell.PaddingTop = -1;
                    //            HeadCell.PaddingBottom = 2;
                    //            HeadCell.Colspan = 1;
                    //            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //            BioRefRangeColumn.AddCell(HeadCell);

                    //            MainTableRow.AddCell(BioRefRangeColumn);
                    //        }
                    //        else
                    //        {
                    //            HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    //            HeadCell.UseVariableBorders = true;
                    //            HeadCell.BorderColor = BaseColor.WHITE;
                    //            HeadCell.PaddingTop = -1;
                    //            HeadCell.PaddingBottom = 2;
                    //            HeadCell.Colspan = 1;
                    //            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //            MainTableRow.AddCell(HeadCell);
                    //        }
                    //    }
                    //}
                }
            }
            return MainTableRow;
        }

        public static PdfPTable LabTestDummyTableRow()
        {
            PdfPTable DummyTableRow = new PdfPTable(4);
            DummyTableRow.SetTotalWidth(new float[] { 50f, 13f, 13f, 24f });

            PdfPCell HeadCell = new PdfPCell();

            HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderWidthTop = (int)BorderStyle.None;
            HeadCell.BorderWidthLeft = (int)BorderStyle.None;
            HeadCell.BorderWidthRight = (int)BorderStyle.None;
            HeadCell.BorderWidthBottom = 0.6f;
            HeadCell.PaddingTop = 2;
            HeadCell.MinimumHeight = 5;
            HeadCell.Colspan = 4;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            DummyTableRow.AddCell(HeadCell);

            return DummyTableRow;
        }
    }
}
