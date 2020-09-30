using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
