using System.Text.Json.Serialization;
using AspireTodo.Core.ExceptionHandler;
using AspireTodo.Core.Identity;
using AspireTodo.Core.MassTransit;
using AspireTodo.Core.OpenApi;
using AspireTodo.Notifications;
using AspireTodo.Notifications.Configurations;
using AspireTodo.Notifications.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultOpenApi();
builder.AddAppDatabase();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddAppAuthentication(builder.Configuration)
    .AddAppServices()
    .AddTodoMassTransit(x => x.SetConsumers())
    .AddFluentValidation()
    .AddSignalR();

builder.Services.AddTodoExceptionHandler();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.MapDefaultEndpoints();
app.UseDefaultOpenApi();

app.MapRoutes();
app.MapHub<NotificationsHub>("/todosHub");

app.Run();