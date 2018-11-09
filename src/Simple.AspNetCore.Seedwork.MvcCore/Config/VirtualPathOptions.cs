/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.Extensions.Options;

namespace Simple.AspNetCore.Seedwork.MvcCore.Config
{
    public class VirtualPathOptions : IOptions<VirtualPathOptions>
    {
        public string BasePath { set; get; }

        public bool IsUseEnvironmentVariable { set; get; }

        public VirtualPathOptions Value { get; }
    }
}