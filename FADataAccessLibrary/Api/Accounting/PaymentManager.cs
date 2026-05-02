using fa.api.OrderManagement;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Accounting.Transactions;
using Fa.api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public class PaymentManager
    {

        private static volatile PaymentManager instance;
        private static object syncRoot = new Object();
        PaymentManager()
        {

        }
        public static PaymentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PaymentManager();
                    }
                }

                return instance;
            }
        }
      
        public IList<Account> GetAllAccount(long CompanyId, long?[] AccountGroupId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where !AccountGroupId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }

        public void ApplyPayment(List<PaymentDetail> PaymentDetail, AccountMasterContext Context)
        {
            foreach (PaymentDetail Details in PaymentDetail)
            {
                if (!string.IsNullOrEmpty(Details.ReferenceTrasnactionId))
                {
                    if (Details.InvoiceType == InvoiceType.Basic)
                    {
                        BillManager.Instance.ApplyPayment(Details, Context);
                    }
                    else if (Details.InvoiceType == InvoiceType.ItemBased)
                    {
                        PurchaseEntryManager.Instance.ApplyPayment(Details, Context);
                    }
                }
            }
        }
        public void ReversePayment(List<PaymentDetail> PaymentDetail, AccountMasterContext Context)
        {
            foreach (PaymentDetail Details in PaymentDetail)
            {
                if (!string.IsNullOrEmpty(Details.ReferenceTrasnactionId))
                {
                    if (Details.InvoiceType == InvoiceType.Basic)
                    {
                        BillManager.Instance.ReversePayment(Details, Context);
                    }
                    else if (Details.InvoiceType == InvoiceType.ItemBased)
                    {
                        PurchaseEntryManager.Instance.ReversePayment(Details, Context);
                    }
                }
            }
        }
        public Payment AddPayment(Payment payment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddPayment(payment,Context);
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
        public UpiTransactionPayment AddUpiTransactionPayment(UpiTransactionPayment UpiTransactionPayment)
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
        public BankTransferPayment AddBankTransferPayment(BankTransferPayment bankTransferPayment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddBankTransferPayment(bankTransferPayment,Context);
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
        public CheckPayment AddCheckPayment(CheckPayment checkPayment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCheckPayment(checkPayment,Context);
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
        public CreditCardPayment AddCreditCardPayment(CreditCardPayment creditCardPayment)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCreditCardPayment(creditCardPayment,Context);
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
        public Payment AddPayment(Payment payment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (payment.PaymentDetails.Count > 0)
            {
                ApplyPayment(payment.PaymentDetails.ToList(), Context);
            }
            Context.Payments.Add(payment);
            Context.SaveChanges();
            //for sale receive payment
            if (payment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordPayment(payment, Context);
            }
            return payment;
        }
        public BankTransferPayment AddBankTransferPayment(BankTransferPayment bankTransferPayment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (bankTransferPayment.PaymentDetails.Count > 0)
            {
                ApplyPayment(bankTransferPayment.PaymentDetails.ToList(), Context);
            }
            Context.BankTransferPayments.Add(bankTransferPayment);
            Context.SaveChanges();
            //for sale receive payment
            if (bankTransferPayment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordPayment(bankTransferPayment, Context);
            }
            return bankTransferPayment;
        }
        public CheckPayment AddCheckPayment(CheckPayment checkPayment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (checkPayment.PaymentDetails.Count > 0)
            {
                ApplyPayment(checkPayment.PaymentDetails.ToList(), Context);
            }
            Context.CheckPayments.Add(checkPayment);
            Context.SaveChanges();
            //for sale receive payment
            if (checkPayment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordPayment(checkPayment, Context);
            }
            return checkPayment;
        }
        public CreditCardPayment AddCreditCardPayment(CreditCardPayment creditCardPayment, AccountMasterContext Context)
        {
            //for sale receive payment
            if (creditCardPayment.PaymentDetails.Count > 0)
            {
                ApplyPayment(creditCardPayment.PaymentDetails.ToList(), Context);
            }
            Context.CreditCardPayments.Add(creditCardPayment);
            Context.SaveChanges();
            //for sale receive payment
            if (creditCardPayment.PaymentDetails.Count > 0)
            {
                PaymentDoubleEntryManager.Instance.RecordPayment(creditCardPayment, Context);
            }
            return creditCardPayment;
        }
        public Payment UpdatePayment(Payment Payment)
        {
            Payment PaymentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PaymentInfo = Context.Payments.Find(Payment.PaymentId);
                        if (PaymentInfo != null)
                        {
                            if (PaymentInfo.TransctionType == PaymentType.CASH)
                            {
                                Payment PaymentInfoFromDB = GetPayment(Payment.PaymentId);
                                if (PaymentInfoFromDB.PaymentDetails.Count > 0)
                                {
                                    ReversePayment(PaymentInfoFromDB.PaymentDetails.ToList(), Context);
                                    dbContextTransaction.Commit();
                                    Context.PaymentDetails.Where(p => p.PaymentId == PaymentInfoFromDB.PaymentId).ToList().ForEach(p => Context.PaymentDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(PaymentInfo).CurrentValues.SetValues(Payment);

                                //for sale receive payment
                                if (Payment.PaymentDetails.Count > 0)
                                {
                                    ApplyPayment(Payment.PaymentDetails.ToList(), Context);
                                    foreach (PaymentDetail Detail in Payment.PaymentDetails)
                                    {
                                        Detail.PaymentId = Payment.PaymentId;
                                        Context.PaymentDetails.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                }
                                Context.SaveChanges();
                                if (Payment.PaymentDetails.Count > 0)
                                {
                                    PaymentDoubleEntryManager.Instance.RecordPayment(Payment, Context);
                                }
                            }
                            else
                            {
                                if (DeletePayment(PaymentInfo.PaymentId, Context))
                                {
                                    Payment.PaymentId = 0L;
                                    AddPayment(Payment, Context);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Payment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Payment;
        }
        public UpiTransactionPayment UpdateUpiTransactionPayment(UpiTransactionPayment upiTransactionPayment)
        {
            UpiTransactionPayment upiTransactionPaymentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Payment PaymentInfo = Context.Payments.Find(upiTransactionPayment.PaymentId);
                        if (PaymentInfo.TransctionType == PaymentType.UPI)
                        {
                            upiTransactionPaymentInfo = Context.UpiTransactionPayments.Find(upiTransactionPayment.PaymentId);
                            if (upiTransactionPaymentInfo != null)
                            {
                                UpiTransactionPayment UpiTransactionPaymentFromDB = GetUpiTranscationPayment(upiTransactionPayment.PaymentId);
                                if (UpiTransactionPaymentFromDB.PaymentDetails.Count > 0)
                                {
                                    ReversePayment(UpiTransactionPaymentFromDB.PaymentDetails.ToList(), Context);
                                    Context.PaymentDetails.Where(p => p.PaymentId == UpiTransactionPaymentFromDB.PaymentId).ToList().ForEach(p => Context.PaymentDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(upiTransactionPaymentInfo).CurrentValues.SetValues(upiTransactionPayment);
                                if (upiTransactionPayment.PaymentDetails.Count > 0)
                                {
                                    ApplyPayment(upiTransactionPayment.PaymentDetails.ToList(), Context);
                                    foreach (PaymentDetail Detail in upiTransactionPayment.PaymentDetails)
                                    {
                                        Detail.PaymentId = upiTransactionPayment.PaymentId;
                                        Context.PaymentDetails.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                }
                                Context.SaveChanges();
                                if (upiTransactionPayment.PaymentDetails.Count > 0)
                                {
                                    PaymentDoubleEntryManager.Instance.RecordPayment(upiTransactionPayment, Context);
                                }
                            }
                        }
                        else
                        {
                            if (DeletePayment(PaymentInfo.PaymentId, Context))
                            {
                                upiTransactionPayment.PaymentId = 0L;
                                AddUpiTransactionPayment(upiTransactionPayment);
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        upiTransactionPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return upiTransactionPayment;
        }
        public BankTransferPayment UpdateBankTransferPayment(BankTransferPayment BankTransferPayment)
        {
            BankTransferPayment BankTransferPaymentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Payment PaymentInfo = Context.Payments.Find(BankTransferPayment.PaymentId);
                        if (PaymentInfo.TransctionType == PaymentType.BANKTRANSFER)
                        {
                            BankTransferPaymentInfo = Context.BankTransferPayments.Find(BankTransferPayment.PaymentId);
                            if (BankTransferPaymentInfo != null)
                            {
                                BankTransferPayment BankTransferPaymentInfoFromDB = GetBankTransferPayment(BankTransferPayment.PaymentId);
                                if (BankTransferPaymentInfoFromDB.PaymentDetails.Count > 0)
                                {
                                    ReversePayment(BankTransferPaymentInfoFromDB.PaymentDetails.ToList(), Context);
                                    Context.PaymentDetails.Where(p => p.PaymentId == BankTransferPaymentInfoFromDB.PaymentId).ToList().ForEach(p => Context.PaymentDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(BankTransferPaymentInfo).CurrentValues.SetValues(BankTransferPayment);
                                if (BankTransferPayment.PaymentDetails.Count > 0)
                                {
                                    ApplyPayment(BankTransferPayment.PaymentDetails.ToList(), Context);
                                    foreach (PaymentDetail Detail in BankTransferPayment.PaymentDetails)
                                    {
                                        Detail.PaymentId = BankTransferPayment.PaymentId;
                                        Context.PaymentDetails.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                }
                                Context.SaveChanges();
                                if (BankTransferPayment.PaymentDetails.Count > 0)
                                {
                                    PaymentDoubleEntryManager.Instance.RecordPayment(BankTransferPayment, Context);
                                }
                            }
                        }
                        else
                        {
                            if (DeletePayment(PaymentInfo.PaymentId, Context))
                            {
                                BankTransferPayment.PaymentId = 0L;
                                AddBankTransferPayment(BankTransferPayment, Context);
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        BankTransferPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return BankTransferPayment;
        }
        public CheckPayment UpdateCheckPayment(CheckPayment CheckPayment)
        {
            CheckPayment CheckPaymentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Payment PaymentInfo = Context.Payments.Find(CheckPayment.PaymentId);
                        if (PaymentInfo.TransctionType == PaymentType.CHECK)
                        {
                            CheckPaymentInfo = Context.CheckPayments.Find(CheckPayment.PaymentId);
                            if (CheckPaymentInfo != null)
                            {
                                CheckPayment CheckPaymentInfoFromDB = GetCheckPayment(CheckPayment.PaymentId);
                                if (CheckPaymentInfoFromDB.PaymentDetails.Count > 0)
                                {
                                    ReversePayment(CheckPaymentInfoFromDB.PaymentDetails.ToList(), Context);
                                    Context.PaymentDetails.Where(p => p.PaymentId == CheckPaymentInfoFromDB.PaymentId).ToList().ForEach(p => Context.PaymentDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(CheckPaymentInfo).CurrentValues.SetValues(CheckPayment);
                                if (CheckPayment.PaymentDetails.Count > 0)
                                {
                                    ApplyPayment(CheckPayment.PaymentDetails.ToList(), Context);
                                    foreach (PaymentDetail Detail in CheckPayment.PaymentDetails)
                                    {
                                        Detail.PaymentId = CheckPayment.PaymentId;
                                        Context.PaymentDetails.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                }
                                Context.SaveChanges();
                                if (CheckPayment.PaymentDetails.Count > 0)
                                {
                                    PaymentDoubleEntryManager.Instance.RecordPayment(CheckPayment, Context);
                                }
                            }
                        }
                        else
                        {
                            if (DeletePayment(PaymentInfo.PaymentId, Context))
                            {
                                CheckPayment.PaymentId = 0L;
                                AddCheckPayment(CheckPayment, Context);
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CheckPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CheckPayment;
        }
        public CreditCardPayment UpdateCreditCardPayment(CreditCardPayment CreditCardPayment)
        {
            CreditCardPayment CreditCardPaymentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Payment PaymentInfo = Context.Payments.Find(CreditCardPayment.PaymentId);
                        if (PaymentInfo.TransctionType == PaymentType.CREDITCARD)
                        {
                            CreditCardPaymentInfo = Context.CreditCardPayments.Find(CreditCardPayment.PaymentId);
                            if (CreditCardPaymentInfo != null)
                            {
                                CreditCardPayment CreditCardPaymentInfoFromDB = GetCreditCardPayment(CreditCardPayment.PaymentId);
                                if (CreditCardPaymentInfoFromDB.PaymentDetails.Count > 0)
                                {
                                    ReversePayment(CreditCardPaymentInfoFromDB.PaymentDetails.ToList(), Context);
                                    Context.PaymentDetails.Where(p => p.PaymentId == CreditCardPaymentInfoFromDB.PaymentId).ToList().ForEach(p => Context.PaymentDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(CreditCardPaymentInfo).CurrentValues.SetValues(CreditCardPayment);
                                if (CreditCardPayment.PaymentDetails.Count > 0)
                                {
                                    ApplyPayment(CreditCardPayment.PaymentDetails.ToList(), Context);
                                    foreach (PaymentDetail Detail in CreditCardPayment.PaymentDetails)
                                    {
                                        Detail.PaymentId = CreditCardPayment.PaymentId;
                                        Context.PaymentDetails.Add(Detail);
                                        Context.SaveChanges();
                                    }
                                }
                                Context.SaveChanges();
                                if (CreditCardPayment.PaymentDetails.Count > 0)
                                {
                                    PaymentDoubleEntryManager.Instance.RecordPayment(CreditCardPayment, Context);
                                }
                            }
                        }
                        else
                        {
                            if (DeletePayment(PaymentInfo.PaymentId, Context))
                            {
                                CreditCardPayment.PaymentId = 0L;
                                AddCreditCardPayment(CreditCardPayment, Context);
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CreditCardPayment = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CreditCardPayment;
        }
        public Boolean DeletePayment(long PaymentId, AccountMasterContext Context)
        {
            Boolean Deleted = false;
            Payment PaymentInfo = null;
            try
            {
                PaymentInfo = Context.Payments.Include("PaymentDetails").FirstOrDefault(x => x.PaymentId == PaymentId);
                if (PaymentInfo.PaymentDetails.Count > 0)
                {
                    ReversePayment(PaymentInfo.PaymentDetails.ToList(), Context);
                    PaymentDetail PaymentDetail = Context.PaymentDetails.FirstOrDefault(x => x.PaymentId == PaymentInfo.PaymentId);
                    if (PaymentDetail != null)
                    {
                        Context.PaymentDetails.Where(p => p.PaymentId == PaymentInfo.PaymentId).ToList().ForEach(p => Context.PaymentDetails.Remove(p));
                    }
                }
                PaymentInfo = Context.Payments.Find(PaymentId);
                Context.Payments.Remove(PaymentInfo);
                Context.SaveChanges();
                PaymentDoubleEntryManager.Instance.DeletePayments(PaymentInfo, Context);
                Deleted = true;
            }
            #pragma warning disable 0168
            catch (Exception e)
            {
                Deleted = false;
            }
            #pragma warning restore 0168
            return Deleted;
        }
        public Boolean DeletePayment(long PaymentId)
        {
            Boolean Deleted = false;
            Payment PaymentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PaymentInfo = Context.Payments.Include("PaymentDetails").FirstOrDefault(x => x.PaymentId == PaymentId);
                        if (PaymentInfo.PaymentDetails.Count > 0)
                        {
                            ReversePayment(PaymentInfo.PaymentDetails.ToList(), Context);
                            PaymentDetail PaymentDetail = Context.PaymentDetails.FirstOrDefault(x => x.PaymentId == PaymentInfo.PaymentId);
                            if (PaymentDetail != null)
                            {
                                Context.PaymentDetails.Where(p => p.PaymentId == PaymentInfo.PaymentId).ToList().ForEach(p => Context.PaymentDetails.Remove(p));
                            }
                        }
                        PaymentInfo = Context.Payments.Find(PaymentId);
                        Context.Payments.Remove(PaymentInfo);
                        Context.SaveChanges();
                        PaymentDoubleEntryManager.Instance.DeletePayments(PaymentInfo, Context);
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    #pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }
            return Deleted;
        }
        public IList<Payment> ListAllUnAppliedPaymentPaymentByAccount(long AccountId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Payment> PaymentInfo = (from Payment in Context.Payments.Include("PaymentDetails").Include("Account") /*where Context.PaymentDetails.Any(x => x.PaymentId == Payment.PaymentId && x.ReferenceTrasnactionId == null)*/ where Payment.AccountId == AccountId select Payment).OrderByDescending(x => x.TransactionDate).ToList();
                return PaymentInfo.Count==0? PaymentInfo : PaymentInfo.Where(x=>x.PaymentDetails.Any(y=>y.ReferenceTrasnactionId==null)).ToList();
            }
        }
        public IList<Payment> ListAllUnAppliedPaymentPaymentBySale(long SaleId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Payment> PaymentInfo = (from Payment in Context.Payments.Include(p => p.PaymentDetails).Include(p => p.Account) where Payment.SalesId == SaleId select Payment).ToList();
                return PaymentInfo;
            }
        }
        public Payment GetPayment(long PaymentId)
        {
            Payment Payment = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Payment = Context.Payments.Include(p => p.PaymentDetails).Include(p => p.Account).FirstOrDefault(x=>x.PaymentId==PaymentId);                
            }
            return Payment;
        }
        public PaymentDetail GetPaymentDetail(long PaymentDetailId)
        {
            PaymentDetail PaymentDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentDetail = Context.PaymentDetails.FirstOrDefault(x => x.PaymentDetailId == PaymentDetailId);
            }
            return PaymentDetail;
        }
        public BankTransferPayment GetBankTransferPayment(long PaymentId)
        {
            BankTransferPayment BankTransferPayment = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BankTransferPayment = Context.BankTransferPayments.Include("PaymentDetails").Include("Account").Include("BankTransfer").FirstOrDefault(x => x.PaymentId == PaymentId);
            }
            return BankTransferPayment;
        }
        public CheckPayment GetCheckPayment(long PaymentId)
        {
            CheckPayment CheckPayment = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CheckPayment = Context.CheckPayments.Include("PaymentDetails").Include("Account").Include("BankAccount").FirstOrDefault(x => x.PaymentId == PaymentId);
            }
            return CheckPayment;
        }
        public UpiTransactionPayment GetUpiTranscationPayment(long PaymentId)
        {
            UpiTransactionPayment UpiTransactionPayment = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                UpiTransactionPayment = Context.UpiTransactionPayments.Include("PaymentDetails").Include("Account").Include("UpiAccount").FirstOrDefault(x => x.PaymentId == PaymentId);
            }
            return UpiTransactionPayment;
        }
        public CreditCardPayment GetCreditCardPayment(long PaymentId)
        {
            CreditCardPayment CreditCardPayment = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CreditCardPayment = Context.CreditCardPayments.Include("PaymentDetails").Include("CCAccount").FirstOrDefault(x => x.PaymentId == PaymentId);
            }
            return CreditCardPayment;
        }


        public IList<Payment> GetPaymentByDate(DateTime Date, long CompanyId)
        {
            IList<Payment> DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNoteInfo = Context.Payments.Include("PaymentDetails").Include("Account").Where(x => x.TransactionDate.Day == Date.Day && x.TransactionDate.Month == Date.Month && x.TransactionDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Payment>();
                return DebitNoteInfo;
            }
        }

        public IList<Payment> GetPaymentByAmount(double Amount, string RefNo, long CompanyId)
        {
            IList<Payment> DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNoteInfo = Context.Payments.Include("PaymentDetails").Include("Account").Where(x => (x.Amount.Equals(Amount) || x.Reference.Contains(RefNo)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Payment>();
                return DebitNoteInfo;
            }
        }
        public IList<Payment> GetPaymentBySupplierName(string SupplierName, long CompanyId)
        {
            IList<Payment> DebitNoteInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DebitNoteInfo = Context.Payments.Include("PaymentDetails").Include("Account").Where(x => (x.Account.Name.Contains(SupplierName) || x.Reference.Contains(SupplierName)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Payment>();
                return DebitNoteInfo;
            }
        }
    }
}
