using fa.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.model.Common
{
    public class AdditionalDetail: AuditableEntityForCostCenter
    {
        [Key]
        public long Id { get; set; }
        public AdditionalDetailSourceType SourceType { get; set; }
        public string SourceId { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Detail);
        }

    }
    public enum AdditionalDetailSourceType
    {       
        Sales = 10,     
    }
}
