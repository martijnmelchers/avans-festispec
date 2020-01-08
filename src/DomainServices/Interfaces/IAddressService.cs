using Festispec.Models;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IAddressService
    {
        Task<Address> SaveAddress(Address address);
        Task RemoveAddress(Address address);
    }
}
