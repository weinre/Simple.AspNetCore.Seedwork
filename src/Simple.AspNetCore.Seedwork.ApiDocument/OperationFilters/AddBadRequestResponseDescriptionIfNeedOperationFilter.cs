/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Net;

namespace Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters
{
    public class AddBadRequestResponseDescriptionIfNeedOperationFilter : IOperationFilter
    {
        private readonly string _description;

        public AddBadRequestResponseDescriptionIfNeedOperationFilter(string description = "参数验证失败")
        {
            _description = description;
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            var statusCode = Convert.ToInt32(HttpStatusCode.BadRequest).ToString();
            if (!operation.Responses.ContainsKey(statusCode))
                operation.Responses.Add(statusCode, new Response { Description = _description });
        }
    }
}