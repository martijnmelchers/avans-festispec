using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class DistanceResultMapping : EntityTypeConfiguration<DistanceResult>
    {
        public DistanceResultMapping()
        {
            HasRequired(a => a.Origin).WithMany().WillCascadeOnDelete(false);
            HasRequired(a => a.Destination).WithMany().WillCascadeOnDelete(false);

            Property(a => a.Distance).IsRequired();
        }
    }
}