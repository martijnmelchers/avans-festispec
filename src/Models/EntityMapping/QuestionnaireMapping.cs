using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionnaireMapping : EntityTypeConfiguration<Questionnaire>
    {
        public QuestionnaireMapping()
        {
            HasRequired(qn => qn.PlannedInspection).WithRequiredDependent(pi => pi.Questionnaire);

            HasMany(qn => qn.Answers).WithRequired(a => a.Questionnaire);
            HasMany(qn => qn.Questions).WithRequired(q => q.Questionnaire);
        }
    }
}