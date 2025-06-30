using fa.model.Accounting.Masters;
using fa.context;

namespace fa.api.Accounting
{
    public class PaymentTermManager
    {
        private static volatile PaymentTermManager instance;
        private static object syncRoot = new Object();
        PaymentTermManager()
        {

        }
        public static PaymentTermManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PaymentTermManager();
                    }
                }

                return instance;
            }
        }
        public PaymentTerm GetPaymentTermById(long PaymentTermId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentTerm PaymentTermInfo = Context.PaymentTerms.Find(PaymentTermId);
                if (PaymentTermInfo != null)
                {
                    return PaymentTermInfo;
                }
            }
            return null;
        }
        public IList<PaymentTerm> GetAllPaymentTermByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<PaymentTerm> PaymentTermInfo = (from PaymentTerm in Context.PaymentTerms where PaymentTerm.CompanyId == CompanyId select PaymentTerm).ToList();
                return PaymentTermInfo;
            }
        }
        public PaymentTerm GetPaymentTermByName(long CompanyID, String PaymentTermName)
        {
            PaymentTerm PaymentTermInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentTermInfo = Context.PaymentTerms.FirstOrDefault(x => x.Name == PaymentTermName && x.CompanyId == CompanyID);
                return PaymentTermInfo;
            }
        }
        public PaymentTerm CheckPaymentTermNameInUpdate(long CompanyID, String PaymentTermName, long PaymentTermId)
        {
            PaymentTerm PaymentTermInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentTermInfo = Context.PaymentTerms.FirstOrDefault(x => x.Name == PaymentTermName && x.CompanyId == CompanyID && !x.Id.Equals(PaymentTermId));
                return PaymentTermInfo;
            }
        }
        public Boolean DeletePaymentTerm(long PaymentTermId)
        {
            Boolean Deleted = false;
            PaymentTerm PaymentTermInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PaymentTermInfo = Context.PaymentTerms.Find(PaymentTermId);
                        Context.PaymentTerms.Remove(PaymentTermInfo);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
                }
            }
            return Deleted;
        }
        public PaymentTerm AddPaymentTerm(PaymentTerm paymentTerm)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.PaymentTerms.Add(paymentTerm);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return paymentTerm;
        }
        public PaymentTerm UpdatePaymentTerm(PaymentTerm PaymentTerm)
        {
            PaymentTerm PaymentTermInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentTermInfo = Context.PaymentTerms.Find(PaymentTerm.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (PaymentTermInfo != null)
                        {
                            Context.Entry(PaymentTermInfo).CurrentValues.SetValues(PaymentTerm);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PaymentTermInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return PaymentTermInfo;
        }
    }
}
