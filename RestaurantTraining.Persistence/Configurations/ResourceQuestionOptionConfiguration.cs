using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class ResourceQuestionOptionConfiguration : IEntityTypeConfiguration<ResourceQuestionOption>
    {
        public void Configure(EntityTypeBuilder<ResourceQuestionOption> builder)
        {
            builder.ToTable("ResourceQuestionOptions");

            builder.HasKey(x => x.OptionId);

            builder.Property(x => x.OptionId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.OptionText)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.ResourceQuestionId);
        }
    }
}
