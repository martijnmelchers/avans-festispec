using Festispec.Models.EntityMapping;
using Festispec.DomainServices.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.UnitTests.Helpers;
using Festispec.Models.Exception;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Helpers;

namespace Festispec.UnitTests
{
    public class AvailabilityServiceTests
    {
        private readonly Mock<FestispecContext> _dbMock;
        private readonly IAvailabilityService _availabilityService;
        private readonly ModelMocks _modelMocks;

        public AvailabilityServiceTests()
        {
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();

            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Employees).Object);

            _dbMock.Setup(x => x.Availabilities).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Availabilities).Object);

            _dbMock.Setup(x => x.PlannedEvents).Returns(MockHelpers.CreateDbSetMock(_modelMocks.PlannedEvents).Object);

            _dbMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);

            _availabilityService = new AvailabilityService(_dbMock.Object);
        }

        [Fact]
        public async void AddUnavailabilityEntireDay()
        {
            var availability = await _availabilityService.AddUnavailabilityEntireDay(1, new DateTime(2500, 10, 10), null);

            Assert.NotNull(availability);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void EnteringPassedDateShouldThrowError()
        {
            await Assert.ThrowsAsync<DateHasPassedException>(() => _availabilityService.AddUnavailabilityEntireDay(1, new DateTime(2000, 10, 10), null));
        }        

        [Fact]
        public void RemoveUnavailability()
        {
            _availabilityService.RemoveUnavailability(4);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void RemovingInvalidUnavailabilityShouldThrowError()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _availabilityService.RemoveUnavailability(10));
        }

        [Fact]
        public async void InvalidAddUnavailabilityThrowsException()
        {
            await Assert.ThrowsAsync<InvalidDataException>(() =>
                _availabilityService.AddUnavailabilityEntireDay(-1, DateTime.Now.Add(new TimeSpan(1,0,0)), string.Empty));
        }

        [Theory]
        [InlineData(5)]
        public void GetUnavailabilityForDayReturnsCorrectAvailability(int availabilityId)
        {
            var expected = _modelMocks.PlannedEvents.First(pe => pe.Id == availabilityId) as Availability;
            Assert.True(expected != null);

            Availability actual = _availabilityService.GetUnavailabilityForDay(expected.Employee.Id,
                QueryHelpers.TruncateTime(expected.StartTime));
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData(2, 2019, 01, 01)]
        public async Task GetUnavailabilityForFutureReturnsCorrectAvailability(int employeeId, int year, int month, int day)
        {
            var datetime = new DateTime(year, month, day);
            var expected = new Dictionary<long, Availability>();
            _modelMocks.PlannedEvents
                .OfType<Availability>()
                .Where(a => a.StartTime > datetime && a.Employee.Id == employeeId && a.EventTitle == "Niet beschikbaar")
                .ToList()
                .ForEach(
                availability => AvailabilityService.CalculateTimeFromEpoch(availability)
                    .ToList()
                    .ForEach(l => expected.Add(l, availability)));

            Dictionary<long, Availability> actual = await _availabilityService.GetUnavailabilityForFuture(employeeId, datetime);
            
            Assert.Equal(expected, actual);
        }
    }
}
