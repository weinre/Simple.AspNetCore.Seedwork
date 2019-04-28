/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters
{
    public class RemoveOkResponseDescriptionIfNotNeedOperationFilter : IOperationFilter
    {
        private IEnumerable<ProducesResponseTypeAttribute> GetRequiredScopes(OperationFilterContext context, int statusCode)
        {
            IEnumerable<ProducesResponseTypeAttribute> requiredScopes = context
                .MethodInfo
                .GetCustomAttributes<ProducesResponseTypeAttribute>(true)
                .Where(attr => attr.StatusCode == statusCode)
                .Distinct();
            return requiredScopes;
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            var statusCode = Convert.ToInt32(HttpStatusCode.OK);
            if (!GetRequiredScopes(context, statusCode).Any())
            {
                operation.Responses.Remove(statusCode.ToString());
            }
        }
    }
}