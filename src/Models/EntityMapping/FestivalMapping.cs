﻿using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class FestivalMapping : EntityTypeConfiguration<Festival>
    {
        public FestivalMapping()
        {
            Property(f => f.FestivalName).IsRequired().HasMaxLength(45);
            Property(f => f.Description).IsRequired().HasMaxLength(250);

            HasRequired(f => f.Address).WithRequiredPrincipal();
            HasOptional(f => f.Report).WithRequired(r => r.Festival);
            HasRequired(f => f.Customer).WithMany(c => c.Festivals);
            HasRequired(f => f.OpeningHours).WithRequiredPrincipal(oh => oh.Festival);

            HasMany(f => f.Questionnaires).WithRequired(q => q.Festival);
        }
    }
}