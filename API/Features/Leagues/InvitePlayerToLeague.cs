using Domain.Leagues;
using Domain.Notifications;
using Domain.Users;
using FastEndpoints;
using FluentValidation;
using Infrastructure.Auth;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class InvitePlayerToLeague
{
    public sealed record InvitePlayerToLeagueRequest(string PlayerEmailAddress, Guid LeagueId);

    public sealed record InvitePlayerToLeagueResponse(Guid LeagueInvitationId);

    public sealed class InvitePlayerToLeagueEndpoint(DatabaseContext databaseContext) : Endpoint<InvitePlayerToLeagueRequest, InvitePlayerToLeagueResponse>
    {
        public override void Configure()
        {
            Post("leagues/invite");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(InvitePlayerToLeagueRequest request, CancellationToken cancellationToken)
        {
            var league = await databaseContext.Leagues.FirstOrDefaultAsync(_ => _.Id == request.LeagueId, cancellationToken);
            if (league is null)
                ThrowError(ErrorKeys.LeagueDoesNotExist);

            var existingLeagueInvitation = await databaseContext.LeagueInvitations.FirstOrDefaultAsync(_ => _.PlayerEmailAddress == request.PlayerEmailAddress && _.League!.Id == league.Id && _.Status == LeagueInvitationStatus.Pending, cancellationToken);
            if (existingLeagueInvitation is not null)
                ThrowError(ErrorKeys.PlayerIsAlreadyInvitedToThisLeague);

            var leagueInvitation = LeagueInvitation.Create(request.PlayerEmailAddress, league);
            league.AddInvitation(leagueInvitation);

            var leagueInvitationNotification = LeagueInvitationNotification.Create(request.PlayerEmailAddress, leagueInvitation, HttpContext.GetAuthenticatedPlayer()!);
            databaseContext.Notifications.Add(leagueInvitationNotification);

            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new InvitePlayerToLeagueResponse(leagueInvitation.Id), 201, cancellationToken);
        }
    }

    public sealed class InvitePlayerToLeagueRequestValidator : Validator<InvitePlayerToLeagueRequest>
    {
        public InvitePlayerToLeagueRequestValidator()
        {
            RuleFor(_ => _.PlayerEmailAddress).NotEmpty().EmailAddress();
            RuleFor(_ => _.LeagueId).NotEmpty();
        }
    }
}