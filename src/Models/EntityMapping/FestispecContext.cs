using Festispec.Models.Answers;
using Festispec.Models.Questions;
using Festispec.Models.Reports;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.Models.EntityMapping
{
    public class FestispecContext : DbContext
    {
        public FestispecContext() : base("default")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = true;

            modelBuilder.Configurations.AddFromAssembly(typeof(FestispecContext).Assembly);
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Availability> Availabilities { get; set; }
        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<ContactPersonNote> ContactPersonNotes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Festival> Festivals { get; set; }
        public virtual DbSet<PlannedEvent> PlannedEvents { get; set; }
        public virtual DbSet<PlannedInspection> PlannedInspections { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionCategory> QuestionCategories { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportEntry> ReportEntries { get; set; }


        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            foreach (var entity in ChangeTracker.Entries().Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified)))
            {
                if (entity.State == EntityState.Added)
                    ((Entity)entity.Entity).CreatedAt = DateTime.UtcNow;

                ((Entity)entity.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}