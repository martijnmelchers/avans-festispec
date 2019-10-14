using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class LiaisonNoteMapping : EntityTypeConfiguration<LiaisonNote>
    {
        public LiaisonNoteMapping()
        {
            HasKey(ln => ln.LiaisonId);
            HasKey(ln => ln.Created);

            HasRequired(ln => ln.Liaison)
                .WithMany(l => l.Notes)
                .HasForeignKey(ln => ln.LiaisonId);

            Property(ln => ln.Note).IsRequired().HasMaxLength(500);
        }
    }
}