using Microsoft.EntityFrameworkCore;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Infrastructure.Data
{
    public class EntitiesConfiguration
    {
        public static void ConfigureEntities(ModelBuilder builder)
        {
            builder.Entity<Employee>().HasIndex(e => new { e.Phone, e.PassportNumber }).IsUnique();

            builder.Entity<Gender>().HasIndex(e => e.Name).IsUnique();

            builder.Entity<EmployeeLockReason>().HasIndex(e => e.Name).IsUnique();

            builder.Entity<Nationality>().HasIndex(e => e.Name).IsUnique();

            builder.Entity<Position>().HasIndex(e => e.Name).IsUnique();

            builder.Entity<Department>().HasIndex(e => e.Name).IsUnique();
        }
    }
}