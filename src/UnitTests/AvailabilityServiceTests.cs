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

        public AvailabilityServiceTests()
        {
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();
            var modelMocks = new ModelMocks();

            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(modelMocks.Employees).Object);

            _dbMock.Setup(x => x.Availabilities).Returns(MockHelpers.CreateDbSetMock(modelMocks.Availabilities).Object);

            _dbMock.Setup(x => x.PlannedEvents).Returns(MockHelpers.CreateDbSetMock(modelMocks.PlannedEvents).Object);

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
            _availabilityService.RemoveUnavailability(2);

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
    }
}
