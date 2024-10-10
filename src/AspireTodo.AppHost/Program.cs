using AspireTodo.AppHost;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitMq = builder.AddRabbitMQ("EventBus")
    .WithImageTag("4.0.2-alpine");

var pgMountDir = builder.Configuration.GetValue<string>("PG_MOUNT_DIR")!;

var postgres = builder.AddPostgres("postgres", password: builder.CreateStablePassword("postgres-password"))
    .WithImage("postgis/postgis", "17-3.5-alpine")
    .WithDataBindMount(pgMountDir);

var userDb = postgres.AddDatabase("UsersDb");
var todosDb = postgres.AddDatabase("TodosDb");

var usersService = builder.AddProject<Projects.AspireTodo_UserManagement>("users")
    .WithReference(userDb)
    .WithReference(rabbitMq);

var todosService = builder.AddProject<Projects.AspireTodo_Todos>("todos")
    .WithReference(todosDb)
    .WithReference(rabbitMq)
    .WithReference(usersService);

var reactApp = builder.AddNpmApp("reactApp", "../AspireTodo.ReactApp/", "dev")
    .WithExternalHttpEndpoints()
    .WithEnvironment("VITE_API_BASE_PATH", "http://localhost:8080");

builder.AddProject<Projects.AspireTodo_Gateway>("gateway")
    .WithExternalHttpEndpoints()
    .WithReference(usersService)
    .WithReference(todosService)
    .WithReference(reactApp);

builder.Build().Run();