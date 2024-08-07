using Domain.Leagues;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class GetLeagueById
{
    public sealed record GetLeagueByIdRequest(Guid LeagueId);

    public sealed record GetLeagueByIdResponse(League League);
    
    public sealed class GetLeagueByIdEndpoint(DatabaseContext databaseContext) : Endpoint<GetLeagueByIdRequest, GetLeagueByIdResponse>
    {
        public override void Configure()
        {
            Get("leagues/{LeagueId}");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(GetLeagueByIdRequest request, CancellationToken cancellationToken)
        {
            var league = await databaseContext.Leagues
                .Include(_ => _.LeaguePlayers)
                .Include(_ => _.LeagueInvitations)
                .Include(_ => _.Seasons)
                .FirstOrDefaultAsync(_ => _.Id == request.LeagueId, cancellationToken);
            if(league is null)
                ThrowError(ErrorKeys.LeagueDoesNotExist);

            await SendAsync(new GetLeagueByIdResponse(league), 200, cancellationToken);
        }
    }

    private record LeaguePlayerProjection(Guid Id, string PlayerNickname);
}