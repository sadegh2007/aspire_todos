using AspireTodo.Core.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseDefaultOpenApi();

app.Run();
