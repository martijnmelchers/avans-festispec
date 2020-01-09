using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<PlannedInspection> CreatePlannedInspection(Festival festival)
        {
            var plannedInspection = new PlannedInspection { Festival = festival };

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
            Employee employee
        )
        {
            PlannedInspection existing = _db.PlannedInspections
                .FirstOrDefault(x => x.Questionnaire.Id == questionnaire.Id && x.Festival.Id == festival.Id &&x.Employee.Id == employee.Id && x.StartTime.Equals(startTime) && x.IsCancelled == null);

            if (existing != null)
                throw new EntityExistsException();

            var plannedInspection = new PlannedInspection
            {
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
            PlannedInspection plannedInspection = await _db.PlannedInspections
                .FirstOrDefaultAsync(e => e.Id == plannedInspectionId);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }


        public List<List<PlannedInspection>> GetPlannedInspectionsGrouped(Festival festival)
        {
            List<PlannedInspection> plannedInspections = _db.PlannedInspections
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
            PlannedInspection plannedInspection = await _db.PlannedInspections
                .FirstOrDefaultAsync(e => e.Festival.Id == festival.Id && e.Employee.Id == employee.Id && e.StartTime.Equals(startTime) && e.IsCancelled == null);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public async Task<List<PlannedInspection>> GetPlannedInspections(Festival festival, DateTime startTime)
        {
            List<PlannedInspection> plannedInspections = await _db.PlannedInspections
                .Where(e => e.Festival.Id == festival.Id && e.StartTime.Equals(startTime) && e.IsCancelled == null)
                .ToListAsync();

            if (plannedInspections == null)
                throw new EntityNotFoundException();

            return plannedInspections;
        }


        public async Task<List<PlannedInspection>> GetPlannedInspections(int employeeId)
        {
            List<PlannedInspection> plannedInspections = await _db.PlannedInspections
                .Include(e => e.Employee)
                .Where(e => e.Employee.Id == employeeId && EntityFunctions.TruncateTime(e.StartTime) == EntityFunctions.TruncateTime(DateTime.Now))
                .ToListAsync();



            if (plannedInspections.Count < 1)
                throw new EntityNotFoundException();

            return plannedInspections;
        }


        public async Task RemoveInspection(int plannedInspectionId, string cancellationreason)
        {
            PlannedInspection plannedInspection = await GetPlannedInspection(plannedInspectionId);


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

        public void Sync()
        {
            FestispecContext db = _syncService.GetSyncContext();
            
            List<PlannedInspection> plannedInspections = db.PlannedInspections
                .Include(i => i.Festival)
                .Include(i => i.Employee)
                .Include(i => i.Employee.Address).ToList();

            _syncService.Flush();
            _syncService.AddEntities(plannedInspections);
            _syncService.SaveChanges();
        }
    }
}