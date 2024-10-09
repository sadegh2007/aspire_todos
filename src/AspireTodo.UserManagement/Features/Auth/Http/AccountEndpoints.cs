using AspireTodo.UserManagement.Features.Auth.Services;
using AspireTodo.UserManagement.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AspireTodo.UserManagement.Features.Auth.Http;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccounts(this IEndpointRouteBuilder endpoints)
    {
        var api = endpoints.MapGroup("account");

        api.MapPost("login", Login).AllowAnonymous();
        api.MapPost("register", Register).AllowAnonymous();
        api.MapGet("profile", GetProfile);
        api.MapPut("profile", UpdateProfile);

        return api;
    }

    private static async Task<TokenResponse> Login([FromBody] LoginRequest request, IAccountService accountService)
        => await accountService.Login(request);

    private static async Task<TokenResponse> Register([FromBody] RegisterRequest request, IAccountService accountService)
        => await accountService.Register(request);

    private static async Task<UserDto> GetProfile(IAccountService accountService)
        => await accountService.GetProfileAsync();

    private static async Task UpdateProfile([FromBody] UpdateProfileRequest request, IAccountService accountService)
        => await accountService.UpdateProfileAsync(request);
}