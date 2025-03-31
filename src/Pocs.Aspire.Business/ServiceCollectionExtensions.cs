using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Pocs.Aspire.Business.User;
using Pocs.Aspire.Business.Users;

namespace Pocs.Aspire.Business;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddValidatorsFromAssemblyContaining<UserValidator>();

        return services;
    }
}
