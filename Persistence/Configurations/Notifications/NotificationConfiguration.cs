using Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Notifications;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.HasDiscriminator(_ => _.Type)
            .HasValue<LeagueInvitationNotification>(nameof(LeagueInvitationNotification));
        
        builder.HasOne(_ => _.Sender);
    }
}