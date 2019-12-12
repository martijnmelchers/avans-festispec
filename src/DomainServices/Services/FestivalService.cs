using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;

namespace Festispec.DomainServices.Services
{
    public class FestivalService : IFestivalService
    {
        private readonly FestispecContext _db;

        public FestivalService(FestispecContext db)
        {
            _db = db;
        }

        public async Task<Festival> CreateFestival(Festival festival)
        {
            if (!festival.Validate() || !festival.Address.Validate() || !festival.OpeningHours.Validate())
                throw new InvalidDataException();

            _db.Festivals.Add(festival);

            await _db.SaveChangesAsync();

            return festival;
        }

        public async Task<Festival> GetFestivalAsync(int festivalId)
        {
            var festival = await _db.Festivals
                .Include(f => f.Questionnaires)
                .Include(f => f.PlannedInspections)
                .FirstOrDefaultAsync(f => f.Id == festivalId);

            if (festival == null)
                throw new EntityNotFoundException();

            return festival;
        }

        public Festival GetFestival(int festivalId)
        {
            var festival = _db.Festivals
                .Include(f => f.Questionnaires)
                .Include(f => f.PlannedInspections)
                .FirstOrDefault(f => f.Id == festivalId);

            if (festival == null)
                throw new EntityNotFoundException();

            return festival;
        }

        public ICollection<Festival> GetFestivals()
        {
            return _db.Festivals.ToList();
        }

        public async Task UpdateFestival(Festival festival)
        {
            if (!festival.Validate() || !festival.Address.Validate() || !festival.OpeningHours.Validate())
                throw new InvalidDataException();

            await _db.SaveChangesAsync();
        }

        public async Task RemoveFestival(int festivalId)
        {
            var festival = await GetFestivalAsync(festivalId);

            if (festival.Questionnaires.Count > 0)
                throw new FestivalHasQuestionnairesException();
            
            _db.Festivals.Remove(festival);

            await _db.SaveChangesAsync();
        }
    }
}
