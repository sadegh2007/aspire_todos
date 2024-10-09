using System.Text.Json.Serialization;
using AspireTodo.Core.Data;
using AspireTodo.Core.ExceptionHandler;
using AspireTodo.Core.Identity;
using AspireTodo.Core.OpenApi;
using AspireTodo.UserManagement;
using AspireTodo.UserManagement.Configuration;
using AspireTodo.UserManagement.Data;
using AspireTodo.UserManagement.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddAppDatabase();
builder.AddDefaultOpenApi();

builder.Services.AddControllers()
    .AddJsonOptions(config =>
    {
        config.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        config.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services
    .AddAppIdentity()
    .AddAppServices()
    .AddAppAuthentication(builder.Configuration)
    .AddFluentValidation();

builder.Services.AddTodoExceptionHandler();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(UsersJsonContext.Default);
});

builder.Services.AddMigration<UsersDbContext, AdminSeeder>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.UseDefaultOpenApi();
app.MapDefaultEndpoints();
app.MapAppRouter();

app.Run();
