using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AddressMapping : ComplexTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            Property(a => a.ZipCode).IsRequired();
            Property(a => a.StreetName).IsRequired();
            Property(a => a.City).IsRequired();
            Property(a => a.Country).IsRequired();

            Property(a => a.Suffix).IsOptional();

            // HouseNumber has explicitly not been required.
            // Festivals don't always have a specific house number.
            Property(a => a.HouseNumber).IsOptional();
        }
    }
}