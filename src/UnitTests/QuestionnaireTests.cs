using Festispec.Models.EntityMapping;
using Festispec.DomainServices.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

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
        }
        [Fact]
        public void AddingQuestionnaire()
        {
            Assert.True(false);
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
