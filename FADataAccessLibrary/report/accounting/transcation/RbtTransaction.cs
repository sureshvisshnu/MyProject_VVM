using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using Microsoft.EntityFrameworkCore;
using FaData.Utils;

namespace fa.report.accounting.transcation
{
    public class RbtTransaction : Report
    {
        public string Transaction { get; set; }
        public double Total=0.00;
        public List<TransactionLineItem> TransactionLineItems = new List<TransactionLineItem>();
        public override string ReportTitle()
        {
            return String.Format("{0} Transaction",Transaction);
        }
        public  string ReportSubTitle()
        {
            return String.Format("From {0} To {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("{6} Transaction {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,Transaction);
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (Transaction== "Invoice")
                    {
                        List<Invoice> lInvoice = Context.Invoices.Include("Customer").Where(x => x.CompanyId == Company.CompanyId && x.InvoiceDate>=FromDate && x.InvoiceDate <= ToDate).ToList();
                        if (lInvoice != null)
                        {
                            foreach(Invoice Invoice in lInvoice)
                            {
                                TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                TransactionLineItem.Date = Invoice.InvoiceDate;
                                TransactionLineItem.Reference = Invoice.ReferenceNumber;
                                TransactionLineItem.Account = Invoice.Customer;
                                TransactionLineItem.Description = Invoice.Memo;
                                TransactionLineItem.Amount = Invoice.Total;
                                TransactionLineItems.Add(TransactionLineItem);
                                Total += TransactionLineItem.Amount;
                            }
                        }
                    }
                    else if(Transaction == "Bill")
                    {
                        List<Bill> lBill = Context.Bills.Include("Vendor").Where(x => x.CompanyId == Company.CompanyId && x.BillDate >= FromDate && x.BillDate <= ToDate).ToList();
                        if (lBill != null)
                        {
                            foreach (Bill Bill in lBill)
                            {
                                TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                TransactionLineItem.Date = Bill.BillDate;
                                TransactionLineItem.Reference = Bill.ReferenceNumber;
                                TransactionLineItem.Account = Bill.Vendor;
                                TransactionLineItem.Description = Bill.Memo;
                                TransactionLineItem.Amount = Bill.Total;
                                TransactionLineItems.Add(TransactionLineItem);
                                Total += TransactionLineItem.Amount;
                            }
                        }
                    }
                    else if (Transaction == "Receipt")
                    {
                        List<Receipt> lReceipt = Context.Receipts.Include("Account").Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= FromDate && x.TransactionDate <= ToDate).ToList();
                        if (lReceipt != null)
                        {
                            foreach (Receipt Receipt in lReceipt)
                            {
                                TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                TransactionLineItem.Date = Receipt.TransactionDate;
                                TransactionLineItem.Reference = Receipt.Reference;
                                TransactionLineItem.Account = Receipt.Account;
                                TransactionLineItem.Description = Receipt.Description;
                                TransactionLineItem.Amount = (double)Receipt.Amount;
                                TransactionLineItems.Add(TransactionLineItem);
                                Total += TransactionLineItem.Amount;
                            }
                        }
                    }
                    else if (Transaction == "Payment")
                    {
                        List<Payment> lPayment = Context.Payments
                            .Include(p => p.Account) 
                            .Where(x => x.CompanyId == Company.CompanyId
                                && x.TransactionDate >= FromDate
                                && x.TransactionDate <= ToDate)
                            .ToList();
                        if (lPayment != null)
                        {
                            foreach (Payment Payment in lPayment)
                            {
                                TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                TransactionLineItem.Date = Payment.TransactionDate;
                                TransactionLineItem.Reference = Payment.Reference;
                                TransactionLineItem.Account = Payment.Account;
                                TransactionLineItem.Description = Payment.Description;
                                TransactionLineItem.Amount = (double)Payment.Amount;
                                TransactionLineItems.Add(TransactionLineItem);
                                Total += TransactionLineItem.Amount;
                            }
                        }
                    }
                    else if (Transaction == "Credit Note")
                    {

                        List<CreditNote> lCreditNote = Context.CreditNotes
                                        .Include(cn => cn.Account)
                                        .Where(x => x.CompanyId == Company.CompanyId &&
                                                    x.TransactionDate >= FromDate &&
                                                    x.TransactionDate <= ToDate &&
                                                    x.Account.AccountType == AccountType.CUSTOMER) 
                                        .ToList();

                        if (lCreditNote != null)
                        {
                            foreach (CreditNote CreditNote in lCreditNote)
                            {
                                TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                TransactionLineItem.Date = CreditNote.TransactionDate;
                                TransactionLineItem.Reference = CreditNote.ReferenceNumber;
                                TransactionLineItem.Account = CreditNote.Account;
                                TransactionLineItem.Description = CreditNote.Note;
                                TransactionLineItem.Amount = CreditNote.Amount;
                                TransactionLineItems.Add(TransactionLineItem);
                                Total += TransactionLineItem.Amount;
                            }
                        }
                    }
                    else if (Transaction == "Debit Note")
                    {
                        List<DebitNote> lDebitNote = Context.DebitNotes
                                        .Include(dn => dn.Account)
                                        .Where(x => x.CompanyId == Company.CompanyId &&
                                                    x.TransactionDate >= FromDate &&
                                                    x.TransactionDate <= ToDate &&
                                                    x.Account.AccountType == AccountType.SUPPLIER)
                                        .ToList();

                        if (lDebitNote != null)
                        {
                            foreach (DebitNote DebitNote in lDebitNote)
                            {
                                TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                TransactionLineItem.Date = DebitNote.TransactionDate;
                                TransactionLineItem.Reference = DebitNote.ReferenceNumber;
                                TransactionLineItem.Account = DebitNote.Account;
                                TransactionLineItem.Description = DebitNote.Note;
                                TransactionLineItem.Amount = DebitNote.Amount;
                                TransactionLineItems.Add(TransactionLineItem);
                                Total += TransactionLineItem.Amount;
                            }
                        }
                    }
                    else if (Transaction == "Expense")
                    {
                        List<Expense> lExpense = Context.Expenses.Include("Payee").Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= FromDate && x.TransactionDate <= ToDate).ToList();
                        if (lExpense != null)
                        {
                            foreach (Expense Expense in lExpense)
                            {
                                TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                TransactionLineItem.Date = Expense.TransactionDate;
                                TransactionLineItem.Reference = Expense.Reference;
                                TransactionLineItem.Account = Expense.Payee;
                                TransactionLineItem.Description = Expense.Memo;
                                TransactionLineItem.Amount = Expense.Amount;
                                TransactionLineItems.Add(TransactionLineItem);
                                Total += TransactionLineItem.Amount;
                            }
                        }
                    }
                    else if (Transaction == "Journal")
                    {
                        List<Journal> lJournal = Context.Journals.Include("JournalDetails").Where(x => x.CompanyId == Company.CompanyId && x.TransactionDate >= FromDate && x.TransactionDate <= ToDate).ToList();
                        if (lJournal != null)
                        {
                            foreach (Journal Journal in lJournal)
                            {
                                foreach (JournalDetail Detail in Journal.JournalDetails)
                                {
                                    JournalDetail JournalDetails = Context.JournalDetails.Include("ToAccount").FirstOrDefault(x => x.JournalDetailId == Detail.JournalDetailId);
                                    TransactionLineItem TransactionLineItem = new TransactionLineItem();
                                    TransactionLineItem.Date = Journal.TransactionDate;
                                    TransactionLineItem.Account = JournalDetails.ToAccount;
                                    TransactionLineItem.Reference = Journal.ReferenceNumber;
                                    TransactionLineItem.Description = JournalDetails.Description;
                                    TransactionLineItem.Amount = (double)JournalDetails.Amount;
                                    TransactionLineItems.Add(TransactionLineItem);
                                    Total += TransactionLineItem.Amount;
                                }
                            }
                        }
                    }
                    
                }
            }

        }
    }
    public class TransactionLineItem
    {
        public DateTime Date { get; set; }
        public String Reference { get; set; }
        public Account Account { get; set; }
        public String Description { get; set; }
        public double Amount { get; set; }
        public TransactionLineItem()
        {

        }
    }
}
