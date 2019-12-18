using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class EmployeeMapping : EntityTypeConfiguration<Employee>
    {
        public EmployeeMapping()
        {
            
            Property(e => e.Iban).IsRequired();
            HasRequired(e => e.Account).WithRequiredPrincipal(a => a.Employee);

            HasMany(e => e.PlannedEvents).WithRequired(pe => pe.Employee);
            HasMany(e => e.Certificates).WithRequired(c => c.Employee);
        }
    }
}