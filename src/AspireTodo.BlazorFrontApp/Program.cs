using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AspireTodo.BlazorFrontApp;
using AspireTodo.BlazorFrontApp.Common.Auth;
using AspireTodo.Todos.HttpClient;
using AspireTodo.UserManagement.HttpClient;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var url = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:8080";

foreach (var variable in Environment.GetEnvironmentVariables())
{
    Console.WriteLine(variable.ToString());
}

builder.Services.AddScoped<ITokenAccessor, TokenAccessor>();
builder.Services.AddBlazoredToast();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddTodosHttpClients(url + "/todos");
builder.Services.AddUsersHttpClients(url + "/users");

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();