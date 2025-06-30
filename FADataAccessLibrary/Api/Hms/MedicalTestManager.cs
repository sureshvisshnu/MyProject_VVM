using fa.context;
using fa.Data;
using fa.model.Accounting.Masters;
using fa.model.hms.common;
using fa.model.Hms.Master;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Fa.api.Hms
{
    public class MedicalTestManager
    {
        private static volatile MedicalTestManager instance;
        private static object syncRoot = new Object();
        MedicalTestManager()
        {

        }
        public static MedicalTestManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MedicalTestManager();
                    }
                }

                return instance;
            }
        }
        public MedicalTestElement GetMedicalTestElementById(long MedicalTestElementId, long CompanyId)
        {
            MedicalTestElement MedicalTestElementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestElementInfo = Context.MedicalTestElements.FirstOrDefault(x => x.CompanyId == CompanyId && x.Id == MedicalTestElementId);
                        return MedicalTestElementInfo;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestElementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
        }
        public MedicalTest GetMedicalTestByName(MedicalTest MedicalTest)
        {
            MedicalTest MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    MedicalTestInfo = Context.MedicalTests.Include("TestElements").Include("Keywords").FirstOrDefault(x => x.CompanyId == MedicalTest.CompanyId && x.MedicalTestCategoryId == MedicalTest.MedicalTestCategoryId && x.TestCode == MedicalTest.TestCode);
                    return MedicalTestInfo;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    MedicalTestInfo = null;
                    throw (e);
                }
            }
        }
        public MedicalTestUOM GetMedicalTestUOMByName(MedicalTestUOM MedicalTestUOM)
        {
            MedicalTestUOM MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    MedicalTestInfo = Context.MedicalTestUOMs.FirstOrDefault(x => x.Name == MedicalTestUOM.Name && x.CompanyId == MedicalTestUOM.CompanyId);
                    return MedicalTestInfo;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    MedicalTestInfo = null;
                    throw (e);
                }
            }
        }
        public MedicalTestElement GetMedicalTestElementByName(MedicalTestElement MedicalTestElement)
        {
            MedicalTestElement MedicalTestElementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (string.IsNullOrEmpty(MedicalTestElement.Class))
                    {
                        MedicalTestElementInfo = Context.MedicalTestElements.FirstOrDefault(x => x.Name == MedicalTestElement.Name && x.ElementCode == MedicalTestElement.ElementCode && x.CompanyId == MedicalTestElement.CompanyId);
                    }
                    else
                    {
                        MedicalTestElementInfo = Context.MedicalTestElements.FirstOrDefault(x => x.Name == MedicalTestElement.Name && x.Class == MedicalTestElement.Class && x.SubClass == MedicalTestElement.SubClass && x.MedicalTestId == MedicalTestElement.MedicalTestId && x.CompanyId == MedicalTestElement.CompanyId);
                    }
                    return MedicalTestElementInfo;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    MedicalTestElementInfo = null;
                    throw (e);
                }
            }
        }
        public MedicalTest GetMedicalTestById(long MedicalTestId)
        {
            MedicalTest MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestInfo = Context.MedicalTests.Include("MedicalTestCategory").Include("TestElements").Include("Keywords").FirstOrDefault(x => x.Id == MedicalTestId);
                        return MedicalTestInfo;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
        }
        public Boolean MedicalTestNameUniqueById(MedicalTest MedicalTest)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTest lMedicalTest = null;
                if (MedicalTest.Id == 0)
                {
                    lMedicalTest = Context.MedicalTests.FirstOrDefault(x => x.TestCode == MedicalTest.TestCode && x.MedicalTestCategoryId == MedicalTest.MedicalTestCategoryId && x.CompanyId == MedicalTest.CompanyId);
                }
                else
                {
                    lMedicalTest = Context.MedicalTests.FirstOrDefault(x => x.TestCode == MedicalTest.TestCode && x.MedicalTestCategoryId == MedicalTest.MedicalTestCategoryId && x.CompanyId == MedicalTest.CompanyId && !x.Id.Equals(MedicalTest.Id));
                }
                if (lMedicalTest != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public Boolean MedicalTestElementNameUniqueById(MedicalTestElement MedicalTestElement)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestElement lMedicalTestElement = null;
                if (MedicalTestElement.Id == 0)
                {
                    if (string.IsNullOrEmpty(MedicalTestElement.Class))
                    {
                        lMedicalTestElement = Context.MedicalTestElements.FirstOrDefault(x => x.Name == MedicalTestElement.Name && x.ElementCode == MedicalTestElement.ElementCode && x.CompanyId == MedicalTestElement.CompanyId && x.MedicalTestId == MedicalTestElement.MedicalTestId);
                    }
                    else
                    {
                        lMedicalTestElement = Context.MedicalTestElements.FirstOrDefault(x => x.Name == MedicalTestElement.Name && x.Class == MedicalTestElement.Class && x.SubClass == MedicalTestElement.SubClass && x.MedicalTestId == MedicalTestElement.MedicalTestId && x.CompanyId == MedicalTestElement.CompanyId && x.MedicalTestId == MedicalTestElement.MedicalTestId);
                    }
                }
                else
                {
                    lMedicalTestElement = Context.MedicalTestElements.FirstOrDefault(x => x.Name == MedicalTestElement.Name && x.ElementCode == MedicalTestElement.ElementCode && x.CompanyId == MedicalTestElement.CompanyId && x.MedicalTestId == MedicalTestElement.MedicalTestId && !x.Id.Equals(MedicalTestElement.Id));
                }
                if (lMedicalTestElement != null)
                {
                    Status = false;
                }
            }
            return Status;
        }
        public Boolean MedicalTestUomNameUniqueById(MedicalTestUOM MedicalTestUOM)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestUOM lMedicalTestUOM = null;
                if (MedicalTestUOM.Id == 0)
                {
                    lMedicalTestUOM = Context.MedicalTestUOMs.FirstOrDefault(x => x.Name == MedicalTestUOM.Name && x.CompanyId == MedicalTestUOM.CompanyId);
                }
                else
                {
                    lMedicalTestUOM = Context.MedicalTestUOMs.FirstOrDefault(x => x.Name == MedicalTestUOM.Name && x.CompanyId == MedicalTestUOM.CompanyId && !x.Id.Equals(MedicalTestUOM.Id));
                }
                if (lMedicalTestUOM != null)
                {
                    Status = false;
                }
            }
            return Status;
        }

        public IList<MedicalTestCategory> ListMedicalTestCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalTestCategory> MedicalTestCategoryInfo = (from MedicalTestCategory in Context.MedicalTestCategorys where MedicalTestCategory.CompanyId == CompanyId select MedicalTestCategory).ToList();
                return MedicalTestCategoryInfo;
            }
        }

        public IList<string> ListMedicalTestSampleByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<string> MedicalTestSampleInfo = (from MedicalTest in Context.MedicalTests where MedicalTest.CompanyId == CompanyId select MedicalTest).Select(x => x.SampleRequirement).Distinct().ToList();
                return MedicalTestSampleInfo;
            }
        }

        public IList<MedicalTestCategory> ListParentMedicalTestCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalTestCategory> MedicalTestParentCategoryInfo = (from MedicalTestCategory in Context.MedicalTestCategorys where MedicalTestCategory.CompanyId == CompanyId where MedicalTestCategory.ParentMedicalTestCategoryId.Equals(null) select MedicalTestCategory).ToList();
                return MedicalTestParentCategoryInfo;
            }
        }

        public IList<MedicalTestCategory> ListMedicalTestCategoryByFilterCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var Filters = new MySqlParameter("@Filter", "%" + Filter + "%");
                var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                var Type = new MySqlParameter("@Type", (int)AccountType.EMPLOYEE);
                string Query = null;
                if (Filter == string.Empty)
                {
                    Query = "SELECT * FROM MedicalTestCategorys WHERE CompanyId = @CompanyIds AND Name LIKE @Filter UNION SELECT * FROM MedicalTestCategorys WHERE Id IN(SELECT ParentMedicalTestCategoryId FROM MedicalTestCategorys where CompanyId = @CompanyIds and Name LIKE @Filter)";
                }
                else
                {
                    //Query = "SELECT * FROM MedicalTestCategorys WHERE CompanyId = @CompanyIds AND Id IN(SELECT MedicalTestCategoryId FROM MedicalTests WHERE CompanyId = @CompanyIds AND Name LIKE @Filter) UNION SELECT * FROM MedicalTestCategorys WHERE Id IN(SELECT ParentMedicalTestCategoryId FROM MedicalTestCategorys WHERE CompanyId = @CompanyIds AND Id IN(SELECT MedicalTestCategoryId FROM MedicalTests WHERE CompanyId = @CompanyIds  and Name LIKE @Filter)) ORDER by Id";
                    Query = "SELECT * FROM MedicalTestCategorys WHERE CompanyId = @CompanyIds AND (Name LIKE @Filter OR Id IN (SELECT MedicalTestCategoryId FROM MedicalTests WHERE CompanyId = @CompanyIds AND Name LIKE @Filter)) UNION SELECT * FROM MedicalTestCategorys WHERE Id IN (SELECT ParentMedicalTestCategoryId FROM MedicalTestCategorys WHERE CompanyId = @CompanyIds AND (Id IN (SELECT MedicalTestCategoryId FROM MedicalTests WHERE CompanyId = @CompanyIds AND Name LIKE @Filter) OR Name LIKE @Filter)) ORDER BY Id";
                }
                IList<MedicalTestCategory> MedicalTestCategoryInfo = Context.MedicalTestCategorys.FromSqlRaw(Query, Type, Filters, companyId).ToList();
                return MedicalTestCategoryInfo;
            }
        }

        public IList<MedicalTest> ListFilterMedicalTestByCompanyId(long CompanyId, string filterString)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                //IList<MedicalTest> MedicalTestInfo = (from MedicalTest in Context.MedicalTests.Include("MedicalTestCategory") where MedicalTest.CompanyId == CompanyId && (MedicalTest.Name.Contains(Filter)) select MedicalTest).ToList();
                //return MedicalTestInfo;
                IList<MedicalTest> MedicalTestInfo = Context.MedicalTests.Include(m => m.Keywords).Include(m => m.TestElements).Where(m => m.CompanyId == CompanyId &&
                                                        (m.Name.Contains(filterString) || m.TestCode.Contains(filterString) || m.TestShortName.Contains(filterString) ||
                                                        m.Keywords.Any(k => k.Text.Contains(filterString)) || m.TestElements.Any(e => e.Name.Contains(filterString)) || 
                                                        m.TestElements.Any(e => e.ElementCode.Contains(filterString)) || m.TestElements.Any(e => e.ElementShortName.Contains(filterString)))).ToList();
                return MedicalTestInfo;
            }
        }

        public MedicalTestCategory GetMedicalTestCategoryInfoById(long MedicalTestCategoryId)
        {
            MedicalTestCategory MedicalTestCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestCategoryInfo = Context.MedicalTestCategorys.Find(MedicalTestCategoryId);
                if (MedicalTestCategoryInfo != null)
                {
                    MedicalTestCategoryInfo = Context.MedicalTestCategorys.Include("ParentMedicalTestCategory").Where(p => p.Id == MedicalTestCategoryId).First<MedicalTestCategory>();
                }
            }
            return MedicalTestCategoryInfo;
        }

        public MedicalTestCategory GetMedicalTestCategoryByName(String MedicalTestInfoName, long CompanyId)
        {
            MedicalTestCategory MedicalTestCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestCategoryInfo = Context.MedicalTestCategorys.FirstOrDefault(x => x.Name == MedicalTestInfoName && x.CompanyId == CompanyId);
                return MedicalTestCategoryInfo;
            }
        }

        public MedicalTest GetMedicalTestsById(long MedicalTestId)
        {
            MedicalTest MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestInfo = Context.MedicalTests.Find(MedicalTestId);
                if (MedicalTestInfo != null)
                {
                    MedicalTestInfo = Context.MedicalTests.Include("MedicalTestCategory").Include("Keywords").FirstOrDefault(x => x.Id == MedicalTestId);
                }
                return MedicalTestInfo;
            }
        }
        public List<MedicalTest> GetMedicalTestsByCompanyId(long CompanyId)
        {
            List<MedicalTest> MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestInfo = Context.MedicalTests.Include("MedicalTestCategory").Include("Keywords").Where(x => x.CompanyId == CompanyId).ToList();
                return MedicalTestInfo;
            }
        }

        public Boolean MedicalTestCategoryNameUniqueById(MedicalTestCategory MedicalTestCategory)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    MedicalTestCategory lMedicalTestCategory = null;
                    if (MedicalTestCategory.Id == 0)
                    {
                        lMedicalTestCategory = Context.MedicalTestCategorys.FirstOrDefault(x => x.Name == MedicalTestCategory.Name && x.CompanyId == MedicalTestCategory.CompanyId);
                    }
                    else
                    {
                        lMedicalTestCategory = Context.MedicalTestCategorys.FirstOrDefault(x => x.Name == MedicalTestCategory.Name && x.CompanyId == MedicalTestCategory.CompanyId && !x.Id.Equals(MedicalTestCategory.Id));
                    }
                    if (lMedicalTestCategory != null)
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
        public MedicalTestCategory AddMedicalTestCategory(MedicalTestCategory MedicalTestCategoryInfo)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.MedicalTestCategorys.Add(MedicalTestCategoryInfo);
                Context.SaveChanges();
            }
            return MedicalTestCategoryInfo;
        }
        public MedicalTestCategory UpdateMedicalTestCategory(MedicalTestCategory MedicalTestCategory)
        {
            MedicalTestCategory MedicalTestCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestCategoryInfo = Context.MedicalTestCategorys.Find(MedicalTestCategory.Id);
                if (MedicalTestCategoryInfo != null)
                {
                    Context.Entry(MedicalTestCategoryInfo).CurrentValues.SetValues(MedicalTestCategory);
                    Context.SaveChanges();
                }
            }
            return MedicalTestCategoryInfo;
        }

        public Boolean DeleteMedicalTest(long MedicalTestId)
        {
            Boolean Deleted = false;
            MedicalTest MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestElement MedicalTestElement = Context.MedicalTestElements.FirstOrDefault(x => x.MedicalTestId == MedicalTestId);
                        if (MedicalTestElement != null)
                        {
                            Context.MedicalTestElements.Where(p => p.MedicalTestId == MedicalTestId).ToList().ForEach(p => Context.MedicalTestElements.Remove(p));
                        }
                        MedicalTestKeyword MedicalTestKeyword = Context.MedicalTestKeywords.FirstOrDefault(x => x.MedicalTestId == MedicalTestId);
                        if (MedicalTestKeyword != null)
                        {
                            Context.MedicalTestKeywords.Where(p => p.MedicalTestId == MedicalTestId).ToList().ForEach(p => Context.MedicalTestKeywords.Remove(p));
                        }
                        MedicalTestInfo = Context.MedicalTests.Find(MedicalTestId);
                        Context.MedicalTests.Remove(MedicalTestInfo);
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

        public Boolean DeleteMedicalTestCategory(long MedicalTestCategoryId)
        {
            Boolean Deleted = false;
            MedicalTestCategory MedicalTestCategoryInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    MedicalTestCategoryInfo = Context.MedicalTestCategorys.Find(MedicalTestCategoryId);
                    Context.MedicalTestCategorys.Remove(MedicalTestCategoryInfo);
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

        public MedicalTest AddMedicalTest(MedicalTest medicalTest)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.MedicalTests.Add(medicalTest);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        medicalTest = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return medicalTest;
        }
        public MedicalTest UpdateMedicalTest(MedicalTest MedicalTest)
        {
            MedicalTest MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestInfo = Context.MedicalTests.Find(MedicalTest.Id);
                        if (MedicalTestInfo != null)
                        {
                            IList<MedicalTestElement> TestElements = Context.MedicalTestElements.Where(x => x.MedicalTestId == MedicalTest.Id).ToList();

                            foreach (MedicalTestElement OldMedicalTestElement in TestElements)
                            {
                                MedicalTestElement NewMedicalTestElement = MedicalTest.TestElements.FirstOrDefault(x => x.Id == OldMedicalTestElement.Id);
                                if (NewMedicalTestElement == null)
                                {
                                    ConsultedLabTestElements Element = Context.ConsultedLabTestElements.FirstOrDefault(x=>x.MedicalTestElementId== OldMedicalTestElement.Id);
                                    if (Element == null)
                                    {
                                        MedicalTestElement ElementFromDB= Context.MedicalTestElements.Find(OldMedicalTestElement.Id);
                                        Context.MedicalTestElements.Remove(ElementFromDB);
                                        Context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    MedicalTest.TestElements.Remove(NewMedicalTestElement);
                                    NewMedicalTestElement.MedicalTestId = MedicalTest.Id;
                                    Context.Entry(OldMedicalTestElement).CurrentValues.SetValues(NewMedicalTestElement);
                                    Context.SaveChanges();
                                }
                            }

                            IList<MedicalTestKeyword> Keywords = Context.MedicalTestKeywords.Where(x => x.MedicalTestId == MedicalTest.Id).ToList();

                            foreach (MedicalTestKeyword OldMedicalTestKeyword in Keywords)
                            {
                                MedicalTestKeyword NewMedicalTestKeyword = MedicalTest.Keywords.FirstOrDefault(x => x.Id == OldMedicalTestKeyword.Id);
                                if (NewMedicalTestKeyword == null)
                                {
                                    Context.MedicalTestKeywords.Remove(Context.MedicalTestKeywords.Find(OldMedicalTestKeyword.Id));
                                    Context.SaveChanges();
                                }
                                else
                                {
                                    MedicalTest.Keywords.Remove(NewMedicalTestKeyword);
                                    NewMedicalTestKeyword.MedicalTestId = MedicalTest.Id;
                                    Context.Entry(OldMedicalTestKeyword).CurrentValues.SetValues(NewMedicalTestKeyword);
                                    Context.SaveChanges();
                                }
                            }
                           
                            Context.Entry(MedicalTestInfo).CurrentValues.SetValues(MedicalTest);

                            foreach (MedicalTestElement Detail in MedicalTest.TestElements)
                            {
                                Detail.MedicalTestId = MedicalTest.Id;
                                Context.MedicalTestElements.Add(Detail);
                                Context.SaveChanges();
                            }
                            foreach (MedicalTestKeyword Detail in MedicalTest.Keywords)
                            {
                                Detail.MedicalTestId = MedicalTest.Id;
                                Context.MedicalTestKeywords.Add(Detail);
                                Context.SaveChanges();
                            }
                            Context.SaveChanges();
                            dbContextTransaction.Commit();

                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return MedicalTestInfo;
        }
        public IList<MedicalTest> ListMedicalTestByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalTest> MedicalTestInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestInfo = (from MedicalTest in Context.MedicalTests.Include("MedicalTestCategory").Include("TestElements").Include("TestElements.Uom").Include("TestElements").Include("Keywords") where MedicalTest.CompanyId == CompanyId select MedicalTest).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalTestInfo;
                }
            }
        }
        public IList<MedicalTest> ListActiveMedicalTestByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalTest> MedicalTestInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestInfo = (from MedicalTest in Context.MedicalTests.Include("Keywords").Include("TestElements") where MedicalTest.CompanyId == CompanyId && MedicalTest.IsActive == true select MedicalTest).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalTestInfo;
                }
            }
        }
        public IList<MedicalTestElement> ListMedicalTestElementByMedicalTestId(long MedicalTestId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalTestElement> MedicalTestElementInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestElementInfo = (from MedicalTestElement in Context.MedicalTestElements where MedicalTestElement.MedicalTestId == MedicalTestId select MedicalTestElement).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestElementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalTestElementInfo;
                }
            }
        }
        public IList<MedicalTestElement> ListMedicalTestElementByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalTestElement> MedicalTestElementInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestElementInfo = (from MedicalTestElement in Context.MedicalTestElements where MedicalTestElement.CompanyId == CompanyId select MedicalTestElement).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestElementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalTestElementInfo;
                }
            }
        }
        public MedicalTestUOM GetMedicalTestUOMByName(string MedicalTestUOMName, long CompanyId)
        {
            MedicalTestUOM MedicalTestUOM = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestUOM = Context.MedicalTestUOMs.FirstOrDefault(x => x.Name == MedicalTestUOMName && x.CompanyId==CompanyId);
            }
            return MedicalTestUOM;
        }
        
        public IList<MedicalTestUOM> ListAllMedicalTestUOMByCompanyId(long CompanyId)
        {
            IList<MedicalTestUOM> MedicalTestUOM = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestUOM = (from UOM in Context.MedicalTestUOMs where (UOM.CompanyId == CompanyId) select UOM).ToList();
                return MedicalTestUOM;
            }
        }
        public IList<MedicalTestUOM> ListAllNonEmptyMedicalTestUOMByCompanyId(long CompanyId)
        {
            IList<MedicalTestUOM> MedicalTestUOM = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                MedicalTestUOM = (from UOM in Context.MedicalTestUOMs where (UOM.CompanyId == CompanyId && UOM.Name != string.Empty) select UOM).ToList();
                return MedicalTestUOM;
            }
        }
        public MedicalTestUOM AddMedicalTestUom(MedicalTestUOM medicalTestUOM)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.MedicalTestUOMs.Add(medicalTestUOM);
                        Context.SaveChanges();
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
            return medicalTestUOM;
        }
        public MedicalTestElement AddMedicalTestElement(MedicalTestElement medicalTestElement)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.MedicalTestElements.Add(medicalTestElement);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        return medicalTestElement;
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
        }


        public MedicalTest UpdateMedicalTestFromUpload(MedicalTest MedicalTest)
        {
            MedicalTest MedicalTestInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestInfo = Context.MedicalTests.Find(MedicalTest.Id);
                        if (MedicalTestInfo != null)
                        {                           
                            IList<MedicalTestKeyword> Keywords = Context.MedicalTestKeywords.Where(x => x.MedicalTestId == MedicalTest.Id).ToList();
                            if (Keywords != null)
                            {
                                Context.MedicalTestKeywords.Where(p => p.MedicalTestId == MedicalTest.Id).ToList().ForEach(p => Context.MedicalTestKeywords.Remove(p));
                            }
                            if (MedicalTest.Keywords != null)
                            {
                                foreach (MedicalTestKeyword Detail in MedicalTest.Keywords)
                                {
                                    Detail.MedicalTestId = MedicalTest.Id;
                                    Context.MedicalTestKeywords.Add(Detail);
                                    Context.SaveChanges();
                                }
                            }
                            MedicalTest.Keywords = null;
                            Context.Entry(MedicalTestInfo).CurrentValues.SetValues(MedicalTest);                            
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        MedicalTestInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return MedicalTestInfo;
        }

        public MedicalTestUOM UpdateMedicalTestUomFromUpload(MedicalTestUOM MedicalTestUOM)
        {
            MedicalTestUOM MedicalTestUOMInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestUOMInfo = Context.MedicalTestUOMs.Find(MedicalTestUOM.Id);
                        if (MedicalTestUOMInfo != null)
                        {                         
                            Context.Entry(MedicalTestUOMInfo).CurrentValues.SetValues(MedicalTestUOM);                          
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        MedicalTestUOMInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return MedicalTestUOMInfo;
        }
        public MedicalTestElement UpdateMedicalTestElementFromUpload(MedicalTestElement MedicalTestElement)
        {
            MedicalTestElement MedicalTestElementInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestElementInfo = Context.MedicalTestElements.Find(MedicalTestElement.Id);
                        if (MedicalTestElementInfo != null)
                        {
                            Context.Entry(MedicalTestElementInfo).CurrentValues.SetValues(MedicalTestElement);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        MedicalTestElementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return MedicalTestElementInfo;
        }

        public IList<string> ListMedicalTestElementRangeTypes(long companyId, string typedText)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<string> MedicalTestElementTypes;
                MedicalTestElementTypes = Context.MedicalTestElements.Where(x => x.CompanyId == companyId && x.Class.Contains(typedText)).Select(x => x.Class).ToList();
                return MedicalTestElementTypes;
            }
        }

        public IList<MedicalTestElement> GetMedicalTestElementByElementCode(string elementCode, long companyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<MedicalTestElement> MedicalTestElementInfo;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        MedicalTestElementInfo = (from MedicalTestElement in Context.MedicalTestElements where MedicalTestElement.CompanyId == companyId && MedicalTestElement.ElementCode == elementCode select MedicalTestElement).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        MedicalTestElementInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return MedicalTestElementInfo;
                }
            }
        }

        public List<MedicalTest> GetMedicalTestByCategoryId(long MedicalTestCategoryId, long companyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<MedicalTest> medicalTest = null;
                medicalTest = Context.MedicalTests.Include("MedicalTestCategory").Include("TestElements").Include("Keywords").Where(x => x.CompanyId == companyId && x.MedicalTestCategoryId == MedicalTestCategoryId).ToList();
                return medicalTest;
            }
        }

        public List<MedicalTestCategory> GetChildMedicalTestCategoryByParentId(long? parentMedicalTestCategoryId, long companyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<MedicalTestCategory> lChildCategory = null;
                lChildCategory = Context.MedicalTestCategorys.Include("ParentMedicalTestCategory").Where(x => x.CompanyId == companyId && x.ParentMedicalTestCategoryId == parentMedicalTestCategoryId).ToList();
                return lChildCategory;
            }
        }
        public List<MedicalTestCategory> GetMedicalTestCatagoryByCompanyId(long companyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<MedicalTestCategory> medicalTestCategory = null;
                medicalTestCategory = Context.MedicalTestCategorys.Include("ParentMedicalTestCategory").Where(x => x.CompanyId == companyId && x.ParentMedicalTestCategoryId == null).ToList();
                return medicalTestCategory;
            }
        }
    }
}
