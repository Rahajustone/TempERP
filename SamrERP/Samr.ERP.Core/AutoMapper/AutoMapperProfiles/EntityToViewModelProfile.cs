﻿using System;
using System.IO.Compression;
using System.Security.AccessControl;
using AutoMapper;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;
using Samr.ERP.Core.ViewModels.News;
using Samr.ERP.Core.ViewModels.News.Categories;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.AutoMapper.AutoMapperProfiles
{
    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();

            CreateMap<LockUserViewModel, User>();
            CreateMap<User, LockUserViewModel>();

            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();
            CreateMap<EditDepartmentViewModel, Department>();

            CreateMap<Department, EditDepartmentViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.GetToShortName()));
            ;
            CreateMap<EmployeeLockReasonViewModel, EmployeeLockReason>();
            CreateMap<EditEmployeeLockReasonViewModel, EmployeeLockReason>();
            CreateMap<EmployeeLockReason, EmployeeLockReasonViewModel>();

            CreateMap<EmployeeLockReason, EditEmployeeLockReasonViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.GetToShortName()));

            CreateMap<Nationality, NationalityViewModel>();
            CreateMap<NationalityViewModel, Nationality>();
            CreateMap<EditNationalityViewModel, Nationality>();
            CreateMap<Nationality, EditNationalityViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                src => src.MapFrom(map =>
                    map.CreatedUser == null ? string.Empty : map.CreatedUser.GetToShortName()));

            CreateMap<Position, PositionViewModel>();
            CreateMap<PositionViewModel, Position>();
            CreateMap<EditPositionViewModel, Position>();
            CreateMap<Position, EditPositionViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.GetToShortName()));

            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<EditEmployeeViewModel, Employee>();
            CreateMap<Employee, EditEmployeeViewModel>();
            CreateMap<AllEmployeeViewModel, Employee>();
            CreateMap<Employee, AllEmployeeViewModel>()
                .ForMember(dst => dst.Position,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ForMember(dst => dst.Department,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => map.FullName()));

            CreateMap<AllLockEmployeeViewModel, Employee>();
            CreateMap<Employee, AllLockEmployeeViewModel>()
                .ForMember(dst => dst.Position,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ForMember(dst => dst.Department,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => map.FullName()))
                .ForMember( dst => dst.LockReason, 
                    src => src.MapFrom( 
                        map => map.EmployeeLockReason.Name))
                .ForMember(dst => dst.HireDate,
                    src => src.MapFrom(
                        map => map.HireDate.ToString("dd.MM.yyyy")))
                .ForMember(dst => dst.LockDate,
                    src => src.MapFrom(
                        map => map.LockDate.HasValue ? map.LockDate.Value.ToString("dd.MM.yyyy") : null))
                .ForMember(dst => dst.PhotoPath, opt => opt.Ignore())
                .ForMember(dst => dst.Phone, opt => opt.Ignore())
                .ForMember(dst => dst.Email, opt => opt.Ignore())
                .ForMember(dst => dst.UserId, opt => opt.Ignore());

            CreateMap<GetEmployeeViewModel, Employee>();
            CreateMap<Employee, GetEmployeeViewModel>()
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => map.FullName()))
                .ForMember(dst => dst.Department,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.Position,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ForMember(dst => dst.GenderName,
                    src => src.MapFrom(
                        map => map.Gender.Name))
                .ForMember(dst => dst.DateOfBirth,
                    src => src.MapFrom(
                        map => map.DateOfBirth.ToShortDateString()))
                .ForMember(dst => dst.HireDate,
                    src => src.MapFrom(
                        map => map.HireDate.ToString("dd.MM.yyyy")));

            CreateMap<GetPassportDataEmployeeViewModel, Employee>();
            CreateMap<Employee, GetPassportDataEmployeeViewModel>()
                .ForMember(dst => dst.Nationality,
                    src => src.MapFrom(
                        map => map.Nationality.Name))
                .ForMember(dst => dst.DateOfBirth,
                    src => src.MapFrom(
                        map => map.DateOfBirth.ToString("dd.MM.yyyy")))
                .ForMember(dst => dst.PassportIssueDate,
                    src => src.MapFrom(
                        map => map.PassportIssueDate.ToShortDateString()));

            // TODO
            CreateMap<NewsViewModel, News>();
            CreateMap<News, NewsViewModel>();
            CreateMap<EditNewsViewModel, News>();
            CreateMap<News, EditNewsViewModel>()
                //.ForMember( dst => dst.PublishAt, opt => opt.ConvertUsing<DateTimeFormatter, DateTime>())
                .ForMember(dst => dst.NewsCategoryName,
                    src => src.MapFrom(
                        map => map.NewsCategory.Name));

            CreateMap<ListNewsViewModel, News>();
            CreateMap<News, ListNewsViewModel>();

            CreateMap<NewsCategoriesViewModel, NewsCategory>();
            CreateMap<NewsCategory, NewsCategoriesViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.GetToShortName()));

            CreateMap<EmailSetting, EmailSettingViewModel>();
            CreateMap<EmailSettingViewModel, EmailSetting>();

            CreateMap<UserLockReasonViewModel, UserLockReason>();
            CreateMap<UserLockReason, UserLockReasonViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.GetToShortName()));

            CreateMap<Gender, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, Gender>();

        }
    }
}
