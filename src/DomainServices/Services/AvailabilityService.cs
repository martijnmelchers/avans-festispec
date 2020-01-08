using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var availability = new Availability()
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

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();

            return availability;
        }

        public async Task<Availability> AddUnavailabilityPartOfDay(int employeeId, DateTime beginDateTime, DateTime endDateTime, string reason)
        {
            if (beginDateTime < DateTime.Now || endDateTime < DateTime.Now)
                throw new DateHasPassedException();

            if (endDateTime < beginDateTime)
                throw new EndDateEarlierThanStartDateException();

            if (beginDateTime.Date != endDateTime.Date)
                throw new StartAndEndDateDifferentDaysException();

            var employee = _db.Employees.FirstOrDefault(e => e.Id == employeeId);

            var availability = new Availability()
            {
                IsAvailable = false,
                Employee = employee,
                StartTime = beginDateTime,
                EndTime = endDateTime,
                Reason = reason,
                EventTitle = "Niet beschikbaar"
            };

            if (!availability.Validate())
                throw new InvalidDataException();

            _db.PlannedEvents.Add(availability);

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();

            return availability;
        }

        public List<Availability> GetUnavailabilitiesForDay(int employeeId, DateTime date)
        {
            var availabilities = _db.Availabilities.Where(a => a.Employee.Id == employeeId && a.StartTime.Date == date.Date && a.EventTitle == "Niet beschikbaar").ToList();

            return availabilities;
        }

        public Dictionary<int, List<Availability>> GetUnavailabilitiesForMonth(int employeeId, int month, int year)
        {
            if (month > 12)
                throw new InvalidDataException();

            Dictionary<int, List<Availability>> availabilities = new Dictionary<int, List<Availability>>();

            for (var x =1; x <= DateTime.DaysInMonth(year,month); x++)
            {
                
                availabilities.Add(x, GetUnavailabilitiesForDay(employeeId, new DateTime(year, month, x)));
            }

            return availabilities;
        }

        public async Task RemoveUnavailablity(int availabilityId)
        {
            var availability = _db.Availabilities.FirstOrDefault(a => a.Id == availabilityId);

            if (availability == null)
                throw new EntityNotFoundException();

            _db.PlannedEvents.Remove(availability);

            await _db.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<Dictionary<int, Availability>> GetUnavailabilitiesForFuture(int employeeId, DateTime startDate)
        {
             var list = await _db.Availabilities
                .OrderByDescending(c => c.EndTime)
                .Where(c => c.StartTime > startDate &&  c.Employee.Id == employeeId)
                .ToListAsync();
            var dictionary = new Dictionary<int, Availability>();
            foreach (Availability availability in list)
            {
                foreach (DateTime day in EachDay(availability.StartTime, availability.EndTime))
                {
                    int epoch = (int)(day - new DateTime(1970, 1, 1)).TotalSeconds;
                    dictionary.Add(epoch, availability);
                }
            }
            return dictionary;
        }

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
