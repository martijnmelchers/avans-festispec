using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class InspectionService : IInspectionService
    {

        private readonly FestispecContext _db;

        public InspectionService(FestispecContext db)
        {
            _db = db;
        }

        public async Task<PlannedInspection> CreatePlannedInspection(Festival festival)
        {
            var plannedInspection = new PlannedInspection(festival);

            if (!plannedInspection.Validate())
                throw new InvalidDataException();

            _db.PlannedInspections.Add(plannedInspection);

            await _db.SaveChangesAsync();

            return null;
        }

        public async Task<PlannedInspection> CreatePlannedInspection(
            Festival festival, 
            Questionnaire questionnaire, 
            DateTime startTime,
            DateTime endTime, 
            string eventTitle, 
            Employee employee)
        {
            var plannedInspection = new PlannedInspection(festival)
            {
                Questionnaire = questionnaire,
                StartTime = startTime,
                EndTime = endTime,
                EventTitle = eventTitle,
                Employee = employee
            };

            if (!plannedInspection.Validate())
                throw new InvalidDataException();

            _db.PlannedInspections.Add(plannedInspection);

            await _db.SaveChangesAsync();

            return null;
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId)
        {

            var plannedInspection = await _db.PlannedInspections.FirstOrDefaultAsync(e => e.Id == plannedInspectionId && e.IsCancelled == null);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }
        public async Task<PlannedInspection> GetPlannedInspection(Festival festival, Employee employee, DateTime StartTime)
        {

            var plannedInspection = await _db.PlannedInspections.FirstOrDefaultAsync(e => e.Festival.Id == festival.Id && e.Employee.Id == employee.Id && e.StartTime.Equals(StartTime) && e.IsCancelled == null);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }
        public async Task<List<PlannedInspection>> GetPlannedInspections(Festival festival, DateTime StartTime)
        {

            var plannedInspections = await _db.PlannedInspections.Where(e => e.Festival.Id == festival.Id && e.StartTime.Equals(StartTime) && e.IsCancelled == null).ToListAsync();

            if (plannedInspections == null)
                throw new EntityNotFoundException();

            return plannedInspections;
        }

        public async Task RemoveInspection(int PlannedInspectionId)
        {
            var plannedInspection = await GetPlannedInspection(PlannedInspectionId);

            //Check if submitted answers by employee
            if (plannedInspection.Answers.Count > 0)
                throw new QuestionHasAnswersException();

            //_db.PlannedInspections.Remove(plannedInspection);
            plannedInspection.IsCancelled = DateTime.Now;

            await _db.SaveChangesAsync();
        }


        #warning temp till medwerkers beheren is made
        public List<Employee> GetEmployees()
        {
            var employees = _db.Employees.ToList();
            if (employees == null)
                throw new EntityNotFoundException();
            return employees;

        }

    }
}
