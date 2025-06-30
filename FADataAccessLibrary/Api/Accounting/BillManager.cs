using fa.context;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using Fa.api.Accounting.DoubleEntry;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public class BillManager
    {
        private static volatile BillManager instance;
        private static object syncRoot = new Object();
        BillManager()
        {

        }

        public static BillManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BillManager();
                    }
                }
                return instance;
            }
        }

        public IList<Bill> ListAllPaymentBills(long CompanyId, long AccountId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Bill> PaymentBillsInfo = (from Bill in Context.Bills where Bill.VendorId == AccountId where Bill.CompanyId == CompanyId select Bill).ToList();
                return PaymentBillsInfo;
            }
        }

        public Bill AddBill(Bill bill)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Bills.Add(bill);
                        Context.SaveChanges();
                        //daybook
                        BillDoubleEntryManager.Instance.RecordBill(bill, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        bill = null;
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return bill;
        }

        public void ReversePayment(PaymentDetail PaymentDetail, AccountMasterContext Context)
        {
            if (!string.IsNullOrEmpty(PaymentDetail.ReferenceTrasnactionId))
            {
                Bill BillInfo = GetBill(long.Parse(PaymentDetail.ReferenceTrasnactionId));
                if (PaymentDetail.Amount>0)
                {
                    BillInfo.Paid -= (float)PaymentDetail.Amount;
                    BillInfo.Balance += (float)PaymentDetail.Amount;
                    UpdateBillForApplayAndReversePayment(BillInfo, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Bill:" + PaymentDetail.ReferenceTrasnactionId);
            }
        }

        public void ApplyPayment(PaymentDetail PaymentDetail, AccountMasterContext Context)
        {
            if(!string.IsNullOrEmpty(PaymentDetail.ReferenceTrasnactionId))
            {
                long BillId = long.Parse(PaymentDetail.ReferenceTrasnactionId);
                Bill BillInfo = Context.Bills.FirstOrDefault(x => x.BillId == BillId);
                if (BillInfo.Balance >= (float)PaymentDetail.Amount)
                {
                    BillInfo.Paid += (float)PaymentDetail.Amount;
                    BillInfo.Balance -= (float)PaymentDetail.Amount;
                    UpdateBillForApplayAndReversePayment(BillInfo, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Bill:" + PaymentDetail.ReferenceTrasnactionId);
            }
        }
        private void UpdateBillForApplayAndReversePayment(Bill Bill, AccountMasterContext Context)
        {
            Bill BillInfo = null;            
            try
            {
                BillInfo = Context.Bills.Find(Bill.BillId);
                if (BillInfo != null)
                {
                    Bill BillInfoFromDB = GetBill(Bill.BillId);
                    //if (BillInfoFromDB.BillDetails.Count > 0)
                    //{
                    //    Context.BillDetails.Where(p => p.BillId == BillInfoFromDB.BillId).ToList().ForEach(p => Context.BillDetails.Remove(p));
                    //    Context.SaveChanges();
                    //}
                    //if (BillInfoFromDB.BillAttachments.Count > 0)
                    //{
                    //    Context.BillAttachments.Where(p => p.BillId == BillInfoFromDB.BillId).ToList().ForEach(p => Context.BillAttachments.Remove(p));
                    //    Context.SaveChanges();
                    //}
                    Bill.Company = null;
                    Bill.Vendor = null;
                    Bill.Term = null;
                    Context.Entry(BillInfo).CurrentValues.SetValues(Bill);
                    //foreach (BillDetail Detail in Bill.BillDetails)
                    //{
                    //    Detail.Bill = null;
                    //    Detail.BillId = Bill.BillId;
                    //    Context.BillDetails.Add(Detail);
                    //    Context.SaveChanges();
                    //}
                    //foreach (BillAttachment Attachment in Bill.BillAttachments)
                    //{
                    //    Attachment.Bill = null;
                    //    Attachment.BillId = Bill.BillId;
                    //    Context.BillAttachments.Add(Attachment);
                    //    Context.SaveChanges();
                    //}
                    Context.SaveChanges();
                }                       
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                BillInfo = null;
                throw (e);
            }            
        }
        public Bill UpdateBill(Bill Bill)
        {
            Bill BillInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        BillInfo = Context.Bills.Find(Bill.BillId);
                        if (BillInfo != null)
                        {                          
                            Bill BillInfoFromDB = GetBill(Bill.BillId);
                            if (BillInfoFromDB.BillDetails.Count > 0)
                            {
                                Context.BillDetails.Where(p => p.BillId == BillInfoFromDB.BillId).ToList().ForEach(p => Context.BillDetails.Remove(p));
                                Context.SaveChanges();
                            }
                            if (BillInfoFromDB.BillAttachments.Count > 0)
                            {
                                Context.BillAttachments.Where(p => p.BillId == BillInfoFromDB.BillId).ToList().ForEach(p => Context.BillAttachments.Remove(p));
                                Context.SaveChanges();
                            }
                            Bill.Company = null;
                            Bill.Vendor = null;
                            Bill.Term = null;
                            Context.Entry(BillInfo).CurrentValues.SetValues(Bill);
                            foreach (BillDetail Detail in Bill.BillDetails)
                            {
                                Detail.Bill = null;
                                Detail.BillId = Bill.BillId;
                                Context.BillDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (BillAttachment Attachment in Bill.BillAttachments)
                            {
                                Attachment.Bill = null;
                                Attachment.BillId = Bill.BillId;
                                Context.BillAttachments.Add(Attachment);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                        }
                        //daybook
                        BillDoubleEntryManager.Instance.RecordBill(Bill, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        BillInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return BillInfo;
        }
      
        public Boolean DeleteBill(long BillId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Bill BillInfo = GetBill(BillId);
                        if (BillInfo.BillDetails.Count > 0)
                        {
                            Context.BillDetails.Where(p => p.BillId == BillInfo.BillId).ToList().ForEach(p => Context.BillDetails.Remove(p));
                            Context.SaveChanges();
                        }
                        Context.Bills.Remove(Context.Bills.Find(BillId));
                        Context.SaveChanges();
                        //daybook delete
                        BillDoubleEntryManager.Instance.DeleteBills(BillInfo, Context);
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Deleted;
        }

        public Bill GetBill(long BillId)
        {
            Bill Bill = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Bill = Context.Bills.Include("BillDetails").Include("BillAttachments").Include("Vendor").Include("Term").FirstOrDefault(x => x.BillId == BillId);
            }
            return Bill;
        }
        public IList<Bill> GetUnPaidBillsByAccountId(long AccountId)
        {
            IList<Bill> lBill = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lBill = (from Bill in Context.Bills where Bill.VendorId == AccountId && Bill.Balance > 0 select Bill).ToList();
            }
            return lBill;
        }
        public IList<Bill> GetRecentBills(long CompanyId)
        {
            IList<Bill> BillInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentBillInfo = Context.Bills.Include("BillDetails").Include("Vendor").Where(y => y.CompanyId == CompanyId).OrderByDescending(x => x.BillDate).ToList<Bill>().Take(20);
                if (RecentBillInfo != null)
                {
                    BillInfo = RecentBillInfo.ToList();
                }
                return BillInfo;
            }
        }
        public IList<Bill> GetBillByDate(DateTime? Date , long CompanyId)
        {
            IList<Bill> BillInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BillInfo = Context.Bills.Include("BillDetails").Include("Vendor").Include("PaymentTerm").Where(x => x.BillDate == Date && x.CompanyId == CompanyId).OrderByDescending(x => x.BillDate).ToList<Bill>();
                return BillInfo;
            }
        }

        public IList<Bill> GetBillByReferenceNo(string RefNo, long CompanyId)
        {
            IList<Bill> BillInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BillInfo = Context.Bills.Include("BillDetails").Include("Vendor").Where(x => x.ReferenceNumber == RefNo && x.CompanyId == CompanyId).OrderByDescending(x => x.BillDate).ToList<Bill>();
                return BillInfo;
            }
        }

        public IList<Bill> GetBillBySupplerName(string SupplierName, long CompanyId)
        {
            IList<Bill> BillInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BillInfo = Context.Bills.Include("BillDetails").Include("Vendor").Where(x => (x.Vendor.Name.Contains(SupplierName)|| x.ReferenceNumber.Contains(SupplierName)) && x.CompanyId == CompanyId).OrderByDescending(x => x.BillDate).ToList<Bill>();
                return BillInfo;
            }
        }
    }
}
