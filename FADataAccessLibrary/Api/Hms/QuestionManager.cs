using Fa.api.Hms;
using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Hms.Master;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FADataAccessLibrary.Model.Hms.Master;
using Microsoft.EntityFrameworkCore;
using FaData.Utils;

namespace FADataAccessLibrary.Api.Hms
{
    public class QuestionManager
    {
        private static volatile QuestionManager instance;
        private static object syncRoot = new Object();
        QuestionManager()
        {

        }
        public static QuestionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new QuestionManager();
                    }
                }

                return instance;
            }
        }
        public Allergie GetAllergiesById(long AllergiesId)
        {
            Allergie AllergiesInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AllergiesInfo = Context.Allergies.Find(AllergiesId);
                if (AllergiesInfo != null)
                {
                    AllergiesInfo = Context.Allergies.Include("AllergieCategory").Include("Keywords").FirstOrDefault(x => x.Id == AllergiesId);
                }
                return AllergiesInfo;
            }
        }
        public AllergieCategory GetAllergieCategoryInfoById(long AllergieCategoryId)
        {
            AllergieCategory AllergieCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AllergieCategoryInfo = Context.AllergieCategorys.Find(AllergieCategoryId);
                if (AllergieCategoryInfo != null)
                {
                    AllergieCategoryInfo = Context.AllergieCategorys.Include("ParentAllergieCategory").Where(p => p.Id == AllergieCategoryId).First<AllergieCategory>();
                }
            }
            return AllergieCategoryInfo;
        }
        public IList<AllergieCategory> ListAllergieCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<AllergieCategory> AllergieCategorieInfo = (from AllergieCategory in Context.AllergieCategorys where AllergieCategory.CompanyId == CompanyId select AllergieCategory).ToList();
                return AllergieCategorieInfo;
            }
        }
        public IList<AllergieCategory> ListParentAllergieCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<AllergieCategory> AllergieCategorieInfo = (from AllergieCategory in Context.AllergieCategorys where AllergieCategory.CompanyId == CompanyId where AllergieCategory.ParentAllergieCategoryId.Equals(null) select AllergieCategory).ToList();
                return AllergieCategorieInfo;
            }
        }
        public IList<Allergie> ListFilterAllergieByCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Allergie> AllergieInfo = (from Allergie in Context.Allergies.Include("AllergieCategory") where Allergie.CompanyId == CompanyId && (Allergie.Name.Contains(Filter)) select Allergie).ToList();
                return AllergieInfo;
            }
        }
        public IList<Allergie> ListAllergieByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Allergie> AllergieInfo = Context.Allergies.Include("AllergieCategory").Include("Keywords").Where(x => x.CompanyId == CompanyId).ToList();
                return AllergieInfo;
            }
        }
        public IList<Allergie> ListActiveAllergieByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Allergie> AllergieInfo = Context.Allergies.Include("Keywords").Where(x => x.CompanyId == CompanyId && x.IsActive == true).ToList();
                return AllergieInfo;
            }
        }
        public bool DeleteAllergie(long AllergieId)
        {
            Boolean Deleted = false;
            Allergie AllergieInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AllergieKeyword AllergieKeyword = Context.AllergieKeywords.FirstOrDefault(x => x.AllergieId == AllergieId);
                        if (AllergieKeyword != null)
                        {
                            Context.AllergieKeywords.Where(p => p.AllergieId == AllergieId).ToList().ForEach(p => Context.AllergieKeywords.Remove(p));
                        }
                        AllergieInfo = Context.Allergies.Find(AllergieId);
                        Context.Allergies.Remove(AllergieInfo);
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
        public Boolean DeleteAllergieCategory(long AllergieCategoryId)
        {
            Boolean Deleted = false;
            AllergieCategory AllergieCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    AllergieCategoryInfo = Context.AllergieCategorys.Find(AllergieCategoryId);
                    Context.AllergieCategorys.Remove(AllergieCategoryInfo);
                    Context.SaveChanges();
                    Deleted = true;
                }
                catch (Exception e)
                {
                    Deleted = false;
                }
            }
            return Deleted;
        }
        public Allergie GetAllergieByName(String AllergieInfoName, long CompanyId)
        {
            Allergie AllergieInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AllergieInfo = Context.Allergies.FirstOrDefault(x => x.Name == AllergieInfoName && x.CompanyId == CompanyId);
                return AllergieInfo;
            }
        }

        public AllergieCategory GetAllergieCategoryByName(String AllergieInfoName, long CompanyId)
        {
            AllergieCategory AllergieCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AllergieCategoryInfo = Context.AllergieCategorys.FirstOrDefault(x => x.Name == AllergieInfoName && x.CompanyId == CompanyId);
                return AllergieCategoryInfo;
            }
        }
        public Boolean AllergieCategoryNameUniqueById(AllergieCategory AllergieCategory)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    AllergieCategory lAllergieCategory = null;
                    if (AllergieCategory.Id == 0)
                    {
                        lAllergieCategory = Context.AllergieCategorys.FirstOrDefault(x => x.Name == AllergieCategory.Name && x.CompanyId == AllergieCategory.CompanyId);
                    }
                    else
                    {
                        lAllergieCategory = Context.AllergieCategorys.FirstOrDefault(x => x.Name == AllergieCategory.Name && x.CompanyId == AllergieCategory.CompanyId && !x.Id.Equals(AllergieCategory.Id));
                    }
                    if (lAllergieCategory != null)
                    {
                        Status = false;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return Status;
        }
        public Boolean AllergieNameUniqueById(Allergie Allergie)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Allergie lAllergie = null;
                if (Allergie.Id == 0)
                {
                    lAllergie = Context.Allergies.FirstOrDefault(x => x.Name == Allergie.Name && x.CompanyId == Allergie.CompanyId);
                }
                else
                {
                    lAllergie = Context.Allergies.FirstOrDefault(x => x.Name == Allergie.Name && x.CompanyId == Allergie.CompanyId && !x.Id.Equals(Allergie.Id));
                }
                if (lAllergie != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public IList<AllergieCategory> ListAllergieCategoryByFilterCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var Filters = new MySqlParameter("@Filter", "%" + Filter + "%");
                var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                var Type = new MySqlParameter("@Type", (int)AccountType.EMPLOYEE);
                string Query = null;
                if (Filter == string.Empty)
                {
                    Query = "SELECT * FROM Allergiecategorys WHERE CompanyId = @CompanyIds AND Name LIKE @Filter UNION SELECT * FROM AllergieCategorys WHERE Id IN(SELECT ParentAllergieCategoryId FROM AllergieCategorys where CompanyId = @CompanyIds and Name LIKE @Filter)";
                }
                else
                {
                    Query = "SELECT * FROM AllergieCategorys WHERE CompanyId = @CompanyIds AND (Name LIKE @Filter OR Id IN(SELECT AllergieCategoryId FROM Allergies WHERE CompanyId = @CompanyIds AND Name LIKE @Filter)) UNION SELECT * FROM AllergieCategorys WHERE Id IN(SELECT ParentAllergieCategoryId FROM AllergieCategorys WHERE CompanyId = @CompanyIds AND (Id IN(SELECT AllergieCategoryId FROM Allergies WHERE CompanyId = @CompanyIds and Name LIKE @Filter) OR Name LIKE @Filter)) ORDER by Id";

                }
                IList<AllergieCategory> AllergieCategoryInfo = Context.AllergieCategorys.FromSqlRaw(Query, Type, Filters, companyId).ToList();
                return AllergieCategoryInfo;
            }
        }
        public AllergieCategory AddAllergieCategory(AllergieCategory AllergieCategoryInfo)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.AllergieCategorys.Add(AllergieCategoryInfo);
                Context.SaveChanges();
            }
            return AllergieCategoryInfo;
        }
        public AllergieCategory UpdateAllergieCategory(AllergieCategory AllergieCategory)
        {
            AllergieCategory AllergieCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AllergieCategoryInfo = Context.AllergieCategorys.Find(AllergieCategory.Id);
                if (AllergieCategoryInfo != null)
                {
                    Context.Entry(AllergieCategoryInfo).CurrentValues.SetValues(AllergieCategory);
                    Context.SaveChanges();
                }
            }
            return AllergieCategoryInfo;
        }
        public Allergie AddAllergie(Allergie Allergie)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Allergies.Add(Allergie);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Allergie = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return Allergie;
        }
        //----
        public Allergie UpdateAndSaveAllergie(Allergie Allergies)
        {
            Allergie AllergieInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AllergieInfo = Context.Allergies.Find(Allergies.Id);
                        if (AllergieInfo != null)
                        {

                            Context.Entry(AllergieInfo).CurrentValues.SetValues(Allergies);

                            if (Allergies.Keywords != null && Allergies.Keywords.Count > 0)
                            {
                                foreach (AllergieKeyword Detail in Allergies.Keywords)
                                {
                                    Detail.AllergieId = Allergies.Id;
                                    Context.AllergieKeywords.Add(Detail);
                                    Context.SaveChanges();
                                }
                            }

                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        AllergieInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return AllergieInfo;
        }
        // --------------------
        public Allergie UpdateAllergie(Allergie Allergie)
        {
            Allergie AllergieInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        AllergieInfo = Context.Allergies.Find(Allergie.Id);
                        if (AllergieInfo != null)
                        {
                            Allergie AllergieInfoFromDB = GetAllergiesById(AllergieInfo.Id);
                            if (AllergieInfoFromDB.Keywords.Count > 0)
                            {
                                Context.AllergieKeywords.Where(p => p.AllergieId == AllergieInfoFromDB.Id).ToList().ForEach(p => Context.AllergieKeywords.Remove(p));
                                Context.SaveChanges();
                            }
                        }
                        Context.Entry(AllergieInfo).CurrentValues.SetValues(Allergie);
                        if (Allergie.Keywords != null && Allergie.Keywords.Count > 0)
                        {
                            foreach (AllergieKeyword Detail in Allergie.Keywords)
                            {
                                Detail.AllergieId = Allergie.Id;
                                Context.AllergieKeywords.Add(Detail);
                                Context.SaveChanges();
                            }
                        }
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        AllergieInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return AllergieInfo;
        }
    }
}
