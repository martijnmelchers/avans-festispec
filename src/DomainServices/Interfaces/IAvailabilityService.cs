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

        Task RemoveUnavailability(int availabilityId);

        Task SaveChanges();

        Availability GetUnavailabilityForDay(int employeeId, DateTime date);

        Task<Dictionary<long, Availability>> GetUnavailabilitiesForFuture(int employeeId, DateTime startDate);
    }
}
