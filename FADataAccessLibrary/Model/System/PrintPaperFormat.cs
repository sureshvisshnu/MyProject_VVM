using System.ComponentModel.DataAnnotations;

namespace fa.model.System
{
    public class PrintPaperFormat
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
