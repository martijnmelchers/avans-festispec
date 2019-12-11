using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models.EntityMapping;
using Festispec.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Festispec.UnitTests.Helpers;
using Xunit;
using Festispec.Models.Exception;
using Festispec.Models.Answers;

namespace Festispec.UnitTests
{
    public class InspectionServiceTests
    {
        private readonly Mock<FestispecContext> _dbMock;

        private readonly IInspectionService _inspectionService;

        public InspectionServiceTests()
        {
            // Setup database mock
            _dbMock = new Mock<FestispecContext>();

            // Setup add mock
            _dbMock.Setup(x => x.PlannedInspections.Add(It.IsAny<PlannedInspection>())).Returns((PlannedInspection u) => u);

            // Mock accounts
            _dbMock.Setup(x => x.PlannedInspections).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().plannedInspections).Object);                        

            // Create InspectionService
            _inspectionService = new InspectionService(_dbMock.Object);
        }


        #region Creating Planned Inspections Tests
        [Fact]
        public async void InvalidDataShouldThrowError()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _inspectionService.CreatePlannedInspection(
                ModelMocks.FestivalPinkPop,
                ModelMocks.Questionnaire4,
                new DateTime(2020, 3, 4, 13, 40, 0),
                new DateTime(2020, 3, 4, 17, 0, 0),
                null,
                ModelMocks.Employee));
        }

        [Fact]
        public async void CreatingExistingPlannedInspectionAgainShouldThrowError()
        {
            await Assert.ThrowsAsync<EntityExistsException>(() => _inspectionService.CreatePlannedInspection(
                ModelMocks.FestivalPinkPop,
                ModelMocks.Questionnaire4,
                new DateTime(2020, 3, 4, 12, 30, 0),
                new DateTime(2020, 3, 4, 17, 0, 0),
                "Pinkpop",
                ModelMocks.Employee));
        }

        #endregion

        #region Removing Inspection Tests
        [Fact]
        public async void RemovingInspectionWithAnswersShouldthrowError() 
        {
            await Assert.ThrowsAsync<QuestionHasAnswersException>(() => _inspectionService.RemoveInspection(ModelMocks.PlannedInspection2.Id));
        }

        #endregion

    }
}
