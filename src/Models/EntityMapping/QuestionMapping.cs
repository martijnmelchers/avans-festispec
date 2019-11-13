using Festispec.Models.Questions;
using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionMapping : EntityTypeConfiguration<Question>
    {
        public QuestionMapping()
        {
            Ignore(q => q.GraphType);

            Property(q => q.Contents).IsRequired().HasMaxLength(250);

            HasRequired(q => q.Category).WithMany(qc => qc.Questions);
            HasRequired(q => q.Questionnaire).WithMany(q => q.Questions);
        }
    }
}