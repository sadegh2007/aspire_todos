using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AspireTodo.Core.Exceptions;
using AspireTodo.Core.Identity;
using AspireTodo.Core.Shared;
using AspireTodo.UserManagement.Data;
using AspireTodo.UserManagement.Events;
using AspireTodo.UserManagement.Exceptions;
using AspireTodo.UserManagement.Features.Users.Data.Mappers;
using AspireTodo.UserManagement.Features.Users.Domains;
using AspireTodo.UserManagement.Shared;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AspireTodo.UserManagement.Features.Auth.Services;

public partial class AccountService(
    UsersDbContext appDbContext,
    UserManager<User> userManager,
    IOptions<JwtConfig> jwtConfig,
    IHttpContextAccessor httpContextAccessor,
    IPublishEndpoint publishEndpoint,
    ILogger<AccountService> logger
): IAccountService
{
    private readonly ILogger<AccountService> _logger = logger;

    [LoggerMessage(1, LogLevel.Information, message: "Login Request Body: {loginRequest}")]
    partial void LogLoginRequest(string loginRequest);

    public async Task<TokenResponse> Login(LoginRequest request, CancellationToken cancellationToken = default)
    {
        LogLoginRequest(JsonSerializer.Serialize(request));

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new IncorrectUserPasswordException();
        }

        if (!(await userManager.CheckPasswordAsync(user, request.Password)))
        {
            throw new IncorrectUserPasswordException();
        }

        var claims = GetClaims(user);
        return GenerateToken(user, claims);
    }

    public async Task<TokenResponse> Register(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await userManager.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken: cancellationToken))
        {
            throw new Exception("This phone number is already registered.");
        }

        var domain = new User(request.PhoneNumber)
        {
            Name = request.Name,
            Family = request.Family,
        };

        var result = await userManager.CreateAsync(domain, request.Password);

        if (!result.Succeeded) throw new Exception(result.Errors.FirstOrDefault()?.Description);

        var user = await userManager.Users.FirstAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken: cancellationToken);

        var claims = GetClaims(user);
        return GenerateToken(user, claims);
    }

    public async Task Logout(CancellationToken cancellationToken = default)
    {
        //
    }

    public async Task<UserDto> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();

        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException("Please login first");
        }

        return await userManager.Users.Select(x => x.ToDto())
                   .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken: cancellationToken)
            ?? throw new UserNotFound();
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var userId = httpContextAccessor.HttpContext!.User.GetUserId()!.Value;
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                   ?? throw new UserNotFound();

        user.Name = request.Name;
        user.Family = request.Family;

        appDbContext.Users.Update(user);
        await appDbContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish<UserUpdated>(new(UserId.FromInt32(user.Id), user.Name, user.Family), cancellationToken);
    }

    private List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>();

        claims.AddRange(new[]
        {
           new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
           new Claim(JwtRegisteredClaimNames.UniqueName, user.PhoneNumber!),
        });

        return claims;
    }

    private TokenResponse GenerateToken(User user, List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Value.Issuer,
            audience: jwtConfig.Value.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtConfig.Value.Expire), // default is a month
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = GenerateRefreshToken(),
            Expires = token.ValidTo.Ticks,
            User = user.ToDto(),
        };
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}