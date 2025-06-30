using fa.context;
using fa.model.OrderManagement;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class HospitalInventoryManager
    {
        private static volatile HospitalInventoryManager instance;
        private static object syncRoot = new Object();
        HospitalInventoryManager()
        {

        }
        public static HospitalInventoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HospitalInventoryManager();
                    }
                }
                return instance;
            }
        }
        public InventoryLocation GetLocationById(long LocationId)
        {
            InventoryLocation LocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                LocationInfo = Context.InventoryLocation.FirstOrDefault(x => x.Id == LocationId);
                return LocationInfo;
            }
        }
        public InventoryLocation GetLocationByName(string Name,long CompanyId)
        {
            InventoryLocation LocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                LocationInfo = Context.InventoryLocation.Where(x => x.CompanyId == CompanyId && x.Name == Name).FirstOrDefault();
                return LocationInfo;
            }
        }
        public InventoryLocation GetLocationIdByName(string IdName)
        {
            InventoryLocation LocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {

                LocationInfo = Context.InventoryLocation.Include("Id").FirstOrDefault(x => x.Name == IdName);
                return LocationInfo;
            }
        }
        public InventoryLocation GetLocationFillById(long LocationId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryLocation LocationInfo = Context.InventoryLocation.Find(LocationId);
                if (LocationInfo != null)
                {
                    return LocationInfo;
                }
            }
            return null;
        }
        public InventoryLocation AddLocation(InventoryLocation location)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.InventoryLocation.Add(location);
                Context.SaveChanges();
            }
            return location;
        }

        public InventoryLocation UpdateLocation(InventoryLocation Location)
        {
            InventoryLocation LocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                LocationInfo = Context.InventoryLocation.Find(Location.Id);
                if (LocationInfo != null)
                {
                    Context.Entry(LocationInfo).CurrentValues.SetValues(Location);
                    Context.SaveChanges();
                }
            }
            return LocationInfo;
        }
        public bool FindLocatioUnique(InventoryLocation Location)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (Location.Id == 0)
                    {
                        Context.InventoryLocation.Where(x => x.Name == Location.Name && x.CompanyId == Location.CompanyId).First<InventoryLocation>();
                        Status = false;
                    }
                    else
                    {
                        Context.InventoryLocation.Where(x => x.Name == Location.Name && !x.Id.Equals(Location.Id) && x.CompanyId == Location.CompanyId).First<InventoryLocation>();
                        Status = false;
                    }
                }
                #pragma warning disable 0168
                catch (Exception ex)
                {
                }
                #pragma warning restore 0168
            }
            return Status;
        }
        public IList<InventoryLocation> ListAllInventoryLocation(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<InventoryLocation> LocationInfo = Context.InventoryLocation.Where(x=>x.CompanyId==CompanyId).OrderBy(x => x.Name).ToList<InventoryLocation>();
                return LocationInfo;
            }
        }
        public bool DeleteLocation(long LocationId)
        {

            Boolean Deleted = false;
            InventoryLocation LocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        LocationInfo = Context.InventoryLocation.Find(LocationId);
                        Context.InventoryLocation.Remove(LocationInfo);
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
        public static void GenerateLocation(string IdName)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InventoryLocation lLocation = Context.InventoryLocation.Include("Id").FirstOrDefault(x => x.Name == IdName); if (lLocation != null)
                {
                    string CurrentName = lLocation.Name;
                    string lastCharacter = CurrentName.Substring(CurrentName.Length - 1);
                    int CurrentLocationNo = Int32.Parse(lastCharacter);
                    CurrentLocationNo = CurrentLocationNo + 1;

                }
            }
        }

        public IList<InventoryLocation> ListInventoryLocations(long HMSInventoryId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                IList<InventoryLocation> InventoryLocationsInfo = context.InventoryLocation.Where(x => x.Id == HMSInventoryId).ToList();
                return InventoryLocationsInfo;
            }
        }
    }
}
