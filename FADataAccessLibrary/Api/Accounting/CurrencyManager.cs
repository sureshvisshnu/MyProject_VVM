using fa.model.Common;
using fa.context;
using fa.model.Accounting.Masters;

namespace fa.api.Accounting
{
    public class CurrencyManager
    {
        private static volatile CurrencyManager instance;
        private static object syncRoot = new Object();
        CurrencyManager()
        {

        }
        public static CurrencyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CurrencyManager();
                    }
                }

                return instance;
            }
        }
        public Currency GetCurrency(Company Company)
        {
            Currency CurrencyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CurrencyInfo = Context.Currencies.Find(Company.PrimaryCurrencyId);
                return CurrencyInfo;
            }
        }
        public Currency GetCurrencyById(long CurrencyId)
        {
            Currency CurrencyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CurrencyInfo = Context.Currencies.Find(CurrencyId);
                return CurrencyInfo;
            }
        }
        public Currency GetCurrencyByName(String CurrencyName)
        {
            Currency CurrencyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CurrencyInfo = Context.Currencies.FirstOrDefault(x => x.Name == CurrencyName);
                return CurrencyInfo;
            }
        }
        public Currency CheckCurrencyNameInUpdate(String CurrencyName, long CurrencyId)
        {
            Currency CurrencyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CurrencyInfo = Context.Currencies.FirstOrDefault(x => x.Name == CurrencyName && !x.CurrencyId.Equals(CurrencyId));
                return CurrencyInfo;
            }
        }
        public Boolean DeleteCurrency(long CurrencyId)
        {
            Boolean Deleted = false;
            Currency CurrencyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        CurrencyInfo = Context.Currencies.Find(CurrencyId);
                        Context.Currencies.Remove(CurrencyInfo);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Deleted = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Deleted;
        }

        public Boolean AddCurrency(Currency Currency)
        {
            Boolean Added = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (Currency.CurrencyId == 0)
                        {
                            Context.Currencies.Add(Currency);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                            Added = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Added = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
                return Added;
            }
        }
        public Currency UpdateCurrency(Currency Currency)
        {
            Currency CurrencyInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CurrencyInfo = Context.Currencies.Find(Currency.CurrencyId);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (CurrencyInfo != null)
                        {
                            Context.Entry(CurrencyInfo).CurrentValues.SetValues(Currency);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CurrencyInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CurrencyInfo;
        }
        public IList<Currency> GetAllCurrencies()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Currency> CurrencyInfo = Context.Currencies.ToList<Currency>();
                return CurrencyInfo;
            }
        }
    }

}
