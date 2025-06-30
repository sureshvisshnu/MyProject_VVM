using fa.context;
using fa.model.Accounting.Transaction;
using fa.model.Accounting.Transactions;
using fa.api.accounting.doubleentry;
using Microsoft.EntityFrameworkCore;
using fa.model.OrderManagement;

namespace fa.api.Accounting
{
    public class InvoiceManager
    {
        private static volatile InvoiceManager instance;
        private static object syncRoot = new Object();
        InvoiceManager()
        {

        }
        public static InvoiceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InvoiceManager();
                    }
                }

                return instance;

            }
        }
     
        public IList<Invoice> ListAllReceiptInvoice(long CompanyId, long AccountId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Invoice> PaymentBillsInfo = (from Invoice in Context.Invoices where Invoice.CustomerId == AccountId where Invoice.CompanyId == CompanyId select Invoice).ToList();
                return PaymentBillsInfo;

            }
        }
     
        public Invoice AddInvoice(Invoice invoice)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Invoices.Add(invoice);
                        Context.SaveChanges();
                        //daybook
                        InvoiceDoubleEntryManager.Instance.RecordInvoice(invoice, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        invoice = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return invoice;
        }
        
        public Invoice UpdateInvoice(Invoice Invoice)
        {
            Invoice InvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        InvoiceInfo = Context.Invoices.Find(Invoice.InvoiceId);
                        if (InvoiceInfo != null)
                        {
                           
                            Invoice InvoiceInfoFromDB = GetInvoice(Invoice.InvoiceId);
                            if (InvoiceInfoFromDB.InvoiceDetails.Count > 0)
                            {
                                Context.InvoiceDetails.Where(p => p.InvoiceId == InvoiceInfoFromDB.InvoiceId).ToList().ForEach(p => Context.InvoiceDetails.Remove(p));
                                Context.SaveChanges();
                            }
                            if (InvoiceInfoFromDB.InvoiceAdditionalTransactions.Count > 0)
                            {
                                Context.InvoiceAdditionalTransactions.Where(p => p.InvoiceId == InvoiceInfoFromDB.InvoiceId).ToList().ForEach(p => Context.InvoiceAdditionalTransactions.Remove(p));
                                Context.SaveChanges();
                            }
                            Invoice.Company = null;
                            Invoice.Customer = null;
                            Invoice.Term = null;
                            Context.Entry(InvoiceInfo).CurrentValues.SetValues(Invoice);
                            foreach (InvoiceDetail Detail in Invoice.InvoiceDetails)
                            {
                                Detail.Invoice = null;
                                Detail.InvoiceId = Invoice.InvoiceId;
                                Context.InvoiceDetails.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (InvoiceAdditionalTransaction Detail in Invoice.InvoiceAdditionalTransactions)
                            {
                                Detail.Invoice = null;
                                Detail.InvoiceId = Invoice.InvoiceId;
                                Context.InvoiceAdditionalTransactions.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                        }
                        //daybook
                        InvoiceDoubleEntryManager.Instance.RecordInvoice(Invoice, Context);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        InvoiceInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return InvoiceInfo;
        }

       
        public Boolean DeleteInvoice(long InvoiceId)
        {
            Boolean Deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Invoice InvoiceInfo = GetInvoice(InvoiceId);
                        if (InvoiceInfo.InvoiceDetails.Count > 0)
                        {
                            Context.InvoiceDetails.Where(p => p.InvoiceId == InvoiceInfo.InvoiceId).ToList().ForEach(p => Context.InvoiceDetails.Remove(p));
                            Context.SaveChanges();
                        }
                        Context.Invoices.Remove(Context.Invoices.Find(InvoiceId));
                        Context.SaveChanges();
                        //daybook delete
                        InvoiceDoubleEntryManager.Instance.DeleteInvoices(InvoiceInfo, Context);
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
        
        public List<Invoice> GetInvoiceByCompanyId(long CompanyId)
        {
            List<Invoice> Invoices = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    Invoices = (from Invoice in Context.Invoices where Invoice.CompanyId == CompanyId select Invoice).ToList();
                }
            }
            return Invoices;
        }

        public Invoice GetInvoice(long InvoiceId)
        {
            Invoice Invoice = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Invoice = Context.Invoices.Include("Company").Include("InvoiceDetails").Include("InvoiceAdditionalTransactions").Include("Customer").Include("Term").FirstOrDefault(x=>x.InvoiceId==InvoiceId);
            }
            return Invoice;
        }

        public IList<Invoice> GetUnPaidInvoicesByAccountId(long AccountId)
        {
            IList<Invoice> Invoices = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Invoices = (from Invoice in Context.Invoices where Invoice.CustomerId == AccountId && Invoice.Balance > 0 select Invoice).ToList();                
            }
            return Invoices;
        }
        public IList<Invoice> GetRecentInvoices(long CompanyId)
        {
            IList<Invoice> InvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var RecentInvoiceInfo = Context.Invoices.Include("InvoiceDetails").Include("Customer").Where(y => y.CompanyId == CompanyId).OrderByDescending(x=>x.InvoiceDate).ToList<Invoice>().Take(20);
                if(RecentInvoiceInfo!=null)
                {
                    InvoiceInfo = RecentInvoiceInfo.ToList();
                }
                return InvoiceInfo;
            }
        }
        public IList<Invoice> GetInvoiceByDate(DateTime? Date, long CompanyId)
        {
            IList<Invoice> InvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InvoiceInfo = Context.Invoices.Include("InvoiceDetails").Include("Customer").Where(x => x.InvoiceDate == Date && x.CompanyId == CompanyId).OrderByDescending(x => x.InvoiceDate).ToList<Invoice>();
                return InvoiceInfo;
            }
        }

        public IList<Invoice> GetInvoiceByReferenceNo(string RefNo, long CompanyId)
        {
            IList<Invoice> InvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InvoiceInfo = Context.Invoices.Include("InvoiceDetails").Include("Customer").Where(x => x.ReferenceNumber == RefNo && x.CompanyId == CompanyId).OrderByDescending(x => x.InvoiceDate).ToList<Invoice>();
                return InvoiceInfo;
            }
        }

        public IList<Invoice> GetInvoiceByCustomerName(string CustomerName, long CompanyId)
        {
            IList<Invoice> InvoiceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InvoiceInfo = Context.Invoices.Include("InvoiceDetails").Include("Customer").Where(x => (x.Customer.Name.Contains(CustomerName)|| x.ReferenceNumber.Contains(CustomerName)) && x.CompanyId == CompanyId).OrderByDescending(x => x.InvoiceDate).ToList<Invoice>();
                return InvoiceInfo;
            }
        }
        public void ReversePayment(ReceiptDetail ReceiptDetail, AccountMasterContext Context)
        {
            if (!string.IsNullOrEmpty(ReceiptDetail.ReferenceTrasnactionId))
            {
                Invoice InvoiceInfo =GetInvoice(long.Parse(ReceiptDetail.ReferenceTrasnactionId));
                if (ReceiptDetail.Amount>0)
                {
                    InvoiceInfo.Paid -= (float)ReceiptDetail.Amount;
                    InvoiceInfo.Balance += (float)ReceiptDetail.Amount;
                    UpdateInvoiceForApplayAndReversePayment(InvoiceInfo, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Invoice:" + ReceiptDetail.ReferenceTrasnactionId);
            }
        }
        public void ApplyPayment(ReceiptDetail ReceiptDetail, AccountMasterContext Context)
        {
            if (!string.IsNullOrEmpty(ReceiptDetail.ReferenceTrasnactionId))
            {
                long InvoiceId = long.Parse(ReceiptDetail.ReferenceTrasnactionId);
                Invoice InvoiceInfo = Context.Invoices.Include("InvoiceDetails").FirstOrDefault(x => x.InvoiceId == InvoiceId);
                if (InvoiceInfo.Balance >= (float)ReceiptDetail.Amount)
                {
                    InvoiceInfo.Paid += (float)ReceiptDetail.Amount;
                    InvoiceInfo.Balance -= (float)ReceiptDetail.Amount;
                    UpdateInvoiceForApplayAndReversePayment(InvoiceInfo, Context);
                }
            }
            else
            {
                throw new ArgumentException("Amount paid is morethan the balance of the Invoice:" + ReceiptDetail.ReferenceTrasnactionId);
            }
        }
        private void UpdateInvoiceForApplayAndReversePayment(Invoice Invoice,AccountMasterContext Context)
        {       
         Invoice InvoiceInfo = null;
            try
            {
                InvoiceInfo = Context.Invoices.Find(Invoice.InvoiceId);
                if (InvoiceInfo != null)
                {
                    Invoice InvoiceInfoFromDB = GetInvoice(Invoice.InvoiceId);
                    //if (InvoiceInfoFromDB.InvoiceDetails.Count > 0)
                    //{
                    //    Context.InvoiceDetails.Where(p => p.InvoiceId == InvoiceInfoFromDB.InvoiceId).ToList().ForEach(p => Context.InvoiceDetails.Remove(p));
                    //    Context.SaveChanges();
                    //}
                    Invoice.Company = null;
                    Invoice.Customer = null;
                    Invoice.Term = null;
                    InvoiceInfo.Company = null;
                    InvoiceInfo.Customer = null;
                    InvoiceInfo.Term = null;
                    InvoiceInfo.CostCenter = null;
                    //foreach (InvoiceDetail Detail in Invoice.InvoiceDetails)
                    //{
                    //    Detail.Invoice = null;
                    //    Detail.InvoiceId = Invoice.InvoiceId;
                    //    Context.InvoiceDetails.Add(Detail);
                    //    Context.SaveChanges();
                    //}
                    Context.Entry(InvoiceInfo).CurrentValues.SetValues(Invoice);
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                InvoiceInfo = null;
                throw (e);
            }          
        }
    }
}
