using System.Security.Claims;
using Domain.Configurations;
using Domain.Users;
using FastEndpoints;
using FastEndpoints.Security;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Users;

public class AuthenticateUser
{
    public sealed record AuthenticateUserRequest(string EmailAddress, string Password);

    public sealed record AuthenticateUserResponse(string Token);

    public sealed class AuthenticateUserEndpoint(DatabaseContext databaseContext, JwtAuthorizationConfiguration jwtAuthorizationConfiguration) : Endpoint<AuthenticateUserRequest, AuthenticateUserResponse>
    {
        public override void Configure()
        {
            Post("authenticate");
            AllowAnonymous();
        }

        public override async Task HandleAsync(AuthenticateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await databaseContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.EmailAddress == request.EmailAddress, cancellationToken);
            if (user is null)
                ThrowError(ErrorKeys.UserDoesNotExist);

            if (!user.IsCorrectPassword(request.Password))
                ThrowError(ErrorKeys.UserPasswordIsNotValid);
            
            if (!user.Is<Player>())
            {
                user.AddUserRole(new Player());
                await databaseContext.SaveChangesAsync(cancellationToken);
            }

            var token = JwtBearer.CreateToken(
                o =>
                {
                    o.SigningKey = jwtAuthorizationConfiguration.IssuerSigningKey;
                    o.ExpireAt = DateTime.Now.AddHours(240);
                    o.User.Claims.Add(
                        new Claim[]
                        {
                            new("id", user.Id.ToString()),
                            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new(ClaimTypes.Name, $"{user.Nickname}")
                        }
                    );
                    o.User.Roles.Add(user.UserRoles.Select(ur => ur.RoleType).ToArray());
                }
            );
            
            await SendAsync(new AuthenticateUserResponse(new string(token)), cancellation: cancellationToken);
        }
    }
}