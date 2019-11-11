using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class PlannedInspectionMapping : EntityTypeConfiguration<PlannedInspection>
    {
        public PlannedInspectionMapping()
        {
            Property(pi => pi.CancellationReason).HasMaxLength(250);

            HasRequired(pi => pi.Questionnaire).WithRequiredPrincipal(q => q.PlannedInspection);
            HasRequired(pi => pi.Festival).WithMany(f => f.PlannedInspections);
        }
    }
}