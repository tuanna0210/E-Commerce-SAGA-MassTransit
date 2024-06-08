using BuildingBlocks.Messaging.MassTransit;
using Carter;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Contracts.Configurations;
using Order.API.Models;
using Order.API.SAGA;
using System.Reflection;
namespace Order.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CommandConsumerEndpoints>(configuration.GetSection("MessageBroker:CommandConsumerEndpoints"));


            var connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {

                options.UseSqlServer(connectionString);
            });

            services.AddCarter();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            //Masstransit
            services.AddMessageBroker(configuration, (cfg) =>
            {
                cfg.AddSagaStateMachine<ECommerceSaga, ECommerceSagaData>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<ApplicationDbContext>();
                        r.UseSqlServer();
                    });
            }, assembly: Assembly.GetExecutingAssembly());

            return services;
        }

        public static WebApplication UseServices(this WebApplication app)
        {
            app.MapCarter();
            return app;
        }
    }
}
