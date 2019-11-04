using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class ReportEntryMapping : EntityTypeConfiguration<ReportEntry>
    {
        public ReportEntryMapping()
        {
            Property(re => re.Order).IsRequired();

            HasRequired(re => re.Question);
            HasRequired(re => re.Report).WithMany(r => r.ReportEntries);
        }
    }
}