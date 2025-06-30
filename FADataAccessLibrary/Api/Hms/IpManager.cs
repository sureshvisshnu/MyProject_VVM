using fa.context;
using fa.model.Hms.Ip;
using fa.model.Hms.Master;
using fa.model.Hms.Op;
using fa.model.hms.config;
using fa.model.Hms.common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FADataAccessLibrary.Model.Hms.common;
using FADataAccessLibrary.Api.Hms;
using fa.Data;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace fa.api.Hms
{
    public class IpManager
    {
        private static volatile IpManager instance;
        private static object syncRoot = new Object();
        IpManager()
        {

        }
        public static IpManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new IpManager();
                    }
                }
                return instance;
            }
        }
        public static string FeachPatientIPID(long CompanyId, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientIPId lPatientIPId = Context.PatientIPIds.FirstOrDefault(x => x.CompanyId == CompanyId && x.Date == Date);
                if (lPatientIPId == null)
                {
                    lPatientIPId = new PatientIPId
                    {
                        CompanyId = CompanyId,
                        Date = Date,
                        NextNumber = 1
                    };
                    Context.PatientIPIds.Add(lPatientIPId);
                    Context.SaveChanges();
                }
                return lPatientIPId.PatientIPNo;
            }
        }
        public static void GenerateNextPatientIPID(long CompanyId, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientIPId lPatientIPId = Context.PatientIPIds.FirstOrDefault(x => x.CompanyId == CompanyId && x.Date == Date);
                if (lPatientIPId != null)
                {
                    int CurrentPatientNo = lPatientIPId.NextNumber;
                    lPatientIPId.NextNumber = CurrentPatientNo + 1;
                    Context.Entry(lPatientIPId).CurrentValues.SetValues(lPatientIPId);
                    Context.SaveChanges();

                }
            }
        }
        public InPatientAdmission GetAdmittedInPatientAdmissionByPatientId(long PatientId)
        {
            InPatientAdmission InPatientAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmissionInfo = Context.InPatientAdmissions.Include("PatientLocationHistory").FirstOrDefault(x => x.PatientId == PatientId && x.Status!= InPatientStatus.DISCHARGED);               
                return InPatientAdmissionInfo;
            }
        }
        public InPatientAdmission GetInPatientAdmissionById(long InPatientAdmissionId)
        {
            InPatientAdmission InPatientAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmissionInfo = Context.InPatientAdmissions.Include("PatientLocationHistory").Include("OpRegistration").FirstOrDefault(x => x.Id == InPatientAdmissionId);
                return InPatientAdmissionInfo;
            }
        }
        public IList<InPatientAdmission> GetListofInPatientAdmissionByCompanyIdDate(long CompanyIds, DateTime From, DateTime To)
        {
            IList<InPatientAdmission> IpAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IpAdmissionInfo = Context.InPatientAdmissions
                                   .Where(admission => admission.CompanyId == CompanyIds
                                                    && admission.DateOfAdmission >= From
                                                    && admission.DateOfAdmission <= To)
                                   .ToList();
                return IpAdmissionInfo;
            }
        }
        public InPatientAdmission GetUnDischargedInPatientAdmissionByPatientId(long PatienId)
        {
            InPatientAdmission InPatientAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmissionInfo = Context.InPatientAdmissions.FirstOrDefault(x => x.PatientId == PatienId && x.Status!= InPatientStatus.DISCHARGED);
                if (InPatientAdmissionInfo != null)
                {
                    if (InPatientAdmissionInfo.PatientLocationHistory == null)
                    {
                        InPatientAdmissionInfo.PatientLocationHistory = Context.InPatientLocations.Where(x => x.AdmissionId == InPatientAdmissionInfo.Id).ToList();
                    }
                    if (InPatientAdmissionInfo.MedicalTeamHistory == null)
                    {
                        InPatientAdmissionInfo.MedicalTeamHistory = Context.MedicalTeams.Where(x => x.AdmissionId == InPatientAdmissionInfo.Id).ToList();
                    }
                }
                return InPatientAdmissionInfo;
            }
        }
        public InPatientAdmission GetAdmittedInPatientAdmissionByOpId(long OPId)
        {
            InPatientAdmission InPatientAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmissionInfo = Context.InPatientAdmissions.Include("PatientLocationHistory").Include("PatientLocationHistory.Ward").Include("PatientLocationHistory.Bed").Include("MedicalTeamHistory").Include("MedicalTeamHistory.PrimaryDoctor").FirstOrDefault(x => x.OpRegistrationId == OPId);
                return InPatientAdmissionInfo;
            }
        }
        public IList<InPatientAdmission> ListAllIPBySearchString(IList<InPatientStatus> IpStatus, string SearchString, long CompantId, long WardId)
        {
            IList<InPatientAdmission> IpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if(WardId == 0L)
                {
                    IpInfo = Context.InPatientAdmissions.Include("PatientLocationHistory").Include("Patient").Where(x => x.CompanyId == CompantId && IpStatus.Contains(x.Status) && (x.Patient.FirstName.Contains(SearchString) || x.Patient.MiddleInitial.Contains(SearchString) || x.Patient.LastName.Contains(SearchString) || (x.Patient.FirstName + (string.IsNullOrEmpty(x.Patient.MiddleInitial) ? "" : " ") + x.Patient.MiddleInitial + (string.IsNullOrEmpty(x.Patient.LastName) ? "" : " ") + x.Patient.LastName).Contains(SearchString) || x.Patient.PatientNumber.Contains(SearchString))).OrderBy(x => x.PatientId).ToList<InPatientAdmission>();
                }
                else
                {
                    IpInfo = Context.InPatientAdmissions.Include("PatientLocationHistory").Include("Patient").Where(x => x.CompanyId == CompantId && IpStatus.Contains(x.Status) && (x.Patient.FirstName.Contains(SearchString) || x.Patient.MiddleInitial.Contains(SearchString) || x.Patient.LastName.Contains(SearchString) || (x.Patient.FirstName + (string.IsNullOrEmpty(x.Patient.MiddleInitial) ? "" : " ") + x.Patient.MiddleInitial + (string.IsNullOrEmpty(x.Patient.LastName) ? "" : " ") + x.Patient.LastName).Contains(SearchString) || x.Patient.PatientNumber.Contains(SearchString)) && (from inpl in Context.InPatientLocations where ((inpl.WardId == WardId && inpl.Active == true) | (inpl.WardId != null && inpl.Active == false)) select inpl.AdmissionId).Contains(x.Id)).OrderBy(x => x.PatientId).ToList<InPatientAdmission>();
                }                    
                return IpInfo;
            }
        }
        public IList<InPatientAdmission> ListAllIP(int statusindex, IList<InPatientStatus> IpStatus, long CompanyId, long WardId)
        {
            IList<InPatientAdmission> IpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (WardId == 0L)
                {
                    IpInfo = Context.InPatientAdmissions.Include("PatientLocationHistory").Include("Patient").Where(x => x.CompanyId.Equals(CompanyId) && IpStatus.Contains(x.Status)).OrderBy(x => x.PatientId).ToList<InPatientAdmission>();
                }
                else
                {
                    var admissionIds = (WardId == 0)
                        ? Context.InPatientLocations
                            .Where(inpl => inpl.WardId == WardId) 
                            .Select(inpl => (long)inpl.AdmissionId)
                        : Context.InPatientLocations
                            .Where(inpl => inpl.WardId == WardId && inpl.Active == true) 
                            .Select(inpl => (long)inpl.AdmissionId);

                    IpInfo = Context.InPatientAdmissions
                        .Include("PatientLocationHistory")
                        .Include("Patient")
                        .Where(x => x.CompanyId == CompanyId && IpStatus.Contains(x.Status) && admissionIds.Contains(x.Id))
                        .OrderBy(x => x.PatientId)
                        .ToList();
                }
                
                return IpInfo;
            }
        }
        public IList<InPatientAdmission> ListAllIP(IList<InPatientStatus> IpStatus, long CompantId)
        {
            IList<InPatientAdmission> IpInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IpInfo = Context.InPatientAdmissions.Include("PatientLocationHistory").Include("Patient").Where(x => x.CompanyId.Equals(CompantId) && IpStatus.Contains(x.Status)).OrderBy(x => x.PatientId).ToList<InPatientAdmission>();
                return IpInfo;
            }
        }
        
        public IList<InPatientAdmission> ListAllIpByPatientId(long PatientId)
        {
            IList<InPatientAdmission> InPatientAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmissionInfo = Context.InPatientAdmissions.Where(x => x.PatientId == PatientId).OrderByDescending(x=>x.DateOfAdmission).ToList();
                return InPatientAdmissionInfo;
            }
        }
        public InPatientAdmission AddInPatientAdmission(InPatientAdmission inPatientAdmission)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.InPatientAdmissions.Add(inPatientAdmission);
                        Context.SaveChanges();
                        OpManager.Instance.UpdateOpRegistrationFromIP(inPatientAdmission.OpRegistrationId,Status.INPATIENT,Context);
                        if (inPatientAdmission.PatientLocationHistory.First().BedId != null)
                        {
                            Bed Bed = WardManager.Instance.GetWardBedById((long)inPatientAdmission.PatientLocationHistory.First().BedId);
                            if (Bed != null)
                            {
                                WardManager.Instance.UpdateWardBedFromIpAdmission(Bed.Id, BedStatus.OCCUPIED, Context);
                            }
                        }

                        HospitalConfiguration HospitalConfiguration = Context.HospitalConfigurations.FirstOrDefault(x => x.CompanyId == inPatientAdmission.CompanyId);
                        if (HospitalConfiguration != null && HospitalConfiguration.DefaultIPConsultingFee > 0)
                        {
                            var patientLedger = new PatientLedger
                            {
                                Date = (DateTime)inPatientAdmission.CreatedDate,
                                PatientId = (long)inPatientAdmission.PatientId,
                                InPatientAdmissionId = inPatientAdmission.Id,
                                OpRegistrationId = inPatientAdmission.OpRegistrationId,
                                Amount = HospitalConfiguration.DefaultIPConsultingFee,
                                Description = "Inpatient admission fee",
                                CompanyId = inPatientAdmission.CompanyId,
                                Type = TransactionType.REGISTRATION_FEE
                            };
                            Context.PatientLedgers.Add(patientLedger);
                            Context.SaveChanges();                                
                        }
                        dbContextTransaction.Commit();
                }
                    catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    inPatientAdmission = null;
                    dbContextTransaction.Rollback();
                    throw (e);
                }
            }
            }
            return inPatientAdmission;
        }
        public InPatientAdmission UpdateInPatientAdmission(InPatientAdmission InPatientAdmission)
        {
            InPatientAdmission InPatientAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmissionInfo = Context.InPatientAdmissions.Find(InPatientAdmission.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (InPatientAdmissionInfo != null)
                        {
                            Context.Entry(InPatientAdmissionInfo).CurrentValues.SetValues(InPatientAdmission);
                            Context.SaveChanges();
                            HospitalConfiguration HospitalConfiguration = Context.HospitalConfigurations.Include("HospitalEntryConfigurations").FirstOrDefault(x => x.CompanyId == InPatientAdmissionInfo.CompanyId);
                            PatientLedger lPatientLedgerForDefaultFee = Context.PatientLedgers.FirstOrDefault(x => x.InPatientAdmissionId == InPatientAdmission.Id && x.RefNumber == null);
                            if (lPatientLedgerForDefaultFee != null)
                            {
                                lPatientLedgerForDefaultFee.Amount = HospitalConfiguration.DefaultIPConsultingFee;
                                Context.Entry(Context.PatientLedgers.Find(lPatientLedgerForDefaultFee.Id)).CurrentValues.SetValues(lPatientLedgerForDefaultFee);
                                Context.SaveChanges();
                            }
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        InPatientAdmissionInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }

            }
            return InPatientAdmissionInfo;
        }
        public InPatientAdmission UpdateInPatientAdmission(InPatientAdmission InPatientAdmission, AccountMasterContext Context)
        {
            InPatientAdmission InPatientAdmissionInfo  = Context.InPatientAdmissions.FirstOrDefault(x=>x.Id==InPatientAdmission.Id);                
            try
            {
                if (InPatientAdmissionInfo != null)
                {
                    Context.Entry(Context.InPatientAdmissions.Find(InPatientAdmission.Id)).CurrentValues.SetValues(InPatientAdmission);
                    Context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                InPatientAdmissionInfo = null;
                throw (e);
            }                
            return InPatientAdmissionInfo;
        }
        public InPatientLocation GetInPatientAdmissionByPatientIpId(long InPatientAdmissionId)
        {
            InPatientLocation inPatientLocation = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                inPatientLocation = Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(w => w.AdmissionId == InPatientAdmissionId);
                return inPatientLocation;
            }
        }
        public InPatientLocation GetInPatientLocationbyAdmitId(long InPatientAdmissionId)
        {
            InPatientLocation InPatientLocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientLocationInfo = Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == InPatientAdmissionId && x.Active == true);
                return InPatientLocationInfo;
            }
        }
        public InPatientLocation GetInPatientLocationbyAdmitIdDate(long InPatientAdmissionId, DateTime From, DateTime To)
        {
            InPatientLocation InPatientLocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientLocationInfo = Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == InPatientAdmissionId && /* x.Active == true && */ x.DateMovedIn >= From && x.DateMovedOut <= To);
                return InPatientLocationInfo;
            }
        }
        public InPatientLocation GetInPatientLocationbyAdmissionId(long InPatientAdmissionId)
        {
            InPatientLocation InPatientLocationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientLocationInfo = Context.InPatientLocations.Include("Ward").Include("Bed").FirstOrDefault(x => x.AdmissionId == InPatientAdmissionId && x.Active==true);
                return InPatientLocationInfo;
            }
        }
        
        public IList<InPatientLocation> ListAllEntryByPatientId(long AdmitId, long CompanyId)
        {
            IList<InPatientLocation> IpLocationHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                
                IpLocationHistoryInfo = (from InPatientLocation in Context.InPatientLocations where InPatientLocation.AdmissionId == AdmitId /*&& InPatientLocation.Active == true*/ select InPatientLocation).OrderBy(x => x.DateMovedIn).ToList();
                return IpLocationHistoryInfo;
            }
        }
        public IList<InPatientLocation> ListInPatientAdmissionId(long WardId)
        {
            IList<InPatientLocation> InPatientLocation = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientLocation = Context.InPatientLocations.Where(x => x.WardId == WardId && x.Active == true).OrderByDescending(x => x.Id).ToList();
                return InPatientLocation;
            }
        }
        public IList<InPatientLocation> ListInPatientLocationId(long Id)
        {
            IList<InPatientLocation> InPatientLocation = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientLocation = Context.InPatientLocations.Where(x => x.AdmissionId == Id).OrderByDescending(x => x.Id).ToList();
                return InPatientLocation;
            }
        }
        
        public bool GetInPatientLocationbyBedId(long BedId)
        {
            bool Status = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientLocation InPatientLocationInfo = Context.InPatientLocations.FirstOrDefault(x => x.BedId == BedId);
                if(InPatientLocationInfo!=null)
                {
                    Status = true;
                }
            }
            return Status;
        }
        public InPatientLocation UpdateIpLocationHist(DateTime DMO, long LctnHistId, bool ActiveStatus)
        {
            InPatientLocation InPatientInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientInfo = Context.InPatientLocations.Find(LctnHistId);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (InPatientInfo != null)
                        {
                            InPatientInfo.Active = ActiveStatus;
                            InPatientInfo.DateMovedOut = DMO.Date;
                            Context.Entry(InPatientInfo).CurrentValues.SetValues(InPatientInfo);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        InPatientInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return InPatientInfo;
        }

        public InPatientLocation AddIpLocationHistory(InPatientLocation ipLocationHistory, long LctnHistId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        InPatientLocation PreInPatientLocationInfo = Context.InPatientLocations.Find(LctnHistId);
                        if (PreInPatientLocationInfo != null)
                        {
                            // add bed fee
                            DateTime startDate;
                            DateTime endDate = DateTime.Now;
                            InPatientAdmission admission = Context.InPatientAdmissions.Find(ipLocationHistory.AdmissionId);
                            PatientRoomRent lastRentRecord = Context.PatientRoomRents.Where(x => x.InPatientAdmissionId == admission.Id).OrderByDescending(x => x.DatePosted).FirstOrDefault();

                            if (lastRentRecord != null)
                            {
                                startDate = lastRentRecord.DatePosted;
                            }
                            else
                            {
                                startDate = admission.DateOfAdmission;
                            }
                            while (startDate < endDate)
                            {
                                DateTime nextDay = startDate.Date.AddDays(1);

                                if (nextDay > endDate)
                                {
                                    nextDay = endDate;
                                }
                                PatientRoomRentManager.Instance.CalculateRoomRent(admission, startDate, nextDay, Context);
                                startDate = nextDay;
                            }

                            //update old bed status
                            PreInPatientLocationInfo.Active = false;
                            PreInPatientLocationInfo.DateMovedOut = ipLocationHistory.DateMovedIn;
                            Context.Entry(PreInPatientLocationInfo).CurrentValues.SetValues(PreInPatientLocationInfo);
                            Context.SaveChanges();
                            WardManager.Instance.UpdateWardBedFromIpAdmission((long)PreInPatientLocationInfo.BedId, BedStatus.AVAILABLE, Context);

                            //update new bed status
                            Context.InPatientLocations.Add(ipLocationHistory);
                            Context.SaveChanges();
                            WardManager.Instance.UpdateWardBedFromIpAdmission((long)ipLocationHistory.BedId, BedStatus.OCCUPIED, Context);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ipLocationHistory = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ipLocationHistory;
        }
        public InPatientLocation UpdateIpLocationHistory(InPatientLocation IpLocationHistory)
        {
            InPatientLocation IpLocationHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IpLocationHistoryInfo = Context.InPatientLocations.Find(IpLocationHistory.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (IpLocationHistoryInfo != null)
                        {
                            Context.Entry(IpLocationHistoryInfo).CurrentValues.SetValues(IpLocationHistory);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        IpLocationHistoryInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return IpLocationHistoryInfo;
        }

        public IList<InPatientAdmission> GetIPNumberContainsPrefix(string IPnumber, long companyId)
        {
            IList<InPatientAdmission> InPatientAdmissionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InPatientAdmissionInfo = Context.InPatientAdmissions.Where(x => x.CompanyId == companyId && x.PatientIPNumber.Contains(IPnumber)).ToList();
            }
            return InPatientAdmissionInfo;
        }
    }
}
