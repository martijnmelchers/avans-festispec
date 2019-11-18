using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.Models.EntityMapping
{
    class ContactDetailsMapping : EntityTypeConfiguration<ContactDetails>
    {
        public ContactDetailsMapping()
        {
            Property(cd => cd.PhoneNumber).HasMaxLength(50);
            Property(cd => cd.EmailAddress).HasMaxLength(50);

            HasOptional(cd => cd.Customer).WithRequired(c => c.ContactDetails);
            HasOptional(cd => cd.ContactPerson).WithRequired(cp => cp.ContactDetails);
            HasOptional(cd => cd.Employee).WithRequired(e => e.ContactDetails);
        }
    }
}
