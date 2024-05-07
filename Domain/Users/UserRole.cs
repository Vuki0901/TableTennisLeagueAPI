using Domain.Common;

namespace Domain.Users;

public abstract class UserRole : Entity
{
    public User? User { get; set; }
}