using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Extension;
using MongoDB.Models;
using MongoDB.Resource;

namespace MongoDB.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Building, DynamicUpdateAtt>();
            CreateMap<Building, BuildingResource>();
            CreateMap<BuildingResource, Building>();
            CreateMap<Building, BuildingAddResource>();
            CreateMap<BuildingAddResource, Building>();
            
        }
    }
}
