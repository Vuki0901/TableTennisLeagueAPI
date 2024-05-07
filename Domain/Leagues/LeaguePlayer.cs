using Domain.Common;
using Domain.Users;

namespace Domain.Leagues;

public sealed class LeaguePlayer : Entity
{
    public LeaguePlayerLevel LeaguePlayerLevel { get; set; }
    
    public League? League { get; set; }
    public Player? Player { get; set; }
}