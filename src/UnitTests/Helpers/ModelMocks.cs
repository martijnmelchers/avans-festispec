using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.UnitTests.Helpers
{
    public class ModelMocks
    {
        public static Festival Festival()
        {
            var festival = new Festival()
            {
                FestivalName = "PinkPop",
                Description = "Placeholder for description",
                Address = Address(),
                OpeningHours = OpeningHours(),
                Customer = Customer()
            };

            return festival;
        }

        public static OpeningHours OpeningHours()
        {
            var openingHours = new OpeningHours()
            {
                StartTime = new DateTime(2020, 3, 10, 20, 0, 0),
                EndTime = new DateTime(2020, 3, 12, 1, 0, 0)
            };

            return openingHours;
        }

        public static Customer Customer()
        {
            var customer = new Customer()
            {
                KvkNr = 12345678,
                CustomerName = "PinkPop",
                Address = Address(),
                ContactDetails = ContactDetails()
            };

            return customer;
        }

        public static Address Address()
        {
            var address = new Address()
            {
                ZipCode = "1013 GM",
                StreetName = "Amsterweg",
                HouseNumber = 23,
                City = "Utrecht",
                Country = "Nederland"
            };

            return address;
        }

        public static ContactDetails ContactDetails()
        {
            var contactDetails = new ContactDetails()
            {
                PhoneNumber = "31695734859",
                EmailAddress = "psmulde@pinkpop.nl"
            };

            return contactDetails;
        }
    }
}
