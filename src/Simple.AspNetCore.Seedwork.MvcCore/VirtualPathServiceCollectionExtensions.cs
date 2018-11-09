/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Simple.AspNetCore.Seedwork.MvcCore.Config;
using Simple.AspNetCore.Seedwork.MvcCore.StartupFilters;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VirtualPathServiceCollectionExtensions
    {
        public static void AddVirtualPath(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.Configure<VirtualPathOptions>(configuration);
            services.AddVirutalPath();
        }

        public static void AddVirtualPath(this IServiceCollection services, Action<VirtualPathOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddVirutalPath();
        }

        private static void AddVirutalPath(this IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, VirtualPathStartupFilter>();
        }
    }
}