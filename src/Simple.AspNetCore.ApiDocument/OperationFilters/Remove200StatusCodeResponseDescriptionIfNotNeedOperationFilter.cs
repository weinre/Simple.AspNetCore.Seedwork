/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters
{
    public class Remove200StatusCodeResponseDescriptionIfNotNeedOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes<SwaggerResponseAttribute>(true)
                .Where(attr => attr.StatusCode == 200)
                .Distinct();
            if (!requiredScopes.Any())
            {
                operation.Responses.Remove("200");
            }
        }
    }
}