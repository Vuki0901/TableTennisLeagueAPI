using Domain.Leagues;
using Domain.Seasons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Seasons;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.BestOf).IsRequired();
        builder.Property(_ => _.GameThreshold).IsRequired();

        builder.HasOne<League>(_ => _.League);
    }
}