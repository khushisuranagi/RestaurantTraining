using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Configurations
{
    public class ResourceSummaryConfiguration : IEntityTypeConfiguration<ResourceSummary>
    {
        public void Configure(EntityTypeBuilder<ResourceSummary> builder)
        {
            builder.ToTable("ResourceSummaries");

            builder.HasKey(x => x.ResourceSummaryId);

            builder.Property(x => x.ResourceSummaryId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.SummaryText)
                .IsRequired();   // nvarchar(max)

            // One summary per resource.
            builder.HasIndex(x => x.ResourceId).IsUnique();
        }
    }
}
