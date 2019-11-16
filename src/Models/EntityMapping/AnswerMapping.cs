using Festispec.Models.Answers;
using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AnswerMapping : EntityTypeConfiguration<Answer>
    {
        public AnswerMapping()
        {
            HasRequired(a => a.Inspector).WithMany();
            HasRequired(a => a.Question).WithRequiredPrincipal();
            HasRequired(a => a.Questionnaire).WithMany(q => q.Answers);
            HasRequired(a => a.PlannedInspection).WithRequiredPrincipal();

            HasMany(a => a.Attachments).WithRequired(a => a.Answer);
        }
    }
}