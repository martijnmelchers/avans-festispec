using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AccountMapping : EntityTypeConfiguration<Account>
    {
        public AccountMapping()
        {
            Property(a => a.Username).IsRequired().HasMaxLength(45);
            Property(a => a.Password).IsRequired().HasMaxLength(100);

            Property(a => a.Role).IsRequired();

            HasRequired(a => a.Employee).WithRequiredDependent(e => e.Account);
        }
    }
}