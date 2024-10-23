using AspireTodo.Core.Identity;
using AspireTodo.Core.MassTransit;
using AspireTodo.Todos.Grpc.Configurations;
using AspireTodo.Todos.Grpc.Data;
using AspireTodo.Todos.Grpc.GrpcServices;
using AspireTodo.Todos.Grpc.Interceptors;
using Gridify;

var builder = WebApplication.CreateBuilder(args);

GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();

builder.AddBasicServiceDefaults();
builder.AddAppDatabase();

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.Interceptors.Add<AuthInterceptor>();
});
builder.Services
    .AddAppAuthentication(builder.Configuration)
    .AddAppServices(builder.Configuration)
    .AddTodoMassTransit(x => x.SetConsumers());

builder.Services.AddDbContext<TodosDbContext>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapGrpcService<TodoService>();

app.Run();