using fa.api.Hms;
using fa.model.Hms.Master;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using fa.model.hms.common;
using Fa.api.Hms;
using fa.views.utils.Common;
using fa.model.Hms.common;
using System.Windows.Forms;
using System.Linq;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Text;
using FaData.Utils;
using Fa.reports.sales;
using fa.model.Hms.Op;
using VisioForge.Libs.DirectShowLib;

namespace fa.views.utils.Hms
{
    public class PatientChartPrinting
    {
        public long PatientId = 0L;
        public long PatientIpId = 0L;
        public List<string> Pages = new List<string>();
        public byte[]? CheckedImg;
        public byte[]? UnCheckedImg;
        public bool IncludeFullChart;
        public bool IncludeIPChart;
        public bool IsConsuting;
        public string? ConsultingFileName;
        public string? ConsultingFilePath;
        long? IpId = null!;

        public void GenerateChart()
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                Patient Patient = PatientManager.Instance.GetPatientById(PatientId);
                if (Patient != null)
                {
                    pdfDoc.Add(LoadPdfHeader(true));
                    CreateChartPages(pdfDoc, Patient);
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
                PdfGeneration.FileName = "Chart";
                if (IsConsuting == true)
                {
                    PdfGeneration.FileName = "PatientChart";
                    PdfGeneration.IsPrint = false;
                }
                else
                {
                    PdfGeneration.IsPrint = true;
                }
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
                if (IsConsuting == true)
                {
                    ConsultingFileName = PdfGeneration.ConsultingFileName;
                    ConsultingFilePath = PdfGeneration.ConsultingFilePath;
                }
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            }
        }
        public void CreateChartPages(Document pdfDoc, Patient Patient)
        {
            if (IncludeFullChart)
            {
                if (IsConsuting || (!IsConsuting && Pages.Contains("Patient Details")))
                {
                    LoadPdfDoc(pdfDoc, Patient, "Patient Details", false, null, null);
                }
                else
                {
                    PdfPTable ReportMainTable = new PdfPTable(2);
                    float[] widths = new float[] { 200f, 60f };
                    ReportMainTable.SetWidths(widths);
                    PdfPCell rowCell = new PdfPCell();
                    rowCell = PdfDataAlignment.CreateHeadingCellPatientChart("Patient Details", 3);
                    ReportMainTable.AddCell(rowCell);
                    ReportMainTable.AddCell(PdfDataAlignment.PatientInfoFristCell(("Name      : " + Patient.Name) + "\n" + ("Address   : " + Patient.Address.FullAddressInSingleLine), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.PatientInfoSecoundCell(("Gender  : " + Patient.Gender) + "\n" + ("DOB       : " + Patient.DateOfBirth.ToShortDateString()) + "\n" + ("Phone    : " + Patient.ContactInfo.Phone), 2));

                    ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 3, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    pdfDoc.Add(ReportMainTable);
                }
                if (Patient.InsuranceInfo != null && Patient.InsuranceInfo.Count > 0)
                {
                    if (IsConsuting || (!IsConsuting && Pages.Contains("Insurance Details")))
                    {
                        LoadPdfDoc(pdfDoc, Patient, "Insurance Details", false, null, null);
                    }
                }
                IList<Vital> Vitals = (IList<Vital>)VitalEntryManager.Instance.ListVitalEntryByPatientId(Patient.Id);
                if (Vitals != null && Vitals.Count > 0)
                {
                    LoadPdfDoc(pdfDoc, Patient, "Vital's History", false, null, Vitals);
                }
                if (IsConsuting || (!IsConsuting && Pages.Contains("Medical History")))
                {
                    LoadPdfDoc(pdfDoc, Patient, "Medical History", false, null, null);
                }
                IList<ConsultationNote> consultationNotes = ConsultationNoteManager.Instance.ListNotesEntryByPatientId(Patient.Id);
                if (consultationNotes != null && consultationNotes.Count > 0)
                {
                    foreach (ConsultationNote note in consultationNotes)
                    {
                        IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(note.Id);
                        IList<ConsultedLabTest> lConsultedLabTest = ConsultationNoteManager.Instance.ListLabTestByNoteId(note.Id);
                        IList<ConsultedSymptom> lConsultedSymptom = ConsultationNoteManager.Instance.ListSymptomByNoteId(note.Id);
                        IList<ConsultedProcedure> lConsultedProcedure = ConsultationNoteManager.Instance.LisConsultedProcedureByNoteId(note.Id);

                        if (Pages.Contains("Prescription History") && !Pages.Contains("LabTest History") && !Pages.Contains("Diagnosis History") && !Pages.Contains("Procedure History"))
                        {
                            if (lConsultedPrescription.Count > 0)
                            {
                                LoadSubHeadings(pdfDoc, note);
                            }
                        }
                        else if (!Pages.Contains("Prescription History") && Pages.Contains("LabTest History") && !Pages.Contains("Diagnosis History") && !Pages.Contains("Procedure History"))
                        {
                            if (lConsultedLabTest.Count > 0)
                            {
                                LoadSubHeadings(pdfDoc, note);
                            }
                        }
                        else if (!Pages.Contains("Prescription History") && !Pages.Contains("LabTest History") && Pages.Contains("Diagnosis History") && !Pages.Contains("Procedure History"))
                        {
                            if (lConsultedSymptom.Count > 0)
                            {
                                LoadSubHeadings(pdfDoc, note);
                            }
                        }
                        else if (!Pages.Contains("Prescription History") && !Pages.Contains("LabTest History") && !Pages.Contains("Diagnosis History") && Pages.Contains("Procedure History"))
                        {
                            if (lConsultedProcedure.Count > 0)
                            {
                                LoadSubHeadings(pdfDoc, note);
                            }
                        }
                        else
                        {
                            LoadSubHeadings(pdfDoc, note);
                        }

                        if (IsConsuting || (!IsConsuting && (Pages.Contains("Diagnosis History") || Pages.Contains("Procedure History"))))
                        {
                            LoadPdfDoc(pdfDoc, Patient, "Note", false, note, null);
                        }

                        //IList<ConsultationNote> consultationNotesPH = ConsultationNoteManager.Instance.ListNotesEntryByPatientId(Patient.Id);
                        //foreach (ConsultationNote noteForPH in consultationNotes)
                        //{
                            if (note.Note != "Patient got Discharged")
                            {
                                if (Pages.Contains("Prescription History"))
                                {
                                    //IList<ConsultedPrescription> ConsultedPrescriptions = ConsultationNoteManager.Instance.ListConsPrescriptionByPatientId(Patient.Id);
                                    if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                                    {
                                        LoadPdfDoc(pdfDoc, Patient, "Prescription History", false, note, null);
                                    }
                                }
                            }
                        //}
                        //foreach (ConsultationNote noteForPH in consultationNotes)
                        //{
                        if(note.InPatientAdmissionId != null)
                        {
                            if (note.Note == "Patient got Discharged" && Pages.Contains("Prescription History"))
                            {
                                //IList<ConsultedPrescription> ConsultedPrescriptions = ConsultationNoteManager.Instance.ListConsPrescriptionByPatientId(Patient.Id);
                                DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId((long)note.InPatientAdmissionId);
                                if (DischargeNote != null)
                                {
                                    LoadPdfDoc(pdfDoc, Patient, "Discharge Prescription History", false, note, null);
                                }
                            }
                        }
                        //}
                        if (Pages.Contains("LabTest History"))
                        {
                            //IList<ConsultedLabTest> ConsultedLabTests = ConsultationNoteManager.Instance.ListConsLabTestByPatientId(Patient.Id);
                            if (lConsultedLabTest != null && lConsultedLabTest.Count > 0)
                            {
                                LoadPdfDoc(pdfDoc, Patient, "LabTest History", false, note, null);
                            }
                        }
                    }
                }
            }
            else
            {
                LoadPdfDoc(pdfDoc, Patient, "Patient Details", true, null, null);
                IList<ConsultedPrescription> ConsultedPrescriptions = ConsultationNoteManager.Instance.ListConsPrescriptionByPatientIdForCurrentIp(Patient.Id, PatientIpId);
                if (ConsultedPrescriptions != null && ConsultedPrescriptions.Count > 0)
                {
                    LoadPdfDoc(pdfDoc, Patient, "Prescription History", true, null, null);
                }
                IList<ConsultedLabTest> ConsultedLabTests = ConsultationNoteManager.Instance.ListConsLabTestByPatientIdForCurrentIp(Patient.Id, PatientIpId);
                if (ConsultedLabTests != null && ConsultedLabTests.Count > 0)
                {
                    LoadPdfDoc(pdfDoc, Patient, "LabTest History", true, null, null);
                }
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
                ReportLine1 = "PATIENT CHART"
            };
            return PdfHeader.PageHeader();
        }
        private bool procedureHistoryHeadingAdded = false;
        private bool prescriptionHistoryHeadingAdded = false;
        private bool dischargePrescriptionSummaryHeadingAdded = false;
        private Document LoadPdfDoc(Document pdfDoc, Patient Patient, string Page, bool IsCurrentIp, ConsultationNote note, IList<Vital> vitals)
        {
            //pdfDoc.NewPage();
            iTextSharp.text.Image Ckimage = iTextSharp.text.Image.GetInstance(CheckedImg);
            iTextSharp.text.Image UnCkimage = iTextSharp.text.Image.GetInstance(UnCheckedImg);
            Ckimage.ScaleToFit(200f, 10f);
            Ckimage.ScaleAbsolute(14, 14);
            UnCkimage.ScaleToFit(200f, 10f);
            UnCkimage.ScaleAbsolute(17, 17);

            if (Page == "Patient Details")
            {
                PdfPTable ReportMainTable = new PdfPTable(2);
                float[] widths = new float[] { 200f, 60f };
                ReportMainTable.SetWidths(widths);
                PdfPCell rowCell = new PdfPCell();
                rowCell = PdfDataAlignment.CreateHeadingCellPatientChart("Patient Details", 3);
                ReportMainTable.AddCell(rowCell);
                ReportMainTable.AddCell(PdfDataAlignment.PatientInfoFristCell(("Name      : " + Patient.Name) + "\n" + ("Address   : " + Patient.Address.FullAddressInSingleLine), 1));
                ReportMainTable.AddCell(PdfDataAlignment.PatientInfoSecoundCell(("Gender  : " + Patient.Gender) + "\n" + ("DOB       : " + Patient.DateOfBirth.ToShortDateString()) + "\n" + ("Phone    : " + Patient.ContactInfo.Phone), 2));
                
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 3, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                if (Patient.Guardians.Count > 0)
                {
                    rowCell = PdfDataAlignment.CreateHeadingCellPatientChart("Guardian Details", 3);
                    ReportMainTable.AddCell(rowCell);
                    foreach (Guardian Info in Patient.Guardians)
                    {
                        Guardian Guardian = GuardianManager.Instance.GetGuardianById(Info.Id);
                        ReportMainTable.AddCell(PdfDataAlignment.PatientInfoFristCell(("Name      : " + (Guardian != null ? Guardian.Name : "")) + "\n" + ("Address  : " + (Guardian != null ? Guardian.Address.FullAddressInSingleLine : "")), 1));
                        ReportMainTable.AddCell(PdfDataAlignment.PatientInfoSecoundCell(("Gender  : " + (Guardian != null ? Guardian.Gender : "")) + "\n" + ("DOB       : " + (Guardian != null ? Guardian.DateOfBirth.ToShortDateString() : "")) + "\n" + ("Phone    : " + (Guardian != null && Guardian.ContactInfo.Phone != null ? Guardian.ContactInfo.Phone : "")), 2));
                        
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 3, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    }
                }
                if (Patient.ResponsibleParty != null)
                {
                    rowCell = PdfDataAlignment.CreateHeadingCellPatientChart("Responsible Party Detail", 3);
                    ReportMainTable.AddCell(rowCell);
                    ReportMainTable.AddCell(PdfDataAlignment.PatientInfoFristCell(("Name      : " + (Patient.ResponsibleParty != null ? Patient.ResponsibleParty.Name : "")), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.PatientInfoSecoundCell(("Gender  : " + (Patient.ResponsibleParty != null ? Patient.ResponsibleParty.Gender : "")) + "\n" + ("DOB       : " + (Patient.ResponsibleParty != null ? Patient.ResponsibleParty.DateOfBirth.ToShortDateString() : "")), 1));
                    
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 3, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                }
                if (Patient.EmergencyContact.Count > 0)
                {
                    rowCell = PdfDataAlignment.CreateHeadingCellPatientChart("Emergency Contact Details", 3);
                    ReportMainTable.AddCell(rowCell);
                    foreach (EmergencyContact Info in Patient.EmergencyContact)
                    {
                        EmergencyContact EmergencyContact = EmergencyContactManager.Instance.GetEmergencyContactById(Info.Id);
                        ReportMainTable.AddCell(PdfDataAlignment.PatientInfoFristCell(("Name      : " + (EmergencyContact != null ? EmergencyContact.Name : "")) + "\n" + ("Address  : " + (EmergencyContact != null ? EmergencyContact.Address.FullAddressInSingleLine : "")), 1));
                        ReportMainTable.AddCell(PdfDataAlignment.PatientInfoSecoundCell(("Gender  : " + (EmergencyContact != null ? EmergencyContact.Gender : "")) + "\n" + ("Phone    : " + (EmergencyContact != null && EmergencyContact.ContactInfo.Phone != null ? EmergencyContact.ContactInfo.Phone : "")), 1));
                        
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 3, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    }
                }
                pdfDoc.Add(ReportMainTable);
            }
            else if (Page == "Insurance Details")
            {
                PdfPTable ReportMainTable = new PdfPTable(7);
                float[] widths = new float[] { 30f, 30f, 30f, 30f, 25f, 20f, 20f };
                ReportMainTable.SetWidths(widths);
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder1("Insurance Details", 7, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Name"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("RelationShip"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Insurance Carrier"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Phone Number"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Group Number"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Policy Number"), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Primary"), 1));
                foreach (InsuranceInfo info in Patient.InsuranceInfo)
                {
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((info.InsuranceHolder.Name), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((info.InsuranceHolderRelationShip.ToString()), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((info.InsuranceName), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((info.EmployerPhone), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((info.GroupNumber), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((info.PolicyNumber), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((info.IsPrimary.ToString()), 1));
                }
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLabTest(" ", 7, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                pdfDoc.Add(ReportMainTable);
            }
            else if (Page == "Vital's History")
            {
                PdfPTable ReportMainTable = new PdfPTable(10);
                float[] widths = new float[] { 70f, 45f, 45f, 30f, 45f, 45f, 45f, 45f, 60f, 45f };
                ReportMainTable.SetWidths(widths);
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder1("Vital's History", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Date", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Height [cm]", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Weight [kg]", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("BMI", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Temp [°C]", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Pulse [bmp]", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Resp.Rate", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("B.Pressure", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("B.Oxygen Level[%]", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Entered By", 0));

                IList<Vital> Vitals = (IList<Vital>)VitalEntryManager.Instance.ListVitalEntryByPatientId(Patient.Id);
                if (Vitals.Count > 0)
                {
                    String dateTime = null;
                    foreach (Vital VitalEntry in Vitals)
                    {
                        String stringLineItemDate = DateUtils.FormatDate(VitalEntry.Date, Global.Company.DateFormat);
                        if (dateTime == null || dateTime != stringLineItemDate)
                        {
                            ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.Date.ToString(Global.Company.DateFormat), 0));
                            dateTime = stringLineItemDate;
                        }
                        else
                        {
                            ReportMainTable.AddCell(PdfDataAlignment.CreateCells("", 1));
                        }
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.Height.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.Weight.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.BMI.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.Temperature.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.Pulse.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.RespRate.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.BPressure.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.BOxyLevel.ToString(), 0));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells(VitalEntry.LastModifiedBy.ToString(), 0));
                    }
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLabTest(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                }
                pdfDoc.Add(ReportMainTable);
            }
            else if (Page == "Medical History")
            {
                PdfPTable ReportMainTable = new PdfPTable(6);
                float[] widths = new float[] { 5f, 20f, 5f, 30f, 5f, 30f };
                ReportMainTable.SetWidths(widths);
                IList<PatientPreMedicalHistory> Historys = PatientMedicalHistoryManager.Instance.ListAllPatientPreMedicalHistoryPatientId(Patient.Id);
                if(Historys.Count > 0)
                {
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder1("Medical History", 6, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                }
                IList<PatientHistoryQuestionGroup> PatientHistoryQuestionGroupInfo = PatientMedicalHistoryManager.Instance.ListAllPatientHistoryQuestionGroup();
                if (PatientHistoryQuestionGroupInfo.Count > 0)
                {
                    foreach (PatientHistoryQuestionGroup lPatientHistoryQuestionGroup in PatientHistoryQuestionGroupInfo.OrderBy(x => x.Name))
                    {
                        IList<PatientHistoryQuestion> PatientHistoryQuestionInfo = PatientMedicalHistoryManager.Instance.ListAllPatientHistoryQuestionsByQuestionGroupId(lPatientHistoryQuestionGroup.Id);
                        if (PatientHistoryQuestionInfo.Count > 0)
                        {
                            bool hasHistory = false;
                            StringBuilder historyDetails = new StringBuilder();
                            //int count = 1;

                            foreach (PatientHistoryQuestion lPatientHistoryQuestion in PatientHistoryQuestionInfo)
                            {
                                PatientPreMedicalHistory History = PatientMedicalHistoryManager.Instance.GetPatientPreMedicalHistoryByQuestionId(lPatientHistoryQuestion.Id, Patient.Id);
                                if (History != null && PatientId == History.PatientId)
                                {
                                    hasHistory = true;
                                    string detail = lPatientHistoryQuestion.AdditionalNotes ? $"{lPatientHistoryQuestion.Name} - {History.AdditionalValue}" : $" {lPatientHistoryQuestion.Name}";
                                    historyDetails.Append(detail).Append(", ");
                                    //count++;
                                }
                            }

                            if (hasHistory)
                            {
                                if (historyDetails.Length > 2)
                                {
                                    historyDetails.Length -= 2;
                                }
                                ReportMainTable.AddCell(PdfDataAlignment.HeaderLastCell(lPatientHistoryQuestionGroup.Name == "Medical History (Do you have now or before?)" ? "Do you have now or before?" : lPatientHistoryQuestionGroup.Name, 2));

                                ReportMainTable.AddCell(PdfDataAlignment.CreateCell1(historyDetails.ToString(), 4));
                            }
                        }
                    }
                    if (Historys.Count > 0 && string.IsNullOrEmpty(Patient.OtherNotes))
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 6, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    }
                    if (!string.IsNullOrEmpty(Patient.OtherNotes))
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 6, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                        ReportMainTable.AddCell(PdfDataAlignment.HeaderLastCell("Other Notes :", 6));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCell1(Patient.OtherNotes, 6));
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithTopBoarder(" ", 6, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    }
                }
                pdfDoc.Add(ReportMainTable);
            }
            else if (Page == "Note" && note.Note != "Patient got Discharged")
            {
                IList<ConsultedSymptom> lConsultedSymptom = ConsultationNoteManager.Instance.ListSymptomByNoteId(note.Id);
                IList<ConsultedProcedure> lConsultedProcedure = ConsultationNoteManager.Instance.LisConsultedProcedureByNoteId(note.Id);
                PdfPTable ReportMainTable = new PdfPTable(4);
                float[] widths = new float[] { 10f, 30f, 30f, 30f };
                ReportMainTable.SetWidths(widths);

                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder2("Consultation History", 4, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));

                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Date", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Consultation Note", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Diagnosis", 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Procedure", 0));

                ReportMainTable.AddCell(PdfDataAlignment.CreateCells(note.Date.ToString(Global.Company.DateFormat), 0));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells(note.Note, 0));

                ReportMainTable = LoadSymptomsDetails(ReportMainTable, lConsultedSymptom);
                ReportMainTable = LoadConsultedProceduresDetails(ReportMainTable, lConsultedProcedure);
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLabTest(" ", 4, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                pdfDoc.Add(ReportMainTable);
            }
            else if (Page == "LabTest History")
            {
                PdfPTable ReportMainTable = new PdfPTable(9);
                float[] widths = new float[] { 50f, 70f, 30f, 45f, 45f, 45f, 45f, 45f, 45f };
                ReportMainTable.SetWidths(widths);

                if (!IsCurrentIp)
                {
                    IList<ConsultedLabTest> lConsultedLabTest = ConsultationNoteManager.Instance.ListLabTestByNoteId(note.Id);
                    if (lConsultedLabTest != null && lConsultedLabTest.Count > 0)
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellPatientChart("LabTest History", 11));
                        foreach (ConsultedLabTest ConsLabTest in lConsultedLabTest)
                        {
                            IList<ConsultedLabTestElements> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestElementsByLabtestId(ConsLabTest.ConsultedLabTestId);
                            MedicalTest MedicalTest = MedicalTestManager.Instance.GetMedicalTestById(ConsLabTest.MedicalTestId);
                            if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
                            {
                                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLeftAlignPatientChart(("Test Name : " + "  " + MedicalTest.Name), 11));
                                ReportMainTable = LoadLabtestDetails(ReportMainTable, lConsultedLabTestElements, note, MedicalTest);
                                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLabTest(" ", 11, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                            }
                            else
                            {
                                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLeftAlignPatientChart(("Test Name : " + "  " + MedicalTest.Name), 11));
                            }
                        }
                    }
                }
                else
                {
                    IList<ConsultationNote> ConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientIdIpId(Patient.Id, PatientIpId);
                    if (ConsultationNote != null && ConsultationNote.Count > 0)
                    {
                        int i = 0;
                        foreach (ConsultationNote Note in ConsultationNote)
                        {
                            IList<ConsultedLabTest> lConsultedLabTest = ConsultationNoteManager.Instance.ListLabTestByNoteId(Note.Id);
                            if (lConsultedLabTest != null && lConsultedLabTest.Count > 0)
                            {
                                foreach (ConsultedLabTest ConsLabTest in lConsultedLabTest)
                                {
                                    IList<ConsultedLabTestElements> lConsultedLabTestElements = ConsultationNoteManager.Instance.ListConsultedLabTestElementsByLabtestId(ConsLabTest.ConsultedLabTestId);
                                    if (lConsultedLabTestElements != null && lConsultedLabTestElements.Count > 0)
                                    {
                                        MedicalTest MedicalTest = MedicalTestManager.Instance.GetMedicalTestById(ConsLabTest.MedicalTestId);
                                        ReportMainTable.AddCell(i == 0 ? PdfDataAlignment.CreateCellWithoutBoarder(" ", 11, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")) : PdfDataAlignment.CreateCellWithTopBoarder(" ", 11));
                                        ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLeftAlignPatientChart(("Test Name : " + "  " + MedicalTest.Name), 11));
                                        ReportMainTable = LoadLabtestDetails(ReportMainTable, lConsultedLabTestElements , note, MedicalTest);
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
                pdfDoc.Add(ReportMainTable);
            }
            else if (Page == "Prescription History")
            {
                PdfPTable ReportMainTable = new PdfPTable(10);
                float[] widths = new float[] { 55f ,100f, 40f, 25f, 40f, 40f, 40f, 40f, 40f, 60f };
                float desiredImageWidth = 10f;
                float desiredImageHeight = 10f;
                ReportMainTable.SetWidths(widths);
                if (!IsCurrentIp)
                {
                    //if (!prescriptionHistoryHeadingAdded)
                    //{
                        
                        //prescriptionHistoryHeadingAdded = true;
                    //}

                    if (!note.IsDischarged)
                    {
                        IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(note.Id);
                        if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                        {
                            ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellPatientChart("Prescription History", 10));
                            //ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                            ReportMainTable = LoadPrescriptionDetails(ReportMainTable, null, lConsultedPrescription, Ckimage, UnCkimage, false, desiredImageWidth, desiredImageHeight, note);
                            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLabTest(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                        }
                    }
                    pdfDoc.Add(ReportMainTable);
                }
                else
                {
                    ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellPatientChart("Prescription History", 10));
                    IList<ConsultationNote> ConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientIdIpId(Patient.Id, PatientIpId);
                    if (ConsultationNote != null && ConsultationNote.Count > 0)
                    {
                        int i = 0;
                        foreach (ConsultationNote Note in ConsultationNote)
                        {
                            if (!Note.IsDischarged)
                            {
                                IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(Note.Id);
                                if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                                {
                                    ReportMainTable.AddCell(i == 0 ? PdfDataAlignment.CreateCellWithoutBoarder(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")) : PdfDataAlignment.CreateCellWithTopBoarderForPatinetChart("", 9));
                                    ReportMainTable = LoadPrescriptionDetails(ReportMainTable, null, lConsultedPrescription, Ckimage, UnCkimage, false, desiredImageWidth, desiredImageHeight, Note);
                                }
                                pdfDoc.Add(ReportMainTable);
                            }
                            if (note != null && note.InPatientAdmissionId != null)
                            {
                                DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId((long)note.InPatientAdmissionId);
                                if (DischargeNote != null)
                                {
                                    ReportMainTable = new PdfPTable(10);
                                    widths = new float[] { 55f, 100f, 40f, 25f, 40f, 40f, 40f, 40f, 40f, 60f };
                                    ReportMainTable.SetWidths(widths);
                                    ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellPatientChart("Discharge Prescription Summary", 10));

                                    IList<DischargePrescription> DischargePrescription = DischargeNoteManager.Instance.ListPrescriptionByDischargeNoteId(DischargeNote.Id);
                                    if (DischargePrescription != null && DischargePrescription.Count > 0)
                                    {
                                        ReportMainTable.AddCell(i == 0 ? PdfDataAlignment.CreateCellWithoutBoarder(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")) : PdfDataAlignment.CreateCellWithTopBoarderForPatinetChart("", 9));
                                        ReportMainTable = LoadPrescriptionDetails(ReportMainTable, DischargePrescription, null, Ckimage, UnCkimage, true, desiredImageWidth, desiredImageHeight, Note);
                                    }
                                    pdfDoc.Add(ReportMainTable);
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            else if (Page == "Discharge Prescription History")
            {
                PdfPTable ReportMainTable = new PdfPTable(10);
                float[] widths = new float[] { 55f, 100f, 40f, 25f, 40f, 40f, 40f, 40f, 40f, 60f };
                float desiredImageWidth = 10f;
                float desiredImageHeight = 10f;
                ReportMainTable.SetWidths(widths);
                if (!IsCurrentIp)
                {
                    //if (!dischargePrescriptionSummaryHeadingAdded)
                    //{
                        
                    //    dischargePrescriptionSummaryHeadingAdded = true;
                    //}
                    if (note.InPatientAdmissionId != null)
                    {
                        DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId((long)note.InPatientAdmissionId);
                        if (DischargeNote != null)
                        {
                            IList<DischargePrescription> DischargePrescription = DischargeNoteManager.Instance.ListPrescriptionByDischargeNoteId(DischargeNote.Id);
                            if (DischargePrescription != null && DischargePrescription.Count > 0)
                            {
                                ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellPatientChart("Discharge Prescription Summary", 10));
                                ReportMainTable = LoadPrescriptionDetails(ReportMainTable, DischargePrescription, null, Ckimage, UnCkimage, true, desiredImageWidth, desiredImageHeight, note);
                                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarderLabTest(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                            }
                            pdfDoc.Add(ReportMainTable);
                        }
                    }
                }
                else
                {
                    ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellPatientChart("Prescription History", 10));
                    IList<ConsultationNote> ConsultationNote = ConsultationNoteManager.Instance.ListNotesEntryByPatientIdIpId(Patient.Id, PatientIpId);
                    if (ConsultationNote != null && ConsultationNote.Count > 0)
                    {
                        int i = 0;
                        foreach (ConsultationNote Note in ConsultationNote)
                        {
                            if (!Note.IsDischarged)
                            {
                                IList<ConsultedPrescription> lConsultedPrescription = ConsultationNoteManager.Instance.ListPrescriptionByNoteId(Note.Id);
                                if (lConsultedPrescription != null && lConsultedPrescription.Count > 0)
                                {
                                    ReportMainTable.AddCell(i == 0 ? PdfDataAlignment.CreateCellWithoutBoarder(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")) : PdfDataAlignment.CreateCellWithTopBoarderForPatinetChart("", 9));
                                    ReportMainTable = LoadPrescriptionDetails(ReportMainTable, null, lConsultedPrescription, Ckimage, UnCkimage, false, desiredImageWidth, desiredImageHeight, Note);
                                }
                                pdfDoc.Add(ReportMainTable);
                            }
                            if (note != null && note.InPatientAdmissionId != null)
                            {
                                DischargeNote DischargeNote = DischargeNoteManager.Instance.GetDischargeNoteByIpId((long)note.InPatientAdmissionId);
                                if (DischargeNote != null)
                                {
                                    ReportMainTable = new PdfPTable(10);
                                    widths = new float[] { 55f, 100f, 40f, 25f, 40f, 40f, 40f, 40f, 40f, 60f };
                                    ReportMainTable.SetWidths(widths);
                                    ReportMainTable.AddCell(PdfDataAlignment.CreateHeadingCellPatientChart("Discharge Prescription Summary", 10));

                                    IList<DischargePrescription> DischargePrescription = DischargeNoteManager.Instance.ListPrescriptionByDischargeNoteId(DischargeNote.Id);
                                    if (DischargePrescription != null && DischargePrescription.Count > 0)
                                    {
                                        ReportMainTable.AddCell(i == 0 ? PdfDataAlignment.CreateCellWithoutBoarder(" ", 10, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")) : PdfDataAlignment.CreateCellWithTopBoarderForPatinetChart("", 9));
                                        ReportMainTable = LoadPrescriptionDetails(ReportMainTable, DischargePrescription, null, Ckimage, UnCkimage, true, desiredImageWidth, desiredImageHeight, Note);
                                    }
                                    pdfDoc.Add(ReportMainTable);
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            return pdfDoc;
        }
        private void LoadSubHeadings(Document pdfDoc, ConsultationNote note)
        {
            if (note.OpRegistrationId != null && note.InPatientAdmissionId == null)
            {
                PdfPTable ReportMainTable = new PdfPTable(4);
                float[] widths = new float[] { 10f, 30f, 30f, 30f };
                ReportMainTable.SetWidths(widths);
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder2("Out Patient   " + "    Registration Date : " + OpManager.Instance.GetRegisterByRegId((long)note.OpRegistrationId).DateOfRegistration.Date.ToString(Global.Company.DateFormat), 4, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                pdfDoc.Add(ReportMainTable);
            }
            else if (note.OpRegistrationId != null && note.InPatientAdmissionId != null)
            {
                if (IpId == null || IpId != note.InPatientAdmissionId)
                {
                    PdfPTable ReportMainTable = new PdfPTable(4);
                    float[] widths = new float[] { 10f, 30f, 30f, 30f };
                    ReportMainTable.SetWidths(widths);
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder2("In Patient   " + "    Admission Date : " + IpManager.Instance.GetInPatientAdmissionById((long)note.InPatientAdmissionId).DateOfAdmission.Date.ToString(Global.Company.DateFormat), 4, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                    pdfDoc.Add(ReportMainTable);
                    IpId = note.InPatientAdmissionId;
                }
            }
            else if (note.OpRegistrationId == null && note.InPatientAdmissionId != null)
            {
                PdfPTable ReportMainTable = new PdfPTable(4);
                float[] widths = new float[] { 10f, 30f, 30f, 30f };
                ReportMainTable.SetWidths(widths);
                ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithoutBoarder2("Patient Discharged     " + "    Discharge Date : " + DischargeNoteManager.Instance.GetDischargeStatusByAdmissionId((long)note.InPatientAdmissionId).DischargeOn, 4, PdfDataAlignment.GetFont("Font_Bold_Italic_9_Black")));
                pdfDoc.Add(ReportMainTable);
            }
        }
        private PdfPTable LoadNoteDetails(PdfPTable ReportMainTable, string notes, iTextSharp.text.Image Ckimage, iTextSharp.text.Image UnCkimage, bool IsDisPres)
        {
            ReportMainTable.AddCell(PdfDataAlignment.CreateCells((notes), 1));
            return ReportMainTable;
        }
        private PdfPTable LoadSymptomsDetails(PdfPTable ReportMainTable, IList<ConsultedSymptom> lConsultedSymptom)
        {
            string Symptoms = string.Empty;
            foreach (ConsultedSymptom Symptom in lConsultedSymptom)
            {
                Symptoms = Symptoms + (string.IsNullOrEmpty(Symptoms) ? Symptom.Symptom.Name : ", " + Symptom.Symptom.Name);
            }
            ReportMainTable.AddCell(PdfDataAlignment.CreateCells((Symptoms), 1));
            return ReportMainTable;
        }
        private PdfPTable LoadConsultedProceduresDetails(PdfPTable ReportMainTable, IList<ConsultedProcedure> lConsultedProcedure)
        {
            string Procedures = string.Empty;
            foreach (ConsultedProcedure Procedure in lConsultedProcedure)
            {
                Procedures = Procedures + (string.IsNullOrEmpty(Procedures) ? Procedure.Name : ", " + Procedure.Name);
            }
            ReportMainTable.AddCell(PdfDataAlignment.CreateCells((Procedures), 1));
            return ReportMainTable;
        }
       private PdfPTable LoadPrescriptionDetails(PdfPTable ReportMainTable, IList<DischargePrescription> lDischargePrescription, IList<ConsultedPrescription> lConsultedPrescription, iTextSharp.text.Image Ckimage, iTextSharp.text.Image UnCkimage, bool IsDisPres, float desiredImageWidth, float desiredImageHeight, ConsultationNote note)
       {
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Date", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Prescription", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Total", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Days"), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Hours", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Morning", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("AfterNoon"), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Evening"), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Night"), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Notes"), 1));
            if (IsDisPres)
            {
                String dateTime = null;
                foreach (DischargePrescription ConsPres in lDischargePrescription)
                {
                    Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById(ConsPres.PrescriptionId);
                    String stringLineItemDate = DateUtils.FormatDate(note.Date, Global.Company.DateFormat);
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells((note.Date.ToShortDateString()), 1));
                        dateTime = stringLineItemDate;
                    }
                    else
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells("", 1));
                    }
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Product == null ? PrescriptionFromDB.CustomProduct : PrescriptionFromDB.Product.Name), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Total), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Days.ToString()), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Hours), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Morning), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Afternoon), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Evening), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Night), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.TakeDosage == TakeDosage.AFTER ? "After Food" : "Before Food"), 1));
                }
            }
            else
            {
                String dateTime = null;
                foreach (ConsultedPrescription ConsPres in lConsultedPrescription)
                {
                    Prescription PrescriptionFromDB = ConsultationNoteManager.Instance.GetPrescriptionById((long)ConsPres.PrescriptionId);
                    String stringLineItemDate = DateUtils.FormatDate(note.Date, Global.Company.DateFormat);
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells((note.Date.ToShortDateString()), 1));
                        dateTime = stringLineItemDate;
                    }
                    else
                    {
                        ReportMainTable.AddCell(PdfDataAlignment.CreateCells("", 1));
                    }
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Product == null ? PrescriptionFromDB.CustomProduct : PrescriptionFromDB.Product.Name), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Total), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Days.ToString()), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Hours), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Morning), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Afternoon), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Evening), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.Night), 1));
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((PrescriptionFromDB.TakeDosage == TakeDosage.AFTER ? "After Food" : "Before Food"), 1));
                }
            }
            return ReportMainTable;
       }
        private PdfPTable LoadLabtestDetails(PdfPTable ReportMainTable, IList<ConsultedLabTestElements> lConsultedLabTestElements , ConsultationNote note, MedicalTest MedicalTest)
        {
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Date", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Element Name", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Uom", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Class"), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("SubClass", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours("Result", 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Single Value"), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Range From"), 1));
            ReportMainTable.AddCell(PdfDataAlignment.CreateCellWithBackColours(("Range To"), 1));

            String dateTime = null!;
            foreach (ConsultedLabTestElements ConsLabTestEle in lConsultedLabTestElements)
            {
                String stringLineItemDate = DateUtils.FormatDate(note.Date, Global.Company.DateFormat);
                if (dateTime == null || dateTime != stringLineItemDate)
                {
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells((note.Date.ToShortDateString()), 1));
                    dateTime = stringLineItemDate;
                }
                else
                {
                    ReportMainTable.AddCell(PdfDataAlignment.CreateCells("", 1));
                }
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.Name), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.Uom != null ? ConsLabTestEle.Uom.Name : ""), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.Class != null ? ConsLabTestEle.Class.ToString() : ""), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.SubClass != null ? ConsLabTestEle.SubClass.ToString() : ""), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.ResultDescription != null ? ConsLabTestEle.ResultDescription.ToString() : ""), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.SingleValue != null ? ConsLabTestEle.SingleValue.ToString() : ""), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.RangeFrom != null ? ConsLabTestEle.RangeFrom.ToString() : ""), 1));
                ReportMainTable.AddCell(PdfDataAlignment.CreateCells((ConsLabTestEle.RangeTo != null ? ConsLabTestEle.RangeTo.ToString() : ""), 1));
            }
            return ReportMainTable;
        }
    }
}
