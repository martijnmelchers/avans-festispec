using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.Models.EntityMapping
{
    class ContactPersonMapping : EntityTypeConfiguration<ContactPerson>
    {
        public ContactPersonMapping()
        {
            Property(l => l.Role).IsRequired().HasMaxLength(20);

            HasRequired(l => l.ContactDetails).WithOptional(cd => cd.ContactPerson);
            HasRequired(l => l.Name).WithOptional(fn => fn.ContactPerson);
            HasRequired(l => l.Customer).WithMany(c => c.ContactPersons);

            HasMany(l => l.Notes).WithRequired(n => n.ContactPerson);
        }
    }
}
