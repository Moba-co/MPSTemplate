using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moba.Domain.Entities.Security;

namespace Moba.Data.EF.Configuration.Auth
{
    public class UserTokenEntityTypeConfiguration : IBaseEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            //Table
            builder.ToTable("UserTokens", "Security");
        }
    }
}