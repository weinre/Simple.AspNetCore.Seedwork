/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using NLog.Web;

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
            var contentRootPath = env.ContentRootPath;
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;
            var logFileName = Path.Combine(applicationBasePath.Replace(contentRootPath, ""), defaultLogConfigFileName);
            logFileName = logFileName.IndexOf('\\') >= 0 ? logFileName.Substring(1, logFileName.Length - 1) : logFileName;
            env.ConfigureNLog(logFileName);
        }
    }
}