namespace Infrastructure.Errors;

public class ErrorKeys
{
    public const string UserEmailAddressMustBeUnique = nameof(UserEmailAddressMustBeUnique);
    public const string UserNicknameMustBeUnique = nameof(UserNicknameMustBeUnique);
    public const string UserDoesNotExist = nameof(UserDoesNotExist);
    public const string UserPasswordIsNotValid = nameof(UserPasswordIsNotValid);
    
    // Leagues
    public const string LeagueNameMustBeUnique = nameof(LeagueNameMustBeUnique);
    public const string PlayerMustBeAuthenticatedToCreateLeagues = nameof(PlayerMustBeAuthenticatedToCreateLeagues);
    public const string LeagueDoesNotExist = nameof(LeagueDoesNotExist);
    public const string NewLeagueNameMustBeDifferentThatTheOldName = nameof(NewLeagueNameMustBeDifferentThatTheOldName);
    public const string LeagueMatchFormatCannotBeChangedAfterAFinishedMatch = nameof(LeagueMatchFormatCannotBeChangedAfterAFinishedMatch);
    
    // Players
    public const string PlayerDoesNotExist = nameof(PlayerDoesNotExist);
    
    // Invitations
    public const string PlayerIsAlreadyInvitedToThisLeague = nameof(PlayerIsAlreadyInvitedToThisLeague);
    public const string LeagueInvitationDoesNotExist = nameof(LeagueInvitationDoesNotExist);
}