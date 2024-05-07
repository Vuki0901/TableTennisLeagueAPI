namespace Domain.Configurations;

public class DatabaseConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DefaultSchema { get; set; } = string.Empty;
    public int CommandTimeout { get; set; }
}