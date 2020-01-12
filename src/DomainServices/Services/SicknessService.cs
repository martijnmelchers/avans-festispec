using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class SicknessService : ISicknessService
    {
        private readonly FestispecContext _db;

        public SicknessService(FestispecContext db)
        {
            _db = db;
        }

        public async Task<Availability> AddAbsence(int employeeId, string reason, DateTime? endTime)
        {
            if (endTime < DateTime.Now)
                throw new DateHasPassedException();

            var employee = _db.Employees.Include(e => e.Address).FirstOrDefault(e => e.Id == employeeId);

            var absence = new Availability()
            {
                IsAvailable = false,
                Employee = employee,
                EndTime = endTime,
                Reason = reason,
                StartTime = DateTime.Now,
                EventTitle = "Afwezig wegens ziekte"
            };

            if (!absence.Validate())
                throw new InvalidDataException();

            _db.PlannedEvents.Add(absence);          

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();

            return absence;
        }

        public async Task EndAbsence(int employeeId)
        {
            var absence = GetCurrentAbsence(employeeId);

            if (absence == null)
                throw new EmployeeNotSickException();

            absence.EndTime = DateTime.Now;

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();
        }

        private Availability GetCurrentAbsence(int employeeId)
        {
            return _db.Availabilities.FirstOrDefault(a => a.Employee.Id == employeeId
            && a.EventTitle == "Afwezig wegens ziekte"
            && (a.EndTime >= DateTime.Now || a.EndTime == null));
        }

        public bool IsSick(int employeeId)
        {
            var absence = GetCurrentAbsence(employeeId);

            return absence != null;
        }
    }
}
