using fa.context;
using fa.model.Accounting.Masters;
using fa.model.Employee;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using static System.Reflection.Metadata.BlobBuilder;

namespace fa.api.Accounting
{
    public class DepartmentManager
    {
        private static volatile DepartmentManager instance;
        private static object syncRoot = new Object();
        DepartmentManager()
        {

        }
        public static DepartmentManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DepartmentManager();
                    }
                }

                return instance;
            }
        }
       
        public Department GetDepartmentInfoById(long DepartmentId)
        {
            Department DepartmentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DepartmentInfo = Context.Departments.Find(DepartmentId);
                if (DepartmentInfo != null)
                {
                    DepartmentInfo = Context.Departments.Include("ParentDepartment").Where(p => p.Id == DepartmentId).First<Department>();
                }
            }
            return DepartmentInfo;
        }
        public static Department GetDepartmentInfoByIdForReport(long DepartmentId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                return Context.Departments
                              .Include("ParentDepartment")
                              .SingleOrDefault(d => d.Id == DepartmentId);
            }
        }
        public IList<Department> ListParentDepartmentByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Department> DepartmentInfo = (from Department in Context.Departments where Department.CompanyId == CompanyId where Department.ParentDepartmentId.Equals(null) select Department).ToList();
                return DepartmentInfo;
            }
        }        
        public IList<Department> ListDepartmentByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Department> DepartmentInfo = (from Department in Context.Departments where Department.CompanyId == CompanyId select Department).ToList();
                return DepartmentInfo;
            }
        }
        public IList<Department> ListDepartmentByFilterCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                var Filters = new MySqlParameter("@Filter", "%" + Filter + "%");
                var companyId = new MySqlParameter("@CompanyIds", CompanyId);
                var Type = new MySqlParameter("@Type", (int)AccountType.EMPLOYEE);
                string Query = null;
                Query = "SELECT * FROM departments WHERE CompanyId = @CompanyIds AND Name LIKE @Filter UNION SELECT * FROM departments WHERE Id IN(SELECT ParentDepartmentId FROM departments where CompanyId = @CompanyIds and Name LIKE @Filter)";
                //if (Filter == string.Empty)
                //{
                //    Query = "SELECT * FROM departments WHERE CompanyId = @CompanyIds AND Name LIKE @Filter UNION SELECT * FROM departments WHERE Id IN(SELECT ParentDepartmentId FROM departments where CompanyId = @CompanyIds and Name LIKE @Filter)";
                //}
                //else
                //{
                //    Query = "SELECT * FROM departments WHERE CompanyId = @CompanyIds AND Id IN(SELECT DepartmentId FROM Employees WHERE CompanyId = @CompanyIds AND Name LIKE @Filter) UNION SELECT * FROM departments WHERE Id IN(SELECT ParentDepartmentId FROM departments WHERE CompanyId = @CompanyIds AND Id IN(SELECT DepartmentId FROM Employees WHERE CompanyId = @CompanyIds  and Name LIKE @Filter)) ORDER by Id";
                //}
                IList<Department> DepartmentInfo = Context.Departments.FromSqlRaw(Query, Type, Filters, companyId).AsNoTracking().ToList();
                return DepartmentInfo;
            }
        }
        public IList<Department> ListDepartmentByDepartmentIdCompanyId(long DepartmentId,long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Department> DepartmentInfo = (from Department in Context.Departments where Department.ParentDepartmentId == DepartmentId where Department.CompanyId == CompanyId select Department).ToList();
                return DepartmentInfo;
            }
        }
        public Boolean DeleteDepartment(long DepartmentId)
        {
            Boolean Deleted = false;
            Department DepartmentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    DepartmentInfo = Context.Departments.Find(DepartmentId);
                    Context.Departments.Remove(DepartmentInfo);
                    Context.SaveChanges();
                    Deleted = true;
                }
                #pragma warning disable 0168 // variable declared but not used.
                catch (Exception e)
                {
                    Deleted = false;
                }
                #pragma warning restore 0168
            }
            return Deleted;
        }
        public Department AddDepartment(Department department)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.Departments.Add(department);
                Context.SaveChanges();
            }
            return department;
        }
        public Department UpdateDepartment(Department Department)
        {
            Department DepartmentInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                DepartmentInfo = Context.Departments.Find(Department.Id);
                if (DepartmentInfo != null)
                {
                    Context.Entry(DepartmentInfo).CurrentValues.SetValues(Department);
                    Context.SaveChanges();
                }
            }
            return DepartmentInfo;
        }
        public Boolean DepartmentNameUniqueById(Department Department)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    Department lDepartment = null;
                    if (Department.Id == 0)
                    {
                        lDepartment= Context.Departments.FirstOrDefault(x => x.Name == Department.Name && x.CompanyId == Department.CompanyId);                  
                    }
                    else
                    {
                        lDepartment = Context.Departments.FirstOrDefault(x => x.Name == Department.Name && x.CompanyId == Department.CompanyId && !x.Id.Equals(Department.Id));
                    }
                    if (lDepartment != null)
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

        public Department GetDepartmentByName(string department, long companyId)
        {
            Department ldepartment = null!;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                ldepartment = Context.Departments.FirstOrDefault( x => x.CompanyId == companyId && x.Name == department);
                if (ldepartment != null)
                {
                    return ldepartment;
                }
                return null;
            }
        }
    }
}
