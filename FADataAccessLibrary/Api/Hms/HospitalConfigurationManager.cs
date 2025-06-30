using fa.api.Accounting;
using fa.api.System;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.hms.config;
using fa.model.System;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class HospitalConfigurationManager
    {
        private static volatile HospitalConfigurationManager instance;
        private static object syncRoot = new Object();
        HospitalConfigurationManager()
        {

        }
        public static HospitalConfigurationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HospitalConfigurationManager();
                    }
                }
                return instance;
            }
        }
        public HospitalConfiguration GetSettingsByCompanyId(long Id)
        {
            HospitalConfiguration HospitalConfiguration = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                HospitalConfiguration = Context.HospitalConfigurations.FirstOrDefault(x => x.CompanyId == Id);
                return HospitalConfiguration;
            }
        }
        public List<IdSpace> GetHospitalIdspaceByCompanyId(long Id)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<IdSpace> IdSpacesFromDB = Context.IdSpaces.Where(x => x.CompanyId == Id && (x.EntryType == EntryType.OP_TOKEN || x.EntryType == EntryType.PATIENT_FEE_RECEIPT || x.EntryType == EntryType.PATIENT_INVOICE || x.EntryType == EntryType.PRESCRIPTION)).ToList();
                return IdSpacesFromDB;
            }
        }
        public HospitalConfiguration AddHospitalConfiguration(HospitalConfiguration hospitalConfiguration, List<IdSpace> IdSpaces)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (hospitalConfiguration.PatientPurchaseAccount != null)
                        {
                            CompanyManager.Instance.UpdateCompany(Context, hospitalConfiguration.PatientPurchaseAccount);
                        }
                        UpdateIdspaceForHospital(Context, IdSpaces, hospitalConfiguration.CompanyId);
                        Context.HospitalConfigurations.Add(hospitalConfiguration);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        hospitalConfiguration = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return hospitalConfiguration;
        }
        public HospitalConfiguration UpdateHospitalConfiguration(HospitalConfiguration HospitalConfiguration, List<IdSpace> IdSpaces)
        {
            HospitalConfiguration HospitalConfigurationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                HospitalConfigurationInfo = Context.HospitalConfigurations.Find(HospitalConfiguration.Id);
                if (HospitalConfigurationInfo != null)
                {
                    if (HospitalConfiguration.PatientPurchaseAccount != null)
                    {
                        CompanyManager.Instance.UpdateCompany(Context, HospitalConfiguration.PatientPurchaseAccount);
                    }
                    UpdateIdspaceForHospital(Context, IdSpaces, HospitalConfiguration.CompanyId);
                    Context.Entry(HospitalConfigurationInfo).CurrentValues.SetValues(HospitalConfiguration);
                    Context.SaveChanges();
                    return HospitalConfigurationInfo;
                }
            }
            return HospitalConfigurationInfo;
        }
        public void UpdateIdspaceForHospital(AccountMasterContext Context, List<IdSpace> IdSpaces, long CompanyId)
        {
            DateTime Start = IdSpaces.First().YearStartDate;
            DateTime End = IdSpaces.First().YearEndDate;
            List<IdSpace> IdSpacesFromDB = Context.IdSpaces.Where(x => x.CompanyId == CompanyId && (x.EntryType == EntryType.OP_TOKEN || x.EntryType == EntryType.PATIENT_FEE_RECEIPT || x.EntryType == EntryType.PATIENT_INVOICE)).ToList();
            foreach (IdSpace OldInfo in IdSpacesFromDB.Where(x => x.YearStartDate == Start && x.YearEndDate == End))
            {
                IdSpace NewInfo = IdSpaces.FirstOrDefault(x => x.EntryType == OldInfo.EntryType);
                if (NewInfo == null)
                {
                    Context.IdSpaces.Remove(Context.IdSpaces.FirstOrDefault(x => x.Id == OldInfo.Id));
                }
                else
                {
                    IdSpaces.Remove(NewInfo);
                    NewInfo.CompanyId = OldInfo.CompanyId;
                    NewInfo.Id = OldInfo.Id;
                    NewInfo.HasDotMatrix = OldInfo.HasDotMatrix;
                    NewInfo.HasRoundOff = OldInfo.HasRoundOff;
                    NewInfo.HasPrinterSetup = OldInfo.HasPrinterSetup;
                    Context.Entry(Context.IdSpaces.Find(OldInfo.Id)).CurrentValues.SetValues(NewInfo);
                }
                Context.SaveChanges();
            }
            foreach (IdSpace Info in IdSpaces)
            {
                IdSpaceEntryTypeDetail Detail = Context.IdSpaceEntryTypeDetails.FirstOrDefault(x => x.EntryType == Info.EntryType);
                if (Detail != null)
                {
                    Info.CompanyId = CompanyId;
                    Info.HasPrinterSetup = Detail.HasPrinterSetup;
                    Info.HasRoundOff = Detail.HasRoundOff;
                    Info.HasDotMatrix = Detail.HasDotMatrix;
                    Context.IdSpaces.Add(Info);
                    Context.SaveChanges();
                }
            }
        }
        public static bool[] HasEntry(long CompanyId, DateTime Start, DateTime End)
        {
            bool[] HasEntry = new bool[25];
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                HasEntry[(int)EntryType.OP_TOKEN] = Context.Registrationes.Where(x => x.CompanyId == CompanyId && x.DateOfRegistration >= Start && x.DateOfRegistration <= End).ToList().Count > 0 ? true : false;
                HasEntry[(int)EntryType.PATIENT_FEE_RECEIPT] = Context.PatientPaymentDetails.Where(x => x.CompanyId == CompanyId && x.Date >= Start && x.Date <= End).ToList().Count > 0 ? true : false;
                HasEntry[(int)EntryType.PATIENT_INVOICE] = Context.PatientInvoices.Where(x => x.CompanyId == CompanyId && x.InvoiceDate >= Start && x.InvoiceDate <= End).ToList().Count > 0 ? true : false;
            }
            return HasEntry;
        }
    }
}
