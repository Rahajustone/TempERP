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
                    new Gender { Id = new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"), Name = "Мужской" },
                    new Gender { Id = new Guid("0ce7a31f-dfd6-4bdc-ae57-32087c383705"), Name = "Женский" }
                );
        }
    }
}
