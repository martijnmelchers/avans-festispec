﻿using Festispec.Models;
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
            StartDate = new DateTime(2020, 3, 10),
            EndDate = new DateTime(2020, 3, 12),
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(1, 0, 0)
        };

        public static Customer Customer1 = new Customer()
        {
            Id = 1,
            KvkNr = 12345678,
            CustomerName = "PinkPop",
            Address = Address,
            ContactDetails = ContactDetails,
            Festivals = new List<Festival>(),
            ContactPersons = new List<ContactPerson>()
        };

        public static Festival Festival = new Festival
        {
            Id = 1,
            FestivalName = "PinkPop",
            Description = "Placeholder for description",
            Address = Address,
            OpeningHours = OpeningHours,
            Customer = Customer1
        };
        
        public static Customer Customer2 = new Customer()
        {
            Id = 2,
            KvkNr = 12345678,
            CustomerName = "PinkPop2",
            Address = Address,
            ContactDetails = ContactDetails,
            Festivals = new List<Festival>
            {
                Festival
            },
            ContactPersons = new List<ContactPerson>()
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

        public List<Account> Accounts { get; set; }

        public List<Question> Questions = Questionnaire4.Questions.ToList();

        public List<Questionnaire> Questionnaires = new List<Questionnaire>()
        {
            Questionnaire1,
            Questionnaire2,
            Questionnaire3,
            Questionnaire4
        };
        
        public List<Customer> Customers = new List<Customer>
        {
            Customer1,
            Customer2
        };
        
        public List<ContactPerson> ContactPersons = new List<ContactPerson>();
        
        public List<Employee> Employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = new FullName {First = "Dit", Middle = "is", Last = "Een Test"},
                Iban = "NL01ABCD1234567890",
                Account = new Account()
                {
                    Id = 1,
                    Username = "JohnDoe",
                    Password = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    Role = Role.Employee
                },
                Address = new Address
                {
                    ZipCode = "1234AB",
                    StreetName = "Teststraat",
                    HouseNumber = 123,
                    Suffix = "a",
                    City = "Teststad",
                    Country = "Nederland"
                },
                ContactDetails = new ContactDetails
                {
                    PhoneNumber = "+316123456789",
                    EmailAddress = "test@testing.com"
                },
                PlannedEvents = new List<PlannedEvent>()
            },
            new Employee
            {
                Id = 2,
                Name = new FullName{First = "Test", Last = "Ing"},
                Iban = "NL02DBCA0987654321",
                Account = new Account
                {
                    Id = 2,
                    Username = "EricKuipers",
                    Password = BCrypt.Net.BCrypt.HashPassword("HeelLangWachtwoord"),
                    Role = Role.Inspector
                },
                Address = new Address
                {
                    ZipCode = "3734AB",
                    StreetName = "Hermelijnlaan",
                    HouseNumber = 12,
                    City = "Den Dolder",
                    Country = "Nederland"
                },
                ContactDetails = new ContactDetails
                {
                    PhoneNumber = "+316314253647",
                    EmailAddress = "tester@testing.com"
                },
                PlannedEvents = new List<PlannedEvent>
                {
                    new PlannedEvent
                    {
                        Id = 1,
                        StartTime = new DateTime(2019, 11, 27, 17, 00, 00),
                        EndTime = new DateTime(2019, 11, 28, 03, 00, 00),
                        EventTitle = "Inspectie bij Q-BASE"
                    }
                }
            }
        };

        public ModelMocks()
        {
            Accounts = Employees.Select(e => e.Account).ToList();
        }
    }
}
