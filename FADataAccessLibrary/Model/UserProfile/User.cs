using fa.model.Common;
using fa.model.Accounting.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.api.Accounting;
using Microsoft.EntityFrameworkCore;

namespace fa.model.UserProfile
{
    public class User: AuditableEntity
    {
        [Key]
        public long UserId { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string Login { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public Boolean IsResetPassword { get; set; }
        public Boolean IsLocked { get; set; }
        public Boolean IsSuperAdmin { get; set; }
        public long? AddressId { get; set; }
        public virtual Address Address{ get; set; }
        public long? ContactInfoId { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
        public long? TaxInfoId { get; set; }
        public virtual TaxInfo TaxInfo { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Access> Accesses { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName,LastName);
        }

        public List<SystemFunction> GetSystemFunctions()
        {
            List<SystemFunction> Functions = new List<SystemFunction>();
            foreach (var role in Roles)
            {
                Functions.AddRange(role.SystemFunctions);
            }
            return Functions;
        }
        [NotMapped]
        public string Name
        {
            get 
            {
                if (EmployeeId != null)
                {
                    return EmployeeManager.Instance.GetEmployeeInfoById((long)EmployeeId).Name;
                }
                else
                {
                    return string.Format("{0} {1}", FirstName, LastName);
                }
            }
        }
        public long? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public fa.model.Employee.Employee Employee { get; set; }


        
    }

    public class Role: AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<SystemFunction> SystemFunctions { get; set; }       
    }

    public class SystemFunction: AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long FunctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MenuViewId { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
    //[Index(nameof(RoleId), nameof(FunctionId), Name = "IDX_Role")]
    //public class RoleSystemFunction
    //{
    //    [Key]
    //    [Required]
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public long FunctionId { get; set; }
    //    [ForeignKey("FunctionId")]
    //    public SystemFunction SystemFunction { get; set; }
    //    [Key]
    //    [Required]
    //    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public long RoleId { get; set; }
    //    [ForeignKey("RoleId")]
    //    public Role Role { get; set; }
    //}
}
