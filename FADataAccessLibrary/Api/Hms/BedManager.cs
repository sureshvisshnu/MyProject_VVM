using fa.context;
using fa.model.Hms.Master;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class BedManager
    {
        private static volatile BedManager instance;
        private static object syncRoot = new Object();
        BedManager()
        {

        }
        public static BedManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BedManager();
                    }
                }
                return instance;
            }
        }

        public BedType GetBedTypeById(long BedTypeId)
        {
            BedType BedTypeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        BedTypeInfo = Context.BedTypes.Include("Rents").FirstOrDefault(x => x.Id == BedTypeId);
                        return BedTypeInfo;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        BedTypeInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
        }
        public Boolean BedtypeNameUniqueById(BedType BedType)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BedType lBedType = null;
                if (BedType.Id == 0)
                {
                    lBedType = Context.BedTypes.FirstOrDefault(x => x.Name == BedType.Name && x.CompanyId == BedType.CompanyId);
                }
                else
                {
                    lBedType = Context.BedTypes.FirstOrDefault(x => x.Name == BedType.Name && x.CompanyId == BedType.CompanyId && !x.Id.Equals(BedType.Id));
                }
                if (lBedType != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
      
        public Boolean DeleteBedType(long BedTypeId)
        {
            Boolean Deleted = false;
            BedType BedTypeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Rent Rent = Context.Rents.FirstOrDefault(x => x.BedTypeId == BedTypeId);
                        if (Rent != null)
                        {
                            Context.Rents.Where(p => p.BedTypeId == BedTypeId).ToList().ForEach(p => Context.Rents.Remove(p));
                        }
                        BedTypeInfo = Context.BedTypes.Find(BedTypeId);
                        Context.BedTypes.Remove(BedTypeInfo);
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
        public BedType AddBedType(BedType bedType)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.BedTypes.Add(bedType);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        bedType = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return bedType;
        }
        public BedType UpdateBedType(BedType BedType)
        {
            BedType BedTypeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BedTypeInfo = Context.BedTypes.Find(BedType.Id);
               

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (BedTypeInfo != null)
                        {
                            Context.Entry(BedTypeInfo).CurrentValues.SetValues(BedType);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();

                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        BedTypeInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return BedTypeInfo;
        }
        public Boolean AddBedRentDetail(BedType BedType)
        {
            bool AddBedRentDetail = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AddBedRentDetail = true;
               
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        BedType BedTypeInfo = Context.BedTypes.Include("Rents").FirstOrDefault(x => x.Id == BedType.Id);
                        if (BedTypeInfo.Rents.Count > 0)
                        {
                            Rent Rent = Context.Rents.FirstOrDefault(x => x.BedTypeId == BedTypeInfo.Id);
                            if (Rent != null)
                            {
                                Context.Rents.Where(p => p.BedTypeId == BedTypeInfo.Id).ToList().ForEach(p => Context.Rents.Remove(p));
                                Context.SaveChanges();
                            }
                        }
                        if (BedType.Rents.Count > 0)
                        {
                            foreach (var Rent in BedType.Rents)
                            {
                                Context.Rents.Add(Rent);
                                Context.SaveChanges();
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        AddBedRentDetail = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
                return AddBedRentDetail;
            }
        }
        public BedType GetBedTypeByName(String BedTypeName, long CompanyId)
        {
            BedType BedTypeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                BedTypeInfo = Context.BedTypes.FirstOrDefault(x => x.Name == BedTypeName && x.CompanyId == CompanyId);
                return BedTypeInfo;
            }
        }
        public IList<BedType> ListBedTypeByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<BedType> BedTypeInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                         BedTypeInfo = (from BedType in Context.BedTypes where BedType.CompanyId == CompanyId select BedType).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        BedTypeInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return BedTypeInfo;
                }
            }
        }
    }
}
