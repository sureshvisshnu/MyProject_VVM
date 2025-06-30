using fa.context;
using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace fa.api.Hms
{
    public class InsuranceInfoManager
    {
        private static volatile InsuranceInfoManager instance;
        private static object syncRoot = new Object();
        InsuranceInfoManager()
        {

        }
        public static InsuranceInfoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InsuranceInfoManager();
                    }
                }

                return instance;
            }
        }

        public InsuranceInfo GetInsuranceInfoById(long InsuranceInfoId, bool ActiveStatus)
        {
            InsuranceInfo InsuranceInfoInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if(ActiveStatus)
                {
                    InsuranceInfoInfo = Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo").Include("Patient").FirstOrDefault(x => x.Id == InsuranceInfoId && x.IsActive == ActiveStatus);                    
                }
                else
                {
                    InsuranceInfoInfo = Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo").Include("Patient").FirstOrDefault(x => x.Id == InsuranceInfoId);                    
                }
                return InsuranceInfoInfo;
            }
        }
       
        public IList<InsuranceInfo> ListAllInsuranceInfoByPatientId(long PatientId, bool ActiveStatus,string Coverage)
        {
            IList<InsuranceInfo> InsuranceInfoInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (ActiveStatus)
                {
                    if(Coverage =="IP")
                    {
                        InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.PatientId == PatientId && InsuranceInfo.IsActive == ActiveStatus && InsuranceInfo.IPInsuranceCoverage == true select InsuranceInfo).ToList();
                    }
                    else if (Coverage == "OP")
                    {
                        InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.PatientId == PatientId && InsuranceInfo.IsActive == ActiveStatus && InsuranceInfo.OPInsuranceCoverage == true select InsuranceInfo).ToList();
                    }
                    else
                    {
                        InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.PatientId == PatientId && InsuranceInfo.IsActive == ActiveStatus select InsuranceInfo).ToList();
                    }                    
                }
                else
                {
                    InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.PatientId == PatientId select InsuranceInfo).ToList();
                }
                return InsuranceInfoInfo;
            }
        }
        public IList<InsuranceInfo> ListAllActiveInsuranceInfoByCompanyId(long CompanyId)
        {
            IList<InsuranceInfo> InsuranceInfoInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.CompanyId == CompanyId && InsuranceInfo.IsActive == true select InsuranceInfo).ToList();
                return InsuranceInfoInfo;
            }
        }
        public IList<InsuranceInfo> ListAllOPActiveInsuranceInfoByPatientId(long PatientId)
        {
            IList<InsuranceInfo> InsuranceInfoInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.PatientId == PatientId && InsuranceInfo.IsActive==true && InsuranceInfo.OPInsuranceCoverage == true select InsuranceInfo).ToList();
                return InsuranceInfoInfo;
            }
        }
        public IList<InsuranceInfo> ListAllIPActiveInsuranceInfoByPatientId(long PatientId)
        {
            IList<InsuranceInfo> InsuranceInfoInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.PatientId == PatientId && InsuranceInfo.IsActive == true && InsuranceInfo.IPInsuranceCoverage == true select InsuranceInfo).ToList();
                return InsuranceInfoInfo;
            }
        }
        public InsuranceInfo AddInsuranceInfo(InsuranceInfo insuranceInfo)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.InsuranceInfos.Add(insuranceInfo);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        insuranceInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return insuranceInfo;
        }
        public InsuranceInfo UpdateInsuranceInfo(InsuranceInfo InsuranceInfo)
        {
            InsuranceInfo InsuranceInfoInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InsuranceInfoInfo = Context.InsuranceInfos.Find(InsuranceInfo.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (InsuranceInfoInfo != null)
                        {
                            Context.Entry(InsuranceInfoInfo).CurrentValues.SetValues(InsuranceInfo);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        InsuranceInfoInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return InsuranceInfoInfo;
        }
        public Boolean DeleteInsuranceInfo(long InsuranceInfoId)
        {
            bool deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        InsuranceInfo InsuranceInfo = Context.InsuranceInfos.Include("EmployerAddress").Include("EmployerContactInfo").Where(p => p.Id == InsuranceInfoId).First<InsuranceInfo>();
                        if (InsuranceInfo != null)
                        {
                            if (InsuranceInfo.EmployerAddress != null)
                            {
                                Context.Addresses.Where(add => add.AddressId == InsuranceInfo.EmployerAddress.AddressId).ToList().ForEach(add => Context.Addresses.Remove(add));
                            }
                            if (InsuranceInfo.EmployerContactInfo != null)
                            {
                                Context.ContactInfos.Where(ci => ci.Id == InsuranceInfo.EmployerContactInfo.Id).ToList().ForEach(ci => Context.ContactInfos.Remove(ci));
                            }

                        }
                        Context.InsuranceInfos.Remove(InsuranceInfo);
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

        public bool CheckInsuranceInfoUniqueByName(string insuranceName, string policyNo, long PatientId, long companyId)
        {
            bool status = true;
            InsuranceInfo insuranceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                insuranceInfo = Context.InsuranceInfos.Where(x => x.CompanyId == companyId && x.InsuranceName == insuranceName && x.PolicyNumber == policyNo && x.PatientId == PatientId).FirstOrDefault<InsuranceInfo>();
                if (insuranceInfo != null)
                {
                    status = false;
                }
            }
            return status;
        }

        public InsuranceInfo GetInsuranceInfoByName(string insuranceName, string policyNo, long PatientId, long companyId)
        {
            InsuranceInfo insuranceInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                insuranceInfo = Context.InsuranceInfos.Where(x => x.CompanyId == companyId && x.InsuranceName == insuranceName && x.PolicyNumber == policyNo && x.PatientId == PatientId).FirstOrDefault<InsuranceInfo>();
            }
            return insuranceInfo;
        }

        public List<InsuranceInfo> GetInsuranceInfoByPatientId(long patientId)
        {
            List<InsuranceInfo> InsuranceInfoInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                InsuranceInfoInfo = (from InsuranceInfo in Context.InsuranceInfos.Include("InsuranceHolder").Include("EmployerAddress").Include("EmployerContactInfo") where InsuranceInfo.PatientId == patientId select InsuranceInfo).ToList();
                return InsuranceInfoInfo;
            }
        }
    }
}
