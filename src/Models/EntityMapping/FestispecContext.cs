using System.Data.Entity;

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
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Availability> Availabilities { get; set; }
        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<ContactDetails> ContactDetails { get; set; }
        public virtual DbSet<ContactPerson> ContactPersons { get; set; }
        public virtual DbSet<ContactPersonNote> ContactPersonNotes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Festival> Festivals { get; set; }
        public virtual DbSet<OpeningHours> OpeningHours { get; set; }
        public virtual DbSet<PlannedEvent> PlannedEvents { get; set; }
        public virtual DbSet<PlannedInspection> PlannedInspections { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionCategory> QuestionCategories { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportEntry> ReportEntries { get; set; }
    }
}