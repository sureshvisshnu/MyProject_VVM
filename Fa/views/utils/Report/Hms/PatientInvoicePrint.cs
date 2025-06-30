using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fa.api.Accounting;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.api.utils;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using fa.model.hms.config;
using fa.model.Hms.common;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.OrderManagement;
using fa.views.utils.Common;
using fa.views.utils.Report.Account;
using Fa.api.Hms;
using Fa.report.accounting.master;
using Fa.report.Hms;
using Fa.Utils.utils;
using Fa.views.utils.Report.Hms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NPOI.SS.Formula.Functions;
using Pango;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static fa.views.utils.Common.BlankTablesWithBorder;
using static fa.views.utils.Common.TableCellAlignment;
using static fa.views.utils.PrinterSetup;
using static fa.views.utils.Sale.SalePrintSaveA5A4;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace fa.views.utils.Report.Hms
{
    public class PatientInvoicePrint
    {
        static readonly String[] InvoiceColumnPrint = new String[]
        {
            "#", "Date", "Type", "Description", "Qty","Rate","Amount"
        };
        static readonly String[] InvoicePaymentColumnPrint = new String[]
        {
             "Date", "Reference No", "Payment Mode","Document No","Account","Amount"
        };
        static double Total = 0;
        public string fileName = string.Empty;
        public bool ExportOrPrintToFile(long PatientInvoiceId, string ReportName, string fileExtension)
        {
            try
            {
                GenerateInvoiceNewPrint(PatientInvoiceId,ReportName,true);
            }
            catch (Exception e)
            {
                MessageBox.Show("File Error Please Contact System Admin");
                Console.WriteLine(e.ToString());
            }
            return true;
        }
       
        public void GenerateInvoiceNewPrint(long PatientInvoiceId, string ReportName,bool isprint)
        {
            string ItemsHeader = string.Empty;
            double PaymentTotal = 0.00;
            double InvoiceTotal = 0.00;
            double GroupViceTotal = 0.00;
            HospitalConfiguration HospitalConfiguration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Global.Company.CompanyId);
            if (HospitalConfiguration != null)
            {
                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    PatientInvoice Invoice = PatientInvoiceManager.Instance.GetInvoiceById(PatientInvoiceId);
                    if (Invoice != null)
                    {
                        //double A4Height = 760;
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
                        int i = 1;
                        int k = 1;
                        int j = 1;
                        int MaxLine = 40;
                        int Cols = 6;
                        PdfPTable PaymentReportMainTable = new PdfPTable(Cols);
                        float[] pwidths = new float[] { 20f, 20f, 30f, 20f, 40f, 23f };
                        PaymentReportMainTable.SetWidths(pwidths);

                        PdfPTable ReportMainTable = new PdfPTable(7);
                        float[] widths = new float[] { 5f, 15f, 10f, 50f, 20f, 20f, 23f };
                        ReportMainTable.SetWidths(widths);

                        PdfPTable Empty = new PdfPTable(Cols);
                        float[] Ewidths = new float[] { 5f, 20f, 50f, 20f, 20f, 23f };
                        Empty.SetWidths(Ewidths);
                        Empty.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                        
                        PdfPTable MiniHTable = PdfHeader.PageHeader();
                        //patient details
                        PdfPTable PatientDetailsTable = PatientDetailsHeading(Invoice);
                        //details table heading
                        PdfPTable MTable = ReportTableHeader();
                        PdfPTable SubTable = ReportTableSubHeader();
                        //payment details table heading
                        PdfPTable PTable = ReportTablePaymentHeader();
                        pdfDoc.Add(HTable);
                        pdfDoc.Add(PatientDetailsTable);
                        pdfDoc.Add(Empty);
                        pdfDoc.Add(MTable);
                        pdfDoc.Add(SubTable);
                        int RoundingPrecision = Global.Company.PrimaryCurrency.RoundingPrecision;
                        string formatSpecifier = $"F{RoundingPrecision}";

                        IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByInvoiceId(Invoice.InvoiceId);
                        if (HospitalConfiguration.HasInvoicingGroup)
                        {
                            IList<PatientLedgerTransactionTypeGroup> lPatientLedgerTransactionTypeGroup = HospitalInvoiceGroupManager.Instance.ListAllPatientLedgerTransactionTypeGroup(Global.Company.CompanyId);
                            if (lPatientLedgerTransactionTypeGroup != null)
                            {
                                int Invg = 1;
                                foreach (PatientLedgerTransactionTypeGroup group in lPatientLedgerTransactionTypeGroup.OrderBy(x => x.Id))
                                {
                                    if (group.PatientLedgerTransactionTypeGroupMappings.FirstOrDefault(x => x.TransactionType == TransactionType.PHARMACY_FEE) != null)
                                    {
                                        ItemsHeader = group.Name;
                                    }
                                    if (lPatientLedger != null && lPatientLedger.Count > 0)
                                    {
                                        //fee details
                                        int FeeCount = 0;
                                        foreach (PatientLedger ledger in lPatientLedger)
                                        {
                                            if (group.PatientLedgerTransactionTypeGroupMappings.Where(t => t.TransactionType == ledger.Type).ToList().Count > 0)
                                            {
                                                if (k == MaxLine)
                                                {
                                                    pdfDoc.Add(ReportMainTable);
                                                    pdfDoc.NewPage();
                                                    pdfDoc.Add(MTable);
                                                    ReportMainTable = new PdfPTable(7);
                                                    ReportMainTable.SetWidths(widths);
                                                    k = 1;
                                                }
                                                if(Invg==1)
                                                {
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,group.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"),false, BaseColor.WHITE, false,7,1));
                                                    Invg = 0;
                                                    k++;
                                                }
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int) BrushBorder.N, i.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int) BrushBorder.N, ledger.Date.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int) BrushBorder.N, ledger.InPatientAdmissionId != null ? "IP" : "OP", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int) BrushBorder.N, ledger.Description, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int) BrushBorder.N, "1", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int) BrushBorder.N, ledger.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int) BrushBorder.N, ledger.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                GroupViceTotal += ledger.Amount;
                                                InvoiceTotal+= ledger.Amount;
                                                i++;
                                                k++;
                                                FeeCount++;
                                            }
                                        }
                                        if (FeeCount > 0)
                                        {
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Sub Total", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, GroupViceTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            GroupViceTotal = 0.00;
                                            k++;
                                        }
                                    }
                                    Invg = 1;
                                }
                            }
                        }
                        else
                        {
                            if (lPatientLedger != null && lPatientLedger.Count > 0)
                            {
                                //fee details
                                int FeeCount = 0;
                                int dayCount = 0;
                                double Amount = 0;
                                string bed = "yyy";
                                int rowCount = 0;
                                foreach (PatientLedger ledger in lPatientLedger)
                                {
                                    int LineCount = lPatientLedger.Where(x => x.RentTimeDuration == "1 day" && x.Bed == ledger.Bed).Count();
                                    if (ledger.Amount != 0)
                                    {
                                        if (k == MaxLine)
                                        {
                                            pdfDoc.Add(ReportMainTable);
                                            pdfDoc.NewPage();
                                            pdfDoc.Add(MTable);
                                            ReportMainTable = new PdfPTable(7);
                                            ReportMainTable.SetWidths(widths);
                                            k = 1;
                                        }
                                        if (ledger.RentTimeDuration == "1 day")
                                        {
                                            rowCount++;
                                            if (bed == "yyy" || ledger.Bed == bed)
                                            {
                                                dayCount++;
                                                Amount += ledger.Amount;
                                                bed = ledger.Bed;
                                                GroupViceTotal += ledger.Amount;
                                                InvoiceTotal += ledger.Amount;
                                                if (rowCount == LineCount)
                                                {
                                                    string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days) - @" + ledger.Amount + " per day";
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, i.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.Date.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.InPatientAdmissionId != null ? "IP" : "OP", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Description, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, dayCount.ToString(), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                    Amount = 0;
                                                    rowCount = 0;
                                                }
                                            }
                                            else
                                            {
                                                string Description = "Room rent for staying " + ledger.Bed + " (" + dayCount + " Days)";
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, i.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.Date.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.InPatientAdmissionId != null ? "IP" : "OP", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Description, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "1", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                GroupViceTotal += ledger.Amount;
                                                InvoiceTotal += ledger.Amount;
                                                bed = ledger.Bed;
                                                Amount = 0;
                                                rowCount = 0;
                                            }
                                        }
                                        else
                                        {
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, i.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.Date.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.InPatientAdmissionId != null ? "IP" : "OP", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.Description, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "1", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                            GroupViceTotal += ledger.Amount;
                                            InvoiceTotal += ledger.Amount;
                                            i++;
                                            k++;
                                            FeeCount++;
                                        }
                                    }
                                }
                                if (FeeCount > 0)
                                {
                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Sub Total", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                                    ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, GroupViceTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                                    GroupViceTotal = 0.00;
                                    k++;
                                }
                            }
                        }
                    //pharma details
                    long? OPID = lPatientLedger!.First().OpRegistrationId;
                    if (OPID != null)
                    {
                        List<SaleEntry> lSaleEntry = SalesManager.Instance.GetSaleEntryByPatientIdOpId(Invoice.PatientId, OPID);
                        if (lSaleEntry != null && lSaleEntry.Count > 0)
                        {
                            bool IsHeader = true;
                            if (lSaleEntry.Where(x => x.EntryType == Entrytype.SALE).ToList().Count > 0)
                            {
                                foreach (SaleEntry Entry in lSaleEntry.Where(x => x.EntryType == Entrytype.SALE))
                                {
                                    if(Entry.SaleMethod==SaleMethod.Cash)
                                    {
                                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Entry.SaleDate.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Entry.RefNumber, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), "CASH", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Entry.NetAmount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        PaymentTotal += Entry.NetAmount;
                                        j++;
                                    }
                                    else
                                    {
                                            ReceiptDetail lReceiptDetail = ReceiptManager.Instance.GetReceiptDetailByItemBased(Entry.Id.ToString());
                                            if (lReceiptDetail != null)
                                            {
                                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), lReceiptDetail.Receipt.TransactionDate.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), lReceiptDetail.Receipt.Reference, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), lReceiptDetail.Receipt.TransactionType.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Entry.NetAmount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                                PaymentTotal += Entry.NetAmount;
                                                j++;
                                            }
                                    }
                                    foreach (SaleDetail Detail in Entry.SaleDetails)
                                    {
                                        if (k == MaxLine)
                                        {
                                            pdfDoc.Add(ReportMainTable);
                                            pdfDoc.NewPage();
                                            pdfDoc.Add(MTable);
                                            ReportMainTable = new PdfPTable(Cols);
                                            ReportMainTable.SetWidths(widths);
                                            k = 1;
                                        }
                                        if (IsHeader)
                                        {
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, string.IsNullOrEmpty(ItemsHeader) ? "Inventory Items" : ItemsHeader, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                                            IsHeader = false;
                                            k++;
                                        }
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, i.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Entry.SaleDate.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Product.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Quantity.ToString(), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        GroupViceTotal += Detail.Amount;
                                        InvoiceTotal += Detail.Amount;
                                        i++;
                                        k++;
                                    }
                                }
                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Sub Total", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 5, 1));
                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, GroupViceTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                                GroupViceTotal = 0.00;
                            }
                            IsHeader = true;
                            if (lSaleEntry.Where(x => x.EntryType == Entrytype.RETURN).ToList().Count > 0)
                            {
                                foreach (SaleEntry Entry in lSaleEntry.Where(x => x.EntryType == Entrytype.RETURN))
                                {
                                    foreach (SaleDetail Detail in Entry.SaleDetails)
                                    {
                                        if (k == MaxLine)
                                        {
                                            pdfDoc.Add(ReportMainTable);
                                            pdfDoc.NewPage();
                                            pdfDoc.Add(MTable);
                                            ReportMainTable = new PdfPTable(Cols);
                                            ReportMainTable.SetWidths(widths);
                                            k = 1;
                                        }
                                        if (IsHeader)
                                        {
                                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Inventory Return", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                                            IsHeader = false;
                                            k++;
                                        }
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, i.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Entry.SaleDate.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Product.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Quantity.ToString(), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Detail.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                        GroupViceTotal += Detail.Amount;
                                        InvoiceTotal -= Detail.Amount;
                                        i++;
                                        k++;
                                    }
                                }
                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Sub Total", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 5, 1));
                                ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "-"+GroupViceTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                                GroupViceTotal = 0.00;
                            }
                        }
                    }
                    //total
                    
                    //payment details
                    i = 1;
                    IList<PatientInvoicePayment> lPatientInvoicePayment = PatientInvoiceManager.Instance.ListEntryByInvoiceId(Invoice.InvoiceId,Global.Company.CompanyId);
                    if (lPatientInvoicePayment != null && lPatientInvoicePayment.Count > 0)
                    {
                        if (k == MaxLine)
                        {
                            pdfDoc.Add(ReportMainTable);
                            pdfDoc.NewPage();
                            pdfDoc.Add(MTable);
                            ReportMainTable = new PdfPTable(Cols);
                            ReportMainTable.SetWidths(widths);
                            k = 1;
                        }
                        //invoice total table
                        //ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 7, 1));
                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Bill Amount", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoiceTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 7, 1));
                        ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 7, 1));
                        pdfDoc.Add(ReportMainTable);
                        k += 5;
                        // payment header                       
                        foreach (PatientInvoicePayment Payment in lPatientInvoicePayment)
                        {
                            PatientLedger Ledger = PatientLedgerManager.Instance.GetPatientLedgerById((long)Payment.LedgerId!);
                            if (Ledger != null)
                            {
                                if (k >= MaxLine)
                                {
                                    if (PaymentReportMainTable.Rows.Count <= MaxLine - k)
                                    {
                                        pdfDoc.Add(PaymentReportMainTable);
                                        k = PaymentReportMainTable.Rows.Count;
                                    }
                                    pdfDoc.NewPage();
                                    pdfDoc.Add(PTable);
                                    if (PaymentReportMainTable.Rows.Count+3 <= MaxLine - k)
                                    {
                                        PaymentReportMainTable = new PdfPTable(Cols);
                                        PaymentReportMainTable.SetWidths(pwidths);
                                    }
                                    k = 1;
                                }
                                else if (PaymentTotal == 0)
                                {
                                    pdfDoc.Add(PTable);
                                }

                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Payment.Date.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Ledger.RefNumber, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Ledger.PatientPaymentDetails.Count>0?Ledger.PatientPaymentDetails.First().PaymentType.ToString():string.Empty, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Ledger.PatientPaymentDetails.Count > 0 ? Ledger.PatientPaymentDetails.First().DocumentNumber != null ? Ledger.PatientPaymentDetails.First().DocumentNumber.ToString() : "":string.Empty, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Ledger.PatientPaymentDetails.Count > 0 ? Ledger.PatientPaymentDetails.First().Account != null ? Ledger.PatientPaymentDetails.First().Account.Name : "":string.Empty, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)(j == 1 ? BrushBorder.T : BrushBorder.N), Payment.Amount.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
                                PaymentTotal += Payment.Amount;
                                i++;
                                k++;
                                j++;
                            }
                        }                           
                        //payment total
                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Sub Total", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 5, 1));
                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "-"+ PaymentTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                        //PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                            
                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TB, "Payable Amount", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 5, 1));
                        PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TB, (InvoiceTotal - PaymentTotal).ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                        pdfDoc.Add(PaymentReportMainTable);
                    }
                    else
                    {
                        if (PaymentTotal > 0)
                        {
                            //invoice total table
                            if (k >= MaxLine)
                            {
                                pdfDoc.Add(ReportMainTable);
                                pdfDoc.NewPage();
                                pdfDoc.Add(MTable);
                                ReportMainTable = new PdfPTable(Cols);
                                ReportMainTable.SetWidths(widths);
                                k = 1;
                            }
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Bill Amount", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 5, 1));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoiceTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                            pdfDoc.Add(ReportMainTable);
                            // payment header
                            if (PaymentReportMainTable.Rows.Count+3 <= MaxLine - k)
                            {
                                pdfDoc.Add(PTable);
                            }
                            else
                            {
                                pdfDoc.NewPage();
                                pdfDoc.Add(PTable);
                                k = PaymentReportMainTable.Rows.Count;
                            }
                            //payment total
                            PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Sub Total", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 5, 1));
                            PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "-" + PaymentTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                            PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                            PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TB, "Payable Amount", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 5, 1));
                            PaymentReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TB, (InvoiceTotal - PaymentTotal).ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                            pdfDoc.Add(PaymentReportMainTable);
                        }
                        else
                        {
                            //invoice total
                            if (k >= MaxLine)
                            {
                                pdfDoc.Add(ReportMainTable);
                                pdfDoc.NewPage();
                                pdfDoc.Add(MTable);
                                ReportMainTable = new PdfPTable(Cols);
                                ReportMainTable.SetWidths(widths);
                                k = 1;
                            }
                            //ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 7, 1));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Bill Amount", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoiceTotal.ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                            //ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 7, 1));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TB, "Payable Amount", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));
                            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TB, (InvoiceTotal - PaymentTotal).ToString(formatSpecifier), (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false));
                            pdfDoc.Add(ReportMainTable);
                        }
                    }
                        PdfPTable EmptyWithNoBorder = new PdfPTable(6);
                        float[] ENBwidths = new float[] { 5f, 20f, 50f, 20f, 20f, 23f };
                        EmptyWithNoBorder.SetWidths(ENBwidths);
                        EmptyWithNoBorder.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "   ", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false, 6, 1));

                        pdfDoc.Add(EmptyWithNoBorder);
                        pdfDoc.Add(EmptyWithNoBorder);
                        pdfDoc.Add(Declaration(Invoice));
                        pdfDoc.Close();
                        Cursor.Current = Cursors.Default;
                        PdfFooter PdfFooter = new PdfFooter();
                        PdfFooter.IsReport = true;
                        PdfFooter.IsDate = true;
                        PdfFooter.Text = string.Empty;
                        PdfFooter.IsPageNumber = true;
                        PdfFooter.PdfFile = myMemoryStream.ToArray();
                        byte[] PdfFileWithFooter = PdfFooter.GetPdfFileWithFooter();
                        myMemoryStream.Close();

                        Cursor.Current = Cursors.WaitCursor;
                        PdfGeneration PdfGeneration = new PdfGeneration();
                        PdfGeneration.IsPrint = isprint;
                        PdfGeneration.FileName = "Invoice";
                        PdfGeneration.PdfFile = PdfFileWithFooter;
                        PdfGeneration.SavePdfFile();
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }
        public static PdfPTable ReportTableHeader()
        {
            int Cols = 7;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] widths = new float[] { 5f, 15f, 10f, 50f, 20f, 20f, 23f };
            ReportMainTable.SetWidths(widths);
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,InvoiceColumnPrint[0], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,InvoiceColumnPrint[1], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,InvoiceColumnPrint[2], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,InvoiceColumnPrint[3], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,InvoiceColumnPrint[4], (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,InvoiceColumnPrint[5], (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T,InvoiceColumnPrint[6], (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            return ReportMainTable;
        }
        public static PdfPTable ReportTableSubHeader()
        {
            int Cols = 7;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] widths = new float[] { 5f, 15f, 10f, 50f, 20f, 20f, 23f };
            ReportMainTable.SetWidths(widths);
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.B, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.B, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.B, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.B, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.B, "", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.B, "Rs.P.", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.B, "Rs.P.", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            return ReportMainTable;
        }
        public static PdfPTable ReportTablePaymentHeader()
        {
            int Cols = 6;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] widths = new float[] { 20f, 20f, 30f, 20f, 40f, 23f };
            ReportMainTable.SetWidths(widths);
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoicePaymentColumnPrint[0], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoicePaymentColumnPrint[1], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoicePaymentColumnPrint[2], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoicePaymentColumnPrint[3], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoicePaymentColumnPrint[4], (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            ReportMainTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, InvoicePaymentColumnPrint[5], (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false));
            return ReportMainTable;
        }
        public static PdfPTable PatientDetailsHeading(PatientInvoice Invoice)
        {
            PdfPTable? HeadTable = null;
            InPatientLocation? InPatientLocation = null;
            InPatientAdmission? InPatientAdmission = null;
            Patient Patient = PatientManager.Instance.GetPatientById((long)Invoice.PatientId!);
            PatientLedger ledger = PatientLedgerManager.Instance.GetLedgerByInvoiceId(Invoice.InvoiceId);
            Registration Registration = OpManager.Instance.GetRegisterByRegId((long)ledger.OpRegistrationId!);
            if (ledger.InPatientAdmissionId != null)
            {
                InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByOpId((long)ledger.OpRegistrationId!);
            }
            Registration Consultant = OpManager.Instance.GetOpPrimaryDoctorByOpId(Registration.Id);
            if (InPatientAdmission != null)
            {
                InPatientLocation = InPatientAdmission.PatientLocationHistory.OrderBy(x => x.DateMovedOut).Last();
            }
            if (Patient != null)
            {
                HeadTable = new PdfPTable(4);
                HeadTable.SetTotalWidth(new float[] { 18f, 37f,18f,27f });
                HeadTable.DefaultCell.BorderColor = BaseColor.DARK_GRAY;
                HeadTable.DefaultCell.BorderColorBottom = BaseColor.DARK_GRAY;
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TL, "Patient Name" + "        :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, Patient.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.T, "Bill Number" + "             :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.TR, Invoice.ReferenceNumber, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Age/Sex" + "                :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Patient.Age.ToString()+"/"+ Patient.Gender.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Bill Date" + "                  :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, Invoice.InvoiceDate.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Patient Id" + "              :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Patient.PatientNumber, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                
                if (ledger.InPatientAdmissionId != null)
                { 
                    HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Admission Date" + "      :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                    HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, InPatientAdmission!.DateOfAdmission.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                }
                else
                {
                    HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Registration Date" + "    :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                    HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, Registration.DateOfRegistration.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                }
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "OutPatient Id" + "        :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, ledger.InPatientAdmissionId != null ? (InPatientAdmission!.Status == InPatientStatus.DISCHARGED ? "" : (!string.IsNullOrEmpty(Registration.PatientOPNumber) ? Registration.PatientOPNumber : "")) : (!string.IsNullOrEmpty(Registration.PatientOPNumber) ? Registration.PatientOPNumber : ""), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));

                if (Registration != null)
                {
                    if (Registration.Status == Status.INPATIENT)
                    {
                        if (ledger.InPatientAdmissionId != null)
                        {
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "InPatient Id" + "             :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, InPatientAdmission!.PatientIPNumber != null ? InPatientAdmission.PatientIPNumber : "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));

                            if (InPatientLocation != null)
                            {
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Ward/Bed" + "             :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, InPatientLocation.Ward.Name + "/" + InPatientLocation.Bed.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                
                            }
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Primary Doctor" + "       :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, InPatientAdmission.MedicalTeamHistory.Last().PrimaryDoctor.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));

                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Address" + "                :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Patient.Address.FullAddressInSingleLine.Trim(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, true));

                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Discharge Date" + "      :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, InPatientAdmission.Status == InPatientStatus.DISCHARGED ? InPatientLocation!.DateMovedOut.ToString(Global.Company.DateFormat) : "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                        }
                        else
                        {
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Primary Doctor" + "        :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, Consultant.RequestedDoctor != null ? Consultant.RequestedDoctor.Name : "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));

                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Address" + "                :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Patient.Address.FullAddressInSingleLine.Trim(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, true));

                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                        }
                    }
                    else if(InPatientAdmission != null)
                    {
                        if (InPatientAdmission.Status == InPatientStatus.DISCHARGED)
                        {
                            if (ledger.InPatientAdmissionId == null)
                            {
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Primary Doctor" + "        :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, InPatientAdmission.MedicalTeamHistory.Last().PrimaryDoctor.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));

                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Address" + "                :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Patient.Address.FullAddressInSingleLine.Trim(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, true));

                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Discharge Date" + "       :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            }
                            else
                            {
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Primary Doctor" + "       :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, InPatientAdmission.MedicalTeamHistory.Last().PrimaryDoctor.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));

                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Address" + "                :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Patient.Address.FullAddressInSingleLine.Trim(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, true));

                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Discharge Date" + "      :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                                HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, InPatientAdmission.Status == InPatientStatus.DISCHARGED ? InPatientLocation!.DateMovedOut.ToString(Global.Company.DateFormat) : "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                            }
                        }
                    }
                    else
                    {
                        HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "Primary Doctor" + "        :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                        HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, Consultant.RequestedDoctor != null ? Consultant.RequestedDoctor.Name : "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                        
                        HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.L, "Address" + "                :", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                        HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, Patient.Address.FullAddressInSingleLine.Trim(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, true));

                        HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                        HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                    }
                }
                else
                {
                    HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.N, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                    HeadTable.AddCell(TableInnerCellAlignment((int)BrushBorder.R, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black"), false, BaseColor.WHITE, false));
                }
            }
            return HeadTable!;
        }
        public static PdfPTable SignatureTables() 
        {
            float[] PaymentSignatureTableWidths = new float[] { 100f };

            PdfPTable PaymentSignatureTables = new PdfPTable(4);

            PaymentSignatureTables.SetTotalWidth(new float[] { 25, 25, 25, 25 });
            PaymentSignatureTables.DefaultCell.BorderColor = BaseColor.WHITE;
            PaymentSignatureTables.DefaultCell.BorderColorBottom = BaseColor.WHITE;

            PdfPCell FillTable = TableInnerCellAlignment((int)BrushBorder.T, "Patient Signature", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false);
            PaymentSignatureTables.AddCell(FillTable);

            FillTable = TableInnerCellAlignment((int)BrushBorder.N, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false);
            PaymentSignatureTables.AddCell(FillTable);

            FillTable = TableInnerCellAlignment((int)BrushBorder.N, "", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false);
            PaymentSignatureTables.AddCell(FillTable);
            
            FillTable = TableInnerCellAlignment((int)BrushBorder.T, "Cashier", (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false);
            PaymentSignatureTables.AddCell(FillTable);            

            return PaymentSignatureTables;
        }
        private PdfPTable Declaration(PatientInvoice Invoice)
        {           
            int TaxCol = 3;
            PdfPTable taxPdfTable = new PdfPTable(TaxCol);
            float[] column = new float[] { 30f, 30f, 30f };            
            int MinimumHeight = 10;
            taxPdfTable.SetWidths(column);
            PdfPCell rowCell = new PdfPCell();            
            bool IsBank = Global.Company.CompanySalesSetup.IsBankDetailDisplayOnInvoice
                    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.BankDetails) ? true : false;
            bool IsDeclaration = Global.Company.CompanySalesSetup.IsDeclarationDisplayOnInvoice
                    && !string.IsNullOrEmpty(Global.Company.CompanySalesSetup.Declarations) ? true : false;
            bool IsUPI = Global.Company.CompanySalesSetup.IsPrintQRCode;
            int Cspan = IsBank && IsDeclaration && IsUPI ? TaxCol / 3 :
                        (IsBank && IsDeclaration && !IsUPI) ||
                        (IsBank && !IsDeclaration && IsUPI) ||
                        (!IsBank && IsDeclaration && IsUPI) ? TaxCol - (TaxCol / 2) :
                        (IsBank || IsDeclaration || IsUPI) ? TaxCol : 1;

            if (IsBank)
            {
                rowCell = new PdfPCell(new Phrase(Global.Company.CompanySalesSetup.BankDetails.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = Cspan;
                rowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                taxPdfTable.AddCell(rowCell);
            }
            if (IsUPI)
            {
                string UpiUrl = Global.Company.CompanySalesSetup.UPIId;
                UpiUrl=UpiUrl.Replace("&am=100.00", "&am="+ Invoice.Total.ToString(Global.Company.PrimaryCurrency.CurrencyFormat).Replace(",",""));
                MemoryStream MStream = new MemoryStream();
                var Image = QRCode.GenerateQRCode(UpiUrl);
                Image.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(MStream.ToArray());
                image.ScaleAbsoluteHeight(55);
                image.ScaleAbsoluteWidth(55);

                rowCell = new PdfPCell(new Phrase("Scan QR Code To Pay.", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.MinimumHeight = 20;
                rowCell.Colspan = Cspan;
                rowCell.Rowspan = 3;
                rowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                rowCell.AddElement(new Phrase("Scan QR Code To Pay.",PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                rowCell.AddElement(new Phrase("  ", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")));
                rowCell.AddElement(image);
                taxPdfTable.AddCell(rowCell);
            }
            if (IsDeclaration)
            {
                rowCell = new PdfPCell(new Phrase("For " + Global.Company.Name.Replace(",", System.Environment.NewLine), PdfDataAlignment.GetFont("Font_Normal_Italic_9_Black")));
                rowCell.MinimumHeight = MinimumHeight;
                rowCell.Colspan = Cspan;
                rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                rowCell.UseVariableBorders = true;
                rowCell.BorderColorLeft = BaseColor.WHITE;
                rowCell.BorderColorTop = BaseColor.WHITE;
                rowCell.BorderColorRight = BaseColor.WHITE;
                rowCell.BorderColorBottom = BaseColor.WHITE;
                taxPdfTable.AddCell(rowCell);
            }
            return taxPdfTable;
        }


        public readonly String[] PatientInvoiceColumnPrint = new String[]
        {
            "#", "Date", "Type", "Description", "Quantity", "Rate", "Amount"
        };
        public enum PatientInvoiceColumn
        {
            Sno, Date, Type, Description, Quantity, Rate, Amount
        };
        public void ExportA5LandscapeDotMatrixPrint(long invoiceId, string PrintPaper, string ReportName, bool isPrint)
        {
            string ItemsHeader = string.Empty;
            double SubTotal = 0.00;
            //double BillAmount = 0.00;
            //double PayableAmount = 0.00;
            PatientInvoice Invoice = PatientInvoiceManager.Instance.GetInvoiceById(invoiceId);
            InPatientLocation? InPatientLocation = null;
            InPatientAdmission? InPatientAdmission = null;
            Patient patient = PatientManager.Instance.GetPatientById((long)Invoice.PatientId!);
            PatientLedger patientledger = PatientLedgerManager.Instance.GetLedgerByInvoiceId(Invoice.InvoiceId);
            Registration Registration = OpManager.Instance.GetRegisterByRegId((long)patientledger.OpRegistrationId!);
            if (patientledger.InPatientAdmissionId != null)
            {
                InPatientAdmission = IpManager.Instance.GetAdmittedInPatientAdmissionByOpId((long)patientledger.OpRegistrationId!);
            }
            Registration Consultant = OpManager.Instance.GetOpPrimaryDoctorByOpId(Registration.Id);
            if (InPatientAdmission != null)
            {
                InPatientLocation = InPatientAdmission.PatientLocationHistory.OrderBy(x => x.DateMovedOut).Last();
            }
            int RoundingPrecision = Global.Company.PrimaryCurrency.RoundingPrecision;
            string formatSpecifier = $"F{RoundingPrecision}";

            if (Invoice == null)
            {
                MessageBox.Show("Somthing went wrong, the selected invoice is not valid.");
                return;
            }
            DataTable PatientInvoiceTotalDataTable = new DataTable();
            DataTable PatientInvoiceTable = new DataTable();
            PatientInvoiceTable.Columns.Add(PatientInvoiceColumnPrint[(int)PatientInvoiceColumn.Sno], typeof(string));
            PatientInvoiceTable.Columns.Add(PatientInvoiceColumnPrint[(int)PatientInvoiceColumn.Date], typeof(string));
            PatientInvoiceTable.Columns.Add(PatientInvoiceColumnPrint[(int)PatientInvoiceColumn.Type], typeof(string));
            PatientInvoiceTable.Columns.Add(PatientInvoiceColumnPrint[(int)PatientInvoiceColumn.Description], typeof(string));
            PatientInvoiceTable.Columns.Add(PatientInvoiceColumnPrint[(int)PatientInvoiceColumn.Quantity], typeof(string));
            PatientInvoiceTable.Columns.Add(PatientInvoiceColumnPrint[(int)PatientInvoiceColumn.Rate], typeof(string));
            PatientInvoiceTable.Columns.Add(PatientInvoiceColumnPrint[(int)PatientInvoiceColumn.Amount], typeof(string));
            if (Invoice != null)
            {
                IList<PatientLedger> lPatientLedger = PatientLedgerManager.Instance.ListAllEntryByInvoiceId(Invoice.InvoiceId);
                try
                {
                    if (lPatientLedger != null && lPatientLedger.Count > 0)
                    {
                        //fee details
                        int i = 1;
                        foreach (PatientLedger ledger in lPatientLedger)
                        {
                            if (ledger.Amount != 0)
                            {
                                DataRow PatientInvoiceTableNewRow = PatientInvoiceTable.NewRow();
                                PatientInvoiceTableNewRow[(int)PatientInvoiceColumn.Sno] = i.ToString();
                                PatientInvoiceTableNewRow[(int)PatientInvoiceColumn.Date] = ledger.Date.ToString(Global.Company.DateFormat);
                                PatientInvoiceTableNewRow[(int)PatientInvoiceColumn.Type] = ledger.InPatientAdmissionId != null ? "IP" : "OP";
                                PatientInvoiceTableNewRow[(int)PatientInvoiceColumn.Description] = ledger.Description;
                                PatientInvoiceTableNewRow[(int)PatientInvoiceColumn.Quantity] = "1";
                                PatientInvoiceTableNewRow[(int)PatientInvoiceColumn.Rate] = ledger.Amount.ToString(formatSpecifier);
                                PatientInvoiceTableNewRow[(int)PatientInvoiceColumn.Amount] = ledger.Amount.ToString(formatSpecifier);
                                SubTotal += ledger.Amount;
                                i++;
                                PatientInvoiceTable.Rows.Add(PatientInvoiceTableNewRow);
                            }
                        }
                    }
                    PatientInvoiceTotalDataTable.Columns.Add("1", typeof(string));
                    PatientInvoiceTotalDataTable.Columns.Add("2", typeof(string));

                    PatientInvoiceTotalDataTable.Rows.Add(new object[] { "Sub Total", (SubTotal).ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision)) });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ex.ToString());
                    return;
                }
            }
            if (PatientInvoiceTable.Rows.Count != 0)
            {
                fileName = "Invoice";
                PatientInvoiceDotMatrixPrint patientInvoiceDotMatrixPrint = new PatientInvoiceDotMatrixPrint(this);
                patientInvoiceDotMatrixPrint.GenerateDotmarixPrint(patient, Invoice!, isPrint, patientledger, InPatientAdmission!, Registration, InPatientLocation!, Consultant, PatientInvoiceTable, PatientInvoiceTotalDataTable);
            }
        }
    }
}
