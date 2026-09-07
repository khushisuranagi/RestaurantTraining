using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence
{//ef core's representation of database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RoleModule> RoleModules { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<LessonResource> LessonResources { get; set; }

        public DbSet<LessonProgress> LessonProgress { get; set; }

        public DbSet<ModuleProgress> ModuleProgress { get; set; }

        public DbSet<QuizAttempt> QuizAttempts { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        public DbSet<QuizOption> QuizOptions { get; set; }

        public DbSet<AIScenario> AIScenarios { get; set; }

        public DbSet<ScenarioAttempt> ScenarioAttempts { get; set; }

        public DbSet<Certificate> Certificates { get; set; }

        public DbSet<ModuleCertificateSetting> ModuleCertificateSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }
    }
}