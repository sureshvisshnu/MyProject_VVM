using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Hms.common;
using fa.model.Hms.Master;
using fa.report;
using fa.report.common;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;

namespace Fa.report.accounting.master
{
    public class RptLedger : Report
    {
        public long[] AccountIds { get; set; }
        public long? CostCenterId { get; set; }
        List<Ledger> _Legers = new List<Ledger>();
        public List<Ledger> Ledgers
        {
            get
            {
                return _Legers;
            }
        }

        public override string ReportTitle()
        {
            return String.Format("A/C:");
        }
        public string ReportSubTitle()
        {
            return String.Format("From: {0} To: {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Ledger {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }

        public override void GenerateReport()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    foreach (long AccountId in AccountIds)
                    {
                        Account Account = AccountManager.Instance.GetAccountById(AccountId);
                        if (Account != null)
                        {
                            List<DayBook> DayBooks = null;
                            Ledger Ledger = new Ledger();
                            Ledger.Account = Account;
                            List<DayBook> dayBook = Context.DoubleEntries.Where(x => x.AccountId == Account.Id && x.Date < this.FromDate).ToList();
                            foreach (DayBook dBook in dayBook)
                            {
                                Ledger.OpeningBalance += (dBook.Amount * (dBook.TransactionType == DaybookTransactionType.Journal ? 1 : Account.AccountGroup.AccountGroupClassification.DebitMultiplier));
                            }
                            Ledger.OpeningBalance += Account.Balance;
                            Ledger.ClosingBalance = Ledger.OpeningBalance;
                            if (CostCenterId == null)
                            {
                                DayBooks = (from daybook in Context.DoubleEntries.Include("Account").Include("Patient").Include("Account.AccountGroup").Include("Account.AccountGroup.AccountGroupClassification")
                                            where (daybook.CompanyId == this.Company.CompanyId
                                            && daybook.Date >= this.FromDate
                                            && daybook.Date <= ToDate
                                            && daybook.AccountId == Account.Id)
                                            select daybook).OrderBy(x => x.AccountId).ThenBy(x => x.Date).ToList();
                            }
                            else
                            {
                                DayBooks = (from daybook in Context.DoubleEntries.Include("Account").Include("Patient").Include("Account.AccountGroup").Include("Account.AccountGroup.AccountGroupClassification")
                                            where (daybook.CompanyId == this.Company.CompanyId
                                            && daybook.Date >= this.FromDate
                                            && daybook.Date <= ToDate
                                            && daybook.AccountId == Account.Id
                                            && daybook.CostCenterId == CostCenterId)
                                            select daybook).OrderBy(x => x.AccountId).ThenBy(x => x.Date).ToList();
                            }

                            foreach (DayBook dBook in DayBooks)
                            {
                                LedgerLineItem LedgerLineItem = new LedgerLineItem(dBook);
                                Ledger.LineItems.Add(LedgerLineItem);
                                Ledger.ClosingBalance += (dBook.Amount * (dBook.TransactionType == DaybookTransactionType.Journal ? 1 : (long)Account.AccountGroup.DebitMultiplier));
                            }
                            if (Ledger.OpeningBalance != 0 || Ledger.LineItems.Count > 0)
                            {
                                Ledgers.Add(Ledger);
                            }

                        }
                    }
                }
            }
        }       
    }
    public class Ledger
    {
        public Account Account { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public CrDr CreditOrDebit(Double lAmount)
        {
            return (lAmount * Account.AccountGroup.DebitMultiplier >= 0 ? CrDr.DR : CrDr.CR);
        }
        public List<LedgerLineItem> LineItems { get; set; } = new List<LedgerLineItem>();
    }
    public class LedgerLineItem
    {
        public Patient Patient { get; set; }
        public Account Account { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public double Amount { get; set; }
        public DaybookTransactionType TransactionType { get; set; }
        public LedgerLineItem()
        {

        }
        public LedgerLineItem(DayBook DayBook)
        {
            this.Patient = DayBook.Patient;
            this.Account = DayBook.Account;
            this.Date = DayBook.Date;
            this.Description = DayBook.Description;
            this.Amount = DayBook.Amount;
            this.TransactionType = DayBook.TransactionType;
        }
        public CrDr CreditOrDebit()
        {
            return (this.Amount * (TransactionType == DaybookTransactionType.Journal ? 1 : Account.AccountGroup.DebitMultiplier) >= 0 ? CrDr.DR : CrDr.CR);
        }
    }
}
