using fa.context;
using fa.model.Hms.Master;

namespace fa.api.Hms
{
    public class PatientIdManager
       {
        private static volatile PatientIdManager instance;
        private static object syncRoot = new Object();
        PatientIdManager()
        {

        }
        public static PatientIdManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PatientIdManager();
                    }
                }
                return instance;
            }
        }
        public static string FeachPatientID(long CompanyId, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientId lPatientId = Context.PatientIds.FirstOrDefault(x => x.CompanyId == CompanyId && x.Date == Date);
                if (lPatientId == null)
                {
                    lPatientId = new PatientId
                    {
                        CompanyId = CompanyId,
                        Date = Date,
                        NextNumber = 1
                    };
                    Context.PatientIds.Add(lPatientId);
                    Context.SaveChanges();
                }
                return lPatientId.PatientNo;
            }
        }
        public static void GenerateNextPatientID(long CompanyId, DateTime Date)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientId lPatientId = Context.PatientIds.FirstOrDefault(x => x.CompanyId == CompanyId && x.Date == Date);
                if (lPatientId != null)
                {
                    int CurrentPatientNo = lPatientId.NextNumber;
                    lPatientId.NextNumber = CurrentPatientNo + 1;
                    Context.Entry(lPatientId).CurrentValues.SetValues(lPatientId);
                    Context.SaveChanges();

                }
            }
        }

    }
}
