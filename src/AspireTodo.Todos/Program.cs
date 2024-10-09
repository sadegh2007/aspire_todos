using System.Text.Json.Serialization;
using AspireTodo.Core.Data;
using AspireTodo.Core.ExceptionHandler;
using AspireTodo.Core.Identity;
using AspireTodo.Core.MassTransit;
using AspireTodo.Core.OpenApi;
using AspireTodo.Todos;
using AspireTodo.Todos.Configurations;
using AspireTodo.Todos.Data;
using AspireTodo.Todos.Shared;
using Gridify;

var builder = WebApplication.CreateBuilder(args);

GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();
builder.AddAppDatabase();

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
    .AddTodoMassTransit(x => x.SetConsumers())
    .AddFluentValidation();

builder.Services.AddTodoExceptionHandler();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(TodosJsonContext.Default);
});

builder.Services.AddMigration<TodosDbContext>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.MapDefaultEndpoints();
app.UseDefaultOpenApi();
app.MapRoutes();

app.Run();
