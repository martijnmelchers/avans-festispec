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

            _dbMock.Setup(x => x.Questionnaires).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questionnaires).Object);

            _dbMock.Setup(x => x.Questions).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questions).Object);

            _dbMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);

            _questionnaireService = new QuestionnaireService(_dbMock.Object, new JsonSyncService<Questionnaire>(_dbMock.Object));
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

        [Fact]
        public async void WithoutFestivalShouldThrowError()
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => _questionnaireService.CreateQuestionnaire("test", null));
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

            var question = _questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, questionId);

            Assert.Equal(expectedQuestion, question);
        }

        [Fact]
        public async void AddingStringQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.StringQuestion;

            Question question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));
            Assert.Equal(expectedQuestion.Contents, question.Contents);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void AddingMultipleChoiceQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.MultipleChoiceQuestion;

            Question question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            if (!(question is MultipleChoiceQuestion))
                throw new WrongQuestionTypeException();

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void NoOptionsShouldThrowError()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            MultipleChoiceQuestion question = new MultipleChoiceQuestion("test", questionnaire);

            await Assert.ThrowsAsync<InvalidDataException>(() => _questionnaireService.AddQuestion(questionnaire.Id, question));
        }

        [Fact]
        public async void AddingNumericQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.NumericQuestion;

            Question question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            if (!(question is NumericQuestion))
                throw new WrongQuestionTypeException();

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));

            Assert.Equal(expectedQuestion.Minimum, ((NumericQuestion)question).Minimum);
            Assert.Equal(expectedQuestion.Maximum, ((NumericQuestion)question).Maximum);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void UploadPictureQuestion()
        {
            var questionnaire = ModelMocks.Questionnaire2;
            var expectedQuestion = ModelMocks.UploadPictureQuestion;

            Question question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void RemovingQuestion(int questionId)
        {
            await _questionnaireService.RemoveQuestion(questionId);

            Assert.Null(_dbMock.Object.Questions.FirstOrDefault(q => q.Id == questionId));

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
        //to be implemented
        /*[Fact]
        public void AddingDrawQuestion()
        {
            Assert.True(false);
        }*/
    }
}
