using Domain.Leagues;
using Domain.Seasons;
using Domain.Users;
using FastEndpoints;
using Infrastructure.Auth;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class GetLeagueById
{
    public sealed record GetLeagueByIdRequest(Guid LeagueId);

    public sealed record GetLeagueByIdResponse(LeagueProjection League);

    public sealed class GetLeagueByIdEndpoint(DatabaseContext databaseContext) : Endpoint<GetLeagueByIdRequest, GetLeagueByIdResponse>
    {
        public override void Configure()
        {
            Get("leagues/{LeagueId}");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(GetLeagueByIdRequest request, CancellationToken cancellationToken)
        {
            var league = await databaseContext.Leagues.AsNoTracking()
                .Include(_ => _.LeaguePlayers)
                .Include(_ => _.LeagueInvitations)
                .Include(_ => _.Seasons)
                .FirstOrDefaultAsync(_ => _.Id == request.LeagueId, cancellationToken);
            if (league is null)
                ThrowError(ErrorKeys.LeagueDoesNotExist);

            var playerLeaguePlayers = databaseContext.Users
                .Include(_ => _.UserRoles)
                .ThenInclude(ur => ((Player)ur).LeaguePlayers)
                .Where(_ => _.UserRoles.Any(ur => ur.RoleType == nameof(Player)))
                .AsEnumerable();

            var leagueProjection = new LeagueProjection()
            {
                Id = league.Id,
                Name = league.Name,
                UserLeaguePlayers = league.LeaguePlayers.Select(_ => new UserLeaguePlayer(_.Id, playerLeaguePlayers.FirstOrDefault(plp => plp.GetRole<Player>().LeaguePlayers.Any(lp => lp.Id == _.Id))?.Nickname, _.LeaguePlayerLevel, _.CreatedOn)).ToList(),
                LeagueInvitations = league.LeagueInvitations.Select(_ => new LeagueInvitationProjection(_.Id, _.PlayerEmailAddress, _.Status, _.CreatedOn)).ToList(),
                Seasons = league.Seasons.ToList()
            };

            await SendAsync(new GetLeagueByIdResponse(leagueProjection), 200, cancellationToken);
        }
    }

    public class LeagueProjection()
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<UserLeaguePlayer> UserLeaguePlayers { get; set; } = [];
        public List<LeagueInvitationProjection> LeagueInvitations { get; set; } = [];
        public List<Season> Seasons { get; set; } = [];
    }

    public sealed record UserLeaguePlayer(Guid Id, string? Nickname, LeaguePlayerLevel LeaguePlayerLevel, DateTimeOffset CreatedOn);

    public sealed record LeagueInvitationProjection(Guid Id, string PlayerEmailAddress, LeagueInvitationStatus Status, DateTimeOffset CreatedOn);
}