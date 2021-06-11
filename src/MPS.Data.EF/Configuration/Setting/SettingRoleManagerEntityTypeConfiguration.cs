using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moba.Domain.Entities.Setting;

namespace Moba.Data.EF.Configuration.Setting
{
    public class SettingRoleManagerEntityTypeConfiguration : IBaseEntityTypeConfiguration<SettingRoleManager>
    {
        public void Configure(EntityTypeBuilder<SettingRoleManager> builder)
        {
            // Fields
            builder.HasKey(p => p.Id);
            // Navigations

            // For add and change Events
            builder.HasOne(a => a.RegistererUser).WithMany(b => b.RegisterdSettingRoleManeger)
                           .HasForeignKey(a => a.RegistererId);
            //Table
            builder.ToTable(nameof(SettingRoleManager), "Setting");
        }
    }
}
