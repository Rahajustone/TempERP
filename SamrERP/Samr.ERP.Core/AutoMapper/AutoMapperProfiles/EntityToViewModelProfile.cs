using AutoMapper;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Core.ViewModels.Handbook.Nationality;
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
            CreateMap<Employee, EditEmployeeViewModel>();
            CreateMap<EditEmployeeViewModel, Employee>();
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

            CreateMap<NewsCategoriesViewModel, NewsCategory>();
            CreateMap<NewsCategory, NewsCategoriesViewModel>()
                .ForMember(dst => dst.CreatedUserName,
                    src => src.MapFrom(map =>
                        map.CreatedUser == null ? string.Empty : map.CreatedUser.GetToShortName()));
        }
    }
}
