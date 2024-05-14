using Domain.Leagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Leagues;

public class LeaguePlayerConfiguration : IEntityTypeConfiguration<LeaguePlayer>
{
    public void Configure(EntityTypeBuilder<LeaguePlayer> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.HasOne(_ => _.League);
        builder.HasOne(_ => _.Player);
    }
}