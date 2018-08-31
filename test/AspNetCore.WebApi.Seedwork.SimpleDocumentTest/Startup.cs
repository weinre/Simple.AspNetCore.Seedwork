using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSimpleApiVersion();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSimpleApiDocument();
            app.UseMvc();
        }
    }
}
