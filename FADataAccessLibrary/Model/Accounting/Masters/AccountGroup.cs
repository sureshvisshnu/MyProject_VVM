using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Accounting.Masters
{
    public class AccountGroup : AuditableEntity
    {
        [Key, Required]        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [Required, MaxLength(250)]
        public string Name { get; set; }
        public bool IsSubType { get; set; }
        public long? ParentAccountGroupId { get; set; }
        [ForeignKey("ParentAccountGroupId")]
        public virtual AccountGroup ParentAccountGroup { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public bool OpenBalance { get; set; }
        public bool TrackDepriciation { get; set; }
        public long? AccountClassificationId {get;set;}
        [ForeignKey("AccountClassificationId")]
        public AccountGroupClassification AccountGroupClassification { get; set; }
        public long? DebitMultiplier { get; set; }
        public long? CreditMultiplier { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public virtual ICollection<AccountGroupForHelp> AccountGroupsForHelp { get; set; }
    }

    public class AccountGroupClassification : AuditableEntity
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [Required, MaxLength(250)]
        public string Name { get; set; }
        public long DebitMultiplier { get; set; }
        public long CreditMultiplier { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AccountGroupForHelp
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public long AccountGroupForHelpId { get; set; }
        public string HelpGroupDescription { get; set; }
        public virtual ICollection<AccountGroup> AccountGroups { get; set; }
    }

}
