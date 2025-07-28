using fa.context;
using fa.model.Accounting.Masters;
using Microsoft.EntityFrameworkCore;
using System;

namespace fa.api.Accounting
{
    public class SupplierManager
    {
        private static volatile SupplierManager instance;
        private static object syncRoot = new Object();
        SupplierManager()
        {

        }
        public static SupplierManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SupplierManager();
                    }
                }

                return instance;
            }
        }
        public Supplier GetSupplierById(long SupplierId)
        {
            Supplier SupplierInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SupplierInfo = Context.Suppliers.FirstOrDefault(x=>x.Id==SupplierId);
                if (SupplierInfo != null)
                {
                    SupplierInfo=Context.Suppliers.Include("ContactInfo").Include("Address").Include("SupplierLicenceDetail.CompanySupplierLicenseMaster").FirstOrDefault(x=>x.Id==SupplierId);
                }
                return SupplierInfo;
            }
        }
        public Supplier GetSupplierByName(String SupplierName, long CompanyId)
        {
            Supplier SupplierInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SupplierInfo = Context.Suppliers.FirstOrDefault(x => x.Name == SupplierName && x.CompanyId == CompanyId && x.AccountType==AccountType.SUPPLIER);
                return SupplierInfo;
            }
        }
        public IList<Supplier> ListSupplierByName(String SupplierName, long CompanyId)
        {
            IList<Supplier> SupplierInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SupplierInfo = Context.Suppliers.Include("AccountGroup").Include("Address").Include("ContactInfo").Where(x => x.CompanyId == CompanyId && (x.Name.Contains(SupplierName)|| x.ContactInfo.Phone.Contains(SupplierName)|| x.ContactInfo.Mobile.Contains(SupplierName))).ToList();
                return SupplierInfo;
            }
        }
        public Supplier CheckSupplierNameInUpdate(String SupplierName, long SupplierId, long CompanyId)
        {
            Supplier SupplierInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SupplierInfo = Context.Suppliers.FirstOrDefault(x => x.Name == SupplierName && x.CompanyId == CompanyId && !x.Id.Equals(SupplierId));
                return SupplierInfo;
            }
        }
        public Supplier CheckSupplierSubSupplier(long SupplierId)
        {
            Supplier SupplierInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SupplierInfo = Context.Suppliers.FirstOrDefault(x => x.ParentAccountId == SupplierId);
            }
            return SupplierInfo;
        }
        public Supplier CheckCompanyHaveSupplier(long CompanyId)
        {
            Supplier SupplierInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SupplierInfo = Context.Suppliers.FirstOrDefault(x => x.CompanyId == CompanyId);
            }
            return SupplierInfo;
        }
        public IList<Supplier> ListParentSupplierByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Supplier> SupplierInfo = (from Supplier in Context.Suppliers where Supplier.CompanyId == CompanyId where Supplier.ParentAccountId.Equals(null) select Supplier).ToList();
                return SupplierInfo;
            }
        }
        public IList<Supplier> ListSupplierByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Supplier> SupplierInfo = (from Supplier in Context.Suppliers.Include("Address").Include("ContactInfo").Include("SupplierLicenceDetail") where Supplier.CompanyId == CompanyId select Supplier).ToList();
                return SupplierInfo;
            }
        }
        public IList<Supplier> GetAllSupplier(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Supplier> SupplierInfo = (from c in Context.Suppliers.Include("AccountGroup").Include("Address").Include("ContactInfo") where c.CompanyId==CompanyId select c).ToList();
                return SupplierInfo;
            }
        }
        public bool DeleteSupplier(long SupplierId)
        {
            bool deleted = false;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Supplier supplier = context.Suppliers.Include("Address").Include("ContactInfo").Include("TaxInfo").Include("SupplierLicenceDetail").Where(p => p.Id == SupplierId).First<Supplier>();
                        if (supplier != null)
                        {
                            if (supplier.Address != null)
                            {
                                context.Addresses.Where(add => add.AddressId == supplier.Address.AddressId).ToList().ForEach(add => context.Addresses.Remove(add));
                            }
                            if (supplier.ContactInfo != null)
                            {
                                context.ContactInfos.Where(ci => ci.Id == supplier.ContactInfo.Id).ToList().ForEach(ci => context.ContactInfos.Remove(ci));
                            }
                            if (supplier.TaxInfo != null)
                            {
                                context.TaxInfos.Where(ti => ti.Id == supplier.TaxInfo.Id).ToList().ForEach(ti => context.TaxInfos.Remove(ti));
                            }

                            if (supplier.SupplierLicenceDetail.Count > 0)
                            {
                                context.SupplierLicenceDetails.Where(p => p.SupplierId == supplier.Id).ToList().ForEach(p => context.SupplierLicenceDetails.Remove(p));
                            }

                            //Delete all account transactions
                        }
                        context.Suppliers.Remove(supplier);
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        deleted = true;
                    }
                    #pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }
            return deleted;
        }
        public Supplier AddSupplier(Supplier supplier)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Suppliers.Add(supplier);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        supplier = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return supplier;
        }
        public Supplier UpdateSupplier(Supplier Supplier)
        {
            Supplier SupplierInfo = null;

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        SupplierInfo = Context.Suppliers.Find(Supplier.Id);

                        if (SupplierInfo != null)
                        {
                            Supplier SupplierInfoFromDB = Context.Suppliers
                                .Include("ContactInfo")
                                .Include("Address")
                                .Include("SupplierLicenceDetail")
                                .Include("SupplierProducts") // ✅ Include products
                                .FirstOrDefault(x => x.Id == Supplier.Id);

                            // 🔁 Sync SupplierLicenceDetail
                            foreach (SupplierLicenceDetail OldInfo in SupplierInfoFromDB.SupplierLicenceDetail.ToList())
                            {
                                SupplierLicenceDetail NewInfo = Supplier.SupplierLicenceDetail
                                    .FirstOrDefault(x => x.SupplierLicenceId == OldInfo.SupplierLicenceId);

                                if (NewInfo == null)
                                {
                                    Context.SupplierLicenceDetails.Remove(OldInfo);
                                }
                                else
                                {
                                    Supplier.SupplierLicenceDetail.Remove(NewInfo);
                                    NewInfo.SupplierId = Supplier.Id;
                                    Context.Entry(Context.SupplierLicenceDetails.Find(NewInfo.SupplierLicenceId)).CurrentValues.SetValues(NewInfo);
                                }

                                Context.SaveChanges();
                            }

                            // ✅ Sync SupplierProducts
                            Context.SupplierProducts.RemoveRange(SupplierInfoFromDB.SupplierProducts);
                            Context.SaveChanges();

                            foreach (var productLink in Supplier.SupplierProducts)
                            {
                                productLink.SupplierId = Supplier.Id;
                                Context.SupplierProducts.Add(productLink);
                            }
                            Context.SaveChanges();

                            // 📝 Update Supplier base info
                            Context.Entry(SupplierInfo).CurrentValues.SetValues(Supplier);
                            Context.SaveChanges();

                            // ➕ Add remaining new licence details
                            foreach (SupplierLicenceDetail LInfo in Supplier.SupplierLicenceDetail)
                            {
                                LInfo.SupplierId = Supplier.Id;
                                Context.SupplierLicenceDetails.Add(LInfo);
                                Context.SaveChanges();
                            }

                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        SupplierInfo = null;
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }

            return SupplierInfo;
        }

        public Supplier UpdateSupplierxx(Supplier Supplier)
        {
            Supplier SupplierInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        SupplierInfo = Context.Suppliers.Find(Supplier.Id);
                        if (SupplierInfo != null)
                        {
                            Supplier SupplierInfoFromDB = Context.Suppliers.Include("ContactInfo").Include("Address").Include("SupplierLicenceDetail").FirstOrDefault(x => x.Id == Supplier.Id);
                            foreach (SupplierLicenceDetail OldInfo in SupplierInfoFromDB.SupplierLicenceDetail)
                            {
                                SupplierLicenceDetail NewInfo = Supplier.SupplierLicenceDetail.FirstOrDefault(x => x.SupplierLicenceId == OldInfo.SupplierLicenceId);
                                if (NewInfo == null)
                                {
                                    Context.SupplierLicenceDetails.Remove(OldInfo);
                                }
                                else
                                {
                                    Supplier.SupplierLicenceDetail.Remove(NewInfo);
                                    NewInfo.SupplierId = Supplier.Id;
                                    Context.Entry(Context.SupplierLicenceDetails.Find(NewInfo.SupplierLicenceId)).CurrentValues.SetValues(NewInfo);
                                }
                                Context.SaveChanges();
                            }
                            Context.Entry(SupplierInfo).CurrentValues.SetValues(Supplier);
                            Context.SaveChanges();        
                            foreach (SupplierLicenceDetail LInfo in Supplier.SupplierLicenceDetail)
                            {
                                LInfo.SupplierId = Supplier.Id;
                                Context.SupplierLicenceDetails.Add(LInfo);
                                Context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        SupplierInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return SupplierInfo;
        }
        public static Supplier GetSupplierWithProductsById(long supplierId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                return Context.Suppliers
                    .Include("SupplierProducts.Product")
                    .FirstOrDefault(x => x.Id == supplierId);
            }
        }

    }
}
