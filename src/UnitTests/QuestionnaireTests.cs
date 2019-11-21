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
    public class QuestionnaireTests
    {
        private readonly Mock<FestispecContext> _dbMock;
        private readonly IQuestionnaireService _questionnaireService;
        public QuestionnaireTests()
        {
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();

            _dbMock.Setup(x => x.Questionnaires).Returns(MockHelpers.CreateDbSetMock(ModelMocks.Questionnaires).Object);

            _dbMock.Setup(x => x.Questions).Returns(MockHelpers.CreateDbSetMock(ModelMocks.Questions).Object);

            _questionnaireService = new QuestionnaireService(_dbMock.Object);
        }

        [Theory]
        [InlineData("PinkPop")]
        [InlineData("Defqon")]
        public async void CreateQuestionnaire(string name)
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

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetQuestionnaire(int id)
        {
            var expectedQuestionnaire = _dbMock.Object.Questionnaires.FirstOrDefault(q => q.Id == id);
            var questionnaire = _questionnaireService.GetQuestionnaire(id);

            Assert.Equal(expectedQuestionnaire, questionnaire);
        }

        [Theory]
        [InlineData(100)]
        public void WrongIdShouldThrowError(int id)
        {
            Assert.Throws<EntityNotFoundException>(() => _questionnaireService.GetQuestionnaire(id)); 
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void RemovingQuestionnaire(int id)
        {
            var expectedRemovedQuestionnaire = _questionnaireService.GetQuestionnaire(id);

            await _questionnaireService.RemoveQuestionnaire(id);

            Assert.Throws<EntityNotFoundException>(() => _questionnaireService.GetQuestionnaire(id));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            //adding the questionnaire back into the mock for future tests
            _dbMock.Object.Questionnaires.Add(expectedRemovedQuestionnaire);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetQuestionFromQuestionnaire(int questionId)
        {
            var questionnaire = ModelMocks.Questionnaire3;
            var expectedQuestion = questionnaire.Questions.FirstOrDefault(q => q.Id == questionId);

            var question = _questionnaireService.GetQuestionFromQuestionnaire(questionnaire, questionId);

            Assert.Equal(expectedQuestion, question);
        }

        [Fact]
        public async void AddingStringQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.StringQuestion;

            var question = await _questionnaireService.AddQuestion(questionnaire, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire, question.Id));
            Assert.Equal(expectedQuestion.Contents, question.Contents);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void AddingMultipleChoiceQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.MultipleChoiceQuestion;

            var question = await _questionnaireService.AddQuestion(questionnaire, expectedQuestion);

            if (!(question is MultipleChoiceQuestion))
                throw new WrongQuestionTypeException();

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire, question.Id));

            Assert.Equal(expectedQuestion.Answer1, ((MultipleChoiceQuestion)question).Answer1);
            Assert.Equal(expectedQuestion.Answer2, ((MultipleChoiceQuestion)question).Answer2);
            Assert.Equal(expectedQuestion.Answer3, ((MultipleChoiceQuestion)question).Answer3);
            Assert.Equal(expectedQuestion.Answer4, ((MultipleChoiceQuestion)question).Answer4);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void AddingNumericQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.NumericQuestion;

            var question = await _questionnaireService.AddQuestion(questionnaire, expectedQuestion);

            if (!(question is NumericQuestion))
                throw new WrongQuestionTypeException();

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire, question.Id));

            Assert.Equal(expectedQuestion.Minimum, ((NumericQuestion)question).Minimum);
            Assert.Equal(expectedQuestion.Maximum, ((NumericQuestion)question).Maximum);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void UploadPictureQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.UploadPictureQuestion;

            var question = await _questionnaireService.AddQuestion(questionnaire, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire, question.Id));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void RemovingQuestion(int questionId)
        {
            var questionnaire = ModelMocks.Questionnaire4;

            _questionnaireService.RemoveQuestion(questionId);

            Assert.Throws<EntityNotFoundException>(() => _questionnaireService.GetQuestionFromQuestionnaire(questionnaire, questionId));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void RemovingQuestionWithReferenceShouldThrowError()
        {
            var questionnaire = ModelMocks.Questionnaire3;
            var question = ModelMocks.ReferencedQuestion;

            Assert.ThrowsAsync<QuestionHasReferencesException>(() => _questionnaireService.RemoveQuestion(question.Id)); 
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async void CopyQuestionnaire(int questionnaireId)
        {
            Questionnaire oldQuestionnaire = _questionnaireService.GetQuestionnaire(questionnaireId);

            Questionnaire newQuestionnaire = await _questionnaireService.CopyQuestionnaire(questionnaireId);

            Assert.Equal(oldQuestionnaire.Questions.Count(), newQuestionnaire.Questions.Count());

            foreach(Question question in newQuestionnaire.Questions.ToList())
            {
                Assert.True(oldQuestionnaire.Questions.Contains(((ReferenceQuestion)question).Question));
            }
        }
        [Fact]
        public void AddingDrawQuestion()
        {
            //is nog niet geimplementeerd
            Assert.True(false);
        }
    }
}
