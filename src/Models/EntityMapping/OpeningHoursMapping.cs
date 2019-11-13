using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class OpeningHoursMapping : EntityTypeConfiguration<OpeningHours>
    {
        public OpeningHoursMapping()
        {
            Property(oh => oh.StartTime).IsRequired();
            Property(oh => oh.EndTime).IsRequired();

            HasRequired(oh => oh.Festival).WithRequiredDependent(f => f.OpeningHours);
        }
    }
}