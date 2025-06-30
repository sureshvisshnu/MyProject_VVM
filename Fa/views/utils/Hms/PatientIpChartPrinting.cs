using fa.api.catalog;
using fa.api.Hms;
using fa.model.Catalog;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fa.views.utils.Hms
{
    public class PatientIpChartPrinting
    {
        public long PatientId = 0L;
        public long PatientIpId = 0L;
        public bool IncludeChart;
        public bool IncludeFullChart;
        public byte[] CheckedImg;
        public byte[] UnCheckedImg;
        DischargeSummaryPrinting DischargeSummaryPrinting = new DischargeSummaryPrinting();
        public void GenerateIpChart()
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -60, -60, 35, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                if (Patient != null)
                {
                    InPatientAdmission InPatientAdmissionInfo = IpManager.Instance.GetInPatientAdmissionById(PatientIpId);
                    if (InPatientAdmissionInfo != null)
                    {                        
                        CreateIpChartPages(pdfDoc, Patient, InPatientAdmissionInfo);
                    }
                    if (IncludeChart)
                    {
                        List<string> Pages = new List<string>() { "Patient Details", "Insurance Details", "Medical History", "Prescription History", "LabTest History" };
                        PatientChartPrinting PatientChartPrinting = new PatientChartPrinting()
                        {
                            PatientIpId = PatientIpId,
                            Pages = Pages,
                            CheckedImg = CheckedImg,
                            UnCheckedImg = UnCheckedImg,
                            IncludeFullChart = IncludeFullChart
                        };
                        PatientChartPrinting.CreateChartPages(pdfDoc, Patient);
                    }
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
                PdfGeneration.FileName = "Ip Chart";
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
                ReportLine1 = "PATIENT IP CHART"
            };
            return PdfHeader.PageHeader();
        }
        private PdfPTable AddNewTableContent()
        {
            PdfPTable Table = new PdfPTable(4);
            float[] widths = new float[] { 45f, 55f, 45f, 55f };
            Table.SetWidths(widths);
            return Table;
        }
        public Document LoadDisSummaryTable(Document pdfDoc, Patient Patient, InPatientAdmission InPatientAdmissionInfo)
        {
            DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId(PatientIpId);
            InPatientLocation inPatientLocation = IpManager.Instance.GetInPatientAdmissionByPatientIpId(PatientIpId);
            PdfPTable ReportMainTable = AddNewTableContent();
            PdfPCell rowCell = new PdfPCell();
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder(" ", 4, PdfDataAlignment.GetFont("Font_Normal_Italic_5_Black")));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Patient Id", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(Patient.PatientNumber, 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Admission Date", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(InPatientAdmissionInfo.DateOfAdmission.ToString(Global.Company.DateFormat) + " " + InPatientAdmissionInfo.DateOfAdmission.ToShortTimeString(), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Name", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(Patient.Name, 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Ward/Bed", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(inPatientLocation.Ward.Name + "/" + inPatientLocation.Bed.Name, 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Sex", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell("" + Patient.Gender, 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Discharge On", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(DischargeNote != null ? DischargeNote.DischargeOn != null ? ((DateTime)DischargeNote.DischargeOn).ToString(Global.Company.DateFormat) + " " + ((DateTime)DischargeNote.DischargeOn).ToShortTimeString() : "" : "", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("DOB/Age", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(Patient.DateOfBirth.ToShortDateString() + " [" + Patient.Age.ToString() + " Y]", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Department", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell("", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Address", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(Patient.Address.FullAddress, 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColourandBold("Primary Doctor", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(InPatientAdmissionInfo.CurrentMedicalTeam.PrimaryDoctor.Name, 1));
            pdfDoc.Add(ReportMainTable);
            return pdfDoc;
        }
        public void CreateIpChartPages(Document pdfDoc, Patient Patient, InPatientAdmission InPatientAdmissionInfo)
        {
            pdfDoc.NewPage();
            pdfDoc.Add(LoadPdfHeader(true));
            pdfDoc = LoadDisSummaryTable(pdfDoc, Patient, InPatientAdmissionInfo);
            IList<ConsultationNote> lConsultationNote = ConsultationNoteManager.Instance.ListConsultationNoteByOPIdIPId(InPatientAdmissionInfo.OpRegistrationId, InPatientAdmissionInfo.Id);
            if (lConsultationNote != null)
            {
                PdfPTable ReportMainTable = new PdfPTable(7);
                float[] widths = new float[] { 35f, 15f, 80f, 50f, 50f, 50f, 50f };
                ReportMainTable.SetWidths(widths);
                PdfPCell rowCell = new PdfPCell();
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder1("Recent IP Chart Details", 7));
                //rowCell = PdfDataAlignment.CreateHeadingCell("", 7);
                //ReportMainTable.AddCell(rowCell);
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColour("Date", 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColour("Visit", 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColour(("Note"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColour("Diagnosis", 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColour("Prescription", 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColour(("Lab Test"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColour(("Procedure"), 1));

                foreach (ConsultationNote ConsultationNote in lConsultationNote)
                {
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCell(ConsultationNote.Date.ToString(Global.Company.DateFormat), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCell((ConsultationNote.InPatientAdmissionId != null ? "IP" : "OP"), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCell(ConsultationNote.Note, 1));
                    //symptoms
                    IList<ConsultedSymptom> ConsultedSymptoms = ConsultationNoteManager.Instance.ListSymptomByNoteId(ConsultationNote.Id);
                    if (ConsultedSymptoms.Count > 0)
                    {                   
                        string SymptomsName = string.Empty;
                        foreach (ConsultedSymptom ConsultedSymptom in ConsultedSymptoms)
                        {
                            SymptomsName = string.IsNullOrEmpty(SymptomsName) ? ConsultedSymptom.Symptom.Name : SymptomsName + ",\n" + ConsultedSymptom.Symptom.Name;
                        }
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCell(SymptomsName, 1));
                    }
                    else
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCell("", 1));
                    }
                    //prescription
                    if (ConsultationNote.IsDischarged)
                    {
                        IList<DischargePrescription> DischargePrescription = DischargeNoteManager.Instance.ListDischargePrescriptionByPatientIpId((long)ConsultationNote.InPatientAdmissionId);
                        if (DischargePrescription != null && DischargePrescription.Count > 0)
                        {
                            string PrescriptionsName = string.Empty;
                            foreach (DischargePrescription DisPrescription in DischargePrescription)
                            {
                                if (!(bool)DisPrescription.IsCustomPrescription!)
                                {
                                    Product Product = CatalogProductManager.Instance.GetProductInfoById((long)DisPrescription.Prescription.ProductId);
                                    PrescriptionsName = string.IsNullOrEmpty(PrescriptionsName) ? Product.Name : PrescriptionsName + ",\n" + Product.Name;
                                }
                                else
                                {
                                    PrescriptionsName = DisPrescription.CustomPrescription;
                                }
                            }
                            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(PrescriptionsName, 1));
                        }
                        else
                        {
                            ReportMainTable.AddCell(PdfDataAlignment.CreateCell("", 1));
                        }
                    }
                    else
                    {                      
                        IList<ConsultedPrescription> ConsultedPrescriptions = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(ConsultationNote.Id);
                        if (ConsultedPrescriptions.Count > 0)
                        {

                            string PrescriptionsName = string.Empty;
                            foreach (ConsultedPrescription ConsultedPrescription in ConsultedPrescriptions)
                            {
                                if (ConsultedPrescription.Prescription.ProductId != null)
                                {
                                    Product Product = CatalogProductManager.Instance.GetProductInfoById((long)ConsultedPrescription.Prescription.ProductId);
                                    PrescriptionsName = string.IsNullOrEmpty(PrescriptionsName) ? Product.Name : PrescriptionsName + ",\n" + Product.Name;
                                }
                                else
                                {
                                    PrescriptionsName = ConsultedPrescription.CustomPrescription;
                                }
                            }
                            ReportMainTable.AddCell(PdfDataAlignment.CreateCell(PrescriptionsName, 1));
                        }
                        else
                        {
                            ReportMainTable.AddCell(PdfDataAlignment.CreateCell("", 1));
                        }
                    }
                    //labtest
                    IList<ConsultedLabTest> ConsultedLabTests = ConsultationNoteManager.Instance.ListLabTestByNoteId(ConsultationNote.Id);
                    if (ConsultedLabTests.Count > 0)
                    {
                        string LabTestsName = string.Empty;
                        foreach (ConsultedLabTest ConsultedLabTest in ConsultedLabTests)
                        {
                            LabTestsName = string.IsNullOrEmpty(LabTestsName) ? ConsultedLabTest.MedicalTest.Name : LabTestsName + ",\n" + ConsultedLabTest.MedicalTest.Name;
                        }
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCell(LabTestsName, 1));
                    }
                    else
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCell("", 1));
                    }

                    // Procedure
                    IList<ConsultedProcedure> ConsultedProcedures = ConsultationNoteManager.Instance.ListProcedureByNoteId(ConsultationNote.Id);
                    if (ConsultedProcedures != null && ConsultedProcedures.Count > 0)
                    {
                        
                        string ProcedureName = string.Empty;
                        foreach (ConsultedProcedure ConsultedProcedure in ConsultedProcedures)
                        {
                            ProcedureName = string.IsNullOrEmpty(ProcedureName) ? ConsultedProcedure.MedicalProcedure.Name : ProcedureName + ",\n" + ConsultedProcedure.MedicalProcedure.Name;
                        }
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCell(ProcedureName, 1));
                    }
                    else
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCell("", 1));
                    }
                }
                pdfDoc.Add(ReportMainTable);
            }
        }
    }
}
