using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Model.Accounting.Masters
{
    public class TestModel
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        private string _displayAs;
        [MaxLength(100)]

        public string DisplayAs
        {
            get
            {
                if (string.IsNullOrEmpty(_displayAs))
                {
                    return Name;
                }
                else
                    return _displayAs;
            }

            set
            {
                if (!value.Equals(this.Name))
                {
                    _displayAs = value;
                }
                else
                {
                    _displayAs = this.Name;
                }
            }
        }
        [MaxLength(250)]
        public string Discription { get; set; }
    }
}
