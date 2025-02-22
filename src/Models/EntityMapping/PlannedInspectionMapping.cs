﻿using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class PlannedInspectionMapping : EntityTypeConfiguration<PlannedInspection>
    {
        public PlannedInspectionMapping()
        {
            Property(pi => pi.CancellationReason).IsOptional();
            Property(pi => pi.IsCancelled).IsOptional();
            Property(pi => pi.WorkedHoursAccepted).IsOptional();
            Property(pi => pi.WorkedHours).IsOptional();

            HasRequired(pi => pi.Questionnaire).WithMany(q => q.PlannedInspections);
            HasRequired(pi => pi.Festival).WithMany(f => f.PlannedInspections).WillCascadeOnDelete(false);

            HasMany(pi => pi.Answers).WithRequired(a => a.PlannedInspection);
        }
    }
}