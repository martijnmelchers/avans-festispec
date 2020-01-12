using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;

namespace Festispec.DomainServices.Services.Offline
{
    [ExcludeFromCodeCoverage]
    public class OfflineAddressService : IAddressService
    {
        public Task<Address> SaveAddress(Address address)
        {
            throw new System.InvalidOperationException();
        }

        public Task RemoveAddress(Address address)
        {
            throw new System.InvalidOperationException();
        }
    }
}