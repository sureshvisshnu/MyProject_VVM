using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Catalog;
using fa.model.Hms.Master;
using fa.model.UserProfile;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class DoctorConsultationManager
    {
        private static volatile DoctorConsultationManager instance;
        private static object syncRoot = new Object();
        DoctorConsultationManager()
        {

        }
        public static DoctorConsultationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DoctorConsultationManager();
                    }
                }

                return instance;
            }
        }
        public ConsultedDoctorConsultationFee GetDoctorConsultationByConsultationsId(long ConsultationsId)
        {
            ConsultedDoctorConsultationFee ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationInfo = Context.ConsultedDoctorConsultationFees.FirstOrDefault(x => x.ConsultationId == ConsultationsId);
                return ConsultationInfo;
            }
        }
        public ConsultedDoctorConsultationFee GetDoctorConsultationByConsultationsandConsultantId(long ConsultantId,long ConsultationsId)
        {
            ConsultedDoctorConsultationFee ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationInfo = Context.ConsultedDoctorConsultationFees.FirstOrDefault(x => x.ConsultationId == ConsultationsId && x.ConsultantId== ConsultantId);
                return ConsultationInfo;
            }
        }

        public ConsultedDoctorConsultationFee GetLatestDoctorConsultationByConsultationsandConsultantId(long ConsultationsId, long ConsultantId)
        {
            ConsultedDoctorConsultationFee ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    ConsultationInfo = (from DocCons in Context.ConsultedDoctorConsultationFees where (DocCons.ConsultationId == ConsultationsId && DocCons.ConsultantId == ConsultantId) select DocCons).OrderByDescending(x => x.LastModifiedDate).First();
                }
                #pragma warning disable 0168
                catch (Exception ex)
                {
                }
                #pragma warning restore 0168
            }
            return ConsultationInfo;
        }
        public IList<ConsultedDoctorConsultationFee> ListDoctorConsultationByConsultantIdCompanyId(long ConsultantId, long CompanyId)
        {
            IList<ConsultedDoctorConsultationFee> DoctorConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                //DoctorConsultationInfo = Context.DoctorConsultations.Include("Consultation").Where(x =>x.ConsultantId== ConsultantId && x.CompanyId == CompanyId).ToList();
                return DoctorConsultationInfo;
            }
        }
        public ConsultedDoctorConsultationFee AddDoctorConsultation(ConsultedDoctorConsultationFee doctorConsultation)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.ConsultedDoctorConsultationFees.Add(doctorConsultation);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        doctorConsultation = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return doctorConsultation;
        }
        public IList<ConsultedDoctorConsultationFee> ListConsultedDoctorConsultationFeeByCompanyId(long CompanyId)
        {
            IList<ConsultedDoctorConsultationFee> ConsultantInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultantInfo = (from ConsultedDoctorConsultationFee in Context.ConsultedDoctorConsultationFees.Include("Company") where ConsultedDoctorConsultationFee.CompanyId == CompanyId select ConsultedDoctorConsultationFee).ToList<ConsultedDoctorConsultationFee>();
            }
            return ConsultantInfo;
        }
        public bool SaveDoctorConsultation(IList<ConsultedDoctorConsultationFee> lDoctorConsultation,User User,Company Company)
        {            
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        //List<DoctorConsultation> lDoctorConsultationFDB = Context.DoctorConsultations.Where(x => x.ConsultantId == User.UserId && x.CompanyId == Company.CompanyId).ToList();
                        //foreach (DoctorConsultation DbCon in lDoctorConsultationFDB)
                        //{
                        //    if (lDoctorConsultation.FirstOrDefault(x => x.ConsultantId == User.UserId && x.CompanyId == Company.CompanyId && x.ConsultationId == DbCon.ConsultationId) == null)
                        //    {
                        //        Context.DoctorConsultations.Remove(DbCon);
                        //        Context.SaveChanges();
                        //    }
                        //}

                        foreach (ConsultedDoctorConsultationFee Con in lDoctorConsultation)
                        {
                       
                                if (Con.Id == 0L)
                                {
                                    Context.ConsultedDoctorConsultationFees.Add(Con);
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    ConsultedDoctorConsultationFee DoctorConsultationFromDb = Context.ConsultedDoctorConsultationFees.Find(Con.Id);
                                    Context.Entry(DoctorConsultationFromDb).CurrentValues.SetValues(Con);
                                    Context.SaveChanges();
                                }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return true;
        }
    }
}
