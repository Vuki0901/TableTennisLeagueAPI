using System.Runtime.CompilerServices;
using Domain.Common;

namespace Domain.Matches;

public sealed class Game : Entity
{
    private Game(int scoreHome, int scoreAway, Match match)
    {
        ScoreHome = scoreHome;
        ScoreAway = scoreAway;
        Match = match;
    }

    public int? ScoreHome { get; set; }
    public int? ScoreAway { get; set; }
    public bool IsFinished => (ScoreHome >= Match?.Season?.GameThreshold || ScoreAway >= Match?.Season?.GameThreshold) && Math.Abs(ScoreHome ?? 0 - ScoreAway ?? 0) == 2;

    public Match? Match { get; set; }

    public static Game Create(int scoreHome, int scoreAway, Match match)
    {
        if (scoreHome > match.Season!.GameThreshold || scoreAway > match.Season!.GameThreshold)
            throw new InvalidOperationException("A team can not have a score greater than the game threshold.");

        var game = new Game(scoreHome, scoreAway, match);

        return game;
    }

    public void UpdateScore(int scoreHome, int scoreAway)
    {
        if ((scoreHome > Match!.Season?.GameThreshold || scoreAway > Match.Season!.GameThreshold) && Math.Abs(ScoreHome ?? 0 - ScoreAway ?? 0) == 2)
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