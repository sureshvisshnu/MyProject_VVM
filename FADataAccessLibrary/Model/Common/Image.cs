using System;
using System.ComponentModel.DataAnnotations;


namespace fa.model.Common
{
    public class CustomImage : AuditableEntity
    {
        [Key]
        public long ImageId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String FileLocation { get; set; }
        public Byte[] image;
    }
}
