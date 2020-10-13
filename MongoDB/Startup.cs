using BooksApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Extension;
using MongoDB.Filter;
using MongoDB.JWT;
using MongoDB.Services;
using MongoDB.SwaggerFile;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;

namespace MongoDB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().AddMvcOptions(options =>
            //{
            //   // options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

            //});
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.1", new OpenApiInfo
                {
                    Version = "v1.1",
                    Title = "测试接口文档",
                    Description = "测试接口"
                });
                // 为 Swagger 设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DocInclusionPredicate((docName, description) => true);
                //添加对控制器的标签(描述)
                c.DocumentFilter<ApplyTagDescriptions>();//显示类名
                c.CustomSchemaIds(type => type.FullName);// 可以解决相同类名会报错的问题
                //c.OperationFilter<AuthTokenHeaderParameter>();
            });
            services.AddTransient<ITokenHelper, TokenHelper>();
            //读取配置文件配置的jwt相关配置
            services.Configure<JWTConfig>(Configuration.GetSection("JWTConfig"));
            //启用JWT
            services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();
            //注册自定义的token标签
           services.AddScoped<TokenFilter>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContent = factory.GetService<IActionContextAccessor>()
                .ActionContext;
                return new UrlHelper(actionContent);
            });
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.Configure<PngstoreDatabaseSettings>(Configuration.GetSection(nameof(PngstoreDatabaseSettings)));
            services.AddSingleton<IPngstoreDatabaseSettings>(sp => sp.GetRequiredService<IOptions<PngstoreDatabaseSettings>>().Value);
            services.AddSingleton<UserService>();
            services.AddSingleton<TileService>();
            services.AddSingleton<SavedBuildingAttService>();
            services.AddSingleton<B3dmService>();
            services.AddSingleton<EstateStaService>();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .WithExposedHeaders("X - Pagination")
                    .AllowAnyOrigin()
                    
                    .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS");
                });
            });
            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseHttpsRedirection();
            app.Use(next => {
                return async httpContext =>
                {
                    await next(httpContext);
                };
            });
            app.UseRouting();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Web App v1.1");
                c.RoutePrefix = string.Empty;//设置根节点访问
                c.DocExpansion(DocExpansion.None);//折叠
                c.DefaultModelsExpandDepth(-1);//不显示Schemas
            });
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();
            //app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
