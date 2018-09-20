/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Simple.AspNetCore.Seedwork.ApiDocument;
using Simple.AspNetCore.Seedwork.ApiDocument.OperationFilters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleApiDocumentServiceCollectionExtensions
    {
        public static void AddSimpleApiDocument(this IServiceCollection services, Action<SimpleApiDocumentOptions> setupAction)
        {
            if (setupAction == null)
                throw new ArgumentNullException();
            var simpleApiDocumentOptions = new SimpleApiDocumentOptions();
            setupAction(simpleApiDocumentOptions);
            services.AddSingleton(simpleApiDocumentOptions);
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                foreach (var (name, info) in simpleApiDocumentOptions.Docs)
                {
                    options.SwaggerDoc(name, info);
                }
                if (!string.IsNullOrWhiteSpace(simpleApiDocumentOptions.IncludeXmlCommentsFilePath))
                    options.IncludeXmlComments(simpleApiDocumentOptions.IncludeXmlCommentsFilePath, true);
                options.OperationFilter<AddActionAuthorizeDescriptionOperationFilter>();
                options.OperationFilter<Add400StatusCodeResponseDescriptionIfNeedOperationFilter>();
                options.OperationFilter<Remove200StatusCodeResponseDescriptionIfNotNeedOperationFilter>();
                if (simpleApiDocumentOptions.IsSupportApiVersion)
                {
                    options.DocInclusionPredicate((docName, apiDesc) =>
                    {
                        if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                            return false;
                        var versions = methodInfo.DeclaringType
                            .GetCustomAttributes(true)
                            .OfType<ApiVersionAttribute>()
                            .SelectMany(attr => attr.Versions)
                            .ToList();
                        return versions.Any(v => $"v{v.ToString()}" == docName);
                    });
                    options.OperationFilter<AddAutoMatchVersionHeaderParameterOperationFilter>();
                }
                if (simpleApiDocumentOptions.IsSupportJWTToken)
                {
                    if (simpleApiDocumentOptions.IsGlobalJWTToken)
                    {
                        options.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "请输入带有Bearer的Token", Name = "Authorization", Type = "apiKey" });
                        options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                            {
                                {
                                    "Bearer",
                                    Enumerable.Empty<string>()
                                }
                            });
                    }
                    else
                    {
                        options.OperationFilter<AddJWTTokenHeaderParameterOperationFilter>();
                    }
                    options.OperationFilter<AddAuthorizeResponseDescriptionOperationFilter>();
                }
            });
        }

        public static void UseSimpleApiDocument(this IApplicationBuilder app, string virtualDirectory = default)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                        swaggerDoc.BasePath = virtualDirectory);
                });
                var simpleApiDocumentOptions = serviceScope.ServiceProvider.GetRequiredService<SimpleApiDocumentOptions>();
                app.UseSwaggerUI(options =>
                {
                    foreach (var (name, _) in simpleApiDocumentOptions.Docs)
                    {
                        options.SwaggerEndpoint($"{virtualDirectory}/swagger/{name}/swagger.json", name);
                    }
                });
            }
        }
    }
}