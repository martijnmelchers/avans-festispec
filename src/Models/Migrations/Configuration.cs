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
                    Country = "Netherlands",
                    HouseNumber = 59,
                    StreetName = "Chopinstraat",
                    ZipCode = "3533EL"
                },
                ContactDetails = new ContactDetails()
                {
                    EmailAddress = "hjanssen80@gmail.com",
                    PhoneNumber = "+31623790426"
                }
            };

            context.Employees.AddOrUpdate(employee);
        }
    }
}
