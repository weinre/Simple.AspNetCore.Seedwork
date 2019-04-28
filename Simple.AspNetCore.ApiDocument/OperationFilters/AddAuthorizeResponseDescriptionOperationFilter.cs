/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters
{
    public class AddAuthorizeResponseDescriptionOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes<AuthorizeAttribute>(true)
                .Select(attr => attr.Policy)
                .Concat(context.MethodInfo.DeclaringType
                    .GetCustomAttributes<AuthorizeAttribute>(true)
                    .Select(attr => attr.Policy))
                .ToList();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized(jwt token不正确)" });
                operation.Responses.Add("403", new Response { Description = "Forbidden(jwt token正确，但权限不够)" });
            }
        }
    }
}