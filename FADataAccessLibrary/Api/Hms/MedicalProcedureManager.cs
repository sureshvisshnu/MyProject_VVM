using fa.model.Hms.Master;
using fa.context;
using Fa.api.exceptions;
using Microsoft.EntityFrameworkCore;
using FaData.Utils;
using fa.model.Accounting.Masters;
using MySqlConnector;
using System.Linq;

namespace Fa.api.Hms
{
    public class MedicalProcedureManager
    {
        private static volatile MedicalProcedureManager instance;
        private static object syncRoot = new Object();
        MedicalProcedureManager()
        {

        }
        public static MedicalProcedureManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MedicalProcedureManager();
                    }
                }
                return instance;
            }
        }
        //MedicalProcedureFilters
        public MedicalProcedure GetMedicalProcedureByActiveId(long MedicalProcedureId)
        {
            MedicalProcedure MedicalProcedureInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalProcedureInfo = Context.MedicalProcedures.Include("ProcedureElements").Include("Keywords").FirstOrDefault(x => x.Id == MedicalProcedureId && x.IsActive == true);
                        return MedicalProcedureInfo;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalProcedureInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
        }
        public MedicalProcedure GetMedicalProcedureById(long MedicalProcedureId)
        {
            MedicalProcedure MedicalProcedureInfo = null;

            using AccountMasterContext Context = new AccountMasterContext();

            MedicalProcedureInfo = Context.MedicalProcedures.Find(MedicalProcedureId);

            if (MedicalProcedureInfo != null)
            {
                MedicalProcedureInfo = Context.MedicalProcedures
                    .Include("MedicalProcedureCategory")
                    .Include("Keywords")
                    .FirstOrDefault(x => x.Id == MedicalProcedureId);
            }

            return MedicalProcedureInfo;
        }

        public MedicalProcedure GetMedicalProcedureByName(String ProcedureInfoName, long CompanyId)
        {
            MedicalProcedure ProcedureInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProcedureInfo = Context.MedicalProcedures.FirstOrDefault(x => x.Name == ProcedureInfoName && x.CompanyId == CompanyId);
                return ProcedureInfo;
            }
        }
        public MedicalProcedure GetMedicalProcedureByCodeName(string ProcedureInfoName, string ProcedureCode, long CompanyId)
        {
            MedicalProcedure ProcedureInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ProcedureInfo = Context.MedicalProcedures.FirstOrDefault(x => x.Name == ProcedureInfoName && x.ProcedureCode == ProcedureCode && x.CompanyId == CompanyId);
                return ProcedureInfo;
            }
        }
        public MedicalProcedureCategory GetMedicalProcedureCategoryByName(String MedicalProcedureInfoName, long CompanyId)
        {
            MedicalProcedureCategory MedicalProcedureCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalProcedureCategoryInfo = Context.MedicalProcedureCategory.FirstOrDefault(x => x.Name == MedicalProcedureInfoName && x.CompanyId == CompanyId);
                return MedicalProcedureCategoryInfo;
            }
        }

        public MedicalProcedureCategory GetMedicalProcedureCategoryByNameCode(string MedicalProcedureInfoName, long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var procedure = Context.MedicalProcedures
                    .Where(p => (p.Name == MedicalProcedureInfoName || p.ProcedureCode == MedicalProcedureInfoName)
                                && p.CompanyId == CompanyId)
                    .Select(p => p.MedicalProcedureCategoryId)
                    .FirstOrDefault();

                if (procedure == null)
                    return null; 

                return Context.MedicalProcedureCategory.FirstOrDefault(c => c.Id == procedure);
            }
        }

        public IList<MedicalProcedure> ListMedicalProcedureByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalProcedure> MedicalProcedureInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalProcedureInfo = Context.MedicalProcedures.Include("MedicalProcedureCategory").Include("ProcedureElements").Include("Keywords").ToList().Where(x=>x.CompanyId == CompanyId).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalProcedureInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalProcedureInfo;
                }
            }
        }
        public IList<MedicalProcedure> ListActiveMedicalProcedureByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalProcedure> MedicalProcedureInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalProcedureInfo = Context.MedicalProcedures.Include("ProcedureElements").Include("Keywords").ToList().Where(x => x.CompanyId == CompanyId && x.IsActive).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalProcedureInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalProcedureInfo;
                }
            }
        }
        public IList<MedicalProcedureElement> ListMedicalProcedureElementById(long MedicalProcedureId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalProcedureElement> MedicalProcedureElementInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalProcedureElementInfo = (from MedicalProcedureElement in Context.MedicalProcedureElements where MedicalProcedureElement.MedicalProcedureId == MedicalProcedureId select MedicalProcedureElement).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalProcedureElementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalProcedureElementInfo;
                }
            }
        }
        public Boolean MedicalProcedureNameUniqueById(MedicalProcedure MedicalProcedure)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalProcedure lMedicalProcedure = null;
                if (MedicalProcedure.Id == 0)
                {
                    lMedicalProcedure = Context.MedicalProcedures.FirstOrDefault(x => x.Name == MedicalProcedure.Name && x.CompanyId == MedicalProcedure.CompanyId);
                }
                else
                {
                    lMedicalProcedure = Context.MedicalProcedures.FirstOrDefault(x => x.Name == MedicalProcedure.Name && x.CompanyId == MedicalProcedure.CompanyId && !x.Id.Equals(MedicalProcedure.Id));
                }
                if (lMedicalProcedure != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        
        public Boolean MedicalProcedureCodeNameUniqueById(MedicalProcedure MedicalProcedure)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var existingRecord = Context.MedicalProcedures
                    .FirstOrDefault(x => x.Id == MedicalProcedure.Id && x.CompanyId == MedicalProcedure.CompanyId);

                if (existingRecord != null)
                {
                    if (existingRecord.Name == MedicalProcedure.Name && existingRecord.ProcedureCode == MedicalProcedure.ProcedureCode)
                        return true; 

                    return false; 
                }

                bool exists = Context.MedicalProcedures
                    .Any(x =>
                        x.CompanyId == MedicalProcedure.CompanyId &&
                        (x.Name == MedicalProcedure.Name || x.ProcedureCode == MedicalProcedure.ProcedureCode));

                return !exists; 
            }
        }
        public Boolean ProcedureCodeByUnique(MedicalProcedure procedure)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalProcedure lMedicalProcedure = null;
                if (procedure.Id == 0)
                {
                    lMedicalProcedure = Context.MedicalProcedures.FirstOrDefault(x => x.ProcedureCode == procedure.ProcedureCode && x.CompanyId == procedure.CompanyId);
                }
                else
                {
                    lMedicalProcedure = Context.MedicalProcedures.FirstOrDefault(x => x.ProcedureCode == procedure.ProcedureCode && x.CompanyId == procedure.CompanyId && !x.Id.Equals(procedure.Id));
                }
                if (lMedicalProcedure != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        // All Updations
        public MedicalProcedure AddMedicalProcedure(MedicalProcedure medicalProcedure)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.MedicalProcedures.Add(medicalProcedure);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        medicalProcedure = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return medicalProcedure;
        }
        
        public MedicalProcedure UpdateAndSaveProcedure(MedicalProcedure medicalProcedure)
        {
            MedicalProcedure medicalProcedureInfo = null;

            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        medicalProcedureInfo = context.MedicalProcedures.Find(medicalProcedure.Id);

                        if (medicalProcedureInfo != null)
                        {
                            context.Entry(medicalProcedureInfo).State = EntityState.Detached;

                            var keywords = context.MedicalProcedureKeywords.Where(x => x.MedicalProcedureId == medicalProcedure.Id).ToList();
                            if (keywords != null)
                            {
                                context.MedicalProcedureKeywords.RemoveRange(keywords);
                            }

                            if (medicalProcedure.Keywords != null)
                            {
                                foreach (MedicalProcedureKeyword detail in medicalProcedure.Keywords)
                                {
                                    detail.MedicalProcedureId = medicalProcedure.Id; 
                                    context.MedicalProcedureKeywords.Add(detail);
                                }
                            }

                            context.Entry(medicalProcedure).State = EntityState.Modified;
                            context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        medicalProcedureInfo = null;
                        dbContextTransaction.Rollback();
                        throw; 
                    }
                }
            }

            return medicalProcedureInfo;
        }
        
        public MedicalProcedure UpdateMedicalProcedure(MedicalProcedure MedicalProcedure)
        {
            MedicalProcedure MedicalProcedureInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalProcedureInfo = Context.MedicalProcedures.Find(MedicalProcedure.Id);
                        if (MedicalProcedureInfo != null)
                        {
                            //IList<MedicalProcedureElement> MedicalProcedureElements = Context.MedicalProcedureElements.Where(x => x.MedicalProcedureId == MedicalProcedure.Id).ToList();
                            //foreach (MedicalProcedureElement OldMedicalProcedureElement in MedicalProcedureElements)
                            //{
                            //    MedicalProcedureElement NewMedicalProcedureElement = MedicalProcedure.ProcedureElements.FirstOrDefault(x => x.Id == OldMedicalProcedureElement.Id);
                            //    if (NewMedicalProcedureElement == null)
                            //    {
                            //        MedicalProcedureElement ElementFromDB = Context.MedicalProcedureElements.Find(OldMedicalProcedureElement.Id);
                            //        Context.MedicalProcedureElements.Remove(ElementFromDB);
                            //        Context.SaveChanges();
                            //    }
                            //    else
                            //    {
                            //        MedicalProcedure.ProcedureElements.Remove(NewMedicalProcedureElement);
                            //        NewMedicalProcedureElement.MedicalProcedureId = MedicalProcedure.Id;
                            //        Context.Entry(OldMedicalProcedureElement).CurrentValues.SetValues(NewMedicalProcedureElement);
                            //        Context.SaveChanges();
                            //    }
                            //}
                            IList<MedicalProcedureKeyword> Keywords = Context.MedicalProcedureKeywords.Where(x => x.MedicalProcedureId == MedicalProcedure.Id).ToList();
                            foreach (MedicalProcedureKeyword OldMedicalProcedureKeyword in Keywords)
                            {
                                MedicalProcedureKeyword NewMedicalProcedureKeyword = MedicalProcedure.Keywords.FirstOrDefault(x => x.Id == OldMedicalProcedureKeyword.Id);
                                if (NewMedicalProcedureKeyword == null)
                                {
                                    Context.MedicalProcedureKeywords.Remove(Context.MedicalProcedureKeywords.Find(OldMedicalProcedureKeyword.Id));
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    MedicalProcedure.Keywords.Remove(NewMedicalProcedureKeyword);
                                    NewMedicalProcedureKeyword.MedicalProcedureId = MedicalProcedure.Id;
                                    Context.Entry(OldMedicalProcedureKeyword).CurrentValues.SetValues(NewMedicalProcedureKeyword);
                                    Context.SaveChanges();
                                }
                            }
                            Context.Entry(MedicalProcedureInfo).CurrentValues.SetValues(MedicalProcedure);
                            //foreach (MedicalProcedureElement Detail in MedicalProcedure.ProcedureElements)
                            //{
                            //    Detail.MedicalProcedureId = MedicalProcedure.Id;
                            //    Context.MedicalProcedureElements.Add(Detail);
                            //    Context.SaveChanges();
                            //}
                            foreach (MedicalProcedureKeyword Detail in MedicalProcedure.Keywords)
                            {
                                Detail.MedicalProcedureId = MedicalProcedure.Id;
                                Context.MedicalProcedureKeywords.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalProcedureInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return MedicalProcedureInfo;
        }
        public Boolean DeleteMedicalProcedure(long MedicalProcedureId)
        {
            Boolean Deleted = false;
            MedicalProcedure MedicalProcedureInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if(Context.ConsultedProcedures.Any(cp=>cp.MedicalProcedureId == MedicalProcedureId))
                        {
                            throw new HasReferenceException("Consulted Procedures Found");
                        }    

                        MedicalProcedureKeyword MedicalProcedureKeyword = Context.MedicalProcedureKeywords.FirstOrDefault(x => x.MedicalProcedureId == MedicalProcedureId);
                        if (MedicalProcedureKeyword != null)
                        {
                            Context.MedicalProcedureKeywords.Where(p => p.MedicalProcedureId == MedicalProcedureId).ToList().ForEach(p => Context.MedicalProcedureKeywords.Remove(p));
                        }
                        MedicalProcedureInfo = Context.MedicalProcedures.Find(MedicalProcedureId);
                        Context.MedicalProcedures.Remove(MedicalProcedureInfo);
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
        public IList<MedicalProcedureCategory> ListMedicalProcedureCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalProcedureCategory> MedicalProcedureCategoryInfo = (from MedicalProcedureCategory in Context.MedicalProcedureCategory where MedicalProcedureCategory.CompanyId == CompanyId select MedicalProcedureCategory).ToList();
                return MedicalProcedureCategoryInfo;
            }
        }

        public IList<MedicalProcedureCategory> ListParentMedicalProcedureCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalProcedureCategory> MedicalProcedureParentCategoryInfo = (from MedicalProcedureCategory in Context.MedicalProcedureCategory where MedicalProcedureCategory.CompanyId == CompanyId where MedicalProcedureCategory.ParentMedicalProcedureCategoryId.Equals(null) select MedicalProcedureCategory).ToList();
                return MedicalProcedureParentCategoryInfo;
            }
        }
        public IList<MedicalProcedureCategory> ListMedicalProcedureCategoryByFilterCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var Filters = new MySqlParameter("@Filter", "%" + Filter + "%");
                var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                var Type = new MySqlParameter("@Type", (int)AccountType.EMPLOYEE);
                string Query = null;
                if (Filter == string.Empty)
                {
                    Query = "SELECT * FROM MedicalProcedureCategory WHERE CompanyId = @CompanyIds AND Name LIKE @Filter UNION SELECT * FROM MedicalProcedureCategory WHERE Id IN(SELECT ParentMedicalProcedureCategoryId FROM MedicalProcedureCategory where CompanyId = @CompanyIds and Name LIKE @Filter)";
                }
                else
                {
                    Query = "SELECT * FROM MedicalProcedureCategory WHERE CompanyId = @CompanyIds AND (Name LIKE @Filter OR Id IN (SELECT MedicalProcedureCategoryId FROM MedicalProcedures WHERE CompanyId = @CompanyIds AND Name LIKE @Filter)) UNION SELECT * FROM MedicalProcedureCategory WHERE Id IN (SELECT ParentMedicalProcedureCategoryId FROM MedicalProcedureCategory WHERE CompanyId = @CompanyIds AND (Id IN (SELECT MedicalProcedureCategoryId FROM MedicalProcedures WHERE CompanyId = @CompanyIds AND Name LIKE @Filter) OR Name LIKE @Filter)) ORDER BY Id";
                }
                IList<MedicalProcedureCategory> MedicalProcedureCategoryInfo = Context.MedicalProcedureCategory.FromSqlRaw(Query, Type, Filters, companyId).ToList();
                return MedicalProcedureCategoryInfo;
            }
        }
        public IList<MedicalProcedure> ListFilterMedicalProcedureByCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalProcedure> MedicalProcedureInfo = (from MedicalProcedure in Context.MedicalProcedures.Include("MedicalProcedureCategory") where MedicalProcedure.CompanyId == CompanyId && (MedicalProcedure.Name.Contains(Filter)) select MedicalProcedure).ToList();
                return MedicalProcedureInfo;
            }
        }

        public IList<MedicalProcedure> FilterMedicalProcedureByNameCodeCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalProcedure> MedicalProcedureInfo = Context.MedicalProcedures
                    .Include("MedicalProcedureCategory")
                    .Where(medicalProcedure => medicalProcedure.CompanyId == CompanyId &&
                                               (medicalProcedure.Name.Contains(Filter) || medicalProcedure.ProcedureCode.Contains(Filter)))
                    .ToList();

                return MedicalProcedureInfo;
            }
        }

        public MedicalProcedureCategory GetMedicalProcedureCategoryInfoById(long MedicalProcedureCategoryId)
        {
            MedicalProcedureCategory MedicalProcedureCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalProcedureCategoryInfo = Context.MedicalProcedureCategory.Find(MedicalProcedureCategoryId);
                if (MedicalProcedureCategoryInfo != null)
                {
                    MedicalProcedureCategoryInfo = Context.MedicalProcedureCategory.Include("ParentMedicalProcedureCategory").Where(p => p.Id == MedicalProcedureCategoryId).First<MedicalProcedureCategory>();
                }
            }
            return MedicalProcedureCategoryInfo;
        }
        public Boolean DeleteMedicalProcedureCategory(long MedicalProcedureCategoryId)
        {
            Boolean Deleted = false;

            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    var medicalProcedureIds = Context.MedicalProcedures
                        .Where(mp => mp.MedicalProcedureCategoryId == MedicalProcedureCategoryId)
                        .Select(mp => mp.Id)
                        .ToList(); 

                    bool hasConsultations = Context.ConsultedProcedures
                        .Any(cp => medicalProcedureIds.Contains(cp.MedicalProcedureId));

                    if (hasConsultations)
                    {
                        return false;
                    }

                    var relatedKeywords = Context.MedicalProcedureKeywords
                        .Where(k => medicalProcedureIds.Contains((long)k.MedicalProcedureId))
                        .ToList();

                    if (relatedKeywords.Any())
                    {
                        Context.MedicalProcedureKeywords.RemoveRange(relatedKeywords);
                        Context.SaveChanges();
                    }

                    var medicalProcedures = Context.MedicalProcedures
                        .Where(mp => medicalProcedureIds.Contains(mp.Id))
                        .ToList();

                    if (medicalProcedures.Any())
                    {
                        Context.MedicalProcedures.RemoveRange(medicalProcedures);
                        Context.SaveChanges();
                    }

                    var medicalProcedureCategory = Context.MedicalProcedureCategory.Find(MedicalProcedureCategoryId);

                    if (medicalProcedureCategory != null)
                    {
                        Context.MedicalProcedureCategory.Remove(medicalProcedureCategory);
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

        public Boolean MedicalProcedureCategoryNameUniqueById(MedicalProcedureCategory MedicalProcedureCategory)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    MedicalProcedureCategory lMedicalProcedureCategory = null;
                    if (MedicalProcedureCategory.Id == 0)
                    {
                        lMedicalProcedureCategory = Context.MedicalProcedureCategory.FirstOrDefault(x => x.Name == MedicalProcedureCategory.Name && x.CompanyId == MedicalProcedureCategory.CompanyId);
                    }
                    else
                    {
                        lMedicalProcedureCategory = Context.MedicalProcedureCategory.FirstOrDefault(x => x.Name == MedicalProcedureCategory.Name && x.CompanyId == MedicalProcedureCategory.CompanyId && !x.Id.Equals(MedicalProcedureCategory.Id));
                    }
                    if (lMedicalProcedureCategory != null)
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
        public MedicalProcedureCategory AddMedicalProcedureCategory(MedicalProcedureCategory MedicalProcedureCategoryInfo)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.MedicalProcedureCategory.Add(MedicalProcedureCategoryInfo);
                Context.SaveChanges();
            }
            return MedicalProcedureCategoryInfo;
        }
        public MedicalProcedureCategory UpdateMedicalProcedureCategory(MedicalProcedureCategory MedicalProcedureCategory)
        {
            MedicalProcedureCategory MedicalProcedureCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalProcedureCategoryInfo = Context.MedicalProcedureCategory.Find(MedicalProcedureCategory.Id);
                if (MedicalProcedureCategoryInfo != null)
                {
                    Context.Entry(MedicalProcedureCategoryInfo).CurrentValues.SetValues(MedicalProcedureCategory);
                    Context.SaveChanges();
                }
            }
            return MedicalProcedureCategoryInfo;
        }
    }
}
