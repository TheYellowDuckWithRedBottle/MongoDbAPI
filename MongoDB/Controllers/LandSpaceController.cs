using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Resource;
using MongoDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MongoDB.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
    [ApiController]
    public class LandSpaceController:ControllerBase
    {
        private LandSpaceService _LandSpaceService;
        private List<LandSpace> landSpaces;
        public LandSpaceController(LandSpaceService landSpaceService)
        {
            this._LandSpaceService = landSpaceService;
            this.landSpaces = _LandSpaceService.GetAll();
        }
        /// <summary>
        /// 获取行政区地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getXZQ()
        {
            var res = landSpaces.GroupBy(item => item.QSDWMC).Select(x => x.Key).ToList();
            return Ok(res);
        }
        /// <summary>
        /// 获取权属性质代码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getQSXZCode()
        {
            var QSXZCodeList = landSpaces.GroupBy(x => x.QSXZ).Select(x => x.Key).ToList();
            var QSXZMC = new Dictionary<string, string>();
           
            foreach (var item in QSXZCodeList)
            {
                switch (item)
                {
                    case "10":
                        QSXZMC.Add(item, "国有土地所有权");
                        break;
                    case "20":
                        QSXZMC.Add(item, "国有土地使用权");
                        break;
                    case "30":
                        QSXZMC.Add(item, "集体土地所有权");
                        break;
                    case "40":
                        QSXZMC.Add(item, "集体土地使用权");
                        break;
                    default:
                        break;
                }
            }
            return Ok(QSXZMC);
        }

        /// <summary>
        /// 根据权属性质查询总的面积
        /// </summary>
        /// <returns>整个地区的权属性质的总面积</returns>
        [HttpGet]
        public ActionResult getStaticByQSXZ() {
            List<LandSpace> landSpaces = _LandSpaceService.GetAll();
            var res = landSpaces.GroupBy(x => x.QSXZ).Select(x => new cateArea { cateName = x.Key, area = x.Sum(t => t.SHAPE_Area) }).ToList();
            return Ok(res);
        }
        /// <summary>
        /// 获取整个区域的分类统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet] 
        public ActionResult getStaticByLocation()
        {
            List<LandSpace> landSpaces = _LandSpaceService.GetAll();
           var res= landSpaces.GroupBy(x => x.ZRZYFLMC).Select(x=>new cateArea {cateName=x.Key,area=x.Sum(t=>t.SHAPE_Area) }).ToList();
            return Ok(res);
        }
        /// <summary>
        /// 根据权属代码获取图斑分类
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getStaticDataByQSXZ([FromQuery]string code) 
        {
            var codeList = landSpaces.GroupBy(x => x.QSXZ).ToList();
            List<StaticXZQResult> staticXZQResultList = new List<StaticXZQResult>();
            foreach (var item in codeList)
            {
                StaticXZQResult staticXZQResults = new StaticXZQResult();
                var totalArea = item.Sum(x => x.SHAPE_Area);
                var res = item.GroupBy(x => x.ZRZYFLMC).Select(x => new cateArea
                {
                    cateName = x.Key,
                    area = x.Sum(t => t.SHAPE_Area),
                    percent=x.Sum(t=>t.SHAPE_Area)/totalArea
                }).ToList();
                staticXZQResults.XZQMC = item.Key;

                staticXZQResults.cateArea = res;
                staticXZQResultList.Add(staticXZQResults);
            }
            var result = staticXZQResultList.Find(item => item.XZQMC == code);
            return Ok(result);
        }
        /// <summary>
        /// 根据村庄名获取分类数据
        /// </summary>
        /// <param name="village"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getStaticFromLoaction([FromQuery]string village)
        {
            
            List<LandSpace> landSpaces= _LandSpaceService.GetAll();
            var xzqList = landSpaces.GroupBy(x => x.QSDWMC).ToList();//按照权属单位名称分为村子一类的列表
            List<StaticXZQResult> list = new List<StaticXZQResult>();
            foreach (var item in xzqList)
            {
                StaticXZQResult staticXZQResult = new StaticXZQResult();
                var totalArea = item.Sum(x => x.SHAPE_Area);//求村子的总面积
                var res= item.GroupBy(x => x.ZRZYFLMC).Select(x=> 
                new cateArea {
                    cateName=x.Key,
                    area=x.Sum(t=>t.SHAPE_Area),
                    percent=x.Sum(t=>t.SHAPE_Area)/totalArea*100 }).ToList();
                staticXZQResult.XZQMC = item.Key;

                staticXZQResult.cateArea = res;
                list.Add(staticXZQResult);
            }
            var result = list.Find(item => item.XZQMC == village);
            return Ok(result);
        }
        /// <summary>
        /// 根据标识码,获取图斑位置
        /// </summary>
        /// <param name="BSM"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getLocationByBSM(string BSM)
        {
            if (string.IsNullOrEmpty(BSM))
            {
                return  Ok(new ReturnModel() {Code=404,Msg="请输入正确参数",Data=null});
            }
           var Shape= landSpaces.Find(item => item.BSM == BSM);
            if (Shape == null)
                return NotFound(new ReturnModel() { Code = 404, Msg = "未查询到此条信息", Data = null });
            return Ok(new ReturnModel() {Code=200,Msg="获取成功",Data=new {  Shape.longitude, Shape.latitude  } });
          
        }
       
    }
}
