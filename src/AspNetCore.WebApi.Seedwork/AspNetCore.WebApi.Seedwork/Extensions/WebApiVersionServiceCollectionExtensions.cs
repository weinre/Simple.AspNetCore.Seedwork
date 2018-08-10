/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.WebApi.Seedwork.Extensions
{
    public static class WebApiVersionServiceCollectionExtensions
    {
        public static void AddWebApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;//这是可选的。但是, 当设置为 true 时, API 将返回响应标头中支持的版本信息。
                option.AssumeDefaultVersionWhenUnspecified = true;//此选项将用于不提供版本的请求。默认情况下, 假定的 API 版本为1.0。
                option.DefaultApiVersion = new ApiVersion(1, 0);//此选项用于指定在请求中未指定版本时要使用的默认 API 版本。这将默认版本为1.0。
            });
        }
    }
}