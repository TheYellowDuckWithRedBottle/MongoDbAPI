using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Resource;
using MongoDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RealEstateController:Controller
    {
        private readonly RealEstateService _RealEstateService;
        private readonly IMapper _mapper;
        public RealEstateController(RealEstateService estateStaService,IMapper mapper)
        {
            _RealEstateService = estateStaService;
            _mapper = mapper;

        }

        [HttpGet]
        public ActionResult GetRealEstates(QueryParameter parameter)
        {
           var RealEstates = _RealEstateService.GetBuildings(parameter);

            return Ok(RealEstates);
        }
        [HttpGet]
        public ActionResult GetRealEstate([FromQuery] string NatbuildNo, [FromQuery]string RoomId)
        {
            //2-201      201
            // 3-301      301
            //18-01       1801
            if (RoomId.Contains('-'))
            {
                var RoomIdArray = RoomId.Split('-');
                if (RoomIdArray[1].ToString().Length == 2)
                {
                    RoomId = RoomIdArray[0] + RoomIdArray[1];
                }
                else
                {
                    RoomId = RoomIdArray[RoomIdArray.Length-1];
                }
            }
           
            //var index = RoomId.IndexOf('-');
            //RoomId= RoomId.Remove(index, 1);
            QueryParameter parameter = new QueryParameter { NatbuildNo = NatbuildNo, RoomId = RoomId };

            var Building = _RealEstateService.GetOneRealEstate(parameter);
            if (Building == null)
                return Ok(new BuildingResource());
            var BuildingRes = _mapper.Map<BuildingResource>(Building);
            
            return Ok(BuildingRes);
        }
    }
}
