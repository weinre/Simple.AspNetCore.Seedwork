/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Simple.AspNetCore.Seedwork.ApiDocument.Extensions
{
    public static class ApiDescriptionConflictResolver
    {
        public static ApiDescription Resolve(IEnumerable<ApiDescription> descriptions)
        {
            var parameters = descriptions
                .SelectMany(desc => desc.ParameterDescriptions)
                .GroupBy(x => x, (x, xs) => new { IsOptional = xs.Count() == 1, Parameter = x }, ApiParameterDescriptionEqualityComparer.Instance)
                .ToList();
            var description = descriptions.First();
            description.ParameterDescriptions.Clear();
            parameters.ForEach(x =>
            {
                if (x.Parameter.RouteInfo != null)
                    x.Parameter.RouteInfo.IsOptional = x.IsOptional;
                description.ParameterDescriptions.Add(x.Parameter);
            });
            return description;
        }
    }
}