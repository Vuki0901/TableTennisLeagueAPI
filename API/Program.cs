using System.Text.Json.Serialization;
using Domain.Configurations;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Infrastructure.Auth;
using Infrastructure.Interaction;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

var jwtAuthorizationSection = builder.Configuration.GetSection(nameof(JwtAuthorizationConfiguration));

var jwtAuthorizationConfiguration = jwtAuthorizationSection.Get<JwtAuthorizationConfiguration>()!;
var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthenticationJwtBearer(options =>
{
    options.SigningKey = jwtAuthorizationConfiguration.IssuerSigningKey;
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.SwaggerDocument(options =>
{
    options.ShortSchemaNames = true;
    options.EnableJWTBearerAuth = true;
    options.DocumentSettings = settings =>
    {
        settings.DocumentName = "TableTennisLeagueAPI";
        settings.Title = "TableTennisLeagueAPI";
    };
});

builder.Services.AddOptions<JwtAuthorizationConfiguration>().Bind(jwtAuthorizationSection).ValidateDataAnnotations().ValidateOnStart();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints(config =>
    {
        config.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
        config.Serializer.Options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        config.Endpoints.RoutePrefix = "api";
        config.Errors.ResponseBuilder = (failures, _, _) =>
        {
            return new Result(
                failures.GroupBy(f => f.PropertyName)
                    .ToDictionary(
                        keySelector: e => e.Key,
                        elementSelector: e => e.Select(m => m.ErrorMessage).ToArray()
                    )
            );
        };
    }
);
app.UseAuthenticatedUserSetter();
app.UseDeveloperExceptionPage();
app.UseOpenApi();
app.UseSwaggerUi(s => s.ConfigureDefaults());

app.Run();