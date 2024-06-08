using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class MasstransitConfiguration
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? extendConfiguration = null, Assembly? assembly = null)
    {
        //Implement RabbitMQ MassTransit configuration
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (extendConfiguration != null)
            {
                extendConfiguration(config);
            }

            if (assembly != null)
            {
                config.AddConsumers(assembly);
                config.AddSagaStateMachines(assembly);
                //config.AddSagas(assembly);
                //config.AddActivities(assembly);
            }


            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                configurator.UseInMemoryOutbox(context);
                configurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}
