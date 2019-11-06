using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.Models.EntityMapping
{
    class CertificateMapping : EntityTypeConfiguration<Certificate>
    {
        public CertificateMapping()
        {
            Property(c => c.CertificateTitle).IsRequired().HasMaxLength(45);
            Property(c => c.CertificationDate).IsRequired();
            Property(c => c.ExpirationDate).IsRequired();

            HasRequired(c => c.Employee).WithMany(e => e.Certificates);
        }
    }
}
