using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AccountMapping : EntityTypeConfiguration<Account>
    {
        public AccountMapping()
        {
            Property(a => a.Username).IsRequired();
            Property(a => a.Password).IsRequired();
            Property(a => a.Role).IsRequired();

            HasRequired(a => a.Employee)
                .WithRequiredDependent(e => e.Account);
        }
    }
}