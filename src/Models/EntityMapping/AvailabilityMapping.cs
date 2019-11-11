using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AvailabilityMapping : EntityTypeConfiguration<Availability>
    {
        public AvailabilityMapping()
        {
            Property(a => a.IsAvailable).IsRequired();
            Property(a => a.Reason).HasMaxLength(250);
        }
    }
}