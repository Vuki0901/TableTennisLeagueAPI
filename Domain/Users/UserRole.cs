using Domain.Common;

namespace Domain.Users;

public abstract class UserRole : Entity
{
    public string RoleType { get; init; } = null!;
}