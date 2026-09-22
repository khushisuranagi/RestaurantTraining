using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class ResourceQuestionAttemptConfiguration : IEntityTypeConfiguration<ResourceQuestionAttempt>
    {
        public void Configure(EntityTypeBuilder<ResourceQuestionAttempt> builder)
        {
            builder.ToTable("ResourceQuestionAttempts");

            builder.HasKey(x => x.AttemptId);

            builder.Property(x => x.AttemptId)
                .ValueGeneratedOnAdd();

            // One "passed" row per learner + resource.
            builder.HasIndex(x => new { x.UserId, x.ResourceId }).IsUnique();
        }
    }
}
