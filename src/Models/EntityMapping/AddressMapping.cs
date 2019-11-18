using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AddressMapping : EntityTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            // HouseNumber has explicitly not been required.
            // Festivals don't always have a specific house number.

            Property(a => a.ZipCode).IsRequired().HasMaxLength(10);
            Property(a => a.StreetName).IsRequired().HasMaxLength(50);
            Property(a => a.City).IsRequired().HasMaxLength(50);
            Property(a => a.Country).IsRequired().HasMaxLength(75);

            Property(a => a.Suffix).IsOptional().HasMaxLength(10);
            Property(a => a.HouseNumber).IsOptional();

            HasOptional(a => a.Customer).WithRequired(c => c.Address);
            HasOptional(a => a.Employee).WithRequired(e => e.Address);
            HasOptional(a => a.Festival).WithRequired(f => f.Address);
        }
    }
}