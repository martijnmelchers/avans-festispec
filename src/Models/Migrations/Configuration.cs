using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using Festispec.Models.Answers;
using Festispec.Models.EntityMapping;
using Festispec.Models.Questions;
using Festispec.Models.Reports;

namespace Festispec.Models.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<FestispecContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FestispecContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.



            //  context.SaveChanges();

            try
            {
                var employee = CreateEmployee(context);

                var customer = new Customer
                {
                    Id = 1,
                    CustomerName = "Q-DANCE",
                    KvkNr = 34212891,
                    Address = new Address
                    {
                        StreetName = "Isolatorweg",
                        HouseNumber = 36,
                        ZipCode = "1014AS",
                        City = "Amsterdam",
                        Country = "Nederland"
                    },
                    ContactDetails = new ContactDetails
                    {
                        EmailAddress = "info@q-dance.com",
                        PhoneNumber = "+31204877300"
                    }
                };

                context.Customers.AddOrUpdate(customer);

                var contactPerson = new ContactPerson
                {
                    Id = 1,
                    Customer = customer,
                    Name = new FullName
                    {
                        First = "Niels",
                        Last = "Kijf"
                    },
                    ContactDetails = new ContactDetails
                    {
                        // fake news
                        EmailAddress = "nielskijf@q-dance.com"
                    },
                    Role = "MA"
                };

                context.ContactPersons.AddOrUpdate(contactPerson);

                var note = new ContactPersonNote
                {
                    Id = 1,
                    ContactPerson = contactPerson,
                    Note = "Contact opgenomen met Niels over een inspectie. Voorstel volgt."
                };

                context.ContactPersonNotes.AddOrUpdate(note);

                var festival = new Festival
                {
                    Id = 1,
                    FestivalName = "Q-BASE",
                    Customer = customer,
                    Description = "Nachtfestival over de grens",
                    Address = new Address
                    {
                        Country = "Duitsland",
                        StreetName = "Flughafen-Ring",
                        HouseNumber = 16,
                        City = "Weeze",
                        ZipCode = "NW47652"
                    },
                    OpeningHours = new OpeningHours
                    {
                        StartTime = new DateTime(2020, 9, 5, 18, 0, 0),
                        EndTime = new DateTime(2020, 9, 6, 8, 0, 0)
                    }
                };

                context.Festivals.AddOrUpdate(festival);

                customer.Festivals = new List<Festival>
            {
                festival
            };

                context.Customers.AddOrUpdate(customer);

                var questionnaire = new Questionnaire
                {
                    Id = 1,
                    Name = "Tester",
                    Festival = festival
                };

                var plannedInspection = new PlannedInspection
                {
                    Id = 2,
                    Employee = employee,
                    Festival = festival,
                    EventTitle = "Inspection " + festival.FestivalName,
                    StartTime = new DateTime(2020, 7, 28, 20, 00, 00),
                    EndTime = new DateTime(2020, 7, 29, 5, 00, 00),
                    Questionnaire = questionnaire
                };

                context.PlannedInspections.AddOrUpdate(plannedInspection);

                var questionCategory = new QuestionCategory
                {
                    Id = 1,
                    CategoryName = "Vragen over veiligheid"
                };

                context.QuestionCategories.AddOrUpdate(questionCategory);

                var drawQuestion = new DrawQuestion
                {
                    Id = 1,
                    Category = questionCategory,
                    PicturePath = "/drawings/map_defqon.png",
                    Questionnaire = questionnaire,
                    Contents = "Wat is de kortste looproute van de mainstage naar de nooduitgang?",
                    Answers = new List<FileAnswer>
                {
                    new FileAnswer
                    {
                        Id = 1,
                        UploadedFilePath = "/uploads/drawing_map_defqon_inspector_1.png",
                        PlannedInspection = plannedInspection
                    }
                }
                };

                var multipleChoiceQuestion = new MultipleChoiceQuestion
                {
                    Id = 2,
                    Category = questionCategory,
                    Contents = "Zijn er evacuatieplannen zichtbaar opgesteld?",
                    Options = "Ja,Nee",
                    OptionCollection = new ObservableCollection<StringObject>()
                    {
                        new StringObject("Option1")
                    },
                    Questionnaire = questionnaire,
                    Answers = new List<MultipleChoiceAnswer>
                {
                    new MultipleChoiceAnswer
                    {
                        Id = 2,
                        MultipleChoiceAnswerKey = 0,
                        PlannedInspection = plannedInspection,
                        Attachments = new List<Attachment>
                        {
                            new Attachment
                            {
                                Id = 1,
                                FilePath = "/attachments/1.png"
                            }
                        }
                    }
                }
                };

                var numericQuestion = new NumericQuestion
                {
                    Id = 3,
                    Category = questionCategory,
                    Contents = "Hoeveel EHBO-posten zijn er aanwezig?",
                    Minimum = 0,
                    Maximum = 99,
                    Questionnaire = questionnaire,
                    Answers = new List<NumericAnswer>
                {
                    new NumericAnswer
                    {
                        Id = 3,
                        IntAnswer = 10,
                        PlannedInspection = plannedInspection
                    }
                }
                };

                var ratingQuestion = new RatingQuestion
                {
                    Id = 4,
                    Category = questionCategory,
                    Contents = "Op een schaal van 1 tot 5, is de beveiliging voldoende aanwezig op het terrein?",
                    HighRatingDescription = "Er is veel beveiliging",
                    LowRatingDescription = "Er is amper beveiliging",
                    Questionnaire = questionnaire,
                    Answers = new List<NumericAnswer>
                {
                    new NumericAnswer
                    {
                        Id = 4,
                        IntAnswer = 3,
                        PlannedInspection = plannedInspection
                    }
                }
                };

                var stringQuestion = new StringQuestion
                {
                    Id = 5,
                    Category = questionCategory,
                    Contents = "Geef een korte samenvatting van het vluchtplan.",
                    IsMultiline = true,
                    Questionnaire = questionnaire,
                    Answers = new List<StringAnswer>
                {
                    new StringAnswer
                    {
                        Id = 5,
                        AnswerContents = "In geval van een calamiteit is voor de bezoekers duidelijk te zien dat er vanaf de mainstage al vier vluchtroutes bestaan.",
                        PlannedInspection = plannedInspection
                    }
                }
                };

                var pictureQuestion = new UploadPictureQuestion
                {
                    Id = 6,
                    Category = questionCategory,
                    Contents = "Plaats een foto van de vluchtroutes op het calamiteitenplan.",
                    Questionnaire = questionnaire,
                    Answers = new List<FileAnswer>
                {
                    new FileAnswer
                    {
                        Id = 6,
                        UploadedFilePath = "/uploads/inspection_adsfadfs.png",
                        PlannedInspection = plannedInspection
                    }
                }
                };

                var referenceQuestion = new ReferenceQuestion
                {
                    Id = 7,
                    Category = questionCategory,
                    Question = pictureQuestion,
                    Contents = pictureQuestion.Contents,
                    Questionnaire = questionnaire,
                    Answers = new List<Answer>
                {
                    new FileAnswer
                    {
                        Id = 7,
                        UploadedFilePath = "/uploads/inspection_eruwioeiruwoio.png",
                        PlannedInspection = plannedInspection
                    }
                }
                };

                context.Questions.AddOrUpdate(
                    drawQuestion,
                    multipleChoiceQuestion,
                    numericQuestion,
                    ratingQuestion,
                    stringQuestion,
                    pictureQuestion,
                    referenceQuestion
                );

                var report = new Report
                {
                    Id = 1,
                    Festival = festival,
                    ReportEntries = new List<ReportEntry>
                {
                    new ReportTextEntry
                    {
                        Id = 1,
                        Order = 1,
                        Header = "Het vluchtplan",
                        Question = stringQuestion,
                        Text =
                            "Het vluchtplan was uitgebreid en zit goed in elkaar, maar de inspecteurs hadden nog wel een aantal dingen op te merken."
                    },
                    new ReportGraphEntry
                    {
                        Id = 2,
                        Order = 2,
                        GraphType = GraphType.Pie,
                        GraphXAxisType = GraphXAxisType.MultipleChoiceOption,
                        Question = multipleChoiceQuestion
                    }
                }
                };

                context.Reports.AddOrUpdate(report);

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private static Employee CreateEmployee(FestispecContext context)
        {
            var employee = new Employee
            {
                Id = 1,
                Iban = "NL01RABO1234567890",
                Name = new FullName
                {
                    First = "Henk",
                    Last = "Janssen"
                },
                Account = new Account
                {
                    Id = 1,

                    // Voorletter + Achternaam + geboortejaar
                    Username = "HJanssen80",
                    Password = BCrypt.Net.BCrypt.HashPassword("test123!"),
                    Role = Role.Inspector
                },
                Address = new Address
                {
                    City = "Utrecht",
                    Country = "Nederland",
                    HouseNumber = 59,
                    StreetName = "Chopinstraat",
                    ZipCode = "3533EL"
                },
                ContactDetails = new ContactDetails
                {
                    EmailAddress = "hjanssen80@gmail.com",
                    PhoneNumber = "+31623790426"
                },
                Certificates = new List<Certificate>
                {
                    new Certificate
                    {
                        Id = 1,
                        CertificateTitle = "Inspector Certificate",
                        CertificationDate = new DateTime(2018, 10, 11, 00, 00, 00),
                        ExpirationDate = new DateTime(2020, 10, 11, 00, 00, 00)
                    }
                }
            };

            context.Employees.AddOrUpdate(employee);

            var availability = new Availability
            {
                Id = 1,
                StartTime = new DateTime(2019, 12, 27, 00, 00, 00),
                EndTime = new DateTime(2019, 12, 27, 23, 59, 59),
                Employee = employee,
                EventTitle = "Henk beschikbaar",
                IsAvailable = true
            };

            context.Availabilities.AddOrUpdate(availability);

            return employee;
        }


    }
}
