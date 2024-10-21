using AspireTodo.UserManagement.Shared;
using Refit;

namespace AspireTodo.UserManagement.HttpClient;

public interface IAccountsHttpApi
{
    [Post("/api/account/login")]
    Task<TokenResponse> LoginAsync([Body] LoginRequest request);
    
    [Post("/api/account/register")]
    Task<TokenResponse> RegisterAsync([Body] RegisterRequest request);
    
    [Get("/api/account/profile")]
    Task<TokenResponse> GetAccountInfoAsync([Authorize] string token);
    
    [Put("/api/account/profile")]
    Task<TokenResponse> UpdateAccountInfoAsync([Body] UpdateProfileRequest request, [Authorize] string token);
}