using fa.context;
using fa.model.Hms.common;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class VitalEntryManager
    {
        private static volatile VitalEntryManager instance;
        private static object syncRoot = new Object();
        VitalEntryManager()
        {

        }
        public static VitalEntryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new VitalEntryManager();
                    }
                }
                return instance;
            }
        }
        public Vital AddVital(Vital vital)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Vitals.Add(vital);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        vital = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }

            return vital;
        }
        
        public Vital UpdateVital(Vital Vital)
        {
            Vital VitalInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                VitalInfo = Context.Vitals.Find(Vital.Id);


                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (VitalInfo != null)
                        {
                            Context.Entry(VitalInfo).CurrentValues.SetValues(Vital);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        VitalInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return VitalInfo;
        }
        
        public Boolean DeleteVital(long VitalId)
        {
            bool deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Vital Vital = Context.Vitals.Where(p => p.Id == VitalId).First<Vital>();                      
                        Context.Vitals.Remove(Vital);
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
        
        public IList<Vital> ListVitalEntryByPatientId(long patientId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Vital> VitalInfo = (from Vital in Context.Vitals.Include("Patient") where Vital.PatientId == patientId select Vital).OrderByDescending(x=>x.Date).ToList();
                return VitalInfo;
            }
        }
        public Vital GetVitalEntryById(long Id)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Vital VitalInfo = (from Vital in Context.Vitals.Include("Patient") where Vital.Id == Id select Vital).First();
                return VitalInfo;
            }
        }

    }
}
