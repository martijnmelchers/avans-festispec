using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class CustomerMapping : EntityTypeConfiguration<Customer>
    {
        public CustomerMapping()
        {
            Property(p => p.KvkNr).IsRequired();
            Property(p => p.CustomerName).IsRequired();

            HasMany(c => c.Festivals).WithRequired(f => f.Customer);
        }
    }
}