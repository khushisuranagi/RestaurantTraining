using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class QuizOptionConfiguration : IEntityTypeConfiguration<QuizOption>
    {
        public void Configure(EntityTypeBuilder<QuizOption> builder)
        {
            builder.ToTable("QuizOptions");

            builder.HasKey(x => x.OptionId);

            builder.Property(x => x.OptionId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.OptionText)
                .IsRequired();

            builder.Property(x => x.IsCorrect)
                .IsRequired();

            builder.HasOne<QuizQuestion>()
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}