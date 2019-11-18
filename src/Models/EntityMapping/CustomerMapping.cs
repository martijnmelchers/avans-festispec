using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class CustomerMapping : EntityTypeConfiguration<Customer>
    {
        public CustomerMapping()
        {
            Property(p => p.KvkNr).IsRequired();
            Property(p => p.CustomerName).IsRequired().HasMaxLength(20);

            HasRequired(c => c.Address).WithRequiredDependent();
            HasRequired(c => c.ContactDetails).WithRequiredDependent();

            HasMany(c => c.ContactPersons).WithRequired(l => l.Customer);
            HasMany(c => c.Festivals).WithRequired(f => f.Customer);
        }
    }
}