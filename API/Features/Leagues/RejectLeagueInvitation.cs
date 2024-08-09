using Domain.Leagues;
using Domain.Users;
using FastEndpoints;
using FluentValidation;
using Infrastructure.Auth;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class RejectLeagueInvitation
{
    public sealed record RejectLeagueInvitationRequest(Guid LeagueInvitationId, Guid NotificationId);

    public sealed record RejectLeagueInvitationResponse(Guid LeagueInvitationId);

    public sealed class RejectLeagueInvitationEndpoint(DatabaseContext databaseContext) : Endpoint<RejectLeagueInvitationRequest, RejectLeagueInvitationResponse>
    {
        public override void Configure()
        {
            Post("league-invitations/reject");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(RejectLeagueInvitationRequest request, CancellationToken cancellationToken)
        {
            var leagueInvitation = await databaseContext.LeagueInvitations.Include(_ => _.League).FirstOrDefaultAsync(_ => _.Id == request.LeagueInvitationId && _.PlayerEmailAddress == HttpContext.GetAuthenticatedUser()!.EmailAddress, cancellationToken);
            if (leagueInvitation is null)
                ThrowError(ErrorKeys.LeagueInvitationDoesNotExist);

            leagueInvitation.Status = LeagueInvitationStatus.Rejected;

            var notification = await databaseContext.Notifications.FirstOrDefaultAsync(_ => _.Id == request.NotificationId, cancellationToken);
            if (notification is not null)
                databaseContext.Notifications.Remove(notification);

            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new RejectLeagueInvitationResponse(leagueInvitation.Id), 201, cancellationToken);
        }
    }

    public sealed class RejectLeagueInvitationRequestValidator : Validator<RejectLeagueInvitationRequest>
    {
        public RejectLeagueInvitationRequestValidator()
        {
            RuleFor(_ => _.LeagueInvitationId).NotEmpty();
            RuleFor(_ => _.NotificationId).NotEmpty();
        }
    }
}