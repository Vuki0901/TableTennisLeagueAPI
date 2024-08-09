using Domain.Common;
using Domain.Leagues;

namespace Domain.Seasons;

public sealed class Season : Entity
{
    public int BestOf { get; set; }
    public int GameThreshold { get; set; }

    public League? League { get; set; }

    public static Season Create(int bestOf, int gameThreshold, League league) => new()
    {
        BestOf = bestOf,
        GameThreshold = gameThreshold,
        League = league
    };
}