using AutoMapper;
using Samr.ERP.WebApi.Configurations.AutoMapper.AutoMapperProfiles;

namespace Samr.ERP.WebApi.Configurations.AutoMapper
{

    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToViewModelProfile());
            });
        }

    }
}
