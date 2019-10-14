using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class PlannedEventMapping : EntityTypeConfiguration<PlannedEvent>
    {
        public PlannedEventMapping()
        {
            Property(pe => pe.StartTime).IsRequired();
            Property(pe => pe.EndTime).IsRequired();
            Property(pe => pe.EventTitle).IsRequired().HasMaxLength(45);

            HasRequired(pe => pe.Employee).WithMany(e => e.PlannedEvents);
        }
    }
}