﻿using System;
using System.IO.Compression;
using System.Security.AccessControl;
using AutoMapper;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.FileArchive;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason;
using Samr.ERP.Core.ViewModels.Handbook.FileCategory;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;
using Samr.ERP.Core.ViewModels.Handbook.NewCategories;
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;
using Samr.ERP.Core.ViewModels.News;
using Samr.ERP.Core.ViewModels.Notification;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Core.ViewModels.UsefulLink;
using Samr.ERP.Core.ViewModels.UsefulLink.UsefulLinkCategory;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.AutoMapper.AutoMapperProfiles
{
    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dst => dst.IsLocked,
                    src => src.MapFrom(
                        map => map.UserLockReasonId.HasValue))
                .ForMember(dst => dst.UserLockReasonName,
                    src => src.MapFrom(
                        map => map.UserLockReason.IfNotNull(p => p.Name)))
                .ForMember(dst => dst.LockDate,
                    src => src.MapFrom(
                        map => map.LockDate.HasValue ? map.LockDate.Value.ToShortDateString() : null))
                .ForMember(dst => dst.LockUserFullName,
                    src => src.MapFrom(
                        map => map.LockUser != null ? map.LockUser.UserName : string.Empty))
                .ReverseMap();

            CreateMap<LockUserViewModel, User>()
                .ReverseMap();

            CreateMap<Employee, GetEmployeeDataViewModel>()
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => map.FullName()))
                .ForMember(dst => dst.DepartmentName,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.PositionName,
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
                        map => map.HireDate.ToShortDateString()))
                .ForMember(dst => dst.PhotoPath,
                    src => src.MapFrom(map => FileService.GetDownloadAction(map.PhotoPath)))
                .ForMember(dst => dst.PhotoPathResized,
                    src => src.MapFrom(map => FileService.GetDownloadAction(FileService.GetResizedPath(map.PhotoPath))))
                .ReverseMap();

            CreateMap<Department, DepartmentViewModel>()
                .ReverseMap();

            CreateMap<Department, EditDepartmentViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ReverseMap();

            CreateMap<Department, SelectListItemViewModel>()
                .ReverseMap();

            CreateMap<EmployeeLockReason, EmployeeLockReasonViewModel>()
                .ReverseMap();

            CreateMap<EmployeeLockReason, EditEmployeeLockReasonViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());

            CreateMap<SelectListItemViewModel, EmployeeLockReason>()
                .ReverseMap();

            CreateMap<Nationality, NationalityViewModel>();
            CreateMap<NationalityViewModel, Nationality>();
            CreateMap<Nationality, EditNationalityViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                src => src.MapFrom(map =>
                    map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ForMember(dst => dst.CreatedAt, src => src.MapFrom(
                   map => map.CreatedAt.ToShortDateString()))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());

            CreateMap<Nationality, SelectListItemViewModel>()
                .ReverseMap();

            CreateMap<Position, PositionViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ReverseMap();
            CreateMap<Position, EditPositionViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ForMember(dst => dst.DepartmentName,
                    src => src.MapFrom(
                        map => map.Department.Name))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore())
                .ForMember( dst => dst.Department, opt => opt.Ignore());


            #region Employee

            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dst => dst.PhotoPath,
                src => src.MapFrom(
                    map => FileService.GetDownloadAction(map.PhotoPath)));
            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<EditEmployeeViewModel, Employee>()
                .ForMember(dst => dst.PhotoPath, src => src.Ignore());
            CreateMap<Employee, EditEmployeeViewModel>()
                .ForMember(dst => dst.PhotoPath,
                    src => src.MapFrom(
                        map => FileService.GetDownloadAction(map.PhotoPath)));
            CreateMap<AllEmployeeViewModel, Employee>();
            CreateMap<Employee, AllEmployeeViewModel>()
                .ForMember(dst => dst.Position,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ForMember(dst => dst.Department,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.HasUser,
                    src => src.MapFrom(
                        map => map.User != null))
                //.ForMember(dst => dst.PhotoPath
                //src => src.MapFrom(
                //map => FileService.GetDownloadAction(FileService.GetResizedPath(map.PhotoPath))))
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => $"{map.LastName} {map.FirstName} {map.MiddleName}"))
                .ForMember(dst => dst.HasAccount, src => src.MapFrom(
                    map => map.UserId.HasValue))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());
            //map.FullName()));

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
                .ForMember(dst => dst.LockReason,
                    src => src.MapFrom(
                        map => map.EmployeeLockReason.Name))
                .ForMember(dst => dst.HireDate,
                    src => src.MapFrom(
                        map => map.HireDate.ToShortDateString()))
                .ForMember(dst => dst.LockDate,
                    src => src.MapFrom(
                        map => map.LockDate.HasValue ? map.LockDate.Value.ToShortDateString() : null))
                .ForMember(dst => dst.PhotoPath, opt => opt.Ignore())
                .ForMember(dst => dst.Phone, opt => opt.Ignore())
                .ForMember(dst => dst.Email, opt => opt.Ignore())
                .ForMember(dst => dst.UserId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<GetEmployeeViewModel, Employee>();
            CreateMap<Employee, GetEmployeeViewModel>()
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => $"{map.LastName} {map.FirstName} {map.MiddleName}"))
                .ForMember(dst => dst.MiddleName,
                src => src.MapFrom(
                        map => $"{ map.MiddleName }"))
                .ForMember(dst => dst.Department,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.DepartmentId,
                    src => src.MapFrom(
                        map => map.Position.Department.Id))
                .ForMember(dst => dst.Position,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ForMember(dst => dst.GenderName,
                    src => src.MapFrom(
                        map => map.Gender.Name))
                .ForMember(dst => dst.IsLocked,
                    src => src.MapFrom(
                        map => map.EmployeeLockReasonId.HasValue))
                .ForMember(dst => dst.EmployeeLockReasonName,
                    src => src.MapFrom(
                        map => map.EmployeeLockReason.Name))
                .ForMember(dst => dst.LockDate,
                    src => src.MapFrom(
                        map => map.LockDate.HasValue ? map.LockDate.Value.ToShortDateString() : null))
                .ForMember(dst => dst.DateOfBirth,
                    src => src.MapFrom(
                        map => map.DateOfBirth.ToShortDateString()))
                .ForMember(dst => dst.HireDate,
                    src => src.MapFrom(
                        map => map.HireDate.ToShortDateString()))
                .ForMember(dst => dst.LockUserFullName,
                    src => src.MapFrom(
                        map => map.LockUser != null ? map.LockUser.UserName : string.Empty))
                .ForMember(dst => dst.PhotoPath,
                    src => src.MapFrom(map => FileService.GetDownloadAction(FileService.GetResizedPath(map.PhotoPath))))
                .ForMember(dst => dst.PhotoPathMax,
                    src => src.MapFrom(map => FileService.GetDownloadAction(map.PhotoPath)));

            CreateMap<Employee, GetEmployeeCardTemplateViewModel>()
                .IncludeBase<Employee, GetEmployeeViewModel>()
                .ForMember(dst => dst.PhotoPath,
                    src => src.MapFrom(map => FileService.GetFullPath(FileService.GetResizedPath(map.PhotoPath))));

            CreateMap<Employee, GetPassportDataEmployeeViewModel>()
                .ForMember(dst => dst.Nationality,
                    src => src.MapFrom(
                        map => map.Nationality.IfNotNull(p => p.Name)))
                .ForMember(dst => dst.PassportIssueDate,
                    src => src.MapFrom(
                        map => map.PassportIssueDate.HasValue ? map.PassportIssueDate.Value.ToShortDateString() : null))
                .ForMember(dst => dst.PassportScanPath,
                    src => src.MapFrom(map => FileService.GetDownloadAction(map.PassportScanPath)))
                .ForMember(dst => dst.DateOfBirth,
                    src => src.MapFrom(
                        map => map.DateOfBirth.ToShortDateString()))
                .ReverseMap();

            CreateMap<EditPassportDataEmployeeViewModel, Employee>()
                .ReverseMap();

            CreateMap<Employee, EmployeeInfoTokenViewModel>()
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => map.FullName()))
                .ForMember(dst => dst.Photo, src => src.MapFrom(
                   map => FileService.GetDownloadAction(FileService.GetResizedPath(map.PhotoPath))))
                .ForMember(dst => dst.PositionName,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());

            CreateMap<Employee, ExportExcelViewModel>()
                .ForMember(dst => dst.PositionName,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ForMember(dst => dst.DepartmentName,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => map.FullName()))
                .ForMember(dst => dst.HasAccount, src => src.MapFrom(
                    map => map.UserId.HasValue ? "Да" : "Нет"))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Employee, SelectListItemViewModel>()
                .ForMember(dst => dst.Id, src => src.MapFrom(
                    map => map.UserId))
                .ForMember(dst => dst.Name, src => src.MapFrom(
                    map => $"{map.FullName()}" + "(" + $"{map.Phone}" + ")"))
                .ReverseMap();
            #endregion

            CreateMap<NewsViewModel, News>()
                .ReverseMap();
            CreateMap<News, EditNewsViewModel>()
                .ForMember(dst => dst.NewsCategoryName,
                    src => src.MapFrom(
                        map => map.NewsCategory.Name))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore())
                .ForMember(dst => dst.NewsCategory, opt => opt.Ignore());

            CreateMap<ListNewsViewModel, News>();
            CreateMap<News, ListNewsViewModel>();

            CreateMap<NewsCategory, NewsCategoriesViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());

            CreateMap<EmailSetting, EmailSettingViewModel>();
            CreateMap<EmailSettingViewModel, EmailSetting>();

            CreateMap<UserLockReason, UserLockReasonViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());

            CreateMap<UserLockReason, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, UserLockReason>();

            CreateMap<Gender, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, Gender>();

            CreateMap<UsefulLinkCategory, UsefulLinkCategoryViewModel>();
            CreateMap<UsefulLinkCategoryViewModel, UsefulLinkCategory>();
            CreateMap<UsefulLinkCategory, EditUsefulLinkCategoryViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());
            CreateMap<UsefulLinkCategory, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, UsefulLinkCategory>();

            CreateMap<UsefulLink, UsefulLinkViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ReverseMap()
                .ForMember(dst => dst.UsefulLinkCategory, opt => opt.Ignore());

            CreateMap<Handbook, HandbookViewModel>()
                .ForMember(dst => dst.LastModifiedAt, src => src.MapFrom(
                   map => map.LastModifiedAt.HasValue ? map.LastModifiedAt.Value.ToString("dd.MM.yyyy H:mm") : null))
                .ReverseMap();

            CreateMap<FileCategory, FileCategoryViewModel>().ReverseMap();
            CreateMap<FileCategory, EditFileCategoryViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(
                        map => map.CreatedUser.UserName))
                .ForMember(dst => dst.CreatedAt, src => src.MapFrom(
                   map => map.CreatedAt.ToShortDateString()))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore());
            CreateMap<FileCategory, SelectListItemViewModel>();

            CreateMap<FileArchive, EditFileArchiveViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(
                        map => map.CreatedUser.UserName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToShortDateString()));
            CreateMap<EditFileArchiveViewModel, FileArchive>();
            CreateMap<FileCategory, FileArchiveViewModel>()
                .ReverseMap();
            CreateMap<FileArchive, SelectListItemViewModel>().ReverseMap();

            CreateMap<NotificationSystemViewModel, Notification>().ReverseMap();
            CreateMap<CreateMessageViewModel, Notification>()
                .ForMember(dst => dst.NotificationType, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
