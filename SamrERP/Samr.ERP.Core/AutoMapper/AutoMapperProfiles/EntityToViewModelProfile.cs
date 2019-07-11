using System;
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
            CreateMap<User, UserViewModel>()
                .ForMember(dst => dst.IsLocked,
                    src => src.MapFrom(
                        map => map.UserLockReasonId.HasValue) )
                .ForMember(dst => dst.UserLockReasonName,
                    src => src.MapFrom(
                        map => map.UserLockReason.IfNotNull(p=>p.Name)))
                .ForMember(dst => dst.LockDate,
                    src => src.MapFrom(
                        map => map.LockDate.HasValue ? map.LockDate.Value.ToShortDateString() : null));
            CreateMap<UserViewModel, User>();

            CreateMap<LockUserViewModel, User>();
            CreateMap<User, LockUserViewModel>();

            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();
            CreateMap<EditDepartmentViewModel, Department>();

            CreateMap<Department, EditDepartmentViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()));

            CreateMap<Department, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, Department>();
            ;
            CreateMap<EmployeeLockReasonViewModel, EmployeeLockReason>();
            CreateMap<EditEmployeeLockReasonViewModel, EmployeeLockReason>();
            CreateMap<EmployeeLockReason, EmployeeLockReasonViewModel>();
            
            CreateMap<EmployeeLockReason, EditEmployeeLockReasonViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()));

            CreateMap<EmployeeLockReason, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, EmployeeLockReason>();

            CreateMap<Nationality, NationalityViewModel>();
            CreateMap<NationalityViewModel, Nationality>();
            CreateMap<EditNationalityViewModel, Nationality>();
            CreateMap<Nationality, EditNationalityViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                src => src.MapFrom(map =>
                    map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()));
            CreateMap<Nationality, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, Nationality>();

            CreateMap<Position, PositionViewModel>();
            CreateMap<PositionViewModel, Position>();
            CreateMap<EditPositionViewModel, Position>();
            CreateMap<Position, EditPositionViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()));

            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dst => dst.PhotoPath,
                src => src.MapFrom(
                    map => FileService.GetDownloadAction(map.PhotoPath)));
            CreateMap<EmployeeViewModel, Employee>();
            CreateMap<EditEmployeeViewModel, Employee>()
                .ForMember(dst => dst.PhotoPath,src => src.Ignore());
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
                .ForMember(dst => dst.PhotoPath,
                    src => src.MapFrom(
                        map => FileService.GetDownloadAction(FileService.GetResizedPath(map.PhotoPath))))
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
                .ForMember(dst => dst.UserId, opt => opt.Ignore());

            CreateMap<GetEmployeeViewModel, Employee>();
            CreateMap<Employee, GetEmployeeViewModel>()
                .ForMember(dst => dst.FullName,
                    src => src.MapFrom(
                        map => map.FullName()))
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
                .ForMember(dst => dst.LockReasonName,
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
                .ForMember(dst => dst.PhotoPath,
                    src => src.MapFrom(map => FileService.GetDownloadAction(map.PhotoPath)));

            CreateMap<GetPassportDataEmployeeViewModel, Employee>();
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
                        map => map.DateOfBirth.ToShortDateString()));

            CreateMap<EditPassportDataEmployeeViewModel, Employee>();
            CreateMap<Employee, EditPassportDataEmployeeViewModel>();

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
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()));

            CreateMap<EmailSetting, EmailSettingViewModel>();
            CreateMap<EmailSettingViewModel, EmailSetting>();

            CreateMap<UserLockReasonViewModel, UserLockReason>();
            CreateMap<UserLockReason, UserLockReasonViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()));

            CreateMap<UserLockReason, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, UserLockReason>();

            CreateMap<Gender, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, Gender>();

        }
    }
}
