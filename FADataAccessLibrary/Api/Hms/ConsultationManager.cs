using fa.context;
using fa.model.hms.common;
using fa.model.Hms.Master;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace fa.api.Hms
{
    public class ConsultationManager
    {
        private static volatile ConsultationManager instance;
        private static object syncRoot = new Object();
        ConsultationManager()
        {

        }
        public static ConsultationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConsultationManager();
                    }
                }

                return instance;
            }
        }
        public IList<ConsultationNote> ListConsultationNotesByCompanyId(long CompanyId)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                IList<ConsultationNote> consultationNotes = context.ConsultationNotes
                    .Include("Consultant") // Include the Consultant navigation property if necessary
                    .Where(x => x.CompanyId == CompanyId)
                    .ToList();

                return consultationNotes;
            }
        }
        public IList<Consultation> ListConsultationByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Consultation> ConsultationInfo = Context.Consultations.Include("ConsultationDetail.Employee").Where(x => x.CompanyId == CompanyId).ToList();
                return ConsultationInfo;
            }
        }

        public Consultation GetConsultationById(long ConsultationsId)
        {
            Consultation ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationInfo = Context.Consultations.Find(ConsultationsId);
                if (ConsultationInfo != null)
                {
                    ConsultationInfo = Context.Consultations.Include("ConsultationDetail").FirstOrDefault(x => x.Id == ConsultationsId);
                }
                return ConsultationInfo;
            }
        }
        public Consultation GetConsultationFromDB(long ConsultationsId)
        {
            Consultation ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationInfo = Context.Consultations.Find(ConsultationsId);
                if (ConsultationInfo != null)
                {
                    ConsultationInfo = Context.Consultations.Include("ConsultationDetail").FirstOrDefault(x => x.Id == ConsultationsId);
                }
                return ConsultationInfo;
            }
        }

        public Boolean ConsultationNameUniqueById(Consultation Consultation)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Consultation lConsultation = null;
                if (Consultation.Id == 0)
                {
                    lConsultation = Context.Consultations.FirstOrDefault(x => x.Name == Consultation.Name && x.CompanyId == Consultation.CompanyId);
                }
                else
                {
                    lConsultation = Context.Consultations.FirstOrDefault(x => x.Name == Consultation.Name && x.CompanyId == Consultation.CompanyId && !x.Id.Equals(Consultation.Id));
                }
                if (lConsultation != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public Consultation GetConsultationByName(String ConsultationName, long CompanyId)
        {
            Consultation ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ConsultationInfo = Context.Consultations.FirstOrDefault(x => x.Name == ConsultationName && x.CompanyId == CompanyId);
                return ConsultationInfo;
            }
        }
        public bool DeleteConsultation(long ConsultationId)
        {

            Boolean Deleted = false;
            Consultation ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ConsultationDetail detail = Context.ConsultationDetails.FirstOrDefault(x => x.ConsultationId == ConsultationId);
                        if (detail != null)
                        {
                            Context.ConsultationDetails.Where(p => p.ConsultationId == ConsultationId).ToList().ForEach(p => Context.ConsultationDetails.Remove(p));
                        }
                        ConsultationInfo = Context.Consultations.Find(ConsultationId);
                        Context.Consultations.Remove(ConsultationInfo);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                        Logger.LogError(e);
                    }
                }
            }
            return Deleted;
        }

        public Consultation AddConsultation(Consultation consultation)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Consultations.Add(consultation);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        consultation = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return consultation;
        }
        public Consultation UpdateConsultation(Consultation Consultation)
        {
            Consultation ConsultationInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        ConsultationInfo = Context.Consultations.Find(Consultation.Id);
                        if (ConsultationInfo != null)
                        {   
                            Consultation ConsultationFromDB = GetConsultationFromDB(ConsultationInfo.Id);
                            Context.Entry(ConsultationInfo).CurrentValues.SetValues(Consultation);                          
                            Context.SaveChanges();

                            if (Consultation.ConsultationDetail != null)
                            {
                                foreach (ConsultationDetail Olddetail in ConsultationFromDB.ConsultationDetail)
                                {
                                    ConsultationDetail Newdetail = Consultation.ConsultationDetail.FirstOrDefault(x => x.Id == Olddetail.Id);
                                    if (Newdetail == null)
                                    {
                                        Context.ConsultationDetails.Remove(Context.ConsultationDetails.FirstOrDefault(x => x.Id == Olddetail.Id));
                                    }
                                    else
                                    {
                                        Consultation.ConsultationDetail.Remove(Newdetail);
                                        Newdetail.ConsultationId = Consultation.Id;
                                        Context.Entry(Newdetail).State = EntityState.Modified;
                                    }
                                    Context.SaveChanges();
                                }
                                foreach (ConsultationDetail Detail in Consultation.ConsultationDetail)
                                {
                                    Context.Consultations.Include("ConsultationDetail").FirstOrDefault(x => x.Id == ConsultationFromDB.Id).ConsultationDetail.Add(Detail);
                                    Context.SaveChanges();
                                }
                            }
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        ConsultationInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return ConsultationInfo;
        }
    }
}
