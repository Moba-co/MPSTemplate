using System;
using System.Threading.Tasks;
using MPS.Data.EF.Interfaces.Repositories;
using MPS.Domain.Entities.Security;
using MPS.Domain.Entities.Setting;

namespace MPS.Data.EF.Interfaces.UOW
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
