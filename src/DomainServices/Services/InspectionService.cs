using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    class InspectionService : IInspectionService
    {

        private readonly FestispecContext _db;

        public InspectionService(FestispecContext db)
        {
            _db = db;
        }

        public Task<PlannedInspection> CreatePlannedInspection(Festival festival)
        {
            throw new NotImplementedException();
        }


        public PlannedInspection GetPlannedInspection(int plannedInspectionId)
        {
            var plannedInspection = _db.PlannedInspections.FirstOrDefault(e => e.Id == plannedInspectionId);
            
            if (plannedInspection == null)
                throw new EntityNotFoundException();
            
            return plannedInspection;

        }

        public async Task RemoveInspection(int PlannedInspectionId)
        {
            var plannedInspection = GetPlannedInspection(PlannedInspectionId);

            //Check if submitted answers by employee
            if (plannedInspection.Answers.Count > 0)
                throw new QuestionHasAnswersException();

                _db.PlannedInspections.Remove(plannedInspection);

            await _db.SaveChangesAsync();
        }

    }
}
