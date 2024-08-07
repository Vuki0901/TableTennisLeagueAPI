using Domain.Leagues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Leagues;

public class LeagueInvitationConfiguration : IEntityTypeConfiguration<LeagueInvitation>
{
    public void Configure(EntityTypeBuilder<LeagueInvitation> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.PlayerEmailAddress).IsRequired();
        builder.Property(_ => _.Status).IsRequired();
        
        builder.HasOne(_ => _.League);
    }
}