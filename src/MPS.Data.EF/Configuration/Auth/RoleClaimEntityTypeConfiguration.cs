using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Configuration.Auth
{
    public class RoleClaimEntityTypeConfiguration : IBaseEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {

            builder.HasKey(rc => rc.Id);

            //Table
            builder.ToTable(nameof(RoleClaim), "Security");
        }
    }
}