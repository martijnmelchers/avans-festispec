using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class FestivalMapping : EntityTypeConfiguration<Festival>
    {
        public FestivalMapping()
        {
            Property(f => f.FestivalName).IsRequired().HasMaxLength(45);
            Property(f => f.Description).IsRequired().HasMaxLength(250);

            HasRequired(f => f.Address).WithRequiredPrincipal();
            HasRequired(f => f.Customer).WithMany(c => c.Festivals);

            HasMany(f => f.OpeningHours).WithRequired(oh => oh.Festival);
            HasMany(f => f.PlannedInspections).WithRequired(pi => pi.Festival);
            HasMany(f => f.Reports).WithRequired(r => r.Festival);
        }
    }
}