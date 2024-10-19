using AspireTodo.AppHost;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var pgMountDir = builder.Configuration.GetValue<string>("PG_MOUNT_DIR")!;
var rbMountDir = builder.Configuration.GetValue<string>("RB_MOUNT_DIR")!;

var rabbitMq = builder.AddRabbitMQ("EventBus")
    .WithImageTag("4.0.2-alpine")
    .WithDataBindMount(rbMountDir);

var postgres = builder.AddPostgres("postgres", password: builder.CreateStablePassword("postgres-password"))
    .WithImage("postgis/postgis", "17-3.5-alpine")
    .WithDataBindMount(pgMountDir);

var userDb = postgres.AddDatabase("UsersDb");
var todosDb = postgres.AddDatabase("TodosDb");
var notificationsDb = postgres.AddDatabase("notificationsDb");

var notificationsService = builder.AddProject<Projects.AspireTodo_Notifications>("notifications")
    .WithReference(notificationsDb)
    .WithReference(rabbitMq);

var usersService = builder.AddProject<Projects.AspireTodo_UserManagement>("users")
    .WithReference(userDb)
    .WithReference(rabbitMq);

var todosService = builder.AddProject<Projects.AspireTodo_Todos>("todos")
    .WithReference(todosDb)
    .WithReference(rabbitMq)
    .WithReference(usersService);

var aiTextCompletion = builder.AddProject<Projects.AspireTodo_Ai_TextCompletion>("textCompletion")
    .WithReference(todosService);

var reactApp = builder.AddNpmApp("reactApp", "../AspireTodo.ReactApp/", "dev")
    .WithExternalHttpEndpoints()
    .WithEnvironment("VITE_API_BASE_PATH", "http://localhost:8080");

var blazorFrontApp = builder.AddProject<Projects.AspireTodo_BlazorFrontApp>("blazorFrontApp")
    .WithExternalHttpEndpoints()
    .WithEnvironment("API_URL", "http://localhost:8080");

builder.AddProject<Projects.AspireTodo_Gateway>("gateway")
    .WithExternalHttpEndpoints()
    .WithReference(notificationsService)
    .WithReference(usersService)
    .WithReference(todosService)
    .WithReference(aiTextCompletion)
    .WithReference(reactApp)
    .WithReference(blazorFrontApp);

builder.Build().Run();