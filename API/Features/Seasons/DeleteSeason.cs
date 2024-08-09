using Domain.Users;
using FastEndpoints;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Seasons;

public class DeleteSeason
{
    public sealed record DeleteSeasonRequest(Guid Id);

    public sealed record DeleteSeasonResponse(Guid Id);

    public sealed class DeleteSeasonEndpoint(DatabaseContext databaseContext) : Endpoint<DeleteSeasonRequest, DeleteSeasonResponse>
    {
        public override void Configure()
        {
            Delete("seasons/{Id}");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(DeleteSeasonRequest request, CancellationToken cancellationToken)
        {
            var season = await databaseContext.Seasons.Include(_ => _.League).FirstOrDefaultAsync(_ => _.Id == request.Id, cancellationToken);
            if (season is null)
                ThrowError(ErrorKeys.SeasonDoesNotExist);

            var seasonMatches = databaseContext.Matches
                .Include(_ => _.Season)
                .Where(_ => _.Season != null && _.Season.Id == season.Id)
                .AsEnumerable();

            if (seasonMatches.Any(_ => _.IsFinished))
                ThrowError(ErrorKeys.SeasonCanNotBeDeletedIfItHasFinishedMatches);

            databaseContext.Seasons.Remove(season);
            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new DeleteSeasonResponse(season.Id), cancellation: cancellationToken);
        }
    }
}