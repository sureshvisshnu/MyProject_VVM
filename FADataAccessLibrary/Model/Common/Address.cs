using fa.api.Accounting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Common
{
    public class Address : AuditableEntity
    {
        [Key]
        public long AddressId { get; set; }
        [MaxLength(50)]
        public String AddressLine1 { get; set; }
        [MaxLength(50)]
        public String AddressLine2 { get; set; }
        [MaxLength(35)]
        public String CityOrTown { get; set; }
        [MaxLength(35)]
        public String District { get; set; }
      
        [MaxLength(6)]
        public String PinCode { set; get; }
        [MaxLength(35)]
        public String StateName { get; set; }
        public long? StatesId { get; set; }
        [ForeignKey("StatesId")]
        public virtual State State { get; set; }
        [NotMapped]
        public string FullAddress
        {         
            get
            {
                string Output = string.Empty;
                Output += (String.IsNullOrEmpty(AddressLine1) ? "" : AddressLine1);
                Output += (String.IsNullOrEmpty(AddressLine2) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", \n") + AddressLine2);
                Output += (String.IsNullOrEmpty(CityOrTown) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", \n") + CityOrTown);
                Output += (String.IsNullOrEmpty(District) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", \n") + District);
                Output += (StatesId == null ? (String.IsNullOrEmpty(StateName) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", \n") + StateName) : (String.IsNullOrEmpty(Output) ? "" : ", \n") + StateManager.Instance.GetStateById((long)StatesId).Name);
                Output += (String.IsNullOrEmpty(PinCode) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", \n") + PinCode);
                return Output;
            }
        }

        [NotMapped]
        public string FullAddressInSingleLine 
        {
            get
            {
                string Output = string.Empty;
                Output += (String.IsNullOrEmpty(AddressLine1) ? "" : AddressLine1);
                Output += (String.IsNullOrEmpty(AddressLine2) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", ") + AddressLine2);
                Output += (String.IsNullOrEmpty(CityOrTown) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", ") + CityOrTown);
                Output += (String.IsNullOrEmpty(District) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", ") + District);
                Output +=  (StatesId==null ? "" : (String.IsNullOrEmpty(Output) ? "" : ", ") + StateManager.Instance.GetStateById((long)StatesId).Name);
                Output +=(String.IsNullOrEmpty(PinCode) ? "" : (String.IsNullOrEmpty(Output) ? "" : ", ") + PinCode);
                return Output;
            }
        }

    }

}
