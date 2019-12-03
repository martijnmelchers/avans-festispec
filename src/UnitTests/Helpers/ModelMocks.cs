using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Festispec.UnitTests.Helpers
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

        public static Questionnaire Questionnaire1 = new Questionnaire("PinkPop Ochtend", Festival)
        {
            Id = 1,
        };

        public static Questionnaire Questionnaire2 = new Questionnaire("PinkPop Middag", Festival)
        {
            Id = 2
        };

        public static RatingQuestion RatingQuestion = new RatingQuestion("Hoe druk is het bij de toiletten", Questionnaire1, "rustig", "druk");

        public static NumericQuestion NumericQuestion = new NumericQuestion("Hoeveel zitplaatsen zijn er bij de foodtrucks", Questionnaire1, 0, 1000);

        public static UploadPictureQuestion UploadPictureQuestion = new UploadPictureQuestion("Maak een foto van de toiletten", Questionnaire1);

        public static StringQuestion StringQuestion = new StringQuestion("Geef een indruk van de sfeer impressie bij de eetgelegenheden", Questionnaire1);

        public static MultipleChoiceQuestion MultipleChoiceQuestion = new MultipleChoiceQuestion("Wat beschrijft het beste de sfeer bij het publiek na de shows bij de main stage?", Questionnaire1)
        {
           Options = "Option1,Option2,Option3,Option4",
           OptionCollection = new ObservableCollection<StringObject>()
           {
              new StringObject("Option1")
           }
        };

        public static StringQuestion ReferencedQuestion = new StringQuestion("test1", Questionnaire3)
        {
            Id = 1
        };

        private static List<Question> QuestionsWithReference = new List<Question>()
        {
            ReferencedQuestion,
            new ReferenceQuestion("test2", Questionnaire3, ReferencedQuestion)
            {
                Id = 2
            }
        };

        public static Questionnaire Questionnaire3 = new Questionnaire("PinkPop MaandagAvond", Festival)
        {
            Id = 3,
            Questions = QuestionsWithReference
        };

        public static Questionnaire Questionnaire4 = new Questionnaire("PinkPop DinsdagOchtend", Festival)
        {
            Id = 4,
            Questions = new List<Question>()
            {
                new StringQuestion("Beschrijf de sfeer bij het evenement", Questionnaire4)
                {
                    Id = 1
                },
                new StringQuestion("Beschrijf de sfeer in de rij", Questionnaire4)
                {
                    Id = 2
                }
            }
        };

        public List<Account> Accounts = new List<Account>()
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

        public List<Question> Questions = Questionnaire4.Questions.ToList();

        public List<Questionnaire> Questionnaires = new List<Questionnaire>()
        {
            Questionnaire1,
            Questionnaire2,
            Questionnaire3,
            Questionnaire4
        };
    }
}
