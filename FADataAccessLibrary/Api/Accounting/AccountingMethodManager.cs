using fa.context;
using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fa.api.Accounting
{
    public class AccountingMethodManager
    {
        private static volatile AccountingMethodManager instance;
        private static object syncRoot = new Object();
        AccountingMethodManager()
        {

        }
        public static AccountingMethodManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AccountingMethodManager();
                    }
                }

                return instance;
            }
        }
        public AccountingMethod GetAccountingMethodById(long AccountingMethodId)
        {
            AccountingMethod lAccountingMethod = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lAccountingMethod = Context.AccountingMethods.Find(AccountingMethodId);
                return lAccountingMethod;
            }
        }
        public IList<AccountingMethod> GetAllAccountingMethod()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<AccountingMethod> AccountingMethod = Context.AccountingMethods.ToList<AccountingMethod>();
                return AccountingMethod;
            }
        }
    }
}
