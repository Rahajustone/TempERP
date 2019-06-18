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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

