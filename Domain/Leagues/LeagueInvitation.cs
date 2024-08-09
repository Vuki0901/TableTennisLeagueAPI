using Domain.Common;

namespace Domain.Leagues;

public sealed class LeagueInvitation : Entity
{
    public string PlayerEmailAddress { get; set; } = string.Empty;
    public LeagueInvitationStatus Status { get; set; } = LeagueInvitationStatus.Pending;

    public League? League { get; set; }

    public static LeagueInvitation Create(string playerEmailAddress, League league) => new()
    {
        PlayerEmailAddress = playerEmailAddress,
        League = league
    };
}