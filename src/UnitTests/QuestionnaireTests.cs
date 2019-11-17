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
            _questionnaireService = new QuestionnaireService(_dbMock.Object);
        }
        [Theory]
        [InlineData("nigger")]
        
        public async void CanCreateQuestionnaire(string name)
        {
            var festival = ModelMocks.Festival();
            var questionnaire = await _questionnaireService.CreateQuestionnaire(name, festival);

            Assert.Equal(festival, questionnaire.Festival);


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
