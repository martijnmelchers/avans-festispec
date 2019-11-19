using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class ContactPersonNoteMapping : EntityTypeConfiguration<ContactPersonNote>
    {
        public ContactPersonNoteMapping()
        {
            HasRequired(ln => ln.ContactPerson).WithMany(l => l.Notes);

            Property(ln => ln.Note).IsRequired().HasMaxLength(500);
        }
    }
}