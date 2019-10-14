using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AnswerMapping : EntityTypeConfiguration<Answer>
    {
        public AnswerMapping()
        {
            Property(a => a.Contents).IsRequired().HasMaxLength(250);

            HasRequired(a => a.Inspector).WithMany();
            HasRequired(a => a.Question).WithRequiredPrincipal();
            HasRequired(a => a.Questionnaire).WithMany(q => q.Answers);

            HasMany(a => a.Attachments).WithRequired(a => a.Answer);
        }
    }
}