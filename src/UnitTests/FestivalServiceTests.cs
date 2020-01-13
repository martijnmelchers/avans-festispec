using System;
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
            _dbMock.Setup(x => x.Customers).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Customers).Object);
            _dbMock.Setup(x => x.Questions).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questions).Object);
            _dbMock.Setup(x => x.Answers).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Answers).Object);
            _dbMock.Setup(x => x.PlannedInspections).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().PlannedInspections).Object);
            _dbMock.Setup(x => x.Festivals).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Festivals).Object);
            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Employees).Object);
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
        [Theory]
        [InlineData(99)]
        public async void RemovingNonExistingShouldThrowError(int festivalId)
        {
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() =>  _festivalService.RemoveFestival(festivalId));

        }
        [Theory]
        [InlineData(1)]
        public async void RemovingWithQuestionnairesShouldThrowError(int festivalId)
        {
            
            await Assert.ThrowsAsync<FestivalHasQuestionnairesException>(() =>  _festivalService.RemoveFestival(festivalId));

        }

        [Theory]
        [InlineData(1, 1)]
        public async void CreateFestivalShouldCreateFestival(int festivalId, int customerId)
        {
            Festival expected = await _dbMock.Object.Festivals.FirstAsync(f => f.Id == festivalId);
            Festival actual = await _festivalService.CreateFestival(expected, customerId);
            
            Assert.Equal(expected,actual);
        }

        [Fact]
        public async void CreateFestivalWithEarlierClosingHoursShouldThrowError()
        {
            Festival festival = await _dbMock.Object.Festivals.FirstAsync(f => f.Id == 1);
            festival.OpeningHours.StartDate = DateTime.Now;
            festival.OpeningHours.EndDate = DateTime.MinValue;
            await Assert.ThrowsAsync<EndDateEarlierThanStartDateException>(() =>
                _festivalService.CreateFestival(festival, 1));
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async void CreateFestivalWithTooLongNameShouldThrowError(string name)
        {
            Festival festival = await _dbMock.Object.Festivals.FirstAsync(f => f.Id == 1);
            festival.FestivalName = name;
            await Assert.ThrowsAsync<InvalidDataException>(()=> _festivalService.CreateFestival( festival, 1 ));
        }
        
        [Theory]
        [InlineData(1)]
        public async void UpdateFestivalShouldUpdateFestival(int festivalId )
        {
            Festival festival = await _dbMock.Object.Festivals.FirstAsync(f => f.Id == festivalId);
            festival.FestivalName = "New Name";
            await _festivalService.UpdateFestival(festival);
            
            Assert.Equal("New Name",_festivalService.GetFestival(festivalId).FestivalName);
        }

        [Fact]
        public async void UpdateFestivalWithEarlierClosingHoursShouldThrowError()
        {
            Festival festival = await _dbMock.Object.Festivals.FirstAsync(f => f.Id == 1);
            festival.OpeningHours.StartDate = DateTime.Now;
            festival.OpeningHours.EndDate = DateTime.MinValue;
            await Assert.ThrowsAsync<EndDateEarlierThanStartDateException>(() =>
                _festivalService.UpdateFestival(festival));
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async void UpdateFestivalWithTooLongNameShouldThrowError(string name)
        {
            Festival festival = await _dbMock.Object.Festivals.FirstAsync(f => f.Id == 1);
            festival.FestivalName = name;
            await Assert.ThrowsAsync<InvalidDataException>(()=> _festivalService.UpdateFestival( festival ));
        }



    }
}