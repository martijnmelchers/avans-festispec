﻿using System;
using System.Data.Entity;
using Festispec.Models.EntityMapping;
using Festispec.DomainServices.Interfaces;
using Moq;
using Xunit;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.UnitTests.Helpers;
using Festispec.Models.Exception;
using System.Linq;
using System.Threading.Tasks;
using Festispec.Models.Answers;
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

            _dbMock.Setup(x => x.Questionnaires)
                .Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questionnaires).Object);
            _dbMock.Setup(x => x.Questions).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Questions).Object);
            _dbMock.Setup(x => x.Answers).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Answers).Object);
            _dbMock.Setup(x => x.PlannedInspections)
                .Returns(MockHelpers.CreateDbSetMock(new ModelMocks().PlannedInspections).Object);
            _dbMock.Setup(x => x.Festivals).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Festivals).Object);
            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Employees).Object);
            _dbMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);

            _questionnaireService =
                new QuestionnaireService(_dbMock.Object, new JsonSyncService<Questionnaire>(_dbMock.Object));
        }

        [Theory]
        [InlineData("PinkPop")]
        [InlineData("Defqon")]
        public async void CreateQuestionnaire(string name)
        {
            var festival = _dbMock.Object.Festivals.First(f => f.Id == 1);
            var questionnaire = await _questionnaireService.CreateQuestionnaire(name, festival.Id);

            Assert.Equal(festival, questionnaire.Festival);
            Assert.Equal(name, questionnaire.Name);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void WithoutFestivalShouldThrowError()
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => _questionnaireService.CreateQuestionnaire("test", 0));
        }

        [Theory]
        [InlineData("PinkPop Middag")]
        [InlineData("PinkPop Ochtend")]
        public async void SameNameShouldThrowError(string name)
        {
            await Assert.ThrowsAsync<EntityExistsException>(() =>
                _questionnaireService.CreateQuestionnaire(name, _dbMock.Object.Festivals.First(f => f.Id == 1).Id));
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async void InvalidDataShouldThrowError(string name)
        {
            await Assert.ThrowsAsync<InvalidDataException>(() =>
                _questionnaireService.CreateQuestionnaire(name, _dbMock.Object.Festivals.First(f => f.Id == 1).Id));
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
        public async void RemovingQuestionnaireWithAnswersShouldThrowError(int questionnaireId)
        {
            await Assert.ThrowsAsync<QuestionHasAnswersException>(() =>
                _questionnaireService.RemoveQuestionnaire(questionnaireId));
        }
        

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetQuestionFromQuestionnaire(int questionId)
        {
            var questionnaire = _dbMock.Object.Questionnaires.First(q => q.Id == 1);
            var expectedQuestion = questionnaire.Questions.FirstOrDefault(q => q.Id == questionId);

            var question = _questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, questionId);

            Assert.Equal(expectedQuestion, question);
        }

        [Fact]
        public async void AddingStringQuestion()
        {
            var questionnaire = _dbMock.Object.Questionnaires.First(q => q.Id == 1);
            var expectedQuestion = _dbMock.Object.Questions.OfType<StringQuestion>().First();

            var question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));
            Assert.Equal(expectedQuestion.Contents, question.Contents);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void AddingMultipleChoiceQuestion()
        {
            var questionnaire = _dbMock.Object.Questionnaires.First(q => q.Id == 1);
            var expectedQuestion = _dbMock.Object.Questions.OfType<MultipleChoiceQuestion>().First();

            var question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void NoOptionsShouldThrowError()
        {
            var questionnaire = _dbMock.Object.Questionnaires.First(q => q.Id == 1);
            var question = new MultipleChoiceQuestion("test", questionnaire);

            await Assert.ThrowsAsync<InvalidDataException>(() =>
                _questionnaireService.AddQuestion(questionnaire.Id, question));
        }

        [Fact]
        public async void AddingNumericQuestion()
        {
            var questionnaire = _dbMock.Object.Questionnaires.First(q => q.Id == 1);
            var expectedQuestion = _dbMock.Object.Questions.OfType<NumericQuestion>().First();

            var question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));

            Assert.Equal(expectedQuestion.Minimum, ((NumericQuestion) question).Minimum);
            Assert.Equal(expectedQuestion.Maximum, ((NumericQuestion) question).Maximum);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async void UploadPictureQuestion()
        {
            var questionnaire = _dbMock.Object.Questionnaires.First(q => q.Id == 1);
            var expectedQuestion = _dbMock.Object.Questions.OfType<UploadPictureQuestion>().First();

            var question = await _questionnaireService.AddQuestion(questionnaire.Id, expectedQuestion);

            Assert.NotNull(_questionnaireService.GetQuestionFromQuestionnaire(questionnaire.Id, question.Id));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void RemovingQuestion(int questionId)
        {
            var question = await _questionnaireService.GetQuestion(questionId);
            await _questionnaireService.RemoveQuestion(questionId);

            Assert.Null(_dbMock.Object.Questions.FirstOrDefault(q => q.Id == questionId));

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _dbMock.Object.Questions.Add(question);
        }
        [Theory]
        [InlineData(4)]
        public async void RemovingQuestionWithAnswersShouldThrowError(int questionId)
        {
            await Assert.ThrowsAsync<QuestionHasAnswersException>( ()=> _questionnaireService.RemoveQuestion(questionId));
        }
        [Theory]
        [InlineData(99)]
        public async void RemovingNonExistingQuestionShouldThrowError(int questionId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>( ()=> _questionnaireService.RemoveQuestion(questionId));
        }
        
        [Theory]
        [InlineData(5)]
        public async void RemovingQuestionLinkedToReferenceQuestionShouldThrowError(int questionId)
        {
            await Assert.ThrowsAsync<QuestionHasReferencesException>( ()=> _questionnaireService.RemoveQuestion(questionId));
        }
        
        [Fact]
        public void RemovingQuestionWithReferenceShouldThrowError()
        {
            var question = _dbMock.Object.Questions.OfType<ReferenceQuestion>().First();

            Assert.ThrowsAsync<QuestionHasReferencesException>(() => _questionnaireService.RemoveQuestion(question.Id));
        }

        [Theory]
        [InlineData(3)]
        public async void CopyQuestionnaire(int questionnaireId)
        {
            var oldQuestionnaire = _questionnaireService.GetQuestionnaire(questionnaireId);

            var newQuestionnaire =
                await _questionnaireService.CopyQuestionnaire(questionnaireId, "Copied questionnaire");

            Assert.Equal(oldQuestionnaire.Questions.Count, newQuestionnaire.Questions.Count);

            foreach (var question in newQuestionnaire.Questions.ToList())
                Assert.True(oldQuestionnaire.Questions.Contains(((ReferenceQuestion) question).Question));
        }

        [Theory]
        [InlineData(1)]
        public void GetQuestionsFromQuestionnaireShouldReturnListOfQuestions(int questionnaireId)
        {
            var expected = _dbMock.Object.Questions
                .Where(q => q.Questionnaire.Id == questionnaireId)
                .ToList();
            var actual = _questionnaireService.GetQuestionsFromQuestionnaire(questionnaireId);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(1)]
        public async Task GetGenericAnswerTAnswerShouldReturnStringAnswer(int answerId)
        {
            var expected = await _dbMock.Object.Answers.FirstAsync(a => a.Id == answerId);
            var actual = await _questionnaireService.GetAnswer<StringAnswer>(answerId);

            Assert.IsType<StringAnswer>(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2)]
        public async void GetPlannedInspectionsShouldReturnListOfPlannedInspections(int employeeId)
        {
            var expected = await _dbMock.Object.PlannedInspections
                .Where(p => p.Employee.Id == employeeId)
                .Where(p => p.StartTime.Date == DateTime.Now.Date)
                .ToListAsync();
            var actual = await _questionnaireService.GetPlannedInspections(employeeId);

            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(1)]
        public async void GetPlannedInspectionShouldReturnPlannedInspection(int plannedInspectionId)
        {
            var expected = await _dbMock.Object.PlannedInspections.FirstAsync(p => p.Id == plannedInspectionId);
            var actual = await _questionnaireService.GetPlannedInspection(plannedInspectionId);
            
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        public async void CreateAnswerShouldAddAnswer(int answerId)
        {
            var expected = await _dbMock.Object.Answers.FirstAsync(a => a.Id == answerId);
            var actual = await _questionnaireService.CreateAnswer(expected);
            
            Assert.Equal(expected,actual);
        }
        
        [Fact]
        public async void CopyQuestionnaireShouldReturnNewQuestionnaire()
        {
            var old = await _dbMock.Object.Questionnaires.FirstAsync(q => q.Id == 1);
            var newQuestionnaire = await _questionnaireService.CopyQuestionnaire(old.Id, "new Text");
            
            Assert.Equal(old.Questions.Count, newQuestionnaire.Questions.Count);
            Assert.Equal("new Text", newQuestionnaire.Name);

        }

        [Fact]
        public async void InvalidQuestionnaireIdThrowsError()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _questionnaireService.AddQuestion(481284, new DrawQuestion()));
        }
        
        [Fact]
        public async void InvalidAnswerThrowsError()
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => _questionnaireService.CreateAnswer(new FileAnswer()));
        }
        
        [Fact]
        public async void InvalidEmployeeIdThrowsException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _questionnaireService.GetPlannedInspections(-020));
        }
        
        [Fact]
        public async void InvalidPlannedInspectionIdThrowsException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _questionnaireService.GetPlannedInspection(-10));
        }
        
        [Fact]
        public void InvalidQuestionnaireIdGetQuestionThrowsError()
        {
            Assert.Throws<EntityNotFoundException>(() => _questionnaireService.GetQuestionFromQuestionnaire(-2, -2));
        }
        
        [Fact]
        public void InvalidQuestionIdGetQuestionThrowsError()
        {
            Assert.Throws<EntityNotFoundException>(() => _questionnaireService.GetQuestionFromQuestionnaire(1, -2));
        }

        
        
        

    }
}