/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleCorsServiceCollectionExtensions
    {
        public const string CorPolicyName = "AllowCors";

        public static void AddSimpleCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(CorPolicyName, builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            }));
        }

        public static void UseSimpleCors(this IApplicationBuilder app)
        {
            app.UseCors(CorPolicyName);
        }
    }
}