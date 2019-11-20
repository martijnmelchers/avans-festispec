using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.DomainServices.Helpers
{
    public class ModelMocks
    {
        public static Address Address = new Address()
        {
            ZipCode = "1013 GM",
            StreetName = "Amsterweg",
            HouseNumber = 23,
            City = "Utrecht",
            Country = "Nederland"
        };

        public static ContactDetails ContactDetails = new ContactDetails()
        {
            PhoneNumber = "31695734859",
            EmailAddress = "psmulde@pinkpop.nl"
        };

        public static OpeningHours OpeningHours = new OpeningHours()
        {
            StartTime = new DateTime(2020, 3, 10, 20, 0, 0),
            EndTime = new DateTime(2020, 3, 12, 1, 0, 0)
        };

        public static Customer Customer = new Customer()
        {
            KvkNr = 12345678,
            CustomerName = "PinkPop",
            Address = Address,
            ContactDetails = ContactDetails
        };

        public static Festival Festival = new Festival()
        {
            Id = 1,
            FestivalName = "PinkPop",
            Description = "Placeholder for description",
            Address = Address,
            OpeningHours = OpeningHours,
            Customer = Customer
        };

        public static List<Account> Accounts = new List<Account>()
        {
            new Account()
            {
                Username = "JohnDoe",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Role = Role.Employee
            },
            new Account()
            {
                Username = "EricKuipers",
                Password = BCrypt.Net.BCrypt.HashPassword("HeelLangWachtwoord"),
                Role = Role.Inspector
            }
        };


        public static List<Questionnaire> Questionnaires = new List<Questionnaire>()
        {
            new Questionnaire("PinkPop Ochtend", Festival){Id= 1 },
            new Questionnaire("PinkPop Middag", Festival){Id = 2}
        };

        public static List<Question> Questions = new List<Question>()
        {
            new NumericQuestion(){
                Category = new QuestionCategory(){ Id = 1, CategoryName = "Kanker" },
                Questionnaire = Questionnaires[0],
                Contents = "Wat is de sfeer?",
                Answers = NumericAnswers
            },

            new NumericQuestion(){
                Category = new QuestionCategory(){ Id = 2, CategoryName = "Kanker" },
                Questionnaire = Questionnaires[0],
                Contents = "Cijfer toilet?",
                Answers = NumericAnswers
            },

            new RatingQuestion(){
                Category = new QuestionCategory(){ Id = 3, CategoryName = "Kanker" },
                Questionnaire = Questionnaires[0],
                Contents = "Hoeveel sterren toilet?",
                Answers = NumericAnswers

            },
        };


        public static List<NumericAnswer> NumericAnswers = new List<NumericAnswer>()
        {
            new NumericAnswer(){
                Id = 1,
                IntAnswer = 6
            },

            new NumericAnswer(){
                Id = 2,
                IntAnswer = 6

            },

            new NumericAnswer(){
                Id = 3,
                IntAnswer = 3
            },

            new NumericAnswer(){
                Id = 4,
                IntAnswer = 1,
            },
            new NumericAnswer(){
                Id = 5,
                IntAnswer = 1,
            },
            new NumericAnswer(){
                Id = 6,
                IntAnswer = 8,
            },
            new NumericAnswer(){
                Id = 7,
                IntAnswer = 8,
            }
        };
    }
}
