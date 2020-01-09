using System.Data.Entity.ModelConfiguration;
using Festispec.Models.Reports;

namespace Festispec.Models.EntityMapping
{
    internal class ReportEntryMapping : EntityTypeConfiguration<ReportEntry>
    {
        public ReportEntryMapping()
        {
            Property(re => re.Order).IsRequired();

            HasRequired(re => re.Question).WithRequiredDependent();
            HasRequired(re => re.Report).WithMany(r => r.ReportEntries);
        }
    }
}