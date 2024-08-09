using Domain.Common;

namespace Domain.Leagues;

public sealed class League : Entity
{
    private readonly IList<LeaguePlayer> _leaguePlayers = new List<LeaguePlayer>();
    private readonly IList<LeagueInvitation> _leagueInvitations = new List<LeagueInvitation>();

    public string Name { get; set; } = string.Empty;
    public MatchFormat MatchFormat { get; set; }

    public IEnumerable<LeaguePlayer> LeaguePlayers => _leaguePlayers;
    public IEnumerable<LeagueInvitation> LeagueInvitations => _leagueInvitations;

    public static League Create(string name, MatchFormat matchFormat) => new League()
    {
        Name = name,
        MatchFormat = matchFormat
    };

    public void AddPlayer(LeaguePlayer leaguePlayer)
    {
        _leaguePlayers.Add(leaguePlayer);
    }

    public void AddInvitation(LeagueInvitation leagueInvitation)
    {
        _leagueInvitations.Add(leagueInvitation);
    }

    public void RemoveInvitation(LeagueInvitation leagueInvitation)
    {
        _leagueInvitations.Remove(leagueInvitation);
    }
}