using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Moba.Data.EF.Configuration;
using Moba.Data.EF.Context.Extenstions;
using Moba.Domain.Core.Interfaces;
using Moba.Domain.Entities.Security;

namespace Moba.Data.EF.Context
{
    public class MobaDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        //private string DbConnection { get; }
        public MobaDbContext(DbContextOptions<MobaDbContext> dbContextOptions) : base(dbContextOptions)
        {
            //DbConnection = configuration.GetConnectionString("DefaultConnection");
            base.ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            base.ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
        }

        public MobaDbContext()
        {

        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (optionsBuilder.IsConfigured) return;
        //     optionsBuilder.UseSqlServer(DbConnection);
        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddDbSetFromModel(typeof(IEntity).Assembly, typeof(IEntity));
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IBaseEntityTypeConfiguration<>).Assembly);
        }
    }

    public class BloggingContextFactory : IDesignTimeDbContextFactory<MobaDbContext>
    {
        public MobaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MobaDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=Moba_db;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new MobaDbContext(optionsBuilder.Options);
        }
    }
}