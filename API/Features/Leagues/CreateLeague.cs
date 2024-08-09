using Domain.Leagues;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Auth;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class CreateLeague
{
    public sealed record CreateLeagueRequest(string Name, MatchFormat MatchFormat);

    public sealed record CreateLeagueResponse(Guid Id);

    public sealed class CreateLeagueEndpoint(DatabaseContext databaseContext) : Endpoint<CreateLeagueRequest, CreateLeagueResponse>
    {
        public override void Configure()
        {
            Post("leagues");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(CreateLeagueRequest request, CancellationToken cancellationToken)
        {
            if (await databaseContext.Leagues.AnyAsync(x => x.Name == request.Name, cancellationToken: cancellationToken))
                ThrowError(ErrorKeys.LeagueNameMustBeUnique);

            var player = HttpContext.GetAuthenticatedPlayer();
            if (player is null)
                ThrowError(ErrorKeys.PlayerMustBeAuthenticatedToCreateLeagues);

            var league = League.Create(request.Name, request.MatchFormat);

            var leaguePlayer = LeaguePlayer.Create(LeaguePlayerLevel.Owner);
            league.AddPlayer(leaguePlayer);
            player.AddLeaguePlayer(leaguePlayer);

            await databaseContext.Leagues.AddAsync(league, cancellationToken);
            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new CreateLeagueResponse(league.Id), cancellation: cancellationToken);
        }
    }
}