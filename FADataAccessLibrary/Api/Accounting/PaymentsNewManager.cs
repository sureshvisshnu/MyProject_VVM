using fa.api.Accounting;
using fa.api.OrderManagement;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using fa.model.OrderManagement;
using Fa.api.Accounting.DoubleEntry;
using FADataAccessLibrary.Model.Accounting.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.Accounting
{
    public class PaymentsNewManager
    {
        private static volatile PaymentsNewManager instance;
        private static object syncRoot = new Object();
        public PaymentsNewManager() { }
        public static PaymentsNewManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new PaymentsNewManager();
                        }
                    }
                }
                return instance;
            }
        }
        
        public List<PaymentNew> GetPaymentsByCustomer(long customerId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                return context.PaymentsNew
                              .Where(x => x.AccountId == customerId
                                       && !x.IsDeleted)
                              .OrderByDescending(x => x.TransactionDate)
                              .ToList();
            }
        }
        public List<CustomerPaymentInvoiceDetails> GetCustomerPaymentDetails(long customerId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                var result = (from sale in context.SaleEntry
                              where sale.AccountsId == customerId
                              select new CustomerPaymentInvoiceDetails
                              {
                                  InvoiceNo = sale.RefNumber,
                                  InvoiceDate = sale.SaleDate,
                                  InvoiceAmount = (decimal)sale.TotalAmount
                              }).ToList();

                return result;
            }
        }

        public IList<PaymentNew> ListAllUnAppliedPaymentPaymentBySale(long SaleId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<PaymentNew> PaymentInfo = (from PaymentNew in Context.PaymentsNew.Include(p => p.PaymentDetails).Include(p => p.Account) where PaymentNew.SalesId == SaleId select PaymentNew).ToList();
                return PaymentInfo;
            }
        }
        public List<CustomerLedger> GetCustomerLedger(long customerId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                List<CustomerLedger> ledger = new List<CustomerLedger>();

                // Load Invoices
                var sales = context.SaleEntry
                                   .Where(x => x.AccountsId == customerId &&
                                               x.EntryType == Entrytype.SALE)
                                   .ToList();

                foreach (var sale in sales)
                {

                    ledger.Add(new CustomerLedger
                    {
                        Date = sale.SaleDate,
                        Reference = sale.RefNumber,
                        Type = "Invoice",
                        Amount = (decimal)sale.NetAmount
                    });
                }

                // Load Payments
                var payments = context.PaymentsNew
                                      .Where(x => x.AccountId == customerId &&
                                                  !x.IsDeleted)
                                      .ToList();

                foreach (var payment in payments)
                {
                    decimal balance = 0;

                    var sale = sales.FirstOrDefault(x => x.Id == payment.SalesId);

                    if (sale != null)
                    {
                        balance = (decimal)sale.Balance;
                    }

                    ledger.Add(new CustomerLedger
                    {
                        Date = payment.TransactionDate,
                        Reference = payment.Reference,
                        Type = "Payment",
                        Amount = payment.Amount
                    });
                }

                return ledger
                        .OrderBy(x => x.Date)
                        .ToList();
            }
        }

        public PaymentNew AddPayment(PaymentNew payment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddPayment(payment, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        payment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return payment;
        }

        public PaymentNew AddPayment(PaymentNew payment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (payment.PaymentDetails.Count > 0)
            {
                ApplyPayment(payment.PaymentDetails.ToList(), Context);
            }
            Context.PaymentsNew.Add(payment);
            Context.SaveChanges();
            //for sale receive payment
            if (payment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordNewPayment(payment, Context);
            }
            return payment;
        }
        public void ApplyPayment(List<PaymentDetailNew> PaymentDetail, AccountMasterContext Context)
        {
            foreach (PaymentDetailNew Details in PaymentDetail)
            {
                if (!string.IsNullOrEmpty(Details.ReferenceTransactionId))
                {
                    if (Details.InvoiceType == InvoiceType.Basic)
                    {
                        BillManager.Instance.ApplyNewPayment(Details, Context);
                    }
                    else if (Details.InvoiceType == InvoiceType.ItemBased)
                    {
                        PurchaseEntryManager.Instance.ApplyNewPayment(Details, Context);
                    }
                }
            }
        }
        public CheckPaymentNew AddCheckPayment(CheckPaymentNew checkPayment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCheckPayment(checkPayment, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        checkPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return checkPayment;
        }
        public CheckPaymentNew AddCheckPayment(CheckPaymentNew checkPayment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (checkPayment.PaymentDetails.Count > 0)
            {
                ApplyPayment(checkPayment.PaymentDetails.ToList(), Context);
            }
            Context.CheckPaymentsNew.Add(checkPayment);
            Context.SaveChanges();
            //for sale receive payment
            if (checkPayment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordNewPayment(checkPayment, Context);
            }
            return checkPayment;
        }
        public CardPaymentNew AddCreditCardPayment(CardPaymentNew creditCardPayment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCreditCardPayment(creditCardPayment, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        creditCardPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return creditCardPayment;
        }
        public CardPaymentNew AddCreditCardPayment(CardPaymentNew creditCardPayment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (creditCardPayment.PaymentDetails.Count > 0)
            {
                ApplyPayment(creditCardPayment.PaymentDetails.ToList(), Context);
            }
            Context.CardPaymentsNew.Add(creditCardPayment);
            Context.SaveChanges();
            //for sale receive payment
            if (creditCardPayment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordNewPayment(creditCardPayment, Context);
            }
            return creditCardPayment;
        }
        public BankTransferPaymentNew AddBankTransferPayment(BankTransferPaymentNew bankTransferPayment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddBankTransferPayment(bankTransferPayment, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        bankTransferPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return bankTransferPayment;
        }
        public BankTransferPaymentNew AddBankTransferPayment(BankTransferPaymentNew bankTransferPayment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (bankTransferPayment.PaymentDetails.Count > 0)
            {
                ApplyPayment(bankTransferPayment.PaymentDetails.ToList(), Context);
            }
            Context.BankTransferPaymentsNew.Add(bankTransferPayment);
            Context.SaveChanges();
            //for sale receive payment
            if (bankTransferPayment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordNewPayment(bankTransferPayment, Context);
            }
            return bankTransferPayment;
        }
        public UpiTransactionPaymentNew AddUpiTransactionPayment(UpiTransactionPaymentNew UpiTransactionPayment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddPayment(UpiTransactionPayment, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        UpiTransactionPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return UpiTransactionPayment;
        }
    }
    public class CustomerPaymentInvoiceDetails
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }

        public string PaymentRefNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
    }
    public class CustomerLedger
    {
        public DateTime Date { get; set; }

        public string Reference { get; set; }

        public string Type { get; set; }   // Invoice or Payment

        public decimal Amount { get; set; }
    }
}
    
