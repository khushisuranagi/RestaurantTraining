using Microsoft.Extensions.DependencyInjection;
using RestaurantTraining.Application.Common.Authentication;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Infrastructure.ArticlePreview;
using RestaurantTraining.Infrastructure.Authentication;
using RestaurantTraining.Infrastructure.CertificateManagement;
using Microsoft.Extensions.Http;

namespace RestaurantTraining.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();


            services.AddScoped<ICertificatePdfGenerator, QuestPdfCertificateGenerator>();
            services.AddHttpClient<IArticlePreviewFetcher, HttpArticlePreviewFetcher>();

            return services;
        }
    }
}