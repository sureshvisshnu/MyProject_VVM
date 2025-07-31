using fa.model.Accounting.Masters;
using fa.model.OrderManagement;
using fa.report;
using fa.views.utils.Bills;
using fa.views.utils.Creditnote;
using fa.views.utils.Debitnote;
using fa.views.utils.Expenses;
using fa.views.utils.Invoices;
using fa.views.utils.Journal;
using fa.views.utils.Payments;
using fa.views.utils.Receipts;
using fa.views.utils.Sale;
using Fa.views.utils.Report.Purchase;
using Fa.views.utils.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static log4net.Appender.RollingFileAppender;

namespace fa.views.utils
{
    public enum PaperTypes
    {
        A2, A3, MM_105, A4_PORTRAIT, A4_LANDSCAPE, A5_PORTRAIT, A5_LANDSCAPE
    }
    public enum TransactionTypes
    {
        PAYMENT, RECEIPT, EXPENSE, INVOICE, BILL, JOURNAL, CREDITNOTE, DEBITNOTE
    }
    public enum AlignmentTypes
    {
        Prefix, Suffix
    }
    public enum LabelType
    {
        BARCODE, QRCODE
    }
    public class PrinterSetup
    {
        public static void SalePrintSetup(long SaleId, bool IsExport, Entrytype entrytype, bool isGSTInvoice, string selectedPrintPaper = null!) // Add optional parameter for selected paper
        {
            string PrintPaper = selectedPrintPaper;
            DateTime YearStartDatex = Global.getCurrentFiscalYearStartDate();
            DateTime YearEndDatex = Global.getCurrentFiscalYearEndDate();

            // Get configured paper format
            if (string.IsNullOrEmpty(PrintPaper))
            {
                DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
                DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();

                if (entrytype == Entrytype.SALE)
                {
                    PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x =>
                        x.YearStartDate == YearStartDate &&
                        x.YearEndDate == YearEndDate &&
                        x.EntryType == EntryType.SALES)?.PrintPaperFormat.Name!;
                }
                else if (entrytype == Entrytype.QUOTE)
                {
                    PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x =>
                        x.YearStartDate == YearStartDate &&
                        x.YearEndDate == YearEndDate &&
                        x.EntryType == EntryType.SALES_QUOTE)?.PrintPaperFormat.Name!;
                }
                else if (entrytype == Entrytype.RETURN)
                {
                    PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x =>
                        x.YearStartDate == YearStartDate &&
                        x.YearEndDate == YearEndDate &&
                        x.EntryType == EntryType.SALES_RETURN)?.PrintPaperFormat.Name!;
                }
            }


            bool IsDotMatrix = Global.Company.IdSpaces.FirstOrDefault(x =>
                x.YearStartDate == YearStartDatex &&
                x.YearEndDate == YearEndDatex &&
                x.EntryType == EntryType.SALES)?.IsDotMatrix ?? false;

            string PrintFormat = IsDotMatrix && !IsExport ? "Dotmatrix" : "Laser";

            // Handle print formats
            if (PrintPaper == "105 MM ROLL" || PrintPaper == "80 MM ROLL")
            {
                if (PrintFormat == "Dotmatrix" && !IsExport)
                {
                    new SalePrintSaveDotmatrix105mm().ExportToFileOrPrint(SaleId, "pdf", !IsExport);
                }
                else
                {
                    new SalePrintSave105mm().ExportToFileOrPrint(SaleId, "pdf", !IsExport);
                }
            }
            else if (!isGSTInvoice && (PrintPaper == "A5 PORTRAIT" || PrintPaper == "A5 LANDSCAPE"))
            {
                // Non-GST A5 paper - use simplified format
                bool isLandscape = PrintPaper.EndsWith("LANDSCAPE");
                new SalePrintSaveA5SimplifiedFormat().ExportToFileOrPrint(
                    SaleId,
                    PrintPaper,
                    PrintFormat,
                    !IsExport,
                    entrytype,
                    isLandscape);
            }
            else if (!isGSTInvoice && (PrintPaper == "A4 PORTRAIT" || PrintPaper == "A4 LANDSCAPE"))
            {
                bool isLandscape = PrintPaper.EndsWith("LANDSCAPE");
                if (ShouldUseSpecialA4Format(SaleId))
                {
                    new SalePrintSaveA4SimplifiedFormat().ExportToFileOrPrint(
                        SaleId,
                        PrintPaper,
                        PrintFormat,
                        !IsExport,
                        isLandscape);
                }
                else
                {
                    // Use regular A4 format
                    new SalePrintSaveA4EinvoiceFormat().ExportToFileOrPrint(
                        SaleId,
                        PrintPaper,
                        PrintFormat,
                        !IsExport);
                }
            }
            else if (isGSTInvoice || PrintPaper == "A4 PORTRAIT" || PrintPaper == "A4 LANDSCAPE")
            {
                // GST invoice or A4 paper - use existing GST logic
                // if ((PrintPaper == "A4 PORTRAIT" || PrintPaper == "A4 LANDSCAPE") && PrintFormat == "Laser")
                if (PrintPaper == "A4 PORTRAIT" && PrintFormat == "Laser")
                {
                    if (ShouldUseSpecialA4Format(SaleId))
                    {
                        bool isLandscape = PrintPaper.EndsWith("LANDSCAPE");

                        // ✅ Correct instantiation and method call
                        new SalePrintSaveA4EinvoiceFormat().ExportToFileOrPrint(
                            SaleId,
                            PrintPaper,
                            PrintFormat,
                            !IsExport
                        );
                    }
                    else
                    {
                        new SalePrintSaveA4EinvoiceFormat().ExportToFileOrPrint(
                            SaleId,
                            PrintPaper,
                            PrintFormat,
                            !IsExport
                        );
                    }
                }

            }
        }

        public static void SalePrintSetupNew(long SaleId, bool IsExport, Entrytype entrytype)
        {
            string PrintPaper = "";
            DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();

            if (entrytype == Entrytype.SALE)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES)!.PrintPaperFormat.Name;
            }
            if (entrytype == Entrytype.QUOTE)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES_QUOTE).PrintPaperFormat.Name;
            }
            if (entrytype == Entrytype.RETURN)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES_RETURN).PrintPaperFormat.Name;
            }

            bool IsDotMatrix = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES)!.IsDotMatrix;
            string PrintFormat = IsDotMatrix && !IsExport ? "Dotmatrix" : "Laser";

            if (PrintPaper == "105 MM ROLL" || PrintPaper == "80 MM ROLL")
            {
                if (PrintFormat == "Dotmatrix" && !IsExport)
                {
                    SalePrintSaveDotmatrix105mm SalePrintSaveDotmatrix105mm = new SalePrintSaveDotmatrix105mm();
                    SalePrintSaveDotmatrix105mm.ExportToFileOrPrint(SaleId, "pdf", IsExport ? false : true);
                }
                else
                {
                    SalePrintSave105mm SalePrintSave105mm = new SalePrintSave105mm();
                    SalePrintSave105mm.ExportToFileOrPrint(SaleId, "pdf", IsExport ? false : true);
                }
            }
            else if (PrintPaper == "A5 PORTRAIT" || PrintPaper == "A5 LANDSCAPE")
            {
                // Use the new simplified format for A5 paper
                SalePrintSaveA5SimplifiedFormat printer = new SalePrintSaveA5SimplifiedFormat();
                bool isLandscape = PrintPaper == "A5 LANDSCAPE";
                //printer.ExportToFileOrPrint(SaleId, PrintPaper, PrintFormat, IsExport ? false : true, isLandscape);
                printer.ExportToFileOrPrint(SaleId, PrintPaper, PrintFormat, IsExport ? false : true, entrytype, isLandscape);

            }
            else if (PrintPaper == "A4 PORTRAIT" || PrintPaper == "A4 LANDSCAPE")
            {
                if (PrintPaper == "A4 PORTRAIT" && PrintFormat == "Laser")
                {
                    SalePrintSaveA4EinvoiceFormat SalePrintSaveA5A4 = new SalePrintSaveA4EinvoiceFormat();
                    SalePrintSaveA5A4.ExportToFileOrPrint(SaleId, PrintPaper, PrintFormat, IsExport ? false : true);
                }
                else
                {
                    SalePrintSaveA5A4 SalePrintSaveA5A4 = new SalePrintSaveA5A4();
                    SalePrintSaveA5A4.ExportToFileOrPrint(SaleId, PrintPaper, PrintFormat, IsExport ? false : true);
                }
            }
        }

        // Add this helper method to determine when to use the special A4 format
        private static bool ShouldUseSpecialA4Format(long saleId)
        {
            // Add your logic here to determine when to use the special A4 format
            // For example:
            // - Check if the sale has a specific customer
            // - Check if the sale has a specific flag
            // - Always return true if you want to always use it

            return true; // Modify this as needed
        }
        public static void SalePrintSetupGST(long SaleId, bool IsExport, Entrytype entrytype)
        {
            string PrintPaper = "";
            DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();
            if (entrytype == Entrytype.SALE)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES)!.PrintPaperFormat.Name;
            }
            if (entrytype == Entrytype.QUOTE)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES_QUOTE)!.PrintPaperFormat.Name;
            }
            if (entrytype == Entrytype.RETURN)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES_RETURN)!.PrintPaperFormat.Name;
            }
            bool IsDotMatrix = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES)!.IsDotMatrix;
            string PrintFormat = IsDotMatrix && !IsExport ? "Dotmatrix" : "Laser";
            if (PrintPaper == "105 MM ROLL" || PrintPaper == "80 MM ROLL")
            {
                if (PrintFormat == "Dotmatrix" && !IsExport)
                {
                    SalePrintSaveDotmatrix105mm SalePrintSaveDotmatrix105mm = new SalePrintSaveDotmatrix105mm();
                    SalePrintSaveDotmatrix105mm.ExportToFileOrPrint(SaleId, "pdf", IsExport ? false : true);
                }
                else
                {
                    SalePrintSave105mm SalePrintSave105mm = new SalePrintSave105mm();
                    SalePrintSave105mm.ExportToFileOrPrint(SaleId, "pdf", IsExport ? false : true);
                }
            }
            else if (PrintPaper == "A5 LANDSCAPE" || PrintPaper == "A4 PORTRAIT" || PrintPaper == "A4 LANDSCAPE")
            {
                if (PrintPaper == "A4 PORTRAIT" && PrintFormat == "Laser")
                {
                    SalePrintSaveA4EinvoiceFormat SalePrintSaveA5A4 = new SalePrintSaveA4EinvoiceFormat();
                    SalePrintSaveA5A4.ExportToFileOrPrint(SaleId, PrintPaper, PrintFormat, IsExport ? false : true);
                }
                else
                {
                    SalePrintSaveA5A4 SalePrintSaveA5A4 = new SalePrintSaveA5A4();
                    SalePrintSaveA5A4.ExportToFileOrPrint(SaleId, PrintPaper, PrintFormat, IsExport ? false : true);
                }
            }
        }
        public static void PurchaseOrderPrintSetup(long PurchaseId, bool IsExport)
        {
            string PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.DEBIT_NOTE).PrintPaperFormat.Name;
            bool IsDotMatrix = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.DEBIT_NOTE).IsDotMatrix;
            string PrintFormat = IsDotMatrix && !IsExport ? "Dotmatrix" : "Laser";
            if (PrintPaper == "105 MM ROLL" || PrintPaper == "80 MM ROLL")
            {
                if (PrintFormat == "Dotmatrix" && !IsExport)
                {
                    PurchasePrintSaveDotmatrix105mm PurchasePrintSaveDotmatrix105mm = new PurchasePrintSaveDotmatrix105mm();
                    PurchasePrintSaveDotmatrix105mm.ExportToFileOrPrint(PurchaseId, "pdf", IsExport ? false : true);
                }
            }
            else if (PrintPaper == "A5 LANDSCAPE" || PrintPaper == "A4 PORTRAIT" || PrintPaper == "A4 LANDSCAPE")
            {
                PurchaseOrderPrintSaveA5A4 PurchaseOrderPrintSaveA5A4 = new PurchaseOrderPrintSaveA5A4();
                PurchaseOrderPrintSaveA5A4.ExportToFileOrPrint(PurchaseId, PrintPaper, PrintFormat, IsExport ? false : true);
            }
        }
        public static void PurchasePrintSetup(long PurchaseId, bool IsExport)
        {

            string PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.DEBIT_NOTE).PrintPaperFormat.Name;
            bool IsDotMatrix = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.DEBIT_NOTE).IsDotMatrix;
            string PrintFormat = IsDotMatrix && !IsExport ? "Dotmatrix" : "Laser";
            if (PrintPaper == "105 MM ROLL" || PrintPaper == "80 MM ROLL")
            {
                if (PrintFormat == "Dotmatrix" && !IsExport)
                {
                    PurchasePrintSaveDotmatrix105mm PurchasePrintSaveDotmatrix105mm = new PurchasePrintSaveDotmatrix105mm();
                    PurchasePrintSaveDotmatrix105mm.ExportToFileOrPrint(PurchaseId, "pdf", IsExport ? false : true);
                }
                //else       
                //{
                //    SalePrintSave105mm SalePrintSave105mm = new SalePrintSave105mm();
                //    SalePrintSave105mm.ExportToFileOrPrint(PurchaseId, "pdf", IsExport ? false : true);
                //}
            }
            else if (PrintPaper == "A5 LANDSCAPE" || PrintPaper == "A4 PORTRAIT" || PrintPaper == "A4 LANDSCAPE")
            {
                PurchasePrintSaveA5A4 PurchasePrintSaveA5A4 = new PurchasePrintSaveA5A4();
                PurchasePrintSaveA5A4.ExportToFileOrPrint(PurchaseId, PrintPaper, PrintFormat, IsExport ? false : true);
            }
        }
        public static void TransactionPrintSetup(long TransactionId, TransactionTypes TransactionTypes)
        {
            if (TransactionTypes == TransactionTypes.INVOICE)
            {
                InvoiceSavePrintA4 InvoiceSavePrintA4 = new InvoiceSavePrintA4();
                InvoiceSavePrintA4.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
            else if (TransactionTypes == TransactionTypes.BILL)
            {
                BillSavePrintA4 BillSavePrintA4 = new BillSavePrintA4();
                BillSavePrintA4.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
            else if (TransactionTypes == TransactionTypes.RECEIPT)
            {
                ReceiptSavePrint ReceiptSavePrint = new ReceiptSavePrint();
                ReceiptSavePrint.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
            else if (TransactionTypes == TransactionTypes.PAYMENT)
            {
                PaymentSavePrint PaymentSavePrint = new PaymentSavePrint();
                PaymentSavePrint.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
            else if (TransactionTypes == TransactionTypes.EXPENSE)
            {
                ExpenseSavePrintA4 ExpenseSavePrintA4 = new ExpenseSavePrintA4();
                ExpenseSavePrintA4.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
            else if (TransactionTypes == TransactionTypes.JOURNAL)
            {
                JournalSavePrintA4 JournalSavePrintA4 = new JournalSavePrintA4();
                JournalSavePrintA4.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
            else if (TransactionTypes == TransactionTypes.DEBITNOTE)
            {
                DebitnoteSavePrintA4 DebitnoteSavePrintA4 = new DebitnoteSavePrintA4();
                DebitnoteSavePrintA4.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
            else if (TransactionTypes == TransactionTypes.CREDITNOTE)
            {
                CreditnoteSavePrintA4 CreditnoteSavePrintA4 = new CreditnoteSavePrintA4();
                CreditnoteSavePrintA4.ExportToFileOrPrint(TransactionId, "pdf", true);
            }
        }
        public class TaxTable
        {
            public long? Id { get; set; }
            public double TaxPer { get; set; }
            public double TaxAmount { get; set; }
            public double SubTotal { get; set; }
            public long? SaleTaxMapId { get; set; }


        }
    }
}
