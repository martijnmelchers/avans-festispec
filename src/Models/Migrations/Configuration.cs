using System.Collections.Generic;
using Festispec.Models.EntityMapping;

namespace Festispec.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

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

            var employee = CreateEmployee(context);

            var customer = new Customer()
            {
                Id = 1,
                CustomerName = "Q-DANCE",
                KvkNr = 34212891,
                Address = new Address
                {
                    Id = 2,
                    StreetName = "Isolatorweg",
                    HouseNumber = 36,
                    ZipCode = "1014AS",
                    City = "Amsterdam",
                    Country = "Nederland"
                },
                ContactDetails = new ContactDetails
                {
                    Id = 2,
                    EmailAddress = "info@q-dance.com",
                    PhoneNumber = "+31204877300"
                }
            };

            context.Customers.AddOrUpdate(customer);

            var contactDetails = new ContactDetails
            {
                Id = 3,

                // fake news
                EmailAddress = "nielskijf@q-dance.com"
            };

            context.ContactDetails.AddOrUpdate(contactDetails);

            var fullName = new FullName
            {
                Id = 2,
                First = "Niels",
                Last = "Kijf"
            };

            context.FullNames.AddOrUpdate(fullName);

            var contactPerson = new ContactPerson()
            {
                Id = 1,
                Customer = customer,
                Name = fullName,
                ContactDetails = contactDetails,
                Role = "MA"
            };

            context.ContactPersons.AddOrUpdate(contactPerson);

            var note = new ContactPersonNote()
            {
                Id = 1,
                ContactPerson = contactPerson,
                Note = "Contact opgenomen met Niels over een inspectie. Voorstel volgt."
            };

            context.ContactPersonNotes.AddOrUpdate(note);

            //var address = new Address
            //{
            //    Id = 3,
            //    Country = "Duitsland",
            //    StreetName = "Flughafen-Ring",
            //    HouseNumber = 16,
            //    City = "Weeze",
            //    ZipCode = "NW47652"
            //};

            //context.Addresses.AddOrUpdate(address);
            //var festival = new Festival
            //{
            //    Id = 1,
            //    FestivalName = "Q-BASE",
            //    Customer = customer,
            //    Description = "Nachtfestival over de grens",
            //    Address = address
            //};

            //context.Festivals.AddOrUpdate(festival);


            //var openingHours = new OpeningHours
            //{
            //    Id = 1,
            //    StartTime = new DateTime(2020, 9, 5, 18, 0, 0),
            //    EndTime = new DateTime(2020, 9, 6, 8, 0, 0),
            //    Festival = festival
            //};

            //context.OpeningHours.AddOrUpdate(openingHours);

            //customer.Festivals = new List<Festival>
            //{
            //    festival
            //};

            //context.Customers.AddOrUpdate(customer);

            //var questionnaire = new Questionnaire
            //{
            //    Id = 1,
            //    Festival = festival,
            //};

            //var plannedInspection = new PlannedInspection
            //{
            //    Id = 1,
            //    Employee = employee,
            //    Festival = festival,
            //    EventTitle = "Inspection " + festival.FestivalName,
            //    StartTime = new DateTime(2020, 7, 28, 20, 00, 00),
            //    EndTime = new DateTime(2020, 7, 29, 5, 00, 00),
            //    Questionnaire = questionnaire
            //};

            //context.PlannedInspections.AddOrUpdate(plannedInspection);

            context.SaveChanges();
        }

        private static Employee CreateEmployee(FestispecContext context)
        {
            var employee = new Employee()
            {
                Id = 1,
                Iban = "NL01RABO1234567890",
                Name = new FullName()
                {
                    Id = 1,
                    First = "Henk",
                    Last = "Janssen"
                },
                Account = new Account()
                {
                    Id = 1,

                    // Voorletter + Achternaam + geboortejaar
                    Username = "HJanssen80",
                    Password = "test123!",
                    Role = Role.Inspector
                },
                Address = new Address()
                {
                    Id = 1,
                    City = "Utrecht",
                    Country = "Nederland",
                    HouseNumber = 59,
                    StreetName = "Chopinstraat",
                    ZipCode = "3533EL"
                },
                ContactDetails = new ContactDetails()
                {
                    Id = 1,
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

            return employee;
        }
    }
}
