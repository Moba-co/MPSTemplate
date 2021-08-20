using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MPS.Services.Interfaces
{
    public interface IIdentityDbInitializer
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// and virtual DB
        /// </summary>
        void Initialize();

        /// <summary>
        /// seed data
        /// </summary>
        Task SeedData();

        Task<IdentityResult> SeedDatabaseWithAdminUserAsync();
    }
}