using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.ToTable("QuizAttempts");

            builder.HasKey(x => x.AttemptId);

            builder.Property(x => x.AttemptId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Score)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.TotalMarks)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(x => x.Passed)
                .IsRequired();

            builder.Property(x => x.AttemptedAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}