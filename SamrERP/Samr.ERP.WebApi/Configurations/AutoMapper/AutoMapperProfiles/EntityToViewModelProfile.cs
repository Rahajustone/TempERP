using AutoMapper;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.ViewModels.Account;
using Samr.ERP.WebApi.ViewModels.Employee;

namespace Samr.ERP.WebApi.Configurations.AutoMapper.AutoMapperProfiles
{
    public class EntityToViewModelProfile:Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Employee, AddEmployeeViewModel>();
            CreateMap<AddEmployeeViewModel, Employee>();
        }
    }
}
