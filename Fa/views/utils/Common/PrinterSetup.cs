using fa.model.OrderManagement;
using fa.views.utils.Bills;
using fa.views.utils.Creditnote;
using fa.views.utils.Debitnote;
using fa.views.utils.Expenses;
using fa.views.utils.Invoices;
using fa.views.utils.Journal;
using fa.views.utils.Payments;
using fa.views.utils.Receipts;
using fa.views.utils.Sale;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using fa.model.Accounting.Masters;
using fa.report;
using Fa.views.utils.Report.Purchase;
using Fa.views.utils.Sale;

namespace fa.views.utils
{
    public enum PaperTypes
    {
        A2, A3,MM_105,A4_PORTRAIT, A4_LANDSCAPE,A5_PORTRAIT,A5_LANDSCAPE
    }
    public enum TransactionTypes
    {
        PAYMENT,RECEIPT,EXPENSE,INVOICE,BILL,JOURNAL,CREDITNOTE,DEBITNOTE
    }
    public enum AlignmentTypes
    {
        Prefix, Suffix
    }
    public enum LabelType
    {
        BARCODE,QRCODE
    }
    public class PrinterSetup
    {
        public static void SalePrintSetup(long SaleId ,bool IsExport, Entrytype entrytype)
        {
            string PrintPaper = "";
            DateTime YearStartDate = Global.getCurrentFiscalYearStartDate();
            DateTime YearEndDate = Global.getCurrentFiscalYearEndDate();
            if (entrytype == Entrytype.SALE)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES).PrintPaperFormat.Name;
            }
            if (entrytype == Entrytype.QUOTE)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES_QUOTE).PrintPaperFormat.Name;
            }
            if (entrytype == Entrytype.RETURN)
            {
                PrintPaper = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES_RETURN).PrintPaperFormat.Name;
            }
            bool IsDotMatrix = Global.Company.IdSpaces.FirstOrDefault(x => x.YearStartDate == YearStartDate && x.YearEndDate == YearEndDate && x.EntryType == EntryType.SALES). IsDotMatrix;
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

            string PrintPaper = Global.Company.IdSpaces.FirstOrDefault( x => x.YearStartDate == Global.getCurrentFiscalYearStartDate() && x.YearEndDate == Global.getCurrentFiscalYearEndDate() && x.EntryType == EntryType.DEBIT_NOTE).PrintPaperFormat.Name;
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
            if(TransactionTypes== TransactionTypes.INVOICE)
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
