using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDB.SwaggerFile
{
    public class ApplyTagDescriptions : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                new OpenApiTag{Name="Login",Description="获取登录token"},
                new OpenApiTag{Name="Home",Description="测试相关信息"},
                new OpenApiTag{Name="B3dm",Description="获取3dtiles"},
                new OpenApiTag{Name="RealEstate",Description="获取不动产相关数据"},
                new OpenApiTag{Name="SavedAttr",Description="获取保存的属性信息"},
                new OpenApiTag{Name="User",Description="获取用户信息"},
                new OpenApiTag{Name="Tile",Description="获取影像信息"},
            };


        }
    }
}
