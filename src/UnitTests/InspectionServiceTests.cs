using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Helpers;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using Festispec.UnitTests.Helpers;
using Moq;
using Xunit;

namespace Festispec.UnitTests
{
    public class InspectionServiceTests
    {
        public InspectionServiceTests()
        {
            // Setup database mock
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();
            // Setup add mock
            _dbMock.Setup(x => x.PlannedInspections.Add(It.IsAny<PlannedInspection>()))
                .Returns((PlannedInspection u) => u);

            // Mock accounts
            _dbMock.Setup(x => x.PlannedInspections)
                .Returns(MockHelpers.CreateDbSetMock(new ModelMocks().PlannedInspections).Object);
            _dbMock.Setup(x => x.Festivals).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Festivals).Object);
            _dbMock.Setup(x => x.Questionnaires)
                .Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questionnaires).Object);
            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Employees).Object);

            // Create InspectionService
            _inspectionService =
                new InspectionService(_dbMock.Object, new JsonSyncService<PlannedInspection>(_dbMock.Object));
        }

        private readonly Mock<FestispecContext> _dbMock;

        private readonly IInspectionService _inspectionService;
        private readonly ModelMocks _modelMocks;

        [Fact]
        public async void CreatingExistingPlannedInspectionAgainShouldThrowError()
        {
            await Assert.ThrowsAsync<EntityExistsException>(() => _inspectionService.CreatePlannedInspection(
                _modelMocks.Festivals.FirstOrDefault(f => f.Id == 1).Id,
                _modelMocks.Questionnaires.First(q => q.Id == 1).Id,
                new DateTime(2020, 3, 4, 12, 30, 0),
                new DateTime(2020, 3, 4, 17, 0, 0),
                "Pinkpop",
                _modelMocks.Employees.First(e => e.Id == 2).Id));
            _dbMock.Verify(x=>x.SaveChangesAsync(),Times.Never);
        }
        [Fact]
        public async void CreatingPlannedInspectionShouldCreatePlannedInspection()
        {
             await _inspectionService.CreatePlannedInspection(
                _modelMocks.Festivals.FirstOrDefault(f => f.Id == 1).Id,
                _modelMocks.Questionnaires.First(q => q.Id == 1).Id,
                new DateTime(2020, 5, 4, 12, 30, 0),
                new DateTime(2020, 5, 4, 17, 0, 0),
                "Pinkpop",
                _modelMocks.Employees.First(e => e.Id == 3).Id);
            _dbMock.Verify(x=>x.SaveChangesAsync(),Times.Once);
            
        }

        [Theory]
        [InlineData(2,2,2)]
        public async void GetPlannedInspectionShouldReturnPlannedInspection(int plannedInspectionId, int employeeId, int festivalId)
        {
           
            PlannedInspection expected = _dbMock.Object.PlannedInspections.FirstOrDefault(p => p.Id == plannedInspectionId);
            Assert.Equal(expected, await _inspectionService.GetPlannedInspection(_modelMocks.Festivals.First(f=>f.Id == festivalId),
            _modelMocks.Employees.First(e=>e.Id == employeeId),
            _modelMocks.PlannedInspections.First(p=>p.Id == plannedInspectionId).StartTime));

        }
        [Theory]
        [InlineData(2,2,2)]
        public async void GetPlannedInspectionShouldReturnError(int plannedInspectionId, int employeeId, int festivalId)
        {
           
            await Assert.ThrowsAsync<EntityNotFoundException>(()=>_inspectionService.GetPlannedInspection(_modelMocks.Festivals.First(f=>f.Id == festivalId),
            _modelMocks.Employees.First(e=>e.Id == employeeId),DateTime.Now));

        }

        [Theory]
        [InlineData(1)]
        public async void GetNonExistingPlannedInspectionsShouldThrowError(int employeeId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _inspectionService.GetPlannedInspections(employeeId));
        }
        
        
        [Theory]
        [InlineData(2,2)]
        public async void GetPlannedInspectionsShouldReturnListOfPlannedInspections(int plannedInspectionId,int festivalId)
        {
            List<PlannedInspection> expected = _dbMock.Object.PlannedInspections.Where(p=>p.Id ==plannedInspectionId).ToList();
            
            List<PlannedInspection> actual = await _inspectionService.GetPlannedInspections(_dbMock.Object.Festivals.First(f=>f.Id == festivalId).Id,_dbMock.Object.PlannedInspections.First(p=> p.Id == plannedInspectionId).StartTime);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllInspectorsShouldReturnListOfInspectors()
        {
            List<Employee> expected = _dbMock.Object.Employees.Where(e => e.Account.Role == Role.Inspector).ToList();

            List<Employee> actual = _inspectionService.GetAllInspectors();
            Assert.Equal(expected,actual);

        }

        [Theory]
        [InlineData(1)]
        public async void GetFestivalAsyncShouldReturnFestival(int festivalId)
        {
            Festival expected = _dbMock.Object.Festivals.First(f => f.Id == festivalId);
            Festival actual = await _inspectionService.GetFestivalAsync(festivalId);
            
            Assert.Equal(expected,actual);
        }

        [Theory]
        [InlineData(99)]
        public async void GetFestivalAsyncvShouldThrowEntityNotFoundException(int festivalId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _inspectionService.GetFestivalAsync(festivalId));
        }
        
        [Theory]
        [InlineData(2)]
        public async void GetPlannedInspectionsShouldReturnPlannedInspections(int plannedInspectionId)
        {
            PlannedInspection expected = _dbMock.Object.PlannedInspections.FirstOrDefault(p => p.Id == plannedInspectionId);
            Assert.Equal(expected, await _inspectionService.GetPlannedInspection(plannedInspectionId));
            
        }
        
        [Theory]
        [InlineData(99)]
        public async void GetPlannedInspectionsThrowEntityNotFoundException(int plannedInspectionId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _inspectionService.GetPlannedInspection(plannedInspectionId));
        }

        [Fact]
        public async void InvalidCancellationReasonShouldThrowError()
        {
            string cancellationReason = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo" +
                                        " ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis " +
                                        "parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, " +
                                        "pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec " +
                                        "pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, " +
                                        "rhoncus ut, imperdiet";

            await Assert.ThrowsAsync<InvalidDataException>(() =>
                _inspectionService.RemoveInspection(_modelMocks.PlannedInspections.First(p=>p.Id == 2).Id, cancellationReason));
        }

        [Fact]
        public async void InvalidDataShouldThrowError()
        {
            PlannedInspection plannedInspection = _modelMocks.PlannedInspections.Find(e => e.Id == 1);
            string eventTitle = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. " +
                                "Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque " +
                                "penatibus et magnis dis parturient montes,";

            await Assert.ThrowsAsync<InvalidDataException>(() => _inspectionService.CreatePlannedInspection(
                plannedInspection.Festival.Id,
                plannedInspection.Questionnaire.Id,
                new DateTime(2019, 12, 10, 12, 30, 0),
                new DateTime(2019, 12, 10, 19, 0, 0),
                eventTitle,
                plannedInspection.Employee.Id));
        }

        [Theory]
        [InlineData(2)]
        public async void GetPlannedInspectionByEmployeeIdShouldReturnListOfPlannedInspections(int employeeId)
        {
            List<PlannedInspection> expected =
                _dbMock.Object.PlannedInspections.Where(p => p.Employee.Id == employeeId && p.StartTime == QueryHelpers.TruncateTime(DateTime.Now)).ToList();
            List<PlannedInspection> actual = await _inspectionService.GetPlannedInspections(employeeId);
            
            Assert.Equal(expected,actual);
        }

        [Fact]
        public async void RemovingInspectionWithAnswersShouldthrowError()
        {
            await Assert.ThrowsAsync<QuestionHasAnswersException>(() =>
                _inspectionService.RemoveInspection(_modelMocks.Festivals.First(f => f.Id == 1).Id, "slecht weer"));
        }



        [Fact]
        public async void GetPlannedInspectionsShouldReturnListOfPlannedInspectionsByFestivalAndStartTime()
        {
            List<PlannedInspection> expected = await _dbMock.Object.PlannedInspections.Where(p =>
                p.Festival.Id == 1 && p.StartTime == new DateTime(2020, 3, 4, 12, 30, 0)).ToListAsync();
            List<PlannedInspection> actual =
                await _inspectionService.GetPlannedInspections(1, new DateTime(2020, 3, 4, 12, 30, 0));
            Assert.Equal(expected,actual);
        }
        
    }
}