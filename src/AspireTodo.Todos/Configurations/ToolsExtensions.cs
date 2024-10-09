using System.Globalization;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace AspireTodo.Todos.Configurations;

public static class ToolsExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("fa");

        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddFluentValidationRulesToSwagger();

        return services;
    }
}