using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Configuration.Auth
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