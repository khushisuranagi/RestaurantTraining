using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class AIScenarioConfiguration : IEntityTypeConfiguration<AIScenario>
    {
        public void Configure(EntityTypeBuilder<AIScenario> builder)
        {
            builder.ToTable("AIScenarios");

            builder.HasKey(x => x.ScenarioId);

            builder.Property(x => x.ScenarioId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.ScenarioPrompt)
                .IsRequired();

            builder.Property(x => x.Category)
                .HasMaxLength(100);

            builder.Property(x => x.PassingScore)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Role>()
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}