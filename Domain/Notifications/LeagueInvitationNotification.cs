using Domain.Leagues;
using Domain.Users;

namespace Domain.Notifications;

public sealed class LeagueInvitationNotification : Notification
{
    public LeagueInvitation? LeagueInvitation { get; set; }

    public static Notification Create(string playerEmailAddress, LeagueInvitation leagueInvitation, Player sender) => new LeagueInvitationNotification()
    {
        RecipientEmailAddress = playerEmailAddress,
        Sender = sender,
        LeagueInvitation = leagueInvitation
    };
}