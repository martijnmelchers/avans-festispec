using System.Data.Entity.ModelConfiguration;
using Festispec.Models.Reports;

namespace Festispec.Models.EntityMapping
{
    internal class ReportMapping : EntityTypeConfiguration<Report>
    {
        public ReportMapping()
        {
            HasMany(r => r.ReportEntries).WithRequired(re => re.Report);
            HasRequired(r => r.Festival).WithOptional(f => f.Report);
        }
    }
}