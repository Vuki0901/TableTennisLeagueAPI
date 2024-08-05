using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Users;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    { 
        builder.HasKey(_ => _.Id);
        builder.HasDiscriminator(_ => _.RoleType)
            .HasValue<Player>(nameof(Player))
            .HasValue<Administrator>(nameof(Administrator));
        
        builder.HasIndex("RoleType", "UserId").IsUnique();
    }
}