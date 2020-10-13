using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.JWT;
using MongoDB.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.Filter
{
    public class TokenFilter : Attribute, IActionFilter
    {
        private ITokenHelper tokenHelper;
        public TokenFilter(ITokenHelper _tokenHelper) 
        {
            tokenHelper = _tokenHelper;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            ReturnModel ret = new ReturnModel();
            //获取token
            //object tokenobj = context.ActionArguments["token"];
            //前端地址栏参数传参
            object tokenobj = context.HttpContext.Request.Headers["token"].ToString();//前端写在header里面获取的
            if (tokenobj == null)
            {
                ret.Code = 201;
                ret.Msg = "token不能为空";
                context.Result = new JsonResult(ret);
                return;
            }
            string token = tokenobj.ToString();

            string userId = "";
            //验证jwt,同时取出来jwt里边的用户ID
            TokenType tokenType = tokenHelper.ValiTokenState(token, a => a["iss"] == "WYY" && a["aud"] == "EveryTestOne", action => { userId = action["loginID"]; });
            if (tokenType == TokenType.Fail)
            {
                ret.Code = 202;
                ret.Msg = "token验证失败";
                context.Result = new JsonResult(ret);
                return;
            }
            if (tokenType == TokenType.Expired)
            {
                ret.Code = 205;
                ret.Msg = "token已经过期";
                context.Result = new JsonResult(ret);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                //给控制器传递参数(需要什么参数其实可以做成可以配置的，在过滤器里边加字段即可)
                context.ActionArguments.Add("userId", Convert.ToString(userId));
            }
        }
    }

}
