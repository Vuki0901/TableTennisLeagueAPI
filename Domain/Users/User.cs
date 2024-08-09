using System.Security.Cryptography;
using System.Text;
using Domain.Common;

namespace Domain.Users;

public sealed class User : Entity
{
    private readonly IList<UserRole> _userRoles = new List<UserRole>();

    public string Nickname { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    
    public IEnumerable<UserRole> UserRoles => _userRoles;

    public static User Create(string nickname, string emailAddress, string password)
    {
        return new User()
        {
            Nickname = nickname,
            EmailAddress = emailAddress,
            PasswordHash = HashPassword(password)
        };
    }
    
    public bool Is<T>() where T : UserRole => UserRoles.OfType<T>().Any();
    public T GetRole<T>() where T : UserRole => Is<T>() ? UserRoles.OfType<T>().Single() : throw new InvalidOperationException($"User does not have {typeof(T).Name} role.");

    public T? GetRoleOrNull<T>() where T : UserRole => Is<T>() ? GetRole<T>() : null;

    public void AddUserRole(UserRole role)
    {
        _userRoles.Add(role);
    }
    
    public bool IsCorrectPassword(string password) => HashPassword(password) == PasswordHash;
    private static string HashPassword(string password) => BitConverter.ToString(SHA512.HashData(Encoding.UTF8.GetBytes(password))).Replace("-", string.Empty);
}