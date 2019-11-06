using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class PlannedInspectionMapping : EntityTypeConfiguration<PlannedInspection>
    {
        public PlannedInspectionMapping()
        {
            Property(pi => pi.StartTime).IsRequired();
            Property(pi => pi.EndTime).IsRequired();
            Property(pi => pi.CancellationReason).HasMaxLength(250);

            HasRequired(pi => pi.Questionnaire).WithRequiredPrincipal(q => q.PlannedInspection);
            HasRequired(pi => pi.Festival).WithMany(f => f.PlannedInspections);
            HasRequired(pi => pi.Inspector).WithMany(i => i.PlannedInspections);
        }
    }
}