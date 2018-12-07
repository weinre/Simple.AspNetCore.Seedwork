# Simple.AspNetCore.Seedwork
WebApi mini framework for aspnetcore

```c#

public void ConfigureServices(IServiceCollection services)
{
    services.AddVirtualPath(Configuration.GetSection(nameof(VirtualPathOptions)));
    services.AddSimpleCors();
    services.AddSimpleApiDocument(options =>
            {
                options.Docs = new List<(string name, Info info)>
                {
                    ("v1.0", new Info
                    {
                        Title = "API",
                        Description = "API说明",
                        Version = "v1.0",
                        Contact = new Contact {Name = "beefsteak", Email = "beefsteak@live.com"}
                    })
                };
                options.IsSupportApiVersion = true;
                options.IsSupportJWTToken = true;
                options.IsGlobalJWTToken = true;
            });
    services
        .AddSimpleMvc(options => { options.Filters.Add<WebApiGlobalExceptionFilter>(); })
        .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
        .AddFluentValidationOptions();
    services.AddSimpleApiVersion();
}

public void Configure(
    IApplicationBuilder app,
    IHostingEnvironment env,
    IOptions<VirtualPathOptions> options)
{
    app.UseSimpleConfig(env);
    app.UseSimpleCors();
    app.UseSimpleApiDocument(options.Value.BasePath);
    app.UseSimpleMvc();
}
```
