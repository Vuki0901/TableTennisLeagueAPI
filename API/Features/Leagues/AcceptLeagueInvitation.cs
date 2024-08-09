using Domain.Leagues;
using Domain.Users;
using FastEndpoints;
using FluentValidation;
using Infrastructure.Auth;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Leagues;

public class AcceptLeagueInvitation
{
    public sealed record AcceptLeagueInvitationRequest(Guid LeagueInvitationId, Guid NotificationId);
    
    public sealed record AcceptLeagueInvitationResponse(Guid LeaguePlayerId);

    public sealed class AcceptLeagueInvitationEndpoint(DatabaseContext databaseContext) : Endpoint<AcceptLeagueInvitationRequest, AcceptLeagueInvitationResponse>
    {
        public override void Configure()
        {
            Post("league-invitations/accept");
            Roles(nameof(Player));
        }

        public override async Task HandleAsync(AcceptLeagueInvitationRequest request, CancellationToken cancellationToken)
        {
            var leagueInvitation = await databaseContext.LeagueInvitations.Include(_ => _.League).FirstOrDefaultAsync(_ => _.Id == request.LeagueInvitationId && _.PlayerEmailAddress == HttpContext.GetAuthenticatedUser()!.EmailAddress, cancellationToken);
            if(leagueInvitation is null)
                ThrowError(ErrorKeys.LeagueInvitationDoesNotExist);

            var leaguePlayer = LeaguePlayer.Create(LeaguePlayerLevel.Member);
            leagueInvitation.League!.AddPlayer(leaguePlayer);
            HttpContext.GetAuthenticatedPlayer()!.AddLeaguePlayer(leaguePlayer);
            databaseContext.LeaguePlayers.Add(leaguePlayer);

            leagueInvitation.Status = LeagueInvitationStatus.Accepted;

            var notification = await databaseContext.Notifications.FirstOrDefaultAsync(_ => _.Id != request.NotificationId, cancellationToken);
            if(notification is not null)
                databaseContext.Notifications.Remove(notification);
            
            await databaseContext.SaveChangesAsync(cancellationToken);

            await SendAsync(new AcceptLeagueInvitationResponse(leaguePlayer.Id), 201, cancellationToken);
        }
    }
    
    public sealed class AcceptLeagueInvitationRequestValidator : Validator<AcceptLeagueInvitationRequest>
    {
        public AcceptLeagueInvitationRequestValidator()
        {
            RuleFor(_ => _.LeagueInvitationId).NotEmpty();
            RuleFor(_ => _.NotificationId).NotEmpty();
        }
    }
}