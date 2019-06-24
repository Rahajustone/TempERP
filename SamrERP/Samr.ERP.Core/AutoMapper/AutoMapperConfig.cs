using AutoMapper;
using Samr.ERP.Core.AutoMapper.AutoMapperProfiles;

namespace Samr.ERP.Core.AutoMapper
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
