/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Simple.AspNetCore.Seedwork.MvcCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApiMvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddSimpleMvc(this IServiceCollection services)
        {
            return services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public static IMvcBuilder AddSimpleMvc(this IServiceCollection services, Action<MvcOptions> setupAction)
        {
            return services
                .AddMvc(setupAction)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public static void UseSimpleConfig(
            this IApplicationBuilder app,
            IHostingEnvironment env,
            Action<SimpleConfigOptions> configOptions = null)
        {
            var simpleConfigOptions = new SimpleConfigOptions();
            configOptions?.Invoke(simpleConfigOptions);
            if (simpleConfigOptions.IsSupportDeveloperExceptionPage && env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (simpleConfigOptions.IsSupportHttps)
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
        }

        public static void UseSimpleMvc(this IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}