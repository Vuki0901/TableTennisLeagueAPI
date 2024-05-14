using Domain.Common;

namespace Domain.Users;

public abstract class UserRole : Entity
{
    public string RoleType { get; set; } = string.Empty;
    public User? User { get; set; }
}