using fa.model.Accounting.Masters;
using fa.context;

namespace fa.api.Accounting
{
    public class PaymentMethodManager
    {
        private static volatile PaymentMethodManager instance;
        private static object syncRoot = new Object();
        PaymentMethodManager()
        {

        }
        public static PaymentMethodManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PaymentMethodManager();
                    }
                }

                return instance;
            }
        }
        public PaymentMethod GetPaymentMethodById(long PaymentMethodId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentMethod PaymentMethodInfo = Context.PaymentMethods.Find(PaymentMethodId);
                if (PaymentMethodInfo != null)
                {
                    return PaymentMethodInfo;
                }
            }
            return null;
        }
        public IList<PaymentMethod> GetAllPaymentMethodByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<PaymentMethod> PaymentMethodInfo = (from PaymentMethod in Context.PaymentMethods where PaymentMethod.CompanyId == CompanyId select PaymentMethod).ToList();
                return PaymentMethodInfo;
            }
        }
        public PaymentMethod GetPaymentMethodByName(long CompanyID,String PaymentMethodName)
        {
            PaymentMethod PaymentMethodInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentMethodInfo = Context.PaymentMethods.FirstOrDefault(x => x.Name == PaymentMethodName && x.CompanyId==CompanyID);
                return PaymentMethodInfo;
            }
        }
        public PaymentMethod CheckPaymentMethodNameInUpdate(long CompanyID, String PaymentMethodName, long PaymentMethodId)
        {
            PaymentMethod PaymentMethodInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentMethodInfo = Context.PaymentMethods.FirstOrDefault(x => x.Name == PaymentMethodName && x.CompanyId==CompanyID && !x.Id.Equals(PaymentMethodId));
                return PaymentMethodInfo;
            }
        }
        public Boolean DeletePaymentMethod(long PaymentMethodId)
        {
            Boolean Deleted = false;
            PaymentMethod PaymentMethodInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PaymentMethodInfo = Context.PaymentMethods.Find(PaymentMethodId);
                        Context.PaymentMethods.Remove(PaymentMethodInfo);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
                }
            }
            return Deleted;
        }
        public PaymentMethod AddPaymentMethod(PaymentMethod paymentMethod)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (paymentMethod.Id == 0)
                        {
                            Context.PaymentMethods.Add(paymentMethod);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return paymentMethod;
        }
        public PaymentMethod UpdatePaymentMethod(PaymentMethod PaymentMethod)
        {
            PaymentMethod PaymentMethodInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PaymentMethodInfo = Context.PaymentMethods.Find(PaymentMethod.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (PaymentMethodInfo != null)
                        {
                            Context.Entry(PaymentMethodInfo).CurrentValues.SetValues(PaymentMethod);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PaymentMethodInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return PaymentMethodInfo;
        }
    }
}
