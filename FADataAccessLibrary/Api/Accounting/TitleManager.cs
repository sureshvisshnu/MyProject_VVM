using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Employee;

namespace fa.api.Accounting
{
    public class TitleManager
    {
        private static volatile TitleManager instance;
        private static object syncRoot = new Object();
        TitleManager()
        {

        }
        public static TitleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TitleManager();
                    }
                }

                return instance;
            }
        }
        public Title GetTitleInfoById(long TitleId)
        {
            Title TitleInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TitleInfo = Context.Titles.Find(TitleId);
                if (TitleInfo != null)
                {
                    TitleInfo = Context.Titles.Where(p => p.Id == TitleId).First<Title>();
                }
            }
            return TitleInfo;
        }
        public IList<Title> ListTitleByCompanyId(long CompanyId, SoftwareType softwareType)
        {
            List<Title> lTitle = new List<Title>();
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Title> TitleInfo = (from Title in Context.Titles where Title.CompanyId == CompanyId select Title).ToList();
                if (softwareType == SoftwareType.VVMATRIX)
                {
                    lTitle.AddRange(TitleInfo.Where(x => !x.Name.Contains("Doctor") && !x.Name.Contains("Nurse") && !x.Name.Contains("Technician")));
                }
                else
                {
                    lTitle.AddRange(TitleInfo);
                }
                return lTitle;
            }
        }
        public Title CheckTitleNameInAdd(String TitleName, long CompanyId)
        {
            Title TitleInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TitleInfo = Context.Titles.FirstOrDefault(x => x.Name == TitleName && x.CompanyId== CompanyId);
                return TitleInfo;
            }
        }
        public Title CheckTitleNameInUpdate(String TitleName, long TitleId, long CompanyId)
        {
            Title TitleInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TitleInfo = Context.Titles.FirstOrDefault(x => x.Name == TitleName && !x.Id.Equals(TitleId) && x.CompanyId == CompanyId);
                return TitleInfo;
            }
        }
        public Boolean DeleteTitle(long TitleId)
        {
            Boolean Deleted = false;
            Title TitleInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        TitleInfo = Context.Titles.Find(TitleId);
                        Context.Titles.Remove(TitleInfo);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Deleted = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Deleted;
        }
        public Boolean AddTitle(Title title)
        {
            Boolean Added = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (title.Id == 0)
                        {
                            Context.Titles.Add(title);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                            Added = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Added = false;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Added;
        }
        public Title AddTitles(Title title)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (title.Id == 0)
                        {
                            Context.Titles.Add(title);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
                return title;
            }
        }
        public Title UpdateTitle(Title Title)
        {
            Title TitleInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                TitleInfo = Context.Titles.Find(Title.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (TitleInfo != null)
                        {
                            Context.Entry(TitleInfo).CurrentValues.SetValues(Title);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        TitleInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return TitleInfo;
        }

        public bool TitleNameUniqueById(Title title)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    Title lTitle = null;
                    if (title.Id == 0)
                    {
                        lTitle = Context.Titles.FirstOrDefault(x => x.Name == title.Name && x.CompanyId == title.CompanyId);
                    }
                    else
                    {
                        lTitle = Context.Titles.FirstOrDefault(x => x.Name == title.Name && x.CompanyId == title.CompanyId && !x.Id.Equals(title.Id));
                    }
                    if (lTitle != null)
                    {
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

        public Title GetTitleInfoByName(string name, long companyId)
        {
            Title lTitle = null!;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lTitle = Context.Titles.FirstOrDefault(x => x.CompanyId == companyId && x.Name == name);
                if (lTitle != null)
                {
                    return lTitle;
                }
                return null;
            }
        }
    }
}
