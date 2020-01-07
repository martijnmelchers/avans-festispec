using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class InspectionService : IInspectionService
    {

        private readonly FestispecContext _db;

        public InspectionService(FestispecContext db)
        {
            _db = db;
        }

        public async Task<PlannedInspection> CreatePlannedInspection(Festival festival)
        {
            var plannedInspection = new PlannedInspection {Festival = festival};

            if (!plannedInspection.Validate())
                throw new InvalidDataException();

            _db.PlannedInspections.Add(plannedInspection);
            await _db.SaveChangesAsync();

            return null;
        }

        public async Task<PlannedInspection> CreatePlannedInspection(
            Festival festival, 
            Questionnaire questionnaire, 
            DateTime startTime,
            DateTime endTime, 
            string eventTitle, 
            Employee employee)
        {

            var existing = _db.PlannedInspections.FirstOrDefault(x => x.Questionnaire.Id == questionnaire.Id && x.Festival.Id == festival.Id && x.Employee.Id == employee.Id && x.StartTime.Equals(startTime) && x.IsCancelled == null);

            if (existing != null)
                throw new EntityExistsException();

            var plannedInspection = new PlannedInspection {
                Festival = festival,
                Questionnaire = questionnaire,
                StartTime = startTime,
                EndTime = endTime,
                EventTitle = eventTitle,
                Employee = employee
            };

            
            if (!plannedInspection.Validate()) 
                throw new InvalidDataException();

            _db.PlannedInspections.Add(plannedInspection);

            await _db.SaveChangesAsync();

            return null;
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId)
        {

            var plannedInspection = await _db.PlannedInspections.FirstOrDefaultAsync(e => e.Id == plannedInspectionId);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public List<List<PlannedInspection>> GetPlannedInspectionsGrouped(Festival festival)
        {

            var plannedInspections = _db.PlannedInspections.Where(e => e.Festival.Id == festival.Id && e.IsCancelled == null).ToList();

            return plannedInspections
                .GroupBy(u => u.StartTime)
                .Select(grp => grp.ToList())
                .ToList();


        }
        public async Task<PlannedInspection> GetPlannedInspection(Festival festival, Employee employee, DateTime StartTime)
        {

            var plannedInspection = await _db.PlannedInspections.FirstOrDefaultAsync(e => e.Festival.Id == festival.Id && e.Employee.Id == employee.Id && e.StartTime.Equals(StartTime) && e.IsCancelled == null);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }
        public async Task<List<PlannedInspection>> GetPlannedInspections(Festival festival, DateTime StartTime)
        {

            var plannedInspections = await _db.PlannedInspections.Where(e => e.Festival.Id == festival.Id && e.StartTime.Equals(StartTime) && e.IsCancelled == null).ToListAsync();

            if (plannedInspections == null)
                throw new EntityNotFoundException();

            return plannedInspections;
        }

        public async Task RemoveInspection(int plannedInspectionId, string cancellationreason)
        {
            var plannedInspection = await GetPlannedInspection(plannedInspectionId);
            if (plannedInspection.Answers == null)
                throw new System.Exception();

            //Check if submitted answers by employee
            if (plannedInspection.Answers.Count > 0)
                throw new QuestionHasAnswersException();

            plannedInspection.IsCancelled = DateTime.Now;
            plannedInspection.CancellationReason = cancellationreason;

            //Check if cancellationreason is not longer than 250 characters
            if (!plannedInspection.Validate())
                throw new InvalidDataException();

            await _db.SaveChangesAsync();
        }
    }
}