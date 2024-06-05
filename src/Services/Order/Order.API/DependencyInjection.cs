using Carter;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using System.Reflection;

namespace Order.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
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

            return services;
        }

        public static WebApplication UseServices(this WebApplication app)
        {
            app.MapCarter();
            return app;
        }
    }
}
