using fa.context;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class GuardianManager
    {
        private static volatile GuardianManager instance;
        private static object syncRoot = new Object();
        GuardianManager()
        {

        }
        public static GuardianManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GuardianManager();
                    }
                }

                return instance;
            }
        }

        public Guardian GetGuardianById(long GuardianId)
        {
            Guardian GuardianInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                GuardianInfo = Context.Guardians.Include("Address").Include("ContactInfo").FirstOrDefault(x=>x.Id==GuardianId);
                return GuardianInfo;
            }
        }
        public IList<Guardian> ListAllGuardianByPatientId(long PatientId)
        {
            IList<Guardian> GuardianInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                GuardianInfo = (from Guardian in Context.Guardians.Include("Address").Include("ContactInfo").Include("Patient") where Guardian.PatientId == PatientId select Guardian).ToList();
                return GuardianInfo;
            }
        }
        public Guardian AddGuardian(Guardian guardian)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Guardians.Add(guardian);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        guardian = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }

            return guardian;
        }
        public Guardian UpdateGuardian(Guardian Guardian)
        {
            Guardian GuardianInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                GuardianInfo = Context.Guardians.Find(Guardian.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (GuardianInfo != null)
                        {
                            Context.Entry(GuardianInfo).CurrentValues.SetValues(Guardian);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        GuardianInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return GuardianInfo;
        }
        public Boolean DeleteGuardian(long GuardianId)
        {
            bool deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Guardian Guardian = Context.Guardians.Include("Address").Include("ContactInfo").Where(p => p.Id == GuardianId).First<Guardian>();
                        if (Guardian != null)
                        {
                            if (Guardian.Address != null)
                            {
                                Context.Addresses.Where(add => add.AddressId == Guardian.Address.AddressId).ToList().ForEach(add => Context.Addresses.Remove(add));
                            }
                            if (Guardian.ContactInfo != null)
                            {
                                Context.ContactInfos.Where(ci => ci.Id == Guardian.ContactInfo.Id).ToList().ForEach(ci => Context.ContactInfos.Remove(ci));
                            }

                        }
                        Context.Guardians.Remove(Guardian);
                        Context.SaveChanges();
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

        //public Boolean AddGuardianDetail(Patient Patient)
        //{
        //    bool AddGuardianDetail = false;
        //    using (AccountMasterContext Context = new AccountMasterContext())
        //    {
        //        AddGuardianDetail = true;
        //        Guardian GuardianInfo = Context.Guardians.FirstOrDefault(x => x.PatientId == Patient.Id);
        //            if(GuardianInfo != null)
        //            {
        //                Context.Guardians.Where(p =>p.PatientId == Patient.Id).ToList().ForEach(p => Context.Guardians.Remove(p));
        //                Context.SaveChanges();
        //            }                
        //        if (Patient.Guardians.Count > 0)
        //        {
        //            foreach (var GuardianDetails in Patient.Guardians)
        //            {
        //                Context.Guardians.Add(GuardianDetails);
        //                Context.SaveChanges();
        //            }
        //        }
        //        return AddGuardianDetail;
        //    }
        //}
        public bool CheckGuardiantuniqueByName(string Name, long PatientId, RelationShip Relationship, long companyID)
        {
            bool status = true;
            Guardian GuardianInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                GuardianInfo = Context.Guardians.AsEnumerable().Where(x => x.CompanyId == companyID && x.PatientId == PatientId && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && x.RelationShip == (RelationShip)Relationship).FirstOrDefault<Guardian>();
                if (GuardianInfo != null)
                {
                    status = false;
                }
            }
            return status;
        }

        public Guardian GetGuardianByName(string Name, long PatientId, RelationShip Relationship, long companyID)
        {
            Guardian GuardianInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                GuardianInfo = Context.Guardians.Include("Address").AsEnumerable().Where(x => x.CompanyId == companyID && x.PatientId == PatientId && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && x.RelationShip == (RelationShip)Relationship).FirstOrDefault<Guardian>();
            }
            return GuardianInfo;
        }
    }
}
