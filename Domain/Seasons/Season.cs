using Domain.Common;
using Domain.Leagues;
using Domain.Matches;

namespace Domain.Seasons;

public sealed class Season : Entity
{
    private readonly IList<Match> _matches = new List<Match>();

    public int NumberOfRounds { get; set; } = 0;
    public int BestOf { get; set; } = 7;
    public int GameThreshold { get; set; } = 11;
    
    public League? League { get; private set; }
    public IEnumerable<Match> Matches => _matches;

    public void AddMatch(Match match)
    {
        _matches.Add(match);
    }
}