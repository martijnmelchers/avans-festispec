using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class FestivalService : IFestivalService
    {
        private readonly IAddressService _addressService;
        private readonly FestispecContext _db;
        private readonly ISyncService<Festival> _syncService;

        public FestivalService(FestispecContext db, ISyncService<Festival> syncService, IAddressService addressService)
        {
            _db = db;
            _syncService = syncService;
            _addressService = addressService;
        }

        public async Task<Festival> CreateFestival(Festival festival, int customerId)
        {
            festival.Customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            
            if (!festival.Validate() || !festival.OpeningHours.Validate())
                throw new InvalidDataException();

            festival.Address = await _addressService.SaveAddress(festival.Address);

            _db.Festivals.Add(festival);

            await _db.SaveChangesAsync();

            return festival;
        }

        public async Task<Festival> GetFestivalAsync(int festivalId)
        {
            Festival festival = await _db.Festivals
                .Include(f => f.Questionnaires)
                .Include(f => f.Questionnaires.Select(q => q.Questions.Select(qe => qe.Answers)))
                .Include(f => f.PlannedInspections)
                .Include(f => f.Address)
                .FirstOrDefaultAsync(f => f.Id == festivalId);

            if (festival == null)
                throw new EntityNotFoundException();

            return festival;
        }

        public Festival GetFestival(int festivalId)
        {
            Festival festival = _db.Festivals
                .Include(f => f.Questionnaires)
                .Include(f => f.PlannedInspections)
                .Include(f => f.Address)
                .FirstOrDefault(f => f.Id == festivalId);

            if (festival == null)
                throw new EntityNotFoundException();

            return festival;
        }

        public ICollection<Festival> GetFestivals()
        {
            return _db.Festivals.Include(f => f.Address).ToList();
        }

        public async Task UpdateFestival(Festival festival)
        {
            if (!festival.Validate() || !festival.OpeningHours.Validate())
                throw new InvalidDataException();

            festival.Address = await _addressService.SaveAddress(festival.Address);

            await _db.SaveChangesAsync();
        }

        public async Task RemoveFestival(int festivalId)
        {
            Festival festival = await GetFestivalAsync(festivalId);

            if (festival.Questionnaires.Count > 0)
                throw new FestivalHasQuestionnairesException();

            await _addressService.RemoveAddress(festival.Address);
            _db.Festivals.Remove(festival);

            await _db.SaveChangesAsync();
        }

        public void Sync()
        {
            FestispecContext db = _syncService.GetSyncContext();
            
            List<Festival> festivals = db.Festivals
                .Include(f => f.Address)
                .Include(f => f.Questionnaires)
                .Include(f => f.Questionnaires.Select(q => q.Questions))
                .Include(f => f.PlannedInspections).ToList();

            _syncService.Flush();
            _syncService.AddEntities(festivals);
            _syncService.SaveChanges();
        }
    }
}