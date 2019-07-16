using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.SeedData;

namespace Samr.ERP.Infrastructure.Data
{
    public class SamrDbContext : IdentityDbContext<User,Role,Guid>
    {
        
		

        public SamrDbContext(DbContextOptions<SamrDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Employee> Employees;
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeeLockReason> EmployeeLockReasons { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<EmailSetting> EmailSettings { get; set; }
        public DbSet<EmailMessageHistory> EmailMessageHistories { get; set; }
        public DbSet<UserLockReason> UserLockReasons { get; set; }
        public DbSet<UsefulLinkCategory> UsefulLinkCategories { get; set; }
        public DbSet<UsefulLink> UsefulLinks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Handbook> Handbooks { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employee>().ToTable("Employees");
            builder.Entity<Department>().ToTable("Departments");

            // cascade delete false
            var cascadeFKs = builder.Model.GetEntityTypes()
                .ToList()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = fk.IsRequired ? DeleteBehavior.Restrict :  DeleteBehavior.SetNull;
            }

            //EntitiesConfiguration.ConfigureEntities(builder);

            SeedDataEntities.AddSeed(builder);
        }
    }
}

