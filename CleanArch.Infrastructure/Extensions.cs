using Microsoft.Extensions.DependencyInjection;
using RPGOnline.Application.Common.Interfaces;
using RPGOnline.Infrastructure.Models;

namespace RPGOnline.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
