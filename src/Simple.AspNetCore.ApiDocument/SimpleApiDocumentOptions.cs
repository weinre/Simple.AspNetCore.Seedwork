/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Simple.AspNetCore.Seedwork.ApiDocument
{
    public class SimpleApiDocumentOptions
    {
        public IEnumerable<(string name, Info info)> Docs { set; get; }

        public bool IsSupportApiVersion { set; get; }

        public bool IsSupportJWTToken { set; get; }

        public bool IsGlobalJWTToken { set; get; }
    }
}