using fa.model.Accounting.Masters;
using fa.context;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public class AccountGroupManager
    {
        private static volatile AccountGroupManager instance;
        private static object syncRoot = new Object();
        AccountGroupManager()
        {

        }
        public static AccountGroupManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AccountGroupManager();
                    }
                }

                return instance;
            }
        }
        public IList<AccountGroup> ListAccountGroupById(long TypeId)
        {
            IList<AccountGroup> lAccountGroup = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
               
                    lAccountGroup = (from AccountGroup in Context.AccountGroups where AccountGroup.ParentAccountGroupId == TypeId select AccountGroup).ToList();
                    return lAccountGroup;
               
            }         
        }
        public IList<AccountGroup> GetAllAccountGroup()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<AccountGroup> AccountGroup = Context.AccountGroups.OrderBy(x => x.Id).ToList<AccountGroup>();
                return AccountGroup;
            }
        }
        public IList<AccountGroup> GetParentAccountGroup()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<AccountGroup> AccountGroups = (from AccountGroup in Context.AccountGroups where AccountGroup.ParentAccountGroupId.Equals(null) select AccountGroup).ToList();
                return AccountGroups;
            }
        }

        public AccountGroup GetAccountGroupById(long TypeId)
        {
            AccountGroup lAccountGroup = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lAccountGroup = Context.AccountGroups.Include("AccountGroupClassification").Include("ParentAccountGroup").Where(x=>x.Id == TypeId).First();
                return lAccountGroup;
            }
        }
    }
}
