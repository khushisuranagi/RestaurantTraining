using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            builder.ToTable("QuizQuestions");

            builder.HasKey(x => x.QuestionId);

            builder.Property(x => x.QuestionId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.QuestionText)
                .IsRequired();

            builder.Property(x => x.QuestionType)
                .IsRequired();

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Explanation)
                .IsRequired();

            builder.Property(x => x.Marks)
                .IsRequired();

            builder.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}