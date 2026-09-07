using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(x => x.LessonId);

            builder.Property(x => x.LessonId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.LessonTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.SortOrder)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(4000);

        }
    }
}