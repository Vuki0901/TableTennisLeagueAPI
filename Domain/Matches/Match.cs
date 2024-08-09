using Domain.Common;
using Domain.Leagues;
using Domain.Seasons;

namespace Domain.Matches;

public sealed class Match : Entity
{
    private Match() { }
    
    private Match(int scoreHome, int scoreAway, int roundNumber, Season season, LeaguePlayer home, LeaguePlayer away)
    {
        ScoreHome = scoreHome;
        ScoreAway = scoreAway;
        RoundNumber = roundNumber;
        Season = season;
        Home = home;
        Away = away;
    }

    private readonly IList<Game> _sets = new List<Game>();

    public int? ScoreHome { get; set; }
    public int? ScoreAway { get; set; }
    public int? RoundNumber { get; set; }
    public bool IsConfirmed { get; set; }

    public bool IsFinished => ScoreHome >= (Season?.BestOf + 1) / 2 || ScoreAway >= (Season?.BestOf + 1) / 2;

    public LeaguePlayer? Home { get; set; }
    public LeaguePlayer? Away { get; set; }
    public Season? Season { get; set; }
    public IEnumerable<Game> Sets => _sets;

    public void AddSet(Game game)
    {
        _sets.Add(game);
    }

    public static Match Create(int scoreHome, int scoreAway, int roundNumber, Season season, LeaguePlayer home, LeaguePlayer away)
    {
        // if (season.Matches.Any(_ => (_.Home!.Player!.Id == home.Player!.Id || _.Home.Player.Id == away.Player!.Id || _.Away!.Player!.Id == away.Player.Id || _.Away.Player.Id == home.Player.Id) && _.RoundNumber == roundNumber))
        //     throw new InvalidOperationException("A player can not play more than 1 match in the same round.");

        var match = new Match(scoreHome, scoreAway, roundNumber, season, home, away);

        return match;
    }
}