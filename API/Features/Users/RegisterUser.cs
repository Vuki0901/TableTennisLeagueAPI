using System.Security.Claims;
using Domain.Configurations;
using Domain.Users;
using FastEndpoints;
using FastEndpoints.Security;
using FluentValidation;
using Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Presentation.Features.Users;

public sealed class RegisterUser
{
    public sealed record RegisterUserRequest(string Nickname, string EmailAddress, string Password);

    public sealed record RegisterUserResponse(string Token);

    public sealed class RegisterUserEndpoint(DatabaseContext databaseContext, JwtAuthorizationConfiguration jwtAuthorizationConfiguration) : Endpoint<RegisterUserRequest, RegisterUserResponse>
    {
        public override void Configure()
        {
            Post("register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            if (await databaseContext.Users.AnyAsync(x => x.EmailAddress == request.EmailAddress, cancellationToken: cancellationToken))
                ThrowError(ErrorKeys.UserEmailAddressMustBeUnique);
            
            if (await databaseContext.Users.AnyAsync(x => x.Nickname == request.Nickname, cancellationToken: cancellationToken))
                ThrowError(ErrorKeys.UserNicknameMustBeUnique);
            
            var user = Domain.Users.User.Create(
                request.Nickname,
                request.EmailAddress,
                request.Password
            );

            user.AddUserRole(new Player());

            databaseContext.Add(user);
            await databaseContext.SaveChangesAsync(cancellationToken);
            
            var token = JwtBearer.CreateToken(
                o =>
                {
                    o.SigningKey = jwtAuthorizationConfiguration.IssuerSigningKey;
                    o.ExpireAt = DateTime.Now.AddHours(24);
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

            await SendAsync(new RegisterUserResponse(new string(token)), cancellation: cancellationToken);
        }
    }

    public sealed class RegisterValidator : Validator<RegisterUserRequest>
    {
        public RegisterValidator()
        {
            RuleFor(_ => _.Nickname).NotEmpty().MaximumLength(50);
            RuleFor(_ => _.EmailAddress).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(_ => _.Password).NotEmpty().MinimumLength(8);
        }
    }
}