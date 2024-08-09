using Domain.Common;
using Domain.Leagues;
using Domain.Matches;

namespace Domain.Seasons;

public sealed class Season : Entity
{
    private Season() { }
    
    private Season(int numberOfRounds, int bestOf, int gameThreshold)
    {
        NumberOfRounds = numberOfRounds;
        BestOf = bestOf;
        GameThreshold = gameThreshold;
    }
    
    private readonly IList<Match> _matches = new List<Match>();

    public int NumberOfRounds { get; set; }
    public int BestOf { get; set; }
    public int GameThreshold { get; set; }
    
    public IEnumerable<Match> Matches => _matches;

    public void AddMatch(Match match)
    {
        _matches.Add(match);
    }

    public static Season Create(int numberOfRounds, int bestOf, int setThreshold)
    {
        var season = new Season(numberOfRounds, bestOf, setThreshold);

        return season;
    }
}