using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class OfflineInspectionService : IInspectionService
    {
        private readonly ISyncService<PlannedInspection> _syncService;

        public OfflineInspectionService(ISyncService<PlannedInspection> syncService)
        {
            _syncService = syncService;
        }
    
        public async Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId)
        {
            return await _syncService.GetEntityAsync(plannedInspectionId);
        }

        public async Task<PlannedInspection> GetPlannedInspection(Festival festival, Employee employee, DateTime StartTime)
        {
            PlannedInspection plannedInspection = (await _syncService.GetAllAsync()).FirstOrDefault(e => e.Festival.Id == festival.Id && e.Employee.Id == employee.Id && e.StartTime.Equals(StartTime) && e.IsCancelled == null);
            
            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public async Task<List<PlannedInspection>> GetPlannedInspections(Festival festival, DateTime StartTime)
        {
            var plannedInspection = (await _syncService.GetAllAsync()).Where(e =>
                e.Festival.Id == festival.Id && e.StartTime.Equals(StartTime) && e.IsCancelled == null).ToList();
            
            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public async Task<List<PlannedInspection>> GetPlannedInspections(int employeeId)
        {
            return (await _syncService.GetAllAsync()).Where(e =>
                e.Employee.Id == employeeId && EntityFunctions.TruncateTime(e.StartTime) ==
                EntityFunctions.TruncateTime(DateTime.Now)).ToList();
        }

        public List<List<PlannedInspection>> GetPlannedInspectionsGrouped(Festival festival)
        {
            var plannedInspections = _syncService.GetAll().Where(e => e.Festival.Id == festival.Id && e.IsCancelled == null).ToList();

            return plannedInspections
                .GroupBy(u => u.StartTime)
                .Select(grp => grp.ToList())
                .ToList();
        }

        public Task<PlannedInspection> CreatePlannedInspection(Festival festival)
        {
            throw new InvalidOperationException();
        }

        public Task<PlannedInspection> CreatePlannedInspection(Festival festival, Questionnaire questionnaire, DateTime startTime, DateTime endTime,
            string eventTitle, Employee employee)
        {
            throw new InvalidOperationException();
        }

        public Task RemoveInspection(int plannedInspectionId, string cancellationreason)
        {
            throw new InvalidOperationException();
        }

        public Task SaveChanges()
        {
            throw new InvalidOperationException();
        }

        public void Sync()
        {
        }
    }
}