/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters
{
    public class AddActionAuthorizeDescriptionOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            IList<string> authorizeAttributes = context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true)
                .Select(auth => auth.Roles.Split(','))
                .Select(roles => roles.Length == 1 ? roles[0] : string.Join("、", roles))
                .ToList();
            if (authorizeAttributes.Count == 0)
            {
                authorizeAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes<AuthorizeAttribute>(true)
                    .Select(auth => string.IsNullOrEmpty(auth.Roles) ? new string[] { } : auth.Roles.Split(','))
                    .Select(roles => roles.Length == 1 ? roles[0] : string.Join("、", roles))
                    .ToList();
            }
            var description = authorizeAttributes.Any()
                ? $"需要{(authorizeAttributes.Count > 0 ? string.Join("和", authorizeAttributes) : authorizeAttributes[0])}权限才能访问"
                : "不需要权限验证";
            if (!string.IsNullOrEmpty(operation.Description))
                operation.Description += $"\r\n{description}";
            else
                operation.Description = description;
        }
    }
}