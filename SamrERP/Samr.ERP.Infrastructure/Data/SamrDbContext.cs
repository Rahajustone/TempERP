using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Entities.BaseObjects;

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

            //builder.Entity<Position>()
            //    .HasIndex(e => e.Name)
            //    .IsUnique();
        }
    }
}

