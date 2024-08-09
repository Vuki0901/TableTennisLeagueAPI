using Domain.Leagues;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class UpdateLeague
{
    public sealed record UpdateLeagueRequest(Guid Id, string Name, MatchFormat MatchFormat);

    public sealed record UpdateLeagueResponse(Guid Id);

    public sealed class UpdateLeagueEndpoint(DatabaseContext databaseContext) : Endpoint<UpdateLeagueRequest, UpdateLeagueResponse>
    {
        public override void Configure()
        {
            Put("leagues");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(UpdateLeagueRequest request, CancellationToken cancellationToken)
        {
            var league = await databaseContext.Leagues.Include(_ => _.Seasons).ThenInclude(s => s.Matches).FirstOrDefaultAsync(_ => _.Id == request.Id, cancellationToken: cancellationToken);
            if(league is null)
                ThrowError(ErrorKeys.LeagueDoesNotExist);
            
            if(league.Name == request.Name)
                ThrowError(ErrorKeys.NewLeagueNameMustBeDifferentThatTheOldName);
            
            if (await databaseContext.Leagues.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken))
                ThrowError(ErrorKeys.LeagueNameMustBeUnique);

            if(league.MatchFormat != request.MatchFormat && league.Seasons.Any(_ => _.Matches.Any(m => m.IsFinished)))
                ThrowError(ErrorKeys.LeagueMatchFormatCannotBeChangedAfterAFinishedMatch);
            
            league.Name = request.Name;
            league.MatchFormat = request.MatchFormat;

            databaseContext.Update(league);
            await databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}