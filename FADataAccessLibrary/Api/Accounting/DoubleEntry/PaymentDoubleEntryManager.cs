using fa.api.Accounting;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using FADataAccessLibrary.Model.Accounting.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Fa.api.Accounting.DoubleEntry
{
    public class PaymentDoubleEntryManager
    {
        static readonly string SupplierEntryForPayment = "Payment {0} {1}";
        static readonly string CurrentCompanyNullMsg = "Missing company details, Please contact your administrator";
        static readonly string InvalidCompanyConfigMsg = "Incorrect company configuration, please contact administrator";
        private static volatile PaymentDoubleEntryManager instance;
        private static object syncRoot = new Object();
        PaymentDoubleEntryManager()
        {
        }
        public static PaymentDoubleEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PaymentDoubleEntryManager();
                    }
                }
                return instance;
            }
        }
        public void RecordPayment(Payment Payment, AccountMasterContext Context)
        {
            DeletePayments(Payment, Context);
            RecordPayments(Payment, Context);
        }

        public void RecordNewPayment(PaymentNew Payment, AccountMasterContext Context)
        {
            DeleteNewPayments(Payment, Context);
            RecordNewPayments(Payment, Context);
        }

        public void DeleteNewPayments(PaymentNew Payment, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Payment && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Payment.Reference).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordNewPayments(PaymentNew Payment, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(GetFromSupplierEntryForNewPayment(Payment));
            Context.DoubleEntries.AddRange(GetToAccountEntryForNewPayment(Payment));
            Context.SaveChanges();
        }

        //Deleteing all exisiting entries for the given company      
        public void DeletePayments(Payment Payment, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            List<DayBook> xx = Context.DoubleEntries.Where(x => x.TransactionType == DaybookTransactionType.Payment && x.CompanyId == CurrentCompany.CompanyId && x.ReferenceTrasnactionId == "" + Payment.Reference).ToList();
            Context.DoubleEntries.RemoveRange(xx);
            Context.SaveChanges();
        }
        public void RecordPayments(Payment Payment, AccountMasterContext Context)
        {
            Company CurrentCompany = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (CurrentCompany == null)
            {
                Exception e = new Exception(CurrentCompanyNullMsg);
                throw e;
            }
            Context.DoubleEntries.Add(getFromSupplierEntryForPayment(Payment));
            Context.DoubleEntries.AddRange(getToAccountEntryForPayment(Payment));
            Context.SaveChanges();
        }
        public DayBook getFromSupplierEntryForPayment(Payment Payment)
        {
            DayBook dBook = new DayBook();
            dBook.AccountId = Payment.AccountId;
            dBook.CompanyId = Payment.CompanyId;
            dBook.CostCenterId = Payment.CostCenterId;
            dBook.Date = Payment.TransactionDate;
            dBook.Debit((double)Payment.Amount);
            dBook.Description = string.Format(SupplierEntryForPayment, Payment.Reference,Payment.Description);
            dBook.ReferenceTrasnactionId = Payment.Reference;
            dBook.TransactionType = DaybookTransactionType.Payment;
            return dBook;
        }
        public DayBook GetFromSupplierEntryForNewPayment(PaymentNew Payment)
        {
            DayBook dBook = new DayBook();
            dBook.AccountId = Payment.AccountId;
            dBook.CompanyId = Payment.CompanyId;
            dBook.CostCenterId = Payment.CostCenterId;
            dBook.Date = Payment.TransactionDate;
            dBook.Debit((double)Payment.Amount);
            dBook.Description = string.Format(SupplierEntryForPayment, Payment.Reference, Payment.Description);
            dBook.ReferenceTrasnactionId = Payment.Reference;
            dBook.TransactionType = DaybookTransactionType.Payment;
            return dBook;
        }
        static readonly string PaymentCashEntryCaption = "Trasnsfer Id:{0}";
        static readonly string PaymentCheckEntryCaption = "Cheque/DD:{0} Dated:{1} Trasnsfer Id:{2}";
        static readonly string PaymentBankEntryCaption = "Bank:{0} Dated:{1} Trasnsfer Id:{2}";
        static readonly string PaymentCardEntryCaption = "CreditCard:{0} Dated:{1} Trasnsfer Id:{2}";
        public List<DayBook> getToAccountEntryForPayment(Payment Payment)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (Company.CashOnHandAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account PaymentAccount = AccountManager.Instance.GetAccountById((long)Payment.AccountId);
            if (PaymentAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account lAccount = null;
            string Desc = string.Empty;
            if (Payment.TransctionType == PaymentType.CASH)
            {
            lAccount = Company.CashOnHandAccount;
            Desc = string.Format(PaymentCashEntryCaption, lAccount.DisplayAs);
            }
            else if (Payment.TransctionType == PaymentType.CHECK)
            {
                CheckPayment CheckPayment = (CheckPayment)Payment;
                lAccount = AccountManager.Instance.GetAccountById((long)CheckPayment.DepositedIntoId);
                Desc = string.Format(PaymentCheckEntryCaption, CheckPayment.DocumentNumber, CheckPayment.DocumentDate, lAccount.DisplayAs);
            }
            else if (Payment.TransctionType == PaymentType.BANKTRANSFER)
            {
                BankTransferPayment BankTransferPayment = ((BankTransferPayment)Payment);
                lAccount = AccountManager.Instance.GetAccountById((long)BankTransferPayment.BankTransferId);
                Desc = string.Format(PaymentBankEntryCaption, BankTransferPayment.TransactionNumber, BankTransferPayment.TransactionDate, lAccount.DisplayAs);
            }
            else if (Payment.TransctionType == PaymentType.CREDITCARD)
            {
                CreditCardPayment CreditCardPayment = ((CreditCardPayment)Payment);
                lAccount = AccountManager.Instance.GetAccountById((long)CreditCardPayment.CCAccountId);
                Desc = string.Format(PaymentCardEntryCaption, CreditCardPayment.CCTransactionNumber, CreditCardPayment.CCTransactionDate, lAccount.DisplayAs);
            }
            foreach (PaymentDetail Detail in Payment.PaymentDetails)
            {
                DayBook dBook = new DayBook();
                dBook.AccountId = lAccount.Id;
                dBook.CompanyId = Payment.CompanyId;
                dBook.CostCenterId = Payment.CostCenterId;
                dBook.Date = Payment.TransactionDate;
                dBook.Credit((double)Detail.Amount);
                dBook.Description = string.Format(SupplierEntryForPayment, Payment.Reference + (string.IsNullOrEmpty(Detail.Description) ? "" : " " + Detail.Description), Desc);
                dBook.ReferenceTrasnactionId = Payment.Reference;
                dBook.TransactionType = DaybookTransactionType.Payment;
                dayBooks.Add(dBook);
            }
            return dayBooks;
        }
        public List<DayBook> GetToAccountEntryForNewPayment(PaymentNew Payment)
        {
            List<DayBook> dayBooks = new List<DayBook>();
            Company Company = CompanyManager.Instance.GetCompany(Payment.CompanyId);
            if (Company.CashOnHandAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account PaymentAccount = AccountManager.Instance.GetAccountById((long)Payment.AccountId);
            if (PaymentAccount == null)
            {
                Exception e = new Exception(InvalidCompanyConfigMsg);
                throw e;
            }
            Account lAccount = null;
            string Desc = string.Empty;
            if (Payment.TransactionType == PaymentType.CASH)
            {
                lAccount = Company.CashOnHandAccount;
                Desc = string.Format(PaymentCashEntryCaption, lAccount.DisplayAs);
            }
            else if (Payment.TransactionType== PaymentType.CHECK)
            {
                CheckPaymentNew CheckPayment = (CheckPaymentNew)Payment;
                lAccount = AccountManager.Instance.GetAccountById((long)CheckPayment.DepositedIntoId);
                Desc = string.Format(PaymentCheckEntryCaption, CheckPayment.DocumentNumber, CheckPayment.DocumentDate, lAccount.DisplayAs);
            }
            else if (Payment.TransactionType == PaymentType.BANKTRANSFER)
            {
                BankTransferPaymentNew BankTransferPayment = ((BankTransferPaymentNew)Payment);
                lAccount = AccountManager.Instance.GetAccountById((long)BankTransferPayment.BankTransferId);
                Desc = string.Format(PaymentBankEntryCaption, BankTransferPayment.TransactionNumber, BankTransferPayment.TransactionDate, lAccount.DisplayAs);
            }
            else if (Payment.TransactionType == PaymentType.CREDITCARD)
            {
                CardPaymentNew CreditCardPayment = ((CardPaymentNew)Payment);
                lAccount = AccountManager.Instance.GetAccountById((long)CreditCardPayment.CAccountId);
                Desc = string.Format(PaymentCardEntryCaption, CreditCardPayment.CTransactionNumber, CreditCardPayment.CCTransactionDate, lAccount.DisplayAs);
            }
            foreach (PaymentDetailNew Detail in Payment.PaymentDetails)
            {
                DayBook dBook = new DayBook();
                dBook.AccountId = lAccount.Id;
                dBook.CompanyId = Payment.CompanyId;
                dBook.CostCenterId = Payment.CostCenterId;
                dBook.Date = Payment.TransactionDate;
                dBook.Credit((double)Detail.Amount);
                dBook.Description = string.Format(SupplierEntryForPayment, Payment.Reference + (string.IsNullOrEmpty(Detail.Description) ? "" : " " + Detail.Description), Desc);
                dBook.ReferenceTrasnactionId = Payment.Reference;
                dBook.TransactionType = DaybookTransactionType.Payment;
                dayBooks.Add(dBook);
            }
            return dayBooks;
        }
    }
}
