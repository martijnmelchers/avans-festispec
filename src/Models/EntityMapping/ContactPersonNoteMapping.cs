using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class ContactPersonNoteMapping : EntityTypeConfiguration<ContactPersonNote>
    {
        public ContactPersonNoteMapping()
        {
            HasKey(ln => ln.ContactPersonId);
            HasKey(ln => ln.Created);

            HasRequired(ln => ln.ContactPerson)
                .WithMany(l => l.Notes)
                .HasForeignKey(ln => ln.ContactPersonId);

            Property(ln => ln.Note).IsRequired().HasMaxLength(500);
        }
    }
}