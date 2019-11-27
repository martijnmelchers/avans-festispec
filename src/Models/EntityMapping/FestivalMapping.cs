using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class FestivalMapping : EntityTypeConfiguration<Festival>
    {
        public FestivalMapping()
        {
            Property(f => f.FestivalName).IsRequired();
            Property(f => f.Description).IsRequired();

            HasOptional(f => f.Report).WithRequired(r => r.Festival);
            HasRequired(f => f.Customer).WithMany(c => c.Festivals);

            HasMany(f => f.PlannedInspections).WithRequired(pi => pi.Festival);
            HasMany(f => f.Questionnaires).WithRequired(q => q.Festival);
        }
    }
}