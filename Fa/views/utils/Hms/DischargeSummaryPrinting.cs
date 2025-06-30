using fa.api.Accounting;
using fa.api.Hms;
using fa.model.Employee;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.views.utils.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace fa.views.utils.Hms
{
    public class DischargeSummaryPrinting
    {
        public void GenerateDischargeSummary(long PatientId, long PatientIpId ,byte[] CheckedImg, byte[] UnCheckedImg, bool IncludeChart, bool IncludeFullChart, bool IncludeIPChart)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -30, -30, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                if (Patient != null)
                {
                    pdfDoc.Add(LoadPdfHeader(true));
                    pdfDoc = LoadDisSummaryTable(pdfDoc,Patient, PatientIpId);
                    IList<DischargePrescription> DischargePrescription = DischargeNoteManager.Instance.ListDischargePrescriptionByPatientIpId(PatientIpId);
                    if (DischargePrescription != null && DischargePrescription.Count > 0)
                    {
                        pdfDoc.Add(LoadPrescriptionTable(DischargePrescription, CheckedImg, UnCheckedImg));
                    }
                    pdfDoc = LoadAuthorizedSignature(pdfDoc);
                }
                if (IncludeIPChart)
                {
                    InPatientAdmission InPatientAdmissionInfo = IpManager.Instance.GetInPatientAdmissionById(PatientIpId);
                    PatientIpChartPrinting PatientIpChartPrinting = new PatientIpChartPrinting()
                    {
                        PatientId = PatientId,
                        PatientIpId = PatientIpId
                    };
                    PatientIpChartPrinting.CreateIpChartPages(pdfDoc, Patient, InPatientAdmissionInfo);

                }
                if (IncludeChart)
                {                                       
                    List<string> Pages = new List<string>() { "Patient Details", "Insurance Details", "Medical History", "Prescription History", "LabTest History" };
                    PatientChartPrinting PatientChartPrinting = new PatientChartPrinting()
                    {
                        PatientId = PatientId,
                        PatientIpId = PatientIpId,
                        Pages=Pages,
                        CheckedImg = CheckedImg,
                        UnCheckedImg = UnCheckedImg,
                        IncludeFullChart = IncludeFullChart
                    };
                    PatientChartPrinting.CreateChartPages(pdfDoc, Patient);
                }                
                pdfDoc.Close();

                PdfFooter PdfFooter = new PdfFooter();
                PdfFooter.IsReport = true;
                PdfFooter.IsDate = true;
                PdfFooter.Text = string.Empty;
                PdfFooter.IsPageNumber = true;
                PdfFooter.PdfFile = myMemoryStream.ToArray();
                byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                myMemoryStream.Close();

                PdfGeneration PdfGeneration = new PdfGeneration();
                PdfGeneration.IsPrint = true;
                PdfGeneration.FileName = "Discharge Summary";
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            }
        }
        private PdfPTable LoadPdfHeader(bool IsMainHeader)
        {          
            PdfPageHeader PdfHeader = new PdfPageHeader()
            {
                IsMainHeader = IsMainHeader,
                Islogo = true,
                IsAddress = true,
                IsPhone = IsMainHeader,
                IsEmail = IsMainHeader,
                IsWebsite = IsMainHeader,
                IsLicenceInfo = IsMainHeader,
                ReportLine1 = "DISCHARGE SUMMARY"
            };
            return PdfHeader.PageHeader();
        }
        private Document AddNewPageContent(Document pdfDoc, PdfPTable ReportMainTable)
        {
            pdfDoc.Add(ReportMainTable);
            return pdfDoc;
        }
        private PdfPTable AddNewTableContent()
        {
            PdfPTable Table = new PdfPTable(4);
            float[] widths = new float[] { 45f, 55f, 45f, 55f };
            Table.SetWidths(widths);
            return Table;
        }
        private Document LoadDisSummaryTable(Document pdfDoc,Patient Patient,long PatientIpId)
        {
            InPatientAdmission InPatientAdmissionInfo = IpManager.Instance.GetInPatientAdmissionById(PatientIpId);
            DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(PatientIpId);
            Employee employee = EmployeeManager.Instance.GetEmployeeInfoById(DischargeNote.EmployeeId);
            InPatientLocation inPatientLocation = IpManager.Instance.GetInPatientAdmissionByPatientIpId(PatientIpId);
            PdfPTable ReportMainTable = AddNewTableContent();
            ReportMainTable.SplitLate = false;
            ReportMainTable.SplitRows = true;

            PdfPCell rowCell = new PdfPCell();
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder(" ", 4, PdfDataAlignment.GetFont("Font_Normal_Italic_5_Black")));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Patient Id", 1, 0.5f));
            ReportMainTable.AddCell(CreateCells(Patient.PatientNumber, 1, 0.5f));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Admission Date", 1, 0.5f, 0f));
            ReportMainTable.AddCell(CreateCells(InPatientAdmissionInfo.DateOfAdmission.ToString(Global.Company.DateFormat)+" "+InPatientAdmissionInfo.DateOfAdmission.ToShortTimeString(), 1, 0.5f));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Name", 1));
            ReportMainTable.AddCell(CreateCells(Patient.Name, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Gender", 1, 0f, 0f));
            ReportMainTable.AddCell(CreateCells("" + Patient.Gender, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("DOB/Age", 1));
            ReportMainTable.AddCell(CreateCells(Patient.DateOfBirth.ToShortDateString() + " [" + Patient.Age.ToString() + " Y]", 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("OP Number", 1, 0f, 0f));
            ReportMainTable.AddCell(CreateCells(InPatientAdmissionInfo.OpRegistration.PatientOPNumber?? string.Empty, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("IP Number", 1));
            ReportMainTable.AddCell(CreateCells(InPatientAdmissionInfo.PatientIPNumber ?? string.Empty, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Primary Doctor", 1, 0f, 0f));
            ReportMainTable.AddCell(CreateCells(InPatientAdmissionInfo.CurrentMedicalTeam.PrimaryDoctor.Name, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Ward/Bed", 1));
            ReportMainTable.AddCell(CreateCells(inPatientLocation.Ward.Name + "/" + inPatientLocation.Bed.Name, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Discharge On", 1, 0f, 0f));
            ReportMainTable.AddCell(CreateCells(DischargeNote!=null?DischargeNote.DischargeOn!=null?((DateTime)DischargeNote.DischargeOn).Date.ToString(Global.Company.DateFormat) :"" : "", 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Department", 1));
            ReportMainTable.AddCell(CreateCells(employee.Department.Name, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Discharged By", 1, 0f, 0f));
            ReportMainTable.AddCell(CreateCells(DischargeNote!.Employee.Name, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Address", 1));
            ReportMainTable.AddCell(CreateCells(Patient.Address.FullAddressInSingleLine, 1));
            ReportMainTable.AddCell(CreateCellWithBackColourAndBold("Next visit Date", 1, 0f, 0f));
            ReportMainTable.AddCell(CreateCells(DischargeNote != null ? DischargeNote.NextFollowUp != null ? ((DateTime)DischargeNote.NextFollowUp).Date.ToString(Global.Company.DateFormat) : "" : "", 1));
            if (DischargeNote != null)
            {
                for (int i = 1; i < 4; i++)
                {
                    if (i > 1)
                    {
                        ReportMainTable.AddCell(CreateDummyRow(4, 10));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderandBold(i == 2 ? "Treatment Summary :" : "Discharge Summary :", 4));
                        AddSplitTextCells(ReportMainTable, i == 2 ? DischargeNote.TreatmentSummary : DischargeNote.DischargeSummary, 4, 10000);
                    }
                    else
                    {
                        ReportMainTable.AddCell(CreateDummyRow(4, 5));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithNOBoarderandBold("Diagnosis Summary :", 4));
                        AddSplitTextCells(ReportMainTable, DischargeNote.DiagnosisSummary, 4, 10000);
                    }
                }
            }
            pdfDoc.Add(ReportMainTable);
            return pdfDoc;
        }

        private PdfPTable LoadPrescriptionTable(IList<DischargePrescription> DischargePrescription, byte[] CheckedImg, byte[] UnCheckedImg)
        {
            iTextSharp.text.Image Ckimage = iTextSharp.text.Image.GetInstance(CheckedImg);
            iTextSharp.text.Image UnCkimage = iTextSharp.text.Image.GetInstance(UnCheckedImg);
            Ckimage.ScaleToFit(200f, 20f);
            Ckimage.ScaleAbsolute(15, 15);
            UnCkimage.ScaleToFit(200f, 20f);
            UnCkimage.ScaleAbsolute(15, 15);

            PdfPTable ReportMainTable = new PdfPTable(10);
            float[] widths = new float[] { 150f, 40f, 30f, 35f, 45f, 45f, 44f, 40f, 60f, 100f };
            ReportMainTable.SetWidths(widths);
            ReportMainTable.AddCell(CreateCellWithoutBoarderAndBold("Discharge Prescription Summary :", 9, 20));
            ReportMainTable.AddCell(CreateCellWithoutBoarderAndBold("", 9, 7));

            ReportMainTable.AddCell(CreateCellWithBackColours("Prescription", 1));
            ReportMainTable.AddCell(CreateCellWithBackColours("Total", 1));
            ReportMainTable.AddCell(CreateCellWithBackColours(("Days"), 1));
            ReportMainTable.AddCell(CreateCellWithBackColours("Hours", 1));
            ReportMainTable.AddCell(CreateCellWithBackColours("Morning", 1));
            ReportMainTable.AddCell(CreateCellWithBackColours(("Aft. Noon"), 1));
            ReportMainTable.AddCell(CreateCellWithBackColours(("Evening"), 1));
            ReportMainTable.AddCell(CreateCellWithBackColours(("Night"), 1));
            ReportMainTable.AddCell(CreateCellWithBackColours(("B/A Food"), 1));
            ReportMainTable.AddCell(CreateCellWithBackColours(("Notes"), 1 , 0.0f));

            string PrescriptionDetails = string.Empty;
            foreach (DischargePrescription ConsPres in DischargePrescription)
            {
                Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById(ConsPres.PrescriptionId);
                //need page break check
                ReportMainTable.AddCell(CreatePrescriptionCells((PrescriptionFromDB.Product!=null?PrescriptionFromDB.Product.Name: PrescriptionFromDB.CustomProduct), 1));
                ReportMainTable.AddCell(CreatePrescriptionCells((PrescriptionFromDB.Total), 1));
                ReportMainTable.AddCell(CreatePrescriptionCells((PrescriptionFromDB.Days.ToString()), 1));
                ReportMainTable.AddCell(CreatePrescriptionCells((PrescriptionFromDB.Hours), 1));
                ReportMainTable.AddCell(CreatePrescriptionCellsWithMiddleAlign((PrescriptionFromDB.Morning), 1));
                ReportMainTable.AddCell(CreatePrescriptionCellsWithMiddleAlign((PrescriptionFromDB.Afternoon), 1));
                ReportMainTable.AddCell(CreatePrescriptionCellsWithMiddleAlign((PrescriptionFromDB.Evening), 1));
                ReportMainTable.AddCell(CreatePrescriptionCellsWithMiddleAlign((PrescriptionFromDB.Night), 1));
                ReportMainTable.AddCell(CreatePrescriptionCells((PrescriptionFromDB.TakeDosage == TakeDosage.AFTER ? "After Food" : "Before Food"), 1));
                ReportMainTable.AddCell(CreatePrescriptionCells((PrescriptionFromDB.AdditionalNotes), 1,0.0f));
            }
            return ReportMainTable;
        }
        private Document LoadAuthorizedSignature(Document pdfDoc)
        {
            PdfPTable ReportMainTable = AddNewTableContent();

            ReportMainTable.AddCell(CreateDummyRow(4, 25));
            pdfDoc = AddNewPageContent(pdfDoc, ReportMainTable);
            ReportMainTable = AddNewTableContent();
            ReportMainTable.AddCell(CreateHeadingCellWithSpan("For " + Global.Company.Name, 4, 4, 0));
            pdfDoc.Add(ReportMainTable);

            ReportMainTable = AddNewTableContent();
            ReportMainTable.AddCell(CreateDummyRow(4, 35));
            pdfDoc = AddNewPageContent(pdfDoc, ReportMainTable);

            ReportMainTable = AddNewTableContent();
            ReportMainTable.AddCell(CreateHeadingCellWithSpan("Authorized Signatory", 4, 4, 30));
            pdfDoc.Add(ReportMainTable);
            return pdfDoc;
        }
        public static PdfPCell CreateCells(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCells(string text, int span, float Border)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = Border;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColourAndBold(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColourAndBold(string text, int span, float BorderTop)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = BorderTop;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColourAndBold(string text, int span, float BorderTop, float BorderLeft)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthTop = BorderTop;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            rowCell.NoWrap = false;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColours(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellWithBackColours(string text, int span, float BorderRight)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BackgroundColor = new BaseColor(230, 230, 230);
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateCellWithoutBoarderAndBold(string text, int span, float height)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.MinimumHeight = height;
            rowCell.Colspan = span;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
            rowCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            return rowCell;
        }
        public static PdfPCell CreateDummyRow(int span, int hieght)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_4_Black")));
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.MinimumHeight = hieght;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateHeadingCellWithSpan(string text, int cspan, int rspan, float padingRight)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = BaseColor.WHITE;
            rowCell.BorderWidthLeft = (float)BorderStyle.None;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.BorderWidthBottom = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.PaddingRight = padingRight;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = cspan;
            rowCell.Rowspan = 2;
            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            rowCell.VerticalAlignment = Element.ALIGN_TOP;
            return rowCell;
        }
        public static PdfPCell CreatePrescriptionCells(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreatePrescriptionCellsWithMiddleAlign(string text, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
            return rowCell;
        }
        public static PdfPCell CreatePrescriptionCells(string text, int span, float BorderLeft)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(new Phrase(text, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        public static PdfPCell CreateImageCellWithTopBlackBorder(iTextSharp.text.Image Img, int span)
        {
            PdfPCell rowCell = new PdfPCell();
            rowCell = new PdfPCell(Img);
            rowCell.BorderColor = new BaseColor(160, 160, 160);
            rowCell.BorderWidthBottom = 0.5f;
            rowCell.BorderWidthTop = (float)BorderStyle.None;
            rowCell.BorderWidthRight = (float)BorderStyle.None;
            rowCell.Padding = 2;
            rowCell.MinimumHeight = 20;
            rowCell.Colspan = span;
            return rowCell;
        }
        
        public static void AddSplitTextCells(PdfPTable table, string text, int span, int maxCharactersPerCell)
        {
            List<string> splitText = SplitText(text, maxCharactersPerCell);
            foreach (var part in splitText)
            {
                PdfPCell cell = new PdfPCell(new Phrase(part, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")))
                {
                    BorderColor = new BaseColor(160, 160, 160),
                    BorderWidth = (float)BorderStyle.None,
                    Padding = 2,
                    MinimumHeight = 14,
                    Colspan = span,
                    NoWrap = false,
                    HorizontalAlignment = Element.ALIGN_JUSTIFIED
                };
                table.AddCell(cell);
            }
        }
        public static List<string> SplitText(string text, int maxCharacters)
        {
            List<string> result = new List<string>();
            int start = 0;
            while (start < text.Length)
            {
                int length = Math.Min(maxCharacters, text.Length - start);
                result.Add(text.Substring(start, length));
                start += length;
            }
            return result;
        }
    }
}
