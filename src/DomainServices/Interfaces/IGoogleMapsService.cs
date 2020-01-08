using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;
using Festispec.Models.Google;

namespace Festispec.DomainServices.Interfaces
{
    public interface IGoogleMapsService : ISyncable
    {
        Task<List<Prediction>> GetSuggestions(string input);
        Task<Address> GetAddress(string placeId);
        Task<double> CalculateDistance(Address origin, Address destination);
    }
}