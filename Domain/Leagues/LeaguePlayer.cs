using Domain.Common;

namespace Domain.Leagues;

public sealed class LeaguePlayer : Entity
{
    public LeaguePlayerLevel LeaguePlayerLevel { get; set; }

    public static LeaguePlayer Create(LeaguePlayerLevel leaguePlayerLevel) => new()
    {
        LeaguePlayerLevel = leaguePlayerLevel
    };
}