using Inventory.API.Contracts.Configurations;
using System.Reflection;

namespace Inventory.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CommandConsumerEndpoints>(configuration.GetSection("MessageBroker:CommandConsumerEndpoints"));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
