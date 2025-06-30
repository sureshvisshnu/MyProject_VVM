using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Employee;
using fa.model.Hms.Master;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.ComponentModel.Design;

namespace Fa.api.Hms
{

    public class SymptomsManager
    {
        private static volatile SymptomsManager instance;
        private static object syncRoot = new Object();
        SymptomsManager()
        {

        }
        public static SymptomsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SymptomsManager();
                    }
                }

                return instance;
            }
        }
        public Symptom GetSymptomsById(long SymptomsId)
        {
            Symptom SymptomsInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SymptomsInfo = Context.Symptoms.Find(SymptomsId);
                if (SymptomsInfo != null)
                {
                    SymptomsInfo = Context.Symptoms.Include("SymptomCategory").Include("Keywords").FirstOrDefault(x => x.Id == SymptomsId);
                }
                return SymptomsInfo;
            }
        }

        public SymptomCategory GetSymptomCategoryInfoById(long SymptomCategoryId)
        {
            SymptomCategory SymptomCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SymptomCategoryInfo = Context.SymptomCategorys.Find(SymptomCategoryId);
                if (SymptomCategoryInfo != null)
                {
                    SymptomCategoryInfo = Context.SymptomCategorys.Include("ParentSymptomCategory").Where(p => p.Id == SymptomCategoryId).First<SymptomCategory>();
                }
            }
            return SymptomCategoryInfo;
        }
        public IList<SymptomCategory> ListSymptomCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<SymptomCategory> SymptomCategorieInfo = (from SymptomCategory in Context.SymptomCategorys where SymptomCategory.CompanyId == CompanyId select SymptomCategory).ToList();
                return SymptomCategorieInfo;
            }
        }
        public IList<SymptomCategory> ListParentSymptomCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<SymptomCategory> SymptomCategorieInfo = (from SymptomCategory in Context.SymptomCategorys where SymptomCategory.CompanyId == CompanyId where SymptomCategory.ParentSymptomCategoryId.Equals(null) select SymptomCategory).ToList();
                return SymptomCategorieInfo;
            }
        }
        public IList<Symptom> ListFilterSymptomByCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Symptom> SymptomInfo = (from Symptom in Context.Symptoms.Include("SymptomCategory") where Symptom.CompanyId == CompanyId && (Symptom.Name.Contains(Filter)) select Symptom).ToList();
                return SymptomInfo;
            }
        }
        public IList<Symptom> ListFilterSymptomByNameCodeCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Symptom> SymptomInfo = Context.Symptoms
                    .Include("SymptomCategory")
                    .Where(Symptom => Symptom.CompanyId == CompanyId &&
                                     (Symptom.Name.Contains(Filter) || Symptom.SymptomCode.Contains(Filter)))
                    .ToList();

                return SymptomInfo;
            }
        }

        public IList<SymptomKeyword> KeywordsBySymptomId(long CompanyId, long SymptomsId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var symptom = Context.Symptoms
                    .Include(s => s.SymptomCategory)
                    .Include(s => s.Keywords) // Ensure keywords are loaded
                    .FirstOrDefault(s => s.Id == SymptomsId && s.CompanyId == CompanyId);

                return symptom?.Keywords?.ToList() ?? new List<SymptomKeyword>();
            }
        }

        public IList<Symptom> ListSymptomByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Symptom> SymptomInfo = Context.Symptoms.Include("SymptomCategory").Include("Keywords").Where(x => x.CompanyId == CompanyId).ToList();
                return SymptomInfo;
            }
        }
        public IList<Symptom> ListActiveSymptomByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Symptom> SymptomInfo = Context.Symptoms.Include("Keywords").Where(x => x.CompanyId == CompanyId && x.IsActive == true).ToList();
                return SymptomInfo;
            }
        }
        public bool DeleteSymptom(long SymptomId)
        {
            Boolean Deleted = false;
            Symptom SymptomInfo = null;

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        bool isSymptomInUse = Context.ConsultedSymptoms
                            .Any(cs => cs.SymptomId == SymptomId);

                        if (isSymptomInUse)
                        {
                            return false;
                        }

                        var symptomKeywords = Context.SymptomsKeywords
                            .Where(k => k.SymptomId == SymptomId)
                            .ToList();

                        if (symptomKeywords.Any())
                        {
                            Context.SymptomsKeywords.RemoveRange(symptomKeywords);
                            Context.SaveChanges();
                        }

                        SymptomInfo = Context.Symptoms.Find(SymptomId);
                        if (SymptomInfo != null)
                        {
                            Context.Symptoms.Remove(SymptomInfo);
                            Context.SaveChanges();
                        }

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
        public Boolean DeleteSymptomCategory(long SymptomCategoryId)
        {
            Boolean Deleted = false;

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    var symptomIds = Context.Symptoms
                        .Where(s => s.SymptomCategoryId == SymptomCategoryId)
                        .Select(s => s.Id)
                        .ToList();

                    bool hasConsultations = Context.ConsultedSymptoms
                        .Any(cs => symptomIds.Contains(cs.SymptomId));

                    if (hasConsultations)
                    {
                        return false; 
                    }

                    var relatedKeywords = Context.SymptomsKeywords
                        .Where(k => symptomIds.Contains((long)k.SymptomId))
                        .ToList();

                    if (relatedKeywords.Any())
                    {
                        Context.SymptomsKeywords.RemoveRange(relatedKeywords);
                        Context.SaveChanges();
                    }

                    var symptoms = Context.Symptoms
                        .Where(s => symptomIds.Contains(s.Id))
                        .ToList();

                    if (symptoms.Any())
                    {
                        Context.Symptoms.RemoveRange(symptoms);
                        Context.SaveChanges();
                    }

                    var symptomCategory = Context.SymptomCategorys.Find(SymptomCategoryId);

                    if (symptomCategory != null)
                    {
                        Context.SymptomCategorys.Remove(symptomCategory);
                        Context.SaveChanges();
                        Deleted = true;
                    }
                }
                catch (Exception)
                {
                    Deleted = false;
                }
            }
            return Deleted;
        }

        public Symptom GetSymptomByName(String SymptomInfoName, long CompanyId)
        {
            Symptom SymptomInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SymptomInfo = Context.Symptoms.FirstOrDefault(x => x.Name == SymptomInfoName && x.CompanyId == CompanyId);
                return SymptomInfo;
            }
        }
        public Boolean GetSymptomByNameCodeUniqueById(Symptom symptom)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var existingRecord = Context.Symptoms
                    .FirstOrDefault(x => x.Id == symptom.Id && x.CompanyId == symptom.CompanyId);

                if (existingRecord != null)
                {
                    if (existingRecord.Name == symptom.Name && existingRecord.SymptomCode == symptom.SymptomCode)
                        return true;

                    return false;
                }

                bool exists = Context.Symptoms
                    .Any(x =>
                        x.CompanyId == symptom.CompanyId &&
                        (x.Name == symptom.Name || x.SymptomCode == symptom.SymptomCode));

                return !exists;
            }
        }

        public Symptom GetSymptomByNameCode(string SymptomInfoName, string SymptomCode, long CompanyId)
        {
            Symptom SymptomInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SymptomInfo = Context.Symptoms.FirstOrDefault(x => x.Name == SymptomInfoName && x.SymptomCode == SymptomCode && x.CompanyId == CompanyId);
                return SymptomInfo;
            }
        }
        public Symptom GetSymptomByCode(String SymptomCodeInfo, long CompanyId)
        {
            Symptom SymptomInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SymptomInfo = Context.Symptoms.FirstOrDefault(x => x.SymptomCode == SymptomCodeInfo && x.CompanyId == CompanyId);
                return SymptomInfo;
            }
        }
        public SymptomCategory GetSymptomCategoryByName(String SymptomInfoName, long CompanyId)
        {
            SymptomCategory SymptomCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SymptomCategoryInfo = Context.SymptomCategorys.FirstOrDefault(x => x.Name == SymptomInfoName && x.CompanyId == CompanyId);
                return SymptomCategoryInfo;
            }
        }

        public SymptomCategory GetSymptomCategoryByNameCode(string SymptomInfoName, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var symptom = Context.Symptoms
                    .Where(p => (p.Name == SymptomInfoName || p.SymptomCode == SymptomInfoName)
                                && p.CompanyId == CompanyId)
                    .Select(p => p.SymptomCategoryId)
                    .FirstOrDefault();

                if (symptom == null)
                    return null;

                return Context.SymptomCategorys.FirstOrDefault(c => c.Id == symptom);
            }
        }
        public Boolean SymptomCategoryNameUniqueById(SymptomCategory SymptomCategory)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    SymptomCategory lSymptomCategory = null;
                    if (SymptomCategory.Id == 0)
                    {
                        lSymptomCategory = Context.SymptomCategorys.FirstOrDefault(x => x.Name == SymptomCategory.Name && x.CompanyId == SymptomCategory.CompanyId);
                    }
                    else
                    {
                        lSymptomCategory = Context.SymptomCategorys.FirstOrDefault(x => x.Name == SymptomCategory.Name && x.CompanyId == SymptomCategory.CompanyId && !x.Id.Equals(SymptomCategory.Id));
                    }
                    if (lSymptomCategory != null)
                    {
                        Status = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Status;
        }
        public Boolean SymptomNameUniqueById(Symptom Symptom)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Symptom lSymptom = null;
                if (Symptom.Id == 0)
                {
                    lSymptom = Context.Symptoms.FirstOrDefault(x => x.Name == Symptom.Name && x.CompanyId == Symptom.CompanyId);
                }
                else
                {
                    lSymptom = Context.Symptoms.FirstOrDefault(x => x.Name == Symptom.Name && x.CompanyId == Symptom.CompanyId && !x.Id.Equals(Symptom.Id));
                }
                if (lSymptom != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public Boolean SymptomCodeByUnique(Symptom Symptom)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Symptom lSymptom = null;
                if (Symptom.Id == 0)
                {
                    lSymptom = Context.Symptoms.FirstOrDefault(x => x.SymptomCode == Symptom.SymptomCode && x.CompanyId == Symptom.CompanyId);
                }
                else
                {
                    lSymptom = Context.Symptoms.FirstOrDefault(x => x.SymptomCode == Symptom.SymptomCode && x.CompanyId == Symptom.CompanyId && !x.Id.Equals(Symptom.Id));
                }
                if (lSymptom != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public IList<SymptomCategory> ListSymptomCategoryByFilterCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var Filters = new MySqlParameter("@Filter", "%" + Filter + "%");
                var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                var Type = new MySqlParameter("@Type", (int)AccountType.EMPLOYEE);
                string Query = null;
                if (Filter == string.Empty)
                {
                    Query = "SELECT * FROM Symptomcategorys WHERE CompanyId = @CompanyIds AND Name LIKE @Filter UNION SELECT * FROM SymptomCategorys WHERE Id IN(SELECT ParentSymptomCategoryId FROM SymptomCategorys where CompanyId = @CompanyIds and Name LIKE @Filter)";
                }
                else
                {
                    Query = "SELECT * FROM SymptomCategorys WHERE CompanyId = @CompanyIds AND (Name LIKE @Filter OR Id IN(SELECT SymptomCategoryId FROM Symptoms WHERE CompanyId = @CompanyIds AND Name LIKE @Filter)) UNION SELECT * FROM SymptomCategorys WHERE Id IN(SELECT ParentSymptomCategoryId FROM SymptomCategorys WHERE CompanyId = @CompanyIds AND (Id IN(SELECT SymptomCategoryId FROM Symptoms WHERE CompanyId = @CompanyIds and Name LIKE @Filter) OR Name LIKE @Filter)) ORDER by Id";

                }
                IList<SymptomCategory> SymptomCategoryInfo = Context.SymptomCategorys.FromSqlRaw(Query, Type, Filters, companyId).ToList();
                return SymptomCategoryInfo;
            }
        }
        public SymptomCategory AddSymptomCategory(SymptomCategory SymptomCategoryInfo)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.SymptomCategorys.Add(SymptomCategoryInfo);
                Context.SaveChanges();
            }
            return SymptomCategoryInfo;
        }
        public SymptomCategory UpdateSymptomCategory(SymptomCategory SymptomCategory)
        {
            SymptomCategory SymptomCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                SymptomCategoryInfo = Context.SymptomCategorys.Find(SymptomCategory.Id);
                if (SymptomCategoryInfo != null)
                {
                    Context.Entry(SymptomCategoryInfo).CurrentValues.SetValues(SymptomCategory);
                    Context.SaveChanges();
                }
            }
            return SymptomCategoryInfo;
        }
        public Symptom AddSymptom(Symptom symptom)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Symptoms.Add(symptom);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        symptom = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return symptom;
        }
        public Symptom UpdateAndSaveSymptom(Symptom Symptoms)
        {
            Symptom SymptomInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        SymptomInfo = Context.Symptoms.Find(Symptoms.Id);
                        if (SymptomInfo != null)
                        {

                            Context.Entry(SymptomInfo).CurrentValues.SetValues(Symptoms);

                            if (Symptoms.Keywords != null && Symptoms.Keywords.Count > 0)
                            {
                                foreach (SymptomKeyword Detail in Symptoms.Keywords)
                                {
                                    Detail.SymptomId = Symptoms.Id;
                                    Context.SymptomsKeywords.Add(Detail);
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
                        SymptomInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return SymptomInfo;
        }
        public Symptom UpdateSymptomKeywords(Symptom Symptom)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Symptom existingSymptom = Context.Symptoms
                            .Include(s => s.Keywords)
                            .FirstOrDefault(s => s.Id == Symptom.Id);

                        if (existingSymptom == null)
                        {
                            throw new Exception("Symptom not found.");
                        }

                        HashSet<string> existingKeywordTexts = new HashSet<string>(existingSymptom.Keywords.Select(k => k.Text));

                        List<SymptomKeyword> updatedKeywords = new List<SymptomKeyword>();

                        foreach (var keyword in Symptom.Keywords)
                        {
                            if (!existingKeywordTexts.Contains(keyword.Text)) // Only add new keywords
                            {
                                keyword.SymptomId = Symptom.Id;
                                updatedKeywords.Add(keyword);
                            }
                        }

                        if (updatedKeywords.Count > 0)
                        {
                            Context.SymptomsKeywords.AddRange(updatedKeywords);
                            Context.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                        return existingSymptom;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public Symptom UpdateSymptom(Symptom Symptom)
        {
            Symptom SymptomInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        SymptomInfo = Context.Symptoms.Find(Symptom.Id);
                        if (SymptomInfo == null)
                        {
                            throw new Exception("Symptom not found.");
                        }

                        Context.Entry(SymptomInfo).CurrentValues.SetValues(Symptom);

                        if (Symptom.Keywords != null && Symptom.Keywords.Count > 0)
                        {
                            UpdateSymptomKeywords(Symptom);
                        }

                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        SymptomInfo = null;
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            return SymptomInfo;
        }

    }
}