using AspireTodo.Core.Identity;
using AspireTodo.Todos.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services
    .AddAppAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TodoService>();

app.UseAuthentication();
app.UseAuthorization();

app.Run();