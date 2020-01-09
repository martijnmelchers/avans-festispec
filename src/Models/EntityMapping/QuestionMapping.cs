using System.Data.Entity.ModelConfiguration;
using Festispec.Models.Questions;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionMapping : EntityTypeConfiguration<Question>
    {
        public QuestionMapping()
        {
            Ignore(q => q.GraphType);

            Property(q => q.Contents).IsRequired();

            HasRequired(q => q.Questionnaire).WithMany(q => q.Questions);

            HasMany(q => q.Answers).WithRequired(a => a.Question);
        }
    }
}