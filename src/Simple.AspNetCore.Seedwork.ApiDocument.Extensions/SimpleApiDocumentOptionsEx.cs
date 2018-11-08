/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Simple.AspNetCore.Seedwork.ApiDocument.Extensions
{
    public class SimpleApiDocumentOptionsEx
    {
        public Func<IEnumerable<ApiDescription>, ApiDescription> Resolver { set; get; }
    }
}