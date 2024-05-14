using Domain.Leagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Leagues;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    public void Configure(EntityTypeBuilder<League> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.MatchFormat).IsRequired();

        builder.HasMany(_ => _.Seasons);
        builder.HasMany(_ => _.LeaguePlayers);
        builder.HasMany(_ => _.LeagueInvitations);
    }
}