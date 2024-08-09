using Domain.Leagues;

namespace Domain.Users;

public sealed class Player : UserRole
{
    private readonly IList<LeaguePlayer> _leaguePlayers = new List<LeaguePlayer>();

    public User? User;
    public IEnumerable<LeaguePlayer> LeaguePlayers => _leaguePlayers;

    public void AddLeaguePlayer(LeaguePlayer leaguePlayer)
    {
        _leaguePlayers.Add(leaguePlayer);
    }
}