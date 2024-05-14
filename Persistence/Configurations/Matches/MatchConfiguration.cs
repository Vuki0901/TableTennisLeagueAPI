using Domain.Leagues;
using Domain.Matches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Matches;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(_ => _.Id);
        
        builder.HasOne<LeaguePlayer>(_ => _.Home);
        builder.HasOne<LeaguePlayer>(_ => _.Away);
        builder.HasOne(_ => _.Season);
        builder.HasMany(_ => _.Games);
    }
}