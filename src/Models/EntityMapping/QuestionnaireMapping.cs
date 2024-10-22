﻿using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionnaireMapping : EntityTypeConfiguration<Questionnaire>
    {
        public QuestionnaireMapping()
        {
            Property(qn => qn.IsComplete).IsOptional();

            Property(qn => qn.Name).IsRequired();
            HasMany(qn => qn.PlannedInspections).WithRequired(pi => pi.Questionnaire);
            HasRequired(qn => qn.Festival).WithMany(f => f.Questionnaires);

            HasMany(qn => qn.Questions).WithRequired(q => q.Questionnaire);
        }
    }
}