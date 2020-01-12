using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models.EntityMapping;
using Festispec.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            _inspectionService = new InspectionService(_dbMock.Object, new JsonSyncService<PlannedInspection>(_dbMock.Object));
        }


        #region Creating Planned Inspections Tests
        [Fact]
        public async void InvalidDataShouldThrowError()
        {
            PlannedInspection plannedInspection = ModelMocks.PlannedInspectionThunderDome;
            string eventTitle = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. " +
                "Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque " +
                "penatibus et magnis dis parturient montes,";

            await Assert.ThrowsAsync<InvalidDataException>(() => _inspectionService.CreatePlannedInspection(
                plannedInspection.Festival,
                plannedInspection.Questionnaire,
                new DateTime(2019, 12, 10, 12, 30, 0),
                new DateTime(2019, 12, 10, 19, 0, 0),
                eventTitle,
                plannedInspection.Employee));
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
            await Assert.ThrowsAsync<QuestionHasAnswersException>(() => _inspectionService.RemoveInspection(ModelMocks.PlannedInspectionThunderDome.Id, "slecht weer"));
        }

        [Fact]
        public async void InvalidCancellationReasonShouldThrowError()
        {
            String cancellationReason = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo" +
                " ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis " +
                "parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, " +
                "pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec " +
                "pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, " +
                "rhoncus ut, imperdiet";

            await Assert.ThrowsAsync<InvalidDataException>(() =>  _inspectionService.RemoveInspection(ModelMocks.PlannedInspectionPinkpop.Id, cancellationReason));
        }
        #endregion

        #region getInspections

        [Fact]
        public async void GetPlannedInspectionShouldReturnPlannedInspection()
        {
            PlannedInspection expected = ModelMocks.PlannedInspectionThunderDome;
            
            PlannedInspection actual = await _inspectionService.GetPlannedInspection(ModelMocks.festivalThunderDome, ModelMocks.Employee, ModelMocks.PlannedInspectionThunderDome.StartTime);
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async void GetPlannedInspectionsShouldReturnListOfPlannedInspections()
        {
            List<PlannedInspection> expected = new List<PlannedInspection>();
            expected.Add(ModelMocks.PlannedInspectionThunderDome);
            
            List<PlannedInspection> actual = await _inspectionService.GetPlannedInspections(ModelMocks.festivalThunderDome, ModelMocks.PlannedInspectionThunderDome.StartTime);
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void GetPlannedInspectionsShouldReturnPlannedInspections()
        {
            PlannedInspection expected = ModelMocks.PlannedInspectionThunderDome;
            
            PlannedInspection actual = await _inspectionService.GetPlannedInspection(2);
            
            Assert.Equal(expected, actual);
        }
        #endregion

    }
}
