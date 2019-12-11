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

        public static Customer CustomerPinkPop = new Customer()
        {
            KvkNr = 12345678,
            CustomerName = "PinkPop",
            Address = Address,
            ContactDetails = ContactDetails
        };

        public static Festival FestivalPinkPop = new Festival()
        {
            Id = 1,
            FestivalName = "PinkPop",
            Description = "Placeholder for description",
            Address = Address,
            OpeningHours = OpeningHours,
            Customer = CustomerPinkPop
        };

        public static Questionnaire Questionnaire1 = new Questionnaire("PinkPop Ochtend", FestivalPinkPop)
        {
            Id = 1,
        };

        public static Questionnaire Questionnaire2 = new Questionnaire("PinkPop Middag", FestivalPinkPop)
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

        public static Questionnaire Questionnaire3 = new Questionnaire("PinkPop MaandagAvond", FestivalPinkPop)
        {
            Id = 3,
            Questions = QuestionsWithReference
        };

        public static Questionnaire Questionnaire4 = new Questionnaire("PinkPop DinsdagOchtend", FestivalPinkPop)
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

        public static Account InspectorAccount = new Account()
        {
            Username = "EricKuipers",
            Password = BCrypt.Net.BCrypt.HashPassword("HeelLangWachtwoord"),
            Role = Role.Inspector
        };

        public List<Account> Accounts = new List<Account>()
        {
            new Account()
            {
                Username = "JohnDoe",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Role = Role.Employee
            },
            InspectorAccount
        };

        public List<Question> Questions = Questionnaire4.Questions.ToList();

        public List<Questionnaire> Questionnaires = new List<Questionnaire>()
        {
            Questionnaire1,
            Questionnaire2,
            Questionnaire3,
            Questionnaire4
        };

        public static Employee Employee = new Employee()
        {            
            Iban = "NL91ABNA0417164300",

            Account = InspectorAccount
        };

        public static StringAnswer StringAnswer = new StringAnswer()
        {
            Question = StringQuestion,
        };

        public static Customer CustomerThunderDome = new Customer()
        {
            KvkNr = 12345678,
            CustomerName = "ThunderDome"
        };

        public static Festival festivalThunderDome = new Festival()
        {
            FestivalName = "ThunderDome",

            Description = "Op 26 oktober 2019 keert Thunderdome terug naar de Jaarbeurs in Utrecht. " +
                        "In 2017 maakte het legendarische Hardcore concept een comeback na vijf jaar afwezig te zijn geweest.",

            Customer = CustomerThunderDome
        };

        public static PlannedInspection PlannedInspection1 = new PlannedInspection()
        {
            Id = 1,

            StartTime = new DateTime(2020, 3, 4, 12, 30, 0),

            EndTime = new DateTime(2020, 3, 4, 17, 0, 0),

            EventTitle = "Pinkpop",

            Employee = Employee,

            Questionnaire = Questionnaire4,

            Festival = FestivalPinkPop,            
        };

        public static PlannedInspection PlannedInspection2 = new PlannedInspection()
        {
            Id = 2,

            StartTime = new DateTime(2019, 12, 10, 16, 0, 0),

            EndTime = new DateTime(2019, 12, 10, 20, 30, 0),

            EventTitle = "ThunderDome",

            Employee = Employee,

            Questionnaire = Questionnaire4,

            Festival = festivalThunderDome,

            Answers = new List<Answer>()
            {
                new StringAnswer()
                {
                    PlannedInspection = PlannedInspection2,

                    Question = new StringQuestion("Geef een indruk van de sfeer impressie bij de eetgelegenheden", Questionnaire4)
                }                      
            }
        };

        public List<PlannedInspection> plannedInspections = new List<PlannedInspection>()
        {
            PlannedInspection1,
            PlannedInspection2
        };
    }
}
