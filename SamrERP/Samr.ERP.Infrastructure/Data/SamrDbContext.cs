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

        public DbSet<Employee> Employees { get;set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentLog> DepartmentLogs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionLog> PositionLogs { get; set; }
        public DbSet<EmployeeLockReason> EmployeeLockReasons { get; set; }
        public DbSet<EmployeeLockReasonLog> EmployeeLockReasonLogs { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<NationalityLog> NationalityLogs { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsCategoryLog> NewsCategoryLogs { get; set; }
        public DbSet<EmailSetting> EmailSettings { get; set; }
        public DbSet<EmailMessageHistory> EmailMessageHistories { get; set; }
        public DbSet<UserLockReason> UserLockReasons { get; set; }
        public DbSet<UsefulLinkCategory> UsefulLinkCategories { get; set; }
        public DbSet<UsefulLinkCategoryLog> UsefulLinkCategoryLogs { get; set; }
        public DbSet<UsefulLink> UsefulLinks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<FileCategory> FileCategories { get; set; }
        public DbSet<FileCategoryLog> FileCategoryLogs { get; set; }
        public DbSet<FileArchive> FileArchives { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActiveUserToken> ActiveUserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<Employee>().ToTable("Employees");
            //builder.Entity<Department>().ToTable("Departments");
            //builder.Entity<DepartmentLog>().ToTable("DepartmentLog");

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

            SeedDataEntities.AddSeed(builder, this);
        }
    }
}

