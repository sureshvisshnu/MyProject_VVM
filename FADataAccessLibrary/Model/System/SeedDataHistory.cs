using System;
using System.ComponentModel.DataAnnotations;


namespace fa.model.System
{
    public class SeedDataHistory 
    {
        [Key]
        public string Key { get; set; }
        public DateTime DateExecuted { get; set; }
    }

}
