using fa.context;
using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class EmergencyContactManager
    {
        private static volatile EmergencyContactManager instance;
        private static object syncRoot = new Object();
        EmergencyContactManager()
        {

        }
        public static EmergencyContactManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new EmergencyContactManager();
                    }
                }

                return instance;
            }
        }

        public EmergencyContact GetEmergencyContactById(long EmergencyContactId)
        {
            EmergencyContact EmergencyContactInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                EmergencyContactInfo = Context.EmergencyContacts.Include("Address").Include("ContactInfo").FirstOrDefault(x => x.Id == EmergencyContactId);
                return EmergencyContactInfo;
            }
        }
        public IList<EmergencyContact> ListAllEmergencyContactByPatientId(long PatientId)
        {
            IList<EmergencyContact> EmergencyContactInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                EmergencyContactInfo = (from EmergencyContact in Context.EmergencyContacts.Include("Address").Include("ContactInfo") where EmergencyContact.PatientId == PatientId select EmergencyContact).ToList();
                return EmergencyContactInfo;
            }
        }
        public EmergencyContact AddEmergencyContact(EmergencyContact emergencyContact)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.EmergencyContacts.Add(emergencyContact);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        emergencyContact = null;
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            return emergencyContact;
        }
        public EmergencyContact UpdateEmergencyContact(EmergencyContact EmergencyContact)
        {
            EmergencyContact EmergencyContactInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        EmergencyContactInfo = Context.EmergencyContacts.Find(EmergencyContact.Id);
                        if (EmergencyContactInfo != null)
                        {
                            Context.Entry(EmergencyContactInfo).CurrentValues.SetValues(EmergencyContact);
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        EmergencyContactInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }

                }
            }
            return EmergencyContactInfo;
        }
        public Boolean DeleteEmergencyContact(long EmergencyContactId)
        {
            bool deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        EmergencyContact EmergencyContact = Context.EmergencyContacts.Include("Address").Include("ContactInfo").Where(p => p.Id == EmergencyContactId).First<EmergencyContact>();
                        if (EmergencyContact != null)
                        {
                            if (EmergencyContact.Address != null)
                            {
                                Context.Addresses.Where(add => add.AddressId == EmergencyContact.Address.AddressId).ToList().ForEach(add => Context.Addresses.Remove(add));
                            }
                            if (EmergencyContact.ContactInfo != null)
                            {
                                Context.ContactInfos.Where(ci => ci.Id == EmergencyContact.ContactInfo.Id).ToList().ForEach(ci => Context.ContactInfos.Remove(ci));
                            }

                        }
                        Context.EmergencyContacts.Remove(EmergencyContact);
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

        public bool CheckEmergencyContactUniqueByName(string Name, long PatientId, RelationShip RelationShip, long companyId)
        {
            bool status = true;
            EmergencyContact EmergencyContactInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                EmergencyContactInfo = Context.EmergencyContacts.AsEnumerable().Where(x => x.CompanyId == companyId && x.PatientId == PatientId && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && x.RelationShip == (RelationShip)RelationShip).FirstOrDefault<EmergencyContact>();
                if (EmergencyContactInfo != null)
                {
                    status = false;
                }
            }
            return status;
        }

        public EmergencyContact GetEmergencyContactByName(string Name, long PatientId, RelationShip RelationShip, long companyId)
        {
            EmergencyContact EmergencyContactInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                EmergencyContactInfo = Context.EmergencyContacts.Include("Address").AsEnumerable().Where(x => x.CompanyId == companyId && x.PatientId == PatientId && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && x.RelationShip == (RelationShip)RelationShip).FirstOrDefault<EmergencyContact>();
            }
            return EmergencyContactInfo;
        }

        //public Boolean AddEmergencyContactDetail(Patient Patient)
        //{
        //    bool AddEmergencyContactDetail = false;
        //    using (AccountMasterContext Context = new AccountMasterContext())
        //    {
        //        AddEmergencyContactDetail = true;
        //        EmergencyContact EmergencyContactInfo = Context.EmergencyContacts.FirstOrDefault(x => x.PatientId == Patient.Id);
        //        if (EmergencyContactInfo != null)
        //        {
        //            Context.EmergencyContacts.Where(p => p.PatientId == Patient.Id).ToList().ForEach(p => Context.EmergencyContacts.Remove(p));
        //            Context.SaveChanges();
        //        }
        //        if (Patient.EmergencyContact.Count > 0)
        //        {
        //            foreach (var EmergencyContactDetails in Patient.EmergencyContact)
        //            {
        //                Context.EmergencyContacts.Add(EmergencyContactDetails);
        //                Context.SaveChanges();
        //            }
        //        }
        //        return AddEmergencyContactDetail;
        //    }
        //}














    }
}
