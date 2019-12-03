﻿using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using System.Collections.Generic;
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

        public async Task<PlannedInspection> CreatePlannedInspection(Festival festival, Questionnaire questionnaire)
        {
            var plannedInspection = new PlannedInspection(festival);
            plannedInspection.Questionnaire = questionnaire;

            if (!plannedInspection.Validate())
                throw new InvalidDataException();

            _db.PlannedInspections.Add(plannedInspection);

            await _db.SaveChangesAsync();

            return null;
        }


        public PlannedInspection GetPlannedInspection(int plannedInspectionId)
        {
            var plannedInspection = _db.PlannedInspections.FirstOrDefault(e => e.Id == plannedInspectionId);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;

        }

        public async Task RemoveInspection(int PlannedInspectionId)
        {
            var plannedInspection = GetPlannedInspection(PlannedInspectionId);

            //Check if submitted answers by employee
            if (plannedInspection.Answers.Count > 0)
                throw new QuestionHasAnswersException();

            _db.PlannedInspections.Remove(plannedInspection);

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
