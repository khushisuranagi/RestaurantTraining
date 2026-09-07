using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class ScenarioAttemptConfiguration : IEntityTypeConfiguration<ScenarioAttempt>
    {
        public void Configure(EntityTypeBuilder<ScenarioAttempt> builder)
        {
            builder.ToTable("ScenarioAttempts");

            builder.HasKey(x => x.AttemptId);

            builder.Property(x => x.AttemptId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Score)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.Passed)
                .IsRequired();

            builder.Property(x => x.Feedback)
                .IsRequired();

            builder.Property(x => x.AttemptedAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<AIScenario>()
                .WithMany()
                .HasForeignKey(x => x.ScenarioId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}