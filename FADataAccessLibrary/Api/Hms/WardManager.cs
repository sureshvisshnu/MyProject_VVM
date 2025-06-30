using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.ComponentModel.Design;
using System.Linq;

namespace fa.api.Hms
{
    public class WardManager
    {
        private static volatile WardManager instance;
        private static object syncRoot = new Object();
        WardManager()
        {

        }
        public static WardManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new WardManager();
                    }
                }
                return instance;
            }
        }

        public Ward GetWardById(long WardId)
        {
            Ward WardInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                WardInfo = Context.Wards.Include("Beds").FirstOrDefault(x => x.Id == WardId);
                return WardInfo;
            }
        }       
        public Ward GetWardByLocationId(long Id)
        {
            Ward WardInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                WardInfo = Context.Wards.Include("Beds").FirstOrDefault(x => x.InventoryLocationId == Id);
                return WardInfo;
            }
        }
        public Boolean WardNameUniqueById(Ward Ward)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    Ward lWard = null;
                    if (Ward.Id == 0)
                    {
                        lWard=Context.Wards.FirstOrDefault(x => x.Name == Ward.Name && x.CompanyId == Ward.CompanyId);
                    }
                    else
                    {
                        lWard=Context.Wards.FirstOrDefault(x => x.Name == Ward.Name && x.CompanyId == Ward.CompanyId && !x.Id.Equals(Ward.Id));
                    }
                    if(lWard != null)
                    {
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
        
        public Boolean DeleteWard(long WardId)
        {
            Boolean Deleted = false;
            Ward WardInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Bed Bed = Context.Beds.FirstOrDefault(x => x.WardId == WardId);
                        if (Bed != null)
                        {
                            Context.Beds.Where(p => p.WardId == WardId).ToList().ForEach(p => Context.Beds.Remove(p));
                        }
                        WardInfo = Context.Wards.Find(WardId);
                        Context.Wards.Remove(WardInfo);
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
        public Ward AddWard(Ward ward)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Wards.Add(ward);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ward = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ward;
        }
        public Ward UpdateWard(Ward Ward)
        {
            Ward WardInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        WardInfo = Context.Wards.Find(Ward.Id);
                        if (WardInfo != null)
                        {
                            Ward WardInfoFromDB = GetWardById(Ward.Id);
                            if (Ward.MaintainInventory && Ward.InventoryLocationId==null)
                            {                               
                                if (Ward.InventoryLocation != null && !string.IsNullOrEmpty(Ward.InventoryLocation.Name))
                                {
                                    Context.InventoryLocation.Add(Ward.InventoryLocation);
                                }
                            }
                            Context.Entry(WardInfo).CurrentValues.SetValues(Ward);
                            Context.SaveChanges();

                            foreach (Bed OldBed in WardInfoFromDB.Beds)
                            {
                                Bed NewBed = Ward.Beds.FirstOrDefault(x => x.Id == OldBed.Id);
                                if (NewBed == null)
                                {
                                    Context.Beds.Remove(Context.Beds.FirstOrDefault(x=>x.Id==OldBed.Id));
                                }
                                else
                                {
                                    Ward.Beds.Remove(NewBed);
                                    NewBed.WardId = Ward.Id;
                                    NewBed.BedStatus = OldBed.BedStatus;
                                    Context.Entry(NewBed).State = EntityState.Modified;
                                }
                                Context.SaveChanges();
                            }
                            foreach (Bed Bed in Ward.Beds)
                            {
                                Context.Wards.Include("Beds").FirstOrDefault(x => x.Id == WardInfoFromDB.Id).Beds.Add(Bed);
                                Context.SaveChanges();
                            }
                            
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        WardInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return WardInfo;
        }
        public Boolean AddBedDetail(Ward Ward)
        {
            bool AddBedBedDetail = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AddBedBedDetail = true;



                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Ward WardInfo = Context.Wards.Include("Beds").FirstOrDefault(x => x.Id == Ward.Id);

                        if (WardInfo.Beds.Count > 0)
                        {
                            Bed Bed = Context.Beds.FirstOrDefault(x => x.WardId == WardInfo.Id);
                            if (Bed != null)
                            {
                                Context.Beds.Where(p => p.WardId == WardInfo.Id).ToList().ForEach(p => Context.Beds.Remove(p));
                                Context.SaveChanges();
                            }
                        }
                        if (Ward.Beds.Count > 0)
                        {
                            foreach (var Bed in Ward.Beds)
                            {
                                Context.Beds.Add(Bed);
                                Context.SaveChanges();
                            }
                        }
                        dbContextTransaction.Commit();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        AddBedBedDetail = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }

                return AddBedBedDetail;
            }
        }
        public IList<Ward> ListWardByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Ward> WardInfo = (from Ward in Context.Wards.Include("Beds") where Ward.CompanyId == CompanyId select Ward).ToList();
                return WardInfo;
            }
        }
        public IList<Ward> ListWardByCompanyIdFilter(long CompanyId,string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var Filters = new MySqlParameter("@Filter", "%" + Filter + "%");
                var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                string Query = "select* FROM Wards where CompanyId = @CompanyIds and Name like @Filter union SELECT *FROM Wards WHERE Id IN(select WardId FROM beds where CompanyId = @CompanyIds and Name like @Filter)";
                IList <Ward> WardInfo = Context.Wards.FromSqlRaw(Query, Filters, companyId).ToList();
                return WardInfo;
            }
        }
        //-----------------------------------------WardBed---------------------------------------------------------------------
        public Bed GetWardBedById(long BedId)
        {
            Bed WardBedInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                WardBedInfo = Context.Beds.Include("Ward").Include("BedType").Include("BedType.Rents").FirstOrDefault(x => x.Id == BedId);
                return WardBedInfo;
            }
        }
        public Boolean BedNameUniqueById(Bed Bed)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    Bed lBed = null;
                    if (Bed.Id == 0)
                    {
                        lBed=Context.Beds.FirstOrDefault(x => x.Name == Bed.Name && x.WardId == Bed.WardId && x.CompanyId == Bed.CompanyId);
                    }
                    else
                    {
                        lBed=Context.Beds.FirstOrDefault(x => x.Name == Bed.Name && x.WardId == Bed.WardId && x.CompanyId == Bed.CompanyId && !x.Id.Equals(Bed.Id));
                    }
                    if (lBed != null)
                    {
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
       
        public Boolean DeleteWardBed(long WardBedId)
        {
            Boolean Deleted = false;
            Bed WardBedInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {                       
                        WardBedInfo = Context.Beds.Find(WardBedId);
                        Context.Beds.Remove(WardBedInfo);
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
        public Bed AddWardBed(Bed wardBed)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Beds.Add(wardBed);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        wardBed = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return wardBed;
        }
        public Bed UpdateWardBed(Bed WardBed)
        {
            Bed WardBedInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                WardBedInfo = Context.Beds.Find(WardBed.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (WardBedInfo != null)
                        {
                            WardBed.BedStatus = WardBedInfo.BedStatus;
                            Context.Entry(WardBedInfo).CurrentValues.SetValues(WardBed);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        WardBedInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return WardBedInfo;
        }

        public void UpdateWardBedFromIpAdmission(long BedId,BedStatus BedStatus,AccountMasterContext Context)
        {
            Bed WardBedInfo  = Context.Beds.Find(BedId);
            try
            {
                if (WardBedInfo != null)
                {
                    WardBedInfo.BedStatus = BedStatus;
                    Context.Entry(Context.Beds.Find(WardBedInfo.Id)).CurrentValues.SetValues(WardBedInfo);
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                WardBedInfo = null;
                throw (e);
            }
        }
        public IList<Bed> ListWardBedByWardId(long WardId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Bed> WardBedInfo = (from WardBed in Context.Beds.Include("Ward").Include("BedType") where WardBed.WardId == WardId && WardBed.BedStatus==BedStatus.AVAILABLE select WardBed).ToList();
                return WardBedInfo;
            }
        }
        public IList<Bed> ListAllWardBedByCompanyIdFilter(long CompanyId,string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Bed> WardBedInfo = (from WardBed in Context.Beds.Include("Ward").Include("BedType") where WardBed.CompanyId == CompanyId && WardBed.Name.Contains(Filter)  select WardBed).ToList();
                return WardBedInfo;
            }
        }

    }
}
