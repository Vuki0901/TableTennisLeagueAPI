using Domain.Seasons;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Seasons;

public class CreateSeason
{
    public sealed record CreateSeasonRequest(Guid LeagueId, int BestOf, int GameThreshold);

    public sealed record CreateSeasonResponse(Guid Id);

    public sealed class CreateSeasonEndpoint(DatabaseContext databaseContext) : Endpoint<CreateSeasonRequest, CreateSeasonResponse>
    {
        public override void Configure()
        {
            Post("seasons");
            Roles(nameof(Player));
        }
    
        public override async Task HandleAsync(CreateSeasonRequest request, CancellationToken cancellationToken)
        {
            var league = await databaseContext.Leagues.FirstOrDefaultAsync(_ => _.Id == request.LeagueId, cancellationToken);
            if (league is null)
                ThrowError(ErrorKeys.LeagueDoesNotExist);

            var season = Season.Create(request.BestOf, request.GameThreshold, league);
            databaseContext.Seasons.Add(season);

            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new CreateSeasonResponse(season.Id), 201, cancellationToken);
        }
    }
}