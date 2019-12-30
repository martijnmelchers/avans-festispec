using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IAvailabilityService
    {
        Task<Availability> AddUnavailabilityEntireDay(int employeeId, DateTime date, string reason);

        Task<Availability> AddUnavailabilityPartOfDay(int employeeId, DateTime beginDateTime, DateTime endDateTime, string reason);

        Task RemoveUnavailablity(int availabilityId);

        Task SaveChanges();

        Dictionary<int, List<Availability>> GetUnavailabilitiesForMonth(int employeeId, int month, int year);

        List<Availability> GetUnavailabilitiesForDay(int employeeId, DateTime date);
    }
}
