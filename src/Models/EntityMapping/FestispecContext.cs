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

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<ContactPersonNote> ContactPersonNotes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Festival> Festivals { get; set; }
        public DbSet<OpeningHours> OpeningHours { get; set; }
        public DbSet<PlannedEvent> PlannedEvents { get; set; }
        public DbSet<PlannedInspection> PlannedInspections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionCategory> QuestionCategories { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportEntry> ReportEntries { get; set; }
    }
}