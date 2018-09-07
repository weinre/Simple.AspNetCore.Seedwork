/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters
{
    public class Add400StatusCodeResponseDescriptionIfNeedOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            ApiControllerAttribute apiController = context.MethodInfo.DeclaringType.GetCustomAttribute<ApiControllerAttribute>(true);
            if (apiController != null)
            {
                operation.Responses.Add("400", new Response { Description = "参数验证失败" });
            }
        }
    }
}