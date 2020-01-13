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
        private SicknessService _fakeSicknessService;

        public SicknessServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            var noRowsMock = new Mock<FestispecContext>();
            var modelMocks = new ModelMocks();

            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(modelMocks.Employees).Object);
            noRowsMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(modelMocks.Employees).Object);

            _dbMock.Setup(x => x.Availabilities)
                .Returns(MockHelpers.CreateDbSetMock(modelMocks.Availabilities).Object);            
            noRowsMock.Setup(x => x.Availabilities)
                .Returns(MockHelpers.CreateDbSetMock(modelMocks.Availabilities).Object);

            _dbMock.Setup(x => x.PlannedEvents).Returns(MockHelpers.CreateDbSetMock(modelMocks.PlannedEvents).Object);
            noRowsMock.Setup(x => x.PlannedEvents).Returns(MockHelpers.CreateDbSetMock(modelMocks.PlannedEvents).Object);

            _dbMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);
            noRowsMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(0);

            _sicknessService = new SicknessService(_dbMock.Object);
            _fakeSicknessService = new SicknessService(noRowsMock.Object);
        }

        [Theory]
        [InlineData("Ik heb griep")]
        [InlineData("Ik heb mijn been gebroken")]
        public async void AddAbsence(string reason)
        {
            var sickness = await _sicknessService.AddAbsence(1, reason, null);

            Assert.NotNull(sickness);
            Assert.True(_sicknessService.IsSick(1));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void EnteringPassedDateShouldThrowException()
        {
            await Assert.ThrowsAsync<DateHasPassedException>(() =>
                _sicknessService.AddAbsence(1, "test", new DateTime(2000, 10, 10)));
        }

        [Fact]
        public void IsSick()
        {
            Assert.True(_sicknessService.IsSick(1));
        }

        [Fact]
        public async void InvalidDataThrowsException()
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => _sicknessService.AddAbsence(-1, string.Empty, null));
        }
        
        [Fact]
        public async void NoRowsChangedExceptionIsThrown()
        {
            await Assert.ThrowsAsync<NoRowsChangedException>(() => _fakeSicknessService.AddAbsence(1, "Test!", null));
        }
    }
}