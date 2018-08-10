/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using AspNetCore.WebApi.Seedwork.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspNetCore.WebApi.Seedwork.Extensions
{
    public static class WebApiMvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddSimpleMvc(this IServiceCollection services)
        {
            return services
                .AddMvc(options => { options.Filters.Add<WebApiGlobalExceptionFilter>(); })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public static void UseSimpleMvc(this IApplicationBuilder app, IHostingEnvironment env, bool isUseHsts = false, bool isUseHttpsRedirection = false)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                if (isUseHsts)
                    app.UseHsts();
            }
            if (isUseHttpsRedirection)
                app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}