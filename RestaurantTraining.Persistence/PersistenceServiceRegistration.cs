using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Persistence.Repositories;

namespace RestaurantTraining.Persistence
{
    //connect to the database, and here are the repository classes to use
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("RestaurantTrainingConnection")));

            // Register repositories (tells the app which class to use for each interface)
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonResourceRepository, LessonResourceRepository>();
            services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
            services.AddScoped<IQuizOptionRepository, QuizOptionRepository>();
            services.AddScoped<IAIScenarioRepository, AIScenarioRepository>();
            services.AddScoped<IModuleCertificateSettingRepository, ModuleCertificateSettingRepository>();
            services.AddScoped<IRoleModuleRepository, RoleModuleRepository>();
            services.AddScoped<IContentCreatorDashboardRepository, ContentCreatorDashboardRepository>();
            services.AddScoped<ILearnerCertificateRepository, LearnerCertificateRepository>();
            services.AddScoped<IContentCreatorPeopleRepository, ContentCreatorPeopleRepository>();
            services.AddScoped<ILearnerDashboardRepository, LearnerDashboardRepository>();
            services.AddScoped<ILearnerModuleRepository, LearnerModuleRepository>();
            services.AddScoped<ILearnerQuizRepository, LearnerQuizRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();

            return services;
        }
    }
}