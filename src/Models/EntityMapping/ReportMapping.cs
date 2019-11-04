﻿using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class ReportMapping : EntityTypeConfiguration<Report>
    {
        public ReportMapping()
        {
            HasMany(r => r.ReportEntries).WithRequired(re => re.Report);
            HasRequired(r => r.Festival).WithMany(f => f.Reports);
        }
    }
}