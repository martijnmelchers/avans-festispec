using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class CertificateMapping : EntityTypeConfiguration<Certificate>
    {
        public CertificateMapping()
        {
            Property(c => c.CertificateTitle).IsRequired();
            Property(c => c.CertificationDate).IsRequired();
            Property(c => c.ExpirationDate).IsRequired();

            HasRequired(c => c.Employee).WithMany(e => e.Certificates);
        }
    }
}
