using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AccountMapping : EntityTypeConfiguration<Account>
    {
        public AccountMapping()
        {
            Property(a => a.Password).HasMaxLength(45);

            HasRequired(a => a.Employee).WithRequiredDependent(e => e.Account);
        }
    }
}