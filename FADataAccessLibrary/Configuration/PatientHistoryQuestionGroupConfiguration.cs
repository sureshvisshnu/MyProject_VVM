using fa.model.Hms.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FADataAccessLibrary.Configuration
{
    public class PatientHistoryQuestionGroupConfiguration : IEntityTypeConfiguration<PatientHistoryQuestionGroup>
    {
        public void Configure(EntityTypeBuilder<PatientHistoryQuestionGroup> builder)
        {
            builder.HasData(
                new PatientHistoryQuestionGroup() { Id = 1, Name = "Medical History (Do you have now or before?)", Order = 1 },
                new PatientHistoryQuestionGroup() { Id = 2, Name = "System Review", Order = 2 },
                new PatientHistoryQuestionGroup() { Id = 3, Name = "General", Order = 1, ParentGroupId = 2 },
                new PatientHistoryQuestionGroup() { Id = 4, Name = "Muscle/joint/Bones", Order = 2, ParentGroupId = 2 },
                new PatientHistoryQuestionGroup() { Id = 5, Name = "Stomach and Intestines", Order = 3, ParentGroupId = 2 },
                new PatientHistoryQuestionGroup() { Id = 6, Name = "Ears", Order = 4, ParentGroupId = 2 },
                new PatientHistoryQuestionGroup() { Id = 7, Name = "Nervous System", Order = 5, ParentGroupId = 2 }
            );
        }
    }
}
