using fa;
using fa.api.Hms;
using fa.api.utils;
using fa.model.Accounting.Transactions;
using fa.model.Common;
using fa.model.hms.common;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.report.Ip;
using fa.reports.Hms;
using fa.views.utils;
using fa.views.utils.Common;
using Fa.reports.Hms;
using Fa.reports.Inventory;
using FADataAccessLibrary.report.Hms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.POIFS.Properties;
using NPOI.SS.UserModel;
using System;
using System.Data;
using System.IO;
using System.Reflection.Metadata;
using System.Windows.Forms;
using VisioForge.MediaFramework.ONVIF;
using Document = iTextSharp.text.Document;

namespace Fa.views.utils.Report.Hms
{
    class PatientTransferSavePrint
    {
        public bool ExportOrPrintToFile(RptPatientTransfer RptPatientTransfer, string ReportName, string fileExtension, bool isPrint)
        {
            if (RptPatientTransfer != null)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(RptPatientTransfer);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, RptPatientTransfer, ReportName, fileExtension, isPrint, RptPatientTransfer.FromDate.ToString(Global.Company.DateFormat), RptPatientTransfer.ToDate.ToString(Global.Company.DateFormat));
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
            }
            return true;
        }
        readonly String[] PatientTransferDataTableColumnByDate = new String[]
        { 
            "#","In Date","Ward","Patient Id","Patient Name","Bed","Out Date","Consultant","Description",
        };
        readonly String[] PatientTransferDataTableColumnByWard = new String[]
        {
            "#","Ward","Patient Id","Patient Name","Bed","In Date","Out Date","Consultant","Description",
        };
        readonly String[] PatientTransferDataTableColumnByConslutant = new String[]
        {
            "#","Consultant","Ward","Patient Id","Patient Name","Bed","In Date","Out Date","Description",
        };
        public DataTable DataGridViewAsDataTable(RptPatientTransfer RptPatientTransfer)
        {
            if(RptPatientTransfer.Type == TransferType.BYDATE)
            {
                DataTable PatientTransferTable = new DataTable();
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.SNO], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.IN_DATE], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.WARD], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.PATIENTId], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.PATIENT], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.BED], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.OUT_DATE], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.CONSULTANT], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.DESCRIPTION], typeof(string));
                
                DataRow PatientTransferTableRow = null;
                String dateTime = null;
                string currentWard = string.Empty;
                string consulted = string.Empty;
                int rn = 0;
                foreach (PatientTransferLineItem LineItem in RptPatientTransfer.PatientTransferLineItems.OrderBy(x => x.AdmittedOn).ThenBy(x => x.WardName).ThenBy(x => x.AuthorizedDoctor))
                {
                    PatientTransferTableRow = PatientTransferTable.NewRow();
                    String stringLineItemDate = DateUtils.FormatDate(LineItem.AdmittedOn, Global.Company.DateFormat);
                    PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.SNO]] = rn + 1;
                    if (dateTime == null || dateTime != stringLineItemDate)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.IN_DATE]] = LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                        dateTime = stringLineItemDate;
                        currentWard = string.Empty;
                        consulted = string.Empty;
                    }
                    if (currentWard != LineItem.WardName)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.WARD]] = LineItem.WardName;
                        currentWard = LineItem.WardName;
                    }
                    PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.PATIENTId]] = LineItem.PatientNumber;
                    PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.PATIENT]] = LineItem.PatientName;
                    PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.BED]] = LineItem.BedName;
                    if (!LineItem.AreActive)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.OUT_DATE]] = LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                    }
                    if (consulted == string.Empty || consulted != LineItem.AuthorizedDoctor)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.CONSULTANT]] = LineItem.AuthorizedDoctor;
                        consulted = LineItem.AuthorizedDoctor;
                    }
                    IList<DischargeNote> dischargeNotes = new List<DischargeNote>();
                    DischargeNote dischargeNote = DischargeNoteManager.Instance.GetLatestDischargeNoteByPatientId(LineItem.patientId);
                    if (dischargeNote != null && dischargeNote.DischargeOn != null && !LineItem.AreActive)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.DESCRIPTION]]= dischargeNote.DischargeSummary;
                    }
                    else
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByDate[(int)PatientTransferReportByDateTableColumn.DESCRIPTION]] = LineItem.Notes;
                    }
                    PatientTransferTable.Rows.Add(PatientTransferTableRow);
                    rn++;
                }
                return PatientTransferTable;
            }
            else if(RptPatientTransfer.Type == TransferType.BYWARD)
            {
                DataTable PatientTransferTable = new DataTable();
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.SNO], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.WARD], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.PATIENTId], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.PATIENT], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.BED], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.IN_DATE], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.OUT_DATE], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.CONSULTANT], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.DESCRIPTION], typeof(string));

                DataRow PatientTransferTableRow = null;
                string wards = string.Empty;
                string consulted = string.Empty;
                int rn = 0;
                foreach (PatientTransferLineItem LineItem in RptPatientTransfer.PatientTransferLineItems.OrderBy(x => x.WardName).ThenBy(x => x.AdmittedOn).ThenBy(x => x.AuthorizedDoctor))
                {
                    PatientTransferTableRow = PatientTransferTable.NewRow();
                    PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.SNO]] = rn + 1;
                    if (wards == string.Empty || wards != LineItem.WardName)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.WARD]] = LineItem.WardName;
                        wards = LineItem.WardName;
                        consulted = string.Empty;
                    }
                    PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.PATIENTId]] = LineItem.PatientNumber;
                    PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.PATIENT]] = LineItem.PatientName;
                    PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.BED]] = LineItem.BedName;
                    PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.IN_DATE]] = LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                    if (!LineItem.AreActive)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.OUT_DATE]] = LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                    }
                    if (consulted == string.Empty || consulted != LineItem.AuthorizedDoctor)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.CONSULTANT]] = LineItem.AuthorizedDoctor;
                        consulted = LineItem.AuthorizedDoctor;
                    }
                    IList<DischargeNote> dischargeNotes = new List<DischargeNote>();
                    DischargeNote dischargeNote = DischargeNoteManager.Instance.GetLatestDischargeNoteByPatientId(LineItem.patientId);

                    if (dischargeNote != null && dischargeNote.DischargeOn != null && !LineItem.AreActive)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.DESCRIPTION]] = dischargeNote.DischargeSummary;
                    }
                    else
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByWard[(int)PatientTransferReportByWardTableColumn.DESCRIPTION]] = LineItem.Notes;
                    }
                    PatientTransferTable.Rows.Add(PatientTransferTableRow);
                    rn++;
                }
                return PatientTransferTable;
            }
            else
            {
                DataTable PatientTransferTable = new DataTable();
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.SNO], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.CONSULTANT], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.WARD], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.PATIENTId], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.PATIENT], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.BED], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.IN_DATE], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.OUT_DATE], typeof(string));
                PatientTransferTable.Columns.Add(PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.DESCRIPTION], typeof(string));

                DataRow PatientTransferTableRow = null;
                string conslutant = string.Empty;
                string currentWard = string.Empty;
                int rn = 0;
                foreach (PatientTransferLineItem LineItem in RptPatientTransfer.PatientTransferLineItems.OrderBy(x => x.AuthorizedDoctor).ThenBy(x => x.WardName).ThenBy(x => x.AdmittedOn))
                {
                    PatientTransferTableRow = PatientTransferTable.NewRow();
                    PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.SNO]] = rn + 1;
                    if (conslutant == string.Empty || conslutant != LineItem.AuthorizedDoctor)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.CONSULTANT]] = LineItem.AuthorizedDoctor;
                        conslutant = LineItem.AuthorizedDoctor;
                    }
                    if (currentWard != LineItem.WardName)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.WARD]] = LineItem.WardName;
                        currentWard = LineItem.WardName;
                    }
                    
                    PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.PATIENTId]] = LineItem.PatientNumber;
                    PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.PATIENT]] = LineItem.PatientName;
                    PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.BED]] = LineItem.BedName;
                    PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.IN_DATE]] = LineItem.AdmittedOn.ToString(Global.Company.DateFormat);
                    if (!LineItem.AreActive)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.OUT_DATE]] = LineItem.DischargedOn.ToString(Global.Company.DateFormat);
                    }
                    IList<DischargeNote> dischargeNotes = new List<DischargeNote>();
                    DischargeNote dischargeNote = DischargeNoteManager.Instance.GetLatestDischargeNoteByPatientId(LineItem.patientId);

                    if (dischargeNote != null && dischargeNote.DischargeOn != null && !LineItem.AreActive)
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.DESCRIPTION]] = dischargeNote.DischargeSummary;
                    }
                    else
                    {
                        PatientTransferTableRow[PatientTransferDataTableColumnByConslutant[(int)PatientTransferReportByConsultantTableColumn.DESCRIPTION]] = LineItem.Notes;
                    }
                    PatientTransferTable.Rows.Add(PatientTransferTableRow);
                    rn++;
                }
                return PatientTransferTable;
            }
        }
        public void GeneratePDF(DataTable DataTable, RptPatientTransfer RptPatientTransfer, string ReportName, string fileExtension, bool isPrint, string FromDate, string Todate)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                double A4Height = 760;
                Document pdfDoc = new Document(PageSize.A4, -45, -45, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();

                PdfPageHeader PdfHeader = new PdfPageHeader()
                {
                    IsMainHeader = true,
                    Islogo = true,
                    IsAddress = true,
                    IsPhone = true,
                    IsEmail = true,
                    IsWebsite = true,
                    IsLicenceInfo = true,
                    ReportLine1 = RptPatientTransfer.ReportTitle(),
                    ReportLine2 = RptPatientTransfer.ReportSubTitle()
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
                    ReportLine1 = RptPatientTransfer.ReportTitle(),
                    ReportLine2 = RptPatientTransfer.ReportSubTitle()
                };
                PdfPTable MiniHTable = PdfHeader.PageHeader();
                PdfTableHeader PdfTableHeader = new PdfTableHeader();
                PdfPTable MTable = PdfTableHeader.ReportTableHeader(DataTable, RptPatientTransfer.ReportTitle());
                pdfDoc.Add(HTable);
                pdfDoc.Add(MTable);

                int Cols = DataTable.Columns.Count;
                int Rows = DataTable.Rows.Count;
                PdfPTable ReportMainTable = new PdfPTable(Cols);
                float[] widths = new float[] {};
                if (RptPatientTransfer.Type == TransferType.BYDATE)
                {
                    widths = new float[] { 10f, 20f, 30f, 30f, 30f, 20f, 20f, 30f, 30f };
                }
                else if (RptPatientTransfer.Type == TransferType.BYWARD)
                {
                    widths = new float[] { 10f, 30f, 30f, 30f, 20f, 20f, 20f, 30f, 30f };
                }
                else
                {
                    widths = new float[] { 10f, 30f, 30f, 30f, 40f, 20f, 20f, 20f, 30f };
                }
                ReportMainTable.SetWidths(widths);

                int k = 2;
                BaseColor[] RowColor = new BaseColor[2];
                RowColor[0] = new BaseColor(255, 255, 255);
                RowColor[1] = new BaseColor(250, 250, 250);
                for (int i = 0; i < Rows; i++)
                {
                    PdfPCell RowCell = new PdfPCell();
                    double TotalWorkingOnPageH = (HTable.TotalHeight + MTable.TotalHeight + PdfDataAlignment.CalculatePdfTableHeight(ReportMainTable));
                    if (TotalWorkingOnPageH > A4Height)
                    {
                        pdfDoc.Add(ReportMainTable);
                        pdfDoc.NewPage();
                        pdfDoc.Add(MiniHTable);
                        pdfDoc.Add(MTable);
                        ReportMainTable = new PdfPTable(Cols);
                        ReportMainTable.SetWidths(widths);
                        k = 2;
                    }
                    BaseColor CurRowColor = RowColor[k % 2];

                    for (int j = 0; j < Cols; j++)
                    {
                        var Temp = DataTable.Rows[i][j].ToString();
                        RowCell = new PdfPCell(new Phrase(Temp, PdfDataAlignment.GetFont("Font_Normal_Italic_7_Black")));
                        RowCell.BorderColor = new BaseColor(160, 160, 160);
                        RowCell.BackgroundColor = CurRowColor;
                        ReportMainTable.AddCell(RowCell);
                    }
                    k++;
                }
                pdfDoc.Add(ReportMainTable);
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
                PdfGeneration.IsPrint = isPrint;
                PdfGeneration.FileName = RptPatientTransfer.ReportName();
                PdfGeneration.PdfFile = PdfFileWithFooter;
                PdfGeneration.SavePdfFile();
            }
        }
        private PdfPTable MainTable(DataTable DataTable)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            iTextSharp.text.Font Font_Bold_Italic_10_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_9_White = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Normal_Italic_8_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            int Cols = DataTable.Columns.Count;
            int Rows = DataTable.Rows.Count;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] Defaultwidths = new float[] { 20f, 15f, 30f, 45f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
            float[] widths = new float[Cols];
            for (int i = 0; i < Cols; i++)
            {
                widths[i] = Defaultwidths[i];
            }
            ReportMainTable.SetWidths(widths);
            //Main table Head
            PdfPCell HeaderCell = new PdfPCell();
            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, Font_Bold_Italic_9_White));
                HeaderCell.BackgroundColor = new BaseColor(160, 160, 160);
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 25;
                HeaderCell.Padding = 4;
                ReportMainTable.AddCell(HeaderCell);
            }

            //Main Table body
            for (int i = 0; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, Font_Normal_Italic_8_Black));
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 20;
                    RowCell.Padding = 2;
                    ReportMainTable.AddCell(RowCell);
                }
            }

            for (int j = 0; j < Cols; j++)
            {
                PdfPCell rowCell = new PdfPCell();
                rowCell = new PdfPCell(new Phrase("", Font_Normal_Italic_8_Black));
                rowCell.BorderColor = BaseColor.BLACK;

                rowCell.MinimumHeight = 1f;
                ReportMainTable.AddCell(rowCell);
            }
            return ReportMainTable;
        }
    }
}
