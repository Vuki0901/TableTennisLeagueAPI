using Domain.Common;
using Domain.Users;

namespace Domain.Leagues;

public sealed class LeaguePlayer : Entity
{
    public LeaguePlayerLevel LeaguePlayerLevel { get; set; }

    // public static LeaguePlayer Create(LeaguePlayerLevel leaguePlayerLevel, League league, Player player)
    // {
    //     return new LeaguePlayer()
    //     {
    //         LeaguePlayerLevel = leaguePlayerLevel,
    //         League = league,
    //         Player = player
    //     };
    // }
    //
    // public League? League { get; set; }
    // public Player? Player { get; set; }
    
    public static LeaguePlayer Create(LeaguePlayerLevel leaguePlayerLevel)
    {
        return new LeaguePlayer()
        {
            LeaguePlayerLevel = leaguePlayerLevel
        };
    }
}