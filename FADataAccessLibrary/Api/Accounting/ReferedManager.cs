using fa.context;
using fa.model.Common;

namespace fa.api.Accounting
{
    public class ReferedManager
    {
        private static volatile ReferedManager instance;
        private static object syncRoot = new Object();
        ReferedManager()
        {
        }
        public static ReferedManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ReferedManager();
                    }
                }
                return instance;
            }
        }


        public List<Refered> GetAllRefered(long CompanyId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                List<Refered> Refered = context.Refereds.Where(x=>x.CompanyId== CompanyId).ToList<Refered>();
                return Refered;
            }
        }
        public Refered GetReferedByName(string Name, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Refered ReferedInfo = Context.Refereds.FirstOrDefault(x=>x.Name==Name && x.CompanyId== CompanyId);
                if (ReferedInfo != null)
                {
                    return ReferedInfo;
                }
            }
            return null;
        }

        public Refered AddRefered(Refered refered)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.Refereds.Add(refered);
                Context.SaveChanges();
            }
            return refered;
        }
        public IList<Refered> ListAllRefered(long CompanyId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                IList<Refered> Refered = context.Refereds.Where(x => x.CompanyId == CompanyId).ToList<Refered>();
                return Refered;
            }
        }
    }
}
