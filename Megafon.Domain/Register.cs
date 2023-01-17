using Megafon.Contracts.Interfaces;
using Megafon.Domain.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Megafon.Domain;

public static class Register
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ICultureService, CultureSerivce>();
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IAuthService, AuthService>();

        return services;
    }
}
