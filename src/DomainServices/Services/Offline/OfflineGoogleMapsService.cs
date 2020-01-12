using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;

namespace Festispec.DomainServices.Services.Offline
{
    public class OfflineGoogleMapsService : IGoogleMapsService
    {
        private readonly ISyncService<DistanceResult> _syncService;

        public OfflineGoogleMapsService(ISyncService<DistanceResult> syncService)
        {
            _syncService = syncService;
        }
        
        public async Task<List<Prediction>> GetSuggestions(string input)
        {
            return await Task.FromResult(new List<Prediction>());
        }

        public Task<Address> GetAddress(string placeId)
        {
            throw new GoogleMapsApiException();
        }

        public async Task<double> CalculateDistance(Address origin, Address destination)
        {
            DistanceResult existing = (await _syncService.GetAllAsync()).FirstOrDefault(x => x.Origin.Id == origin.Id && x.Destination.Id == destination.Id);

            if (existing == null)
                throw new GoogleMapsApiException();
            
            return existing.Distance;
        }

        public void Sync()
        {
        }
    }
}