using DocumentFormat.OpenXml.Math;
using fa.api.Accounting;
using fa.api.Hms;
using fa.api.UserProfile;
using fa.model.Accounting.Masters;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.hms.config;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.UserProfile;
using fa.views.hms.patient;
using fa.views.utils.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Paragraph = iTextSharp.text.Paragraph;
using Rectangle = iTextSharp.text.Rectangle;
using System.Reflection;

namespace fa.views.utils.Hms
{
    public class PrescriptionPrinting
    {
        public bool ExportOrPrintToFileA4andA5Format(long NoteId, long PatientId, long PrescriptionNos, string Author, bool IsDisCharge, string ReportName, string fileExtension, bool isPrint, byte[] CheckedImg, byte[] UnCheckedImg, string PrintPaperFormat)
        {
            try
            {
                switch (fileExtension.ToLower())
                {
                    case "xls":
                        break;
                    case "pdf":
                        var DataTable = DataGridViewDataTable(NoteId, IsDisCharge);
                        if (DataTable != null)
                        {
                            GeneratePrescriptionPreviewNEW(DataTable, PatientId, NoteId, isPrint, ReportName, PrescriptionNos, Author, CheckedImg, UnCheckedImg, PrintPaperFormat);
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

        static readonly String[] PrescriptionColumn = new String[]
        {
            "Prescription", "Total", "Days", "Before/\nAfter Food", "Interval", "Morning", "Afternoon", "Evening", "Night", "AdditionalNotes", "PrescribedBy", "PrescriptionNos"
        };
        
        public static DataTable DataGridViewDataTable(long NoteId,bool IsDisCharge)
        {
            DataTable PrescriptionTable = new DataTable();
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.NAME], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.TOTAL], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.DAYS], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.HOURS], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.MORNING], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.AFTERNOON], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.EVENING], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.NIGHT], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.BEFOREAFTER], typeof(string));
            PrescriptionTable.Columns.Add(PrescriptionColumn[(int)PrescriptionDetailsGridColumn.ADDNOTES], typeof(string));

            DataRow PrescriptionList = null!;
            int rn = 0;
            ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteById(NoteId);
            IsDisCharge = Note.IsDischarged;
            if (IsDisCharge)
            {
                IList<DischargePrescription> lDischargePrescription = DischargeNoteManager.Instance.ListDischargePrescriptionByPatientIpId((long)Note.InPatientAdmissionId!);
                if (lDischargePrescription != null && lDischargePrescription.Count > 0)
                {
                    foreach (DischargePrescription ConsPres in lDischargePrescription)
                    {
                        Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById(ConsPres.PrescriptionId);
                        PrescriptionList = PrescriptionTable.NewRow();
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.NAME]] = PrescriptionFromDB.IsCustomProduct == true ? PrescriptionFromDB.CustomProduct :PrescriptionFromDB.Product.Name;
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.TOTAL]] = PrescriptionFromDB.Total.ToString();
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.DAYS]] = PrescriptionFromDB.Days.ToString();
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.HOURS]] = PrescriptionFromDB.Hours.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.MORNING]] = PrescriptionFromDB.Morning.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.AFTERNOON]] = PrescriptionFromDB.Afternoon.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.EVENING]] = PrescriptionFromDB.Evening.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.NIGHT]] = PrescriptionFromDB.Night.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.BEFOREAFTER]] = PrescriptionFromDB.TakeDosage == TakeDosage.AFTER ? "After" : PrescriptionFromDB.TakeDosage == TakeDosage.BEFORE ? "Before" : "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.ADDNOTES]] = PrescriptionFromDB.AdditionalNotes;

                        PrescriptionTable.Rows.Add(PrescriptionList);
                        rn++;
                    }
                }
            }
            else
            {
                IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(NoteId);
                if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                {
                    foreach (ConsultedPrescription ConsPres in lConsultedPrescription)
                    {
                        Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById((long)ConsPres.PrescriptionId!);
                        PrescriptionList = PrescriptionTable.NewRow();
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.NAME]] = PrescriptionFromDB.Product!=null? PrescriptionFromDB.Product.Name: PrescriptionFromDB.CustomProduct;
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.TOTAL]] = PrescriptionFromDB.Total.ToString();
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.DAYS]] = PrescriptionFromDB.Days.ToString();
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.HOURS]] = PrescriptionFromDB.Hours.ToString();
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.MORNING]] = PrescriptionFromDB.Morning.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.AFTERNOON]] = PrescriptionFromDB.Afternoon.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.EVENING]] = PrescriptionFromDB.Evening.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.NIGHT]] = PrescriptionFromDB.Night.ToString() ?? "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.BEFOREAFTER]] = PrescriptionFromDB.TakeDosage == TakeDosage.AFTER ? "After" : PrescriptionFromDB.TakeDosage == TakeDosage.BEFORE ? "Before" : "";
                        PrescriptionList[PrescriptionColumn[(int)PrescriptionDetailsGridColumn.ADDNOTES]] = PrescriptionFromDB.AdditionalNotes;

                        PrescriptionTable.Rows.Add(PrescriptionList);
                        rn++;
                    }
                }
            }
            return PrescriptionTable;
        }

        public void GeneratePrescriptionPreviewNEW(DataTable DataTable, long PatientId, long NoteId, bool isPrint, string ReportName, long PresicNos, string By,  byte[] CheckedImg, byte[] UnCheckedImg, string PrintPaperFormat)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc;
                double A5LandscapeHeight = 0;
                double A4Height = 0;

                if (PrintPaperFormat == "A5 LANDSCAPE")
                {
                    A5LandscapeHeight = 300;
                    pdfDoc = new Document(PageSize.A5.Rotate(), -45, -45, 20, 20);
                }
                else
                {
                    A4Height = 760;
                    pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                }
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                ConsultationNote Note = ConsultationNoteManager.Instance.GetConsultationNoteById(NoteId);
                HospitalConfiguration configuration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Patient.CompanyId);
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
                    PdfPTable PatientDetailsTable = PatientDetailsHeading(Patient, PresicNos, By);

                    PdfTableHeader BlankHeader = new PdfTableHeader();

                    PdfPTable BlankHeadTable = new PdfPTable(1);
                    PdfPCell blankRow = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    blankRow.UseVariableBorders = true;
                    blankRow.MinimumHeight = 3;
                    blankRow.BorderColorLeft = BaseColor.WHITE;
                    blankRow.BorderColorTop = BaseColor.GRAY;
                    blankRow.BorderColorRight = BaseColor.WHITE;
                    blankRow.BorderColorBottom = BaseColor.WHITE;
                    BlankHeadTable.AddCell(blankRow);

                    PdfTableHeader PdfTableHeader = new PdfTableHeader();

                    pdfDoc.Add(HTable);
                    pdfDoc.Add(PatientDetailsTable);
                    pdfDoc.Add(BlankHeadTable);
                    //RxSymbol Add
                    if (EmployeeDetails != null && EmployeeDetails.IsRxSymbolDisplayOn)
                    {
                        PdfPTable RxSymbolTable = new PdfPTable(1);
                        RxSymbolTable.TotalWidth = pdfDoc.PageSize.Width - 50f;
                        RxSymbolTable.LockedWidth = true;

                        float[] rxWidths = new float[] { 100f };
                        RxSymbolTable.SetWidths(rxWidths);

                        PdfPCell RxImageCell = new PdfPCell();
                        RxImageCell.Border = Rectangle.NO_BORDER;
                        RxImageCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        string resourceName = "Fa.Resources.Rx_Symbol.png";
                        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                        {
                            if (stream != null)
                            {
                                iTextSharp.text.Image RxImage = iTextSharp.text.Image.GetInstance(stream);
                                RxImage.ScaleToFit(20f, 20f);
                                RxImage.Alignment = Element.ALIGN_LEFT;
                                RxImageCell.AddElement(RxImage);
                            }
                            else
                            {
                                throw new Exception("Embedded resource not found.");
                            }
                        }
                        RxSymbolTable.AddCell(RxImageCell);

                        pdfDoc.Add(RxSymbolTable);
                    }
                    PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, "PrescriptionNew");
                    pdfDoc.Add(MTable);

                    int Cols = DataTable.Columns.Count;
                    int Rows = DataTable.Rows.Count;

                    PdfPTable ReportMainTable = new PdfPTable(10);
                    float[] widths = new float[] { 130f, 45f, 50f, 45f, 50f, 60f, 50f, 50f, 60f, 160f };


                    ReportMainTable.SetWidths(widths);

                    int k = 2;
                    int rowCount = 0;
                    BaseColor[] RowColor = new BaseColor[2];
                    RowColor[0] = new BaseColor(255, 255, 255);
                    RowColor[1] = new BaseColor(250, 250, 250);
                    Cursor.Current = Cursors.WaitCursor;

                    // Signature & Footer Details --------
                    PdfPTable signatureTable = new PdfPTable(2);
                    signatureTable.TotalWidth = pdfDoc.PageSize.Width - 50f;
                    signatureTable.LockedWidth = true;

                    float[] sigWidths = new float[] { 75f, 25f };
                    signatureTable.SetWidths(sigWidths);

                    // Add Footer Details Cell
                    PdfPCell footerCell;
                    if (configuration.IsFooterDetailsDisplayOnPresc)
                    {
                        if (!string.IsNullOrEmpty(configuration.FooterDetailsPrescroption))
                        {
                            footerCell = new PdfPCell(new Phrase(configuration.FooterDetailsPrescroption, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
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

                    if (configuration.IsDigitalSignatureDisplayOnPresc)
                    {
                        if (EmployeeDetails != null && EmployeeDetails.DigitalSignature != null)
                        {
                            iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(EmployeeDetails.DigitalSignature);
                            signatureImage.ScaleToFit(100f, 50f);
                            signatureImage.Alignment = Element.ALIGN_CENTER;
                            signatureCell.AddElement(signatureImage);
                        }
                    }

                    if (configuration.IsSignatureNameDisplayOnPresc)
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
                    //-------//
                    for (int i = 0; i < Rows; i++)
                    {
                        PdfPCell RowCell = new PdfPCell();
                        double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                        if ((PrintPaperFormat == "A4 PORTRAIT" && TotalWorkingOnPageH > A4Height) || (PrintPaperFormat == "A5 LANDSCAPE" && TotalWorkingOnPageH > A5LandscapeHeight) || (PrintPaperFormat == "A5 LANDSCAPE" && rowCount == 10))
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MiniHTable);
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(Cols);
                            ReportMainTable.SetWidths(widths);
                            rowCount = 0;
                            k = 2;
                        }
                        BaseColor CurRowColor = RowColor[k % 2];

                        for (int j = 0; j < Cols; j++)
                        {
                            var Temp = DataTable.Rows[i][j].ToString();
                            if (j == 0)
                            {
                                RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Bold_Italic_7_Black")));
                            }
                            else
                            {
                                RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                            }
                            RowCell.BorderColor = new BaseColor(160, 160, 160);
                            RowCell.BackgroundColor = CurRowColor;

                            if (DataTable.Columns[j].ColumnName == "Dosage" ||
                                DataTable.Columns[j].ColumnName == "Days" ||
                                DataTable.Columns[j].ColumnName == "Total" ||
                                DataTable.Columns[j].ColumnName == "Prescription" || 
                                DataTable.Columns[j].ColumnName == "Before/\nAfter Food" || 
                                DataTable.Columns[j].ColumnName == "Interval" || 
                                DataTable.Columns[j].ColumnName == "AdditionalNotes" )
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            else if (DataTable.Columns[j].ColumnName == "Morning" || DataTable.Columns[j].ColumnName == "Afternoon" || DataTable.Columns[j].ColumnName == "Evening" || DataTable.Columns[j].ColumnName == "Night")
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            else
                            {
                                RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            RowCell.MinimumHeight = 15;
                            ReportMainTable.AddCell(RowCell);
                        }
                        rowCount++;
                        k++;
                    }
                    Cursor.Current = Cursors.Default;

                    pdfDoc.Add(ReportMainTable);
                    if (writer.PageNumber > 1)
                    {
                        signatureTable.WriteSelectedRows(0, -1, 30f, 80f, writer.DirectContent);
                    }
                    pdfDoc.Close();
                    PdfFooter PdfFooter = new PdfFooter();
                    PdfFooter.IsReport = true;
                    PdfFooter.IsDate = true;
                    if (PrintPaperFormat == "A5 LANDSCAPE")
                    {
                        PdfFooter.IsA5LandScape = true;
                    }
                    PdfFooter.Text = string.Empty;
                    PdfFooter.IsPageNumber = true;
                    PdfFooter.PdfFile = myMemoryStream.ToArray();
                    byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                    myMemoryStream.Close();

                    Cursor.Current = Cursors.WaitCursor;
                    PdfGeneration PdfGeneration = new PdfGeneration();
                    PdfGeneration.IsPrint = isPrint;
                    PdfGeneration.FileName = "Prescription";
                    PdfGeneration.PdfFile = PdfFileWithFooter;
                    PdfGeneration.SavePdfFile();
                    Cursor.Current = Cursors.Default;
                }

            }
        }
        public static PdfPTable PatientDetailsHeading(Patient Patient, long PrescriptionNos, string PrescriptionBy)
        {
            int HeadColumns = 1;
            float[] HeadWidths = new float[] { 100f };

            PdfPTable MainHeadTable = new PdfPTable(2);
            MainHeadTable.SetTotalWidth(new float[] { 61, 39 });
            MainHeadTable.DefaultCell.BorderColor = BaseColor.GRAY;
            MainHeadTable.DefaultCell.BorderColorBottom = BaseColor.WHITE;

            PdfPTable HeadTable = new PdfPTable(HeadColumns);

            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);

            HeadCell = new PdfPCell(new Phrase("Patient Information", PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientDetails = "";

            PatientDetails = Patient.Name;

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            PatientDetails = "";
            if (Patient.Address != null && !string.IsNullOrEmpty(Patient.Address.FullAddress))
            {
                PatientDetails = Patient.Address.FullAddressInSingleLine;
            }
            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);
            MainHeadTable.AddCell(HeadTable);

            HeadColumns = 2;
            HeadWidths = new float[] { 38f, 62f };
            HeadTable = new PdfPTable(HeadColumns);
            HeadTable.SetWidths(HeadWidths);

            var PatientHeading = "Gender : ";

            PatientDetails = Patient.Gender.ToString();

            HeadCell = new PdfPCell(new Phrase(PatientHeading, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientHeadingDOB = "Age :";

            PatientDetails = Patient.Age != null ? Patient.Age.ToString() : "";

            HeadCell = new PdfPCell(new Phrase(PatientHeadingDOB, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientHeadingPhone = "";

            PatientDetails = "";

            if (Patient.ContactInfo != null && !string.IsNullOrEmpty(Patient.ContactInfo.Phone))
            {
                PatientHeadingPhone = "Phone : ";
                PatientDetails = Patient.ContactInfo.Phone;
            }
            else
            {
                PatientHeadingPhone = "Patient Id : ";
                PatientDetails = Patient.PatientNumber;
            }

            HeadCell = new PdfPCell(new Phrase(PatientHeadingPhone, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            PatientDetails = "";
            PatientHeadingPhone = "";
            if (Patient.ContactInfo != null && !string.IsNullOrEmpty(Patient.ContactInfo.Phone))
            {
                PatientHeadingPhone = "Patient Id : ";
                PatientDetails = Patient.PatientNumber;
            }

            HeadCell = new PdfPCell(new Phrase(PatientHeadingPhone, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientHeadingNos = "Prescription No :";

            if (PrescriptionNos != 0)
            {
                PatientDetails = PrescriptionNos.ToString();
            }
            else
            {
                PatientDetails = "";
            }

            HeadCell = new PdfPCell(new Phrase(PatientHeadingNos, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            var PatientHeadingBy = "Prescribed By :";

            PatientDetails = "Dr. " + PrescriptionBy.ToString();

            HeadCell = new PdfPCell(new Phrase(PatientHeadingBy, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(PatientDetails, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.UseVariableBorders = true;
            HeadCell.BorderColorLeft = BaseColor.WHITE;
            HeadCell.BorderColorTop = BaseColor.WHITE;
            HeadCell.BorderColorRight = BaseColor.WHITE;
            HeadCell.BorderColorBottom = BaseColor.WHITE;
            HeadCell.Colspan = 1;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);
            MainHeadTable.AddCell(HeadTable);

            PatientDetails = "";

            return MainHeadTable;
        }
    }
}
