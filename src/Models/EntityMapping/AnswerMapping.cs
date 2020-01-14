using System.Data.Entity.ModelConfiguration;
using Festispec.Models.Answers;

namespace Festispec.Models.EntityMapping
{
    internal class AnswerMapping : EntityTypeConfiguration<Answer>
    {
        public AnswerMapping()
        {
            HasRequired(a => a.Question)
                .WithMany(q => q.Answers)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.PlannedInspection)
                .WithMany(pi => pi.Answers)
                .WillCascadeOnDelete(false); 
        }
    }
}