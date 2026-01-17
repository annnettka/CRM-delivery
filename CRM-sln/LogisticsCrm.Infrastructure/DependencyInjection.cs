using LogisticsCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Infrastructure.Repositories;
using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Infrastructure.Repositories;



namespace LogisticsCrm.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
     this IServiceCollection services,
     IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<LogisticsCrmDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderStatusHistoryRepository, OrderStatusHistoryRepository>();



            return services;
        }

    }
} 
