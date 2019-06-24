using AutoMapper;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Entities;

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
