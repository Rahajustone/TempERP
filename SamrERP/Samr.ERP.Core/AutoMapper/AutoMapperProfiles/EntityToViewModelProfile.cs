using AutoMapper;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.AutoMapper.AutoMapperProfiles
{
    public class EntityToViewModelProfile:Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<Employee, AddEmployeeViewModel>();
            CreateMap<AddEmployeeViewModel, Employee>();
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();
            CreateMap<EditDepartmentViewModel, Department>();
            CreateMap<Department, EditDepartmentViewModel>();
        }
    }
}
