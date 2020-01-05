using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;

namespace Festispec.DomainServices.Services
{
    public class OfflineFestivalService : IFestivalService
    {
        private readonly SyncService<Festival> _syncService;

        public OfflineFestivalService(SyncService<Festival> syncService)
        {
            _syncService = syncService;
        }
        
        public Task<Festival> CreateFestival(Festival festival)
        {
            throw new System.InvalidOperationException();
        }

        public async Task<Festival> GetFestivalAsync(int festivalId)
        {
            return _syncService.GetEntity(festivalId);
        }

        public Festival GetFestival(int festivalId)
        {
            return _syncService.GetEntity(festivalId);
        }

        public ICollection<Festival> GetFestivals()
        {
            return _syncService.GetAll().ToList();
        }

        public Task UpdateFestival(Festival festival)
        {
            throw new System.InvalidOperationException();
        }

        public Task RemoveFestival(int festivalId)
        {
            throw new System.InvalidOperationException();
        }
        
        public void Sync()
        {
        }
    }
}