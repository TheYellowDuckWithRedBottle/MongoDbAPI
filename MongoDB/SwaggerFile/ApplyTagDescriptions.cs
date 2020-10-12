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
                new OpenApiTag{Name="LoginTest",Description="获取登录token"},
                new OpenApiTag{Name="HomeTest",Description="测试相关信息"}
            };

        }
    }
}
