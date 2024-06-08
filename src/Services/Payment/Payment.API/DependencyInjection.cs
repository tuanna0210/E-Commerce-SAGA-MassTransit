using Carter;
using Payment.API.Contracts.Configurations;
using System.Reflection;

namespace Payment.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CommandConsumerEndpoints>(configuration.GetSection("MessageBroker:CommandConsumerEndpoints"));

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
}
