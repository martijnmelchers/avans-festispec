using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class AttachmentMapping : EntityTypeConfiguration<Attachment>
    {
        public AttachmentMapping()
        {
            Property(a => a.FileName).IsRequired().HasMaxLength(45);
            // 5 MB
            Property(a => a.FileContents).IsRequired().HasMaxLength(5000000);

            HasRequired(a => a.Answer).WithMany(a => a.Attachments);
        }
    }
}