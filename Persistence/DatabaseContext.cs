using Domain.Leagues;
using Domain.Matches;
using Domain.Notifications;
using Domain.Seasons;
using Domain.Users;
using Persistence.Configurations.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<UserRole> UserRoles { get; init; } = null!;
    public DbSet<Season> Seasons { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Set> Sets { get; set; } = null!;
    public DbSet<LeaguePlayer> LeaguePlayers { get; set; } = null!;
    public DbSet<Match> Matches { get; set; } = null!;
    public DbSet<LeagueInvitation> LeagueInvitations { get; set; } = null!;
    public DbSet<League> Leagues { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName + "/Presentation/")
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("Database");
        optionsBuilder.UseSqlServer(connectionString);

        return new DatabaseContext(optionsBuilder.Options);
    }
}