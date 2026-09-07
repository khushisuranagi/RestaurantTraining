using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class ModuleCertificateSettingConfiguration
        : IEntityTypeConfiguration<ModuleCertificateSetting>
    {
        public void Configure(EntityTypeBuilder<ModuleCertificateSetting> builder)
        {
            builder.ToTable("ModuleCertificateSettings");

            builder.HasKey(x => x.SettingId);

            builder.Property(x => x.SettingId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Title)
                .HasMaxLength(200);

            builder.Property(x => x.Message)
                .HasMaxLength(1000);

            builder.Property(x => x.Template)
                .HasMaxLength(100);

            builder.Property(x => x.IssuerName)
                .HasMaxLength(200);

            // One certificate setting per module.
            builder.HasIndex(x => x.ModuleId)
                .IsUnique();

            builder.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}