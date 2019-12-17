using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    public class FullNameMapping : ComplexTypeConfiguration<FullName>
    {
        public FullNameMapping()
        {
            Property(e => e.First).IsRequired();
            Property(e => e.Middle).IsOptional();
            Property(e => e.Last).IsRequired();
        }
    }
}
