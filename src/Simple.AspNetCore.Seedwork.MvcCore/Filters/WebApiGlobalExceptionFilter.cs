/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspNetCore.WebApi.Seedwork.Filters
{
    public class WebApiGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerFactory _loggerFactory;

        public WebApiGlobalExceptionFilter(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void OnException(ExceptionContext context)
        {
            var logger = _loggerFactory.CreateLogger(context.Exception.TargetSite.ReflectedType);
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception != null ? context.Exception.StackTrace : nameof(WebApiGlobalExceptionFilter));
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}