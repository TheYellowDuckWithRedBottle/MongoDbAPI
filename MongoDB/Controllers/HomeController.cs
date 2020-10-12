using Microsoft.AspNetCore.Mvc;
using MongoDB.Filter;
using MongoDB.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Controllers
{
    [ServiceFilter(typeof(TokenFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController:ControllerBase
    {
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
