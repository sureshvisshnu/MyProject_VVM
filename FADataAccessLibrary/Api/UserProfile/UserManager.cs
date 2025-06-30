using fa.context;
using fa.model.UserProfile;
using fa.model.Common;
using fa.model.Accounting.Masters;
using fa.api.Accounting;
using Microsoft.EntityFrameworkCore;
using fa.Data;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using System.Linq;

namespace fa.api.UserProfile
{
    public class UserManager
    {
        private static volatile UserManager instance;
        private static object syncRoot = new Object();
        UserManager()
        {

        }
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new UserManager();
                    }
                }

                return instance;
            }
        }
        public User GetUserById(long UserId)
        {
            User UserInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                UserInfo = Context.Users.Find(UserId);
                if (UserInfo != null)
                {
                    Context.Users.Include("Roles").Include("Accesses").FirstOrDefault(x => x.UserId == UserId);
                    Context.Entry(UserInfo).Reference("ContactInfo").Load();
                    ContactInfo lContactInfo = UserInfo.ContactInfo;
                }
                return UserInfo;
            }
        }

        public User GetUserByLogin(string Login)
        {
            User UserInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                //return all related object as the context will not be available once the user is returned.
                var Query = from User in Context.Users.Include("Roles").Include("Employee").Include("Employee.Title").Include("Roles.SystemFunctions").Include("Accesses")
                            where (User.Login == Login) 
                            select User;
                foreach (var Result in Query)
                {
                    //Expecting only one user per login, it is safe to assume that the first record is good.
                    UserInfo = Result;
                    UserInfo.Roles.ToList();

                    break;
                }
                return UserInfo;
            }          
        }
        public User GetUserByName(String FirstName, String LastName)
        {
            User UserInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                UserInfo = Context.Users.FirstOrDefault(x => x.FirstName == FirstName && x.LastName == LastName);
                return UserInfo;
            }
        }

        public User CheckUserNameInUpdate(String FirstName, String LastName, long UserId)
        {
            User UserInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                UserInfo = Context.Users.FirstOrDefault(x => x.FirstName == FirstName && x.LastName == LastName && !x.UserId.Equals(UserId));
                return UserInfo;
            }
        }
        public bool FindNameUnique(User User)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (User.UserId == 0)
                    {
                        Context.Users.Where(x => x.FirstName == User.FirstName && x.LastName == User.LastName).First<User>();
                        Status = false;
                    }
                    else
                    {
                        Context.Users.Where(x => x.FirstName == User.FirstName && x.LastName == User.LastName && !x.UserId.Equals(User.UserId)).First<User>();
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
        public bool FindLoginIdUnique(User User)
        {
            bool Status = true;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                try
                {
                    if (User.UserId == 0)
                    {
                        Context.Users.Where(p => p.Login == User.Login).First<User>();
                        Status = false;
                    }
                    else
                    {
                        Context.Users.Where(p =>p.Login == User.Login && p.UserId != User.UserId).First<User>();
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
        public Boolean DeleteUser(long UserId)
        {
            Boolean Deleted = false;          
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        User User = context.Users.Include("Address").Include("ContactInfo").Include("TaxInfo").Include("Roles").Include("Accesses").Where(p => p.UserId == UserId).First<User>();

                        if (User != null)
                        {
                            if (User.Address != null)
                            {
                                context.Addresses.Where(add => add.AddressId == User.Address.AddressId).ToList().ForEach(add => context.Addresses.Remove(add));
                            }
                            if (User.ContactInfo != null)
                            {
                                context.ContactInfos.Where(ci => ci.Id == User.ContactInfo.Id).ToList().ForEach(ci => context.ContactInfos.Remove(ci));
                            }
                            if (User.TaxInfo != null)
                            {
                                context.TaxInfos.Where(ti => ti.Id == User.TaxInfo.Id).ToList().ForEach(ti => context.TaxInfos.Remove(ti));
                            }
                            if(User.Accesses!=null)
                            {
                                context.Accesses.Where(Ac => Ac.UserId == User.UserId).ToList().ForEach(Ac => context.Accesses.Remove(Ac));
                            }

                            //Delete all account transactions
                        }
                        context.Users.Remove(User);
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        Deleted = true;
                    }
                    #pragma warning disable 0168
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        Deleted = false;
                    }
                    #pragma warning restore 0168
                }
            }
            return Deleted;
        }
        public User AddUser(User user)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                Context.Users.Add(user);
                Context.SaveChanges();
            }
            return user;
        }
        public User UpdateUser(User User)
        {
            User UserInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                UserInfo = Context.Users.Find(User.UserId);
                if (UserInfo != null)
                {
                    Context.Entry(UserInfo).CurrentValues.SetValues(User);
                    Context.SaveChanges();
                }
            }
            return UserInfo;
        }
        public IList<User> ListAllUser(BuisnessType BuisnessType)
        {
            IList<User> UserInfo = null;
            List<long> SkipForRetailRolesIds = new List<long>() { 6L,7L,11L,12L,13L};
            List<long> SkipForProfessionalRolesIds = new List<long>() { 4L,5L,6L, 7L,8L,9L,10L, 11L, 12L, 13L };
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (BuisnessType == BuisnessType.Retail || BuisnessType == BuisnessType.Wholesale)
                {
                    UserInfo = Context.Users.Where(x => x.IsSuperAdmin == false && x.Roles.Where(y=>SkipForRetailRolesIds.Contains(y.RoleId)).ToList().Count==0).ToList<User>();
                }
                else if (BuisnessType == BuisnessType.Professionals)
                {
                    UserInfo = Context.Users.Where(x => x.IsSuperAdmin == false && x.Roles.Where(y => SkipForProfessionalRolesIds.Contains(y.RoleId)).ToList().Count == 0).ToList<User>();
                }
                else 
                {
                    UserInfo = Context.Users.Where(x => x.IsSuperAdmin == false).ToList<User>();
                }
                return UserInfo;
            }
        }
        public IList<User> ListAllUserForCounslting(BuisnessType businessType)
        {
            IList<User> userInfo = null;
            List<long> skipRoleIds = new List<long>();
            switch (businessType)
            {
                case BuisnessType.Retail:
                case BuisnessType.Wholesale:
                    skipRoleIds = new List<long> { 6L, 7L, 11L, 12L, 13L };
                    break;
                case BuisnessType.Professionals:
                    skipRoleIds = new List<long> { 4L, 5L, 6L, 7L, 8L, 9L, 10L, 11L, 12L, 13L };
                    break;
            }

            using (AccountMasterContext context = new AccountMasterContext())
            {
                userInfo = context.Users
                    .Where(user =>!user.Roles.Any(role => skipRoleIds.Contains(role.RoleId)))
                    .ToList();
            }

            return userInfo;
        }

        public IList<Role> ListAllUserRole()
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Role> RoleInfo = Context.Roles.ToList<Role>();
                return RoleInfo;
            }
        }
        public IList<Role> ListAllUserRoleByUserId(long userId)
        {
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                IList<Role> RoleInfo = Context.Roles.Where(x => x.Users.Any(user => user.UserId == userId)).ToList<Role>();
                return RoleInfo;
            }
        }
        public IList<Role> ListAllUserRole(BuisnessType BuisnessType)
        {
            IList<Role> RoleInfo = null;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                if (BuisnessType == BuisnessType.Hospital)
                {
                    RoleInfo = Context.Roles.ToList<Role>();
                }
                if (BuisnessType == BuisnessType.Wholesale || BuisnessType == BuisnessType.Retail)
                {
                    RoleInfo = Context.Roles.Where(x => x.RoleId >= 8L && x.RoleId <= 10L || x.RoleId <= 5L).ToList<Role>();
                }
                if (BuisnessType == BuisnessType.Professionals)
                {
                    RoleInfo = Context.Roles.Where(x => x.RoleId >= 1L && x.RoleId < 4L).ToList<Role>();
                }
                if (BuisnessType == BuisnessType.Pharmacy)
                {
                    RoleInfo = Context.Roles.Where(x => x.RoleId >= 8L && x.RoleId <= 10L || x.RoleId <= 5L).ToList<Role>();
                }
                return RoleInfo;
            }
        }
        public Boolean AddRoleAccess(User User)
        {
            bool AddRoleAccess = false;
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                AddRoleAccess = true;
                User UserInfo = Context.Users.Include("Roles").Include("Accesses").FirstOrDefault(x => x.UserId == User.UserId);
                if (UserInfo.Roles.Count > 0)
                {
                    foreach (var Role in UserInfo.Roles.ToList())
                    {
                        var RoleInfo = Context.Roles.Find(Role.RoleId);
                        UserInfo.Roles.Remove(RoleInfo);
                        Context.SaveChanges();                    
                    }
                } 
                if (User.Roles.Count > 0)
                {
                    foreach (var Role in User.Roles)
                    {
                        var RoleInfo = Context.Roles.Find(Role.RoleId);
                        Context.Users.Include("Roles").FirstOrDefault(x => x.UserId == User.UserId).Roles.Add(RoleInfo);
                        Context.SaveChanges();
                    }
                }

                if(UserInfo.Accesses.Count>0)
                {
                    foreach (var Access in UserInfo.Accesses.ToList())
                    {
                        var AccessInfo = Context.Accesses.Find(Access.AccessId);
                        Context.Accesses.Remove(AccessInfo);
                        Context.SaveChanges();
                    }
                }
                if (User.Accesses.Count > 0)
                {
                    foreach (var Access in User.Accesses.ToList())
                    {                    
                        Context.Users.Include("Accesses").FirstOrDefault(x => x.UserId == User.UserId).Accesses.Add(Access);
                        Context.SaveChanges();
                    }
                }
                return AddRoleAccess;
            }
        }
        public List<Company> GetAccessibleCompaniesBySoftwareType(User User, SoftwareType sType)
        {
            CompanyManager CompanyManager = CompanyManager.Instance;
            List<Company> Companies = new List<Company>();
            if (User.IsSuperAdmin)
            {
                Companies = (List<Company>)CompanyManager.GetCompaniesBySoftwareType(sType);
            }
            else if (User.Accesses != null && User.Accesses.Count > 0)
            {
                List<long> Keys = new List<long>();
                foreach (Access access in User.Accesses)
                {
                    if (access.ResourceType.Equals(ResourceType.Company))
                    {
                        Keys.Add(access.ResourceId);
                    }
                    Companies = (List<Company>)CompanyManager.GetCompanies(Keys.ToArray<long>());
                }
            }
            return Companies;
        }

        public List<Company> GetAccessibleCompanies(User User)
        {
            CompanyManager CompanyManager =  CompanyManager.Instance;
            List<Company> Companies = new List<Company>();
            if(User.IsSuperAdmin)
            {
                Companies = (List<Company>) CompanyManager.GetCompanies();
            }
            else if (User.Accesses != null && User.Accesses.Count>0)
            {                
                List<long> Keys = new List<long>();
                foreach (Access access in User.Accesses)
                {
                    if (access.ResourceType.Equals(ResourceType.Company))
                    {
                        Keys.Add(access.ResourceId);
                    }                    
                    Companies = (List<Company>)CompanyManager.GetCompanies(Keys.ToArray<long>());
                }                
            }
            return Companies;
        }
        public List<Company> GetAccessibleParentCompanies(User User)
        {
            CompanyManager CompanyManager = CompanyManager.Instance;
            List<Company> Companies = new List<Company>();
            if (User.IsSuperAdmin)
            {
                Companies = (List<Company>)CompanyManager.GetParentCompanies();
            }
            else if (User.Accesses != null && User.Accesses.Count > 0)
            {
                List<long> Keys = new List<long>();
                foreach (Access access in User.Accesses)
                {
                    if (access.ResourceType.Equals(ResourceType.Company))
                    {
                        Keys.Add(access.ResourceId);
                    }
                    Companies = (List<Company>)CompanyManager.GetRootCompanies(Keys.ToArray<long>());
                }
            }
            return Companies;
        }

        public List<CostCenter> GetAccessibleCostCenter(User User,long CompanyId)
        {
            CostCenterManager CostCenterManager =  CostCenterManager.Instance;
            List<CostCenter> CostCenter = new List<CostCenter>();
            if (User.IsSuperAdmin)
            {
                CostCenter = (List<CostCenter>)CostCenterManager.ListCostCenterByCompanyId(CompanyId);
            }
            else if (User.Accesses != null && User.Accesses.Count > 0)
            {
                List<long> Keys = new List<long>();
                foreach (Access access in User.Accesses)
                {
                    if (access.ResourceType.Equals(ResourceType.CostCenter))
                    {
                        Keys.Add(access.ResourceId);
                    }
                    CostCenter = (List<CostCenter>)CostCenterManager.GetCostCenter(Keys.ToArray<long>(),CompanyId);
                }
            }
            return CostCenter;
        }

    }
}
