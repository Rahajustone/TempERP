using System;
using AutoMapper;
using Samr.ERP.Core.AutoMapper.AutoMapperProfiles;

namespace Samr.ERP.Core.AutoMapper
{

    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            Action<IMapperConfigurationExpression> expressionConfig = cfg =>
            {
                cfg.AddProfile(new EntityToViewModelProfile());
            };
            var mapperConfiguration = new MapperConfiguration(expressionConfig);

            //TODO надо получить instance через service
            Mapper.Initialize(expressionConfig);
            return mapperConfiguration;
        }

    }
}
