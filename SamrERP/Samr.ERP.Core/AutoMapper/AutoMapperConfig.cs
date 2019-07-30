using System;
using AutoMapper;
using Samr.ERP.Core.AutoMapper.AutoMapperProfiles;

namespace Samr.ERP.Core.AutoMapper
{

    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            void ExpressionConfig(IMapperConfigurationExpression cfg)
            {
                cfg.AddProfile(new EntityToViewModelProfile());
            }

            var mapperConfiguration = new MapperConfiguration((Action<IMapperConfigurationExpression>) ExpressionConfig);

            //TODO надо получить instance через service
#pragma warning disable 618
            Mapper.Initialize((Action<IMapperConfigurationExpression>) ExpressionConfig);
#pragma warning restore 618
            return mapperConfiguration;
        }

    }
}
