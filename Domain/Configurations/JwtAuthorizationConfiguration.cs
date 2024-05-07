namespace Domain.Configurations;

public class JwtAuthorizationConfiguration
{
    public string IssuerSigningKey { get; set; } = string.Empty;
}