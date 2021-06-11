using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Configuration.Auth
{
    public class UserEntityTypeConfiguration: IBaseEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Fields
            builder.HasKey(l => new { l.Id });

            builder.Property(e => e.LastName).IsRequired(false).HasMaxLength(255);
            builder.Property(e => e.FirstName).IsRequired(false).HasMaxLength(255);
            builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(11);
            builder.Property(e => e.ProfileImage).IsRequired(false);
            builder.Property(e => e.Email).IsRequired(false);
            builder.Property(e => e.Bio).IsRequired(false);

            // Navigations
            builder.ToTable(nameof(User), "Security").HasMany(u => u.UserRoles).WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            // For add and change Events
            builder.HasMany(p => p.Users).WithOne(p => p.RegistererUser)
                .HasForeignKey(f => f.RegistererId).IsRequired(false);
            builder.HasMany(p => p.UserRoles).WithOne(p => p.User)
                .HasForeignKey(f => f.UserId).IsRequired(false);
            
            // builder.HasOne(a => a.RegistererUser).WithOne(b=>b.RegistererUser)
            //     .HasForeignKey<User>(a=>a.RegistererId);
            // builder.HasOne(a => a.ModifierUser).WithOne(b=>b.ModifierUser)
            //     .HasForeignKey<User>(a=>a.ModifierId);
        }
    }
}