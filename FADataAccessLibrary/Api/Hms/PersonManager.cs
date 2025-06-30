using fa.context;
using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Hms
{
    public class PersonManager
    {
        private static volatile PersonManager instance;
        private static object syncRoot = new Object();
        PersonManager()
        {

        }
        public static PersonManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PersonManager();
                    }
                }
                return instance;
            }
        }
        public Person GetPersonById(long PersonId)
        {
            Person PersonInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PersonInfo = Context.Persons.Include("Address").Include("ContactInfo").FirstOrDefault(x => x.Id == PersonId);
                return PersonInfo;
            }
        }
        public Person GetPersonByName(string Name)
        {
            Person PersonInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PersonInfo = Context.Persons.Where(x => x.FirstName==Name).FirstOrDefault();
                return PersonInfo;
            }
        }
        public IList<Person> ListAllPerson()
        {
            IList<Person> PersonInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PersonInfo = Context.Persons.Include("Address").Include("ContactInfo").ToList<Person>();
                return PersonInfo;
            }
        }
        public IList<Person> ListPersonid(int id)
        {
            IList<Person> PersonInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PersonInfo = Context.Persons.Include("Address").Include("ContactInfo").Where(x=>x.Id==id).ToList<Person>();
                return PersonInfo;
            }
        }
        public IList<Person> ListAllInsurerPersonByCompanyId(long CompanyId,long PatientId)
        {
            IList<Person> PersonInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PersonInfo = (from person in Context.Persons.Include("Address").Include("ContactInfo") where person.Id==PatientId || Context.Guardians.Where(x=>x.PatientId==PatientId).Select(p=>p.Id).ToList().Contains(person.Id) select person).ToList();
                return PersonInfo;
            }
        }

        public long? GetInsuranceHolderInfoByName(string insurer, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                long PersonInfo = Context.Persons.AsEnumerable().Where(x => x.CompanyId == CompanyId && x.Name == insurer).Select(x => x.Id).FirstOrDefault();
                return PersonInfo;
            }
        }
    }
}
