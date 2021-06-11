using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Configuration.Auth
{
    public class UserLoginEntityTypeConfiguration : IBaseEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            //Fields
            builder.HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });

            //Table
            builder.ToTable(nameof(UserLogin), "Security");
        }
    }
}