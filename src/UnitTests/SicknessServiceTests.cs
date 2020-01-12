using System.Collections.Generic;
using System.Linq;
using Festispec.Models.EntityMapping;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UnitTests.Helpers;
using Moq;
using Xunit;
using System;

namespace Festispec.UnitTests
{
    public class SicknessServiceTests
    {
        private readonly Mock<FestispecContext> _dbMock;
        private readonly ISicknessService _sicknessService;
        private ModelMocks _modelMocks;

        public SicknessServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();

            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Employees).Object);

            _dbMock.Setup(x => x.Availabilities).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Availabilities).Object);

            _dbMock.Setup(x => x.PlannedEvents).Returns(MockHelpers.CreateDbSetMock(_modelMocks.PlannedEvents).Object);

            _dbMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);

            _sicknessService = new SicknessService(_dbMock.Object);
        }

        [Theory]
        [InlineData("Ik heb griep")]
        [InlineData("Ik heb mijn been gebroken")]
        public async void AddAbsense(string reason)
        {
            var sickness = await _sicknessService.AddAbsense(1, reason, null);

            Assert.NotNull(sickness);

            Assert.True(_sicknessService.IsSick(1));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void EnteringPassedDateShouldThrowException()
        {
            await Assert.ThrowsAsync<DateHasPassedException>(() => _sicknessService.AddAbsense(1, "test", new DateTime(2000, 10, 10)));
        }

        [Fact]
        public void IsSick()
        {
            Assert.True(_sicknessService.IsSick(1));
        }
    }
}