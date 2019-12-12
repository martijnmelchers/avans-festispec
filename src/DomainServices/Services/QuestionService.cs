using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.EntityMapping;
using Festispec.Models.Questions;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    class QuestionService: IQuestionService
    {
        private readonly FestispecContext _db;

        public QuestionService(FestispecContext db)
        {
            _db = db;
        }

        public async Task<List<Answer>> GetAnswers(Question question)
        { 
            List<Answer> questionAnswers = new List<Answer>();
            return await _db.Answers.Where(x => x.Question == question).ToListAsync();
        }

        public async Task<Questionnaire> GetQuestionaire(int id)
        {
            return await _db.Questionnaires.Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
