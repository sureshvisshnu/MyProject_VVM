using fa.context;
using fa.model.Hms.common;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class HospitalInvoiceGroupManager
    {
        private static volatile HospitalInvoiceGroupManager instance;
        private static object syncRoot = new Object();
        HospitalInvoiceGroupManager()
        {

        }
        public static HospitalInvoiceGroupManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HospitalInvoiceGroupManager();
                    }
                }
                return instance;
            }
        }
        public PatientLedgerTransactionTypeGroup GetPatientLedgerTransactionTypeGroupById(long Id)
        {
            PatientLedgerTransactionTypeGroup PatientLedgerTransactionTypeGroupInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerTransactionTypeGroupInfo=Context.PatientLedgerTransactionTypeGroups.Include("PatientLedgerTransactionTypeGroupMappings").FirstOrDefault(x => x.Id == Id);
                return PatientLedgerTransactionTypeGroupInfo;
            }
        }
        public IList<PatientLedgerTransactionTypeGroupMapping> ListAllransactionTypeforGroup(long PLTransactionTypeGroupId, long CompanyId)
        {
            IList<PatientLedgerTransactionTypeGroupMapping> TransactionTypeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TransactionTypeInfo = Context.PatientLedgerTransactionTypeGroupMappings.Where(x => x.CompanyId == CompanyId && x.PatientLedgerTransactionTypeGroupId == PLTransactionTypeGroupId).ToList<PatientLedgerTransactionTypeGroupMapping>();
            }
            return TransactionTypeInfo;
        }
        public IList<PatientLedgerTransactionTypeGroup> ListAllPatientLedgerTransactionTypeGroup(long CompanyId)
        {
            IList<PatientLedgerTransactionTypeGroup> PatientLedgerTransactionTypeGroupInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientLedgerTransactionTypeGroupInfo = Context.PatientLedgerTransactionTypeGroups.Include("PatientLedgerTransactionTypeGroupMappings").Where(x => x.CompanyId == CompanyId).ToList<PatientLedgerTransactionTypeGroup>();
                return PatientLedgerTransactionTypeGroupInfo;
            }
        }
        public IList<PatientLedgerTransactionTypeGroupMapping> ListAllPatientLedgerTransactionTypeGroupMapping(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<PatientLedgerTransactionTypeGroupMapping> PatientLedgerTransactionTypeGroupMappingInfo = Context.PatientLedgerTransactionTypeGroupMappings.Where(x => x.CompanyId == CompanyId).ToList<PatientLedgerTransactionTypeGroupMapping>();
                return PatientLedgerTransactionTypeGroupMappingInfo;
            }
        }

        public bool FindNameUnique(PatientLedgerTransactionTypeGroup PatientLedgerTransactionTypeGroup)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (PatientLedgerTransactionTypeGroup.Id == 0)
                    {
                        Context.PatientLedgerTransactionTypeGroups.Where(x => x.Name == PatientLedgerTransactionTypeGroup.Name && x.CompanyId == PatientLedgerTransactionTypeGroup.CompanyId).First<PatientLedgerTransactionTypeGroup>();
                        Status = false;
                    }
                    else
                    {
                        Context.PatientLedgerTransactionTypeGroups.Where(x => x.Name == PatientLedgerTransactionTypeGroup.Name && !x.Id.Equals(PatientLedgerTransactionTypeGroup.Id) && x.CompanyId== PatientLedgerTransactionTypeGroup.CompanyId).First<PatientLedgerTransactionTypeGroup>();
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
        public Boolean DeletePatientLedgerTransactionTypeGroup(long Id)
        {
            Boolean Deleted = false;
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientLedgerTransactionTypeGroup PatientLedgerTransactionTypeGroup = context.PatientLedgerTransactionTypeGroups.Include("PatientLedgerTransactionTypeGroupMappings").Where(p => p.Id == Id).First<PatientLedgerTransactionTypeGroup>();

                        if (PatientLedgerTransactionTypeGroup != null)
                        {
                            if (PatientLedgerTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings != null)
                            {
                                context.PatientLedgerTransactionTypeGroupMappings.Where(t => t.PatientLedgerTransactionTypeGroupId == PatientLedgerTransactionTypeGroup.Id).ToList().ForEach(t => context.PatientLedgerTransactionTypeGroupMappings.Remove(t));
                            }
                            
                        }
                        context.PatientLedgerTransactionTypeGroups.Remove(PatientLedgerTransactionTypeGroup);
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
#pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
#pragma warning restore 0168
                }
            }
            return Deleted;
        }
        public PatientLedgerTransactionTypeGroup AddPatientLedgerTransactionTypeGroup(PatientLedgerTransactionTypeGroup patientLedgerTransactionTypeGroup)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.PatientLedgerTransactionTypeGroups.Add(patientLedgerTransactionTypeGroup);
                Context.SaveChanges();
            }
            return patientLedgerTransactionTypeGroup;
        }
        public PatientLedgerTransactionTypeGroup UpdatePatientLedgerTransactionTypeGroup(PatientLedgerTransactionTypeGroup PatientLedgerTransactionTypeGroup)
        {
            PatientLedgerTransactionTypeGroup PatientLedgerTransactionTypeGroupInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientLedgerTransactionTypeGroupInfo = Context.PatientLedgerTransactionTypeGroups.Include("PatientLedgerTransactionTypeGroupMappings").FirstOrDefault(x=>x.Id==PatientLedgerTransactionTypeGroup.Id);

                        foreach (PatientLedgerTransactionTypeGroupMapping OldMapping in PatientLedgerTransactionTypeGroupInfo.PatientLedgerTransactionTypeGroupMappings.ToList())
                        {
                            PatientLedgerTransactionTypeGroupMapping NewMapping = PatientLedgerTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings.FirstOrDefault(x => x.TransactionType == OldMapping.TransactionType);
                            if (NewMapping == null)
                            {
                                Context.PatientLedgerTransactionTypeGroupMappings.Remove(Context.PatientLedgerTransactionTypeGroupMappings.FirstOrDefault(x=>x.Id==OldMapping.Id));
                            }
                            else
                            {
                                PatientLedgerTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings.Remove(NewMapping);
                                NewMapping.Id = OldMapping.Id;

                                Context.Entry(Context.PatientLedgerTransactionTypeGroupMappings.Find(OldMapping.Id)).CurrentValues.SetValues(NewMapping);
                                Context.SaveChanges();
                            }
                        }
                        foreach(PatientLedgerTransactionTypeGroupMapping NewMapping in  PatientLedgerTransactionTypeGroup.PatientLedgerTransactionTypeGroupMappings.ToList())
                        {
                            Context.PatientLedgerTransactionTypeGroupMappings.Add(NewMapping);
                            Context.SaveChanges();
                        }
                        if (PatientLedgerTransactionTypeGroupInfo != null)
                        {
                            Context.Entry(PatientLedgerTransactionTypeGroupInfo).CurrentValues.SetValues(PatientLedgerTransactionTypeGroup);
                            Context.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
            return PatientLedgerTransactionTypeGroupInfo;
        }
        



    }
}
