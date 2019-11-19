﻿using Festispec.Models.Answers;
using System.Data.Entity.ModelConfiguration;

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

            HasMany(a => a.Attachments).WithRequired(a => a.Answer);
        }
    }
}