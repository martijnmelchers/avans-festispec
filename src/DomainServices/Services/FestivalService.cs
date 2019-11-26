using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class FestivalService : IFestivalService
    {
        private readonly FestispecContext _db;

        public FestivalService(FestispecContext db)
        {
            _db = db;
        }

        public async Task<Festival> CreateFestival(string festivalName, string description, Customer customer)
        {
            var festival = new Festival(festivalName, description, customer);

            if (!festival.Validate())
                throw new InvalidDataException();

            _db.Festivals.Add(festival);

            await _db.SaveChangesAsync();

            return festival;
        }

        public Festival GetFestival(int festivalId)
        {
            var festival = _db.Festivals.FirstOrDefault(f => f.Id == festivalId);

            if (festival == null)
                throw new EntityNotFoundException();

            return festival;
        }

        public async Task RemoveQuestionnaire(int festivalId)
        {
            var festival = GetFestival(festivalId);

            if (festival.Questionnaires.Count > 0)
                throw new FestivalHasQuestionnairesException();

            _db.Festivals.Remove(festival);

            await _db.SaveChangesAsync();
        }
    }
}
