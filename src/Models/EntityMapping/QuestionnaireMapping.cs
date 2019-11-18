using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionnaireMapping : EntityTypeConfiguration<Questionnaire>
    {
        public QuestionnaireMapping()
        {
            Property(qn => qn.IsComplete).IsOptional();

            HasRequired(qn => qn.PlannedInspection).WithRequiredDependent(pi => pi.Questionnaire);
            HasRequired(qn => qn.Festival).WithMany(f => f.Questionnaires);

            HasMany(qn => qn.Answers).WithRequired(a => a.Questionnaire);
            HasMany(qn => qn.Questions).WithRequired(q => q.Questionnaire);
        }
    }
}