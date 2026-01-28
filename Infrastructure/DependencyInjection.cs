using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseNpgsql(config.GetConnectionString("Database")));

            services.AddScoped<DeviceRepository>();

            return services;
        }
    }
}
