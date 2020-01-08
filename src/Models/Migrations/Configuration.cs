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
                var address = new Address
                {
                    Id = 1,
                    StreetName = "Isolatorweg",
                    HouseNumber = 36,
                    ZipCode = "1014AS",
                    City = "Amsterdam",
                    Country = "Nederland",
                    Latitude = 52.39399f,
                    Longitude = 4.8507514f
                };

                var address2 = new Address
                {
                    Id = 2,
                    Country = "Duitsland",
                    StreetName = "Flughafen-Ring",
                    HouseNumber = 16,
                    City = "Weeze",
                    ZipCode = "NW47652",
                    Latitude = 51.5924149f,
                    Longitude = 6.1545434f
                };

                var address3 = new Address
                {
                    Id = 3,
                    City = "Utrecht",
                    Country = "Nederland",
                    HouseNumber = 59,
                    StreetName = "Chopinstraat",
                    ZipCode = "3533EL",
                    Latitude = 52.0857048f,
                    Longitude = 5.08541441f
                };

                var address4 = new Address
                {
                    City = "Amsterdam",
                    Country = "Nederland",
                    HouseNumber = 14,
                    StreetName = "Lutmastraat",
                    ZipCode = "1072JL",
                    Latitude = 52.350400f,
                    Longitude = 4.892710f
                };

                context.Addresses.AddOrUpdate(address, address2, address3, address4);

                var employee = CreateEmployee(context, address3);

                var customer = new Customer
                {
                    Id = 1,
                    CustomerName = "Q-DANCE",
                    KvkNr = 34212891,
                    Address = address,
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
                var now = DateTime.Now;
                var festival = new Festival
                {
                    Id = 1,
                    FestivalName = "Q-BASE",
                    Customer = customer,
                    Description = "Nachtfestival over de grens",
                    Address = address2,
                    OpeningHours = new OpeningHours
                    {
                        StartTime = new TimeSpan(now.Hour, now.Minute, now.Second),
                        EndTime = new TimeSpan(8, 0, 0),
                        StartDate = new DateTime(2020, 1, 1),
                        EndDate = new DateTime(2020, 9, 6)
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
                    Festival = festival,
                };

                context.Questionnaires.AddOrUpdate(questionnaire);

                var employeeInspector = new Employee
                {
                    Id = 2,
                    Iban = "NL01RABO12789410",
                    Name = new FullName
                    {
                        First = "Jan",
                        Last = "Dirksen"
                    },
                    Account = new Account
                    {
                        Id = 1,

                        // Voorletter + Achternaam + geboortejaar
                        Username = "JDirksen89",
                        Password = BCrypt.Net.BCrypt.HashPassword("TestWachtwoord"),
                        Role = Role.Inspector
                    },
                    Address = address4,
                    ContactDetails = new ContactDetails
                    {
                        EmailAddress = "jdirksen89@gmail.com",
                        PhoneNumber = "+31987654321"
                    },
                    Certificates = new List<Certificate>
                    {
                        new Certificate
                        {
                            Id = 2,
                            CertificateTitle = "Inspection Certificate",
                            CertificationDate = new DateTime(2019, 3, 4, 00, 00, 00),
                            ExpirationDate = new DateTime(2021, 3, 4, 00, 00, 00)
                        }
                    }
                };

                var plannedInspection = new PlannedInspection
                {
                    Id = 1,
                    Employee = employeeInspector,
                    Festival = festival,
                    EventTitle = "Inspection " + festival.FestivalName,
                    StartTime = DateTime.Now,
                    EndTime = new DateTime(2020, 7, 29, 5, 00, 00),
                    Questionnaire = questionnaire,
                };

                context.PlannedInspections.AddOrUpdate(plannedInspection);

                var questionCategory = new QuestionCategory
                {
                    Id = 1,
                    CategoryName = "Vragen over veiligheid"
                };

                context.QuestionCategories.AddOrUpdate(questionCategory);


                #region DrawQuestion
                var drawQuestion = new DrawQuestion
                {
                    Id = 1,
                    Category = questionCategory,
                    PicturePath = "/Uploads/grasso.png",
                    Questionnaire = questionnaire,
                    Contents = "Wat is de kortste looproute van de mainstage naar de nooduitgang?",
                };

                var drawQuestionAnswer = new FileAnswer
                {
                    Id = 1,
                    Question = drawQuestion,
                    UploadedFilePath = "/Uploads/inspection_adsfadfs.png",
                    PlannedInspection = plannedInspection
                };

                context.Answers.AddOrUpdate(drawQuestionAnswer);
                context.Questions.AddOrUpdate(drawQuestion);
                #endregion

                #region MultipleChoiceQuestion
                var multipleChoiceQuestion = new MultipleChoiceQuestion
                {
                    Id = 2,
                    Category = questionCategory,
                    Contents = "Zijn er evacuatieplannen zichtbaar opgesteld?",
                    Options = "Ja~Nee",
                    OptionCollection = new ObservableCollection<StringObject>()
                    {
                        new StringObject("Option1")
                    },
                    Questionnaire = questionnaire
                };

                var multipleChoiceQuestionAnswer = new MultipleChoiceAnswer
                {
                    Id = 2,
                    MultipleChoiceAnswerKey = 0,
                    PlannedInspection = plannedInspection,
                    Question = multipleChoiceQuestion,
                    Attachments = new List<Attachment>
                    {
                        new Attachment
                        {
                            Id = 1,
                            FilePath = "/attachments/1.png"
                        }
                    }
                };

                context.Answers.AddOrUpdate(multipleChoiceQuestionAnswer);
                context.Questions.AddOrUpdate(multipleChoiceQuestion);
                #endregion

                #region NumericQuestion
                var numericQuestion = new NumericQuestion
                {
                    Id = 3,
                    Category = questionCategory,
                    Contents = "Hoeveel EHBO-posten zijn er aanwezig?",
                    Minimum = 0,
                    Maximum = 99,
                    Questionnaire = questionnaire,

                };

                var numericQuestionAnswer = new NumericAnswer
                {
                    Id = 3,
                    Question = numericQuestion,
                    IntAnswer = 3,
                    PlannedInspection = plannedInspection
                };

                context.Answers.AddOrUpdate(numericQuestionAnswer);
                context.Questions.AddOrUpdate(numericQuestion);
                #endregion

                #region RatingQuestion
                var ratingQuestion = new RatingQuestion
                {
                    Id = 4,
                    Category = questionCategory,
                    Contents = "Op een schaal van 1 tot 5, is de beveiliging voldoende aanwezig op het terrein?",
                    HighRatingDescription = "Er is veel beveiliging",
                    LowRatingDescription = "Er is amper beveiliging",
                    Questionnaire = questionnaire,
                };

                var ratingQuestionAnswer = new NumericAnswer
                {
                    Id = 3,
                    Question = numericQuestion,
                    IntAnswer = 4,
                    PlannedInspection = plannedInspection
                };

                context.Answers.AddOrUpdate(ratingQuestionAnswer);
                context.Questions.AddOrUpdate(ratingQuestion);

                #endregion

                #region StringQuestion
                var stringQuestion = new StringQuestion
                {
                    Id = 5,
                    Category = questionCategory,
                    Contents = "Geef een korte samenvatting van het vluchtplan.",
                    IsMultiline = true,
                    Questionnaire = questionnaire,
                };

                var stringQuestionAnswer = new StringAnswer
                {
                    Id = 5,
                    Question = stringQuestion,
                    AnswerContents = "In geval van een calamiteit is voor de bezoekers duidelijk te zien dat er vanaf de mainstage al vier vluchtroutes bestaan.",
                    PlannedInspection = plannedInspection
                };

                context.Answers.AddOrUpdate(stringQuestionAnswer);
                context.Questions.AddOrUpdate(stringQuestion);

                #endregion

                #region PictureQuestion
                var pictureQuestion = new UploadPictureQuestion
                {
                    Id = 6,
                    Category = questionCategory,
                    Contents = "Plaats een foto van de vluchtroutes op het calamiteitenplan.",
                    Questionnaire = questionnaire,
                };

                var pictureQuestionAnswer = new FileAnswer
                {
                    Id = 6,
                    Question = pictureQuestion,
                    UploadedFilePath = "/uploads/inspection_adsfadfs.png",
                    PlannedInspection = plannedInspection
                };

                context.Answers.AddOrUpdate(pictureQuestionAnswer);
                context.Questions.AddOrUpdate(pictureQuestion);

                #endregion

                #region ReferenceQuestion
                var referenceQuestion = new ReferenceQuestion
                {
                    Id = 7,
                    Category = questionCategory,
                    Question = pictureQuestion,
                    Contents = pictureQuestion.Contents,
                    Questionnaire = questionnaire
                };

                var referenceQuestionAnswer = new FileAnswer
                {
                    Id = 7,
                    Question = referenceQuestion,
                    UploadedFilePath = "/uploads/inspection_eruwioeiruwoio.png",
                    PlannedInspection = plannedInspection
                };

                context.Answers.AddOrUpdate(referenceQuestionAnswer);
                context.Questions.AddOrUpdate(referenceQuestion);
                #endregion

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

           

                context.Employees.AddOrUpdate(employeeInspector);

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

        private static Employee CreateEmployee(FestispecContext context, Address address)
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
                    Role = Role.Employee,
                },
                Address = address,
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
                StartTime = DateTime.Now,
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
