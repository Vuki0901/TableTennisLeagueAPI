using Domain.Common;
using Domain.Leagues;
using Domain.Seasons;

namespace Domain.Matches;

public sealed class Match : Entity
{
    private readonly IList<Game> _games = new List<Game>();

    public int? ScoreHome { get; set; }
    public int? ScoreAway { get; set; }
    public int? RoundNumber { get; set; }
    public bool IsConfirmed { get; set; }

    public bool IsFinished => ScoreHome >= (Season?.BestOf + 1) / 2 || ScoreAway >= (Season?.BestOf + 1) / 2;

    public LeaguePlayer? Home { get; set; }
    public LeaguePlayer? Away { get; set; }
    public Season? Season { get; set; }
    public IEnumerable<Game> Games => _games;

    public void AddGame(Game game)
    {
        _games.Add(game);
    }

    public static Match Create(int scoreHome, int scoreAway, int roundNumber, Season season, LeaguePlayer home, LeaguePlayer away) => new Match()
    {
        ScoreHome = scoreHome,
        ScoreAway = scoreAway,
        RoundNumber = roundNumber,
        Season = season,
        Home = home,
        Away = away
    };
}