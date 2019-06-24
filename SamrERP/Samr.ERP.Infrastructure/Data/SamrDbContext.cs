using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Infrastructure.Entities;

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
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Employee>().ToTable("Employees");
            //builder.Entity<Employee>()
            //    .HasIndex(e => e.PhotoPath)
            //    .IsUnique();
        }
    }
}

