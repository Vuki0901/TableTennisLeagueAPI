using System.Runtime.CompilerServices;
using Domain.Common;

namespace Domain.Matches;

public sealed class Set : Entity
{
    private Set() { }
    
    private Set(int scoreHome, int scoreAway, Match match)
    {
        ScoreHome = scoreHome;
        ScoreAway = scoreAway;
        Match = match;
    }

    public int? ScoreHome { get; set; }
    public int? ScoreAway { get; set; }
    public bool IsFinished => (ScoreHome >= Match?.Season?.SetThreshold || ScoreAway >= Match?.Season?.SetThreshold) && Math.Abs(ScoreHome ?? 0 - ScoreAway ?? 0) == 2;

    public Match? Match { get; set; }

    public static Set Create(int scoreHome, int scoreAway, Match match)
    {
        if (scoreHome > match.Season!.SetThreshold || scoreAway > match.Season!.SetThreshold)
            throw new InvalidOperationException("A team can not have a score greater than the game threshold.");

        var game = new Set(scoreHome, scoreAway, match);

        return game;
    }

    public void UpdateScore(int scoreHome, int scoreAway)
    {
        if ((scoreHome > Match!.Season?.SetThreshold || scoreAway > Match.Season!.SetThreshold) && Math.Abs(ScoreHome ?? 0 - ScoreAway ?? 0) == 2)
            throw new InvalidOperationException("A team can not have a score greater than the game threshold.");

        ScoreHome = scoreHome;
        ScoreAway = scoreAway;

        if (!IsFinished)
            return;

        if (ScoreHome > ScoreAway)
            Match.ScoreHome++;
        else
            Match.ScoreAway++;
    }
}