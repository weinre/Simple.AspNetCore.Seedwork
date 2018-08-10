/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.WebApi.Seedwork.Extensions
{
    public static class WebApiCorsServiceCollectionExtensions
    {
        public const string CorPolicyName = "AllowCors";

        public static void AddWebApiCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(CorPolicyName, builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            }));
        }

        public static void UseWebApiCors(this IApplicationBuilder app)
        {
            app.UseCors(CorPolicyName);
        }
    }
}