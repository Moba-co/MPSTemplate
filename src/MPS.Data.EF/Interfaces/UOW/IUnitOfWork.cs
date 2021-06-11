using System;
using System.Threading.Tasks;
using Moba.Data.EF.Interfaces.Repositories;
using Moba.Domain.Entities.Security;
using Moba.Domain.Entities.Setting;

namespace Moba.Data.EF.Interfaces.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        #region  AuthEntity
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<UserRole> UserRoleRepository { get; }
        IGenericRepository<UserClaim> UserClaimRepository { get; }
        IGenericRepository<RoleClaim> RoleClaimRepository { get; }
        IGenericRepository<UserLogin> UserLoginRepository { get; }
        IGenericRepository<UserToken> UserTokenRepository { get; }
        #endregion

        #region  SettingEntity
        IGenericRepository<SettingRoleManager> SettingRoleManagerRepository { get; }
        #endregion
        
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
