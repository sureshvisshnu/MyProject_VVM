using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.UserProfile
{
    public enum ResourceType
    {
        Company=1,
        CostCenter=2
    }

    public class Access : AuditableEntity
    {
        [Key]
        public long AccessId { get; set; }
        public ResourceType ResourceType { get; set; }
        public long ResourceId { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User {get;set;}
    }
}
