using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Dto;
using MongoDB.Models;
using MongoDB.Resource;
using MongoDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EquityController:Controller
    {
        private readonly EquityService _EquityService;
        private readonly IMapper _Mapper;
        public EquityController(EquityService equityService,IMapper mapper)
        {
            _EquityService = equityService;
            _Mapper = mapper;
        }
        [HttpGet]
        public IActionResult Equities()
        {
           var equities= _EquityService.Get();
            if (equities == null || equities.Count == 0) return Ok(new ReturnModel() { Code = 404, Msg = "未查找到数据", Data = null });
            return Ok(equities);
        }
        [HttpGet]
        public IActionResult Equity(string RealEstateNo)
        {
            if (string.IsNullOrEmpty(RealEstateNo))
                return Ok(new ReturnModel() { Code = 404, Msg = "查询参数为空", Data = null });
            var Equity= _EquityService.GetOne(new Models.QueryParameter() { EstateUnitNo = RealEstateNo });
            if (Equity == null) return Ok(new ReturnModel() { Code = 404, Msg = "查询参数为空", Data = null });
            var RetEquity= _Mapper.Map<ReturnEquity>(Equity);
            return Ok(RetEquity);
        }
        [HttpGet]
        public IActionResult EquityByRoomId([FromQuery] string NatbuildNo, [FromQuery] string RoomId)
        {
            string LayerId = "";
            if (RoomId.Contains('-'))
            {
                var RoomIdArray = RoomId.Split('-');
                LayerId = RoomIdArray[0];
                if (RoomIdArray[1].ToString().Length == 2)
                {
                    RoomId = RoomIdArray[0] + RoomIdArray[1];
                }
                else
                {
                    RoomId = RoomIdArray[RoomIdArray.Length - 1];
                }
            }
            QueryParameter parameter = new QueryParameter { NatbuildNo = NatbuildNo, FloLayer = LayerId, RoomId = RoomId };
            RealEstateService realEstateService = new RealEstateService();
            var Building = realEstateService.GetOneRealEstate(parameter);
            if (Building == null)
                return Ok(new BuildingResource());
            parameter.EstateUnitNo = Building.EstateUnitNo;
        
            var Equity=_EquityService.GetOne(parameter);
            if (Equity == null) return Ok(new ReturnModel() { Code = 404, Msg = "查询参数为空", Data = null });
            if (Equity.IsForecast == "否")
            {
                var RetEquity = _Mapper.Map<ReturnEquity>(Equity);
                return Ok(RetEquity);
            }
            else
            {
                var RetEquity = _Mapper.Map<ReturnEquityCopy>(Equity);
                return Ok(RetEquity);
            }
        }
    }
}
