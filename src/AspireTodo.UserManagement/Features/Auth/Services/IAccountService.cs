using AspireTodo.UserManagement.Shared;

namespace AspireTodo.UserManagement.Features.Auth.Services;

public interface IAccountService
{
    public Task<TokenResponse> Login(LoginRequest request, CancellationToken cancellationToken = default);
    public Task<TokenResponse> Register(RegisterRequest request, CancellationToken cancellationToken = default);
    public Task Logout(CancellationToken cancellationToken = default);
    public Task<UserDto> GetProfileAsync(CancellationToken cancellationToken = default);
}