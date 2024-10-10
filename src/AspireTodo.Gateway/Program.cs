using AspireTodo.Gateway;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCors(options =>
{
    options.AddPolicy("myPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var (routes, clusters) = ProxyBuilder.Create()
    .AddRoute("users", "http://users")
    .AddRoute("todos", "http://todos")
    .Build();

builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);
builder.Services.AddServiceDiscovery();
builder.Services.AddHttpForwarderWithServiceDiscovery();

var app = builder.Build();

app.UseCors();
app.MapReverseProxy();
app.UseSwaggerUI(options =>
{
    options.EnableDeepLinking();

    options.SwaggerEndpoint("/users/swagger/v1/swagger.json", "Users Management V1");
    options.SwaggerEndpoint("/todos/swagger/v1/swagger.json", "Todos V1");

    options.DocExpansion(DocExpansion.List);
});

app.Run();
