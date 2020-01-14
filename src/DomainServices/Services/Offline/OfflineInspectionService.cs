using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Helpers;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services.Offline
{
    [ExcludeFromCodeCoverage]
    public class OfflineInspectionService : IInspectionService
    {
        private readonly ISyncService<PlannedInspection> _plannedInspectionSyncService;
        private readonly ISyncService<Employee> _employeeSyncService;
        private readonly ISyncService<Festival> _festivalSyncService;

        public OfflineInspectionService(ISyncService<PlannedInspection> plannedInspectionSyncService,
            ISyncService<Employee> employeeSyncService, ISyncService<Festival> festivalSyncService)
        {
            _plannedInspectionSyncService = plannedInspectionSyncService;
            _employeeSyncService = employeeSyncService;
            _festivalSyncService = festivalSyncService;
        }

        public List<Employee> GetAllInspectors()
        {
            return _employeeSyncService.GetAll().Where(e => e.Account.Role == Role.Inspector).ToList();
        }

        public Task<PlannedInspection> CreatePlannedInspection(int festivalId, int questionnaireId, DateTime startTime,
            DateTime endTime,
            string eventTitle, int employeeId)
        {
            throw new InvalidOperationException();
        }

        public async Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId)
        {
            return await _plannedInspectionSyncService.GetEntityAsync(plannedInspectionId);
        }

        public async Task<PlannedInspection> GetPlannedInspection(Festival festival, Employee employee,
            DateTime startTime)
        {
            var plannedInspection = (await _plannedInspectionSyncService.GetAllAsync()).FirstOrDefault(
                e => e.Festival.Id == festival.Id && e.Employee.Id == employee.Id && e.StartTime.Equals(startTime) &&
                     e.IsCancelled == null);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public async Task<List<PlannedInspection>> GetPlannedInspections(int festivalId, DateTime startTime)
        {
            var plannedInspection = (await _plannedInspectionSyncService.GetAllAsync()).Where(e =>
                e.Festival.Id == festivalId && e.StartTime.Equals(startTime) && e.IsCancelled == null).ToList();

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        public async Task<List<PlannedInspection>> GetPlannedInspections(int employeeId)
        {
            return (await _plannedInspectionSyncService.GetAllAsync()).Where(e =>
                e.Employee.Id == employeeId && QueryHelpers.TruncateTime(e.StartTime) ==
                QueryHelpers.TruncateTime(DateTime.Now)).ToList();
        }

        public List<List<PlannedInspection>> GetPlannedInspectionsGrouped(Festival festival)
        {
            var plannedInspections = _plannedInspectionSyncService.GetAll()
                .Where(e => e.Festival.Id == festival.Id && e.IsCancelled == null).ToList();

            return plannedInspections
                .GroupBy(u => u.StartTime)
                .Select(grp => grp.ToList())
                .ToList();
        }

        public Task RemoveInspection(int plannedInspectionId, string cancellationReason)
        {
            throw new InvalidOperationException();
        }

        public async Task<Festival> GetFestivalAsync(int festivalId)
        {
            return await _festivalSyncService.GetEntityAsync(festivalId);
        }

        public Task<int> ProcessPlannedInspections(IEnumerable<PlannedInspection> plannedInspections,
            Questionnaire questionnaire, string instructions)
        {
            throw new InvalidOperationException();
        }

        public void Sync()
        {
        }
    }
}