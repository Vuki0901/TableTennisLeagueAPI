using Domain.Seasons;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Seasons;

public class GetSeasonById
{
    public sealed record GetSeasonByIdRequest(Guid Id);

    public sealed record GetSeasonByIdResponse(Season Season);

    public sealed class GetSeasonByIdEndpoint(DatabaseContext databaseContext) : Endpoint<GetSeasonByIdRequest, GetSeasonByIdResponse>
    {
        public override void Configure()
        {
            Get("seasons/{Id}");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(GetSeasonByIdRequest request, CancellationToken cancellationToken)
        {
            var season = await databaseContext.Seasons.Include(_ => _.League).FirstOrDefaultAsync(_ => _.Id == request.Id, cancellationToken);
            if (season is null)
                ThrowError(ErrorKeys.SeasonDoesNotExist);

            await SendAsync(new GetSeasonByIdResponse(season), cancellation: cancellationToken);
        }
    }
}