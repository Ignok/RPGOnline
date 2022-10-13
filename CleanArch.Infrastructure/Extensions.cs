using Microsoft.Extensions.DependencyInjection;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Infrastructure.Persistence;

namespace RPGOnline.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}
