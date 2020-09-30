using BooksApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Extension;
using MongoDB.Services;

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
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();
            //app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
