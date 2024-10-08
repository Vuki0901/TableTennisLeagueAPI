﻿using Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Notifications;

public class LeagueInvitationNotificationConfiguration : IEntityTypeConfiguration<LeagueInvitationNotification>
{
    public void Configure(EntityTypeBuilder<LeagueInvitationNotification> builder)
    {
        builder.HasOne(_ => _.LeagueInvitation);
    }
}