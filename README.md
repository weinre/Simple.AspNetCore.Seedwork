# Simple.AspNetCore.Seedwork
WebApi mini framework for aspnetcore

# Simple.AspNetCore.Seedwork.Cors
```c#

services.AddSimpleCors();
app.UseSimpleCors();
```

# Simple.AspNetCore.Seedwork.ApiDocument
```c#

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
services.AddSimpleApiVersion();
app.UseSimpleApiDocument();
```

# Simple.AspNetCore.Seedwork.MvcCore
```c#

services.AddVirtualPath();
services
    .AddSimpleMvc(options => { options.Filters.Add<WebApiGlobalExceptionFilter>(); })
    .AddJsonOptions(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    })
    .AddFluentValidationOptions();
app.UseSimpleConfig(env);
app.UseSimpleMvc();
```
  
