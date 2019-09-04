using System;
using System.IO.Compression;
using System.Security.AccessControl;
using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Asn1.Ocsp;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.FileArchive;
using Samr.ERP.Core.ViewModels.Handbook.EmployeeLockReason;
using Samr.ERP.Core.ViewModels.Handbook.FileArchiveCategory;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;
using Samr.ERP.Core.ViewModels.Handbook.NewCategories;
using Samr.ERP.Core.ViewModels.Handbook.UsefulLinkCategory;
using Samr.ERP.Core.ViewModels.Handbook.UserLockReason;
using Samr.ERP.Core.ViewModels.Message;
using Samr.ERP.Core.ViewModels.News;
using Samr.ERP.Core.ViewModels.Position;
using Samr.ERP.Core.ViewModels.SMPPSetting;
using Samr.ERP.Core.ViewModels.UsefulLink;
using Samr.ERP.Infrastructure.Entities;
using SixLabors.ImageSharp.ColorSpaces.Companding;

namespace Samr.ERP.Core.AutoMapper.AutoMapperProfiles
{
    public class EntityToViewModelProfile : Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap(typeof(Source<>), typeof(Destination<>));

            #region Log

            CreateMap<EmailMessageHistory, EmailMessageHistoryLogViewModel>()
                .ForMember(dst => dst.ReceiverUser,
                    src => src.MapFrom(map => Extension.FullNameToString(map.ReceiverUser.Employee.LastName,
                                                  map.ReceiverUser.Employee.FirstName,
                                                  map.ReceiverUser.Employee.MiddleName)
                                                  + " (" + map.ReceiverUser.PhoneNumber + ")"))
                .ForMember(dst => dst.EmailSettingId,
                    src => src.MapFrom(
                        map => map.EmailSetting.Id))
                .ForMember(dst => dst.SenderEmail,
                    src => src.MapFrom(
                        map => map.EmailSetting.Sender))
                .ForMember(dst => dst.CreatedAt, 
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<Department, DepartmentLog>()
                .ForMember(dst => dst.DepartmentId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Department, opt => opt.Ignore());

            CreateMap<DepartmentLog, DepartmentLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<Position, PositionLog>()
                .ForMember(dst => dst.PositionId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Position, opt => opt.Ignore());
            CreateMap<PositionLog, PositionLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<Nationality, NationalityLog>()
                .ForMember(dst => dst.NationalityId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Nationality, opt => opt.Ignore());
            CreateMap<NationalityLog, NationalityLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<Employee, EmployeeLog>()
                .ForMember(dst => dst.EmployeeId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Employee, opt => opt.Ignore());
            CreateMap<EmployeeLog, EmployeeLogViewModel>()
                .ForMember(dst => dst.Position,
                    src => src.MapFrom(
                        map => map.Position.Name))
                .ForMember(dst => dst.Department,
                    src => src.MapFrom(
                        map => map.Position.Department.Name))
                .ForMember(dst => dst.HasUser,
                    src => src.MapFrom(
                        map => map.User != null))
                .ForMember(dst => dst.HasAccount, src => src.MapFrom(
                    map => map.UserId.HasValue))
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName));

            CreateMap<EmployeeLockReason, EmployeeLockReasonLog>()
                .ForMember(dst => dst.EmployeeLockReasonId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.EmployeeLockReason, opt => opt.Ignore());
            CreateMap<EmployeeLockReasonLog, EmployeeLockReasonLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<UserLockReason, UserLockReasonLog>()
                .ForMember(dst => dst.UserLockReasonId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.UserLockReason, opt => opt.Ignore());
            CreateMap<UserLockReasonLog, UserLockReasonLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<NewsCategory, NewsCategoryLog>()
                .ForMember(dst => dst.NewsCategoryId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.NewsCategory, opt => opt.Ignore());
            CreateMap<NewsCategoryLog, NewsCategoryLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<UsefulLinkCategory, UsefulLinkCategoryLog>()
                .ForMember(dst => dst.UsefulLinkCategoryId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.UsefulLinkCategory, opt => opt.Ignore());
            CreateMap<UsefulLinkCategoryLog, UsefulLinkCategoryLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<FileArchiveCategory, FileArchiveCategoryLog>()
                .ForMember(dst => dst.FileCategoryId, src => src.MapFrom(map => map.Id))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.FileArchiveCategory, opt => opt.Ignore());
            CreateMap<FileArchiveCategoryLog, FileArchiveCategoryLogViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            #endregion

            CreateMap<User, UserViewModel>()
                .ForMember(dst => dst.IsLocked,
                    src => src.MapFrom(
                        map => map.UserLockReasonId.HasValue))
                .ForMember(dst => dst.UserLockReasonName,
                    src => src.MapFrom(
                        map => map.UserLockReason.IfNotNull(p => p.Name)))
                .ForMember(dst => dst.LockDate,
                    src => src.MapFrom(
                        map => map.LockDate.Value.ToStringCustomFormat()))
                .ForMember(dst => dst.LockUserFullName,
                    src => src.MapFrom(
                        map => Extension.FullNameToString(map.UserLockReason.CreatedUser.Employee.LastName,
                            map.UserLockReason.CreatedUser.Employee.FirstName,
                            map.UserLockReason.CreatedUser.Employee.MiddleName)))
                .ReverseMap();

            CreateMap<LockUserViewModel, User>()
                .ReverseMap();

            CreateMap<User, SelectListItemViewModel>()
                .ForMember(dst => dst.Name, src => src.MapFrom(
                    map => map.Employee.FullName()  + $" ({map.Employee.Phone})"));

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

            #region Department

            CreateMap<RequestDepartmentViewModel, ResponseDepartmentViewModel>();

            CreateMap<RequestDepartmentViewModel, Department>();

            CreateMap<Department, ResponseDepartmentViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            // TODO
            CreateMap<Department, DepartmentViewModel>()
                .ReverseMap();

            CreateMap<Department, EditDepartmentViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToShortDateString()));

            CreateMap<Department, SelectListItemViewModel>();
            #endregion

            #region EmployeeLockReason

            CreateMap<RequestEmployeeLockReasonViewModel, ResponseEmployeeLockReasonViewModel>();

            CreateMap<RequestEmployeeLockReasonViewModel, EmployeeLockReason>();

            CreateMap<EmployeeLockReason, ResponseEmployeeLockReasonViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<EmployeeLockReason, SelectListItemViewModel>();

            #endregion

            #region Nationality

            CreateMap<RequestNationalityViewModel, ResponseNationalityViewModel>();

            CreateMap<RequestNationalityViewModel, Nationality>();

            CreateMap<Nationality, ResponseNationalityViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<Nationality, SelectListItemViewModel>();

            #endregion

            #region Position

            CreateMap<RequestPositionViewModel, ResponsePositionViewModel>();

            CreateMap<RequestPositionViewModel, Position>();

            CreateMap<Position, ResponsePositionViewModel>()
                .ForMember( p => p.DepartmentName, 
                    src => src.MapFrom(
                        map => map.Department.Name))
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.DepartmentName,
                    src => src.MapFrom(
                        map => map.Department.Name))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));
             
            CreateMap<Position, SelectListItemViewModel>();
            #endregion

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
                        map => Extension.FullNameToString(map.LastName, map.FirstName, map.MiddleName)))
                //.ForMember(dst => dst.HasAccount, src => src.MapFrom(
                //    map => map.UserId.HasValue))
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
                        map => Extension.FullNameToString(map.LastName, map.FirstName, map.MiddleName)))
                .ForMember(dst => dst.MiddleName,
                src => src.MapFrom(
                        map =>  map.MiddleName ))
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
                        map => map.LockDate.HasValue ? map.LockDate.Value.ToStringCustomFormat() : null))
                .ForMember(dst => dst.DateOfBirth,
                    src => src.MapFrom(
                        map => map.DateOfBirth.ToShortDateString()))
                .ForMember(dst => dst.HireDate,
                    src => src.MapFrom(
                        map => map.HireDate.ToShortDateString()))
                .ForMember(dst => dst.LockUserFullName,
                    src => src.MapFrom(
                        map =>
                            Extension.FullNameToString(map.LockUser.Employee.LastName,
                                map.LockUser.Employee.FirstName, map.LockUser.Employee.MiddleName)))
                .ForMember(dst => dst.PhotoPath,
                    src => src.MapFrom(map => FileService.GetDownloadAction(FileService.GetResizedPath(map.PhotoPath))))
                .ForMember(dst => dst.PhotoPathMax,
                    src => src.MapFrom(map => FileService.GetDownloadAction(map.PhotoPath)))
                .ForMember(dst => dst.CreateUserFullName, 
                    src => src.MapFrom(
                    map => Extension.FullNameToString(map.CreatedUser.Employee.LastName,
                        map.CreatedUser.Employee.FirstName, map.CreatedUser.Employee.MiddleName)))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

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
            #endregion

            CreateMap<NewsViewModel, News>()
                .ReverseMap();
            CreateMap<News, EditNewsViewModel>()
                .ForMember(dst => dst.NewsCategoryName,
                    src => src.MapFrom(
                        map => map.NewsCategory.Name))
                .ForMember(dst => dst.ImagePath, src => src.MapFrom(
                    map => FileService.GetDownloadAction(map.Image)))
                .ForMember(dst => dst.CreatedAt, 
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()))
                .ForMember(dst => dst.PublishAt,
                    src => src.MapFrom(
                        map => map.PublishAt.ToStringCustomFormat()))
                .ForMember(dest => dest.Author, 
                    src => src.MapFrom(
                        map => new MiniProfileViewModel
                        {
                            EmployeeId = map.CreatedUser.Employee.Id.ToString(),
                            FullName = Extension.FullNameToString(map.CreatedUser.Employee.LastName, map.CreatedUser.Employee.FirstName, map.CreatedUser.Employee.MiddleName),
                            PhotoPath = FileService.GetDownloadAction(map.CreatedUser.Employee.PhotoPath),
                            PositionName = map.CreatedUser.Employee.Position.Name
                        }))
                .ReverseMap()
                .ForMember(dst => dst.CreatedAt, opt => opt.Ignore())
                .ForMember(dst => dst.NewsCategory, opt => opt.Ignore());
            
            CreateMap<ListNewsViewModel, News>();
            CreateMap<News, ListNewsViewModel>();

            //CreateMap<News, SelectListItemViewModel>()
            //    .ForMember(dst => dst.Id, src => src.MapFrom(
            //        map => map.NewsCategory.Id))
            //    .ForMember(dst => dst.Name, src => src.MapFrom(
            //        map => map.NewsCategory.Name))
            //    .ForMember(dst => dst.ItemsCount, src => src.MapFrom(
            //        map => Count(map.NewsCategory)));

            #region NewsCategory

            CreateMap<RequestNewsCategoryViewModel, ResponseNewsCategoryViewModel>();

            CreateMap<RequestNewsCategoryViewModel, NewsCategory>();

            CreateMap<NewsCategory, ResponseNewsCategoryViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<NewsCategory, SelectListItemViewModel>();

            #endregion



            CreateMap<EmailSetting, EmailSettingViewModel>()
                .ForMember( dst => dst.SSL, src => src.MapFrom(map => map.EnabledSSL));
            CreateMap<EmailSettingViewModel, EmailSetting>()
                .ForMember(dst => dst.EnabledSSL, src => src.MapFrom(map => map.SSL));

            #region UserLockReason

            CreateMap<RequestUserLockReasonViewModel, ResponseUserLockReasonViewModel>();

            CreateMap<RequestUserLockReasonViewModel, UserLockReason>();

            CreateMap<UserLockReason, ResponseUserLockReasonViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<UserLockReason, SelectListItemViewModel>();
            #endregion


            CreateMap<Gender, SelectListItemViewModel>();
            CreateMap<SelectListItemViewModel, Gender>();

            #region UsefulLinkCategory

            CreateMap<RequestUsefulLinkCategoryViewModel, ResponseUsefulLinkCategoryViewModel>();

            CreateMap<RequestUsefulLinkCategoryViewModel, UsefulLinkCategory>();

            CreateMap<UsefulLinkCategory, ResponseUsefulLinkCategoryViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));
            CreateMap<UsefulLinkCategory, SelectListItemViewModel>();
            #endregion


            CreateMap<UsefulLink, UsefulLinkViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.ToShortName()))
                .ForMember(dst => dst.UsefulLinkCategoryName,
                    src => src.MapFrom( map => map.UsefulLinkCategory.Name))
                .ForMember(dst => dst.CreatedAt, src => src.MapFrom(
                    map => map.CreatedAt.ToShortDateString()))
                .ForMember(dest => dest.Author,
                    src => src.MapFrom(
                        map => new MiniProfileViewModel
                        {
                            EmployeeId = map.CreatedUser.Employee.Id.ToString(),
                            FullName = Extension.FullNameToString(map.CreatedUser.Employee.LastName, map.CreatedUser.Employee.FirstName,map.CreatedUser.Employee.MiddleName),
                            PhotoPath = FileService.GetDownloadAction(map.CreatedUser.Employee.PhotoPath),
                            PositionName = map.CreatedUser.Employee.Position.Name
                        }))
                .ReverseMap()
                .ForMember(dst => dst.UsefulLinkCategory, opt => opt.Ignore());

            #region FileArchiveCategory

            CreateMap<RequestFileArchiveCategoryViewModel, ResponseFileArchiveCategoryViewModel>();
            CreateMap<RequestFileArchiveCategoryViewModel, FileArchiveCategory>();

            CreateMap<FileArchiveCategory, ResponseFileArchiveCategoryViewModel>()
                .ForMember(dst => dst.FirstName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.FirstName))
                .ForMember(dst => dst.LastName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.LastName))
                .ForMember(dst => dst.MiddleName,
                    src => src.MapFrom(
                        map => map.CreatedUser.Employee.MiddleName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));
            CreateMap<FileArchiveCategory, SelectListItemViewModel>();
            #endregion

            CreateMap<FileArchive, EditFileArchiveViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(
                        map => map.CreatedUser.UserName))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToShortDateString()))
                .ForMember(dst => dst.FileCategoryName,
                    src => src.MapFrom(
                        map => map.FileArchiveCategory.Name))
                .ForMember(dst => dst.FileName,
                    src => src.MapFrom(
                        map => map.Title + "" + System.IO.Path.GetExtension(map.FilePath)))
                .ForMember(dest => dest.Author,
                    src => src.MapFrom(
                        map => new MiniProfileViewModel
                        {
                            EmployeeId = map.CreatedUser.Employee.Id.ToString(),
                            FullName = Extension.FullNameToString(map.CreatedUser.Employee.LastName, map.CreatedUser.Employee.FirstName, map.CreatedUser.Employee.MiddleName),
                            PhotoPath = FileService.GetDownloadAction(map.CreatedUser.Employee.PhotoPath),
                            PositionName = map.CreatedUser.Employee.Position.Name
                        }));
            CreateMap<EditFileArchiveViewModel, FileArchive>()
                .ForMember( dst => dst.FilePath, opt => opt.Ignore());
            CreateMap<FileArchiveCategory, FileArchiveViewModel>()
                .ReverseMap();
            CreateMap<FileArchive, SelectListItemViewModel>().ReverseMap();

            CreateMap<Notification, GetSenderMessageViewModel>()
                .ForMember(dest => dest.User,
                    src => src.MapFrom(
                        map => new MiniProfileViewModel
                        {
                            EmployeeId = map.ReceiverUser.Employee.Id.ToString(),
                            PhotoPath = map.ReceiverUser.Employee.PhotoPath,
                            FullName = Extension.FullNameToString(map.ReceiverUser.Employee.LastName, map.ReceiverUser.Employee.FirstName, map.ReceiverUser.Employee.MiddleName),
                            PositionName = map.ReceiverUser.Employee.Position.Name
                        }))
                .ForMember(dst => dst.ReadDate,
                    src => src.MapFrom(
                        map => map.ReadDate.HasValue ? map.ReadDate.Value.ToStringCustomFormat() : null))
                .ForMember(dst => dst.CreatedAt,
                    src => src.MapFrom(
                        map => map.CreatedAt.ToStringCustomFormat()));

            CreateMap<Notification, SenderMessageViewModel>()
                .IncludeBase<Notification, GetSenderMessageViewModel>()
                .ForMember(dst => dst.Body, opt => opt.Ignore());

            CreateMap<Notification, GetReceiverMessageViewModel>()
                .IncludeBase<Notification, GetSenderMessageViewModel>()
                .ForMember(dest => dest.User,
                    src => src.MapFrom(
                        map => new MiniProfileViewModel
                        {
                            EmployeeId = map.SenderUser.Employee.Id.ToString(),
                            PhotoPath = map.SenderUser.Employee.PhotoPath,
                            FullName = Extension.FullNameToString(map.SenderUser.Employee.LastName,
                                map.SenderUser.Employee.FirstName, map.SenderUser.Employee.MiddleName),
                            PositionName = map.SenderUser.Employee.Position.Name
                        }));
            CreateMap<Notification, ReceiverMessageViewModel>()
                .IncludeBase<Notification, GetReceiverMessageViewModel>()
                .ForMember(dst => dst.Body, opt => opt.Ignore());

            CreateMap<CreateMessageViewModel, Notification>();

            CreateMap<Notification, NotifyMessageViewModel>()
                .ForMember(dest => dest.Message,
                    src => src.MapFrom(
                        map => new SenderMessageViewModel
                        {
                            Id = map.Id,
                            ReadDate = map.ReadDate.ToString(),
                            SenderUserId = map.SenderUserId,
                            ReceiverUserId = map.ReceiverUserId,
                            CreatedAt = map.CreatedAt.ToShortDateString(),
                            Title = map.Title,
                            User = new MiniProfileViewModel
                            {
                                EmployeeId = map.ReceiverUser.Employee.Id.ToString(),
                                PhotoPath = FileService.GetDownloadAction(FileService.GetResizedPath(map.ReceiverUser.Employee.PhotoPath)),
                                FullName = Extension.FullNameToString(map.ReceiverUser.Employee.LastName,
                                    map.ReceiverUser.Employee.FirstName, map.ReceiverUser.Employee.MiddleName),
                                PositionName = map.ReceiverUser.Employee.Position.Name
                            }
                        }));

            CreateMap<User, MiniProfileViewModel>();

            CreateMap<SMPPSetting, SMPPSettingViewModel>();
            CreateMap<SMPPSettingViewModel, SMPPSetting>();

            CreateMap<SMPPSetting, SMPPSettingResponseViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(
                        map => map.CreatedUser.UserName))
                .ForMember(dst => dst.CreatedAt, 
                    src => src.MapFrom(
                        map => map.CreatedAt.ToShortDateString()));

            CreateMap<SMPPSettingViewModel, SMPPSettingResponseViewModel>().ReverseMap();
        }
    }
}
