using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Infrastructure.SeedData
{
    public class SeedDataEntities
    {
        public static void AddSeed(ModelBuilder builder)
        {
            AddGenders(builder);
        }

        private static void AddGenders(ModelBuilder builder)
        {
            builder.Entity<Gender>()
                .HasData( 
                    new Gender { Name = "Мужской" },
                    new Gender { Name = "Женский" }
                );
        }
    }
}
