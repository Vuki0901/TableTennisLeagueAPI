using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Nickname).IsRequired().HasMaxLength(50);
        builder.Property(_ => _.EmailAddress).IsRequired();
        builder.HasMany(_ => _.UserRoles);
    }
}