using AspNetCore.WebApi.Seedwork.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Simple.AspNetCore.Seedwork.Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSimpleApiDocument(options =>
            {
                options.Docs = new List<(string name, Info info)>
                {
                    ("v1.0", new Info
                    {
                        Title = "标题",
                        Description = "描述",
                        Version = "v1.0"
                    })
                };
                options.IsSupportApiVersion = true;
            });
            services.AddSimpleMvc(options => options.Filters.Add<WebApiGlobalExceptionFilter>());
            services.AddSimpleApiVersion();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseSimpleConfig(env);
            app.UseSimpleApiDocument();
            app.UseSimpleMvc();
        }
    }
}
