using System.Text;
using AspireTodo.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AspireTodo.Core.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration) {

        services.Configure<JwtConfig>(configuration.GetSection("JWT"));

        var jwtConfig = configuration.GetRequiredSection("JWT");

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = jwtConfig.GetValue<string>("Issuer");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Key"))),
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfig.GetValue<string>("Issuer"),
                };
                // enable token validation for SignalR
                // options.Events = new JwtBearerEvents
                // {
                //     OnMessageReceived = context =>
                //     {
                //         var accessToken = context.Request.Query["access_token"];
                //
                //         // If the request is for our hub...
                //         var path = context.HttpContext.Request.Path;
                //         if (!string.IsNullOrEmpty(accessToken) &&
                //             (path.StartsWithSegments("/tenantsHub")))
                //         {
                //             // Read the token out of the query string
                //             context.Token = accessToken;
                //         }
                //
                //         return Task.CompletedTask;
                //     }
                // };
            });
        services.AddAuthorization();

        return services;
    }
}