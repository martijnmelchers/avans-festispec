using System.Collections.Generic;

namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Festispec.Models.EntityMapping.FestispecContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Festispec.Models.EntityMapping.FestispecContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var employee = new Employee() {
                Iban = "NL01RABO1234567890",
                Name = new FullName()
                {
                    First = "Henk",
                    Last = "Janssen"
                },
                Account = new Account()
                {
                    // Voorletter + Achternaam + geboortejaar
                    Username = "HJanssen80",
                    Password = "test123!",
                    Role = Role.Inspector
                },
                Address = new Address()
                {
                    City = "Utrecht",
                    Country = "Nederland",
                    HouseNumber = 59,
                    StreetName = "Chopinstraat",
                    ZipCode = "3533EL"
                },
                ContactDetails = new ContactDetails()
                {
                    EmailAddress = "hjanssen80@gmail.com",
                    PhoneNumber = "+31623790426"
                },
                Certificates = new List<Certificate>
                {
                    new Certificate
                    {
                        CertificateTitle = "Inspector Certificate",
                        CertificationDate = new DateTime(2018, 10, 11, 00, 00, 00),
                        ExpirationDate = new DateTime(2020, 10, 11, 00, 00, 00)
                    }
                }
            };

            context.Employees.AddOrUpdate(employee);

            var customer = new Customer()
            {
                CustomerName = "Q-DANCE",
                KvkNr = 34212891,
                Address = new Address {
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

            customer.ContactPersons = new List<ContactPerson>
            {
                new ContactPerson()
                {
                    Name = new FullName {
                        First = "Niels",
                        Last = "Kijf"
                    },
                    ContactDetails = new ContactDetails
                    {
                        // fake news
                        EmailAddress = "nielskijf@q-dance.com"
                    },
                    Role = "MA",
                    Notes = new List<ContactPersonNote>
                    {
                        new ContactPersonNote()
                        {
                            Note = "Contact opgenomen met Niels over een inspectie. Voorstel volgt."
                        }
                    }
                }
            };

            context.Customers.AddOrUpdate(customer);

            var festival = new Festival
            {
                FestivalName = "Q-BASE",
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

            customer.Festivals = new List<Festival>
            {
                festival
            };

            context.Customers.AddOrUpdate(customer);

            var questionnaire = new Questionnaire {
                Festival = festival,
            };

            var plannedInspection = new PlannedInspection
            {
                Employee = employee,
                Festival = festival,
                EventTitle = "Inspection " + festival.FestivalName,
                StartTime = new DateTime(2020, 7, 28, 20, 00, 00),
                EndTime = new DateTime(2020, 7, 29, 5, 00, 00),
                Questionnaire = questionnaire
            };

            context.PlannedInspections.AddOrUpdate(plannedInspection);

            context.SaveChanges();
        }
    }
}
