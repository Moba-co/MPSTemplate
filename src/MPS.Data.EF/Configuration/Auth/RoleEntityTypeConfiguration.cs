using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Configuration.Auth
{
    public class RoleEntityTypeConfiguration : IBaseEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            //Fields
            builder.HasKey(l => new { l.Id });
            builder.ToTable(nameof(Role), "Security").Property(p => p.Id);
            builder.ToTable(nameof(Role), "Security").Property(p => p.Name);

            builder.ToTable("Roles").HasKey(r => r.Id);
            builder.HasIndex(r => r.NormalizedName).IsUnique();
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            builder.Property(u => u.Name).HasMaxLength(256);
            builder.Property(u => u.NormalizedName).HasMaxLength(256);

            //navigation

            builder.ToTable("Roles", "Auth").HasMany(u => u.UsersInThisRole).WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            builder.HasMany<RoleClaim>().WithOne()
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
                                          
            //Table
            builder.ToTable(nameof(Role), "Security");
        }
    }
}