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
            //CreateMap<Building, ReturnStatusModel>().ForMember(d=>d.RoomId,opt=>opt.MapFrom(s=>s.FloLayerId.IndexOf(s.CoverType)==1?s.)
            //    .ForMember(d=>d.Status,opt=>opt.MapFrom(s=>s.CoverType));
            CreateMap<Equity, ReturnEquity>();
            CreateMap<Equity, ReturnEquityCopy>();
        }
    }
}
