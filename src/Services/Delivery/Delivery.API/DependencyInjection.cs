using BuildingBlocks.Messaging.MassTransit;
using Carter;
using Delivery.API.Contracts.Configurations;
using System.Reflection;

namespace Delivery.API;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CommandConsumerEndpoints>(configuration.GetSection("MessageBroker:CommandConsumerEndpoints"));
        services.AddMessageBroker(configuration, null, Assembly.GetExecutingAssembly());
        services.AddCarter();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        return services;
    }

    public static WebApplication UseServices(this WebApplication app)
    {
        app.MapCarter();
        return app;
    }
}
