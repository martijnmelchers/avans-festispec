using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class AddressService : IAddressService
    {
        private readonly FestispecContext _db;

        public AddressService(FestispecContext db)
        {
            _db = db;
        }


        public async Task<Address> SaveAddress(Address address)
        {
            var existing = await _db.Addresses.FirstOrDefaultAsync(a => a.Latitude == address.Latitude && a.Longitude == a.Longitude);

            if (existing != null)
                return existing;

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync();

            return address;
        }

        // TODO: Add distance calculation :)
    }
}
