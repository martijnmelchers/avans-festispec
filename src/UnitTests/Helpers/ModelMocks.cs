using Festispec.Models;
using System;
using System.Collections.Generic;
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

        public static Questionnaire Questionnaire = new Questionnaire("PinkPop Ochtend", Festival);

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
            Questionnaire,
            new Questionnaire("PinkPop Middag", Festival)
        };

    }
}
