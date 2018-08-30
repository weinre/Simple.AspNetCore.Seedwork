/* ***********************************************
 * author:  奔跑的牛排
 * email:   beefsteak@live.cn  
 * ***********************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebApiDocumentServiceCollectionExtenions
    {
        public static void AddSimpleDocument(this IServiceCollection services, IEnumerable<(string name, Info info)> docInfos)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                foreach (var docInfo in docInfos)
                {
                    options.SwaggerDoc(docInfo.name, docInfo.info);
                }

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace}.xml");
                options.IncludeXmlComments(filePath);

                if (docInfos.Count() > 1)
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
                    options.OperationFilter<AddAutoVersionHeaderParamterOperationFilter>();
                }

                options.OperationFilter<AddActionAuthorizeDescriptionOperationFilter>();
                //options.OperationFilter<AddAuthTokenHeaderParameterOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.OperationFilter<Remove200StatuCodeIfNotNeedOperationFilter>();

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "请输入带有Bearer的Token", Name = "Authorization", Type = "apiKey" });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {
                        "Bearer",
                        Enumerable.Empty<string>()
                    }
                });
            });
        }

        public static void UseSimpleDocument(this IApplicationBuilder app, string virtualDirectory = "", IEnumerable<string> names = null)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = virtualDirectory);
            });
            app.UseSwaggerUI(c =>
            {
                if (names == null)
                    c.SwaggerEndpoint($"{virtualDirectory}/swagger/v1/swagger.json", "v1");
                else
                {
                    foreach (var name in names)
                    {
                        c.SwaggerEndpoint($"{virtualDirectory}/swagger/{name}/swagger.json", name);
                    }
                }
            });
        }
    }

    public class AddActionAuthorizeDescriptionOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            IList<string> authorizeAttributes = context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true)
                .Select(auth => auth.Roles.Split(','))
                .Select(auths => auths.Length == 1 ? auths[0] : string.Join("、", auths))
                .ToList();
            if (authorizeAttributes.Count == 0)
            {
                authorizeAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes<AuthorizeAttribute>(true)
                    .Select(auth => string.IsNullOrEmpty(auth.Roles) ? new string[] { } : auth.Roles.Split(','))
                    .Select(auths => auths.Length == 1 ? auths[0] : string.Join("、", auths))
                    .ToList();
            }
            var description = authorizeAttributes.Any()
                ? $"需要{(authorizeAttributes.Count > 0 ? string.Join('和', authorizeAttributes) : authorizeAttributes[0])}权限才能访问"
                : "不需要权限验证";
            if (!string.IsNullOrEmpty(operation.Description))
                operation.Description += $"\r\n{description}";
            else
                operation.Description = description;
        }
    }

    public class AddAutoVersionHeaderParamterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                IParameter versionParameter = operation.Parameters.SingleOrDefault(parameter => parameter.Name == "version");
                if (versionParameter is NonBodyParameter nonBodyParameter)
                {
                    nonBodyParameter.Description = "WebApi版本号";
                    //context.SchemaRegistry.Definitions.

                    var apiVersion = context.MethodInfo.DeclaringType.GetCustomAttributes<ApiVersionAttribute>(true).Single().Versions.Single();
                    nonBodyParameter.Default = apiVersion.ToString();
                    //versionParameter.Extensions.Add("Default", "1");
                }
            }
        }
    }

    //public class AddAuthTokenHeaderParameterOperationFilter : IOperationFilter
    //{
    //    public void Apply(Operation operation, OperationFilterContext context)
    //    {
    //        if (operation.Parameters == null)
    //        {
    //            operation.Parameters = new List<IParameter>();
    //        }
    //        operation.Parameters.Add(new NonBodyParameter
    //        {
    //            Name = "Authorization",
    //            In = "header",
    //            Type = "apiKey",
    //            Description = "请输入带有Bearer的Token",
    //            Required = false
    //        });
    //    }
    //}

    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes<AuthorizeAttribute>(true)
                .Select(attr => attr.Policy)
                .Concat(context.MethodInfo.DeclaringType
                    .GetCustomAttributes<AuthorizeAttribute>(true)
                    .Select(attr => attr.Policy))
                .ToList();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized(jwt token不正确)" });
                operation.Responses.Add("403", new Response { Description = "Forbidden(jwt token正确，但权限不够)" });
                //operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                //operation.Security.Add(new Dictionary<string, IEnumerable<string>>
                //{
                //    { "oauth2", requiredScopes }
                //});
            }
        }
    }

    public class Remove200StatuCodeIfNotNeedOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes<SwaggerResponseAttribute>(true)
                .Where(attr => attr.StatusCode == 200)
                .Distinct();
            if (!requiredScopes.Any())
            {
                operation.Responses.Remove("200");
            }
        }
    }

    //public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    //{
    //    public void Apply(ControllerModel controller)
    //    {
    //        var controllerNamespace = controller.ControllerType.Namespace; // e.g. "Controllers.V1"
    //        var apiVersion = controllerNamespace.Split('.').Last().ToLower();

    //        controller.ApiExplorer.GroupName = apiVersion;
    //    }
    //}
}