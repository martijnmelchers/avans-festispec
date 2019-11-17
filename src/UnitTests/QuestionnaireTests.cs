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

namespace Festispec.UnitTests
{
    public class QuestionnaireTests
    {
        private readonly Mock<FestispecContext> _dbMock;
        private readonly IQuestionnaireService _questionnaireService;
        public QuestionnaireTests()
        {
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();

            _dbMock.Setup(x => x.Questionnaires).Returns(MockHelpers.CreateDbSetMock(ModelMocks.Questionnaires).Object);

            _questionnaireService = new QuestionnaireService(_dbMock.Object);
        }

        [Theory]
        [InlineData("PinkPop")]
        [InlineData("Defqon")]
        public async void CanCreateQuestionnaire(string name)
        {
            var festival = ModelMocks.Festival;
            var questionnaire = await _questionnaireService.CreateQuestionnaire(name, festival);

            Assert.Equal(festival, questionnaire.Festival);
            Assert.Equal(name, questionnaire.Name);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("PinkPop Middag")]
        [InlineData("PinkPop Ochtend")]
        public async void SameNameShouldThrowError(string name)
        {
            await Assert.ThrowsAsync<EntityExistsException>(() => _questionnaireService.CreateQuestionnaire(name, ModelMocks.Festival));
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async void InvalidDataShouldThrowError(string name)
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => _questionnaireService.CreateQuestionnaire(name, ModelMocks.Festival));
        }

        [Fact]
        public void RemovingQuestionnaire()
        {
            Assert.True(false);
        }
        [Fact]
        public void AddingStringQuestion()
        {
            Assert.True(false);
        }
        [Fact]
        public void AddingMultipleChoiceQuestion()
        {
            Assert.True(false);
        }
        [Fact]
        public void AddingDrawQuestion()
        {
            Assert.True(false);
        }
        [Fact]
        public void AddingNumberQuestion()
        {
            Assert.True(false);
        }
        [Fact]
        public void AddingBooleanQuestion()
        {
            Assert.True(false);
        }
        [Fact]
        public void AddingImageQuestion()
        {
            Assert.True(false);
        }
        [Fact]
        public void RemovingQuestion()
        {
            Assert.True(false);
        }
    }
}
