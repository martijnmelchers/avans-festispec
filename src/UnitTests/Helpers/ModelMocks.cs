using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using Festispec.DomainServices.Helpers;

namespace Festispec.UnitTests.Helpers
{
    public class ModelMocks
    {
        public List<Festival> Festivals { get; }

        public List<Customer> Customers { get; }

        public List<Employee> Employees { get; }

        public List<Account> Accounts { get; }
        public List<Certificate> Certificates { get; }

        public List<Questionnaire> Questionnaires { get; }

        public List<PlannedInspection> PlannedInspections { get; }

        public List<Availability> Sickness { get; }

        public List<PlannedEvent> PlannedEvents { get; }

        public List<Availability> Availabilities { get; }

        public List<Question> Questions { get; }

        public List<Answer> Answers { get; }

        public List<Address> Addresses { get; }

        public ModelMocks()
        {
            Customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    KvkNr = 12345678,
                    CustomerName = "PinkPop",
                    Address = new Address
                    {
                        ZipCode = "1013 GM",
                        StreetName = "Amsterweg",
                        HouseNumber = 23,
                        City = "Utrecht",
                        Country = "Nederland"
                    },
                    ContactDetails = new ContactDetails
                    {
                        PhoneNumber = "31695734859",
                        EmailAddress = "psmulde@pinkpop.nl"
                    }
                },
                new Customer
                {
                    Id = 2,
                    KvkNr = 12345678,
                    CustomerName = "ThunderDome",
                    Address = new Address
                    {
                        ZipCode = "1013 GM",
                        StreetName = "Amsterweg",
                        HouseNumber = 23,
                        City = "Utrecht",
                        Country = "Nederland"
                    },
                    ContactDetails = new ContactDetails
                    {
                        PhoneNumber = "3123456789",
                        EmailAddress = "info@thunderdome.nl"
                    }
                },
                new Customer // deze customer GEEN festivals geven! hij wordt gebruikt voor het verwijderen te testen.
                {
                    Id = 3,
                    KvkNr = 12345678,
                    CustomerName = "Test Voor Verwijderen Customers",
                    Address = new Address
                    {
                        ZipCode = "1234 AB",
                        StreetName = "AAAAAAAAAweg",
                        HouseNumber = 1,
                        City = "Amsterdam",
                        Country = "Nederland"
                    },
                    ContactDetails = new ContactDetails
                    {
                        PhoneNumber = "31123456789",
                        EmailAddress = "info@test.nl"
                    }
                }
            };

            Festivals = new List<Festival>
            {
                new Festival
                {
                    Id = 1,
                    FestivalName = "PinkPop",
                    Description = "Placeholder for description",
                    Address = new Address
                    {
                        ZipCode = "1013 GM",
                        StreetName = "Amsterweg",
                        HouseNumber = 23,
                        City = "Utrecht",
                        Country = "Nederland"
                    },
                    OpeningHours = new OpeningHours
                    {
                        StartDate = new DateTime(2020, 3, 10),
                        EndDate = new DateTime(2020, 3, 12),
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(1, 0, 0)
                    },
                    Customer = Customers.First(c => c.Id == 1)
                },
                new Festival
                {
                    Id = 2,
                    FestivalName = "ThunderDome",
                    Description = "Op 26 oktober 2019 keert Thunderdome terug naar de Jaarbeurs in Utrecht. " +
                                  "In 2017 maakte het legendarische Hardcore concept een comeback na vijf jaar afwezig te zijn geweest.",

                    Customer = Customers.First(c => c.Id == 2),
                    Address = new Address
                    {
                        ZipCode = "3245JK",
                        StreetName = "Kadestraat",
                        City = "Biddinghuizen",
                        Country = "Nederland"
                    },
                    OpeningHours = new OpeningHours()
                    {
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(2, 0, 0),
                        StartDate = new DateTime(2019, 12, 10),
                        EndDate = new DateTime(2019, 12, 14)
                    }
                },
                new Festival
                {
                    Id = 3,
                    FestivalName = "Intents",
                    Description = "Op 26 oktober 2019 keert Intents terug naar Brabant. " +
                                  "een legendarische Hardcore/Hardstyle concept een comeback na een jaar afwezig te zijn geweest.",

                    Customer = Customers.First(c => c.Id == 2),
                    Address = new Address
                    {
                        ZipCode = "5731JR",
                        StreetName = "Vaanakker",
                        City = "Mierlo",
                        Country = "Nederland"
                    },
                    OpeningHours = new OpeningHours()
                    {
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(2, 0, 0),
                        StartDate = new DateTime(2019, 12, 10),
                        EndDate = new DateTime(2019, 12, 14)
                    }
                }
            };

            Employees = new List<Employee>
            {
                new Employee
                {
                    Id = 1,
                    Name = new FullName { First = "Dit", Middle = "is", Last = "Een Test" },
                    Iban = "NL01ABCD1234567890",
                    Account = new Account
                    {
                        Id = 1,
                        Username = "JohnDoe",
                        Password = "$2y$12$jKRUmk7DrgcdTGc5YIoW8uRZWp98aa6b3/MEweMe82CKFKmI2Xerm", // Password123
                        Role = Role.Employee
                    },
                    Address = new Address
                    {
                        Id = 1,
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
                    Certificates = new List<Certificate>
                    {
                        new Certificate
                        {
                            Id = 1,
                            CertificateTitle = "Festispec Training Certificate",
                            CertificationDate = new DateTime(2019, 11, 25),
                            ExpirationDate = new DateTime(2025, 11, 25)
                        }
                    }
                },
                new Employee
                {
                    Id = 2,
                    Name = new FullName { First = "Test", Last = "Ing" },
                    Iban = "NL02DBCA0987654321",
                    Account = new Account
                    {
                        Id = 2,
                        Username = "EricKuipers",
                        Password = "$2y$12$fAj/kSCqzIE5BmSYxn9hmOVo.CSAMUrGcTl6SLV6S5Bx88QD3DbGe", // HeelLangWachtwoord
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
                    Certificates = new List<Certificate>
                    {
                        new Certificate
                        {
                            Id = 2,
                            CertificateTitle = "Festispec Training Certificate",
                            CertificationDate = new DateTime(2020, 11, 25),
                            ExpirationDate = new DateTime(2026, 11, 25)
                        }
                    }
                },
                new Employee // deze employeet GEEN PlannedEvents geven!
                {
                    Id = 3,
                    Name = new FullName { First = "Employee Remove", Last = "Test" },
                    Iban = "NL3457ABNA234578978923457892347892",
                    Account = new Account
                    {
                        Id = 3,
                        Username = "Henk2",
                        Password = "$2y$12$YNZ3G6P9WX.II7.05PpohOQ0PMyaORCPYmBK5DS9wvEvSEiz5UTNy", // HeelLangWachtwoord
                        Role = Role.Employee,
                        IsNonActive = DateTime.Now.Subtract(new TimeSpan(24,0,0))
                    },
                    Address = new Address
                    {
                        ZipCode = "1234AB",
                        StreetName = "YuriLaan",
                        HouseNumber = 12,
                        City = "Den Dolder",
                        Country = "Nederland"
                    },
                    ContactDetails = new ContactDetails
                    {
                        PhoneNumber = "+3112345678",
                        EmailAddress = "tester@testing.com"
                    },
                    Certificates = new List<Certificate>()
                }
            };

            Questionnaires = new List<Questionnaire>
            {
                new Questionnaire
                {
                    Id = 1,
                    Name = "PinkPop Ochtend",
                    Festival = Festivals.First(f => f.Id == 1)
                },
                new Questionnaire
                {
                    Id = 2,
                    Name = "PinkPop Middag",
                    Festival = Festivals.First(f => f.Id == 1)
                },
                new Questionnaire
                {
                    Id = 3,
                    Name = "ThunderDome DinsdagOchtend",
                    Festival = Festivals.First(f => f.Id == 2)
                }
            };

            Questions = new List<Question>
            {
                new RatingQuestion
                {
                    Id = 1,
                    Contents = "Hoe druk is het bij de toiletten?",
                    Questionnaire = Questionnaires.First(q => q.Id == 1),
                    LowRatingDescription = "rustig",
                    HighRatingDescription = "druk"
                },
                new NumericQuestion
                {
                    Id = 2,
                    Contents = "Hoeveel zitplaatsen zijn er bij de foodtrucks",
                    Questionnaire = Questionnaires.First(q => q.Id == 1),
                    Minimum = 0,
                    Maximum = 1000
                },
                new UploadPictureQuestion
                {
                    Id = 3,
                    Contents = "Maak een foto van de toiletten",
                    Questionnaire = Questionnaires.First(q => q.Id == 1),
                },
                new StringQuestion
                {
                    Id = 4,
                    Contents = "Geef een indruk van de sfeer impressie bij de eetgelegenheden",
                    Questionnaire = Questionnaires.First(q => q.Id == 1),
                },
                new MultipleChoiceQuestion
                {
                    Id = 5,
                    Contents = "Wat beschrijft het beste de sfeer bij het publiek na de shows bij de main stage?",
                    Questionnaire = Questionnaires.First(q => q.Id == 1),
                    Options = "Option1,Option2,Option3,Option4",
                }
            };
            // reference questions have to be declared separately.
            Questions.Add(new ReferenceQuestion
            {
                Id = 6,
                Contents = "Wat beschrijft het beste de sfeer bij het publiek na de shows bij de main stage?",
                Questionnaire = Questionnaires.First(q => q.Id == 2),
                Question = Questions.First(q => q.Id == 5)
            });

            PlannedInspections = new List<PlannedInspection>
            {
                new PlannedInspection
                {
                    Id = 1,
                    StartTime = new DateTime(2020, 3, 4, 12, 30, 0),
                    EndTime = new DateTime(2020, 3, 4, 17, 0, 0),
                    EventTitle = "PinkPop Ochtend",
                    Employee = Employees.First(e => e.Id == 2),
                    Questionnaire = Questionnaires.First(q => q.Id == 1),
                    Festival = Festivals.First(f => f.Id == 1)
                },
                new PlannedInspection
                {
                    Id = 2,
                    StartTime = new DateTime(2019, 12, 10, 16, 0, 0),
                    EndTime = new DateTime(2019, 12, 10, 20, 30, 0),
                    EventTitle = "ThunderDome",
                    Employee = Employees.First(e => e.Id == 2),
                    Questionnaire = Questionnaires.First(f => f.Id == 3),
                    Festival = Festivals.First(f => f.Id == 2)
                },
                new PlannedInspection
                {
                    Id = 3,
                    StartTime = QueryHelpers.TruncateTime(DateTime.Now),
                    EndTime = new DateTime(2019, 12, 10, 20, 30, 0),
                    EventTitle = "ThunderDome Test",
                    Employee = Employees.First(e => e.Id == 2),
                    Questionnaire = Questionnaires.First(f => f.Id == 3),
                    Festival = Festivals.First(f => f.Id == 2)
                }
            };

            Answers = new List<Answer>
            {
                new StringAnswer
                {
                    Id = 1,
                    Question = Questions.First(q => q.Id == 4),
                    PlannedInspection = PlannedInspections.First(pi => pi.Id == 1),
                    AnswerContents = "De sfeer was goed."
                }
            };

            Sickness = new List<Availability>
            {
                new Availability
                {
                    Id = 4,
                    Employee = Employees.First(e => e.Id == 1),
                    IsAvailable = false,
                    Reason = "Ik heb griep",
                    EventTitle = "Afwezig wegens ziekte",
                    StartTime = new DateTime(2019, 12, 28)
                },
            };

            PlannedEvents = new List<PlannedEvent>()
                .Concat(PlannedInspections)
                .Concat(Sickness)
                .Concat(new List<PlannedEvent>
                {
                    new Availability
                    {
                        Id = 5,
                        Employee = Employees.First(e => e.Id == 2),
                        IsAvailable = false,
                        Reason = "Ik heb een verjaardag",
                        EventTitle = "Niet beschikbaar",
                        StartTime = new DateTime(2019, 12, 28, 10, 0, 0),
                        EndTime = new DateTime(2019, 12, 28, 16, 0, 0)
                    }
                })
                .ToList();

            // glue it all together.
            Customers.ForEach(c => c.Festivals = Festivals.FindAll(f => f.Customer.Id == c.Id));
            Festivals.ForEach(f =>
            {
                f.PlannedInspections = PlannedInspections.FindAll(pi => pi.Festival.Id == f.Id);
                f.Questionnaires = Questionnaires.FindAll(q => q.Festival.Id == f.Id);
            });
            Employees.ForEach(e => e.PlannedEvents = PlannedEvents.FindAll(pe => pe.Employee.Id == e.Id));
            Accounts = Employees.Select(e => e.Account).ToList();
            Certificates = Employees.SelectMany(e => e.Certificates).ToList();
            Questionnaires.ForEach(qn =>
            {
                qn.PlannedInspections = PlannedInspections.FindAll(pi => pi.Questionnaire.Id == qn.Id);
                qn.Questions = Questions.FindAll(q => q.Questionnaire.Id == qn.Id);
            });
            Questions.ForEach(q => q.Answers = Answers.FindAll(a => a.Question.Id == q.Id));
            PlannedInspections.ForEach(pi => pi.Answers = Answers.FindAll(a => a.PlannedInspection.Id == pi.Id));
            Availabilities = PlannedEvents.OfType<Availability>().ToList();
            Addresses = new List<Address>()
                .Concat(Customers.Select(c => c.Address))
                .Concat(Employees.Select(e => e.Address))
                .ToList();
        }
    }
}