using Domain.Common;

namespace Domain.Users;

public sealed class User : Entity
{
    private readonly IList<UserRole> _userRoles = new List<UserRole>();

    public string Nickname { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    
    public IEnumerable<UserRole> UserRoles => _userRoles;

    public void AddUserRole(UserRole role)
    {
        _userRoles.Add(role);
    }
}