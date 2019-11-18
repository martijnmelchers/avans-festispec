using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    public class FullNameMapping : EntityTypeConfiguration<FullName>
    {
        public FullNameMapping()
        {
            Property(e => e.First).IsRequired().HasMaxLength(40);
            Property(e => e.Middle).IsOptional().HasMaxLength(40);
            Property(e => e.Last).IsRequired().HasMaxLength(40);

            HasOptional(fn => fn.ContactPerson).WithRequired(cp => cp.Name);
            HasOptional(fn => fn.Employee).WithRequired(e => e.Name);
        }
    }
}
