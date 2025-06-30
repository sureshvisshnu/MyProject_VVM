using fa.context;
using fa.model.Employee;
using Microsoft.EntityFrameworkCore;

namespace fa.api.Accounting
{
    public class EmployeeManager
    {
        private static volatile EmployeeManager instance;
        private static object syncRoot = new Object();
        EmployeeManager()
        {

        }
        public static EmployeeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new EmployeeManager();
                    }
                }

                return instance;
            }
        }
        public Employee GetEmployeeInfoById(long EmployeeId)
        {
            Employee EmployeeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                EmployeeInfo = Context.Employees.FirstOrDefault(x=>x.Id==EmployeeId);
                if (EmployeeInfo != null)
                {
                    EmployeeInfo = Context.Employees.Include("Address").Include("ContactInfo").Include("TaxInfo").Include("Department").Include("Title").Where(p => p.Id == EmployeeId).First<Employee>();
                }
            }
            return EmployeeInfo;
        }
        public Employee GetEmployeeInfoByIdType(long EmployeeId)
        {
            Employee EmployeeInfo = null;
            string TitleDr = "Doctor";
            string TitleNr = "Nurse";
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                EmployeeInfo = Context.Employees.FirstOrDefault(x => x.Id == EmployeeId);
                if (EmployeeInfo != null)
                {
                    EmployeeInfo = Context.Employees
                                    .Include("Address")
                                    .Include("ContactInfo")
                                    .Include("TaxInfo")
                                    .Include("Department")
                                    .Include("Title")
                                    .Where(p => p.Id == EmployeeId && (p.Title.Name == TitleDr || p.Title.Name == TitleNr))
                                    .FirstOrDefault();
                }
            }
            return EmployeeInfo;
        }
        public IList<Employee> ListFilterEmployeeByCompanyId(long CompanyId, string Filter)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Employee> EmployeeInfo = (from Employee in Context.Employees.Include("ContactInfo").Include("Department") where Employee.CompanyId == CompanyId && (Employee.Name.Contains(Filter) || Employee.ContactInfo.Phone.Contains(Filter)) select Employee).ToList();
                return EmployeeInfo;
            }
        }
        public IList<Employee> ListEmployeeByDepartmentId(long DepartmentId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Employee> EmployeeInfo = (from Employee in Context.Employees.Include("Department") where Employee.DepartmentId == DepartmentId select Employee).ToList();
                return EmployeeInfo;
            }
        }

        public IList<Employee> ListEmployeeByCompanyId(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Employee> EmployeeInfo = (from Employee in Context.Employees.Include("Department").Include("AccountGroup") where Employee.CompanyId == CompanyId select Employee).ToList();
                return EmployeeInfo;
            }
        }
        public IList<Employee> ListEmployeeByServiceProvider(long CompanyId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Employee> EmployeeInfo = (from Employee in Context.Employees.Include("Department").Include("AccountGroup") where Employee.CompanyId == CompanyId && Employee.IsServiceProvider == true select Employee).ToList();
                return EmployeeInfo;
            }
        }
        public IList<Employee> ListEmployeeByCompanyIdTitle(long CompanyId,string Title)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Employee> EmployeeInfo = (from Employee in Context.Employees.Include("Department").Include("AccountGroup") where Employee.CompanyId == CompanyId && Employee.Title.Name== Title select Employee).ToList();
                return EmployeeInfo;
            }
        }
        public IList<Employee> ListEmployeeByCompanyIdTitleForReport(long CompanyId, params string[] Titles)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Employee> EmployeeInfo = (from Employee in Context.Employees.Include("Department").Include("AccountGroup") where Employee.CompanyId == CompanyId && Titles.Contains(Employee.Title.Name) select Employee).ToList();
                return EmployeeInfo;
            }
        }
        public IList<Employee> ListEmployeeByCompanyIdWithFilter(string SearchText, long CompanyId )
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if(!String.IsNullOrEmpty(SearchText))
                {
                    IList<Employee> EmployeeInfo = (from Employee in Context.Employees.Include("Department").Include("AccountGroup").Include("ContactInfo") where (Employee.CompanyId == CompanyId && (Employee.Name.Contains(SearchText) || Employee.ContactInfo.Phone.Contains(SearchText))) select Employee).ToList();
                    return EmployeeInfo;
                }
                else
                {
                    return ListEmployeeByCompanyId(CompanyId);
                }
            }
        }

        public Boolean DeleteEmployee(long EmployeeId)
        {          
            bool deleted = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Employee Employee = Context.Employees.Include("Address").Include("ContactInfo").Include("TaxInfo").Where(p => p.Id == EmployeeId).First<Employee>();
                        if (Employee != null)
                        {
                            if (Employee.Address != null)
                            {
                                Context.Addresses.Where(add => add.AddressId == Employee.Address.AddressId).ToList().ForEach(add => Context.Addresses.Remove(add));
                            }
                            if (Employee.ContactInfo != null)
                            {
                                Context.ContactInfos.Where(ci => ci.Id == Employee.ContactInfo.Id).ToList().ForEach(ci => Context.ContactInfos.Remove(ci));
                            }
                            if (Employee.TaxInfo != null)
                            {
                                Context.TaxInfos.Where(ti => ti.Id == Employee.TaxInfo.Id).ToList().ForEach(ti => Context.TaxInfos.Remove(ti));
                            }
                        }
                        Context.Employees.Remove(Employee);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                        deleted = true;
                    }
                    #pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }
            return deleted;

        }
        public Employee AddEmployee(Employee employee)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        Context.Employees.Add(employee);
                        Context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        employee = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return employee;
        }
        public Employee UpdateEmployee(Employee Employee)
        {
            Employee EmployeeInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                EmployeeInfo = Context.Employees.Find(Employee.Id);
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    try
                    {
                        if (EmployeeInfo != null)
                        {
                            Context.Entry(EmployeeInfo).CurrentValues.SetValues(Employee);
                            Context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        EmployeeInfo = null;
                        dbContextTransaction.Rollback();
                        throw (e);
                    }
                }
            }
            return EmployeeInfo;
        }
        public Boolean EmployeeNameUniqueById(Employee Employee)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    Employee lEmployee = null;
                    if (Employee.Id == 0)
                    {
                        //lEmployee = Context.Employees.FirstOrDefault(x => x.DisplayAs == Employee.DisplayAs && x.Name== Employee.Name && x.CompanyId == Employee.CompanyId);
                        lEmployee = Context.Employees.FirstOrDefault(x => x.Name == Employee.Name && x.CompanyId == Employee.CompanyId);
                    }
                    else
                    {
                        //lEmployee = Context.Employees.FirstOrDefault(x => x.DisplayAs == Employee.DisplayAs && x.Name == Employee.Name && x.CompanyId == Employee.CompanyId && !x.Id.Equals(Employee.Id));
                        lEmployee = Context.Employees.FirstOrDefault(x => x.Name == Employee.Name && x.CompanyId == Employee.CompanyId && !x.Id.Equals(Employee.Id));
                    }
                    if (lEmployee != null)
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

        public Employee GetEmployeeByName(string employee, long companyId)
        {
            Employee lEmployee = null!;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                lEmployee = Context.Employees.FirstOrDefault(x => x.CompanyId == companyId && x.Name == employee);
                if (lEmployee != null)
                {
                    return lEmployee;
                }
                return null;
            }
        }
    }
}
