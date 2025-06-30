using fa.context;
using fa.model.Common;
using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class PatientManager
    {
        private static volatile PatientManager instance;
        private static object syncRoot = new Object();
        PatientManager()
        {

        }
        public static PatientManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PatientManager();
                    }
                }

                return instance;
            }
        }

        public Patient GetPatientByNo(string PatientNo)
        {
            Patient PatientInfoByNo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfoByNo = Context.Patients.Include("Address").Include("ContactInfo").Include("EmergencyContact").Include("InsuranceInfo").Include("Guardians").Where(x => x.PatientNumber==PatientNo).FirstOrDefault<Patient>();
                //PatientInfoByNo = Context.Patients.Where(x => x.PatientNumber == PatientNo).FirstOrDefault<Patient>();
                return PatientInfoByNo;
            }
        }
        public Patient GetPatientById(long PatientId)
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Include("EmergencyContact").Include("InsuranceInfo").Include("InsuranceInfo.InsuranceHolder").Include("Guardians").FirstOrDefault(x=>x.Id==PatientId);
                return PatientInfo;
            }
        }
        public Patient GetLastPatient()
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Include("EmergencyContact").Include("InsuranceInfo").Include("Guardians").OrderByDescending(x=> x.Id).First();
                return PatientInfo;
            }
        }
        public Patient GetPatientNoById(long Id, long CompanyId)
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Include("EmergencyContact").Include("InsuranceInfo").Include("Guardians").Where(x => x.Id == Id && x.CompanyId == CompanyId).First<Patient>();
                return PatientInfo;
            }
        }
        public Patient GetPatientInfo(long Id)
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Find(Id);
            }
            return PatientInfo;
        }
        public Patient GetPatientByName(string Name, long CompanyId)
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => (x.FirstName.Contains(Name) || x.MiddleInitial.Contains(Name) || x.LastName.Contains(Name) || (x.FirstName + (string.IsNullOrEmpty(x.MiddleInitial) ? "" : " ") + x.MiddleInitial + (string.IsNullOrEmpty(x.LastName) ? "" : " ") + x.LastName).Contains(Name) || x.PatientNumber.Contains(Name)) && x.CompanyId == CompanyId).FirstOrDefault<Patient>();
                 //   PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.FirstName.Contains(Name) || x.MiddleInitial.Contains(Name) || x.LastName.Contains(Name)).ToList<Patient>();

                }
                return PatientInfo;
            }
        }
        public IList<Patient> ListAllPatient(long CompanyId)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Include("EmergencyContact").Include("EmergencyContact.ContactInfo").Include("InsuranceInfo").Include("Guardians").Include("Guardians.ContactInfo").Where(x => x.CompanyId == CompanyId).ToList<Patient>();
                return PatientInfo;
            }
        }
        public IList<Patient> ListAllPatientWithSearchString(long CompanyId, string filtertext)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (string.IsNullOrEmpty(filtertext))
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.CompanyId == CompanyId).ToList<Patient>();
                }
                else
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.CompanyId == CompanyId && (x.PatientNumber.Contains(filtertext) || x.FirstName.Contains(filtertext) || x.MiddleInitial.Contains(filtertext) || x.LastName.Contains(filtertext) || (x.ContactInfo != null && (x.ContactInfo != null && (x.ContactInfo.Phone != null && x.ContactInfo.Phone.Contains(filtertext)) || (x.ContactInfo.Mobile != null && x.ContactInfo.Mobile.Contains(filtertext)))))).ToList<Patient>();
                }
                return PatientInfo;
            }
        }
        public IList<Patient> ListAllIpPatient()
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x=>(Context.InPatientAdmissions.Where(i => i.Status != model.Hms.Ip.InPatientStatus.DISCHARGED).Select(j => j.Patient)).ToList().Contains(x)).ToList<Patient>();
                return PatientInfo;
            }
        }
        public IList<Patient> GetPatientBySearchQuery(string Name, DateTime? DOB)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (!string.IsNullOrEmpty(Name) && DOB != null)
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => (x.FirstName.Contains(Name) || x.MiddleInitial.Contains(Name) || x.LastName.Contains(Name)) && x.DateOfBirth == DOB).ToList<Patient>();
                }
                if (!string.IsNullOrEmpty(Name) && DOB == null)
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.FirstName.Contains(Name) || x.MiddleInitial.Contains(Name) || x.LastName.Contains(Name)).ToList<Patient>();
                }
                if (string.IsNullOrEmpty(Name) && DOB != null)
                {
                     PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x =>x.DateOfBirth==DOB).ToList<Patient>();
                }               
                return PatientInfo;
            }
        }

        public IList<Patient> GetPatientByDOB(DateTime DOB,long CompanyId, PatientSearchType PatientSearchType)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (PatientSearchType==PatientSearchType.Patient)
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.DateOfBirth == DOB.Date && x.CompanyId == CompanyId).ToList<Patient>();
                }
                else
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.DateOfBirth == DOB.Date && x.CompanyId == CompanyId && (Context.InPatientAdmissions.Where(i => i.Status != model.Hms.Ip.InPatientStatus.DISCHARGED).Select(j => j.Patient)).ToList().Contains(x)).ToList<Patient>();
                }
                return PatientInfo;
            }
        }
        public IList<Patient> GetPatientContainsPrefix(string prefix, long CompanyId)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.CompanyId == CompanyId && x.PatientNumber.Contains(prefix)).ToList<Patient>();
                }
                return PatientInfo;
            }
        }
        public IList<Patient> GetPatientById(string Id, long CompanyId)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.PatientNumber.Equals(Id) && x.CompanyId == CompanyId).ToList<Patient>(); 
                return PatientInfo;
            }
        }

        public IList<Patient> GetPatientByName(string Name, long CompanyId, PatientSearchType PatientSearchType)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (PatientSearchType == PatientSearchType.Patient)
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => (x.FirstName.Contains(Name) || x.MiddleInitial.Contains(Name) || x.LastName.Contains(Name) || (x.FirstName + (string.IsNullOrEmpty(x.MiddleInitial) ? "" : " ") + x.MiddleInitial + (string.IsNullOrEmpty(x.LastName) ? "" : " ") + x.LastName).Contains(Name) || x.PatientNumber.Contains(Name)) && x.CompanyId == CompanyId).ToList<Patient>();
                }
                else
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => (x.FirstName.Contains(Name) || x.MiddleInitial.Contains(Name) || x.LastName.Contains(Name) || (x.FirstName + (string.IsNullOrEmpty(x.MiddleInitial) ? "" : " ") + x.MiddleInitial + (string.IsNullOrEmpty(x.LastName) ? "" : " ") + x.LastName).Contains(Name) || x.PatientNumber.Contains(Name)) && x.CompanyId == CompanyId && (Context.InPatientAdmissions.Where(i => i.Status != model.Hms.Ip.InPatientStatus.DISCHARGED).Select(j => j.Patient)).ToList().Contains(x)).ToList<Patient>();
                }

                return PatientInfo;
            }
        }
        public IList<Patient> GetPatientByPhoneNumber(string PhoneNumber, long CompanyId, PatientSearchType PatientSearchType)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (PatientSearchType == PatientSearchType.Patient)
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.CompanyId == CompanyId && (x.ContactInfo.Phone.Contains(PhoneNumber) || x.ContactInfo.Mobile.Contains(PhoneNumber))).ToList<Patient>();
                }
                else
                {
                    PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.CompanyId == CompanyId && (x.ContactInfo.Phone.Contains(PhoneNumber) || x.ContactInfo.Mobile.Contains(PhoneNumber)) && (Context.InPatientAdmissions.Where(i => i.Status != model.Hms.Ip.InPatientStatus.DISCHARGED).Select(j => j.Patient)).ToList().Contains(x)).ToList<Patient>();
                }
                return PatientInfo;
            }
        }

        public Boolean DeletePatient(long PatientId)
        {
            bool deleted = false;
            Patient Patient = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Patient = GetPatientById(PatientId);
                        if (Patient.ResponsiblePartyId != null)
                        {
                            Patient.ResponsiblePartyId = null;
                            Patient=UpdatePatient(Patient);
                        }

                        if (Patient != null)
                        {
                          
                            if (Patient.InsuranceInfo.Count > 0)
                            {
                                foreach (var lInsuranceInfo in Patient.InsuranceInfo)
                                {
                                    InsuranceInfoManager InsuranceInfoManager = InsuranceInfoManager.Instance;
                                    InsuranceInfoManager.DeleteInsuranceInfo(lInsuranceInfo.Id);
                                }
                            }
                            if (Patient.Guardians.Count > 0)
                            {
                                foreach (var lGuardian in Patient.Guardians)
                                {
                                    GuardianManager GuardianManager = GuardianManager.Instance;
                                    GuardianManager.DeleteGuardian(lGuardian.Id);
                                }
                            }
                            if (Patient.EmergencyContact.Count > 0)
                            {
                                foreach (var lEmergencyContact in Patient.EmergencyContact)
                                {
                                    EmergencyContactManager EmergencyContactManager = EmergencyContactManager.Instance;
                                    EmergencyContactManager.DeleteEmergencyContact(lEmergencyContact.Id);
                                }
                            }
                            if (Patient.Address != null)
                            {
                                Context.Addresses.Where(add => add.AddressId == Patient.Address.AddressId).ToList().ForEach(add => Context.Addresses.Remove(add));
                            }
                            if (Patient.ContactInfo != null)
                            {
                                Context.ContactInfos.Where(ci => ci.Id == Patient.ContactInfo.Id).ToList().ForEach(ci => Context.ContactInfos.Remove(ci));
                            }
                       
                        Context.Patients.Remove(GetPatientById(PatientId));
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        deleted = true;
                        }
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
        public Patient AddPatient(Patient patient)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Patients.Add(patient);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        patient = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return patient;
        }
        public Patient UpdateFromCossultingPatient(Patient Patient)
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Find(Patient.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (PatientInfo != null)
                        {
                            PatientInfo.OtherNotes = Patient.OtherNotes;
                            Context.Entry(PatientInfo).CurrentValues.SetValues(PatientInfo);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PatientInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return PatientInfo;
        }
        public Patient UpdatePatient(Patient Patient)
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Find(Patient.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (PatientInfo != null)
                        {
                            Context.Entry(PatientInfo).CurrentValues.SetValues(Patient);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PatientInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return PatientInfo;
        }
        public Patient UpdatePatientNo(string PatientNo, long PatientId)
        {
            Patient PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Find(PatientId);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (PatientInfo != null)
                        {
                            PatientInfo.PatientNumber = PatientNo;
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PatientInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }                    
            }
            return PatientInfo;
        }
        public Patient UpdatePatient(Patient Patient, AccountMasterContext Context)
        {
            Patient PatientInfo = null;           
            PatientInfo = Context.Patients.Find(Patient.Id);                
            try
            {
                if (PatientInfo != null)
                {
                    Context.Entry(PatientInfo).CurrentValues.SetValues(Patient);
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PatientInfo = null;
                throw (e);
            }           
            return PatientInfo;
        }

        public IList<Patient> GetPatientBySearchText(string text, long companyId)
        {
            IList<Patient> PatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientInfo = Context.Patients.Include("Address").Include("ContactInfo").Where(x => x.CompanyId == companyId && x.PatientNumber.Contains(text)).ToList<Patient>();
                return PatientInfo;
            }
        }
        public bool CheckPatientUniqueByPatientName(string Name, DateTime DoB, string PatientNumber, long companyID, string format)
        {
            bool status = true;
            Patient patient = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                patient = Context.Patients.AsEnumerable().Where(x => x.CompanyId == companyID && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && x.DateOfBirth.Date.ToShortTimeString() == DoB.Date.ToShortTimeString() && x.PatientNumber == PatientNumber).FirstOrDefault<Patient>();
                if (patient != null) 
                {
                    status = false;
                }
            }
            return status;
        }

        public Patient GetPatientByPatientNumber(string Name, DateTime DoB, string number, long companyID, string format)
        {
            Patient patient = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                patient = Context.Patients.Include("Address").AsEnumerable().Where(x => x.CompanyId == companyID && x.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && x.DateOfBirth.Date.ToShortTimeString() == DoB.Date.ToShortTimeString() && x.PatientNumber == number).FirstOrDefault<Patient>();
            }
            return patient;
        }

        public Address GetAddressById(long? addressId)
        {
            Address laddress = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                laddress = Context.Addresses.Include("State").FirstOrDefault(x => x.AddressId == addressId);
            }
            return laddress;
        }
    }
}
