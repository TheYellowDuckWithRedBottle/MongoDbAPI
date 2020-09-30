using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using MongoDB.Bson;
using System.IO;
using System;
using MongoDB.Common;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cors;

namespace MongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class B3dmController:ControllerBase
    {
        private readonly B3dmService _b3dmService;
        public List<string> urlList { get; set; }
        public B3dmController(B3dmService b3dmService)
        {
            _b3dmService = b3dmService;
        }
        [HttpGet]
        [EnableCors("any")]
        public async Task<IActionResult> Get()
        
        {
            MemoryStream destination = await _b3dmService.downTileData("G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\tileset.json");
            //MemoryStream destination1 = await _b3dmService.downTileData("G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\0\\0.b3dm");
            //MemoryStream destination2 = await _b3dmService.downTileData("G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\0\\0\\0.b3dm");
            //MemoryStream destination3 = await _b3dmService.downTileData("G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\0\\0\\0\\0.b3dm");
            //MemoryStream destination4 = await _b3dmService.downTileData("G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\0\\0\\0\\0\\0.b3dm");
            //MemoryStream destination5 = await _b3dmService.downTileData("G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\0\\0\\0\\0\\0\\0.b3dm");
            //MemoryStream destination6 = await _b3dmService.downTileData("G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\0\\0\\0\\0\\0\\0\\0.b3dm");
            if (destination!=null&& destination.Length>0)
            {
                destination.Position = 0;
                StreamReader reader = new StreamReader(destination, true);
                string tilesetContnet = reader.ReadToEnd().ToJson();

                UrlHelper urlHelper = new UrlHelper(tilesetContnet);
                urlList=urlHelper.ExtractUrl(tilesetContnet);
                urlHelper.FixedUrl(urlList);

                
                foreach (var item in urlHelper.UrlList)
                {
                    destination = await _b3dmService.downTileData(item);
                    byte[] buffer = new byte[destination.Length];
                    reader = new StreamReader(destination,true);
                    var binary=reader.BaseStream.Read(buffer,0, buffer.Length);
                    File(buffer, "application/octet-stream");
                   
                }   
                return Ok(tilesetContnet);   
            }
            else
            {
              return  NotFound();
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> GetB3dm(List<string> urlList)
        //{
        //    if (urlList.Count != 0)
        //    {
        //        foreach (var item in urlList)
        //        {
        //            MemoryStream destination = await _b3dmService.downTileData(item);
        //            if (destination != null && destination.Length > 0)
        //            {
                        
        //                return Ok(destination);

        //            }
        //            else
        //            {
        //                return NotFound();
        //            }

        //        }
        //        return NoContent();
        //    }
        //    else
        //    {

        //        return NotFound();
        //    }
           
        //}
    }
}
