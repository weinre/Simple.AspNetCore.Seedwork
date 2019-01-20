/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Simple.AspNetCore.Seedwork.ApiDocument
{
    public class SimpleApiDocumentOptions
    {
        public IEnumerable<(string name, Info info)> Docs { set; get; }

        public string IncludeXmlCommentsFilePath { set; get; }

        public Func<IEnumerable<ApiDescription>, ApiDescription> Resolver { set; get; }

        public bool IsSupportApiVersion { set; get; }

        public bool IsSupportJWTToken { set; get; }

        public bool IsGlobalJWTToken { set; get; }
    }
}