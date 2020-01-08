using Festispec.Models;
using Festispec.Models.Google;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IGoogleMapsService : ISyncable
    {
        Task<List<Prediction>> GetSuggestions(string input);
        Task<Address> GetAddress(string placeId);
        Task<double> CalculateDistance(Address origin, Address destination);
    }
}
