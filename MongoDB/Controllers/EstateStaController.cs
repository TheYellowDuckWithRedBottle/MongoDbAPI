using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Common;
using MongoDB.Extension;
using MongoDB.Filter;
using MongoDB.Models;
using MongoDB.Resource;
using MongoDB.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Controllers
{
   // [ServiceFilter(typeof(TokenFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EstateStaController:ControllerBase
    {
        private readonly EstateStaService _estateStaService;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        public EstateStaController(EstateStaService estateStaService,IMapper mapper,IUrlHelper urlHelper)
        {
            _estateStaService = estateStaService;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }
        [HttpGet(Name ="GetBuildings")]
        [EnableCors("_myAllowSpecificOrigins")]
        public ActionResult GetBuildings([FromQuery] QueryParameter parameter)
        {

            SavedBuildingAttService savedBuildingAttService = new SavedBuildingAttService();
            SavedAttrController savedAttrController = new SavedAttrController(savedBuildingAttService);
            var strings = savedAttrController.GetAttribute();//获取到的属性值
       
            List<Dictionary<string,string>> listDic = new List<Dictionary<string, string>>();
            List<Attributes> listAtt = new List<Attributes>();
           
            var buildings = _estateStaService.GetBuildings(parameter);
            foreach (var item in buildings)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var item1 in strings)
                {
                    if(item1.isShow)
                    {
                        var value = item.GetValue(item1.name);
                        dic.Add(item1.cname, value);
                    }                               
                }
               
                listDic.Add(dic);

            }
          
               // var properties=type.GetProperties();
               //var query1=from building in buildings select (building.AppLandArea,building.AreaBuilding);
               //var query2=query1.ToList();
               // var buildingResource = _mapper.Map<List<BuildingResource>>(buildings);


               // Type t = ClassHelper.BuildType("MyClass");
               // var propertys= type.GetProperties();
               //Type newType = typeof(DynamicUpdateAtt);
               //var property= newType.GetProperties();
              // var returnRes = _mapper.Map<DynamicUpdateAtt> (buildings);
            var preLink = buildings.HasPrevious ? CreateBuildingUri(parameter, PaginationResourceUriType_uriType.pagePre) : null;
            var nextLink = buildings.HasNext ? CreateBuildingUri(parameter, PaginationResourceUriType_uriType.pageNext) : null;
            var mate = new
            {
                buildings.TotalItemsCount,
                buildings.PaginationBase.PageSize,
                buildings.PaginationBase.PageIndex,
                buildings.PageCount,
                PreviousPageLink = preLink,
                NextPageLink = nextLink
            };
           
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(mate));

            return Ok(listDic);
        }
        [HttpGet(Name ="GetBuilding")]  
        public ActionResult<Building> GetBuilding([FromQuery]string HouseholdID)
        {
            if(HouseholdID == null)
            {
                var buildingList = _estateStaService.Get();
                return Ok(buildingList);
            }
            else
            {
                var building = _estateStaService.Get(HouseholdID);
              
                if (building == null)
                {
                    return NotFound();
                }
                var buildingResource = _mapper.Map<BuildingResource>(building);
                return Ok(buildingResource);
            }
           
        }
   

        [HttpPost]
        public ActionResult<Building> Create([FromForm]BuildingAddResource buildingAdd)
        {
            if(buildingAdd == null)
            {
                return BadRequest();
            }
            var building = _mapper.Map<Building>(buildingAdd);
           var buildingReturn= _estateStaService.Create(building);
            var router= CreatedAtRoute("GetBuilding",new { HouseholdID = building.HouseholdID.ToString() }, buildingReturn);
            return router;
        }

        [HttpPut("{EstateUnitNo:length(28)}")]
        public IActionResult Update(string EstateUnitNo,Building building)
        {
            var building1 = _estateStaService.Get(EstateUnitNo);
            if(building1==null)
            {
                return NotFound();
            }
            _estateStaService.Update(EstateUnitNo, building);
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete(string EstateUnitNo)
        {
            var building1 = _estateStaService.Get(EstateUnitNo);
            if(building1==null)
            {
                return NotFound();
            }
            _estateStaService.Remove(building1.EstateUnitNo);
            return NoContent();
        }

        private string CreateBuildingUri(PaginationBase parameters,PaginationResourceUriType_uriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType_uriType.pagePre:
                    var previousParameters = parameters.Clone();
                    previousParameters.PageIndex--;
                    return _urlHelper.Link("GetBuildings", previousParameters);
                        
                case PaginationResourceUriType_uriType.pageCurren:
                    var currentParameters = parameters.Clone();
                    return _urlHelper.Link("GetBuildings", currentParameters);
                   
                case PaginationResourceUriType_uriType.pageNext:
                    var nextParameters = parameters.Clone();
                    nextParameters.PageIndex++;
                   return _urlHelper.Link("GetBuildings", nextParameters);
                   
                default:
                    return _urlHelper.Link("GetBuildings", parameters);
            }
        }

       
        private  Type GenerateDynamicClass(params string[] MID)
        {
            List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
            foreach (var item in MID)
            {
                ClassHelper.CustPropertyInfo cpi = new ClassHelper.CustPropertyInfo("System.String.", item);
                lcpi.Add(cpi);
            }

           Type type= ClassHelper.AddProperty(typeof(DynamicUpdateAtt), lcpi);
            return type;
        }
    }

}
