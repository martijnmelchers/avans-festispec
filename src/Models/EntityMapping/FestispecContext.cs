﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using Microsoft.Extensions.Configuration;

namespace Festispec.Models.EntityMapping
{
    public class FestispecContext : DbContext
    {
        public FestispecContext(IConfiguration config) : base(config["ConnectionString"])
        {
        }

        public FestispecContext() : base("Default")
        {
        }
        
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Availability> Availabilities { get; set; }
        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Festival> Festivals { get; set; }
        public virtual DbSet<PlannedEvent> PlannedEvents { get; set; }
        public virtual DbSet<PlannedInspection> PlannedInspections { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<DistanceResult> DistanceResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.LazyLoadingEnabled = true;

            modelBuilder.Configurations.AddFromAssembly(typeof(FestispecContext).Assembly);
        }

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
        
        [DbFunction("Edm", "TruncateTime")]
        public static DateTime? TruncateTime(DateTime? dateValue)
        {
            return dateValue?.Date;
        }

        private void AddTimestamps()
        {
            foreach (DbEntityEntry entity in ChangeTracker.Entries().Where(x =>
                x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified)))
            {
                if (entity.State == EntityState.Added)
                    ((Entity) entity.Entity).CreatedAt = DateTime.UtcNow;

                ((Entity) entity.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
