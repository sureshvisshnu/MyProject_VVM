using fa.model.Accounting.Masters;
using fa.context;
using fa.model.UserProfile;
using FaData.Utils;

namespace fa.api.Accounting
{
    public class CostCenterManager
    {
        private static volatile CostCenterManager instance;
        private static object syncRoot = new Object();
        CostCenterManager()
        {

        }
        public static CostCenterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CostCenterManager();
                    }
                }

                return instance;
            }
        }
        public CostCenter GetCostCenterById(long CostCenterId)
        {
            CostCenter CostCenterInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CostCenterInfo = Context.CostCenters.Find(CostCenterId);
                return CostCenterInfo;
            }
        }   
        public CostCenter GetCostCenterByCompanyId(long CompanyId)
        {
            CostCenter CostCenterInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CostCenterInfo = Context.CostCenters.FirstOrDefault(costCenter => costCenter.ParentCompanyId == CompanyId);
                return CostCenterInfo;
            }
        }
        public Boolean CostCenterNameUniqueById(CostCenter CostCenter)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {               
                    CostCenter lCostCenter = null;
                    if (CostCenter.CostCenterId == 0)
                    {
                         lCostCenter=Context.CostCenters.FirstOrDefault(x => x.Name == CostCenter.Name && x.ParentCompanyId == CostCenter.ParentCompanyId);                     
                    }
                    else
                    {
                        lCostCenter = Context.CostCenters.FirstOrDefault(x => x.Name == CostCenter.Name && x.ParentCompanyId == CostCenter.ParentCompanyId && !x.CostCenterId.Equals(CostCenter.CostCenterId));
                    }
                    if (lCostCenter != null)
                    {
                        Status = false;
                    }              
            }
            return Status;
        }
       
        public Boolean DeleteCostCenter(long CostCenterId)
        {
            Boolean Deleted = false;
            CostCenter CostCenterInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Access Access = Context.Accesses.FirstOrDefault(x => x.ResourceId == CostCenterId);
                        if (Access != null)
                        {
                            Context.Accesses.Where(p => p.ResourceId == CostCenterId).ToList().ForEach(p => Context.Accesses.Remove(p));
                        }
                        CostCenterInfo = Context.CostCenters.Find(CostCenterId);
                        Context.CostCenters.Remove(CostCenterInfo);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {                        
                        dbContextTransaction.Rollback();
                        Deleted = false;
                        Logger.LogError(e);
                    }
                }
            }
            return Deleted;
        }
        public CostCenter AddCostCenter(CostCenter costCenter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.CostCenters.Add(costCenter);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        costCenter = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
                return costCenter;
            }
        }
        public CostCenter UpdateCostCenter(CostCenter CostCenter)
        {
            CostCenter CostCenterInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                CostCenterInfo = Context.CostCenters.Find(CostCenter.CostCenterId);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (CostCenterInfo != null)
                        {
                            Context.Entry(CostCenterInfo).CurrentValues.SetValues(CostCenter);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        CostCenterInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return CostCenterInfo;
        }
        public IList<CostCenter> GetAllCostCenter()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<CostCenter> CostCenterInfo = Context.CostCenters.ToList<CostCenter>();
                return CostCenterInfo;
            }
        }

        public IList<CostCenter> ListCostCenterByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<CostCenter> CostCenterInfo = (from CostCenter in Context.CostCenters where CostCenter.ParentCompanyId == CompanyId select CostCenter).ToList();
                return CostCenterInfo;
            }
        }

        public IList<CostCenter> GetCostCenter(long[] Keys,long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<CostCenter> CostCenterInfo = (from CostCenter in Context.CostCenters where Keys.Contains(CostCenter.CostCenterId) where CostCenter.ParentCompanyId==CompanyId select CostCenter).ToList();
                return CostCenterInfo;
            }
        }
        public List<CostCenter> GetAccessibleCostCenter(User User, long CompanyId)
        {
            CostCenterManager CostCenterManager = new CostCenterManager();
            List<CostCenter> CostCenter = new List<CostCenter>();
            if (User.IsSuperAdmin)
            {
                CostCenter = (List<CostCenter>)CostCenterManager.ListCostCenterByCompanyId(CompanyId);
            }
            else if (User.Accesses != null && User.Accesses.Count > 0)
            {
                List<long> Keys = new List<long>();
                foreach (Access access in User.Accesses)
                {
                    if (access.ResourceType.Equals(ResourceType.CostCenter))
                    {
                        Keys.Add(access.ResourceId);
                    }
                    CostCenter = (List<CostCenter>)CostCenterManager.GetCostCenter(Keys.ToArray<long>(), CompanyId);
                }
            }
            return CostCenter;
        }
    }
}
