using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Configuration.Auth
{
    public class UserClaimEntityTypeConfiguration : IBaseEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            //Fields
            builder.HasKey(l => new { l.Id });

            //Table
            builder.ToTable(nameof(UserClaim), "Security");
        }
    }
}