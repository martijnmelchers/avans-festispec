using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

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
            if (!festival.Validate())
                throw new InvalidDataException();

            _db.Festivals.Add(festival);

            await _db.SaveChangesAsync();

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

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFestival(int festivalId)
        {
            var festival = GetFestival(festivalId);

            if (festival.Questionnaires.Count > 0)
                throw new FestivalHasQuestionnairesException();
            
            _db.Festivals.Remove(festival);

            await _db.SaveChangesAsync();
        }
    }
}
