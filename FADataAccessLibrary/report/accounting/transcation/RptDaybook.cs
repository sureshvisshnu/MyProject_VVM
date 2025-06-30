using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.report;
using fa.report.common;
using Microsoft.EntityFrameworkCore;
using FaData.Utils;
using fa.model.Hms.Master;
using MySqlConnector;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;
using System.ComponentModel.Design;
using System.Transactions;

namespace Fa.report.accounting.master
{
    public class RptDayBook : Report
    {
        public long? CostcenterId { get; set; }
        public RptDayBookLineItem OpeningBalance { get; set; }
        public RptDayBookLineItem ClosingBalance { get; set; }
        public double TotalCredit { get; set; }
        public double TotalDebit { get; set; }
        public IList<RptDayBookLineItem> LineItems { get; } = new List<RptDayBookLineItem>();

        public override string ReportTitle()
        {
            return String.Format("Daybook From: {0} To {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));
        }
        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("DayBook {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        public override void GenerateReport()
        {
            GetDbookOpBalance();
            if (OpeningBalance != null)
            {
                TotalCredit = OpeningBalance.CreditOrDebit() == CrDr.CR ? Math.Abs(OpeningBalance.Amount) : 0;
                TotalDebit = OpeningBalance.CreditOrDebit() == CrDr.DR ? Math.Abs(OpeningBalance.Amount) : 0;
                ClosingBalance = new RptDayBookLineItem(OpeningBalance.Patient, OpeningBalance.Account, this.ToDate, "Cl.Balance", OpeningBalance.Amount, DaybookTransactionType.Invoice);
            }
            List<DayBook> DayBooks = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (CostcenterId != null)
                {
                    DayBooks = (from daybook in Context.DoubleEntries
                                where (daybook.CompanyId == this.Company.CompanyId
                                && daybook.CostCenterId == CostcenterId
                                && daybook.AccountId != Company.CashOnHandAccountId
                                && daybook.Date >= this.FromDate
                                && daybook.Date <= ToDate
                                && daybook.LedgerOnly == false)
                                select daybook).OrderBy(x => x.Date).ToList<DayBook>();
                }
                else
                {
                    DayBooks = (from daybook in Context.DoubleEntries
                                .Include("Account")
                                .Include("Patient")
                                .Include("Account.AccountGroup")
                                .Include("Account.AccountGroup.AccountGroupClassification")
                                where daybook.CompanyId == this.Company.CompanyId
                   && daybook.Date >= this.FromDate
                   && daybook.Date <= this.ToDate
                   && daybook.LedgerOnly == false
                   && daybook.AccountId != Company.CashOnHandAccountId
                                select daybook)
              .OrderBy(x => x.Date)
              .ToList<DayBook>();
                }

                if (DayBooks != null && DayBooks.Count > 0)
                {
                    foreach (DayBook db in DayBooks)
                    {
                        RptDayBookLineItem LineItem = new RptDayBookLineItem(db.Patient, db.Account, db.Date, db.Description, db.Amount, db.TransactionType);
                        if (LineItem.CreditOrDebit() == CrDr.CR)
                        {
                            TotalCredit += Math.Abs(LineItem.Amount);
                        }
                        else
                        {
                            TotalDebit += Math.Abs(LineItem.Amount);
                        }
                        LineItems.Add(LineItem);
                    }
                }
            }
            GetDbookClBalance();
        }

        private void GetDbookOpBalance()
        {
            double OpBal = 0;
            Account Account = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> lAccount = (from account in Context.Accounts.Include("AccountGroup").Include("AccountGroup.AccountGroupClassification") where (account.CompanyId == Company.CompanyId && account.Id == Company.CashOnHandAccountId) select account).ToList();
                if (lAccount.Count > 0)
                {
                    Account = lAccount.First();
                    OpBal = Account.Balance;
                    if (CostcenterId != null)
                    {
                        OpBal -= (from daybook in Context.DoubleEntries/*.Include("AccountGroup").Include("AccountGroup.AccountGroupClassification")*/
                                  where (daybook.CompanyId == this.Company.CompanyId && daybook.AccountId != Company.CashOnHandAccountId && daybook.CostCenterId == CostcenterId
                                 && daybook.LedgerOnly == false && daybook.Date < this.FromDate)
                                  select daybook).Sum(x => (float?)x.Amount) ?? 0;
                    }
                    else
                    {
                        OpBal -= (from daybook in Context.DoubleEntries
                                .Include("Account")
                                .Include("Account.AccountGroup")
                                .Include("Account.AccountGroup.AccountGroupClassification")
                                  where daybook.CompanyId == this.Company.CompanyId
                                    && ((daybook.AccountId == Company.CashOnHandAccountId) || (daybook.AccountId == null && daybook.TransactionType == DaybookTransactionType.Payment_Hms && daybook.PatientId != null))
                                    && daybook.LedgerOnly == false
                                    && daybook.Date < this.FromDate
                                  select daybook).Sum(x => (float?)x.Amount) ?? 0;
                    }
                    Account.Balance = OpBal;
                    OpeningBalance = new RptDayBookLineItem(null, Account, this.FromDate, "Opening Balance", OpBal, DaybookTransactionType.Invoice);
                }
            }
        }

        private void GetDbookClBalance()
        {
            double ClBal = 0;
            Account Account = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> lAccount = (from account in Context.Accounts.Include("AccountGroup").Include("AccountGroup.AccountGroupClassification") where (account.CompanyId == Company.CompanyId && account.Id == Company.CashOnHandAccountId) select account).ToList();
                if (lAccount.Count > 0)
                {
                    Account = lAccount.First();
                    ClBal = Account.Balance;
                    //Company.CashOnHandAccount.Balance;
                    if (CostcenterId != null)
                    {
                        ClBal -= (from daybook in Context.DoubleEntries
                                  where (daybook.CompanyId == this.Company.CompanyId && daybook.AccountId != Company.CashOnHandAccountId && daybook.CostCenterId == CostcenterId
                                  && daybook.LedgerOnly == false && daybook.Date <= this.ToDate)
                                  select daybook).Sum(x => (float?)x.Amount) ?? 0;
                    }
                    else
                    {
                        ClBal += (from daybook in Context.DoubleEntries
                                .Include("Account")
                                .Include("Account.AccountGroup")
                                .Include("Account.AccountGroup.AccountGroupClassification")
                                  where daybook.CompanyId == this.Company.CompanyId
                                  && ((daybook.AccountId == Company.CashOnHandAccountId) || (daybook.AccountId == null && daybook.TransactionType == DaybookTransactionType.Payment_Hms && daybook.PatientId != null))
                                  && daybook.LedgerOnly == false
                                  && daybook.Date <= this.ToDate
                                  select daybook).Sum(x => (float?)x.Amount) ?? 0;
                    }
                    Account.Balance = ClBal;
                    ClosingBalance = new RptDayBookLineItem(null, Account, this.FromDate, "Closing Balance", ClBal, DaybookTransactionType.Invoice);
                }
            }
        }
    }
    public class RptDayBookLineItem
    {
        public DateTime Date { get; set; }
        public Account Account { get; set; }
        public String Description { get; set; }
        public double Amount { get; set; }
        public DaybookTransactionType TransactionType { get; set; }
        public Patient Patient { get; set; }
        public RptDayBookLineItem(Patient Patient,Account account, DateTime Date, String Description, double Amount, DaybookTransactionType TransactionType)
        {
            this.Patient = Patient;
            this.Account = account;
            this.Date = Date;
            this.Description = Description;
            this.Amount = Amount;
            this.TransactionType = TransactionType;
        }
        public CrDr CreditOrDebit()
        {
            return (this.Amount *(TransactionType==DaybookTransactionType.Journal || TransactionType == DaybookTransactionType.Invoice_Hms || TransactionType == DaybookTransactionType.Payment_Hms ? 1 : Account.AccountGroup.DebitMultiplier) >= 0 ? CrDr.DR : CrDr.CR);
        }
    }
}
