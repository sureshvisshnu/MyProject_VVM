using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class PatientHistoryQuestionConfiguration : IEntityTypeConfiguration<PatientHistoryQuestion>
    {
        public void Configure(EntityTypeBuilder<PatientHistoryQuestion> builder)
        {
            builder.Ignore(phq => phq.PosibleValues);
            builder.HasData(
                new PatientHistoryQuestion() { 
                    Id = 1, Name = "Diabetes", Order = 1, GroupId = 1, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 2, Name = "High Blood Sugar", Order = 2, GroupId = 1, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 3, Name = "High Cholesterol", Order = 3, GroupId = 1, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 4, Name = "Hypothyrodisim", Order = 4, GroupId = 1, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 5, Name = "Goiter", Order = 5, GroupId = 1, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 6, Name = "Cancer", Order = 6, GroupId = 1, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" }, 
                    AdditionalNotes = true, AdditionalNotesCaption = "Type?" 
                },
                new PatientHistoryQuestion() { 
                    Id = 7, Name = "Other Medical Condition", Order = 7, GroupId = 1, 
                    ValueType = PatientHistoryQuestionType.TEXT 
                },
                new PatientHistoryQuestion() { 
                    Id = 8, Name = "Recent weight gain", Order = 1, GroupId = 3, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" }, 
                    AdditionalNotes = true, AdditionalNotesCaption = "How much?" 
                },
                new PatientHistoryQuestion() { 
                    Id = 9, Name = "Recent weight loss", Order = 2, GroupId = 3, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" }, 
                    AdditionalNotes = true, AdditionalNotesCaption = "How much?" 
                },
                new PatientHistoryQuestion() { 
                    Id = 10, Name = "Fatique", Order = 3, GroupId = 3, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 11, Name = "Weaknes", Order = 4, GroupId = 3, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 12, Name = "Fever", Order = 5, GroupId = 3, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                },
                new PatientHistoryQuestion() { 
                    Id = 13, Name = "Night Sweads", Order = 6, GroupId = 3, 
                    ValueType = PatientHistoryQuestionType.YESNO, PosibleValues = new string[] { "Yes", "No" } 
                }
            );
        }
    }
}
