﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Samr.ERP.Infrastructure.Data;

namespace Samr.ERP.Infrastructure.Migrations
{
    [DbContext(typeof(SamrDbContext))]
    partial class SamrDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid?>("RootId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.EmailMessageHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<Guid>("EmailSettingId");

                    b.Property<string>("Message");

                    b.Property<string>("RecieverEMail");

                    b.Property<Guid>("RecieverUserId");

                    b.Property<string>("Subject");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("EmailSettingId");

                    b.HasIndex("RecieverUserId");

                    b.ToTable("EmailMessageHistories");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.EmailSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDefault");

                    b.Property<int>("MailPort");

                    b.Property<string>("MailServer")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("MailServerName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Password");

                    b.Property<string>("Sender")
                        .IsRequired();

                    b.Property<string>("SenderName")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("EmailSettings");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Description")
                        .HasMaxLength(256);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<Guid?>("EmployeeLockReasonId");

                    b.Property<string>("FactualAddress")
                        .HasMaxLength(256);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<Guid>("GenderId");

                    b.Property<DateTime>("HireDate");

                    b.Property<string>("ImageName")
                        .HasMaxLength(32);

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("LockDate");

                    b.Property<Guid?>("LockUserId");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(32);

                    b.Property<Guid?>("NationalityId");

                    b.Property<string>("PassportAddress")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("PassportIssueDate");

                    b.Property<string>("PassportIssuer")
                        .HasMaxLength(64);

                    b.Property<string>("PassportNumber")
                        .HasMaxLength(32);

                    b.Property<string>("PassportScanPath");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(9);

                    b.Property<string>("PhotoPath");

                    b.Property<Guid>("PositionId");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("EmployeeLockReasonId");

                    b.HasIndex("GenderId");

                    b.HasIndex("LockUserId");

                    b.HasIndex("NationalityId");

                    b.HasIndex("PositionId");

                    b.HasIndex("UserId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.EmployeeLockReason", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("EmployeeLockReasons");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Gender", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.ToTable("Genders");

                    b.HasData(
                        new
                        {
                            Id = new Guid("dac6d4fa-0502-43da-9368-9198e479f89d"),
                            Name = "Мужской"
                        },
                        new
                        {
                            Id = new Guid("0ce7a31f-dfd6-4bdc-ae57-32087c383705"),
                            Name = "Женский"
                        });
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Nationality", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("Nationalities");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<Guid>("NewsCategoryId");

                    b.Property<string>("PublishAt")
                        .IsRequired();

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("NewsCategoryId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.NewsCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("NewsCategories");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<Guid>("DepartmentId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.UsefulLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<string>("Description")
                        .HasMaxLength(512);

                    b.Property<bool>("IsActive");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<Guid>("UsefulLinkCategoryId");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("UsefulLinkCategoryId");

                    b.ToTable("UsefulLinks");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.UsefulLinkCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("UsefulLinkCategories");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("LockDate");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<Guid?>("UserLockReasonId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UserLockReasonId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.UserLockReason", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("CreatedUserId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.ToTable("UserLockReasons");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Department", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.EmailMessageHistory", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.EmailSetting", "EmailSetting")
                        .WithMany()
                        .HasForeignKey("EmailSettingId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "RecieverUser")
                        .WithMany()
                        .HasForeignKey("RecieverUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.EmailSetting", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Employee", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.EmployeeLockReason", "EmployeeLockReason")
                        .WithMany()
                        .HasForeignKey("EmployeeLockReasonId");

                    b.HasOne("Samr.ERP.Infrastructure.Entities.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "LockUser")
                        .WithMany()
                        .HasForeignKey("LockUserId");

                    b.HasOne("Samr.ERP.Infrastructure.Entities.Nationality", "Nationality")
                        .WithMany()
                        .HasForeignKey("NationalityId");

                    b.HasOne("Samr.ERP.Infrastructure.Entities.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.EmployeeLockReason", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Nationality", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.News", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.NewsCategory", "NewsCategory")
                        .WithMany()
                        .HasForeignKey("NewsCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.NewsCategory", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.Position", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.UsefulLink", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Samr.ERP.Infrastructure.Entities.UsefulLinkCategory", "UsefulLinkCategory")
                        .WithMany()
                        .HasForeignKey("UsefulLinkCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.UsefulLinkCategory", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.User", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.UserLockReason", "UserLockReason")
                        .WithMany()
                        .HasForeignKey("UserLockReasonId");
                });

            modelBuilder.Entity("Samr.ERP.Infrastructure.Entities.UserLockReason", b =>
                {
                    b.HasOne("Samr.ERP.Infrastructure.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
