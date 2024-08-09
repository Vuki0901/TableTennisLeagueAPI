using Domain.Leagues;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Auth;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class GetLeagues
{
    public sealed record GetLeaguesResponse(IEnumerable<LeagueProjection> Leagues);

    public sealed class GetLeaguesEndpoint(DatabaseContext databaseContext) : EndpointWithoutRequest<GetLeaguesResponse>
    {
        public override void Configure()
        {
            Get("leagues");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(CancellationToken cancellationToken)
        {
            var player = HttpContext.GetAuthenticatedPlayer();
            if (player is null)
                ThrowError(ErrorKeys.PlayerDoesNotExist);

            var leaguePlayerIds = player.LeaguePlayers.Select(plp => plp.Id).ToList();

            var leagues = databaseContext.Leagues
                .Include(_ => _.LeaguePlayers)
                .Where(_ => _.LeaguePlayers.Any(lp => leaguePlayerIds.Contains(lp.Id)))
                .AsEnumerable() // Switch to client-side evaluation from here
                .Select(l => new LeagueProjection(l.Id, l.Name, l.MatchFormat, l.LeaguePlayers.Count()))
                .ToList();

            await SendAsync(new GetLeaguesResponse(leagues), 200, cancellationToken);
        }
    }

    public sealed record LeagueProjection(Guid Id, string Name, MatchFormat MatchFormat, int NumberOfPlayers);
}