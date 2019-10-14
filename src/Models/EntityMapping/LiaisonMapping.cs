using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.Models.EntityMapping
{
    class LiaisonMapping : EntityTypeConfiguration<Liaison>
    {
        public LiaisonMapping()
        {
            Property(l => l.LiaisonName).IsRequired().HasMaxLength(45);
            Property(l => l.Role).IsRequired().HasMaxLength(20);

            HasRequired(l => l.ContactDetails).WithRequiredPrincipal();
            HasRequired(l => l.Customer).WithMany(c => c.Liaisons);

            HasMany(l => l.Notes).WithRequired(n => n.Liaison);
        }
    }
}
