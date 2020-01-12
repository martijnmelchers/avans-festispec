using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface IAddressService
    {
        Task<Address> SaveAddress(Address address);
        Task RemoveAddress(Address address);
    }
}