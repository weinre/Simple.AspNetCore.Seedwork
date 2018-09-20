/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleLogServiceCollectionExtensions
    {
        public static void AddSimpleNLog(this IServiceCollection services, IConfiguration configuration, string defaultLoggingConfigKey = "Logging")
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection(defaultLoggingConfigKey));
            });
        }

        public static void UseSimpleNLog(this ILoggerFactory loggerFactory, IHostingEnvironment env, string defaultLogConfigFileName = "nlog.config")
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog(defaultLogConfigFileName);
        }
    }
}