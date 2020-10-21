using Aspose.Cells;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MongoDB.Filter;
using MongoDB.Models;
using MongoDB.Resource;
using MongoDB.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MongoDB.Controllers
{
   //[ServiceFilter(typeof(TokenFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController:ControllerBase
    {
      
        private readonly EstateStaService _estateStaService;

        public HomeController(EstateStaService estateStaService)
        {
            _estateStaService = estateStaService;
        }
       [HttpPost]
       public ReturnModel GetList(string token)
        {
            var ret = new ReturnModel();
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            for (int i = 0; i < 1000; i++)
            {
                var data = new Dictionary<string, string>() { { "Wyy", "sily" + i } };
               list.Add(data);
            }
            ret.Code = 200;
            ret.Msg = "成功";
            ret.Data = list;
            ret.TnToken = null;
            return ret;
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
                if(buildingList.Count != 0)
                {
                    buildings.AddRange(buildingList);
                }
               
                //// 把 Stream 转换成 byte[] 
                //byte[] bytes = new byte[stream.Length];
                //stream.Read(bytes, 0, bytes.Length);
                //// 设置当前流的位置为流的开始 
                //stream.Seek(0, SeekOrigin.Begin);
                //// 把 byte[] 写入文件 
                //FileStream fs = new FileStream("D:\\" + file.FileName, FileMode.Create);
                //BinaryWriter bw = new BinaryWriter(fs);
                //bw.Write(bytes);
                //bw.Close();
                //fs.Close();

            });
            _estateStaService.CreateMany(buildings);
            return new OkResult();
        }

        private List<Building> ReadExcel(Stream stream)
        {
            Workbook workbook = new Workbook();
            try { workbook.LoadData(stream); }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            };
            
            Worksheet sheet = workbook.Worksheets[0];

            var rowCount = sheet.Cells.Rows.Count;
            var columnCount = sheet.Cells.Columns.Count;
            var records = new List<Building>();
            for (int row = 1; row < rowCount; row++)
            {
                var myRecord = new Building();
                for(int column=0;column<columnCount;column++)
                {
                    myRecord.EstateUnitNo = sheet.Cells[row, column++].StringValue;
                    myRecord.NatbuildNo = sheet.Cells[row, column++].StringValue;
                    myRecord.LogicBuildNo = sheet.Cells[row, column++].StringValue;
                    myRecord.CoverId = sheet.Cells[row, column++].StringValue;
                    myRecord.UnitId = sheet.Cells[row, column++].StringValue;
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
                    //myRecord.PreBuilArea = sheet.Cells[row, column++].StringValue;
                    //myRecord.PreInterArea = sheet.Cells[row, column++].DoubleValue;
                    //myRecord.PreSharedArea = sheet.Cells[row, column++].DoubleValue;
                  
                }
                records.Add(myRecord);

            }
            return records;
        }
    }
}
