using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Resource;
using MongoDB.Services;
using MongoDB.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Aspose.Cells;
using System.IO;

namespace MongoDB.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RealEstateController:Controller
    {
        private readonly RealEstateService _RealEstateService;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        public RealEstateController(RealEstateService estateStaService,IMapper mapper, IUrlHelper urlHelper)
        {
            _RealEstateService = estateStaService;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }
        /// <summary>
        /// 获取不动产信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>

        [HttpGet]
        public ActionResult GetRealEstateByHouseId([FromQuery] string RealEstateNo)
        {
            QueryParameter parameter = new QueryParameter { HouseHoldeID= RealEstateNo };
            var RealEstates = _RealEstateService.Get(RealEstateNo);
            if(RealEstates!=null)
            {
                return Ok(RealEstates);
            }
            else
            {
                return NotFound();
            }
        }

     
        /// <summary>
        /// 根据幢号和房间号查询不动产属性
        /// </summary>
        /// <param name="NatbuildNo">幢号</param>
        /// <param name="RoomId">栋号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRealEstate([FromQuery] string NatbuildNo, [FromQuery]string RoomId)
        {
            string LayerId="";
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
                    RoomId = RoomIdArray[RoomIdArray.Length-1];
                }
            }           
            QueryParameter parameter = new QueryParameter { NatbuildNo = NatbuildNo,FloLayer= LayerId, RoomId = RoomId };
            var Building = _RealEstateService.GetOneRealEstate(parameter);
            if (Building == null)
                return Ok(new BuildingResource());
            var BuildingRes = _mapper.Map<BuildingResource>(Building);
            
            return Ok(BuildingRes);
        }

        [HttpGet]
        public ActionResult GetRealEstateStaus()
        {
           
            var RealEstates = _RealEstateService.GetAll();
            List<ReturnStatusModel> BuildingRes=new List<ReturnStatusModel>();
         
            if (RealEstates != null) {
                foreach (var item in RealEstates)
                {

                    ReturnStatusModel returnStatusModel = new ReturnStatusModel();
                    if (item.CoverType==null)
                    {
                        returnStatusModel.Status = "0";
                    }
                    else
                    {
                        returnStatusModel.Status =item.CoverType;
                    }
                    returnStatusModel.BuildingId = item.NatbuildNo;
                    var index = item.RoomId.IndexOf(item.FloLayerId);
                    if (index == 0)
                    {
                        returnStatusModel.RoomId = item.RoomId.Insert(item.FloLayerId.Length, "-");
                    }
                    else
                    {
                        returnStatusModel.RoomId = item.FloLayerId + '-' + item.RoomId;
                    }
                    BuildingRes.Add(returnStatusModel);
                }
                
            }

            var res = BuildingRes.GroupBy(x => x.Status).Select(x => new ListStatus { Status = x.Key, RoomId = x.ToList() }).ToList();
            return Ok(res);
            
        }
        /// <summary>
        /// 获取保留属性的不动产信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBuildings([FromQuery] QueryParameter parameter)
        {

            SavedBuildingAttService savedBuildingAttService = new SavedBuildingAttService();
            SavedAttrController savedAttrController = new SavedAttrController(savedBuildingAttService);
            var strings = savedAttrController.GetAttribute();//获取到的属性值

            List<Dictionary<string, string>> listDic = new List<Dictionary<string, string>>();
            List<Attributes> listAtt = new List<Attributes>();

            var buildings = _RealEstateService.GetBuildings(parameter);
            foreach (var item in buildings)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var item1 in strings)
                {
                    if (item1.isShow)
                    {
                        var value = item.GetValue(item1.name);
                        dic.Add(item1.cname, value);
                    }
                }
                listDic.Add(dic);
            }
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
        [HttpPost]
        public ActionResult Upload([FromForm(Name = "file")] List<IFormFile> files)
        {
            List<Building> buildings = new List<Building>();
            files.ForEach(file =>
            {
                var fileName = file.FileName;

                string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);//获取文件名称后缀 
                                                                                                   //保存文件
                var stream = file.OpenReadStream();

                var buildingList = ReadExcel(stream);
                if (buildingList.Count != 0)
                {
                    buildings.AddRange(buildingList);
                }
            });
            _RealEstateService.CreateMany(buildings);
            return new OkResult();
        }
        private List<Building> ReadExcel(Stream stream)
        {
            var records = new List<Building>();
            Workbook workbook = new Workbook(stream);
            try
            {
                Worksheet sheet = workbook.Worksheets[0];
                var rowCount = sheet.Cells.Rows.Count;
                var columnCount = sheet.Cells.Columns.Count;
                for (int row = 1; row < rowCount; row++)
                {
                    var myRecord = new Building();
                    for (int column = 0; column < columnCount; column++)
                    {
                        myRecord.EstateUnitNo = sheet.Cells[row, column++].StringValue;
                        myRecord.NatbuildNo = sheet.Cells[row, column++].StringValue;
                        myRecord.LogicBuildNo = sheet.Cells[row, column++].StringValue;
                        myRecord.CoverId = sheet.Cells[row, column++].StringValue;
                        myRecord.UnitId = sheet.Cells[row, column++].DoubleValue;
                        myRecord.FloLayerId = sheet.Cells[row, column++].StringValue;
                        myRecord.NameId = sheet.Cells[row, column++].StringValue;
                        myRecord.RoomId = sheet.Cells[row, column++].StringValue;
                        myRecord.UnitName = sheet.Cells[row, column++].StringValue;
                        myRecord.UnitName = sheet.Cells[row, column++].StringValue;
                        myRecord.PowerType = sheet.Cells[row, column++].StringValue;
                        myRecord.RomPowCharacter = sheet.Cells[row, column++].StringValue;
                        myRecord.RomTyeStru = sheet.Cells[row, column++].StringValue;
                        myRecord.CoverType = sheet.Cells[row, column++].StringValue;
                        column = columnCount;
                    }
                    records.Add(myRecord);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            };
            return records;
        }
        private string CreateBuildingUri(PaginationBase parameters, PaginationResourceUriType_uriType uriType)
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
    }

  
}
