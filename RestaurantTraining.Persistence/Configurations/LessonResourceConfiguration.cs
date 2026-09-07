using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class LessonResourceConfiguration : IEntityTypeConfiguration<LessonResource>
    {
        public void Configure(EntityTypeBuilder<LessonResource> builder)
        {
            builder.ToTable("LessonResources");

            builder.HasKey(x => x.ResourceId);

            builder.Property(x => x.ResourceId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ResourceType)
                .IsRequired();

            builder.Property(x => x.ResourceUrl)
                .HasMaxLength(500);

            builder.Property(x => x.FileData);

            builder.Property(x => x.FileName)
                .HasMaxLength(255);

            builder.Property(x => x.ContentType)
                .HasMaxLength(100);

            builder.Property(x => x.ContentText)
                .IsRequired();

            builder.Property(x => x.SortOrder)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne<Lesson>()
                .WithMany()
                .HasForeignKey(x => x.LessonId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}