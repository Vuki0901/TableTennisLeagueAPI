using Domain.Leagues;

namespace Domain.Notifications;

public sealed class LeagueInvitationNotification : Notification
{
    public LeagueInvitation? LeagueInvitation { get; set; }
}