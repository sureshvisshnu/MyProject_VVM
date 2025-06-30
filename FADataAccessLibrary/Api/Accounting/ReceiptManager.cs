using fa.model.Accounting.Masters;
using fa.context;
using fa.model.Accounting.Transactions;
using fa.api.OrderManagement;
using Fa.api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;
using fa.model.OrderManagement;

namespace fa.api.Accounting
{
    public class ReceiptManager
    {
        private static volatile ReceiptManager instance;
        private static object syncRoot = new Object();
        ReceiptManager()
        {

        }
        public List<SaleEntry> salesEntry { get; set; } = new List<SaleEntry>();
        public static ReceiptManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ReceiptManager();
                    }
                }

                return instance;
            }
        }
        public IList<Account> GetAllAccount(long CompanyId,long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where !AccountTypeId.Contains(Account.AccountGroupId) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }

        public IList<Account> ListAllBankTransferAccount(long CompanyId,long?[] AccountTypeId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where AccountTypeId.Contains(Account.AccountGroupId) where Account.CompanyId== CompanyId select Account).ToList();
                return AccountInfo;
            }
        }

        public IList<Account> ListAllCreditDebitAccount(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Account> AccountInfo = (from Account in Context.Accounts where (Account.AccountGroupId == 58) where Account.CompanyId == CompanyId select Account).ToList();
                return AccountInfo;
            }
        }

        public void ApplyPayment(List<ReceiptDetail> ReceiptDetail, AccountMasterContext Context, List<SaleEntry> saleEntry)
        {
            foreach (ReceiptDetail Details in ReceiptDetail)
            {
                if (!string.IsNullOrEmpty(Details.ReferenceTrasnactionId))
                {
                    if(Details.InvoiceType==InvoiceType.Basic)
                    {
                        InvoiceManager.Instance.ApplyPayment(Details, Context);
                    }
                    else if(Details.InvoiceType == InvoiceType.ItemBased)
                    {
                        SalesManager.Instance.ApplyPayment(Details, Context, saleEntry);
                    }
                }
            }
        }
        public void ReversePayment(List<ReceiptDetail> ReceiptDetail, AccountMasterContext Context)
        {
            foreach (ReceiptDetail Details in ReceiptDetail)
            {
                if (!string.IsNullOrEmpty(Details.ReferenceTrasnactionId))
                {
                    if (Details.InvoiceType == InvoiceType.Basic)
                    {
                        InvoiceManager.Instance.ReversePayment(Details, Context);
                    }
                    else if (Details.InvoiceType == InvoiceType.ItemBased)
                    {
                        SalesManager.Instance.ReversePayment(Details, Context);
                    }
                }
            }
        }
        public Receipt AddReceipt(Receipt receipt)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddReceipt(receipt,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        receipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return receipt;
        }
        public BankTransferReceipt AddBankTransferReceipt(BankTransferReceipt bankTransferReceipt)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddBankTransferReceipt(bankTransferReceipt,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        bankTransferReceipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return bankTransferReceipt;
        }
        public CheckReceipt AddCheckReceipt(CheckReceipt checkReceipt)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCheckReceipt(checkReceipt,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        checkReceipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return checkReceipt;
        }
        public CreditCardReceipt AddCreditCardReceipt(CreditCardReceipt creditCardReceipt)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AddCreditCardReceipt(creditCardReceipt,Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        creditCardReceipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return creditCardReceipt;
        }
        public Receipt AddReceipt(Receipt receipt, AccountMasterContext Context)
        {
            ApplyPayment(receipt.ReceiptDetails.ToList(), Context, null);
            Context.SaveChanges();
            Context.Receipts.Add(receipt);
            Context.SaveChanges();
            ReceiptDoubleEntryManager.Instance.RecordReceipt(receipt, Context);
            return receipt;
        }
        public BankTransferReceipt AddBankTransferReceipt(BankTransferReceipt bankTransferReceipt, AccountMasterContext Context)
        {           
            ApplyPayment(bankTransferReceipt.ReceiptDetails.ToList(), Context, null);
            Context.BankTransferReceipts.Add(bankTransferReceipt);
            Context.SaveChanges();
            ReceiptDoubleEntryManager.Instance.RecordReceipt(bankTransferReceipt, Context);                     
            return bankTransferReceipt;
        }
        public CheckReceipt AddCheckReceipt(CheckReceipt checkReceipt, AccountMasterContext Context)
        {
            ApplyPayment(checkReceipt.ReceiptDetails.ToList(), Context, null);
            Context.Entry(checkReceipt.DepositedInto).State = EntityState.Unchanged;
            Context.CheckReceipts.Add(checkReceipt);
            Context.SaveChanges();
            ReceiptDoubleEntryManager.Instance.RecordReceipt(checkReceipt, Context);
            return checkReceipt;
        }
        public CreditCardReceipt AddCreditCardReceipt(CreditCardReceipt creditCardReceipt, AccountMasterContext Context)
        {
            ApplyPayment(creditCardReceipt.ReceiptDetails.ToList(), Context, null);
            Context.CreditCardReceipts.Add(creditCardReceipt);
            Context.SaveChanges();
            ReceiptDoubleEntryManager.Instance.RecordReceipt(creditCardReceipt, Context);
            return creditCardReceipt;
        }
        public Receipt UpdateReceipt(Receipt Receipt)
        {
            Receipt ReceiptInfo = null;
            salesEntry.Clear();
            using (AccountMasterContext Context = new AccountMasterContext())
                {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ReceiptInfo = Context.Receipts.Find(Receipt.ReceiptId);
                        if (ReceiptInfo != null)
                        {
                            if (ReceiptInfo.TransactionType == PaymentType.CASH)
                            {
                                Receipt ReceiptInfoFromDB = GetReceipt(Receipt.ReceiptId);
                                if (ReceiptInfoFromDB.ReceiptDetails.Count > 0)
                                {
                                    ReversePayment(ReceiptInfoFromDB.ReceiptDetails.ToList(), Context);
                                    Context.ReceiptDetails.Where(p => p.ReceiptId == ReceiptInfoFromDB.ReceiptId).ToList().ForEach(p => Context.ReceiptDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(ReceiptInfo).CurrentValues.SetValues(Receipt);
                                ApplyPayment(Receipt.ReceiptDetails.ToList(), Context, salesEntry);
                                foreach (ReceiptDetail Detail in Receipt.ReceiptDetails)
                                {
                                    Detail.ReceiptId = Receipt.ReceiptId;
                                    Context.ReceiptDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ReceiptDoubleEntryManager.Instance.RecordReceipt(Receipt, Context);
                            }
                            else
                            {
                                DeleteReceipt(ReceiptInfo.ReceiptId, Context);
                                Receipt.ReceiptId = 0L;
                                AddReceipt(Receipt,Context);
                            }

                        }
                        dbContextTransaction.Commit();
                        }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Receipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Receipt;
        }
        public BankTransferReceipt UpdateBankTransferReceipt(BankTransferReceipt BankTransferReceipt)
        {
            BankTransferReceipt BankTransferReceiptInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Receipt ReceiptInfo = Context.Receipts.Find(BankTransferReceipt.ReceiptId);
                        if (ReceiptInfo.TransactionType == PaymentType.BANKTRANSFER)
                        {
                            BankTransferReceiptInfo = Context.BankTransferReceipts.Find(BankTransferReceipt.ReceiptId);
                            if (BankTransferReceiptInfo != null)
                            {

                                BankTransferReceipt ReceiptInfoFromDB = GetBankTransferReceipt(BankTransferReceipt.ReceiptId);
                                if (ReceiptInfoFromDB.ReceiptDetails.Count > 0)
                                {
                                    ReversePayment(ReceiptInfoFromDB.ReceiptDetails.ToList(), Context);
                                    Context.ReceiptDetails.Where(p => p.ReceiptId == ReceiptInfoFromDB.ReceiptId).ToList().ForEach(p => Context.ReceiptDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(BankTransferReceiptInfo).CurrentValues.SetValues(BankTransferReceipt);
                                ApplyPayment(BankTransferReceipt.ReceiptDetails.ToList(), Context, null);
                                foreach (ReceiptDetail Detail in BankTransferReceipt.ReceiptDetails)
                                {
                                    Detail.ReceiptId = BankTransferReceipt.ReceiptId;
                                    Context.ReceiptDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ReceiptDoubleEntryManager.Instance.RecordReceipt(BankTransferReceipt, Context);
                            }

                        }
                        else
                        {
                            DeleteReceipt(ReceiptInfo.ReceiptId, Context);
                            BankTransferReceipt.ReceiptId = 0L;
                            AddBankTransferReceipt(BankTransferReceipt, Context);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        BankTransferReceipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return BankTransferReceipt;
        }
        public CheckReceipt UpdateCheckReceipt(CheckReceipt CheckReceipt)
        {
            CheckReceipt CheckReceiptInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Receipt ReceiptInfo = Context.Receipts.Find(CheckReceipt.ReceiptId);
                        if (ReceiptInfo.TransactionType == PaymentType.CHECK)
                        {
                            CheckReceiptInfo = Context.CheckReceipts.Find(CheckReceipt.ReceiptId);
                            if (CheckReceiptInfo != null)
                            {
                                CheckReceipt ReceiptInfoFromDB = GetCheckReceipt(CheckReceipt.ReceiptId);
                                if (ReceiptInfoFromDB.ReceiptDetails.Count > 0)
                                {
                                    ReversePayment(ReceiptInfoFromDB.ReceiptDetails.ToList(), Context);
                                    Context.ReceiptDetails.Where(p => p.ReceiptId == ReceiptInfoFromDB.ReceiptId).ToList().ForEach(p => Context.ReceiptDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(CheckReceiptInfo).CurrentValues.SetValues(CheckReceipt);
                                ApplyPayment(CheckReceipt.ReceiptDetails.ToList(), Context, null);
                                foreach (ReceiptDetail Detail in CheckReceipt.ReceiptDetails)
                                {
                                    Detail.ReceiptId = CheckReceipt.ReceiptId;
                                    Context.ReceiptDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ReceiptDoubleEntryManager.Instance.RecordReceipt(CheckReceipt, Context);
                            }
                        }
                        else
                        {
                            DeleteReceipt(ReceiptInfo.ReceiptId, Context);
                            CheckReceipt.ReceiptId = 0L;
                            AddCheckReceipt(CheckReceipt, Context);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CheckReceipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CheckReceipt;
        }
        public CreditCardReceipt UpdateCreditCardReceipt(CreditCardReceipt CreditCardReceipt)
        {
            CreditCardReceipt CreditCardReceiptInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Receipt ReceiptInfo = Context.Receipts.Find(CreditCardReceipt.ReceiptId);
                        if (ReceiptInfo.TransactionType == PaymentType.CREDITCARD)
                        {
                            CreditCardReceiptInfo = Context.CreditCardReceipts.Find(CreditCardReceipt.ReceiptId);
                            if (CreditCardReceiptInfo != null)
                            {
                                CreditCardReceipt ReceiptInfoFromDB = GetCreditCardReceipt(CreditCardReceipt.ReceiptId);
                                if (ReceiptInfoFromDB.ReceiptDetails.Count > 0)
                                {
                                    ReversePayment(ReceiptInfoFromDB.ReceiptDetails.ToList(), Context);
                                    Context.ReceiptDetails.Where(p => p.ReceiptId == ReceiptInfoFromDB.ReceiptId).ToList().ForEach(p => Context.ReceiptDetails.Remove(p));
                                    Context.SaveChanges();
                                }
                                Context.Entry(CreditCardReceiptInfo).CurrentValues.SetValues(CreditCardReceipt);
                                ApplyPayment(CreditCardReceipt.ReceiptDetails.ToList(), Context, null);
                                foreach (ReceiptDetail Detail in CreditCardReceipt.ReceiptDetails)
                                {
                                    Detail.ReceiptId = CreditCardReceipt.ReceiptId;
                                    Context.ReceiptDetails.Add(Detail);
                                    Context.SaveChanges();
                                }
                                Context.SaveChanges();
                                ReceiptDoubleEntryManager.Instance.RecordReceipt(CreditCardReceipt, Context);
                            }
                        }
                        else
                        {
                            DeleteReceipt(ReceiptInfo.ReceiptId, Context);
                            CreditCardReceipt.ReceiptId = 0L;
                            AddCreditCardReceipt(CreditCardReceipt, Context);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CreditCardReceipt = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CreditCardReceipt;
        }

        public Boolean DeleteReceipt(long ReceiptId)
        {
            Boolean Deleted = false;
            Receipt ReceiptInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ReceiptInfo = Context.Receipts.Include("ReceiptDetails").FirstOrDefault(x => x.ReceiptId == ReceiptId);
                        if (ReceiptInfo.ReceiptDetails.Count > 0)
                        {
                            ReversePayment(ReceiptInfo.ReceiptDetails.ToList(), Context);
                            ReceiptDetail ReceiptDetail = Context.ReceiptDetails.FirstOrDefault(x => x.ReceiptId == ReceiptInfo.ReceiptId);
                            if (ReceiptDetail != null)
                            {
                                Context.ReceiptDetails.Where(p => p.ReceiptId == ReceiptInfo.ReceiptId).ToList().ForEach(p => Context.ReceiptDetails.Remove(p));
                            }
                        }
                        ReceiptInfo = Context.Receipts.Find(ReceiptId);
                        Context.Receipts.Remove(ReceiptInfo);
                        Context.SaveChanges();
                        ReceiptDoubleEntryManager.Instance.DeleteReceipts(ReceiptInfo, Context);
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    #pragma warning disable 0168 // variable declared but not used.
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
        public Boolean DeleteReceipt(long ReceiptId, AccountMasterContext Context)
        {
            Boolean Deleted = false;
            Receipt ReceiptInfo = null;
            try
            {
                ReceiptInfo = Context.Receipts.Include("ReceiptDetails").FirstOrDefault(x => x.ReceiptId == ReceiptId);
                if (ReceiptInfo.ReceiptDetails.Count > 0)
                {
                    ReversePayment(ReceiptInfo.ReceiptDetails.ToList(), Context);
                    ReceiptDetail ReceiptDetail = Context.ReceiptDetails.FirstOrDefault(x => x.ReceiptId == ReceiptInfo.ReceiptId);
                    if (ReceiptDetail != null)
                    {
                        Context.ReceiptDetails.Where(p => p.ReceiptId == ReceiptInfo.ReceiptId).ToList().ForEach(p => Context.ReceiptDetails.Remove(p));
                    }
                }
                ReceiptInfo = Context.Receipts.Find(ReceiptId);
                Context.Receipts.Remove(ReceiptInfo);
                Context.SaveChanges();
                ReceiptDoubleEntryManager.Instance.DeleteReceipts(ReceiptInfo, Context);
                Deleted = true;
            }
            catch (Exception e)
            {
                Deleted = false;
            }                
            return Deleted;
        }
        // changes there
        public IList<Receipt> ListAllUnAppliedPaymentReceipt(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Receipt> ReceiptInfo = (from Receipt in Context.Receipts.Include("ReceiptDetails").Include("Account") where Context.ReceiptDetails.Any(x => x.ReceiptId == Receipt.ReceiptId && x.ReferenceTrasnactionId == null) where Receipt.CompanyId==CompanyId select Receipt).OrderByDescending(x => x.TransactionDate).ToList();
                return ReceiptInfo;
            }
        }
        public IList<Receipt> ListAllUnAppliedPaymentReceiptByAccount(long AccountId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Receipt> ReceiptInfo = (from Receipt in Context.Receipts.Include("ReceiptDetails").Include("Account") where Context.ReceiptDetails.Any(x => x.ReceiptId == Receipt.ReceiptId && x.ReferenceTrasnactionId == null) where Receipt.AccountId==AccountId select Receipt).OrderByDescending(x => x.TransactionDate).ToList();
                return ReceiptInfo;
            }
        }
        public Receipt GetReceipt(long ReceiptId)
        {
            Receipt Receipt = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Receipt = Context.Receipts.Include("ReceiptDetails").Include("Account").FirstOrDefault(x=>x.ReceiptId==ReceiptId);              
            }
            return Receipt;
        }
        public ReceiptDetail GetReceiptDetail(long ReceiptDetailId)
        {
            ReceiptDetail ReceiptDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ReceiptDetail = Context.ReceiptDetails.FirstOrDefault(x => x.ReceiptDetailId == ReceiptDetailId);
            }
            return ReceiptDetail;
        }
        public ReceiptDetail GetReceiptDetailByItemBased(string ReferenceId)
        {
            ReceiptDetail ReceiptDetail = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ReceiptDetail = Context.ReceiptDetails.Include("Receipt").Include("Account").FirstOrDefault(x =>x.InvoiceType==InvoiceType.ItemBased && x.ReferenceTrasnactionId == ReferenceId);
            }
            return ReceiptDetail;
        }
        public BankTransferReceipt GetBankTransferReceipt(long ReceiptId)
        {
            BankTransferReceipt BankTransferReceipt = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BankTransferReceipt = Context.BankTransferReceipts.Include("ReceiptDetails").Include("Account").Include("BankTransfer").FirstOrDefault(x => x.ReceiptId == ReceiptId);
            }
            return BankTransferReceipt;
        }
        public CheckReceipt GetCheckReceipt(long ReceiptId)
        {
            CheckReceipt CheckReceipt = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CheckReceipt = Context.CheckReceipts.Include("ReceiptDetails").Include("Account").Include("DepositedInto").FirstOrDefault(x => x.ReceiptId == ReceiptId);
            }
            return CheckReceipt;
        }
        public CreditCardReceipt GetCreditCardReceipt(long ReceiptId)
        {
            CreditCardReceipt CreditCardReceipt = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CreditCardReceipt = Context.CreditCardReceipts.Include("ReceiptDetails").Include("CCAccount").FirstOrDefault(x => x.ReceiptId == ReceiptId);
            }
            return CreditCardReceipt;
        }


        public IList<Receipt> GetReceiptByDate(DateTime Date, long CompanyId)
        {
            IList<Receipt> ReceiptInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ReceiptInfo = Context.Receipts.Include("ReceiptDetails").Include("Account").Where(x => x.TransactionDate.Day == Date.Day && x.TransactionDate.Month == Date.Month && x.TransactionDate.Year == Date.Year && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Receipt>();
                return ReceiptInfo;
            }
        }

        public IList<Receipt> GetReceiptByAmount(double Amount, string RefNo, long CompanyId)
        {
            IList<Receipt> ReceiptInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ReceiptInfo = Context.Receipts.Include("ReceiptDetails").Include("Account").Where(x => (x.Amount.Equals(Amount) || x.Reference.Contains(RefNo)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Receipt>();
                return ReceiptInfo;
            }
        }
        public IList<Receipt> GetReceiptByCustomerName(string CustomerName, long CompanyId)
        {
            IList<Receipt> ReceiptInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ReceiptInfo = Context.Receipts.Include("ReceiptDetails").Include("Account").Where(x => (x.Account.Name.Contains(CustomerName) || x.Reference.Contains(CustomerName)) && x.CompanyId == CompanyId).OrderByDescending(x => x.TransactionDate).ToList<Receipt>();
                return ReceiptInfo;
            }
        }

    }
}
