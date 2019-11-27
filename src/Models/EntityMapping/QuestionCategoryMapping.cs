using Festispec.Models.Questions;
using System.Data.Entity.ModelConfiguration;

namespace Festispec.Models.EntityMapping
{
    internal class QuestionCategoryMapping : EntityTypeConfiguration<QuestionCategory>
    {
        public QuestionCategoryMapping()
        {
            Property(qc => qc.CategoryName).IsRequired();

            HasMany(qc => qc.Questions).WithRequired(q => q.Category);
        }
    }
}