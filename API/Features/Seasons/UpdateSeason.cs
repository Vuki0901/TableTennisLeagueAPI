using Domain.Users;
using FastEndpoints;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Seasons;

public class UpdateSeason
{
    public sealed record UpdateSeasonRequest(Guid Id, int BestOf, int GameThreshold);

    public sealed record UpdateSeasonResponse(Guid Id);

    public sealed class UpdateSeasonEndpoint(DatabaseContext databaseContext) : Endpoint<UpdateSeasonRequest, UpdateSeasonResponse>
    {
        public override void Configure()
        {
            Put("seasons/{Id}");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(UpdateSeasonRequest request, CancellationToken cancellationToken)
        {
            var season = await databaseContext.Seasons.FirstOrDefaultAsync(_ => _.Id == request.Id, cancellationToken);
            if (season is null)
                ThrowError(ErrorKeys.SeasonDoesNotExist);

            var seasonMatches = databaseContext.Matches
                .Include(_ => _.Season)
                .Where(_ => _.Season != null && _.Season.Id == season.Id)
                .AsEnumerable();

            if (seasonMatches.Any(_ => _.IsFinished))
                ThrowError(ErrorKeys.SeasonCanNotBeUpdatedIfItHasFinishedMatches);

            season.GameThreshold = request.GameThreshold;
            season.BestOf = request.BestOf;

            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new UpdateSeasonResponse(season.Id), cancellation: cancellationToken);
        }
    }
}