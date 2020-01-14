using System.Data.Entity;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;

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
            if (!address.Validate())
                throw new InvalidAddressException();

            Address existing = await _db.Addresses.FirstOrDefaultAsync(a => a.Latitude == address.Latitude && a.Longitude == address.Longitude);

            if (existing != null)
                return existing;

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync();

            return address;
        }

        public async Task RemoveAddress(Address address)
        {
            var existing = 0;
            existing += await _db.Festivals.Include(f => f.Address).CountAsync(a => a.Address.Id == address.Id && a.Address.Latitude == address.Latitude && a.Address.Longitude == address.Longitude);
            existing += await _db.Employees.Include(e => e.Address).CountAsync(e => e.Address.Id == address.Id && e.Address.Latitude == address.Latitude && e.Address.Longitude == address.Longitude);
            existing += await _db.Customers.Include(c => c.Address).CountAsync(c => c.Address.Id == address.Id && c.Address.Latitude == address.Latitude && c.Address.Longitude == address.Longitude);

            if (existing == 0)
                _db.Addresses.Remove(address);

            await _db.SaveChangesAsync();
        }
    }
}
