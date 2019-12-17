using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class OpeningHoursMapping : ComplexTypeConfiguration<OpeningHours>
    {
        public OpeningHoursMapping()
        {
            Property(oh => oh.StartTime).IsRequired();
            Property(oh => oh.EndTime).IsRequired();
        }
    }
}