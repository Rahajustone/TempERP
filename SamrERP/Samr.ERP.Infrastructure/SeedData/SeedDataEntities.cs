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
            AddHandbook(builder);
        }

        private static void AddHandbook(ModelBuilder builder)
        {
            builder.Entity<Handbook>()
                .HasData(
                    new Handbook { Id = new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"), Name = "Nationality", DisplayName = "Национальность", ActionName = "Nationality/All" },
                    new Handbook { Id = new Guid("6a5587f0-f20e-47c5-9be9-de5aa3134c97"), Name = "Department", DisplayName = "Отдел", ActionName = "Department/All" },
                    new Handbook { Id = new Guid("0a07b4b6-76b5-4758-ae87-d4ff24bb1d12"), Name = "NewsCategories", DisplayName = "Категория полезных ссылок", ActionName = "NewsCategories/All" },
                    new Handbook { Id = new Guid("7a54980c-296e-4dee-b7cf-68a495c80ee0"), Name = "EmployeeLockReason", DisplayName = "Причина блокировки сотрудника", ActionName = "EmployeeLockReason/All" },
                    new Handbook { Id = new Guid("5a1b9eac-d4a4-4d92-aa77-53c0fe1bead0"), Name = "Position", DisplayName = "Позиция", ActionName = "Position/All" },
                    new Handbook { Id = new Guid("90fdba24-d34f-4347-896e-3bc652328c1f"), Name = "UserLockReason", DisplayName = "Причина блокировки пользователя", ActionName = "UserLockReason/All" },
                    new Handbook { Id = new Guid("3e11f7c3-ee41-4bea-aaf1-1fda2d4cb001"), Name = "UsefulLinkCategory", DisplayName = "Полезная ссылка", ActionName = "UsefulLinkCategory/All" }
                   

                );
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
