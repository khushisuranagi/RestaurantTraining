using Microsoft.Extensions.DependencyInjection;
using RestaurantTraining.Application.Common.Authentication;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Infrastructure.ArticlePreview;
using RestaurantTraining.Infrastructure.Authentication;
using RestaurantTraining.Infrastructure.CertificateManagement;
using RestaurantTraining.Infrastructure.Ai;
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

            // Gemini AI question generator (typed HttpClient).
            services.AddHttpClient<IAiQuestionGenerator, GeminiQuestionGenerator>();

            // Gemini plain-text generator (used by the /api/gemini-test endpoint).
            services.AddHttpClient<IAiTextGenerator, GeminiTextGenerator>();

            // Gemini multimodal summarizer — a longer timeout, since reading a
            // video/PDF can take a while on the first (uncached) request.
            services.AddHttpClient<IAiSummarizer, GeminiSummarizer>(c =>
                c.Timeout = TimeSpan.FromSeconds(150));

            // Gemini roleplay (AI practice scenarios).
            services.AddHttpClient<IAiRoleplay, GeminiRoleplay>(c =>
                c.Timeout = TimeSpan.FromSeconds(60));

            // Gemini help chatbot.
            services.AddHttpClient<IAiAssistant, GeminiAssistant>(c =>
                c.Timeout = TimeSpan.FromSeconds(60));

            return services;
        }
    }
}