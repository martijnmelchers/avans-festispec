using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Helpers;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class InspectionService : IInspectionService
    {
        private readonly FestispecContext _db;
        private readonly ISyncService<PlannedInspection> _syncService;

        public InspectionService(FestispecContext db, ISyncService<PlannedInspection> syncService)
        {
            _db = db;
            _syncService = syncService;
        }

        public List<Employee> GetAllInspectors()
        {
            return _db.Employees
                .Include(e => e.Address)
                .Include(e => e.PlannedEvents)
                .Where(e => e.Account.Role == Role.Inspector)
                .ToList();
        }

        public async Task<Festival> GetFestivalAsync(int festivalId)
        {
            var festival = await _db.Festivals
                .Include(f => f.Questionnaires)
                .Include(f => f.PlannedInspections)
                .Include(f => f.Address)
                .FirstOrDefaultAsync(f => f.Id == festivalId);

            if (festival == null)
                throw new EntityNotFoundException();

            return festival;
        }

        public async Task<PlannedInspection> CreatePlannedInspection(
            int festivalId,
            int questionnaireId,
            DateTime startTime,
            DateTime endTime,
            string eventTitle,
            int employeeId
        )
        {
            var existing = _db.PlannedInspections
                .FirstOrDefault(x =>
                    x.Questionnaire.Id == questionnaireId && x.Festival.Id == festivalId &&
                    x.Employee.Id == employeeId && x.StartTime.Equals(startTime) && x.IsCancelled == null);

            if (existing != null)
                throw new EntityExistsException();

            var plannedInspection = new PlannedInspection
            {
                Festival = _db.Festivals.FirstOrDefault(x => x.Id == festivalId),
                Questionnaire = _db.Questionnaires.FirstOrDefault(x => x.Id == questionnaireId),
                Employee = _db.Employees.FirstOrDefault(x => x.Id == employeeId),
                StartTime = startTime,
                EndTime = endTime,
                EventTitle = eventTitle
            };

            if (!plannedInspection.Validate())
                throw new InvalidDataException();

            _db.PlannedInspections.Add(plannedInspection);

            await _db.SaveChangesAsync();

            return null;
        }
        
        [ExcludeFromCodeCoverage] // the _db.Entry makes this borderline untestable
        public async Task<int> ProcessPlannedInspections(IEnumerable<PlannedInspection> plannedInspections,
            Questionnaire questionnaire)
        {
            foreach (var plannedInspection in plannedInspections)
            {
                plannedInspection.Questionnaire = questionnaire;
                if (plannedInspection.Id != 0)
                    _db.Entry(plannedInspection).State = EntityState.Modified;
                else
                {
                    if (plannedInspection.EndTime != null)
                        await CreatePlannedInspection(
                            plannedInspection.Festival.Id, 
                            plannedInspection.Questionnaire.Id,
                            plannedInspection.StartTime, 
                            (DateTime) plannedInspection.EndTime,
                            plannedInspection.EventTitle,
                            plannedInspection.Employee.Id);
                }
            }

            return await SaveChanges();
        }
        
        [ExcludeFromCodeCoverage]
        private async Task<int> SaveChanges()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId)
        {
            var plannedInspection = await _db.PlannedInspections
                .Include(pi => pi.Festival)
                .Include(pi => pi.Festival.Address)
                .FirstOrDefaultAsync(e => e.Id == plannedInspectionId);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public List<List<PlannedInspection>> GetPlannedInspectionsGrouped(Festival festival)
        {
            var plannedInspections = _db.PlannedInspections
                .Include(e => e.Employee.Address)
                .Where(e => e.Festival.Id == festival.Id && e.IsCancelled == null)
                .ToList();

            return plannedInspections
                .GroupBy(u => u.StartTime)
                .Select(grp => grp.ToList())
                .ToList();
        }

        public async Task<PlannedInspection> GetPlannedInspection(Festival festival, Employee employee,
            DateTime startTime)
        {
            var plannedInspection = await _db.PlannedInspections
                .FirstOrDefaultAsync(e => e.Festival.Id == festival.Id && e.Employee.Id == employee.Id && e.StartTime.Equals(startTime) && e.IsCancelled == null);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public async Task<List<PlannedInspection>> GetPlannedInspections(int festivalId, DateTime startTime)
        {
            List<PlannedInspection> plannedInspections = await _db.PlannedInspections
                .Where(e => e.Festival.Id == festivalId && e.StartTime.Equals(startTime) && e.IsCancelled == null)
                .ToListAsync();
            

            return plannedInspections;
        }


        public async Task<List<PlannedInspection>> GetPlannedInspections(int employeeId)
        {
            var plannedInspections = await _db.PlannedInspections
                .Include(e => e.Employee)
                .Where(e => e.Employee.Id == employeeId && QueryHelpers.TruncateTime(e.StartTime) == QueryHelpers.TruncateTime(DateTime.Now))
                .ToListAsync();

            if (plannedInspections.Count < 1)
                throw new EntityNotFoundException();

            return plannedInspections;
        }
        
        public async Task RemoveInspection(int plannedInspectionId, string cancellationReason)
        {
            var plannedInspection = await GetPlannedInspection(plannedInspectionId);
            
            //Check if submitted answers by employee
            if (plannedInspection.Answers.Count > 0)
                throw new QuestionHasAnswersException();

            plannedInspection.IsCancelled = DateTime.Now;
            plannedInspection.CancellationReason = cancellationReason;

            //Check if cancellation reason is not longer than 250 characters
            if (!plannedInspection.Validate())
                throw new InvalidDataException();

            await _db.SaveChangesAsync();
        }

        [ExcludeFromCodeCoverage]
        public void Sync()
        {
            var db = _syncService.GetSyncContext();
            
            var plannedInspections = db.PlannedInspections
                .Include(i => i.Festival)
                .Include(i => i.Festival.Address)
                .Include(i => i.Employee)
                .Include(i => i.Employee.Address).ToList();

            _syncService.Flush();
            _syncService.AddEntities(plannedInspections);
            _syncService.SaveChanges();
        }
    }
}