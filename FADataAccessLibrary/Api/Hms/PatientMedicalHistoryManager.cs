using fa.context;
using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class PatientMedicalHistoryManager
    {
        private static volatile PatientMedicalHistoryManager instance;
        private static object syncRoot = new Object();
        PatientMedicalHistoryManager()
        {

        }
        public static PatientMedicalHistoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PatientMedicalHistoryManager();
                    }
                }
                return instance;
            }
        }

        public IList<PatientHistoryQuestionGroup> ListAllPatientHistoryQuestionGroup()
        {
            IList<PatientHistoryQuestionGroup> PatientHistoryQuestionGroupInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientHistoryQuestionGroupInfo = Context.PatientHistoryQuestionGroups.OrderBy(x => x.Order).ToList<PatientHistoryQuestionGroup>();
                return PatientHistoryQuestionGroupInfo;
            }
        }
        
        public IList<PatientHistoryQuestion> ListAllPatientHistoryQuestionsByQuestionGroupId(long QuestionGroupId)
        {
            IList<PatientHistoryQuestion> PatientHistoryQuestionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientHistoryQuestionInfo = Context.PatientHistoryQuestions.Where(x=>x.GroupId== QuestionGroupId).ToList<PatientHistoryQuestion>();
                return PatientHistoryQuestionInfo;
            }
        }
        public PatientHistoryQuestion GetPatientHistoryQuestionById(long PatientHistoryQuestionId)
        {
            PatientHistoryQuestion PatientHistoryQuestionInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientHistoryQuestionInfo = Context.PatientHistoryQuestions.Where(x => x.Id == PatientHistoryQuestionId).First<PatientHistoryQuestion>();
                return PatientHistoryQuestionInfo;
            }
        }
        public IList<PatientPreMedicalHistory> ListAllPatientPreMedicalHistoryPatientId(long PatientId)
        {
            IList<PatientPreMedicalHistory> PatientPreMedicalHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientPreMedicalHistoryInfo = Context.PatientPreMedicalHistories.Include("Patient").Where(x => x.PatientId == PatientId).ToList<PatientPreMedicalHistory>();
                return PatientPreMedicalHistoryInfo;
            }
        }

        public PatientPreMedicalHistory GetPatientPreMedicalHistoryByQuestionId(long PatientHistoryQuestionId,long PaitentId)
        {
            PatientPreMedicalHistory PatientPreMedicalHistoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientPreMedicalHistoryInfo = Context.PatientPreMedicalHistories.FirstOrDefault(x => x.HistoryItemId == PatientHistoryQuestionId && x.PatientId== PaitentId);
                return PatientPreMedicalHistoryInfo;
            }
        }

        public Boolean AddPatientPreMedicalHistory(Patient Patient)
        {
            bool AddPatientPreMedicalHistory = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AddPatientPreMedicalHistory = true;
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Patient PatientInfo = Context.Patients.Include("History").FirstOrDefault(x => x.Id == Patient.Id);
                        if (PatientInfo.History.Count > 0)
                        {
                            PatientPreMedicalHistory PatientPreMedicalHistory = Context.PatientPreMedicalHistories.FirstOrDefault(x => x.PatientId == Patient.Id);
                            if (PatientPreMedicalHistory != null)
                            {
                                Context.PatientPreMedicalHistories.Where(p => p.PatientId == PatientInfo.Id).ToList().ForEach(p => Context.PatientPreMedicalHistories.Remove(p));
                                Context.SaveChanges();
                            }
                        }
                        if (Patient.History.Count > 0)
                        {
                            foreach (var PatientPreMedicalHistory in Patient.History)
                            {
                                Context.PatientPreMedicalHistories.Add(PatientPreMedicalHistory);
                                Context.SaveChanges();
                            }
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        AddPatientPreMedicalHistory = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
                return AddPatientPreMedicalHistory;
            }
        }
    }
}
