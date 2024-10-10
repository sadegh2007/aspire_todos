using System.Text.Json.Serialization;
using AspireTodo.Ai.TextCompletion;
using AspireTodo.Ai.TextCompletion.Configurations;
using AspireTodo.Core.ExceptionHandler;
using AspireTodo.Core.Identity;
using AspireTodo.Core.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();
builder.Services.AddTodoExceptionHandler();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        options.JsonSerializerOptions.ReferenceHandler= ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services
    .AddAppAuthentication(builder.Configuration)
    .AddAppServices()
    .AddFluentValidation();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.UseDefaultOpenApi();
app.MapDefaultEndpoints();
app.MapRouter();

app.Run();