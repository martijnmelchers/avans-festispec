using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionCategoryMapping : EntityTypeConfiguration<QuestionCategory>
    {
        public QuestionCategoryMapping()
        {
            Property(qc => qc.CategoryName).HasMaxLength(45);

            HasMany(qc => qc.Questions).WithRequired(q => q.Category);
        }
    }
}