using System;
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
            AddRoles(builder);
            AddUsers(builder);
            AddUserLockReasons(builder);
            AddEmailSettings(builder);
        }

        private static void AddEmailSettings(ModelBuilder builder)
        {
            builder.Entity<EmailSetting>()
                .HasData(
                    new EmailSetting()
                    {
                        Id = Guid.Parse("9A3FCDDB-4680-4206-B712-4E07DF82E354"),
                        MailServer = "smtp.yandex.ru",
                        MailServerName = "Yandex Mail",
                        MailPort = 465,
                        SenderName = "Mig Dev",
                        Sender = "migdev@mig.tj",
                        Password = "formignow",
                        IsDefault = true,
                        IsActive = true,
                        CreatedUserId = GuidExtensions.FULL_GUID,
                        CreatedAt = new DateTime(2019,07,01),
                        EnabledSSL = true,
                        
                    });
        }

        private static void AddUsers(ModelBuilder builder)
        {
            var systemUser = new User()
            {
                Id = GuidExtensions.FULL_GUID,
                UserName = "000000000",
                Email = "migdev@mig.tj",
                PhoneNumber = "000000000",
                NormalizedUserName = "000000000",
                PasswordHash = "AQAAAAEAACcQAAAAEOvH5DQ4ttSuk1j8EVrg4uyxzHJGcnZbuhkdRvuppk2ttPByA/FjKpVcrA001HW68w==",//123qwe
                ConcurrencyStamp = "9132A248-C5F8-4B01-91FB-9AF3777FCA48",
                SecurityStamp = "83147D9F-26BC-486F-AE7E-5DD581362FAA"
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
                        CreatedUserId = GuidExtensions.FULL_GUID,
                        CreatedAt = new DateTime(2019, 07, 01),
                    }

                );
        }

        private static void AddRoles(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasData(
                    new Role { Id = new Guid("2702BCDD-104B-475D-14B5-08D70C357974"), Name = "Employee.All", Description = "Просмотр списка", Category = "Employee", CategoryName = "Сотрудники", NormalizedName = "EMPLOYEE.ALL" ,ConcurrencyStamp = "23F4A768-BCF3-4BA4-8D20-CC3E4A9C333A" },
                    new Role { Id = new Guid("29ECF6CE-B82F-4FC5-AE01-08D70CF9F908"), Name = "Employee.Create", Description = "Создание", Category = "Employee", CategoryName = "Сотрудники", NormalizedName = "EMPLOYEE.CREATE", ConcurrencyStamp = "B8EFD7E4-72E8-4110-96CC-A531AD35D9B4" },
                    new Role { Id = new Guid("A8EB0E97-EAAA-4976-AE02-08D70CF9F908"), Name = "Employee.Edit", Description = "Редактирование", Category = "Employee", CategoryName = "Сотрудники", NormalizedName = "EMPLOYEE.EDIT", ConcurrencyStamp = "98003B79-EE18-4D7F-B8A5-357E74E8F77A" },
                    new Role { Id = new Guid("C5DBEAAB-86A3-4400-B50A-08D70E6B40DC"), Name = "Employee.Details", Description = "Подробная информация", Category = "Employee", CategoryName = "Сотрудники", NormalizedName = "EMPLOYEE.DETAILS", ConcurrencyStamp = "36271C6B-8972-4A69-90D0-D9921B6F90D3" },
                    new Role { Id = new Guid("d8bc97fc-7a24-41d7-9612-b9e9ace30af9"), Name = "FileArchive.Create", Description = "Создание", Category = "FileArchive", CategoryName = "Файл Архиве", NormalizedName = "FILEARCHIVE.CREATE", ConcurrencyStamp = "8fa485ea-1ed7-40ba-8e01-4f7f9079f667" },
                    new Role { Id = new Guid("be43f589-77ba-475d-bba8-af504aae540e"), Name = "FileArchive.Edit", Description = "Редактирование", Category = "FileArchive", CategoryName = "Файл Архиве", NormalizedName = "FILEARCHIVE.EDIT", ConcurrencyStamp = "429f6fed-a00a-4cbb-a105-70f5c6b040aa" }
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
