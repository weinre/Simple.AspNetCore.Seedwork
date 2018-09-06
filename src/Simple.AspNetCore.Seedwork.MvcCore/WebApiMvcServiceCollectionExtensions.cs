/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using AspNetCore.WebApi.Seedwork.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApiMvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddSimpleMvc(this IServiceCollection services)
        {
            return services
                .AddMvc(options => { options.Filters.Add<WebApiGlobalExceptionFilter>(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public static void UseSimpleMvc(
            this IApplicationBuilder app,
            IHostingEnvironment env,
            bool isUseHsts = false,
            bool isUseHttpsRedirection = false)
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