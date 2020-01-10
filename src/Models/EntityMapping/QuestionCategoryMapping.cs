using System.Data.Entity.ModelConfiguration;
using Festispec.Models.Questions;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionCategoryMapping : EntityTypeConfiguration<QuestionCategory>
    {
        public QuestionCategoryMapping()
        {
            Property(qc => qc.CategoryName).IsRequired();
            HasMany(qc => qc.Questions).WithOptional(q => q.Category);
        }
    }
}