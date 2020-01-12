using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface IInspectionService : ISyncable
    {

        Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId);
        Task<PlannedInspection> GetPlannedInspection(Festival festival, Employee employee, DateTime startTime);
        Task<List<PlannedInspection>> GetPlannedInspections(int festivalId, DateTime startTime);
        Task<List<PlannedInspection>> GetPlannedInspections(int employeeId);
        List<List<PlannedInspection>> GetPlannedInspectionsGrouped(Festival festival);
        
        List<Employee> GetAllInspectors();
        
        Task<PlannedInspection> CreatePlannedInspection(
            int festivalId,
            int questionnaireId,
            DateTime startTime,
            DateTime endTime,
            string eventTitle,
            int employeeId
        );

        Task RemoveInspection(int plannedInspectionId, string cancellationReason);
        Task<Festival> GetFestivalAsync(int festivalId);
        Task<int> ProcessPlannedInspections(IEnumerable<PlannedInspection> plannedInspections,
            Questionnaire questionnaire);
    }
}