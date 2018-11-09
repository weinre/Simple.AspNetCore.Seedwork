/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Simple.AspNetCore.Seedwork.MvcCore.StartupFilters
{
    public class VirtualPathOptions : IOptions<VirtualPathOptions>
    {
        public string BasePath { set; get; }

        public bool IsUseEnvironmentVariable { set; get; }

        public VirtualPathOptions Value { get; }
    }

    public class VirtualPathStartupFilter : IStartupFilter
    {
        public const string DefaultEnvironmentVariableValue = "ASPNETCORE_BASEPATH";
        private readonly VirtualPathOptions _options;

        public VirtualPathStartupFilter(IOptions<VirtualPathOptions> options)
        {
            _options = options.Value;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                var basePath = _options.IsUseEnvironmentVariable
                    ? Environment.GetEnvironmentVariable(DefaultEnvironmentVariableValue)
                    : _options.BasePath;
                if (!string.IsNullOrEmpty(basePath))
                {
                    app.Use(async (context, next2) =>
                    {
                        context.Request.PathBase = basePath;
                        await next2.Invoke();
                    });
                    next(app);
                }
            };
        }
    }
}