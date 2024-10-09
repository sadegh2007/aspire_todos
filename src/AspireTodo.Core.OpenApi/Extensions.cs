using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspireTodo.Core.OpenApi;

public static class Extensions
{
	public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
	{
		var configuration = app.Configuration;

		var openApiSettings = app.Services.GetRequiredService<IOptions<OpenApiSettings>>().Value;

		app.UseSwagger();
		app.UseSwaggerUI(setup =>
		{
			var pathBase = configuration["PATH_BASE"];
			var authSection = openApiSettings.Auth;
			var endpointSettings = openApiSettings.Endpoint;

			var swaggerUrl = endpointSettings.Url ?? $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json";

			setup.SwaggerEndpoint(swaggerUrl, endpointSettings.Name);

			if (authSection is not null)
			{
				setup.OAuthClientId(authSection.ClientId);
				setup.OAuthAppName(authSection.AppName);
			}
		});

		// Add a redirect from the root of the app to the swagger endpoint
		app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

		return app;
	}

	public static IHostApplicationBuilder AddDefaultOpenApi(this IHostApplicationBuilder builder)
	{
		var services = builder.Services;
		var configuration = builder.Configuration;

		builder.Services.AddOptions<OpenApiSettings>()
			.BindConfiguration("OpenApi")
			.ValidateDataAnnotations()
			.ValidateOnStart();

		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(options =>
		{
			var openApiSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<OpenApiSettings>>().Value;

			foreach (var document in openApiSettings.Documents)
			{
				var version = document.Version;

				options.SwaggerDoc(version, new OpenApiInfo
				{
					Title = document.Title,
					Version = version,
					Description = document.Description
				});
			}

			options.EnableAnnotations();

			options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
			options.OperationFilter<SecurityRequirementsOperationFilter>();

			var identitySection = configuration.GetSection("Identity");

			if (!identitySection.Exists())
			{
				// No identity section, so no authentication open api definition
				return;
			}

			var identityUrlExternal = identitySection.GetValue<string>("Url")!;
			var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);

			options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.OAuth2,
				Flows = new OpenApiOAuthFlows()
				{
					// TODO: Change this to use Authorization Code flow with PKCE
					Implicit = new OpenApiOAuthFlow()
					{
						AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
						TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
						Scopes = scopes,
					}
				}
			});

			options.OperationFilter<AuthorizeCheckOperationFilter>([scopes.Keys.ToArray()]);
		});

		return builder;
	}

	private sealed class AuthorizeCheckOperationFilter(string[] scopes) : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

			if (!metadata.OfType<IAuthorizeData>().Any())
			{
				return;
			}

			operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
			operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

			var oAuthScheme = new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
			};

			operation.Security = new List<OpenApiSecurityRequirement>
			{
				new()
				{
					[oAuthScheme] = scopes
				}
			};
		}
	}
}
