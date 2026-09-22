using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class ResourceQuestionConfiguration : IEntityTypeConfiguration<ResourceQuestion>
    {
        public void Configure(EntityTypeBuilder<ResourceQuestion> builder)
        {
            builder.ToTable("ResourceQuestions");

            builder.HasKey(x => x.ResourceQuestionId);

            builder.Property(x => x.ResourceQuestionId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.QuestionText)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Explanation)
                .HasMaxLength(1000);

            // One question per resource.
            builder.HasIndex(x => x.ResourceId).IsUnique();
        }
    }
}
