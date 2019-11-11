using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AttachmentMapping : EntityTypeConfiguration<Attachment>
    {
        public AttachmentMapping()
        {
            Property(a => a.FilePath).IsRequired();

            HasRequired(a => a.Answer).WithMany(a => a.Attachments);
        }
    }
}