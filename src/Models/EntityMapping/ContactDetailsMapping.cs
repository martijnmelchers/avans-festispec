using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.Models.EntityMapping
{
    class ContactDetailsMapping : ComplexTypeConfiguration<ContactDetails>
    {
        public ContactDetailsMapping()
        {
            Property(cd => cd.PhoneNumber).HasMaxLength(50);
            Property(cd => cd.EmailAddress).HasMaxLength(50);
        }
    }
}
