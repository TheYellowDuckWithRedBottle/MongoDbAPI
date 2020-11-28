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
       
    }
}
