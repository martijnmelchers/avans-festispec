﻿using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System;
using System.Linq;
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

        public async Task AddAbsense(Employee employee, string reason, DateTime endTime)
        {
            var absense = new Availability()
            {
                IsAvailable = false,
                Employee = employee,
                EndTime = endTime,
                Reason = reason,
                StartTime = DateTime.Now,
                EventTitle = "Afwezig wegens ziekte"
            };

            if (!absense.Validate())
                throw new InvalidDataException();

            _db.PlannedEvents.Add(absense);

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();
        }

        public async Task EndAbsense(int employeeId)
        {
            var absense = GetCurrentAbsense(employeeId);

            if (absense == null)
                throw new EmployeeNotSickException();

            absense.EndTime = DateTime.Now;

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();
        }

        private Availability GetCurrentAbsense(int employeeId)
        {
            return _db.Availabilities.FirstOrDefault(a => a.Employee.Id == employeeId
            && a.EventTitle == "Afwezig wegens ziekte"
            && a.EndTime <= DateTime.Now);
        }

        public bool IsSick(int employeeId)
        {
            var absense = GetCurrentAbsense(employeeId);

            return absense != null;
        }

        


    }
}
