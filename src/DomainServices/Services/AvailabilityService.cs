using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly FestispecContext _db;

        public AvailabilityService(FestispecContext db)
        {
            _db = db;
        }

        public async Task<Availability> AddUnavailabilityEntireDay(int employeeId, DateTime date, string reason)
        {
            if (date < DateTime.Now)
                throw new DateHasPassedException();

            var employee = _db.Employees.FirstOrDefault(e => e.Id == employeeId);

            var availability = new Availability
            {
                IsAvailable = false,
                Employee = employee,
                StartTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0),
                EndTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59),
                Reason = reason,
                EventTitle = "Niet beschikbaar"
            };

            if (!availability.Validate())
                throw new InvalidDataException();

            _db.PlannedEvents.Add(availability);
            await _db.SaveChangesAsync();

            return availability;
        }  

        public Availability GetUnavailabilityForDay(int employeeId, DateTime date)
        {
            return _db.Availabilities.FirstOrDefault(
                a => a.Employee.Id == employeeId 
                     && _db.TruncateTime(a.StartTime) == _db.TruncateTime(date) 
                     && a.EventTitle == "Niet beschikbaar");
        }

        public async Task RemoveUnavailability(int availabilityId)
        {
            var availability = _db.Availabilities.FirstOrDefault(a => a.Id == availabilityId);

            if (availability == null)
                throw new EntityNotFoundException();

            _db.PlannedEvents.Remove(availability);

            await _db.SaveChangesAsync();
        }

        public async Task<Dictionary<long, Availability>> GetUnavailabilityForFuture(int employeeId, DateTime startDate)
        {
             var list = await _db.Availabilities
                .OrderByDescending(c => c.EndTime)
                .Where(c => c.StartTime > startDate)
                .Where(c => c.Employee.Id == employeeId)
                .Where(c => c.EventTitle == "Niet beschikbaar") // This is really bad practice!
                .ToListAsync();
            var dictionary = new Dictionary<long, Availability>();
            foreach (Availability availability in list.Where(availability => availability.EndTime != null))
            {
                CalculateTimeFromEpoch(availability).ToList().ForEach(l => dictionary.Add(l, availability));
            }
            return dictionary;
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public static IEnumerable<long> CalculateTimeFromEpoch(Availability availability)
        {
            return EachDay(availability.StartTime, (DateTime) availability.EndTime)
                .Select(day => (long) (day - new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
