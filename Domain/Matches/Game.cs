using Domain.Common;

namespace Domain.Matches;

public sealed class Game : Entity
{
    public int? ScoreHome { get; set; }
    public int? ScoreAway { get; set; }
    
    public Match? Match { get; set; }
}