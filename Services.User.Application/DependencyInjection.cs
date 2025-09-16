using Microsoft.Extensions.DependencyInjection;
using Services.User.Application.Services;

namespace Services.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
