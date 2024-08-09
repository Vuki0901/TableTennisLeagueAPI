using Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth;

public static class HttpContextExtensions
{
    public static User? GetAuthenticatedUser(this HttpContext httpContext)
    {
        return (User?)httpContext.Items[nameof(User)];
    }
    
    public static Player? GetAuthenticatedPlayer(this HttpContext httpContext)
    {
        var user = httpContext.GetAuthenticatedUser();
        var player = user?.GetRoleOrNull<Player>();
        return player;
    }
}