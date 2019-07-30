﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Extensions;

namespace Samr.ERP.Infrastructure.SeedData
{
    public class SeedDataEntities
    {
        public static void AddSeed(ModelBuilder builder, Data.SamrDbContext samrDbContext)
        {
            AddGenders(builder);
            AddHandbook(builder);
            //AddRoles(builder);
            AddUsers(builder);
            AddUserLockReasons(builder);
        }

        private static void AddUsers(ModelBuilder builder)
        {
            var systemUser = new User()
            {
                Id = GuidExtensions.FULL_GUID,
                UserName = "000000000",
                Email = "migdev@mig.tj",
                NormalizedUserName = "000000000",
                PasswordHash = "AQAAAAEAACcQAAAAEOvH5DQ4ttSuk1j8EVrg4uyxzHJGcnZbuhkdRvuppk2ttPByA/FjKpVcrA001HW68w==",
                ConcurrencyStamp = "9132A248-C5F8-4B01-91FB-9AF3777FCA48"
            };
            
            builder.Entity<User>()
                .HasData(systemUser);
        }

        private static void AddUserLockReasons(ModelBuilder builder)
        {
            builder.Entity<UserLockReason>()
                .HasData(
                    new UserLockReason()
                    {
                        Id = GuidExtensions.FULL_GUID,
                        CreatedUserId = GuidExtensions.FULL_GUID
                    }

                );
        }

        private static void AddRoles(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasData(
                    new Role { Id = new Guid("2702BCDD-104B-475D-14B5-08D70C357974"), Name = "Employee.All", Description = "Просмотр списка", Category = "Employee", NormalizedName = "EMPLOYEE.ALL", ConcurrencyStamp = "23F4A768-BCF3-4BA4-8D20-CC3E4A9C333A" },
                    new Role { Id = new Guid("29ECF6CE-B82F-4FC5-AE01-08D70CF9F908"), Name = "Employee.Create", Description = "Создание", Category = "Employee", NormalizedName = "EMPLOYEE.CREATE", ConcurrencyStamp = "B8EFD7E4-72E8-4110-96CC-A531AD35D9B4" },
                    new Role { Id = new Guid("A8EB0E97-EAAA-4976-AE02-08D70CF9F908"), Name = "Employee.Edit", Description = "Редактирование", Category = "Employee", NormalizedName = "EMPLOYEE.EDIT", ConcurrencyStamp = "98003B79-EE18-4D7F-B8A5-357E74E8F77A" },
                    new Role { Id = new Guid("C5DBEAAB-86A3-4400-B50A-08D70E6B40DC"), Name = "Employee.Details", Description = "Подробная информация", Category = "Employee", NormalizedName = "EMPLOYEE.DETAILS", ConcurrencyStamp = "36271C6B-8972-4A69-90D0-D9921B6F90D3" }
                );
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
                    new Handbook { Id = new Guid("3e11f7c3-ee41-4bea-aaf1-1fda2d4cb001"), Name = "UsefulLinkCategory", DisplayName = "Полезная ссылка", ActionName = "UsefulLinkCategory/All" },
                    new Handbook { Id = new Guid("92ddaaaf-fd9f-4f99-8443-2bed011e9d78"), Name = "FileCategory", DisplayName = "Категория файла", ActionName = "FileCategory/All" }


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
