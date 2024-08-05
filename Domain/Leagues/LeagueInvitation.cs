using Domain.Common;

namespace Domain.Leagues;

public sealed class LeagueInvitation : Entity
{
    public string InvitationToken { get; set; } = string.Empty;
    public LeagueInvitationStatus Status { get; set; } = LeagueInvitationStatus.Pending;
    
    public League? League { get; set; }
}