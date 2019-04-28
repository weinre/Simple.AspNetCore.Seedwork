/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters
{
    public class AddAutoMatchVersionHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                IParameter versionParameter = operation.Parameters.SingleOrDefault(parameter => parameter.Name == "version");
                if (versionParameter is NonBodyParameter nonBodyParameter)
                {
                    nonBodyParameter.Description = "API 版本号";
                    var apiVersion = context.MethodInfo.DeclaringType.GetCustomAttributes<ApiVersionAttribute>(true).Single().Versions.Single();
                    nonBodyParameter.Default = apiVersion.ToString();
                }
            }
        }
    }
}