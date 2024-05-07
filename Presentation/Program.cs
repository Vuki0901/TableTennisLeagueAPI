using Domain.Configurations;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

var jwtAuthorizationConfiguration = builder.Configuration.GetSection(nameof(JwtAuthorizationConfiguration)).Get<JwtAuthorizationConfiguration>()!;

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
        config.Endpoints.RoutePrefix = "api";
    }
);
app.UseDeveloperExceptionPage();
app.UseOpenApi();
app.UseSwaggerUi(s => s.ConfigureDefaults());

app.Run();