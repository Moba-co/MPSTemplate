using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Configuration.Auth
{
    public class UserRoleEntityTypeConfiguration : IBaseEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            //Fields
            builder.HasKey(r => new { r.UserId, r.RoleId });

            //Table
            builder.ToTable(nameof(UserRole), "Security");
        }
    }
}