using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class EmployeeMapping : EntityTypeConfiguration<Employee>
    {
        public EmployeeMapping()
        {
            Property(e => e.EmployeeName).IsRequired().HasMaxLength(45);
            Property(e => e.Role).IsRequired();

            // https://en.wikipedia.org/wiki/International_Bank_Account_Number#Basic_Bank_Account_Number
            // "Each country can have a different national routing/account numbering system, up to a maximum of 30 alphanumeric characters."
            Property(e => e.Iban).IsRequired().HasMaxLength(30);

            HasRequired(e => e.Account).WithRequiredPrincipal(a => a.Employee);
            HasRequired(e => e.Address).WithRequiredPrincipal();
            HasRequired(e => e.ContactDetails).WithRequiredPrincipal();

            HasMany(e => e.PlannedEvents).WithRequired(pe => pe.Employee);
            HasMany(e => e.Availabilities).WithRequired(a => a.Employee);
            HasMany(e => e.Certificates).WithRequired(c => c.Employee);
        }
    }
}