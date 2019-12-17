using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IInspectionService
    {
        #region planned Event
        //PlannedInspection GetPlannedInspection(int plannedInspectionId);
        //PlannedInspection GetPlannedInspection(Festival festival, Employee employee, DateTime StartTime);
        //List<PlannedInspection> GetPlannedInspections(Festival festival, DateTime StartTime);
        Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId);
        Task<PlannedInspection> GetPlannedInspection(Festival festival, Employee employee, DateTime StartTime);
        Task<List<PlannedInspection>> GetPlannedInspections(Festival festival, DateTime StartTime);
        IEnumerable<IEnumerable<PlannedInspection>> GetPlannedInspectionsGrouped(Festival festival);
        Task<PlannedInspection> CreatePlannedInspection(Festival festival);
        Task<PlannedInspection> CreatePlannedInspection(Festival festival, Questionnaire questionnaire, DateTime startTime,
            DateTime endTime, string eventTitle, Employee employee);
        Task RemoveInspection(int PlannedInspectionId, String CancellationReason);
        List<Employee> GetEmployees();
        Festival GetFestival(int id);
        Task SaveChanges();
        #endregion
    }
}
