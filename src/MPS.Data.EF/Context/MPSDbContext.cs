using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MPS.Data.EF.Configuration;
using MPS.Data.EF.Context.Extenstions;
using MPS.Domain.Core.Interfaces;
using MPS.Domain.Entities.Security;

namespace MPS.Data.EF.Context
{
    public class MPSDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        //private string DbConnection { get; }
        public MPSDbContext(DbContextOptions<MPSDbContext> dbContextOptions) : base(dbContextOptions)
        {
            //DbConnection = configuration.GetConnectionString("DefaultConnection");
            base.ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            base.ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
        }

        public MPSDbContext()
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

    public class MPSContextFactory : IDesignTimeDbContextFactory<MPSDbContext>
    {
        public MPSDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../MPS.WebApp.MVC/appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<MPSDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new MPSDbContext(optionsBuilder.Options);
        }
    }
}