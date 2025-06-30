using fa.context;
using fa.model.Catalog;
using fa.model.Hms.Master;
using FaData.Utils;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Diagnostics;

namespace fa.api.Hms
{
    public class DocumentManager
    {
        private static volatile DocumentManager instance;
        private static object syncRoot = new Object();
        DocumentManager()
        {

        }
        public static DocumentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DocumentManager();
                    }
                }

                return instance;
            }
        }
        public bool CheckFileNameExists(long CategoryId,long PatientId,string Name)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientDocument PatientDocument = Context.PatientDocuments.FirstOrDefault(x => x.PatientDocumentCategoryId == CategoryId && x.PatientId==PatientId && x.FileName==Name);
                if(PatientDocument!=null)
                {
                    return false;
                }
            }
            return true;
        }
        public DocumentCategory GetDocumentCategoryById(long CategoryId)
        {
            DocumentCategory PatientDocumentCategory = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientDocumentCategory = Context.DocumentCategories.FirstOrDefault(x=>x.Id== CategoryId);
            }
            return PatientDocumentCategory;
        }
        public PatientDocument GetPatientDocumentById(long Id)
        {
            PatientDocument PatientDocument = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                PatientDocument = Context.PatientDocuments.FirstOrDefault(x => x.Id == Id);
            }
            return PatientDocument;
        }
        public IList<DocumentCategory> ListPatientDocumentCategoryByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<DocumentCategory> PatientDocumentCategory;

                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientDocumentCategory = (from Category in Context.DocumentCategories where Category.CompanyId == CompanyId select Category).ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PatientDocumentCategory = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return PatientDocumentCategory;
                }
            }
        }
        public IList<DocumentCategory> ListDocumentCategoryByCompanyId(long CompanyId, String Stxt)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                List<DocumentCategory> lCategoryInfo = new List<DocumentCategory>();
                try
                {
                    string Query = null;
                    if (Stxt == null || Stxt == string.Empty)
                    {
                        //var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                        //Query = "SELECT P.* FROM CatalogItems P WHERE P.CompanyId = @CompanyIds order by P.Id";
                        lCategoryInfo = (from cat in Context.DocumentCategories where cat.CompanyId == CompanyId select cat).ToList();
                    }
                    else
                    {

                        var Filters = new MySqlParameter("@Filter", "%" + Stxt + "%");
                        var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                        Query = "SELECT C.* FROM DocumentCategories C LEFT JOIN DocumentCategories P ON C.Id = P.ParentId LEFT JOIN DocumentCategories G ON P.Id = G.ParentId LEFT JOIN DocumentCategories L ON G.Id = L.ParentId LEFT JOIN DocumentCategories R ON L.Id = R.ParentId WHERE R.Id IN (SELECT Id FROM DocumentCategories WHERE Id IN (SELECT Id FROM DocumentCategories WHERE Name LIKE @Filter AND CompanyId = @CompanyIds)) GROUP BY G.ParentId UNION SELECT C.* FROM DocumentCategories C LEFT JOIN DocumentCategories P ON C.Id = P.ParentId LEFT JOIN DocumentCategories G ON P.Id = G.ParentId LEFT JOIN DocumentCategories L ON G.Id = L.ParentId LEFT JOIN DocumentCategories R ON L.Id = R.ParentId WHERE L.Id IN (SELECT Id from DocumentCategories WHERE Id IN (SELECT Id FROM DocumentCategories WHERE Name LIKE @Filter AND CompanyId = @CompanyIds)) GROUP BY P.ParentId UNION SELECT C.* FROM DocumentCategories C LEFT JOIN DocumentCategories P ON C.Id = P.ParentId LEFT JOIN DocumentCategories G ON P.Id = G.ParentId LEFT JOIN DocumentCategories L ON G.Id = L.ParentId LEFT JOIN DocumentCategories R ON L.Id = R.ParentId WHERE G.Id IN (SELECT Id FROM DocumentCategories WHERE Id IN (SELECT Id FROM DocumentCategories WHERE Name LIKE @Filter AND CompanyId = @CompanyIds)) UNION SELECT C.* FROM DocumentCategories C LEFT JOIN DocumentCategories P ON C.Id = P.ParentId LEFT JOIN DocumentCategories G ON P.Id = G.ParentId LEFT JOIN DocumentCategories L ON G.Id = L.ParentId LEFT JOIN DocumentCategories R ON L.Id = R.ParentId WHERE P.Id IN (SELECT Id FROM DocumentCategories WHERE Id IN (SELECT Id FROM DocumentCategories WHERE Name LIKE @Filter AND CompanyId = @CompanyIds)) UNION SELECT C.* FROM DocumentCategories C LEFT JOIN DocumentCategories P ON C.Id = P.ParentId LEFT JOIN DocumentCategories G ON P.Id = G.ParentId LEFT JOIN DocumentCategories L ON G.Id = L.ParentId LEFT JOIN DocumentCategories R ON L.Id = R.ParentId WHERE C.Id IN (SELECT Id FROM DocumentCategories WHERE Id IN (SELECT Id FROM DocumentCategories WHERE Name LIKE @Filter AND CompanyId = @CompanyIds))";
                        lCategoryInfo = Context.DocumentCategories.FromSqlRaw(Query, Filters, companyId).ToList();
                    }
                }
                catch (MySqlException ex)
                {
                    int errorcode = ex.Number;
                    if (ex.HResult == -2147467259)
                    {
                        Console.WriteLine("Query was stoped: " + ex.HResult);
                    }
                }
                return lCategoryInfo;
            }
        }        
        public IList<PatientDocument> ListPatientDocumentByCategoryId(long CategoryId,long PatientId)
        {
            IList < PatientDocument> PatientDocument = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientDocument = Context.PatientDocuments.Where(x => x.PatientDocumentCategoryId == CategoryId && x.PatientId== PatientId).ToList<PatientDocument>();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        PatientDocument = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                    return PatientDocument;
                }
            }
        }
        public bool CheckGlobalMaxAllowedPackets()
        {
            bool GlobalMaxAllowedPacket = false;
            bool CheckAllowedPacket = false;

            string ConString = "datasource=localhost;uid=admin;password=adminpass;database=vv-matrix;";
            string Query = null;
            Query = "SELECT @@global.max_allowed_packet"; // "SELECT @@global.max_allowed_packet as 'AllowedPacket'";
                        
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {               
                con.Open();

                MySqlCommand cmd = new MySqlCommand(Query, con);

                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();
                int ident = Int32.Parse(dataReader.GetValue(0).ToString());
                if(ident < 10485759)
                {
                    GlobalMaxAllowedPacket = false;
                    dataReader.Close();
                    con.Close();
                    CheckAllowedPacket = SetGlobalMaxAllowedPackets();
                    GlobalMaxAllowedPacket = CheckAllowedPacket;
                }
                else
                {
                    dataReader.Close();
                    con.Close();
                    GlobalMaxAllowedPacket = true;
                }                   
            }
            catch(Exception ex)
            {
                con.Close();
                GlobalMaxAllowedPacket = false;
                Console.WriteLine(ex.HResult);
            }
            return GlobalMaxAllowedPacket;
        }
        public bool SetGlobalMaxAllowedPackets()
        {
            bool GlobalMaxAllowedPacket = false;

            string ConString = "datasource=localhost;uid=admin;password=adminpass;database=vv-matrix;";
            string Query = null;
            Query = "SET @@global.max_allowed_packet = 10485760 ";
            MySqlConnection con = new MySqlConnection(ConString);
            try
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(Query, con);

                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                con.Close();
                ReStartDatabase();
                GlobalMaxAllowedPacket = true;

            }
            catch (Exception ex)
            {
                con.Close();
                GlobalMaxAllowedPacket = false;
                Console.WriteLine(ex.HResult);
            }
            return GlobalMaxAllowedPacket;
        }
        public void ReStartDatabase()
        {
            Process mysqlCMD = new Process();
            mysqlCMD.StartInfo.FileName = Environment.CurrentDirectory + "\\mysql-server\\bin\\mysqld.exe";
            mysqlCMD.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // This is to hide the black CMD window
            mysqlCMD.Start();

        }
        public PatientDocument AddPatientDocument(PatientDocument patientDocument)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.PatientDocuments.Add(patientDocument);
                Context.SaveChanges();
            }
            return patientDocument;
        }

        public PatientDocument UpdatePatientDocument(PatientDocument PatientDocument)
        {
            PatientDocument PatientDocumentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientDocumentInfo = Context.PatientDocuments.Find(PatientDocument.Id);
                        if (PatientDocumentInfo != null)
                        {
                            Context.Entry(PatientDocumentInfo).CurrentValues.SetValues(PatientDocument);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Logger.LogError(e);
                    }
                }
            }
            return PatientDocumentInfo;
        }
        public Boolean DeletePatientDocument(long DocmentId)
        {
            Boolean Deleted = false;
            PatientDocument PatientDocumentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        PatientDocumentInfo = Context.PatientDocuments.Find(DocmentId);
                        Context.PatientDocuments.Remove(PatientDocumentInfo);
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

    }
}
