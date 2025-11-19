using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using PrazskaLitacka.Webapi.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.WebApi.Tests.Extensions;
public static class MapperExtension
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, NullLoggerFactory.Instance);

        var mapper = config.CreateMapper();
        return mapper;
    }
}
