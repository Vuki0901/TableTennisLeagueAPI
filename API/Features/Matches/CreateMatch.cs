using Domain.Matches;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Matches;

public class CreateMatch
{
    public sealed record CreateMatchRequest(Guid LeagueId, Guid SeasonId, Guid HomeLeaguePlayerId, Guid AwayLeaguePlayerId, int RoundNumber);

    public sealed record CreateMatchResponse(Guid Id);

    public sealed class CreateMatchEndpoint(DatabaseContext databaseContext) : Endpoint<CreateMatchRequest, CreateMatchResponse>
    {
        public override void Configure()
        {
            Post("matches");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(CreateMatchRequest request, CancellationToken cancellationToken)
        {
            var league = await databaseContext.Leagues.FirstOrDefaultAsync(_ => _.Id == request.LeagueId, cancellationToken);
            if (league is null)
                ThrowError(ErrorKeys.LeagueDoesNotExist);

            var season = await databaseContext.Seasons.FirstOrDefaultAsync(_ => _.Id == request.SeasonId, cancellationToken);
            if (season is null)
                ThrowError(ErrorKeys.SeasonDoesNotExist);

            var homeLeaguePlayer = await databaseContext.LeaguePlayers.FirstOrDefaultAsync(_ => _.Id == request.HomeLeaguePlayerId, cancellationToken);
            if (homeLeaguePlayer is null)
                ThrowError(ErrorKeys.LeaguePlayerDoesNotExist);

            var awayLeaguePlayer = await databaseContext.LeaguePlayers.FirstOrDefaultAsync(_ => _.Id == request.AwayLeaguePlayerId, cancellationToken);
            if (awayLeaguePlayer is null)
                ThrowError(ErrorKeys.LeaguePlayerDoesNotExist);

            var matches = databaseContext.Matches
                .Include(_ => _.Home)
                .Include(_ => _.Away)
                .ToList();

            if (matches
                .Where(_ => _ is { Home: not null, Away: not null } && (_.Home.Id == request.HomeLeaguePlayerId || _.Home.Id == request.AwayLeaguePlayerId || _.Away.Id == request.HomeLeaguePlayerId || _.Away.Id == request.AwayLeaguePlayerId))
                .Any(_ => _.RoundNumber == request.RoundNumber)
               )
                ThrowError(ErrorKeys.PlayerHasAlreadyPlayedInThatRound);
            
            if(request.RoundNumber - matches.Max(_ => _.RoundNumber) > 1)
                ThrowError(ErrorKeys.MatchRoundNumbersMustBeSequential);

            var match = Match.Create(request.RoundNumber, season, homeLeaguePlayer, awayLeaguePlayer);

            databaseContext.Matches.Add(match);
            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new CreateMatchResponse(match.Id), 201, cancellationToken);
        }
    }
}