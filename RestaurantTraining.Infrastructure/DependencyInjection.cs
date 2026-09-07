using Microsoft.Extensions.DependencyInjection;
using RestaurantTraining.Application.Common.Authentication;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Infrastructure.Authentication;

namespace RestaurantTraining.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            
            return services;
        }
    }
}