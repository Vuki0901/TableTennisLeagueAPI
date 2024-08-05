namespace Infrastructure.Errors;

public class ErrorKeys
{
    public const string UserEmailAddressMustBeUnique = nameof(UserEmailAddressMustBeUnique);
    public const string UserNicknameMustBeUnique = nameof(UserNicknameMustBeUnique);
    public const string UserDoesNotExist = nameof(UserDoesNotExist);
    public const string UserPasswordIsNotValid = nameof(UserPasswordIsNotValid);
}