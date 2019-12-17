using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class ContactPersonMapping : EntityTypeConfiguration<ContactPerson>
    {
        public ContactPersonMapping()
        {
            Property(l => l.Role).IsRequired();

            HasRequired(l => l.Customer).WithMany(c => c.ContactPersons);

            HasMany(l => l.Notes).WithRequired(n => n.ContactPerson);
        }
    }
}
