using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
    public class FestivalServiceTests
    {
        
        private readonly Mock<FestispecContext> _dbMock;
        private readonly IFestivalService _festivalService;
        
        public FestivalServiceTests()
        {
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();

            _dbMock.Setup(x => x.Questionnaires).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questionnaires).Object);
            _dbMock.Setup(x => x.Questions).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questions).Object);
            _dbMock.Setup(x => x.Answers).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Answers).Object);
            _dbMock.Setup(x => x.PlannedInspections).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().PlannedInspections).Object);
            _dbMock.Setup(x => x.Festivals).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Festivals).Object);
            _dbMock.Setup(x => x.Addresses).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Addresses).Object);
            _dbMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);

            _festivalService = new FestivalService(_dbMock.Object, new JsonSyncService<Festival>(_dbMock.Object), new AddressService(_dbMock.Object));
        }


        [Theory]
        [InlineData(1)]
        public async void GetFestivalShouldReturnFestival(int festivalId)
        {
            Festival expected = await _dbMock.Object.Festivals.FirstAsync(f => f.Id == festivalId);
            Festival actual = _festivalService.GetFestival(festivalId);
            
            Assert.Equal(expected,actual);
        }
        [Theory]
        [InlineData(99)]
        public void GetNonExistingFestivalShouldReturnErrror(int festivalId)
        {
            Assert.Throws<EntityNotFoundException>(() => _festivalService.GetFestival(festivalId));
        }

        [Fact]
        public async void GetFestivalsShouldReturnFestivalList()
        {
            List<Festival> expected = await _dbMock.Object.Festivals.ToListAsync();
            List<Festival> actual =  _festivalService.GetFestivals().ToList();
            
            Assert.Equal(expected,actual);
        }

        [Theory]
        [InlineData(3)]
        public async void RemoveFestivalShouldRemoveFestival(int festivalId)
        {
            await _festivalService.RemoveFestival(festivalId);
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() =>  _festivalService.RemoveFestival(festivalId));

        }



    }
}