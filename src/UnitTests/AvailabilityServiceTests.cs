using Festispec.Models.EntityMapping;
using Festispec.DomainServices.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.UnitTests.Helpers;
using Festispec.Models.Exception;
using System.Linq;
using Festispec.Models.Questions;

namespace Festispec.UnitTests
{
    public class AvailabilityServiceTests
    {
        private readonly Mock<FestispecContext> _dbMock;
        private readonly IAvailabilityService _availabilityService;
        private ModelMocks _modelMocks;
        public AvailabilityServiceTests()
        {
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();

            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Employees).Object);

            _dbMock.Setup(x => x.Availabilities).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Availability).Object);

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
        public void GetUnavailabilitiesForDay()
        {
            var availabilities = _availabilityService.GetUnavailabilityForDay(1, new DateTime(2019, 12, 28));

            Assert.NotNull(availabilities[0]);
        }

        [Fact]
        public void GetUnavailabilitiesForMonth()
        {
            var availabilities = _availabilityService.GetUnavailabilitiesForMonth(1, 12, 2019);

            Assert.NotNull(availabilities[28][0]);

            Assert.Throws<ArgumentOutOfRangeException>(() => availabilities[29][0]);
        }

        [Fact]
        public void RemoveUnavailablity()
        {
            _availabilityService.RemoveUnavailablity(2);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void RemovingInvalidUnavailabilityShouldThrowError()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _availabilityService.RemoveUnavailablity(10));
        }
    }
}
