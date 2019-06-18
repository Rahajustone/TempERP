using AutoMapper;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.ViewModels.Account;

namespace Samr.ERP.WebApi.Configurations.AutoMapper.AutoMapperProfiles
{
    public class EntityToViewModelProfile:Profile
    {
        public EntityToViewModelProfile()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}
