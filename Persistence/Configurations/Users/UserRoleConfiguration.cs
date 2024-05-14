using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Users;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    { 
        builder.HasKey(_ => _.Id);
        builder.HasOne(_ => _.User);
        builder.HasDiscriminator(_ => _.RoleType);
    }
}