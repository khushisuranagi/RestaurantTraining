using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class ModuleProgressConfiguration : IEntityTypeConfiguration<ModuleProgress>
    {
        public void Configure(EntityTypeBuilder<ModuleProgress> builder)
        {
            builder.ToTable("ModuleProgress");

            builder.HasKey(x => x.ModuleProgressId);

            builder.Property(x => x.ModuleProgressId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.IsCompleted)
                .IsRequired();

            builder.Property(x => x.CompletedAt)
                .IsRequired(false);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(x => x.StartedAt)
                .IsRequired();

            builder.Property(x => x.LastAccessedAt)
                .IsRequired();
            
        }
    }
}