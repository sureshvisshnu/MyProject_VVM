using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fa.model.Hms.Master
{
    public class PatientHistoryQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [Required][MaxLength(30)]
        public string Name { set; get; }
        [Required]
        public long? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual PatientHistoryQuestionGroup Group {get;set;}
        [Required]
        public PatientHistoryQuestionType ValueType { get; set; }
        public string[] PosibleValues { get; set; }
        public bool AdditionalNotes { get; set; }
        public String AdditionalNotesCaption { get; set; }
        public int Order { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public enum PatientHistoryQuestionType
    {
        DATE,YEAR, MONTHYEAR, TEXT, YESNO, SELECTONE , MULTISELECT 
    }

    public class PatientHistoryQuestionGroup 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { set; get; }
        public int Order { get; set; }
        public long? ParentGroupId { get; set; }
        [ForeignKey("ParentGroupId")]
        public virtual PatientHistoryQuestionGroup ParentGroup { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
