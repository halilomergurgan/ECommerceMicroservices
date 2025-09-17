using Microsoft.Extensions.DependencyInjection;
using Services.User.Application.Services;

namespace Services.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserAddressService, UserAddressService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
